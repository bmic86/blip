using Chip.Random;
using System;

namespace Chip
{

	internal class Machine
	{
		private const int InstructionSize = 2;

		internal MachineState State { get; private set; } = new();

		private bool _isInvalidInstruction = false;
		private readonly IRandomGenerator _randomGenerator;

		internal Machine() => _randomGenerator = new RandomGenerator();

		internal Machine(IRandomGenerator randomGenerator) => _randomGenerator = randomGenerator;

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
			_isInvalidInstruction = false;
			while (!_isInvalidInstruction && IsNextInstructionAccessible())
			{
				State.Registers.PC = ExecuteNextInstruction();
			}
		}

		private bool IsNextInstructionAccessible() => State.Registers.PC + InstructionSize < Default.MemorySize;

		private ushort ExecuteNextInstruction()
		{
			ushort currentPc = State.Registers.PC;
			var instruction = new Instruction(State.Memory[currentPc], State.Memory[currentPc + 1]);

			return instruction.Nibbles switch
			{
				(0x1000, _, _, _) => instruction.Address,
				(0x3000, _, _, _) => SkipNextOnEqual(instruction.VXIndex, instruction.Value),
				(0x4000, _, _, _) => SkipNextOnNotEqual(instruction.VXIndex, instruction.Value),
				(0x5000, _, _, 0x0000) => SkipNextOnRegistersEqual(instruction.VXIndex, instruction.VYIndex),
				(0x6000, _, _, _) => LoadValueToRegister(instruction.VXIndex, instruction.Value),
				(0x7000, _, _, _) => AddValueToRegister(instruction.VXIndex, instruction.Value),
				(0x8000, _, _, 0x0000) => CopyRegisterVyToRegisterVx(instruction.VXIndex, instruction.VYIndex),
				(0x8000, _, _, 0x0001) => ApplyVxOrVy(instruction.VXIndex, instruction.VYIndex),
				(0x8000, _, _, 0x0002) => ApplyVxAndVy(instruction.VXIndex, instruction.VYIndex),
				(0x8000, _, _, 0x0003) => ApplyVxXorVy(instruction.VXIndex, instruction.VYIndex),
				(0x8000, _, _, 0x0004) => AddWithCarry(instruction.VXIndex, instruction.VYIndex),
				(0x8000, _, _, 0x0005) => SubtractWithBorrow(instruction.VXIndex, instruction.VYIndex),
				(0x8000, _, _, 0x0006) => RightBitShift(instruction.VXIndex, instruction.VYIndex),
				(0x8000, _, _, 0x0007) => ReversedSubtractWithBorrow(instruction.VXIndex, instruction.VYIndex),
				(0x8000, _, _, 0x000E) => LeftBitShift(instruction.VXIndex, instruction.VYIndex),
				(0x9000, _, _, 0x0000) => SkipNextOnRegistersNotEqual(instruction.VXIndex, instruction.VYIndex),
				(0xA000, _, _, _) => LoadAddressToIndexRegister(instruction.Address),
				(0xB000, _, _, _) => (ushort)(instruction.Address + State.Registers.V[0]),
				(0xC000, _, _, _) => SetRandomValueWithMaskOnVx(instruction.VXIndex, instruction.Value),
				_ => InvalidInstruction()
			};
		}

		private ushort LeftBitShift(int x, int y)
		{
			byte mostSignificantBit = (byte)((State.Registers.V[y] & 0b10000000) >> 7);
			State.Registers.V[x] = (byte)(State.Registers.V[y] << 1);
			State.Registers.V[0xF] = mostSignificantBit;
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort RightBitShift(int x, int y)
		{
			byte leastSignificantBit = (byte)(State.Registers.V[y] & 1);
			State.Registers.V[x] = (byte)(State.Registers.V[y] >> 1);
			State.Registers.V[0xF] = leastSignificantBit;
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort ReversedSubtractWithBorrow(int x, int y)
		{
			byte notBorrowFlag = (byte)(State.Registers.V[y] >= State.Registers.V[x] ? 1 : 0);
			State.Registers.V[x] = (byte)(State.Registers.V[y] - State.Registers.V[x]);
			State.Registers.V[0xF] = notBorrowFlag;
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort SubtractWithBorrow(int x, int y)
		{
			byte notBorrowFlag = (byte)(State.Registers.V[x] >= State.Registers.V[y] ? 1 : 0);
			State.Registers.V[x] = (byte)(State.Registers.V[x] - State.Registers.V[y]);
			State.Registers.V[0xF] = notBorrowFlag;
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort AddWithCarry(int x, int y)
		{
			int result = State.Registers.V[y] + State.Registers.V[x];
			State.Registers.V[x] = (byte)result;
			State.Registers.V[0xF] = (byte)(result > byte.MaxValue ? 1 : 0);
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort ApplyVxXorVy(int x, int y)
		{
			State.Registers.V[x] ^= State.Registers.V[y];
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort ApplyVxAndVy(int x, int y)
		{
			State.Registers.V[x] &= State.Registers.V[y];
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort ApplyVxOrVy(int x, int y)
		{
			State.Registers.V[x] |= State.Registers.V[y];
			return (ushort)(State.Registers.PC + InstructionSize);
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

		private ushort SkipNextOnRegistersNotEqual(int x, int y)
		{
			int offset = State.Registers.V[x] != State.Registers.V[y] ? InstructionSize * 2 : InstructionSize;
			return (ushort)(State.Registers.PC + offset);
		}

		private ushort LoadAddressToIndexRegister(ushort address)
		{
			State.Registers.I = address;
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort SetRandomValueWithMaskOnVx(int x, byte mask)
		{
			State.Registers.V[x] = (byte)(_randomGenerator.Generate() & mask);
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort InvalidInstruction()
		{
			_isInvalidInstruction = true;
			return State.Registers.PC;
		}
	}
}
