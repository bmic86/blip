# Blip

Chip-8 emulator/interpreter written in C# and Blazor. It is running fully in web browser using WebAssembly.

Implementation is based on classic COSMAC VIP Chip-8 interpreter specification found on the internet.

## Key mappings

Original keypad:

| 1 | 2 | 3 | C |
| - | - | - | - |
| 4 | 5 | 6 | D |
| 7 | 8 | 9 | E |
| A | 0 | B | F |

Is mapped to:

| 1 | 2 | 3 | 4 |
| - | - | - | - |
| Q | W | E | R |
| A | S | D | F |
| Z | X | C | V |

## Specification Sources

1. RCA COSMAC VIP CDP18S711 Instruction Manual (1978)
2. [Cowgod's Chip-8 Technical Reference](http://devernay.free.fr/hacks/chip8/C8TECH10.HTM)
3. [Chip-8 wiki](https://github.com/mattmikolay/chip-8/wiki)
4. [CHIP-8 extensions and compatibility](https://chip-8.github.io/extensions/)
5. Reddit [r/EmuDev](https://www.reddit.com/r/EmuDev/) channel



