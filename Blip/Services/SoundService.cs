using Chip.Output;
using Microsoft.JSInterop;

namespace Blip.Services
{
    public class SoundService : ISound, IAsyncDisposable
    {
        private readonly IJSRuntime _js;

        private IJSObjectReference? _soundModule;

        public SoundService(IJSRuntime js)
        {
            _js = js ?? throw new ArgumentNullException(nameof(js));
        }

        private async Task InitSoundModuleAsync()
        {
            if (_soundModule == null)
            {
                _soundModule = await _js.InvokeAsync<IJSObjectReference>("import", "./js/sound.js");
            }
        }

        public async Task EmitToneAsync()
        {
            await InitSoundModuleAsync();
#pragma warning disable CS8604 // Possible null reference argument.
            await _soundModule.InvokeVoidAsync("play", 0.4);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public async ValueTask DisposeAsync()
        {
            if (_soundModule != null)
            {
                await _soundModule.DisposeAsync();
            }
        }
    }
}
