using System;
using System.Diagnostics;
using UnityEngine;

namespace TSS.Achievements
{
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public class AchievementKeyAttribute : PropertyAttribute { }
}