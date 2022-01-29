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
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            await emulator.StartProgramAsync(new byte[] { 0x00, 0xE0 });
            TurnOnAllPixelsOnScreen(emulator);

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            var screenPixels = emulator.Screen.ReadPixels(0, 0, emulator.Screen.Width, emulator.Screen.Height);
            Assert.IsFalse(screenPixels.Any(p => p.Value != false));
        }

        [TestMethod]
        public async Task GivenInstruction00E0_WhenExecuteInstruction_ThenCallCleanScreenRendererMethod()
        {
            // Given
            var renderer = Substitute.For<IRenderer>();
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = renderer
            };

            await emulator.StartProgramAsync(new byte[] { 0x00, 0xE0 });

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            await renderer.Received().ClearScreenAsync();
        }

        [TestMethod]
        public async Task GivenInstructionDXYN_WhenExecuteInstruction_ThenIndexRegisterValueShouldNotChange()
        {
            // Given
            const int initialIndexRegisterValue = 16;

            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            await emulator.StartProgramAsync(new byte[] { 0xD0, 0x1F });
            emulator.State.Registers.I = initialIndexRegisterValue;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(initialIndexRegisterValue, emulator.State.Registers.I);
        }

        [TestMethod]
        [DataRow((byte)0, (byte)0)]
        [DataRow((byte)32, (byte)16)]
        [DataRow((byte)56, (byte)0)]
        [DataRow((byte)0, (byte)31)]
        [DataRow((byte)56, (byte)31)]
        [DataRow((byte)64, (byte)32)]
        [DataRow((byte)248, (byte)255)]
        public async Task GivenInstructionDXYN_WhenExecuteInstruction_ThenDrawSpriteOnProperScreenPosition(byte positionX, byte positionY)
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            await emulator.StartProgramAsync(new byte[] { 0xD0, 0x1F });
            emulator.State.Registers.V[0x0] = positionX;
            emulator.State.Registers.V[0x1] = positionY;

            int expectedPositionX = positionX % emulator.Screen.Width;
            int expectedPositionY = positionY % emulator.Screen.Height;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            var screenPixels = emulator.Screen.ReadPixels(expectedPositionX, expectedPositionY, 8, 1);
            CollectionAssert.AreEqual(Enumerable.Range(expectedPositionX, 8).ToArray(), screenPixels.Select(pixel => pixel.X).ToArray());
            CollectionAssert.AreEqual(Enumerable.Repeat<int>(expectedPositionY, 8).ToArray(), screenPixels.Select(pixel => pixel.Y).ToArray());
        }

        [TestMethod]
        public async Task GivenInstructionDXYNAndEmptyDrawingRegionOnScreen_WhenExecuteInstruction_ThenDoNotSetCollisionFlag()
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            await emulator.StartProgramAsync(new byte[] { 0xD0, 0x1F });

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(0, emulator.State.Registers.V[0xF]);
        }

        [TestMethod]
        public async Task GivenInstructionDXYNAndNonEmptyDrawingRegionOnScreen_WhenExecuteInstruction_ThenSetCollisionFlag()
        {
            // Given
            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = Substitute.For<IRenderer>()
            };

            await emulator.StartProgramAsync(new byte[] { 0xD0, 0x1F });
            emulator.Screen.DrawPixelsOctetFromByte(0, 0, 0xFF);

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(1, emulator.State.Registers.V[0xF]);
        }

        // "DXYN clips sprites that are partially drawn outside of the display area."
        // Source: https://chip-8.github.io/extensions/#chip-8
        [TestMethod]
        [DataRow((byte)63, (byte)0, 15)]
        [DataRow((byte)63, (byte)31, 1)]
        [DataRow((byte)60, (byte)16, 4 * 15)]
        [DataRow((byte)30, (byte)25, 8 * 7)]
        public async Task GivenInstructionDXYN_WhenExecuteInstruction_ThenDoNotDrawSpritePartsThatAreOutsideOfAScreen(byte positionX, byte positionY, int expectedPixelsNumToDraw)
        {
            // Given
            IEnumerable<Pixel> result = null;
            var renderer = Substitute.For<IRenderer>();
            await renderer.DrawPixelsAsync(Arg.Do<IEnumerable<Pixel>>(arg => result = arg));

            var emulator = new Emulator(Substitute.For<ISound>())
            {
                Renderer = renderer
            };

            await emulator.StartProgramAsync(new byte[] { 0xD0, 0x1F });
            emulator.State.Registers.V[0x0] = positionX;
            emulator.State.Registers.V[0x1] = positionY;

            // When
            await emulator.ProcessNextMachineCycleAsync();

            // Then
            Assert.AreEqual(expectedPixelsNumToDraw, result.Count());
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

            await emulator.StartProgramAsync(new byte[]
            {
                0xF0, 0x29, // Set index register to sprite address of a digit stored in V0.
                0xD1, 0x25  // Draw it.
            });
            emulator.State.Registers.V[0] = (byte)digit;

            // When
            await emulator.ProcessNextMachineCycleAsync();
            await emulator.ProcessNextMachineCycleAsync();

            // Then
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
