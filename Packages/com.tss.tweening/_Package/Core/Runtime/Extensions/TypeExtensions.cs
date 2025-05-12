using JetBrains.Annotations;
using System;
using System.Linq;

namespace TSS.Core.Extensions
{
    [PublicAPI]
    public static class TypeExtensions
    {
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            while (true)
            {
                Type[] interfaceTypes = givenType.GetInterfaces();

                if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
                {
                    return true;
                }

                if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType) return true;

                Type baseType = givenType.BaseType;
                if (baseType == null) return false;

                givenType = baseType;
            }
        }
    }
}