using Chip;
using Chip.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ChipTests
{
	[TestClass]
	public class MachineTests
	{
		[TestMethod]
		[DataRow(new byte[] { 0x10, 0x00 }, (ushort)0x000)]
		[DataRow(new byte[] { 0x12, 0x34 }, (ushort)0x234)]
		[DataRow(new byte[] { 0x1F, 0xFF }, (ushort)0xFFF)]
		public void GivenInstruction1NNN_WhenExecuteProgram_ThenProgramShouldJumpToAddressNNN(byte[] instruction, ushort expectedResult) 
		{
			// Given
			var machine = new Machine();
			byte[] program = instruction;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(expectedResult, machine.State.Registers.PC);
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
		public void GivenInstruction3XNNAndValueOfRegisterVXEqualsToNN_WhenExecuteProgram_ThenSkipNextInstruction(byte[] instruction, int x, byte value)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = value;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 4, machine.State.Registers.PC);
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
		public void GivenInstruction3XNNAndValueOfRegisterVXNotEqualsToNN_WhenExecuteProgram_ThenDoNotSkipNextInstruction(byte[] instruction, int x, byte value)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = value;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 2, machine.State.Registers.PC);
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
		public void GivenInstruction4XNNAndValueOfRegisterVXNotEqualsToNN_WhenExecuteProgram_ThenSkipNextInstruction(byte[] instruction, int x, byte value)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = value;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 4, machine.State.Registers.PC);
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
		public void GivenInstruction4XNNAndValueOfRegisterVXEqualsToNN_WhenExecuteProgram_ThenDoNotSkipNextInstruction(byte[] instruction, int x, byte value)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = value;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 2, machine.State.Registers.PC);
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
		public void GivenInstruction5XY0AndValueOfRegisterVXEqualsToValueOfRegisterVY_WhenExecuteProgram_ThenSkipNextInstruction(byte[] instruction, int x, int y, byte vxValue, byte vyValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = vxValue;
			machine.State.Registers.V[y] = vyValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 4, machine.State.Registers.PC);
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
		public void GivenInstruction5XY0AndValueOfRegisterVXNotEqualsToValueOfRegisterVY_WhenExecuteProgram_ThenDoNotSkipNextInstruction(byte[] instruction, int x, int y, byte vxValue, byte vyValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = vxValue;
			machine.State.Registers.V[y] = vyValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 2, machine.State.Registers.PC);
		}

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
		public void GivenInstruction6XNN_WhenExecuteProgram_ThenLoadNNToRegisterVX(byte[] instruction, int x, byte expectedValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(expectedValue, machine.State.Registers.V[x]);
		}

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
		public void GivenInstruction7XNN_WhenExecuteProgram_ThenAddNNToRegisterVX(byte[] instruction, int x, byte initialRegisterValue, byte expectedValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(expectedValue, machine.State.Registers.V[x]);
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
		public void GivenInstruction8XY0_WhenExecuteProgram_ThenLoadValueOfRegisterVYIntoRegisterVX(byte[] instruction, int x, int y, byte initialRegisterValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[y] = initialRegisterValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(machine.State.Registers.V[y], machine.State.Registers.V[x]);
		}

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
		public void GivenInstruction8XY1_WhenExecuteProgram_ThenValueOfRegisterVXEqualsToVXOrVY(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(initialRegisterXValue | initialRegisterYValue, machine.State.Registers.V[x]);
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
		public void GivenInstruction8XY2_WhenExecuteProgram_ThenValueOfRegisterVXEqualsToVXAndVY(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(initialRegisterXValue & initialRegisterYValue, machine.State.Registers.V[x]);
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
		public void GivenInstruction8XY3_WhenExecuteProgram_ThenValueOfRegisterVXEqualsToVXExclusiveOrVY(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(initialRegisterXValue ^ initialRegisterYValue, machine.State.Registers.V[x]);
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
		public void GivenInstruction8XY4AndRegistersVXAndVYWithSumOfValuesLessOrEqual255_WhenExecuteProgram_ThenValueOfRegisterVXEqualsToVXPlusVYAndCarryIsSetToZeroOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(initialRegisterYValue + initialRegisterXValue, machine.State.Registers.V[x]);
			Assert.AreEqual(0, machine.State.Registers.V[0xF]);
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
		public void GivenInstruction8XY4AndRegistersVXAndVYWithSumOfValuesGreaterThan255_WhenExecuteProgram_ThenValueOfRegisterVXEqualsToVXPlusVYAndCarryIsSetToOneOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual((byte)(initialRegisterYValue + initialRegisterXValue), machine.State.Registers.V[x]);
			Assert.AreEqual(1, machine.State.Registers.V[0xF]);
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
		public void GivenInstruction8XY5AndRegisterValueVXGreaterOrEqualVY_WhenExecuteProgram_ThenValueOfRegisterVXEqualsVXMinusVYAndNotBorrowIsSetToOneOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual((byte)(initialRegisterXValue - initialRegisterYValue), machine.State.Registers.V[x]);
			Assert.AreEqual(1, machine.State.Registers.V[0xF]);
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
		public void GivenInstruction8XY5AndRegisterValuesVYGreaterThanVX_WhenExecuteProgram_ThenValueOfRegisterVXEqualsVXMinusVYAndNotBorrowIsSetToZeroOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual((byte)(initialRegisterXValue - initialRegisterYValue), machine.State.Registers.V[x]);
			Assert.AreEqual(0, machine.State.Registers.V[0xF]);
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
		public void GivenInstruction8XY6_WhenExecuteProgram_ThenStoreVYWithBitsShiftedRightInVXAndStoreLeastSignificantBitOfVYInVF(byte[] instruction, int x, int y, byte initialRegisterYValue, byte expectedXValue, byte expectedVFValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(expectedXValue, machine.State.Registers.V[x]);
			Assert.AreEqual(expectedVFValue, machine.State.Registers.V[0xF]);
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
		public void GivenInstruction8XY7AndRegisterValueVYGreaterOrEqualVX_WhenExecuteProgram_ThenValueOfRegisterVXEqualsVYMinusVXAndNotBorrowIsSetToOneOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual((byte)(initialRegisterYValue - initialRegisterXValue), machine.State.Registers.V[x]);
			Assert.AreEqual(1, machine.State.Registers.V[0xF]);
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
		public void GivenInstruction8XY7AndRegisterValuesVXGreaterThanVY_WhenExecuteProgram_ThenValueOfRegisterVXEqualsVYMinusVXAndNotBorrowIsSetToZeroOnVF(byte[] instruction, int x, int y, byte initialRegisterXValue, byte initialRegisterYValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = initialRegisterXValue;
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual((byte)(initialRegisterYValue - initialRegisterXValue), machine.State.Registers.V[x]);
			Assert.AreEqual(0, machine.State.Registers.V[0xF]);
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
		public void GivenInstruction8XYE_WhenExecuteProgram_ThenStoreVYWithBitsShiftedLeftInVXAndStoreMostSignificantBitOfVYInVF(byte[] instruction, int x, int y, byte initialRegisterYValue, byte expectedXValue, byte expectedVFValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[y] = initialRegisterYValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(expectedXValue, machine.State.Registers.V[x]);
			Assert.AreEqual(expectedVFValue, machine.State.Registers.V[0xF]);
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
		public void GivenInstruction9XY0AndValueOfRegisterVXNotEqualsToValueOfRegisterVY_WhenExecuteProgram_ThenSkipNextInstruction(byte[] instruction, int x, int y, byte vxValue, byte vyValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = vxValue;
			machine.State.Registers.V[y] = vyValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 4, machine.State.Registers.PC);
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
		public void GivenInstruction9XY0AndValueOfRegisterVXEqualsToValueOfRegisterVY_WhenExecuteProgram_ThenDoNotSkipNextInstruction(byte[] instruction, int x, int y, byte vxValue, byte vyValue)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[x] = vxValue;
			machine.State.Registers.V[y] = vyValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 2, machine.State.Registers.PC);
		}

		[TestMethod]
		[DataRow(new byte[] { 0xA0, 0x00 }, (ushort)0x000)]
		[DataRow(new byte[] { 0xA1, 0x23 }, (ushort)0x123)]
		[DataRow(new byte[] { 0xAF, 0xFF }, (ushort)0xFFF)]
		public void GivenInstructionANNN_WhenExecuteProgram_ThenStoreAddressNNNInIndexRegister(byte[] instruction, ushort expectedResult)
		{
			// Given
			byte[] program = instruction;
			var machine = new Machine();

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(expectedResult, machine.State.Registers.I);
		}

		[TestMethod]
		[DataRow(new byte[] { 0xB0, 0x00 }, (byte)0x00, (ushort)0x0000)]
		[DataRow(new byte[] { 0xBA, 0xBC }, (byte)0x00, (ushort)0x0ABC)]
		[DataRow(new byte[] { 0xB0, 0x00 }, (byte)0xEF, (ushort)0x00EF)]
		[DataRow(new byte[] { 0xB1, 0x23 }, (byte)0x45, (ushort)0x0168)]
		[DataRow(new byte[] { 0xBF, 0xFF }, (byte)0xFF, (ushort)0x10FE)]
		public void GivenInstructionBNNN_WhenExecuteProgram_ProgramShouldJumpToAddressWhichIsSumOfNNNAndValueOfRegisterV0(byte[] instruction, byte initialRegisterValue, ushort expectedResult)
		{
			// Given
			byte[] program = instruction;

			var machine = new Machine();
			machine.State.Registers.V[0] = initialRegisterValue;

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(expectedResult, machine.State.Registers.PC);
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
		public void GivenInstructionCXNN_WhenExecuteProgram_ShouldSetVXToRandomNumberWithMaskNN(byte[] instruction, byte x, byte randomValue, ushort expectedResult)
		{
			// Given
			byte[] program = instruction;

			var randomGenerator = Substitute.For<IRandomGenerator>();
			randomGenerator.Generate().Returns(randomValue);

			var machine = new Machine(randomGenerator);

			// When
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(expectedResult, machine.State.Registers.V[x]);
		}
	}
}
