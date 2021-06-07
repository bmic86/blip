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
				new Emulator().Run(program);
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
			void action() => new Emulator().Run(program);

			// Then
			Assert.ThrowsException<ArgumentNullException>(action);
		}

		[TestMethod]
		public void GivenRunningEmulator_WhenTryingToRunAnotherProgram_ThenThrowException()
		{
			// Given
			var emu = new Emulator();
			emu.Run(new byte[1] { 0x1 });

			// When
			void action() => emu.Run(new byte[1] { 0x2 });

			// Then
			Assert.ThrowsException<InvalidOperationException>(action);
		}
	}
}
