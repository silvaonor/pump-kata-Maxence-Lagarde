namespace Katas.Pump
{
    public class Measure
    {
        public DateTime Time { get; }

        public bool IsOn { get; }

        public Measure(DateTime time, bool isOn)
        {
            Time = time;
            IsOn = isOn;
        }
    }
}
