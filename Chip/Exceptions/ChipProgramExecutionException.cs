using System;

namespace Chip.Exceptions
{
    public class ChipProgramExecutionException : Exception
    {
        public ChipProgramExecutionException(string message) : base(message)
        {
        }
    }
}
