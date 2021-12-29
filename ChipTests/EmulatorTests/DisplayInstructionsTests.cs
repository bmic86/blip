using Chip;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using System.Threading.Tasks;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class DisplayInstructionsTests
    {
        [TestMethod]
        [DataRow(0x0)]
        [DataRow(0x1)]
        [DataRow(0x2)]
        [DataRow(0x3)]
        [DataRow(0x4)]
        [DataRow(0x5)]
        [DataRow(0x6)]
        [DataRow(0x7)]
        [DataRow(0x8)]
        [DataRow(0x9)]
        [DataRow(0xA)]
        [DataRow(0xB)]
        [DataRow(0xC)]
        [DataRow(0xD)]
        [DataRow(0xE)]
        [DataRow(0xF)]
        public async Task GivenDigitDrawingProgramAndValueInRegisterV0_WhenExecuteProgram_ThenProgramShouldDrawSpriteOfAHexDigitFromV0Value(int digit)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            emulator.LoadProgram(new byte[] 
            { 
                0xF0, 0x29, // Set index register to sprite address of a digit stored in V0.
                0xD1, 0x25  // Draw it.
            });
            emulator.State.Registers.V[0] = (byte)digit;

            // When
            await emulator.ProcessNextMachineCycleAsync();
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            var frame = emulator.Display.ReadFrame();
            CollectionAssert.AreEqual(SpriteBitMaps.HexDigits[digit][0], frame.ElementAt(0).Take(8).ToArray());
            CollectionAssert.AreEqual(SpriteBitMaps.HexDigits[digit][1], frame.ElementAt(1).Take(8).ToArray());
            CollectionAssert.AreEqual(SpriteBitMaps.HexDigits[digit][2], frame.ElementAt(2).Take(8).ToArray());
            CollectionAssert.AreEqual(SpriteBitMaps.HexDigits[digit][3], frame.ElementAt(3).Take(8).ToArray());
            CollectionAssert.AreEqual(SpriteBitMaps.HexDigits[digit][4], frame.ElementAt(4).Take(8).ToArray());
        }
    }
}
