using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable] 
    public class RandomColorGradientValueProvider : IColorValueProvider
    {
        [SerializeField] private Gradient _gradient;

        public Color Get() => _gradient.Evaluate(Random.Range(0f, 1f));
    }
}