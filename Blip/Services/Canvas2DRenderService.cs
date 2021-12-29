using Blazor.Extensions.Canvas.Canvas2D;
using Chip.Output;

namespace Blip.Services
{
    public class Canvas2DRenderService : IRenderer
    {
        Canvas2DContext _context;

        public Canvas2DRenderService(Canvas2DContext context) 
            => _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task ClearScreenAsync()
        {
            await _context.SetFillStyleAsync("black");
            await _context.FillRectAsync(0, 0, 640, 320);
        }

        public async Task DrawFrameAsync(IEnumerable<IEnumerable<bool>> frame, int width, int height)
        {
            await ClearScreenAsync();
            await _context.SetFillStyleAsync("white");

            int y = 0;
            foreach (var row in frame)
            {
                int x = 0;
                foreach (var val in row)
                {
                    if (val)
                    {
                        await _context.FillRectAsync(x * 10, y * 10, 10, 10);
                    }
                    x++;
                }
                y++;
            }
        }
    }
}
