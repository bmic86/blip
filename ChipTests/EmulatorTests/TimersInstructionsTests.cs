using Chip;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class TimersInstructionsTests
    {
        [TestMethod]
        [DataRow(new byte[] { 0xF0, 0x07 }, 0x0, (byte)1)]
        [DataRow(new byte[] { 0xF1, 0x07 }, 0x1, (byte)2)]
        [DataRow(new byte[] { 0xF2, 0x07 }, 0x2, (byte)4)]
        [DataRow(new byte[] { 0xF3, 0x07 }, 0x3, (byte)8)]
        [DataRow(new byte[] { 0xF4, 0x07 }, 0x4, (byte)16)]
        [DataRow(new byte[] { 0xF5, 0x07 }, 0x5, (byte)32)]
        [DataRow(new byte[] { 0xF6, 0x07 }, 0x6, (byte)64)]
        [DataRow(new byte[] { 0xF7, 0x07 }, 0x7, (byte)128)]
        [DataRow(new byte[] { 0xF8, 0x07 }, 0x8, (byte)255)]
        [DataRow(new byte[] { 0xF9, 0x07 }, 0x9, (byte)52)]
        [DataRow(new byte[] { 0xFA, 0x07 }, 0xA, (byte)111)]
        [DataRow(new byte[] { 0xFB, 0x07 }, 0xB, (byte)200)]
        [DataRow(new byte[] { 0xFC, 0x07 }, 0xC, (byte)42)]
        [DataRow(new byte[] { 0xFD, 0x07 }, 0xD, (byte)80)]
        [DataRow(new byte[] { 0xFE, 0x07 }, 0xE, (byte)90)]
        [DataRow(new byte[] { 0xFF, 0x07 }, 0xF, (byte)25)]
        public async Task GivenInstructionFX07_WhenExecuteInstruction_ThenSetRegisterVXToValueOfDelayTimer(byte[] instruction, int x, byte expectedValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);

            emulator.DelayTimer.Value = expectedValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(emulator.DelayTimer.Value, emulator.State.Registers.V[x]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xF0, 0x15 }, 0x0, (byte)1)]
        [DataRow(new byte[] { 0xF1, 0x15 }, 0x1, (byte)2)]
        [DataRow(new byte[] { 0xF2, 0x15 }, 0x2, (byte)4)]
        [DataRow(new byte[] { 0xF3, 0x15 }, 0x3, (byte)8)]
        [DataRow(new byte[] { 0xF4, 0x15 }, 0x4, (byte)16)]
        [DataRow(new byte[] { 0xF5, 0x15 }, 0x5, (byte)32)]
        [DataRow(new byte[] { 0xF6, 0x15 }, 0x6, (byte)64)]
        [DataRow(new byte[] { 0xF7, 0x15 }, 0x7, (byte)128)]
        [DataRow(new byte[] { 0xF8, 0x15 }, 0x8, (byte)255)]
        [DataRow(new byte[] { 0xF9, 0x15 }, 0x9, (byte)52)]
        [DataRow(new byte[] { 0xFA, 0x15 }, 0xA, (byte)123)]
        [DataRow(new byte[] { 0xFB, 0x15 }, 0xB, (byte)200)]
        [DataRow(new byte[] { 0xFC, 0x15 }, 0xC, (byte)42)]
        [DataRow(new byte[] { 0xFD, 0x15 }, 0xD, (byte)80)]
        [DataRow(new byte[] { 0xFE, 0x15 }, 0xE, (byte)76)]
        [DataRow(new byte[] { 0xFF, 0x15 }, 0xF, (byte)25)]
        public async Task GivenInstructionFX15_WhenExecuteInstruction_ThenSetDelayTimerToValueOfRegisterVX(byte[] instruction, int x, byte expectedValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);

            emulator.State.Registers.V[x] = expectedValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(emulator.DelayTimer.Value, emulator.State.Registers.V[x]);
        }
    }
}
