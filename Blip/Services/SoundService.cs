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

        public async Task PlayKeyDownToneAsync() => await PlayToneAsync(0.4);

        public async Task PlayToneAsync(double playTimeInSeconds)
        {
            await InitSoundModuleAsync();

#pragma warning disable CS8604 // Possible null reference argument.

            // InitSoundModuleAsync() call must guarantee, that _soundModule is not null.
            await _soundModule.InvokeVoidAsync("play", playTimeInSeconds);

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
