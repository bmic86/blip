using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Display
{
    public interface IRenderer
    {
        Task DrawPixelsAsync(IEnumerable<Pixel> pixels);

        Task ClearScreenAsync();
    }
}
