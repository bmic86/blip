using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chip
{
	public class Emulator
	{
		private readonly MachineState _state = new();
		private readonly Processor _processor;
		private readonly CancellationTokenSource _taskCancelation = new();
		private Task _execution;

		public Emulator()
		{
			_processor = new(_state);
		}

		public void Run(byte[] program)
		{
			_ = program ?? throw new ArgumentNullException(nameof(program));

			if (_execution != null && !_execution.IsCompleted)
			{
				throw new InvalidOperationException("Emulator is already running.");
			}

			LoadProgram(program);
			StartProgram();
		}

		private void ExecuteProgram()
		{
			while (_processor.ExecuteNextInstruction()) { }
		}

		private void LoadProgram(byte[] program) => program.CopyTo(_state.Memory, Default.StartAddress);
		private void StartProgram() => _execution = Task.Factory.StartNew(ExecuteProgram, _taskCancelation.Token);

	}
}
