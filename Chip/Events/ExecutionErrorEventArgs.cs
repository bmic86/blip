using System;
using System.Collections.Generic;

namespace Chip.Events
{
	public class ExecutionErrorEventArgs : EventArgs
	{
		public IReadOnlyCollection<Exception> ErrorList { get; private set; }

		public ExecutionErrorEventArgs(IReadOnlyCollection<Exception> innerExceptions)
		{
			ErrorList = innerExceptions;
		}
	}
}
