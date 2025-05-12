using System;
using System.Diagnostics;

namespace TSS.Tweening
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    internal class OrderAttribute : Attribute
    {
        public int Order { get; }
        public OrderAttribute(int order) => Order = order;
    }
}