using Chip.Exceptions;
using Chip.Input;
using Chip.Output;
using Chip.Random;
using System;

namespace Chip
{
    public class Emulator
    {
        private const int InstructionSize = 2;

        private readonly IRandomGenerator _randomGenerator;
        private readonly IRenderer _renderer;

        internal Display Display { get; private set; } = new();

        internal MachineState State { get; private set; } = new();

        public Keypad Keypad { get; private set; }

        public Emulator(ISound sound, IRenderer renderer)
        {
            Keypad = new Keypad(sound);
            _renderer = renderer;
            _randomGenerator = new RandomGenerator();
        }

        internal Emulator(ISound sound, IRandomGenerator randomGenerator)
        {
            Keypad = new Keypad(sound);
            _randomGenerator = randomGenerator;
        }

        public void LoadProgram(byte[] program)
        {
            _ = program ?? throw new ArgumentNullException(nameof(program));

            if (program.Length > State.Memory.Length - Default.StartAddress)
            {
                throw new InvalidChipProgramException("Program is too large, it cannot be loaded into the memory.");
            }

            State.Stack.Clear();
            State.Registers.ClearAll();
            State.Registers.PC = Default.StartAddress;
            InitializeMemory(program);
        }

        public void ProcessNextMachineCycle() =>
            State.Registers.PC = ExecuteNextInstruction();

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

        private ushort ExecuteNextInstruction()
        {
            ushort currentPc = State.Registers.PC;
            var instruction = new Instruction(State.Memory[currentPc], State.Memory[currentPc + 1]);

            return instruction.Nibbles switch
            {
                (0x0000, 0x0000, 0x00E0, 0x0000) => ClearScreen(),
                (0x0000, 0x0000, 0x00E0, 0x000E) => ReturnFromSubroutine(),
                (0x1000, _, _, _) => instruction.Address,
                (0x2000, _, _, _) => CallSubroutine(instruction.Address),
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
                (0xD000, _, _, _) => DrawSprite(instruction.VXIndex, instruction.VYIndex, instruction.Nibbles.n4),
                (0xE000, _, 0x0090, 0x000E) => SkipNextOnKeyPressed(instruction.VXIndex),
                (0xE000, _, 0x00A0, 0x0001) => SkipNextOnKeyNotPressed(instruction.VXIndex),
                (0xF000, _, 0x0000, 0x000A) => WaitForKeyPress(instruction.VXIndex),
                (0xF000, _, 0x0010, 0x000E) => AddVxToIndexRegister(instruction.VXIndex),
                (0xF000, _, 0x0020, 0x0009) => SetIndexRegisterToCharacterSpriteAddress(instruction.VXIndex),
                (0xF000, _, 0x0030, 0x0003) => StoreVxAsBinaryCodedDecimal(instruction.VXIndex),
                (0xF000, _, 0x0050, 0x0005) => StoreRegistersInBulk(instruction.VXIndex),
                (0xF000, _, 0x0060, 0x0005) => LoadRegistersInBulk(instruction.VXIndex),
                _ => throw new ChipProgramExecutionException($"Unrecognized instruction.")
            };
        }

        private ushort DrawSprite(int x, int y, int n)
        {
            int drawPositionX = State.Registers.V[x] % Display.Width;
            int drawPositionY = State.Registers.V[y] % Display.Height;
            int startIndex = State.Registers.I;

            bool wasCollision = false;
            for (int i = 0; i < n && drawPositionY < Display.Height; ++i)
            {
                wasCollision |= Display.DrawPixelsOctet(drawPositionX, drawPositionY, State.Memory[startIndex + i]);
                ++drawPositionY;
            }

            State.Registers.V[0xF] = Convert.ToByte(wasCollision);

            _renderer.DrawFrame(Display.ReadFrame(), Display.Width, Display.Height);
            return GetNextInstructionAddress();
        }

        private ushort ClearScreen()
        {
            Display.Clear();
            _renderer.ClearScreen();
            return GetNextInstructionAddress();
        }

        private ushort WaitForKeyPress(int x)
        {
            Keypad.EnableCaptureSingleKeyMode();
            if (Keypad.CapturedKey.HasValue)
            {
                var keyCode = (byte)Keypad.CapturedKey.Value;
                State.Registers.V[x] = keyCode;
                if (!Keypad.IsKeyPressed(keyCode))
                {
                    Keypad.DisableCaptureSingleKeyMode();
                    return GetNextInstructionAddress();
                }
            }
            return State.Registers.PC;
        }

        private ushort SkipNextOnKeyNotPressed(int x)
             => !IsVxValueDownOnKeyboard(x) ? GetInstructionAddress(InstructionSize * 2) : GetNextInstructionAddress();

        private ushort SkipNextOnKeyPressed(int x)
            => IsVxValueDownOnKeyboard(x) ? GetInstructionAddress(InstructionSize * 2) : GetNextInstructionAddress();

        private bool IsVxValueDownOnKeyboard(int x) => Keypad.IsKeyPressed(State.Registers.V[x] & 0x0F);

        private ushort CallSubroutine(ushort subroutineAddress)
        {
            State.Stack.Push(GetNextInstructionAddress());
            return subroutineAddress;
        }

        private ushort ReturnFromSubroutine() => State.Stack.Pop();

        private ushort LoadRegistersInBulk(int x)
        {
            for (int i = 0; i <= x; ++i)
            {
                int memoryAddress = State.Registers.I;
                State.Registers.V[i] = State.Memory[memoryAddress];
                ++State.Registers.I;
            }

            return GetNextInstructionAddress();
        }

