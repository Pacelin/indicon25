using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class RandomInBoundsVector3ValueProvider : IVector3ValueProvider
    {
        [SerializeField] private Bounds _bounds;

        public RandomInBoundsVector3ValueProvider(Bounds bounds) => _bounds = bounds;

        public Vector3 Get() => _bounds.min + new Vector3(
            Random.Range(0, _bounds.size.x), Random.Range(0, _bounds.size.y), Random.Range(0, _bounds.size.z));
    }
}