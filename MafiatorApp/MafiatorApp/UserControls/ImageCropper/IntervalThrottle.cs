using System;
using System.Threading.Tasks;

namespace MafiatorApp.UserControls.ImageCropper
{
    internal class IntervalThrottle
    {
        private readonly Action _onStart;
        private readonly Action _onInterval;
        private readonly Action _onFinish;
        private readonly object _lock = new();

        private bool _isThrottling;
        private int _count;

        public IntervalThrottle(int initialDelay, Action onStart, Action onInterval, Action onFinish)
        {
            Delay = initialDelay;
            this._onFinish = onFinish;
            this._onInterval = onInterval;
            this._onStart = onStart;
        }

        public int Delay { get; set; }

        public void Handle()
        {
            lock (_lock)
            {
                InternalHandle();
            }
        }

        private void InternalHandle()
        {
            _count += 1;

            if (_isThrottling) return;
            _isThrottling = true;
            _count = 0;
            RunTimer();
        }

        private async void RunTimer()
        {
            _onStart.Invoke();

            while (_isThrottling)
            {
                var current = _count;
                await Task.Delay(Delay);

                if (current == _count)
                {
                    _isThrottling = false;
                }
                else
                {
                    _onInterval.Invoke();
                }
            }

            await Task.Delay(Delay);
            _onFinish?.Invoke();
        }
    }
}
