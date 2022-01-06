using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Display
{
    internal class Screen
    {
        private const int MaxDisplayWidth = 64;
        private const int MaxDisplayHeight = 32;

        private bool[,] _screenBuffer = new bool[MaxDisplayWidth, MaxDisplayHeight];

        internal int Width => MaxDisplayWidth;
        internal int Height => MaxDisplayHeight;

        internal void Clear() => Array.Clear(_screenBuffer);

        internal bool DrawPixelsOctetFromByte(int x, int y, byte octet)
        {
            bool wasCollision = false;
            byte bitMask = 0b10000000;

            for (int i = 0; i < 8 && x < Width; ++i)
            {
                bool newPixelValue = (octet & bitMask) > 0;

                wasCollision |= newPixelValue && _screenBuffer[x, y];
                _screenBuffer[x, y] ^= newPixelValue;

                bitMask >>= 1;
                ++x;
            }

            return wasCollision;
        }

        internal IEnumerable<Pixel> ReadPixels(int x, int y, int width, int height)
        {
            for (int j = y; j < y + height && j < Height; ++j)
            {
                for (int i = x; i < x + width && i < Width; ++i)
                {
                    yield return new Pixel(i, j, _screenBuffer[i, j]);
                }
            }
        }
    }
}
