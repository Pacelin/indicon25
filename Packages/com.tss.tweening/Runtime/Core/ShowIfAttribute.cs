using System;
using System.Diagnostics;

namespace TSS.Tweening
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    internal class ShowIfAttribute : Attribute
    {
        public string ValueProvider { get; }
        public ShowIfAttribute(string valueProvider) => ValueProvider = valueProvider;
    }
}