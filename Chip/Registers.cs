using System;

namespace Chip
{
    internal class Registers
    {
        internal byte[] V { get; private set; } = new byte[16];
        internal ushort I { get; set; }
        internal ushort pc { get; set; }

        internal void ClearAll()
        {
            Array.Clear(V, 0, 16);
            I = 0;
            pc = 0;
        }
    }
}
