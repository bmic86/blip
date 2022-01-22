using Chip;
using Chip.Output;
using Chip.Random;
using Chip.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class BitInstructionsTests
    {
        [TestMethod]
        [DataRow(new byte[] { 0x80, 0xF1 }, 0x0, 0xF, (byte)0b00101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x81, 0xE1 }, 0x1, 0xE, (byte)0b00101000, (byte)0b01000101)]
        [DataRow(new byte[] { 0x82, 0xD1 }, 0x2, 0xD, (byte)0b10001010, (byte)0b01000101)]
        [DataRow(new byte[] { 0x83, 0xC1 }, 0x3, 0xC, (byte)0b00101010, (byte)0b01010001)]
        [DataRow(new byte[] { 0x84, 0xB1 }, 0x4, 0xB, (byte)0b11111010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x85, 0xA1 }, 0x5, 0xA, (byte)0b10101010, (byte)0b01110101)]
        [DataRow(new byte[] { 0x86, 0x91 }, 0x6, 0x9, (byte)0b11101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x87, 0x81 }, 0x7, 0x8, (byte)0b10101011, (byte)0b01110101)]
        [DataRow(new byte[] { 0x88, 0x71 }, 0x8, 0x7, (byte)0b10101011, (byte)0b01110101)]
        [DataRow(new byte[] { 0x89, 0x61 }, 0x9, 0x6, (byte)0b11101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x8A, 0x51 }, 0xA, 0x5, (byte)0b11111010, (byte)0b01011111)]
        [DataRow(new byte[] { 0x8B, 0x41 }, 0xB, 0x4, (byte)0b11101010, (byte)0b11010101)]
        [DataRow(new byte[] { 0x8C, 0x31 }, 0xC, 0x3, (byte)0b00000000, (byte)0b11010101)]
        [DataRow(new byte[] { 0x8D, 0x21 }, 0xD, 0x2, (byte)0b00001010, (byte)0b01010000)]
        [DataRow(new byte[] { 0x8E, 0x11 }, 0xE, 0x1, (byte)0b00001010, (byte)0b00000101)]
        [DataRow(new byte[] { 0x8F, 0x01 }, 0xF, 0x0, (byte)0b11111111, (byte)0b11111111)]
        public async Task GivenInstruction8XY1_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsToVXOrVY(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(initialRegisterXValue | initialRegisterYValue, emulator.State.Registers.V[x]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0xF2 }, 0x0, 0xF, (byte)0b00101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x81, 0xE2 }, 0x1, 0xE, (byte)0b00101000, (byte)0b01000101)]
        [DataRow(new byte[] { 0x82, 0xD2 }, 0x2, 0xD, (byte)0b10001010, (byte)0b01000101)]
        [DataRow(new byte[] { 0x83, 0xC2 }, 0x3, 0xC, (byte)0b00101010, (byte)0b01010001)]
        [DataRow(new byte[] { 0x84, 0xB2 }, 0x4, 0xB, (byte)0b11111010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x85, 0xA2 }, 0x5, 0xA, (byte)0b10101010, (byte)0b01110101)]
        [DataRow(new byte[] { 0x86, 0x92 }, 0x6, 0x9, (byte)0b11101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x87, 0x82 }, 0x7, 0x8, (byte)0b10101011, (byte)0b01110101)]
        [DataRow(new byte[] { 0x88, 0x72 }, 0x8, 0x7, (byte)0b10101011, (byte)0b01110101)]
        [DataRow(new byte[] { 0x89, 0x62 }, 0x9, 0x6, (byte)0b11101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x8A, 0x52 }, 0xA, 0x5, (byte)0b11111010, (byte)0b01011111)]
        [DataRow(new byte[] { 0x8B, 0x42 }, 0xB, 0x4, (byte)0b11101010, (byte)0b11010101)]
        [DataRow(new byte[] { 0x8C, 0x32 }, 0xC, 0x3, (byte)0b00000000, (byte)0b11010101)]
        [DataRow(new byte[] { 0x8D, 0x22 }, 0xD, 0x2, (byte)0b00001010, (byte)0b01010000)]
        [DataRow(new byte[] { 0x8E, 0x12 }, 0xE, 0x1, (byte)0b00001010, (byte)0b00000101)]
        [DataRow(new byte[] { 0x8F, 0x02 }, 0xF, 0x0, (byte)0b11111111, (byte)0b11111111)]
        public async Task GivenInstruction8XY2_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsToVXAndVY(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(initialRegisterXValue & initialRegisterYValue, emulator.State.Registers.V[x]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0xF3 }, 0x0, 0xF, (byte)0b00101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x81, 0xE3 }, 0x1, 0xE, (byte)0b00101000, (byte)0b01000101)]
        [DataRow(new byte[] { 0x82, 0xD3 }, 0x2, 0xD, (byte)0b10001010, (byte)0b01000101)]
        [DataRow(new byte[] { 0x83, 0xC3 }, 0x3, 0xC, (byte)0b00101010, (byte)0b01010001)]
        [DataRow(new byte[] { 0x84, 0xB3 }, 0x4, 0xB, (byte)0b11111010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x85, 0xA3 }, 0x5, 0xA, (byte)0b10101010, (byte)0b01110101)]
        [DataRow(new byte[] { 0x86, 0x93 }, 0x6, 0x9, (byte)0b11101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x87, 0x83 }, 0x7, 0x8, (byte)0b10101011, (byte)0b01110101)]
        [DataRow(new byte[] { 0x88, 0x73 }, 0x8, 0x7, (byte)0b10101011, (byte)0b01110101)]
        [DataRow(new byte[] { 0x89, 0x63 }, 0x9, 0x6, (byte)0b11101010, (byte)0b01010101)]
        [DataRow(new byte[] { 0x8A, 0x53 }, 0xA, 0x5, (byte)0b11111010, (byte)0b01011111)]
        [DataRow(new byte[] { 0x8B, 0x43 }, 0xB, 0x4, (byte)0b11101010, (byte)0b11010101)]
        [DataRow(new byte[] { 0x8C, 0x33 }, 0xC, 0x3, (byte)0b00000000, (byte)0b11010101)]
        [DataRow(new byte[] { 0x8D, 0x23 }, 0xD, 0x2, (byte)0b00001010, (byte)0b01010000)]
        [DataRow(new byte[] { 0x8E, 0x13 }, 0xE, 0x1, (byte)0b00001010, (byte)0b00000101)]
        [DataRow(new byte[] { 0x8F, 0x03 }, 0xF, 0x0, (byte)0b11111111, (byte)0b11111111)]
        public async Task GivenInstruction8XY3_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsToVXExclusiveOrVY(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(initialRegisterXValue ^ initialRegisterYValue, emulator.State.Registers.V[x]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0x16 }, 0x0, 0x1, (byte)0b00000001, (byte)0b00000000, (byte)1)]
        [DataRow(new byte[] { 0x81, 0xE6 }, 0x1, 0xE, (byte)0b00000010, (byte)0b00000001, (byte)0)]
        [DataRow(new byte[] { 0x82, 0xD6 }, 0x2, 0xD, (byte)0b00000100, (byte)0b00000010, (byte)0)]
        [DataRow(new byte[] { 0x83, 0xC6 }, 0x3, 0xC, (byte)0b00000101, (byte)0b00000010, (byte)1)]
        [DataRow(new byte[] { 0x84, 0xB6 }, 0x4, 0xB, (byte)0b00010101, (byte)0b00001010, (byte)1)]
        [DataRow(new byte[] { 0x85, 0xA6 }, 0x5, 0xA, (byte)0b00010100, (byte)0b00001010, (byte)0)]
        [DataRow(new byte[] { 0x86, 0x96 }, 0x6, 0x9, (byte)0b11110000, (byte)0b01111000, (byte)0)]
        [DataRow(new byte[] { 0x87, 0x86 }, 0x7, 0x8, (byte)0b00001111, (byte)0b00000111, (byte)1)]
        [DataRow(new byte[] { 0x88, 0x76 }, 0x8, 0x7, (byte)0b10000000, (byte)0b01000000, (byte)0)]
        [DataRow(new byte[] { 0x89, 0x66 }, 0x9, 0x6, (byte)0b10000001, (byte)0b01000000, (byte)1)]
        [DataRow(new byte[] { 0x8A, 0x56 }, 0xA, 0x5, (byte)0b10000010, (byte)0b01000001, (byte)0)]
        [DataRow(new byte[] { 0x8B, 0x46 }, 0xB, 0x4, (byte)0b10101010, (byte)0b01010101, (byte)0)]
        [DataRow(new byte[] { 0x8C, 0x36 }, 0xC, 0x3, (byte)0b01010101, (byte)0b00101010, (byte)1)]
        [DataRow(new byte[] { 0x8D, 0x26 }, 0xD, 0x2, (byte)0b11111110, (byte)0b01111111, (byte)0)]
        [DataRow(new byte[] { 0x8E, 0x16 }, 0xE, 0x1, (byte)0b11111111, (byte)0b01111111, (byte)1)]
        public async Task GivenInstruction8XY6_WhenExecuteInstruction_ThenStoreVYWithBitsShiftedRightInVXAndStoreLeastSignificantBitOfVYInVF(byte[] instruction, int x, int y, byte initialRegisterYValue, byte expectedXValue, byte expectedVFValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(expectedXValue, emulator.State.Registers.V[x]);
            Assert.AreEqual(expectedVFValue, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0x1E }, 0x0, 0x1, (byte)0b00000001, (byte)0b00000010, (byte)0)]
        [DataRow(new byte[] { 0x81, 0xEE }, 0x1, 0xE, (byte)0b00000010, (byte)0b00000100, (byte)0)]
        [DataRow(new byte[] { 0x82, 0xDE }, 0x2, 0xD, (byte)0b00000100, (byte)0b00001000, (byte)0)]
        [DataRow(new byte[] { 0x83, 0xCE }, 0x3, 0xC, (byte)0b10000101, (byte)0b00001010, (byte)1)]
        [DataRow(new byte[] { 0x84, 0xBE }, 0x4, 0xB, (byte)0b10010101, (byte)0b00101010, (byte)1)]
        [DataRow(new byte[] { 0x85, 0xAE }, 0x5, 0xA, (byte)0b00010100, (byte)0b00101000, (byte)0)]
        [DataRow(new byte[] { 0x86, 0x9E }, 0x6, 0x9, (byte)0b11110000, (byte)0b11100000, (byte)1)]
        [DataRow(new byte[] { 0x87, 0x8E }, 0x7, 0x8, (byte)0b00001111, (byte)0b00011110, (byte)0)]
        [DataRow(new byte[] { 0x88, 0x7E }, 0x8, 0x7, (byte)0b10000000, (byte)0b00000000, (byte)1)]
        [DataRow(new byte[] { 0x89, 0x6E }, 0x9, 0x6, (byte)0b10000001, (byte)0b00000010, (byte)1)]
        [DataRow(new byte[] { 0x8A, 0x5E }, 0xA, 0x5, (byte)0b01000001, (byte)0b10000010, (byte)0)]
        [DataRow(new byte[] { 0x8B, 0x4E }, 0xB, 0x4, (byte)0b10101010, (byte)0b01010100, (byte)1)]
        [DataRow(new byte[] { 0x8C, 0x3E }, 0xC, 0x3, (byte)0b00101010, (byte)0b01010100, (byte)0)]
        [DataRow(new byte[] { 0x8D, 0x2E }, 0xD, 0x2, (byte)0b01111111, (byte)0b11111110, (byte)0)]
        [DataRow(new byte[] { 0x8E, 0x1E }, 0xE, 0x1, (byte)0b11111111, (byte)0b11111110, (byte)1)]
        public async Task GivenInstruction8XYE_WhenExecuteInstruction_ThenStoreVYWithBitsShiftedLeftInVXAndStoreMostSignificantBitOfVYInVF(byte[] instruction, int x, int y, byte initialRegisterYValue, byte expectedXValue, byte expectedVFValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.LoadProgram(instruction);
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(expectedXValue, emulator.State.Registers.V[x]);
            Assert.AreEqual(expectedVFValue, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xC0, 0xFF }, (byte)0x00, (byte)0xFF, (byte)0xFF)]
        [DataRow(new byte[] { 0xC1, 0x0F }, (byte)0x01, (byte)0xFF, (byte)0x0F)]
        [DataRow(new byte[] { 0xC2, 0xF0 }, (byte)0x02, (byte)0xFF, (byte)0xF0)]
        [DataRow(new byte[] { 0xC3, 0xFF }, (byte)0x03, (byte)0x0F, (byte)0x0F)]
        [DataRow(new byte[] { 0xC4, 0xFF }, (byte)0x04, (byte)0xF0, (byte)0xF0)]
        [DataRow(new byte[] { 0xC5, 0x00 }, (byte)0x05, (byte)0xFF, (byte)0x00)]
        [DataRow(new byte[] { 0xC6, 0xFF }, (byte)0x06, (byte)0x00, (byte)0x00)]
        [DataRow(new byte[] { 0xC7, 0b10101010 }, (byte)0x07, (byte)0b00100000, (byte)0b00100000)]
        [DataRow(new byte[] { 0xC8, 0b01010101 }, (byte)0x08, (byte)0b01000001, (byte)0b01000001)]
        [DataRow(new byte[] { 0xC9, 0b11001100 }, (byte)0x09, (byte)0b01010101, (byte)0b01000100)]
        [DataRow(new byte[] { 0xCA, 0b11101010 }, (byte)0x0A, (byte)0b11110000, (byte)0b11100000)]
        [DataRow(new byte[] { 0xCB, 0b11010011 }, (byte)0x0B, (byte)0b10000001, (byte)0b10000001)]
        [DataRow(new byte[] { 0xCC, 0b10000001 }, (byte)0x0C, (byte)0b11010011, (byte)0b10000001)]
        [DataRow(new byte[] { 0xCD, 0b11111000 }, (byte)0x0D, (byte)0b00011000, (byte)0b00011000)]
        [DataRow(new byte[] { 0xCE, 0b00011000 }, (byte)0x0E, (byte)0b11111000, (byte)0b00011000)]
        [DataRow(new byte[] { 0xCF, 0b01010101 }, (byte)0x0F, (byte)0b10101010, (byte)0x00)]
        public async Task GivenInstructionCXNN_WhenExecuteInstruction_ThenSetVXToRandomNumberWithMaskNN(byte[] instruction, byte x, byte randomValue, ushort expectedResult)
        {
            // Given
            var randomGenerator = Substitute.For<IRandomGenerator>();
            randomGenerator.Generate().Returns(randomValue);

            var emulator = new Emulator(Substitute.For<ISound>(), randomGenerator, Substitute.For<ITimeProvider>());
            emulator.LoadProgram(instruction);

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(expectedResult, emulator.State.Registers.V[x]);
        }
    }
}
