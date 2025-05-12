using System;
using System.Diagnostics;

namespace TSS.Tweening
{
    [AttributeUsage(AttributeTargets.Class)]
    [Conditional("UNITY_EDITOR")]
    public class ScriptableTweenPathAttribute : Attribute
    {
        public string Path { get; }
        public int Order { get; }

        public ScriptableTweenPathAttribute(string path, int order = 0)
        {
            Path = path;
            Order = order;
        } 
    }
}