using System.Collections.Generic;

namespace Chip
{
	internal class MachineState
	{
		internal byte[] Memory { get; private set; } = new byte[Default.MemorySize];
		internal Registers Registers { get; private set; } = new();
		internal Stack<ushort> Stack = new();
	}
}
