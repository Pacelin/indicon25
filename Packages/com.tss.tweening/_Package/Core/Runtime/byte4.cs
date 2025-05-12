using System.Runtime.InteropServices;

namespace TSS.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct byte4
    {
        public readonly byte x;
        public readonly byte y;
        public readonly byte z;
        public readonly byte w;
        
        public byte4(in byte x, in byte y, in byte z, byte w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}