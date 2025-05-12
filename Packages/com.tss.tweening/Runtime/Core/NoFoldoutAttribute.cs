using System;
using System.Diagnostics;

namespace TSS.Tweening
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Class)]
    internal class NoFoldoutAttribute : Attribute
    {
    }
}