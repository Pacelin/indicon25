using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class Vector2FollowValueProvider : IVector2ValueProvider
    {
        [SerializeField] private Transform _target;

        public Vector2 Get() => _target.position;
    }
}