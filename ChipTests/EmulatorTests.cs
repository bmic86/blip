using Chip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChipTests
{
	[TestClass]
	public class EmulatorTests
	{
		[TestMethod]
		public void GivenNonNullProgram_WhenTryingToRunIt_ThenNotThrowAnyException()
		{
			// Given
			var program = new byte[1] { 0x1 };

			// When
			try
			{
				new Emulator().RunProgram(program);
			}
			// Then
			catch
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void GivenNullProgram_WhenTryingToRunIt_ThenThrowException()
		{
			// Given
			byte[] program = null;

			// When
			void action() => new Emulator().RunProgram(program);

			// Then
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void GivenTooLargeProgram_WhenTryingToRunIt_ThenThrowException()
		{
			// Given
			byte[] program = new byte[3585];

			// When
			void action() => new Emulator().RunProgram(program);

			// Then
			Assert.ThrowsException<InvalidProgramException>(action);
		}
	}
}
