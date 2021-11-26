using System;

namespace Chip.Exceptions
{
    public class InvalidChipProgramException : Exception
    {
        public InvalidChipProgramException(string message) : base(message)
        {
        }
    }
}
