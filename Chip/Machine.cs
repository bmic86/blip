using Chip.Exceptions;
using Chip.Random;
using System;

namespace Chip
{

	internal class Machine
	{
		private const int InstructionSize = 2;

		internal MachineState State { get; private set; } = new();

		private bool _isBreakInstruction = false;
		private readonly IRandomGenerator _randomGenerator;

		internal Machine() => _randomGenerator = new RandomGenerator();

		internal Machine(IRandomGenerator randomGenerator) => _randomGenerator = randomGenerator;

		internal void ExecuteProgram(byte[] program)
		{
			State.Registers.PC = Default.StartAddress;
			InitializeMemory(program);
			Start();
		}

		private void InitializeMemory(byte[] program)
		{
			CharacterSprites.Data.CopyTo(State.Memory, 0);

			Array.Clear(State.Memory,
				CharacterSprites.TotalDataSize,
				Default.StartAddress - CharacterSprites.TotalDataSize);

			program.CopyTo(State.Memory, Default.StartAddress);

			Array.Clear(State.Memory,
				Default.StartAddress + program.Length,
				State.Memory.Length - Default.StartAddress - program.Length);
		}

		private void Start()
		{
			_isBreakInstruction = false;
			while (!_isBreakInstruction && IsNextInstructionAccessible())
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
				(0xF000, _, 0x0010, 0x000E) => AddVxToIndexRegister(instruction.VXIndex),
				(0xF000, _, 0x0020, 0x0009) => SetIndexRegisterToCharacterSpriteAddress(instruction.VXIndex),
				(0xF000, _, 0x0030, 0x0003) => StoreVxAsBinaryCodedDecimal(instruction.VXIndex),
				_ => BreakInstruction()
			};
		}

		private ushort StoreVxAsBinaryCodedDecimal(int x)
		{
			int index = State.Registers.I;
			if (index + 2 >= State.Memory.Length)
			{
				throw new ProgramExecutionException($"Cannot store V{x:X1} value as BCD under address 0x{index:X4}: operation will overflow the memory.");
			}

			State.Memory[index] = (byte)(State.Registers.V[x] / 100);
			State.Memory[index + 1] = (byte)(State.Registers.V[x] / 10 % 10);
			State.Memory[index + 2] = (byte)(State.Registers.V[x] % 10);

			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort SetIndexRegisterToCharacterSpriteAddress(int x)
		{
			State.Registers.I = (ushort)((State.Registers.V[x] % CharacterSprites.CharactersCount) * CharacterSprites.CharacterSize);
			return (ushort)(State.Registers.PC + InstructionSize);
		}

		private ushort AddVxToIndexRegister(int x)
		{
			State.Registers.I += State.Registers.V[x];
			return (ushort)(State.Registers.PC + InstructionSize);
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

		private ushort BreakInstruction()
		{
			_isBreakInstruction = true;
			return State.Registers.PC;
		}
	}
}
