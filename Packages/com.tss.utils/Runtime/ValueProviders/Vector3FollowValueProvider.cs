using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class Vector3FollowValueProvider : IVector3ValueProvider
    {
        [SerializeField] private Transform _target;

        public Vector3 Get() => _target.position;
    }
}