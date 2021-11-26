using Chip;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class JumpInstructionsTests
    {
        [TestMethod]
        [DataRow(new byte[] { 0x10, 0x00 }, (ushort)0x000)]
        [DataRow(new byte[] { 0x12, 0x34 }, (ushort)0x234)]
        [DataRow(new byte[] { 0x1F, 0xFF }, (ushort)0xFFF)]
        public void GivenInstruction1NNN_WhenExecuteInstruction_ThenProgramShouldJumpToAddressNNN(byte[] instruction, ushort expectedResult)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(expectedResult, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x30, 0x00 }, 0x0, (byte)0x00)]
        [DataRow(new byte[] { 0x31, 0x11 }, 0x1, (byte)0x11)]
        [DataRow(new byte[] { 0x32, 0x22 }, 0x2, (byte)0x22)]
        [DataRow(new byte[] { 0x33, 0x33 }, 0x3, (byte)0x33)]
        [DataRow(new byte[] { 0x34, 0x44 }, 0x4, (byte)0x44)]
        [DataRow(new byte[] { 0x35, 0x55 }, 0x5, (byte)0x55)]
        [DataRow(new byte[] { 0x36, 0x66 }, 0x6, (byte)0x66)]
        [DataRow(new byte[] { 0x37, 0x77 }, 0x7, (byte)0x77)]
        [DataRow(new byte[] { 0x38, 0x88 }, 0x8, (byte)0x88)]
        [DataRow(new byte[] { 0x39, 0x99 }, 0x9, (byte)0x99)]
        [DataRow(new byte[] { 0x3A, 0xAA }, 0xA, (byte)0xAA)]
        [DataRow(new byte[] { 0x3B, 0xBB }, 0xB, (byte)0xBB)]
        [DataRow(new byte[] { 0x3C, 0xCC }, 0xC, (byte)0xCC)]
        [DataRow(new byte[] { 0x3D, 0xDD }, 0xD, (byte)0xDD)]
        [DataRow(new byte[] { 0x3E, 0xEE }, 0xE, (byte)0xEE)]
        [DataRow(new byte[] { 0x3F, 0xFF }, 0xF, (byte)0xFF)]
        public void GivenInstruction3XNNAndValueOfRegisterVXEqualsToNN_WhenExecuteInstruction_ThenSkipNextInstruction(byte[] instruction, int x, byte value)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = value;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 4, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x30, 0x00 }, 0x0, (byte)0x12)]
        [DataRow(new byte[] { 0x31, 0x11 }, 0x1, (byte)0x23)]
        [DataRow(new byte[] { 0x32, 0x22 }, 0x2, (byte)0x34)]
        [DataRow(new byte[] { 0x33, 0x33 }, 0x3, (byte)0x45)]
        [DataRow(new byte[] { 0x34, 0x44 }, 0x4, (byte)0x56)]
        [DataRow(new byte[] { 0x35, 0x55 }, 0x5, (byte)0x67)]
        [DataRow(new byte[] { 0x36, 0x66 }, 0x6, (byte)0x78)]
        [DataRow(new byte[] { 0x37, 0x77 }, 0x7, (byte)0x89)]
        [DataRow(new byte[] { 0x38, 0x88 }, 0x8, (byte)0x9A)]
        [DataRow(new byte[] { 0x39, 0x99 }, 0x9, (byte)0xAB)]
        [DataRow(new byte[] { 0x3A, 0xAA }, 0xA, (byte)0xBC)]
        [DataRow(new byte[] { 0x3B, 0xBB }, 0xB, (byte)0xCD)]
        [DataRow(new byte[] { 0x3C, 0xCC }, 0xC, (byte)0xDE)]
        [DataRow(new byte[] { 0x3D, 0xDD }, 0xD, (byte)0xDF)]
        [DataRow(new byte[] { 0x3E, 0xEE }, 0xE, (byte)0xF0)]
        [DataRow(new byte[] { 0x3F, 0xFF }, 0xF, (byte)0xF1)]
        public void GivenInstruction3XNNAndValueOfRegisterVXNotEqualsToNN_WhenExecuteInstruction_ThenDoNotSkipNextInstruction(byte[] instruction, int x, byte value)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = value;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 2, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x40, 0x00 }, 0x0, (byte)0x12)]
        [DataRow(new byte[] { 0x41, 0x11 }, 0x1, (byte)0x23)]
        [DataRow(new byte[] { 0x42, 0x22 }, 0x2, (byte)0x34)]
        [DataRow(new byte[] { 0x43, 0x33 }, 0x3, (byte)0x45)]
        [DataRow(new byte[] { 0x44, 0x44 }, 0x4, (byte)0x56)]
        [DataRow(new byte[] { 0x45, 0x55 }, 0x5, (byte)0x67)]
        [DataRow(new byte[] { 0x46, 0x66 }, 0x6, (byte)0x78)]
        [DataRow(new byte[] { 0x47, 0x77 }, 0x7, (byte)0x89)]
        [DataRow(new byte[] { 0x48, 0x88 }, 0x8, (byte)0x9A)]
        [DataRow(new byte[] { 0x49, 0x99 }, 0x9, (byte)0xAB)]
        [DataRow(new byte[] { 0x4A, 0xAA }, 0xA, (byte)0xBC)]
        [DataRow(new byte[] { 0x4B, 0xBB }, 0xB, (byte)0xCD)]
        [DataRow(new byte[] { 0x4C, 0xCC }, 0xC, (byte)0xDE)]
        [DataRow(new byte[] { 0x4D, 0xDD }, 0xD, (byte)0xDF)]
        [DataRow(new byte[] { 0x4E, 0xEE }, 0xE, (byte)0xF0)]
        [DataRow(new byte[] { 0x4F, 0xFF }, 0xF, (byte)0xF1)]
        public void GivenInstruction4XNNAndValueOfRegisterVXNotEqualsToNN_WhenExecuteInstruction_ThenSkipNextInstruction(byte[] instruction, int x, byte value)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = value;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 4, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x40, 0x00 }, 0x0, (byte)0x00)]
        [DataRow(new byte[] { 0x41, 0x11 }, 0x1, (byte)0x11)]
        [DataRow(new byte[] { 0x42, 0x22 }, 0x2, (byte)0x22)]
        [DataRow(new byte[] { 0x43, 0x33 }, 0x3, (byte)0x33)]
        [DataRow(new byte[] { 0x44, 0x44 }, 0x4, (byte)0x44)]
        [DataRow(new byte[] { 0x45, 0x55 }, 0x5, (byte)0x55)]
        [DataRow(new byte[] { 0x46, 0x66 }, 0x6, (byte)0x66)]
        [DataRow(new byte[] { 0x47, 0x77 }, 0x7, (byte)0x77)]
        [DataRow(new byte[] { 0x48, 0x88 }, 0x8, (byte)0x88)]
        [DataRow(new byte[] { 0x49, 0x99 }, 0x9, (byte)0x99)]
        [DataRow(new byte[] { 0x4A, 0xAA }, 0xA, (byte)0xAA)]
        [DataRow(new byte[] { 0x4B, 0xBB }, 0xB, (byte)0xBB)]
        [DataRow(new byte[] { 0x4C, 0xCC }, 0xC, (byte)0xCC)]
        [DataRow(new byte[] { 0x4D, 0xDD }, 0xD, (byte)0xDD)]
        [DataRow(new byte[] { 0x4E, 0xEE }, 0xE, (byte)0xEE)]
        [DataRow(new byte[] { 0x4F, 0xFF }, 0xF, (byte)0xFF)]
        public void GivenInstruction4XNNAndValueOfRegisterVXEqualsToNN_WhenExecuteInstruction_ThenDoNotSkipNextInstruction(byte[] instruction, int x, byte value)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = value;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 2, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x50, 0xF0 }, 0x0, 0xF, (byte)0x00, (byte)0x00)]
        [DataRow(new byte[] { 0x51, 0xE0 }, 0x1, 0xE, (byte)0x11, (byte)0x11)]
        [DataRow(new byte[] { 0x52, 0xD0 }, 0x2, 0xD, (byte)0x22, (byte)0x22)]
        [DataRow(new byte[] { 0x53, 0xC0 }, 0x3, 0xC, (byte)0x33, (byte)0x33)]
        [DataRow(new byte[] { 0x54, 0xB0 }, 0x4, 0xB, (byte)0x44, (byte)0x44)]
        [DataRow(new byte[] { 0x55, 0xA0 }, 0x5, 0xA, (byte)0x55, (byte)0x55)]
        [DataRow(new byte[] { 0x56, 0x90 }, 0x6, 0x9, (byte)0x66, (byte)0x66)]
        [DataRow(new byte[] { 0x57, 0x80 }, 0x7, 0x8, (byte)0x77, (byte)0x77)]
        [DataRow(new byte[] { 0x58, 0x70 }, 0x8, 0x7, (byte)0x88, (byte)0x88)]
        [DataRow(new byte[] { 0x59, 0x60 }, 0x9, 0x6, (byte)0x99, (byte)0x99)]
        [DataRow(new byte[] { 0x5A, 0x50 }, 0xA, 0x5, (byte)0xAA, (byte)0xAA)]
        [DataRow(new byte[] { 0x5B, 0x40 }, 0xB, 0x4, (byte)0xBB, (byte)0xBB)]
        [DataRow(new byte[] { 0x5C, 0x30 }, 0xC, 0x3, (byte)0xCC, (byte)0xCC)]
        [DataRow(new byte[] { 0x5D, 0x20 }, 0xD, 0x2, (byte)0xDD, (byte)0xDD)]
        [DataRow(new byte[] { 0x5E, 0x10 }, 0xE, 0x1, (byte)0xEE, (byte)0xEE)]
        [DataRow(new byte[] { 0x5F, 0x00 }, 0xF, 0x0, (byte)0xFF, (byte)0xFF)]
        public void GivenInstruction5XY0AndValueOfRegisterVXEqualsToValueOfRegisterVY_WhenExecuteInstruction_ThenSkipNextInstruction(byte[] instruction, int x, int y, byte vxValue, byte vyValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = vxValue;
            emulator.State.Registers.V[y] = vyValue;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 4, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x50, 0xF0 }, 0x0, 0xF, (byte)0x00, (byte)0xFF)]
        [DataRow(new byte[] { 0x51, 0xE0 }, 0x1, 0xE, (byte)0x11, (byte)0xEE)]
        [DataRow(new byte[] { 0x52, 0xD0 }, 0x2, 0xD, (byte)0x22, (byte)0xDD)]
        [DataRow(new byte[] { 0x53, 0xC0 }, 0x3, 0xC, (byte)0x33, (byte)0xCC)]
        [DataRow(new byte[] { 0x54, 0xB0 }, 0x4, 0xB, (byte)0x44, (byte)0xBB)]
        [DataRow(new byte[] { 0x55, 0xA0 }, 0x5, 0xA, (byte)0x55, (byte)0xAA)]
        [DataRow(new byte[] { 0x56, 0x90 }, 0x6, 0x9, (byte)0x66, (byte)0x99)]
        [DataRow(new byte[] { 0x57, 0x80 }, 0x7, 0x8, (byte)0x77, (byte)0x88)]
        [DataRow(new byte[] { 0x58, 0x70 }, 0x8, 0x7, (byte)0x88, (byte)0x77)]
        [DataRow(new byte[] { 0x59, 0x60 }, 0x9, 0x6, (byte)0x99, (byte)0x66)]
        [DataRow(new byte[] { 0x5A, 0x50 }, 0xA, 0x5, (byte)0xAA, (byte)0x55)]
        [DataRow(new byte[] { 0x5B, 0x40 }, 0xB, 0x4, (byte)0xBB, (byte)0x44)]
        [DataRow(new byte[] { 0x5C, 0x30 }, 0xC, 0x3, (byte)0xCC, (byte)0x33)]
        [DataRow(new byte[] { 0x5D, 0x20 }, 0xD, 0x2, (byte)0xDD, (byte)0x22)]
        [DataRow(new byte[] { 0x5E, 0x10 }, 0xE, 0x1, (byte)0xEE, (byte)0x11)]
        [DataRow(new byte[] { 0x5F, 0x00 }, 0xF, 0x0, (byte)0xFF, (byte)0x00)]
        public void GivenInstruction5XY0AndValueOfRegisterVXNotEqualsToValueOfRegisterVY_WhenExecuteInstruction_ThenDoNotSkipNextInstruction(byte[] instruction, int x, int y, byte vxValue, byte vyValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = vxValue;
            emulator.State.Registers.V[y] = vyValue;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 2, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x90, 0xF0 }, 0x0, 0xF, (byte)0x00, (byte)0xFF)]
        [DataRow(new byte[] { 0x91, 0xE0 }, 0x1, 0xE, (byte)0x11, (byte)0xEE)]
        [DataRow(new byte[] { 0x92, 0xD0 }, 0x2, 0xD, (byte)0x22, (byte)0xDD)]
        [DataRow(new byte[] { 0x93, 0xC0 }, 0x3, 0xC, (byte)0x33, (byte)0xCC)]
        [DataRow(new byte[] { 0x94, 0xB0 }, 0x4, 0xB, (byte)0x44, (byte)0xBB)]
        [DataRow(new byte[] { 0x95, 0xA0 }, 0x5, 0xA, (byte)0x55, (byte)0xAA)]
        [DataRow(new byte[] { 0x96, 0x90 }, 0x6, 0x9, (byte)0x66, (byte)0x99)]
        [DataRow(new byte[] { 0x97, 0x80 }, 0x7, 0x8, (byte)0x77, (byte)0x88)]
        [DataRow(new byte[] { 0x98, 0x70 }, 0x8, 0x7, (byte)0x88, (byte)0x77)]
        [DataRow(new byte[] { 0x99, 0x60 }, 0x9, 0x6, (byte)0x99, (byte)0x66)]
        [DataRow(new byte[] { 0x9A, 0x50 }, 0xA, 0x5, (byte)0xAA, (byte)0x55)]
        [DataRow(new byte[] { 0x9B, 0x40 }, 0xB, 0x4, (byte)0xBB, (byte)0x44)]
        [DataRow(new byte[] { 0x9C, 0x30 }, 0xC, 0x3, (byte)0xCC, (byte)0x33)]
        [DataRow(new byte[] { 0x9D, 0x20 }, 0xD, 0x2, (byte)0xDD, (byte)0x22)]
        [DataRow(new byte[] { 0x9E, 0x10 }, 0xE, 0x1, (byte)0xEE, (byte)0x11)]
        [DataRow(new byte[] { 0x9F, 0x00 }, 0xF, 0x0, (byte)0xFF, (byte)0x00)]
        public void GivenInstruction9XY0AndValueOfRegisterVXNotEqualsToValueOfRegisterVY_WhenExecuteInstruction_ThenSkipNextInstruction(byte[] instruction, int x, int y, byte vxValue, byte vyValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = vxValue;
            emulator.State.Registers.V[y] = vyValue;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 4, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x90, 0xF0 }, 0x0, 0xF, (byte)0x00, (byte)0x00)]
        [DataRow(new byte[] { 0x91, 0xE0 }, 0x1, 0xE, (byte)0x11, (byte)0x11)]
        [DataRow(new byte[] { 0x92, 0xD0 }, 0x2, 0xD, (byte)0x22, (byte)0x22)]
        [DataRow(new byte[] { 0x93, 0xC0 }, 0x3, 0xC, (byte)0x33, (byte)0x33)]
        [DataRow(new byte[] { 0x94, 0xB0 }, 0x4, 0xB, (byte)0x44, (byte)0x44)]
        [DataRow(new byte[] { 0x95, 0xA0 }, 0x5, 0xA, (byte)0x55, (byte)0x55)]
        [DataRow(new byte[] { 0x96, 0x90 }, 0x6, 0x9, (byte)0x66, (byte)0x66)]
        [DataRow(new byte[] { 0x97, 0x80 }, 0x7, 0x8, (byte)0x77, (byte)0x77)]
        [DataRow(new byte[] { 0x98, 0x70 }, 0x8, 0x7, (byte)0x88, (byte)0x88)]
        [DataRow(new byte[] { 0x99, 0x60 }, 0x9, 0x6, (byte)0x99, (byte)0x99)]
        [DataRow(new byte[] { 0x9A, 0x50 }, 0xA, 0x5, (byte)0xAA, (byte)0xAA)]
        [DataRow(new byte[] { 0x9B, 0x40 }, 0xB, 0x4, (byte)0xBB, (byte)0xBB)]
        [DataRow(new byte[] { 0x9C, 0x30 }, 0xC, 0x3, (byte)0xCC, (byte)0xCC)]
        [DataRow(new byte[] { 0x9D, 0x20 }, 0xD, 0x2, (byte)0xDD, (byte)0xDD)]
        [DataRow(new byte[] { 0x9E, 0x10 }, 0xE, 0x1, (byte)0xEE, (byte)0xEE)]
        [DataRow(new byte[] { 0x9F, 0x00 }, 0xF, 0x0, (byte)0xFF, (byte)0xFF)]
        public void GivenInstruction9XY0AndValueOfRegisterVXEqualsToValueOfRegisterVY_WhenExecuteInstruction_ThenDoNotSkipNextInstruction(byte[] instruction, int x, int y, byte vxValue, byte vyValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = vxValue;
            emulator.State.Registers.V[y] = vyValue;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(Default.StartAddress + 2, emulator.State.Registers.pc);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xB0, 0x00 }, (byte)0x00, (ushort)0x0000)]
        [DataRow(new byte[] { 0xBA, 0xBC }, (byte)0x00, (ushort)0x0ABC)]
        [DataRow(new byte[] { 0xB0, 0x00 }, (byte)0xEF, (ushort)0x00EF)]
        [DataRow(new byte[] { 0xB1, 0x23 }, (byte)0x45, (ushort)0x0168)]
        [DataRow(new byte[] { 0xBF, 0xFF }, (byte)0xFF, (ushort)0x10FE)]
        public void GivenInstructionBNNN_WhenExecuteInstruction_ThenShouldJumpToAddressWhichIsSumOfNNNAndValueOfRegisterV0(byte[] instruction, byte initialRegisterValue, ushort expectedResult)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[0] = initialRegisterValue;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(expectedResult, emulator.State.Registers.pc);
        }
    }
}
