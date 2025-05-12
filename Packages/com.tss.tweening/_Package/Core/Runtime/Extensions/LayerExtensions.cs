using JetBrains.Annotations;
using UnityEngine;

namespace TSS.Core.Extensions
{
    [PublicAPI]
    public static class LayerExtensions
    {
        public static int GetLayerId(this LayerMask mask)
        {
            int layerNumber = -1;
            int layer = mask.value;
            while (layer > 0)
            {
                layer >>= 1;
                layerNumber++;
            }

            return layerNumber;
        }
    }
}
