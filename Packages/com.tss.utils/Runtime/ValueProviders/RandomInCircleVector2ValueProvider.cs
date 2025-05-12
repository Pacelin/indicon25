using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class RandomInCircleVector2ValueProvider : IVector2ValueProvider
    {
        [SerializeField] private Vector2 _circlePosition;
        [SerializeField] private float _circleRadius;

        public RandomInCircleVector2ValueProvider(Vector2 circlePosition, float circleRadius)
        {
            _circlePosition = circlePosition;
            _circleRadius = circleRadius;
        }

        public Vector2 Get() => _circlePosition + Random.insideUnitCircle * _circleRadius;
    }
}