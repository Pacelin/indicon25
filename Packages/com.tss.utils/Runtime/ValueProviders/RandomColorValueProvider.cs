using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable] 
    public class RandomColorValueProvider : IColorValueProvider
    {
        [SerializeReference] private float _alpha;

        public RandomColorValueProvider(float alpha) => _alpha = alpha;
        public RandomColorValueProvider() => _alpha = 1;
        
        public Color Get() => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), _alpha);
    }
}