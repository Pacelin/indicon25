using System;
using System.Diagnostics;

namespace TSS.Tweening
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    internal class BoxAttribute : Attribute
    {
        public string Name { get; }
        public BoxAttribute(string name) => Name = name;
    }
}