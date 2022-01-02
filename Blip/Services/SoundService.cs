using Chip.Output;
using Howler.Blazor.Components;

namespace Blip.Services
{
    public class SoundService : ISound
    {
        private readonly IHowl _howler;

        public SoundService(IHowl howler)
        {
            _howler = howler ?? throw new ArgumentNullException(nameof(howler));
        }

        public async Task EmitToneAsync() => await _howler.Play("/sound/tone.wav");
    }
}
