using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip
{
	internal struct Instruction
	{
		int _instructionCode;

		public (int n1, int n2, int n3, int n4) Nibbles { get; private set; }

		public byte Value { get; private set; }

		public ushort Address => (ushort)(_instructionCode & 0x0FFF);

		public int VXIndex => Nibbles.n2 >> 8;

		public int VYIndex => Nibbles.n3 >> 4;

		public Instruction(byte highOrderInstructionByte, byte lowOrderInstructionByte)
		{
			_instructionCode = (highOrderInstructionByte << 8) | lowOrderInstructionByte;
			Nibbles = (n1: _instructionCode & 0xF000, n2: _instructionCode & 0x0F00, n3: _instructionCode & 0x00F0, n4: _instructionCode & 0x000F);
			Value = lowOrderInstructionByte;
		}
	}
}
