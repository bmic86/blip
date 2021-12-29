using Chip;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class AssignmentInstructionsTests
    {
        [TestMethod]
        [DataRow(new byte[] { 0x60, 0x01 }, 0x0, (byte)0x01)]
        [DataRow(new byte[] { 0x61, 0x12 }, 0x1, (byte)0x12)]
        [DataRow(new byte[] { 0x62, 0x23 }, 0x2, (byte)0x23)]
        [DataRow(new byte[] { 0x63, 0x34 }, 0x3, (byte)0x34)]
        [DataRow(new byte[] { 0x64, 0x45 }, 0x4, (byte)0x45)]
        [DataRow(new byte[] { 0x65, 0x56 }, 0x5, (byte)0x56)]
        [DataRow(new byte[] { 0x66, 0x67 }, 0x6, (byte)0x67)]
        [DataRow(new byte[] { 0x67, 0x78 }, 0x7, (byte)0x78)]
        [DataRow(new byte[] { 0x68, 0x89 }, 0x8, (byte)0x89)]
        [DataRow(new byte[] { 0x69, 0x9A }, 0x9, (byte)0x9A)]
        [DataRow(new byte[] { 0x6A, 0xAB }, 0xA, (byte)0xAB)]
        [DataRow(new byte[] { 0x6B, 0xBC }, 0xB, (byte)0xBC)]
        [DataRow(new byte[] { 0x6C, 0xCD }, 0xC, (byte)0xCD)]
        [DataRow(new byte[] { 0x6D, 0xDE }, 0xD, (byte)0xDE)]
        [DataRow(new byte[] { 0x6E, 0xEF }, 0xE, (byte)0xEF)]
        [DataRow(new byte[] { 0x6F, 0xFF }, 0xF, (byte)0xFF)]
        public void GivenInstruction6XNN_WhenExecuteInstruction_ThenLoadNNToRegisterVX(byte[] instruction, int x, byte expectedValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(expectedValue, emulator.State.Registers.V[x]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0xF0 }, 0x0, 0xF, (byte)0x01)]
        [DataRow(new byte[] { 0x81, 0xE0 }, 0x1, 0xE, (byte)0x02)]
        [DataRow(new byte[] { 0x82, 0xD0 }, 0x2, 0xD, (byte)0x03)]
        [DataRow(new byte[] { 0x83, 0xC0 }, 0x3, 0xC, (byte)0x04)]
        [DataRow(new byte[] { 0x84, 0xB0 }, 0x4, 0xB, (byte)0x05)]
        [DataRow(new byte[] { 0x85, 0xA0 }, 0x5, 0xA, (byte)0x02)]
        [DataRow(new byte[] { 0x86, 0x90 }, 0x6, 0x9, (byte)0xA0)]
        [DataRow(new byte[] { 0x87, 0x80 }, 0x7, 0x8, (byte)0xB0)]
        [DataRow(new byte[] { 0x88, 0x70 }, 0x8, 0x7, (byte)0xC0)]
        [DataRow(new byte[] { 0x89, 0x60 }, 0x9, 0x6, (byte)0xD1)]
        [DataRow(new byte[] { 0x8A, 0x50 }, 0xA, 0x5, (byte)0x1F)]
        [DataRow(new byte[] { 0x8B, 0x40 }, 0xB, 0x4, (byte)0x25)]
        [DataRow(new byte[] { 0x8C, 0x30 }, 0xC, 0x3, (byte)0xDD)]
        [DataRow(new byte[] { 0x8D, 0x20 }, 0xD, 0x2, (byte)0x1D)]
        [DataRow(new byte[] { 0x8E, 0x10 }, 0xE, 0x1, (byte)0x0F)]
        [DataRow(new byte[] { 0x8F, 0x00 }, 0xF, 0x0, (byte)0xFF)]
        public void GivenInstruction8XY0_WhenExecuteInstruction_ThenLoadValueOfRegisterVYIntoRegisterVX(byte[] instruction, int x, int y, byte initialRegisterValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[y] = initialRegisterValue;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(emulator.State.Registers.V[y], emulator.State.Registers.V[x]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xA0, 0x00 }, (ushort)0x000)]
        [DataRow(new byte[] { 0xA1, 0x23 }, (ushort)0x123)]
        [DataRow(new byte[] { 0xAF, 0xFF }, (ushort)0xFFF)]
        public void GivenInstructionANNN_WhenExecuteInstruction_ThenStoreAddressNNNInIndexRegister(byte[] instruction, ushort expectedResult)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(expectedResult, emulator.State.Registers.I);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xF0, 0x29 }, (byte)0x0, (byte)0xF, (ushort)75)]
        [DataRow(new byte[] { 0xF1, 0x29 }, (byte)0x1, (byte)0xE, (ushort)70)]
        [DataRow(new byte[] { 0xF2, 0x29 }, (byte)0x2, (byte)0xD, (ushort)65)]
        [DataRow(new byte[] { 0xF3, 0x29 }, (byte)0x3, (byte)0xC, (ushort)60)]
        [DataRow(new byte[] { 0xF4, 0x29 }, (byte)0x4, (byte)0xB, (ushort)55)]
        [DataRow(new byte[] { 0xF5, 0x29 }, (byte)0x5, (byte)0xA, (ushort)50)]
        [DataRow(new byte[] { 0xF6, 0x29 }, (byte)0x6, (byte)0x9, (ushort)45)]
        [DataRow(new byte[] { 0xF7, 0x29 }, (byte)0x7, (byte)0x8, (ushort)40)]
        [DataRow(new byte[] { 0xF8, 0x29 }, (byte)0x8, (byte)0x7, (ushort)35)]
        [DataRow(new byte[] { 0xF9, 0x29 }, (byte)0x9, (byte)0x6, (ushort)30)]
        [DataRow(new byte[] { 0xFA, 0x29 }, (byte)0xA, (byte)0x5, (ushort)25)]
        [DataRow(new byte[] { 0xFB, 0x29 }, (byte)0xB, (byte)0x4, (ushort)20)]
        [DataRow(new byte[] { 0xFC, 0x29 }, (byte)0xC, (byte)0x3, (ushort)15)]
        [DataRow(new byte[] { 0xFD, 0x29 }, (byte)0xD, (byte)0x2, (ushort)10)]
        [DataRow(new byte[] { 0xFE, 0x29 }, (byte)0xE, (byte)0x1, (ushort)5)]
        [DataRow(new byte[] { 0xFF, 0x29 }, (byte)0xF, (byte)0x0, (ushort)0)]
        [DataRow(new byte[] { 0xFF, 0x29 }, (byte)0xF, (byte)0x18, (ushort)40)]
        public void GivenInstructionFX29_WhenExecuteInstruction_ThenSetIndexRegisterToCharacterSpriteAddressOfLowestSignificantDigitInVXValue(byte[] instruction, byte x, byte initialVxValue, ushort expectedValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = initialVxValue;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(expectedValue, emulator.State.Registers.I);
        }
    }
}
