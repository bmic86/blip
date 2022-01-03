using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Timers
{
    internal class DelayTimer
    {
        private const int TimerFrequencyInHz = 60;
        private const double MilisecondsPerTick = 1000 / TimerFrequencyInHz;

        private DateTime? _lastCheckTime;
        private double _value = 0.0;

        internal byte Value => (byte)_value;

        internal void Start(byte delayValue)
        {
            _value = delayValue;
            _lastCheckTime = DateTime.Now;
        }

        internal void Update()
        {
            if (!_lastCheckTime.HasValue)
            {
                return;
            }

            var now = DateTime.Now;
            double diffInMs = (now - _lastCheckTime.Value).TotalMilliseconds;

            double diffInTicks = diffInMs / MilisecondsPerTick;
            double result = _value - diffInTicks;
            if (result > 0.0)
            {
                _value = result;
                _lastCheckTime = now;

            }
            else
            {
                _value = 0.0;
                _lastCheckTime = null;
            }
        }
    }
}
