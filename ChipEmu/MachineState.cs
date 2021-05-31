using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip
{
	internal class MachineState
	{
		internal byte[] Memory { get; private set; } = new byte[Default.MemorySize];
		internal Registers Registers { get; private set; } = new();

		internal void ClearAll()
		{
			Array.Clear(Memory, 0, Default.MemorySize);
			Registers.ClearAll();
		} 
	}
}
