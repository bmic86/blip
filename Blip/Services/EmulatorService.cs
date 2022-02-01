using Blip.Models;
using Chip;
using Microsoft.AspNetCore.Components.Forms;

namespace Blip.Services
{
    public class EmulatorService : IDisposable
    {
        private readonly Emulator _chipEmulator;

        private int _timerIntervalInMs = (int)ExecutionSpeed.Medium;
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

        public void ChangeExecutionSpeed(ExecutionSpeed speed)
        {
            _timerIntervalInMs = (int)speed;
            _timer?.Change(0, _timerIntervalInMs);
        }

        private void StartNewTimer()
        {
            _timer = new Timer(
                async _ => await _chipEmulator.ProcessNextMachineCycleAsync(),
                null,
                0,
                _timerIntervalInMs);
        }

        public void Dispose() => _timer?.Dispose();
    }
}
