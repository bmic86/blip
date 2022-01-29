using Chip;
using Chip.Display;
using Chip.Exceptions;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class ProgramLoadTests
    {
        [TestMethod]
        public async Task GivenNonNullProgram_WhenTryingToLoadIt_ThenShouldLoadProgramProperly()
        {
            // Given
            var program = new byte[] { 0x00, 0xE0 };
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            // When
            await emulator.StartProgramAsync(program);

            // Then
            CollectionAssert.AreEquivalent(
                program,
                new ArraySegment<byte>(emulator.State.Memory, Default.StartAddress, program.Length).ToArray());
        }

        [TestMethod]
        public void GivenNullProgram_WhenTryingToLoadIt_ThenThrowException()
        {
            // Given
            byte[] program = null;
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            // When
            async Task action() => await emulator.StartProgramAsync(program);

            // Then
            Assert.ThrowsExceptionAsync<ArgumentNullException>(action);
        }

        [TestMethod]
        public void GivenTooLargeProgram_WhenTryingToLoadIt_ThenThrowException()
        {
            // Given
            byte[] program = new byte[3585];
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            // When
            async Task action() => await emulator.StartProgramAsync(program);

            // Then
            Assert.ThrowsExceptionAsync<InvalidChipProgramException>(action);
        }
    }
}
