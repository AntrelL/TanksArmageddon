using System;

namespace IJunior.UI
{
    public class TimeText : BasicDigitalText
    {
        private Func<TimeSpan, string> _timeFormatter;

        public void Initialize(Func<TimeSpan, string> timeFormatter)
        {
            Initialize();
            _timeFormatter = timeFormatter;
        }

        public override void SetValue(float time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            string textTime = _timeFormatter.Invoke(timeSpan);

            Value = time;
            SetText(textTime);
        }
    }
}