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
		public void GivenInstruction3XNN_WhenExecuteProgramAndValueOfRegisterVXEqualsToNN_ThenSkipNextInstruction(byte[] instruction, int x, byte value)
		{
			// Given
			var machine = new Machine();
			byte[] program = instruction;

			// When
			machine.State.Registers.V[x] = value;
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
		public void GivenInstruction3XNN_WhenExecuteProgramAndValueOfRegisterVXNotEqualsToNN_ThenDontSkipNextInstruction(byte[] instruction, int x, byte value)
		{
			// Given
			var machine = new Machine();
			byte[] program = instruction;

			// When
			machine.State.Registers.V[x] = value;
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
		public void GivenInstruction4XNN_WhenExecuteProgramAndValueOfRegisterVXNotEqualsToNN_ThenSkipNextInstruction(byte[] instruction, int x, byte value)
		{
			// Given
			var machine = new Machine();
			byte[] program = instruction;

			// When
			machine.State.Registers.V[x] = value;
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
		public void GivenInstruction4XNN_WhenExecuteProgramAndValueOfRegisterVXEqualsToNN_ThenDontSkipNextInstruction(byte[] instruction, int x, byte value)
		{
			// Given
			var machine = new Machine();
			byte[] program = instruction;

			// When
			machine.State.Registers.V[x] = value;
			machine.ExecuteProgram(program);

			// Then
			Assert.AreEqual(Default.StartAddress + 2, machine.State.Registers.PC);
		}
	}
}
