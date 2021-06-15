using Chip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public void GivenInstruction7XNNAndRegisterVXWithInitialValue_WhenExecuteProgram_ThenAddNNToRegisterVX(byte[] instruction, int x, byte initialRegisterValue, byte expectedValue)
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
		public void GivenInstruction8XY0AndRegisterVYWithInitialValue_WhenExecuteProgram_ThenRegisterVXEqualsRegisterVY(byte[] instruction, int x, int y, byte initialRegisterValue)
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
	}
}
