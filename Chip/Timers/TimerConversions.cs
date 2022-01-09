using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Timers
{
    internal static class TimerConversions
    {
        private const double TimerFrequencyInHz = 60.0;
        private const double SecondsPerTick = 1.0 / TimerFrequencyInHz;

        public static double SecondsToTicks(double timeInSeconds) => timeInSeconds / SecondsPerTick;

        public static double TicksToSeconds(byte timerTicks) => timerTicks * SecondsPerTick;
    }
}
