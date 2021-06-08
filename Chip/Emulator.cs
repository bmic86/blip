using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chip
{
	public class Emulator
	{
		private readonly Machine _machine = new();
		private readonly CancellationTokenSource _taskCancelation = new();
		private Task _execution;

		public void RunProgram(byte[] program)
		{
			_ = program ?? throw new ArgumentNullException(nameof(program));

			if (_execution != null && !_execution.IsCompleted)
			{
				throw new InvalidOperationException("Emulator is already running.");
			}

			_execution = Task.Run(() => _machine.ExecuteProgram(program), _taskCancelation.Token);
		}

	}
}
