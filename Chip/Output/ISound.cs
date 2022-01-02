using System.Threading.Tasks;

namespace Chip.Output
{
    public interface ISound
    {
        public Task EmitToneAsync();
    }
}
