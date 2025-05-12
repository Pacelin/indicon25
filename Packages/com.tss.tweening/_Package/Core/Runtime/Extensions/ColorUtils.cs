using JetBrains.Annotations;
using UnityEngine;

namespace TSS.Core.Extensions
{
    [PublicAPI]
    public static class ColorUtils
    {
        // ReSharper disable once InconsistentNaming
        public static Color HexToRGBA(string hex)
        {
            if (!hex.StartsWith('#'))
                hex = "#" + hex;
            if (ColorUtility.TryParseHtmlString(hex, out var color))
                return color;
            return Color.black;
        }
    }
}