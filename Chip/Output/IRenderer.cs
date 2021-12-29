using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Output
{
    public interface IRenderer
    {
        void DrawFrame(IEnumerable<IEnumerable<bool>> frame, int width, int height);

        void ClearScreen();
    }
}
