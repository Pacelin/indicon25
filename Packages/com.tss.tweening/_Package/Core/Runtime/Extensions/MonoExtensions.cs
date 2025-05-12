using JetBrains.Annotations;
using UnityEngine;

namespace TSS.Core.Extensions
{
    [PublicAPI]
    public static class MonoExtensions
    {
        public static T GetOrAddComponent<T>(this Component self) where T : Component
        {
            if (self.TryGetComponent(out T component))
                return component;

            return self.gameObject.AddComponent<T>();
        }
        
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            if (self.TryGetComponent(out T component))
                return component;

            return self.AddComponent<T>();
        }
    }
}