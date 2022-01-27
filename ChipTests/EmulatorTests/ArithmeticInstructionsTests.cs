using Chip;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class ArithmeticInstructionsTests
    {
        [TestMethod]
        [DataRow(new byte[] { 0x70, 0x01 }, 0x0, (byte)0x01, (byte)0x02)]
        [DataRow(new byte[] { 0x71, 0x02 }, 0x1, (byte)0x01, (byte)0x03)]
        [DataRow(new byte[] { 0x72, 0x03 }, 0x2, (byte)0x03, (byte)0x06)]
        [DataRow(new byte[] { 0x73, 0x04 }, 0x3, (byte)0x04, (byte)0x08)]
        [DataRow(new byte[] { 0x74, 0x05 }, 0x4, (byte)0x05, (byte)0x0A)]
        [DataRow(new byte[] { 0x75, 0x06 }, 0x5, (byte)0x02, (byte)0x08)]
        [DataRow(new byte[] { 0x76, 0x07 }, 0x6, (byte)0xA0, (byte)0xA7)]
        [DataRow(new byte[] { 0x77, 0x08 }, 0x7, (byte)0xB0, (byte)0xB8)]
        [DataRow(new byte[] { 0x78, 0x09 }, 0x8, (byte)0xC0, (byte)0xC9)]
        [DataRow(new byte[] { 0x79, 0x0A }, 0x9, (byte)0xD1, (byte)0xDB)]
        [DataRow(new byte[] { 0x7A, 0xA0 }, 0xA, (byte)0x1F, (byte)0xBF)]
        [DataRow(new byte[] { 0x7B, 0xB0 }, 0xB, (byte)0x25, (byte)0xD5)]
        [DataRow(new byte[] { 0x7C, 0xC0 }, 0xC, (byte)0x1D, (byte)0xDD)]
        [DataRow(new byte[] { 0x7D, 0xD1 }, 0xD, (byte)0x1D, (byte)0xEE)]
        [DataRow(new byte[] { 0x7E, 0xE2 }, 0xE, (byte)0x0F, (byte)0xF1)]
        [DataRow(new byte[] { 0x7F, 0xF0 }, 0xF, (byte)0x0F, (byte)0xFF)]
        public async Task GivenInstruction7XNN_WhenExecuteInstruction_ThenAddNNToRegisterVX(byte[] instruction, int x, byte initialRegisterValue, byte expectedValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.StartProgramAsync(instruction);
            emulator.State.Registers.V[x] = initialRegisterValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(expectedValue, emulator.State.Registers.V[x]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0x14 }, 0x0, 0x1, (byte)1, (byte)2)]
        [DataRow(new byte[] { 0x81, 0xE4 }, 0x1, 0xE, (byte)3, (byte)4)]
        [DataRow(new byte[] { 0x82, 0xD4 }, 0x2, 0xD, (byte)5, (byte)6)]
        [DataRow(new byte[] { 0x83, 0xC4 }, 0x3, 0xC, (byte)7, (byte)8)]
        [DataRow(new byte[] { 0x84, 0xB4 }, 0x4, 0xB, (byte)9, (byte)10)]
        [DataRow(new byte[] { 0x85, 0xA4 }, 0x5, 0xA, (byte)11, (byte)12)]
        [DataRow(new byte[] { 0x86, 0x94 }, 0x6, 0x9, (byte)13, (byte)14)]
        [DataRow(new byte[] { 0x87, 0x84 }, 0x7, 0x8, (byte)15, (byte)16)]
        [DataRow(new byte[] { 0x88, 0x74 }, 0x8, 0x7, (byte)17, (byte)18)]
        [DataRow(new byte[] { 0x89, 0x64 }, 0x9, 0x6, (byte)19, (byte)20)]
        [DataRow(new byte[] { 0x8A, 0x54 }, 0xA, 0x5, (byte)21, (byte)22)]
        [DataRow(new byte[] { 0x8B, 0x44 }, 0xB, 0x4, (byte)23, (byte)0)]
        [DataRow(new byte[] { 0x8C, 0x34 }, 0xC, 0x3, (byte)25, (byte)3)]
        [DataRow(new byte[] { 0x8D, 0x24 }, 0xD, 0x2, (byte)5, (byte)7)]
        [DataRow(new byte[] { 0x8E, 0x14 }, 0xE, 0x1, (byte)200, (byte)55)]
        public async Task GivenInstruction8XY4AndRegistersVXAndVYWithSumOfValuesLessOrEqual255_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsToVXPlusVYAndCarryIsSetToZeroOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.StartProgramAsync(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(initialRegisterYValue + initialRegisterXValue, emulator.State.Registers.V[x]);
            Assert.AreEqual(0, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0x14 }, 0x0, 0x1, (byte)255, (byte)2)]
        [DataRow(new byte[] { 0x81, 0xE4 }, 0x1, 0xE, (byte)3, (byte)255)]
        [DataRow(new byte[] { 0x82, 0xD4 }, 0x2, 0xD, (byte)5, (byte)255)]
        [DataRow(new byte[] { 0x83, 0xC4 }, 0x3, 0xC, (byte)255, (byte)8)]
        [DataRow(new byte[] { 0x84, 0xB4 }, 0x4, 0xB, (byte)255, (byte)10)]
        [DataRow(new byte[] { 0x85, 0xA4 }, 0x5, 0xA, (byte)255, (byte)12)]
        [DataRow(new byte[] { 0x86, 0x94 }, 0x6, 0x9, (byte)13, (byte)255)]
        [DataRow(new byte[] { 0x87, 0x84 }, 0x7, 0x8, (byte)255, (byte)16)]
        [DataRow(new byte[] { 0x88, 0x74 }, 0x8, 0x7, (byte)17, (byte)255)]
        [DataRow(new byte[] { 0x89, 0x64 }, 0x9, 0x6, (byte)255, (byte)20)]
        [DataRow(new byte[] { 0x8A, 0x54 }, 0xA, 0x5, (byte)21, (byte)255)]
        [DataRow(new byte[] { 0x8B, 0x44 }, 0xB, 0x4, (byte)255, (byte)255)]
        [DataRow(new byte[] { 0x8C, 0x34 }, 0xC, 0x3, (byte)25, (byte)255)]
        [DataRow(new byte[] { 0x8D, 0x24 }, 0xD, 0x2, (byte)255, (byte)7)]
        [DataRow(new byte[] { 0x8E, 0x14 }, 0xE, 0x1, (byte)255, (byte)8)]
        public async Task GivenInstruction8XY4AndRegistersVXAndVYWithSumOfValuesGreaterThan255_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsToVXPlusVYAndCarryIsSetToOneOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.StartProgramAsync(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual((byte)(initialRegisterYValue + initialRegisterXValue), emulator.State.Registers.V[x]);
            Assert.AreEqual(1, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0x15 }, 0x0, 0x1, (byte)2, (byte)1)]
        [DataRow(new byte[] { 0x81, 0xE5 }, 0x1, 0xE, (byte)4, (byte)3)]
        [DataRow(new byte[] { 0x82, 0xD5 }, 0x2, 0xD, (byte)6, (byte)5)]
        [DataRow(new byte[] { 0x83, 0xC5 }, 0x3, 0xC, (byte)8, (byte)7)]
        [DataRow(new byte[] { 0x84, 0xB5 }, 0x4, 0xB, (byte)10, (byte)9)]
        [DataRow(new byte[] { 0x85, 0xA5 }, 0x5, 0xA, (byte)12, (byte)11)]
        [DataRow(new byte[] { 0x86, 0x95 }, 0x6, 0x9, (byte)14, (byte)13)]
        [DataRow(new byte[] { 0x87, 0x85 }, 0x7, 0x8, (byte)16, (byte)15)]
        [DataRow(new byte[] { 0x88, 0x75 }, 0x8, 0x7, (byte)18, (byte)17)]
        [DataRow(new byte[] { 0x89, 0x65 }, 0x9, 0x6, (byte)31, (byte)20)]
        [DataRow(new byte[] { 0x8A, 0x55 }, 0xA, 0x5, (byte)98, (byte)12)]
        [DataRow(new byte[] { 0x8B, 0x45 }, 0xB, 0x4, (byte)100, (byte)100)]
        [DataRow(new byte[] { 0x8C, 0x35 }, 0xC, 0x3, (byte)73, (byte)57)]
        [DataRow(new byte[] { 0x8D, 0x25 }, 0xD, 0x2, (byte)66, (byte)28)]
        [DataRow(new byte[] { 0x8E, 0x15 }, 0xE, 0x1, (byte)85, (byte)33)]
        public async Task GivenInstruction8XY5AndRegisterValueVXGreaterOrEqualVY_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsVXMinusVYAndNotBorrowIsSetToOneOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.StartProgramAsync(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual((byte)(initialRegisterXValue - initialRegisterYValue), emulator.State.Registers.V[x]);
            Assert.AreEqual(1, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0x15 }, 0x0, 0x1, (byte)2, (byte)10)]
        [DataRow(new byte[] { 0x81, 0xE5 }, 0x1, 0xE, (byte)4, (byte)30)]
        [DataRow(new byte[] { 0x82, 0xD5 }, 0x2, 0xD, (byte)6, (byte)50)]
        [DataRow(new byte[] { 0x83, 0xC5 }, 0x3, 0xC, (byte)8, (byte)70)]
        [DataRow(new byte[] { 0x84, 0xB5 }, 0x4, 0xB, (byte)10, (byte)90)]
        [DataRow(new byte[] { 0x85, 0xA5 }, 0x5, 0xA, (byte)12, (byte)101)]
        [DataRow(new byte[] { 0x86, 0x95 }, 0x6, 0x9, (byte)14, (byte)130)]
        [DataRow(new byte[] { 0x87, 0x85 }, 0x7, 0x8, (byte)16, (byte)150)]
        [DataRow(new byte[] { 0x88, 0x75 }, 0x8, 0x7, (byte)18, (byte)170)]
        [DataRow(new byte[] { 0x89, 0x65 }, 0x9, 0x6, (byte)31, (byte)200)]
        [DataRow(new byte[] { 0x8A, 0x55 }, 0xA, 0x5, (byte)98, (byte)120)]
        [DataRow(new byte[] { 0x8B, 0x45 }, 0xB, 0x4, (byte)100, (byte)101)]
        [DataRow(new byte[] { 0x8C, 0x35 }, 0xC, 0x3, (byte)7, (byte)57)]
        [DataRow(new byte[] { 0x8D, 0x25 }, 0xD, 0x2, (byte)66, (byte)90)]
        [DataRow(new byte[] { 0x8E, 0x15 }, 0xE, 0x1, (byte)85, (byte)123)]
        public async Task GivenInstruction8XY5AndRegisterValuesVYGreaterThanVX_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsVXMinusVYAndNotBorrowIsSetToZeroOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.StartProgramAsync(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual((byte)(initialRegisterXValue - initialRegisterYValue), emulator.State.Registers.V[x]);
            Assert.AreEqual(0, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0x17 }, 0x0, 0x1, (byte)1, (byte)2)]
        [DataRow(new byte[] { 0x81, 0xE7 }, 0x1, 0xE, (byte)2, (byte)4)]
        [DataRow(new byte[] { 0x82, 0xD7 }, 0x2, 0xD, (byte)4, (byte)6)]
        [DataRow(new byte[] { 0x83, 0xC7 }, 0x3, 0xC, (byte)6, (byte)10)]
        [DataRow(new byte[] { 0x84, 0xB7 }, 0x4, 0xB, (byte)11, (byte)11)]
        [DataRow(new byte[] { 0x85, 0xA7 }, 0x5, 0xA, (byte)0, (byte)12)]
        [DataRow(new byte[] { 0x86, 0x97 }, 0x6, 0x9, (byte)13, (byte)13)]
        [DataRow(new byte[] { 0x87, 0x87 }, 0x7, 0x8, (byte)255, (byte)255)]
        [DataRow(new byte[] { 0x88, 0x77 }, 0x8, 0x7, (byte)0, (byte)255)]
        [DataRow(new byte[] { 0x89, 0x67 }, 0x9, 0x6, (byte)64, (byte)128)]
        [DataRow(new byte[] { 0x8A, 0x57 }, 0xA, 0x5, (byte)98, (byte)198)]
        [DataRow(new byte[] { 0x8B, 0x47 }, 0xB, 0x4, (byte)111, (byte)222)]
        [DataRow(new byte[] { 0x8C, 0x37 }, 0xC, 0x3, (byte)5, (byte)55)]
        [DataRow(new byte[] { 0x8D, 0x27 }, 0xD, 0x2, (byte)2, (byte)68)]
        [DataRow(new byte[] { 0x8E, 0x17 }, 0xE, 0x1, (byte)42, (byte)54)]
        public async Task GivenInstruction8XY7AndRegisterValueVYGreaterOrEqualVX_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsVYMinusVXAndNotBorrowIsSetToOneOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.StartProgramAsync(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual((byte)(initialRegisterYValue - initialRegisterXValue), emulator.State.Registers.V[x]);
            Assert.AreEqual(1, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x80, 0x17 }, 0x0, 0x1, (byte)10, (byte)2)]
        [DataRow(new byte[] { 0x81, 0xE7 }, 0x1, 0xE, (byte)30, (byte)4)]
        [DataRow(new byte[] { 0x82, 0xD7 }, 0x2, 0xD, (byte)50, (byte)6)]
        [DataRow(new byte[] { 0x83, 0xC7 }, 0x3, 0xC, (byte)70, (byte)8)]
        [DataRow(new byte[] { 0x84, 0xB7 }, 0x4, 0xB, (byte)90, (byte)10)]
        [DataRow(new byte[] { 0x85, 0xA7 }, 0x5, 0xA, (byte)101, (byte)12)]
        [DataRow(new byte[] { 0x86, 0x97 }, 0x6, 0x9, (byte)130, (byte)14)]
        [DataRow(new byte[] { 0x87, 0x87 }, 0x7, 0x8, (byte)150, (byte)16)]
        [DataRow(new byte[] { 0x88, 0x77 }, 0x8, 0x7, (byte)170, (byte)18)]
        [DataRow(new byte[] { 0x89, 0x67 }, 0x9, 0x6, (byte)200, (byte)31)]
        [DataRow(new byte[] { 0x8A, 0x57 }, 0xA, 0x5, (byte)120, (byte)98)]
        [DataRow(new byte[] { 0x8B, 0x47 }, 0xB, 0x4, (byte)101, (byte)100)]
        [DataRow(new byte[] { 0x8C, 0x37 }, 0xC, 0x3, (byte)57, (byte)7)]
        [DataRow(new byte[] { 0x8D, 0x27 }, 0xD, 0x2, (byte)90, (byte)66)]
        [DataRow(new byte[] { 0x8E, 0x17 }, 0xE, 0x1, (byte)123, (byte)85)]
        public async Task GivenInstruction8XY7AndRegisterValuesVXGreaterThanVY_WhenExecuteInstruction_ThenValueOfRegisterVXEqualsVYMinusVXAndNotBorrowIsSetToZeroOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.StartProgramAsync(instruction);
            emulator.State.Registers.V[x] = initialRegisterXValue;
            emulator.State.Registers.V[y] = initialRegisterYValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual((byte)(initialRegisterYValue - initialRegisterXValue), emulator.State.Registers.V[x]);
            Assert.AreEqual(0, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        [DataRow(new byte[] { 0xF0, 0x1E }, (byte)0x0, (ushort)1, (byte)2)]
        [DataRow(new byte[] { 0xF1, 0x1E }, (byte)0x1, (ushort)0, (byte)3)]
        [DataRow(new byte[] { 0xF2, 0x1E }, (byte)0x2, (ushort)4, (byte)0)]
        [DataRow(new byte[] { 0xF3, 0x1E }, (byte)0x3, (ushort)255, (byte)255)]
        [DataRow(new byte[] { 0xF4, 0x1E }, (byte)0x4, (ushort)5, (byte)6)]
        [DataRow(new byte[] { 0xF5, 0x1E }, (byte)0x5, (ushort)10, (byte)10)]
        [DataRow(new byte[] { 0xF6, 0x1E }, (byte)0x6, (ushort)255, (byte)0)]
        [DataRow(new byte[] { 0xF7, 0x1E }, (byte)0x7, (ushort)65280, (byte)255)]
        [DataRow(new byte[] { 0xF8, 0x1E }, (byte)0x8, (ushort)0, (byte)0)]
        [DataRow(new byte[] { 0xF9, 0x1E }, (byte)0x9, (ushort)8, (byte)12)]
        [DataRow(new byte[] { 0xFA, 0x1E }, (byte)0xA, (ushort)23, (byte)35)]
        [DataRow(new byte[] { 0xFB, 0x1E }, (byte)0xB, (ushort)10, (byte)20)]
        [DataRow(new byte[] { 0xFC, 0x1E }, (byte)0xC, (ushort)1234, (byte)234)]
        [DataRow(new byte[] { 0xFD, 0x1E }, (byte)0xD, (ushort)76, (byte)34)]
        [DataRow(new byte[] { 0xFE, 0x1E }, (byte)0xE, (ushort)1285, (byte)127)]
        [DataRow(new byte[] { 0xFF, 0x1E }, (byte)0xF, (ushort)45, (byte)59)]
        public async Task GivenInstructionFX1E_WhenExecuteInstruction_ThenSumIndexRegisterWithVX(byte[] instruction, byte x, ushort initialIndexValue, byte initialVxValue)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>());
            emulator.StartProgramAsync(instruction);
            emulator.State.Registers.I = initialIndexValue;
            emulator.State.Registers.V[x] = initialVxValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(initialIndexValue + initialVxValue, emulator.State.Registers.I);
        }
    }
}
