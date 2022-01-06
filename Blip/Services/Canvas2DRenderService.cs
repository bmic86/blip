using Blazor.Extensions.Canvas.Canvas2D;
using Chip.Display;
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

        public async Task DrawPixelsAsync(IEnumerable<Pixel> pixels)
        {
            foreach (var pixel in pixels)
            {
                await _context.SetFillStyleAsync(pixel.Value ? "white" : "black");
                await _context.FillRectAsync(pixel.X * 10, pixel.Y * 10, 10, 10);
            }
        }
    }
}
