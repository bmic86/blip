using Chip;
using Chip.Display;
using Chip.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChipTests.EmulatorTests
{
    [TestClass]
    public class DisplayInstructionsTests
    {
        [TestMethod]
        public async Task GivenInstruction00E0AndAllPixelsOnScreenTurnedOn_WhenExecuteInstruction_ThenClearDisplayScreen()
        {
            // Given
            var renderer = Substitute.For<IRenderer>();
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = renderer
            };

            emulator.LoadProgram(new byte[] { 0x00, 0xE0 });
            TurnOnAllPixelsOnScreen(emulator);

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            var screenPixels = emulator.Screen.ReadPixels(0, 0, emulator.Screen.Width, emulator.Screen.Height);

            Assert.IsFalse(screenPixels.Any(p => p.Value != false));
            await renderer.Received().ClearScreenAsync();
        }

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
            var expectedPixelsToDraw = GetCharacterSpriteToDraw(digit);

            IEnumerable<Pixel> result = null;
            var renderer = Substitute.For<IRenderer>();
            await renderer.DrawPixelsAsync(Arg.Do<IEnumerable<Pixel>>(arg => result = arg));

            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = renderer
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
            var frame = emulator.Screen;
            CollectionAssert.AreEqual(expectedPixelsToDraw, result.ToList());
        }

        private static List<Pixel> GetCharacterSpriteToDraw(int characterDigit)
        {
            var pixelsToDraw = new List<Pixel>();

            int offset = characterDigit * CharacterSprites.CharacterSize;
            var bytes = new ArraySegment<byte>(CharacterSprites.Data, offset, CharacterSprites.CharacterSize).ToArray();
            for (int y = 0; y < bytes.Length; ++y)
            {
                var bits = new BitArray(new[] { bytes[y] }).Cast<bool>().Reverse().ToArray();
                for (int x = 0; x < bits.Length; ++x)
                {
                    pixelsToDraw.Add(new Pixel(x, y, bits[x]));
                }
            }

            return pixelsToDraw;
        }

        private static void TurnOnAllPixelsOnScreen(Emulator emulator)
        {
            for (int y = 0; y < emulator.Screen.Height; y++)
            {
                for (int x = 0; x < emulator.Screen.Width; x += 8)
                {
                    emulator.Screen.DrawPixelsOctetFromByte(x, y, 0xFF);
                }
            }
        }
    }
}
