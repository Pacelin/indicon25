using UnityEngine;

namespace TSS.Utils
{
    [System.Serializable]
    public class ColorValueProvider : IColorValueProvider
    {
        [SerializeField] private Color _color;

        public ColorValueProvider(Color color) => _color = color;
        public ColorValueProvider() => _color = Color.white;
        
        public Color Get() => _color;
    }
}