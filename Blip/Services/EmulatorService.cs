using Chip;
using Microsoft.AspNetCore.Components.Forms;

namespace Blip.Services
{
    public class EmulatorService : IDisposable
    {
        private const int FrequencyInHz = 500;

        private readonly Emulator _chipEmulator;
        private Timer? _timer;

        public EmulatorService(Emulator chipEmulator)
        {
            _chipEmulator = chipEmulator ?? throw new ArgumentNullException(nameof(chipEmulator));
        }

        public async Task StartNewProgramAsync(IBrowserFile browserFile)
        {
            _timer?.Dispose();

            await using (MemoryStream ms = new())
            {
                await browserFile.OpenReadStream().CopyToAsync(ms);
                await _chipEmulator.StartProgramAsync(ms.ToArray());
            }

            StartNewTimer();
        }

        public async Task StartNewProgramAsync(byte[] program)
        {
            _timer?.Dispose();
            await _chipEmulator.StartProgramAsync(program);
            StartNewTimer();
        }

        private void StartNewTimer()
        {
            _timer = new Timer(
                async _ => await _chipEmulator.ProcessNextMachineCycleAsync(),
                null,
                0,
                GetTimerIntervalInMs());
        }

        private int GetTimerIntervalInMs() => 1000 / FrequencyInHz;

        public void Dispose() => _timer?.Dispose();
    }
}
