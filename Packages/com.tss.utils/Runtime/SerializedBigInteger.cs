using System;
using System.Numerics;
using UnityEngine;

namespace TSS.Utils
{
    [Serializable]
    public struct SerializedBigInteger : ISerializationCallbackReceiver
    {
        [SerializeField] private int[] _representation;

        private BigInteger _bigInteger;

        public SerializedBigInteger(BigInteger bigInteger, int dimensions)
        {
            _bigInteger = bigInteger;
            _representation = _bigInteger.BrokeIntoParts(dimensions);
        }

        public void OnBeforeSerialize() => _representation = _bigInteger.BrokeIntoParts(_representation.Length);
        public void OnAfterDeserialize() => _bigInteger = BigInteger.Parse(_representation.ConnectIntoBigInt());

        public static implicit operator BigInteger(SerializedBigInteger serialized) => serialized._bigInteger;
    }
}