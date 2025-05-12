using JetBrains.Annotations;
using UnityEngine;

namespace TSS.Utils.Editor
{
    [PublicAPI]
    public static partial class TSSEditorUtils
    {
        public static Color AlphaMultiplied(this Color color, float multiplier) =>
            new Color(color.r, color.g, color.b, color.a * multiplier);
    }
}