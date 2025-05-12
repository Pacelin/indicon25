using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class RandomInSphereVector3ValueProvider : IVector3ValueProvider
    {
        [SerializeReference] private Vector3 _circlePosition;
        [SerializeReference] private float _circleRadius;

        public RandomInSphereVector3ValueProvider(Vector3 circlePosition, float circleRadius)
        {
            _circlePosition = circlePosition;
            _circleRadius = circleRadius;
        }

        public Vector3 Get() => _circlePosition + Random.insideUnitSphere * _circleRadius;
    }
}