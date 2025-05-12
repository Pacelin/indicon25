using System;

namespace TSS.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RuntimeOrderAttribute : Attribute
    {
        public int Order { get; }
        public RuntimeOrderAttribute(int order) =>
            Order = order;

        public RuntimeOrderAttribute(ERuntimeOrder order, int add = 0) =>
            Order = (int) order + add;
    }
}