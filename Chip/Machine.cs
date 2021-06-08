using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip
{
	internal class Machine
	{
		private readonly MachineState _state = new();

		public bool ExecuteNextInstruction()
		{
			throw new NotImplementedException();
		}

		internal ushort Execute(byte highOrderInstructionByte, byte lowOrderInstructionByte)
		{
			throw new NotImplementedException();
		}

		internal void ExecuteProgram(byte[] program)
		{
			LoadProgram(program);
			Start();
		}

		private void LoadProgram(byte[] program) => program.CopyTo(_state.Memory, Default.StartAddress);

		private void Start()
		{
			while (ExecuteNextInstruction()) { }
		}
	}
}
