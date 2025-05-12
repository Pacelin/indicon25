using System;
using System.Diagnostics;

namespace TSS.Tweening
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    internal class LabelAttribute : Attribute
    {
        public string Label { get; }
        public LabelAttribute(string label) => Label = label;
    }
}