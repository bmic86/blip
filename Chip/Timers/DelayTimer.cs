using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip.Timers
{
    internal class DelayTimer
    {
        private readonly ITimeProvider _timeProvider;

        private DateTime? _lastCheckTime;
        private double _value = 0.0;

        internal DelayTimer()
            => _timeProvider = new TimeProvider();

        internal DelayTimer(ITimeProvider timeProvider)
            => _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

        internal byte Value => (byte)_value;

        internal void Start(byte delayValue)
        {
            _value = delayValue;
            _lastCheckTime = _timeProvider.CurrentTime;
        }

        internal void Update()
        {
            if (!_lastCheckTime.HasValue)
            {
                return;
            }

            var now = _timeProvider.CurrentTime;
            double diffInSeconds = (now - _lastCheckTime.Value).TotalSeconds;

            double result = _value - TimerConversions.SecondsToTicks(diffInSeconds);
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
