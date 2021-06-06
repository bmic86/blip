using System;
using System.Threading.Tasks;

namespace Chip
{
	public class Emulator
	{
		private MachineState _state = new();
		private Processor _processor;
		private Task _execution;

		public Emulator()
		{
			_processor = new(_state);
		}

		public void Run(byte[] program)
		{
			_ = program ?? throw new ArgumentNullException(nameof(program));

			//_state.ClearAll();
			LoadProgram(program);

			if (_execution == null)
			{
				_execution = Task.Factory.StartNew(ExecuteProgram);
			}

		}

		private void ExecuteProgram()
		{
			while (_processor.ExecuteNextInstruction()){ }
		}

		private void LoadProgram(byte[] program) => program.CopyTo(_state.Memory, Default.StartAddress);
	}
}
