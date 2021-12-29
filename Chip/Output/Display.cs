using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Output
{
    internal class Display
    {
        private const int MaxDisplayWidth = 64;
        private const int MaxDisplayHeight = 32;

        private bool[,] _displayBuffer = new bool[MaxDisplayWidth, MaxDisplayHeight];

        internal int Width => MaxDisplayWidth;
        internal int Height => MaxDisplayHeight;

        internal void Clear() => Array.Clear(_displayBuffer);

        internal bool DrawPixelsOctet(int x, int y, byte octet)
        {
            bool wasCollision = false;
            byte bitMask = 0b10000000;

            for (int i = 0; i < 8 && x < Width; ++i)
            {
                bool newPixelValue = (octet & bitMask) > 0;

                wasCollision |= newPixelValue && _displayBuffer[x, y];
                _displayBuffer[x, y] ^= newPixelValue;

                bitMask >>= 1;
                ++x;
            }

            return wasCollision;
        }

        internal IEnumerable<IEnumerable<bool>> ReadFrame()
        {
            for (int rowNum = 0; rowNum < MaxDisplayHeight; rowNum++)
            {
                yield return ReadPixelsRow(rowNum);
            }
        }

        private IEnumerable<bool> ReadPixelsRow(int rowNum)
        {
            for (int colNum = 0; colNum < MaxDisplayWidth; colNum++)
            {
                yield return _displayBuffer[colNum, rowNum];
            }
        }
    }
}
