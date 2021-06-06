using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip
{
	internal class Processor
	{
		private readonly MachineState _machineState;

		internal Processor(MachineState machineState)
		{
			_machineState = machineState;
		}

		public bool ExecuteNextInstruction()
		{
			throw new NotImplementedException();
		}

		internal ushort Execute(byte highOrderInstructionByte, byte lowOrderInstructionByte)
		{
			throw new NotImplementedException();
		}


	}
}
