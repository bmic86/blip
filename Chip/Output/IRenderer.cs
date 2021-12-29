using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Output
{
    public interface IRenderer
    {
        Task DrawFrameAsync(IEnumerable<IEnumerable<bool>> frame, int width, int height);

        Task ClearScreenAsync();
    }
}
