using Chip;
using Chip.Display;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class SubroutineInstructionsTests
    {
        [TestMethod]
        public async Task GivenInstruction00EEAndNotEmptyStack_WhenExecuteInstruction_ThenPopAddressFromStackAndLoadItToProgramCounter()
        {
            // Given
            byte[] instruction = { 0x00, 0xEE };
            ushort expectedResult = 0xABC;

            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };
            await emulator.StartProgramAsync(instruction);
            emulator.State.Stack.Push(expectedResult);

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(0, emulator.State.Stack.Count);
            Assert.AreEqual(expectedResult, emulator.State.Registers.PC);
        }

        [TestMethod]
        public async Task GivenInstruction2NNN_WhenExecuteInstruction_ThenPushNextInstructionAddressToTheStackAndJumpToAddressNNN()
        {
            // Given
            byte[] instruction = { 0x2F, 0xFF };
            ushort addressToJump = 0xFFF;
            ushort nextInstructionAddress = Default.StartAddress + 2;

            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };
            await emulator.StartProgramAsync(instruction);

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(1, emulator.State.Stack.Count);
            Assert.AreEqual(nextInstructionAddress, emulator.State.Stack.Peek());
            Assert.AreEqual(addressToJump, emulator.State.Registers.PC);
        }
    }
}
