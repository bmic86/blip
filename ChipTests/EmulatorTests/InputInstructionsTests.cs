using Chip;
using Chip.Input;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class InputInstructionsTests
    {
        [TestMethod]
        [DataRow(new byte[] { 0xE0, 0x9E }, 0x0, (byte)0xF, Key.F)]
        [DataRow(new byte[] { 0xE1, 0x9E }, 0x1, (byte)0xE, Key.E)]
        [DataRow(new byte[] { 0xE2, 0x9E }, 0x2, (byte)0xD, Key.D)]
        [DataRow(new byte[] { 0xE3, 0x9E }, 0x3, (byte)0xC, Key.C)]
        [DataRow(new byte[] { 0xE4, 0x9E }, 0x4, (byte)0xB, Key.B)]
        [DataRow(new byte[] { 0xE5, 0x9E }, 0x5, (byte)0xA, Key.A)]
        [DataRow(new byte[] { 0xE6, 0x9E }, 0x6, (byte)0x9, Key.Num9)]
        [DataRow(new byte[] { 0xE7, 0x9E }, 0x7, (byte)0x8, Key.Num8)]
        [DataRow(new byte[] { 0xE8, 0x9E }, 0x8, (byte)0x7, Key.Num7)]
        [DataRow(new byte[] { 0xE9, 0x9E }, 0x9, (byte)0x6, Key.Num6)]
        [DataRow(new byte[] { 0xEA, 0x9E }, 0xA, (byte)0x5, Key.Num5)]
        [DataRow(new byte[] { 0xEB, 0x9E }, 0xB, (byte)0x4, Key.Num4)]
        [DataRow(new byte[] { 0xEC, 0x9E }, 0xC, (byte)0x3, Key.Num3)]
        [DataRow(new byte[] { 0xED, 0x9E }, 0xD, (byte)0x2, Key.Num2)]
        [DataRow(new byte[] { 0xEE, 0x9E }, 0xE, (byte)0x1, Key.Num1)]
        [DataRow(new byte[] { 0xEF, 0x9E }, 0xF, (byte)0x0, Key.Num0)]
        [DataRow(new byte[] { 0xE8, 0x9E }, 0x8, (byte)0xAF, Key.F)]
        public void GivenInstructionEX9EAndLeastSignificantDigitOfVXEqualToPressedKey_WhenExecuteInstruction_ThenSkipNextInstruction(byte[] instruction, int x, byte vxValue, Key key)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = vxValue;
            emulator.Keypad.KeyDown(key);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 4, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xE0, 0x9E }, 0x0, (byte)0x0, Key.F)]
        [DataRow(new byte[] { 0xE1, 0x9E }, 0x1, (byte)0x1, Key.E)]
        [DataRow(new byte[] { 0xE2, 0x9E }, 0x2, (byte)0x2, Key.D)]
        [DataRow(new byte[] { 0xE3, 0x9E }, 0x3, (byte)0x3, Key.C)]
        [DataRow(new byte[] { 0xE4, 0x9E }, 0x4, (byte)0x4, Key.B)]
        [DataRow(new byte[] { 0xE5, 0x9E }, 0x5, (byte)0x5, Key.A)]
        [DataRow(new byte[] { 0xE6, 0x9E }, 0x6, (byte)0x6, Key.Num9)]
        [DataRow(new byte[] { 0xE7, 0x9E }, 0x7, (byte)0x7, Key.Num8)]
        [DataRow(new byte[] { 0xE8, 0x9E }, 0x8, (byte)0x8, Key.Num7)]
        [DataRow(new byte[] { 0xE9, 0x9E }, 0x9, (byte)0x9, Key.Num6)]
        [DataRow(new byte[] { 0xEA, 0x9E }, 0xA, (byte)0xA, Key.Num5)]
        [DataRow(new byte[] { 0xEB, 0x9E }, 0xB, (byte)0xB, Key.Num4)]
        [DataRow(new byte[] { 0xEC, 0x9E }, 0xC, (byte)0xC, Key.Num3)]
        [DataRow(new byte[] { 0xED, 0x9E }, 0xD, (byte)0xD, Key.Num2)]
        [DataRow(new byte[] { 0xEE, 0x9E }, 0xE, (byte)0xE, Key.Num1)]
        [DataRow(new byte[] { 0xEF, 0x9E }, 0xF, (byte)0xF, Key.Num0)]
        [DataRow(new byte[] { 0xE8, 0x9E }, 0x8, (byte)0xAF, Key.Num0)]
        public void GivenInstructionEX9EAndLeastSignificantDigitOfVXNotEqualToPressedKey_WhenExecuteInstruction_ThenDoNotSkipNextInstruction(byte[] instruction, int x, byte vxValue, Key key)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = vxValue;
            emulator.Keypad.KeyDown(key);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 2, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xE0, 0xA1 }, 0x0, (byte)0x0, Key.F)]
        [DataRow(new byte[] { 0xE1, 0xA1 }, 0x1, (byte)0x1, Key.E)]
        [DataRow(new byte[] { 0xE2, 0xA1 }, 0x2, (byte)0x2, Key.D)]
        [DataRow(new byte[] { 0xE3, 0xA1 }, 0x3, (byte)0x3, Key.C)]
        [DataRow(new byte[] { 0xE4, 0xA1 }, 0x4, (byte)0x4, Key.B)]
        [DataRow(new byte[] { 0xE5, 0xA1 }, 0x5, (byte)0x5, Key.A)]
        [DataRow(new byte[] { 0xE6, 0xA1 }, 0x6, (byte)0x6, Key.Num9)]
        [DataRow(new byte[] { 0xE7, 0xA1 }, 0x7, (byte)0x7, Key.Num8)]
        [DataRow(new byte[] { 0xE8, 0xA1 }, 0x8, (byte)0x8, Key.Num7)]
        [DataRow(new byte[] { 0xE9, 0xA1 }, 0x9, (byte)0x9, Key.Num6)]
        [DataRow(new byte[] { 0xEA, 0xA1 }, 0xA, (byte)0xA, Key.Num5)]
        [DataRow(new byte[] { 0xEB, 0xA1 }, 0xB, (byte)0xB, Key.Num4)]
        [DataRow(new byte[] { 0xEC, 0xA1 }, 0xC, (byte)0xC, Key.Num3)]
        [DataRow(new byte[] { 0xED, 0xA1 }, 0xD, (byte)0xD, Key.Num2)]
        [DataRow(new byte[] { 0xEE, 0xA1 }, 0xE, (byte)0xE, Key.Num1)]
        [DataRow(new byte[] { 0xEF, 0xA1 }, 0xF, (byte)0xF, Key.Num0)]
        [DataRow(new byte[] { 0xE8, 0xA1 }, 0x8, (byte)0xAF, Key.Num0)]
        public void GivenInstructionEXA1AndLeastSignificantDigitOfVXNotEqualToPressedKey_WhenExecuteInstruction_ThenSkipNextInstruction(byte[] instruction, int x, byte vxValue, Key key)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = vxValue;
            emulator.Keypad.KeyDown(key);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 4, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xE0, 0xA1 }, 0x0, (byte)0xF, Key.F)]
        [DataRow(new byte[] { 0xE1, 0xA1 }, 0x1, (byte)0xE, Key.E)]
        [DataRow(new byte[] { 0xE2, 0xA1 }, 0x2, (byte)0xD, Key.D)]
        [DataRow(new byte[] { 0xE3, 0xA1 }, 0x3, (byte)0xC, Key.C)]
        [DataRow(new byte[] { 0xE4, 0xA1 }, 0x4, (byte)0xB, Key.B)]
        [DataRow(new byte[] { 0xE5, 0xA1 }, 0x5, (byte)0xA, Key.A)]
        [DataRow(new byte[] { 0xE6, 0xA1 }, 0x6, (byte)0x9, Key.Num9)]
        [DataRow(new byte[] { 0xE7, 0xA1 }, 0x7, (byte)0x8, Key.Num8)]
        [DataRow(new byte[] { 0xE8, 0xA1 }, 0x8, (byte)0x7, Key.Num7)]
        [DataRow(new byte[] { 0xE9, 0xA1 }, 0x9, (byte)0x6, Key.Num6)]
        [DataRow(new byte[] { 0xEA, 0xA1 }, 0xA, (byte)0x5, Key.Num5)]
        [DataRow(new byte[] { 0xEB, 0xA1 }, 0xB, (byte)0x4, Key.Num4)]
        [DataRow(new byte[] { 0xEC, 0xA1 }, 0xC, (byte)0x3, Key.Num3)]
        [DataRow(new byte[] { 0xED, 0xA1 }, 0xD, (byte)0x2, Key.Num2)]
        [DataRow(new byte[] { 0xEE, 0xA1 }, 0xE, (byte)0x1, Key.Num1)]
        [DataRow(new byte[] { 0xEF, 0xA1 }, 0xF, (byte)0x0, Key.Num0)]
        [DataRow(new byte[] { 0xE8, 0xA1 }, 0x8, (byte)0xAF, Key.F)]
        public void GivenInstructionEXA1AndLeastSignificantDigitOfVXEqualToPressedKey_WhenExecuteInstruction_ThenDoNotSkipNextInstruction(byte[] instruction, int x, byte vxValue, Key key)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = vxValue;
            emulator.Keypad.KeyDown(key);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 2, emulator.State.Registers.pc);
        }
    }
}
