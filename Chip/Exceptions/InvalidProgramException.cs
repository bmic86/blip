using System;

namespace Chip.Exceptions
{
	public class InvalidProgramException : Exception
	{
		public InvalidProgramException(string message) : base(message)
		{
		}
	}
}
