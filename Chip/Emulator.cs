using Chip.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chip
{
	public class Emulator
	{
		public event EventHandler<ExecutionErrorEventArgs> OnExecutionError;

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

			if (program.Length > _machine.State.Memory.Length - Default.StartAddress)
			{
				throw new InvalidProgramException("Program is too large, it cannot be loaded into the memory.");
			}

			_execution = Task.Run(() => ExecuteProgram(program), _taskCancelation.Token)
				.ContinueWith(
					task => OnExecutionError?.Invoke(this, new ExecutionErrorEventArgs(task.Exception.InnerExceptions)),
					TaskContinuationOptions.OnlyOnFaulted);
		}

		private void ExecuteProgram(byte[] program)
		{
			_machine.State.Registers.ClearAll();
			_machine.ExecuteProgram(program);
		}
	}
}
