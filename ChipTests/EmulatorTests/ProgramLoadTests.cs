using Chip;
using Chip.Exceptions;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class ProgramLoadTests
    {
        [TestMethod]
        public void GivenNonNullProgram_WhenTryingToLoadIt_ThenShouldLoadProgramProperly()
        {
            // Given
            var program = new byte[] { 0x00, 0xE0 };
            var emulator = new Emulator(Substitute.For<ISound>());

            // When
            emulator.LoadProgram(program);

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

            // When
            void action() => new Emulator(Substitute.For<ISound>()).LoadProgram(program);

            // Then
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void GivenTooLargeProgram_WhenTryingToLoadIt_ThenThrowException()
        {
            // Given
            byte[] program = new byte[3585];

            // When
            void action() => new Emulator(Substitute.For<ISound>()).LoadProgram(program);

            // Then
            Assert.ThrowsException<InvalidChipProgramException>(action);
        }
    }
}
