using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable] 
    public class RandomColorSelectedValueProvider : IColorValueProvider
    {
        [SerializeField] private Color[] _availableColors;

        public RandomColorSelectedValueProvider(Color[] availableColors) => _availableColors = availableColors;
        public RandomColorSelectedValueProvider() => _availableColors = System.Array.Empty<Color>();
        
        public Color Get() => _availableColors[Random.Range(0, _availableColors.Length)];
    }
}