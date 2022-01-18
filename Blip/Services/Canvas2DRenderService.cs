using Chip.Display;
using Chip.Output;
using Microsoft.JSInterop;

namespace Blip.Services
{
    public class Canvas2DRenderService : IRenderer, IAsyncDisposable
    {
        private readonly IJSRuntime _js;

        private IJSObjectReference? _renderModule;

        public Canvas2DRenderService(IJSRuntime js)
        {
            _js = js ?? throw new ArgumentNullException(nameof(js));
        }

        private async Task InitRenderModuleAsync()
        {
            if (_renderModule == null)
            {
                _renderModule = await _js.InvokeAsync<IJSObjectReference>("import", "./js/canvas-render.js");
                await _renderModule.InvokeVoidAsync("init", "screen", 64, 32);
            }
        }

        public async Task ClearScreenAsync()
        {
            await InitRenderModuleAsync(); // Must guarantee, that _renderModule is not null.

#pragma warning disable CS8604 // Possible null reference argument.
            await _renderModule.InvokeVoidAsync("clearFrame");
            await _renderModule.InvokeVoidAsync("renderFrame");
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public async Task DrawPixelsAsync(IEnumerable<Pixel> pixels)
        {
            await InitRenderModuleAsync(); // Must guarantee, that _renderModule is not null.

#pragma warning disable CS8604 // Possible null reference argument.
            foreach (var pixel in pixels)
            {
                await _renderModule.InvokeVoidAsync("drawPixel", pixel.X, pixel.Y, pixel.Value);
            }

            await _renderModule.InvokeVoidAsync("renderFrame");
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public async ValueTask DisposeAsync()
        {
            if(_renderModule != null)
            {
                await _renderModule.DisposeAsync();
            }
        }
    }
}
