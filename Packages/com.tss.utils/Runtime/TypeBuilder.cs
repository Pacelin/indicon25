using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TSS.Utils
{
    public class TypeBuilder
    {
        private readonly List<Predicate<Type>> _filters = new List<Predicate<Type>>();
        
        public TypeBuilder NonAbstractClass()
        {
            _filters.Add(t => t.IsClass && !t.IsAbstract);
            return this;
        }

        public TypeBuilder DerivedFrom<T>()
        {
            var derivedType = typeof(T);
            _filters.Add(t => t != derivedType && derivedType.IsAssignableFrom(t));
            return this;
        }
        
        public IEnumerable<Type> Get()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var type in assembly.GetTypes())
                    if (_filters.All(filter => filter(type)))
                        yield return type;
        }
    }
}