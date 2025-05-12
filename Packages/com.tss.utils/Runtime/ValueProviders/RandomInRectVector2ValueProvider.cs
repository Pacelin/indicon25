using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class RandomInRectVector2ValueProvider : IVector2ValueProvider
    {
        [SerializeField] private Rect _rect;

        public RandomInRectVector2ValueProvider(Rect rect) => _rect = rect;

        public Vector2 Get() => new(_rect.xMin + Random.Range(0, _rect.width),
            _rect.yMin + Random.Range(0, _rect.height));
    }
}