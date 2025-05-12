using System;

namespace TSS.Utils.Editor.ValueProviders
{
    public class ValueProviderEntry
    {
        public string Name { get; }
        public Type Type { get; }
        public Func<object> ObjFactory { get; }
        
        private ValueProviderEntry(string name, Func<object> objFactory, Type type)
        {
            Name = name;
            Type = type;
            ObjFactory = objFactory;
        }

        public static ValueProviderEntry Create<T>(string name, Func<object> factory) =>
            new ValueProviderEntry(name, factory, typeof(T));
    }
}