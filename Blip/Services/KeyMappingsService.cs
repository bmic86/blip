using Chip.Input;

namespace Blip.Services
{
    public class KeyMappingsService
    {
        private Dictionary<string, Key> _mappings = new Dictionary<string, Key>
        {
            { "Digit1", Key.Num1 }, { "Digit2", Key.Num2 }, { "Digit3", Key.Num3 }, { "Digit4", Key.C },
            { "KeyQ", Key.Num4 }, { "KeyW", Key.Num5 }, { "KeyE", Key.Num6 }, { "KeyR", Key.D },
            { "KeyA", Key.Num7 }, { "KeyS", Key.Num8 }, { "KeyD", Key.Num9 }, { "KeyF", Key.E },
            { "KeyZ", Key.A }, { "KeyX", Key.Num0 }, { "KeyC", Key.B }, { "KeyV", Key.F },
        };

        public Key? GetKeyMapping(string key) 
            => _mappings.TryGetValue(key, out var mappedKey) ? mappedKey : null;
    }
}
