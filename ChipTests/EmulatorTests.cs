using Chip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChipTests
{
	[TestClass]
	public class EmulatorTests
	{
		[TestMethod]
		public void GivenNonNullProgram_WhenTryingToRunIt_ThenEmulatorDoesNotThrowException()
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
	}
}
