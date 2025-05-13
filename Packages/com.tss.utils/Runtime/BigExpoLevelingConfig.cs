using System.Numerics;
using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public struct BigExpoLevelStep
    {
        public int Level;
        [Range(1f, 10f)]
        public double Increment;
    }
    
    [CreateAssetMenu(menuName = "TSS/Big Expo Leveling", fileName = "SO_Leveling")]
    public class BigExpoLevelingConfig : ScriptableObject
    {
        public int MaxLevel => _maxLevel;

        [SerializeField] internal BigExpoLevelStep[] _steps;
        [BigDimensions(30, 5)]
        [SerializeField] private SerializedBigInteger _baseValue = new SerializedBigInteger(100, 30);
        [SerializeField] private int _maxLevel;

        public BigInteger GetByLevel(int level) => _baseValue.ApplyExpo(_steps, level);
    }
}