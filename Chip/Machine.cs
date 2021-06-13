﻿namespace Chip
{

	internal class Machine
	{
		private const int InstructionSize = 2;

		internal MachineState State { get; private set; } = new();

		private bool isInvalidInstruction = false;

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
			isInvalidInstruction = false;
			while (!isInvalidInstruction && IsNextInstructionAccessible())
			{
				ushort currentPc = State.Registers.PC;
				int instructionCode = ExtractInstructionCode(State.Memory[currentPc], State.Memory[currentPc + 1]);
				State.Registers.PC = Execute(instructionCode);
			}
		}

		private bool IsNextInstructionAccessible() => State.Registers.PC + InstructionSize < Default.MemorySize;

		private ushort Execute(int instructionCode)
		{
			var nibbles = ExtractNibbles(instructionCode);
			return nibbles switch
			{
				(0x1000, _, _, _) => (ushort)(instructionCode & 0x0FFF),
				(0x3000, _, _, _) => SkipNextOnEqual(nibbles.n2 >> 8, (byte)(instructionCode & 0x00FF)),
				(0x4000, _, _, _) => SkipNextOnNotEqual(nibbles.n2 >> 8, (byte)(instructionCode & 0x00FF)),
				_ => InvalidInstruction()
			};
		}

		private ushort SkipNextOnEqual(int x, byte nn)
		{
			int offset = State.Registers.V[x] == nn ? InstructionSize * 2 : InstructionSize;
			return (ushort)(State.Registers.PC + offset);
		}

		private ushort SkipNextOnNotEqual(int x, byte nn)
		{
			int offset = State.Registers.V[x] != nn ? InstructionSize * 2 : InstructionSize;
			return (ushort)(State.Registers.PC + offset);
		}

		private ushort InvalidInstruction()
		{
			isInvalidInstruction = true;
			return State.Registers.PC;
		}

		private static int ExtractInstructionCode(byte highOrderInstructionByte, byte lowOrderInstructionByte) =>
			(highOrderInstructionByte << 8) | lowOrderInstructionByte;

		private static (int n1, int n2, int n3, int n4) ExtractNibbles(int value) =>
			(n1: value & 0xF000, n2: value & 0x0F00, n3: value & 0x00F0, n4: value & 0x000F);
	}
}
