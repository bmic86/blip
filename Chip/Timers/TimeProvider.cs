using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Timers
{
    internal class TimeProvider : ITimeProvider
    {
        public DateTime CurrentTime => DateTime.Now;
    }
}
