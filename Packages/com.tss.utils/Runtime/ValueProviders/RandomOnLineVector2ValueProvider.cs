using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class RandomOnLineVector2ValueProvider : IVector2ValueProvider
    {
        [SerializeField] private Vector2 _from;
        [SerializeField] private Vector2 _to;

        public RandomOnLineVector2ValueProvider(Vector2 from, Vector2 to)
        {
            _from = from;
            _to = to;
        }
        
        public Vector2 Get() => Vector2.Lerp(_from, _to, Random.Range(0f, 1f));
    }
}