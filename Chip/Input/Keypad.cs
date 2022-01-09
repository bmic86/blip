using Chip.Output;
using System.Threading.Tasks;

namespace Chip.Input
{
    public class Keypad
    {
        private readonly bool[] _isKeyPressed = new bool[16];
        private readonly ISound _sound;
        private bool _isInCaptureSingleKeyMode = false;

        public void KeyUp(Key key) => _isKeyPressed[(int)key] = false;

        public async Task KeyDownAsync(Key key)
        {
            _isKeyPressed[(int)key] = true;
            if (_isInCaptureSingleKeyMode && CapturedKey == null)
            {
                CapturedKey = key;
                await _sound.PlayKeyDownToneAsync();
            }
        }

        internal Keypad(ISound sound) => _sound = sound;

        internal bool IsKeyPressed(int keyValue) => _isKeyPressed[keyValue];

        internal Key? CapturedKey { get; private set; }

        internal void EnableCaptureSingleKeyMode() => _isInCaptureSingleKeyMode = true;

        internal void DisableCaptureSingleKeyMode()
        {
            _isInCaptureSingleKeyMode = false;
            CapturedKey = null;
        }
    }
}
