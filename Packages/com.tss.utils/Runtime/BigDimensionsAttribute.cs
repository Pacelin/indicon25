using System;
using System.Diagnostics;
using UnityEngine;

namespace TSS.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public class BigDimensionsAttribute : PropertyAttribute
    {
        public int Dimensions { get; }
        public int Rows { get; }
        
        public BigDimensionsAttribute(int dimensions, int rows)
        {
            Dimensions = Mathf.Min(dimensions, TSSUtils.MaxBigIntDimensions);
            Rows = rows;
        }
    }
}