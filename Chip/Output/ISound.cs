using System.Threading.Tasks;

namespace Chip.Output
{
    public interface ISound
    {
        public Task PlayKeyDownToneAsync();

        public Task PlayToneAsync(double playTimeInSeconds);
    }
}
