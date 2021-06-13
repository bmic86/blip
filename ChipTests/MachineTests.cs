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
	}
}
