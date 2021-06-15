namespace Chip
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
				(0x5000, _, _, 0x0000) => SkipNextOnRegistersEqual(nibbles.n2 >> 8, nibbles.n3 >> 4),
				(0x6000, _, _, _) => LoadValueToRegister(nibbles.n2 >> 8, (byte)(instructionCode & 0x00FF)),
				(0x7000, _, _, _) => AddValueToRegister(nibbles.n2 >> 8, (byte)(instructionCode & 0x00FF)),
				(0x8000, _, _, 0x0000) => CopyRegisterVyToRegisterVx(nibbles.n2 >> 8, nibbles.n3 >> 4),
				_ => InvalidInstruction()
			};
		}

		private ushort CopyRegisterVyToRegisterVx(int x, int y)
		{
			State.Registers.V[x] = State.Registers.V[y];
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort AddValueToRegister(int x, byte value)
		{
			State.Registers.V[x] += value;
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort LoadValueToRegister(int x, byte value)
		{
			State.Registers.V[x] = value;
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort SkipNextOnEqual(int x, byte valueToCompare)
		{
			int offset = State.Registers.V[x] == valueToCompare ? InstructionSize * 2 : InstructionSize;
			return (ushort)(State.Registers.PC + offset);
		}

		private ushort SkipNextOnNotEqual(int x, byte valueToCompare)
		{
			int offset = State.Registers.V[x] != valueToCompare ? InstructionSize * 2 : InstructionSize;
			return (ushort)(State.Registers.PC + offset);
		}

		private ushort SkipNextOnRegistersEqual(int x, int y)
		{
			int offset = State.Registers.V[x] == State.Registers.V[y] ? InstructionSize * 2 : InstructionSize;
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
