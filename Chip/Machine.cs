using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chip
{

	internal class Machine
	{
		internal MachineState State { get; private set; } = new();

		internal void ExecuteProgram(byte[] program)
		{
			LoadProgram(program);
			Start();
		}

		private void LoadProgram(byte[] program)
		{
			program.CopyTo(State.Memory, Default.StartAddress);
			State.Registers.PC = Default.StartAddress;
		}

		private void Start()
		{
			while (IsNextInstructionAccessible())
			{
				ushort currentPc = State.Registers.PC;
				State.Registers.PC = Execute(State.Memory[currentPc], State.Memory[currentPc + 1]);
			}
		}

		private bool IsNextInstructionAccessible() => State.Registers.PC + 1 < Default.MemorySize;

		private ushort Execute(byte highOrderInstructionByte, byte lowOrderInstructionByte)
		{
			var nibbles = ExtractNibbles(highOrderInstructionByte, lowOrderInstructionByte);
			return nibbles switch
			{
				(0x0, _, _, _) => IgnoreInstruction()
			};
		}

		private ushort IgnoreInstruction()
		{
			return (ushort)(State.Registers.PC + 2);
		}

		private static (byte n1, byte n2, byte n3, byte n4) ExtractNibbles(byte highOrderByte, byte lowOrderByte)
		{
			byte n1 = (byte)(highOrderByte & 0xF0 >> 4);
			byte n2 = (byte)(highOrderByte & 0x0F);
			byte n3 = (byte)(lowOrderByte & 0xF0 >> 4);
			byte n4 = (byte)(lowOrderByte & 0x0F);
			return (n1, n2, n3, n4);
		}
	}
}
