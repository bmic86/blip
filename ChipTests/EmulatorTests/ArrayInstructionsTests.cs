using Chip;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class ArrayInstructionsTests
    {
        [TestMethod]
        [DataRow(new byte[] { 0xF0, 0x65 }, (byte)0x0, (ushort)0x202)]
        [DataRow(new byte[] { 0xF9, 0x65 }, (byte)0x9, (ushort)0xABC)]
        [DataRow(new byte[] { 0xFF, 0x65 }, (byte)0xF, (ushort)0xFF0)]
        public void GivenInstructionFX65_WhenExecuteInstruction_ThenLoadValuesOfRegistersV0ToVXFromMemoryAndUpdateIndexRegister(byte[] instruction, byte x, ushort initialIndexValue)
        {
            // Given
            int valuesCount = x + 1;
            byte[] expectedRegisterValues = Enumerable.Range(123, valuesCount).Select(value => (byte)value).ToArray();

            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);

            emulator.State.Registers.I = initialIndexValue;
            for (int i = 0; i <= x; ++i)
            {
                emulator.State.Memory[initialIndexValue + i] = expectedRegisterValues[i];
            }

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            CollectionAssert.AreEqual(expectedRegisterValues, new ArraySegment<byte>(emulator.State.Registers.V, 0, valuesCount).ToArray());
            Assert.AreEqual(initialIndexValue + valuesCount, emulator.State.Registers.I);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xF0, 0x55 }, (byte)0x0, (ushort)0x202)]
        [DataRow(new byte[] { 0xF9, 0x55 }, (byte)0x9, (ushort)0xABC)]
        [DataRow(new byte[] { 0xFF, 0x55 }, (byte)0xF, (ushort)0xFF0)]
        public void GivenInstructionFX55_WhenExecuteInstruction_ThenStoreValuesOfRegistersV0ToVXInMemoryAndUpdateIndexRegister(byte[] instruction, byte x, ushort initialIndexValue)
        {
            // Given
            int valuesCount = x + 1;
            byte[] expectedRegisterValues = Enumerable.Range(123, valuesCount).Select(value => (byte)value).ToArray();

            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);

            emulator.State.Registers.I = initialIndexValue;
            for (int i = 0; i <= x; ++i)
            {
                emulator.State.Registers.V[i] = expectedRegisterValues[i];
            }

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            CollectionAssert.AreEqual(expectedRegisterValues, new ArraySegment<byte>(emulator.State.Memory, initialIndexValue, valuesCount).ToArray());
            Assert.AreEqual(initialIndexValue + valuesCount, emulator.State.Registers.I);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xF0, 0x33 }, (byte)0x0, (byte)123, new byte[] { 1, 2, 3 })]
        [DataRow(new byte[] { 0xF1, 0x33 }, (byte)0x1, (byte)254, new byte[] { 2, 5, 4 })]
        [DataRow(new byte[] { 0xF2, 0x33 }, (byte)0x2, (byte)10, new byte[] { 0, 1, 0 })]
        [DataRow(new byte[] { 0xF3, 0x33 }, (byte)0x3, (byte)1, new byte[] { 0, 0, 1 })]
        [DataRow(new byte[] { 0xF4, 0x33 }, (byte)0x4, (byte)32, new byte[] { 0, 3, 2 })]
        [DataRow(new byte[] { 0xF5, 0x33 }, (byte)0x5, (byte)140, new byte[] { 1, 4, 0 })]
        [DataRow(new byte[] { 0xF6, 0x33 }, (byte)0x6, (byte)128, new byte[] { 1, 2, 8 })]
        [DataRow(new byte[] { 0xF7, 0x33 }, (byte)0x7, (byte)255, new byte[] { 2, 5, 5 })]
        [DataRow(new byte[] { 0xF8, 0x33 }, (byte)0x8, (byte)65, new byte[] { 0, 6, 5 })]
        [DataRow(new byte[] { 0xF9, 0x33 }, (byte)0x9, (byte)16, new byte[] { 0, 1, 6 })]
        [DataRow(new byte[] { 0xFA, 0x33 }, (byte)0xA, (byte)105, new byte[] { 1, 0, 5 })]
        [DataRow(new byte[] { 0xFB, 0x33 }, (byte)0xB, (byte)6, new byte[] { 0, 0, 6 })]
        [DataRow(new byte[] { 0xFC, 0x33 }, (byte)0xC, (byte)59, new byte[] { 0, 5, 9 })]
        [DataRow(new byte[] { 0xFD, 0x33 }, (byte)0xD, (byte)74, new byte[] { 0, 7, 4 })]
        [DataRow(new byte[] { 0xFE, 0x33 }, (byte)0xE, (byte)15, new byte[] { 0, 1, 5 })]
        [DataRow(new byte[] { 0xFF, 0x33 }, (byte)0xF, (byte)0, new byte[] { 0, 0, 0 })]
        public void GivenInstructionFX33_WhenExecuteInstruction_ThenStoreValueOfVXInMemoryAsBinaryCodedDecimalAndNotUpdateIndexRegister(byte[] instruction, byte x, byte initialVxValue, byte[] expectedResult)
        {
            // Given
            const ushort initialIndexValue = 0x300;

            var emulator = new Emulator(Substitute.For<ISound>(), Substitute.For<IRenderer>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = initialVxValue;
            emulator.State.Registers.I = initialIndexValue;

            // When
            emulator.ProcessNextMachineCycle();

            // Then
            CollectionAssert.AreEqual(expectedResult, new ArraySegment<byte>(emulator.State.Memory, initialIndexValue, 3).ToArray());
            Assert.AreEqual(initialIndexValue, emulator.State.Registers.I);
        }
    }
}
