using System;

namespace Chip.Exceptions
{
	public class ProgramExecutionException : Exception
	{
		public ProgramExecutionException(string message) : base(message)
		{
		}
	}
}