        private ushort StoreRegistersInBulk(int x)
        {
            for (int i = 0; i <= x; ++i)
            {
                int memoryAddress = State.Registers.I;
                State.Memory[memoryAddress] = State.Registers.V[i];
                ++State.Registers.I;
            }

            return GetNextInstructionAddress();
        }

        private ushort StoreVxAsBinaryCodedDecimal(int x)
        {
            int index = State.Registers.I;
            if (index + 2 >= State.Memory.Length)
            {
                throw new ChipProgramExecutionException($"Cannot store V{x:X1} value as BCD under address 0x{index:X4}: operation will overflow the memory.");
            }

            State.Memory[index] = (byte)(State.Registers.V[x] / 100);
            State.Memory[index + 1] = (byte)(State.Registers.V[x] / 10 % 10);
            State.Memory[index + 2] = (byte)(State.Registers.V[x] % 10);

            return GetNextInstructionAddress();
        }

        private ushort SetIndexRegisterToCharacterSpriteAddress(int x)
        {
            State.Registers.I = (ushort)((State.Registers.V[x] % CharacterSprites.CharactersCount) * CharacterSprites.CharacterSize);
            return GetNextInstructionAddress();
        }

        private ushort AddVxToIndexRegister(int x)
        {
            State.Registers.I += State.Registers.V[x];
            return GetNextInstructionAddress();
        }

        private ushort LeftBitShift(int x, int y)
        {
            byte mostSignificantBit = (byte)((State.Registers.V[y] & 0b10000000) >> 7);
            State.Registers.V[x] = (byte)(State.Registers.V[y] << 1);
            State.Registers.V[0xF] = mostSignificantBit;
            return GetNextInstructionAddress();
        }

        private ushort RightBitShift(int x, int y)
        {
            byte leastSignificantBit = (byte)(State.Registers.V[y] & 1);
            State.Registers.V[x] = (byte)(State.Registers.V[y] >> 1);
            State.Registers.V[0xF] = leastSignificantBit;
            return GetNextInstructionAddress();
        }

        private ushort ReversedSubtractWithBorrow(int x, int y)
        {
            byte notBorrowFlag = (byte)(State.Registers.V[y] >= State.Registers.V[x] ? 1 : 0);
            State.Registers.V[x] = (byte)(State.Registers.V[y] - State.Registers.V[x]);
            State.Registers.V[0xF] = notBorrowFlag;
            return GetNextInstructionAddress();
        }

        private ushort SubtractWithBorrow(int x, int y)
        {
            byte notBorrowFlag = (byte)(State.Registers.V[x] >= State.Registers.V[y] ? 1 : 0);
            State.Registers.V[x] = (byte)(State.Registers.V[x] - State.Registers.V[y]);
            State.Registers.V[0xF] = notBorrowFlag;
            return GetNextInstructionAddress();
        }

        private ushort AddWithCarry(int x, int y)
        {
            int result = State.Registers.V[y] + State.Registers.V[x];
            State.Registers.V[x] = (byte)result;
            State.Registers.V[0xF] = (byte)(result > byte.MaxValue ? 1 : 0);
            return GetNextInstructionAddress();
        }

        private ushort ApplyVxXorVy(int x, int y)
        {
            State.Registers.V[x] ^= State.Registers.V[y];
            return GetNextInstructionAddress();
        }

        private ushort ApplyVxAndVy(int x, int y)
        {
            State.Registers.V[x] &= State.Registers.V[y];
            return GetNextInstructionAddress();
        }

        private ushort ApplyVxOrVy(int x, int y)
        {
            State.Registers.V[x] |= State.Registers.V[y];
            return GetNextInstructionAddress();
        }

        private ushort CopyRegisterVyToRegisterVx(int x, int y)
        {
            State.Registers.V[x] = State.Registers.V[y];
            return GetNextInstructionAddress();
        }

        private ushort AddValueToRegister(int x, byte value)
        {
            State.Registers.V[x] += value;
            return GetNextInstructionAddress();
        }

        private ushort LoadValueToRegister(int x, byte value)
        {
            State.Registers.V[x] = value;
            return GetNextInstructionAddress();
        }

        private ushort SkipNextOnEqual(int x, byte valueToCompare)
        {
            int offset = State.Registers.V[x] == valueToCompare ? InstructionSize * 2 : InstructionSize;
            return GetInstructionAddress(offset);
        }

        private ushort SkipNextOnNotEqual(int x, byte valueToCompare)
        {
            int offset = State.Registers.V[x] != valueToCompare ? InstructionSize * 2 : InstructionSize;
            return GetInstructionAddress(offset);
        }

        private ushort SkipNextOnRegistersEqual(int x, int y)
        {
            int offset = State.Registers.V[x] == State.Registers.V[y] ? InstructionSize * 2 : InstructionSize;
            return GetInstructionAddress(offset);
        }

        private ushort SkipNextOnRegistersNotEqual(int x, int y)
        {
            int offset = State.Registers.V[x] != State.Registers.V[y] ? InstructionSize * 2 : InstructionSize;
            return GetInstructionAddress(offset);
        }

        private ushort LoadAddressToIndexRegister(ushort address)
        {
            State.Registers.I = address;
            return GetNextInstructionAddress();
        }

        private ushort SetRandomValueWithMaskOnVx(int x, byte mask)
        {
            State.Registers.V[x] = (byte)(_randomGenerator.Generate() & mask);
            return GetNextInstructionAddress();
        }

        private ushort GetInstructionAddress(int offset)
            => (ushort)(State.Registers.PC + offset);

        private ushort GetNextInstructionAddress()
            => GetInstructionAddress(InstructionSize);
    }
}
