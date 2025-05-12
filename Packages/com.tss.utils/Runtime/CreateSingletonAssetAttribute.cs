using System;

namespace TSS.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateSingletonAssetAttribute : Attribute
    {
        public string Path { get; }
        public string Address { get; }

        public CreateSingletonAssetAttribute(string path, string address = null)
        {
            Path = path;
            Address = address;
        }
    }
}