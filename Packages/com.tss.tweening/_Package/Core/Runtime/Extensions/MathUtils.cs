using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace TSS.Core.Extensions
{
    [PublicAPI]
    public static class MathUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IncrementRepeat(in int value, in int count)
        {
            return (value + 1) % count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DecrementRepeat(in int value, in int count)
        {
            return (value - 1 + count) % count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 ToFloat4(this Color color)
        {
            return new float4(color.r, color.g, color.b, color.a);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ToFloat3(this Color color)
        {
            return new float3(color.r, color.g, color.b);
        }

        // ReSharper disable once InconsistentNaming
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte4 Float4ToRGBA32(float4 color)
        {
            color = math.clamp(color, 0f, 1f) * 255f;

            byte r = (byte)color.w;
            byte g = (byte)color.z;
            byte b = (byte)color.y;
            byte a = (byte)color.x;

            return new byte4(r, g, b, a);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte3 Float3ToRGB24(float3 color)
        {
            color = math.clamp(color, 0f, 1f) * 255f;

            byte r = (byte)color.x;
            byte g = (byte)color.y;
            byte b = (byte)color.z;

            return new byte3(r, g, b);
        }
    }
}
