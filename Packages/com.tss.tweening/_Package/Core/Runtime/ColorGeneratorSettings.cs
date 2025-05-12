using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TSS.Core
{
    [Serializable]
    public struct ColorGeneratorSettings
    {
        [MinMaxSlider(0f, 1f)]
        [SerializeField] private Vector2 _minMaxHue;
        [MinMaxSlider(0f, 1f)]
        [SerializeField] private Vector2 _minMaxSaturation;
        [MinMaxSlider(0f, 1f)]
        [SerializeField] private Vector2 _minMaxValue;

        public Color GetColor()
        {
            float hue = Random.Range(_minMaxHue.x, _minMaxHue.y);
            float sat = Random.Range(_minMaxSaturation.x, _minMaxSaturation.y);
            float val = Random.Range(_minMaxValue.x, _minMaxValue.y);

            return Color.HSVToRGB(hue, sat, val);
        }
    }
}