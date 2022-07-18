namespace Katas.Pump
{
    public class Measure : IComparable<Measure>
    {
        public DateTime Time { get; }

        public bool IsOn { get; }

        public Measure(DateTime time, bool isOn)
        {
            Time = time;
            IsOn = isOn;
        }

        /// <summary>
        /// Returns 0 if the two compared measures have the same time, returns 1 if the
        /// current measure's time is greater than the <paramref name="other"/> measure's time
        /// or if the <paramref name="other"/> measure is null, otherwise returns -1.
        /// </summary>
        /// <param name="other">Other measure to be compared to the current one.</param>
        /// <returns></returns>
        public int CompareTo(Measure? other)
        {
            if (other == null)
                return 1;
            if (other.Time == Time)
                return 0;
            return other.Time < Time
                ? 1
                : -1;
        }

        /// <summary>
        /// Returns true if the current measure's time is between the provided range's bounds.
        /// </summary>
        /// <param name="start">Range lower bound</param>
        /// <param name="end">Range upper bound</param>
        /// <returns></returns>
        public bool IsBetween(DateTime start, DateTime end)
            => Time >= start && Time <= end;

        /// <summary>
        /// Determines if the measure describes a pump which was open before the
        /// provided <paramref name="start"/> time.
        /// </summary>
        /// <param name="start">Higher bound against which the current time will be compared</param>
        /// <returns></returns>
        public bool IsAnteriorOpenPump(DateTime start)
            => Time < start && IsOn;
    }
}
