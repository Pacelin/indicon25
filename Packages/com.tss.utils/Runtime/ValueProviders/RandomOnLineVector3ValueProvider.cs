using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class RandomOnLineVector3ValueProvider : IVector3ValueProvider
    {
        [SerializeField] private Vector3 _from;
        [SerializeField] private Vector3 _to;

        public RandomOnLineVector3ValueProvider(Vector3 from, Vector3 to)
        {
            _from = from;
            _to = to;
        }
        
        public Vector3 Get() => Vector3.Lerp(_from, _to, Random.Range(0f, 1f));
    }
}