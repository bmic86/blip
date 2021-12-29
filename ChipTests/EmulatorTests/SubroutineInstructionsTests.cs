using Chip;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class SubroutineInstructionsTests
    {
        [TestMethod]
        public void GivenInstruction00EEAndNotEmptyStack_WhenExecuteInstruction_ThenPopAddressFromStackAndLoadItToProgramCounter()
        {
            // Given
            byte[] instruction = { 0x00, 0xEE };
            ushort expectedResult = 0xABC;

            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);
            emulator.State.Stack.Push(expectedResult);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(0, emulator.State.Stack.Count);
            Assert.AreEqual(expectedResult, emulator.State.Registers.PC);
        }

        [TestMethod]
        public void GivenInstruction2NNN_WhenExecuteInstruction_ThenPushNextInstructionAddressToTheStackAndJumpToAddressNNN()
        {
            // Given
            byte[] instruction = { 0x2F, 0xFF };
            ushort addressToJump = 0xFFF;
            ushort nextInstructionAddress = Default.StartAddress + 2;

            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            Assert.AreEqual(1, emulator.State.Stack.Count);
            Assert.AreEqual(nextInstructionAddress, emulator.State.Stack.Peek());
            Assert.AreEqual(addressToJump, emulator.State.Registers.PC);
        }
    }
}
