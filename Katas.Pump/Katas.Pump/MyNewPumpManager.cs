namespace Katas.Pump
{
    public class MyNewPumpManager
    {
        /// <summary>
        /// Returns the pump's total usage time between the provided lower and upper time bounds.
        /// </summary>
        /// <param name="measures">Measures related to the current pump</param>
        /// <param name="start">Time range lower bound</param>
        /// <param name="end">Time range upper bound</param>
        public TimeSpan GetPumpUsageByDateTimeRange(Measure[] measures, DateTime start, DateTime end)
        {
            var sortedMeasures = measures.OrderBy(m => m.Time).ToList();
            if (!sortedMeasures.Any()                   //no data
                || end <= start                         //inconsistent data
                || sortedMeasures.First().Time > start) //provided range starts before data (unprocessable)
                return TimeSpan.Zero;

            var firstIndexInRange = sortedMeasures.FindIndex(m => m.Time > start);
            var lastIndexInRange = sortedMeasures.FindLastIndex(m => m.Time < end);
            if (firstIndexInRange == -1)
            {
                return sortedMeasures[lastIndexInRange].IsOn ? end - start : TimeSpan.Zero; //provided range starts after data (weird but theorically processable)
            }
            var totalUsageTime = GetTotalUsageTimesBeetweenIndexes(sortedMeasures, firstIndexInRange, lastIndexInRange);

            totalUsageTime += firstIndexInRange > 0 && sortedMeasures[firstIndexInRange - 1].IsOn ? sortedMeasures[firstIndexInRange].Time - start : TimeSpan.Zero;
            totalUsageTime += sortedMeasures[lastIndexInRange].IsOn ? end - sortedMeasures[lastIndexInRange].Time : TimeSpan.Zero;

            return totalUsageTime;
        }


        /// <summary>
        /// Get the total usage time while the pump was on within the provided range
        /// </summary>
        /// <param name="sortedMeasures">Enumerable of <see cref="Measure"/> sorted by <see cref="Measure.Time"/> in ascending order</param>
        /// <param name="firstIndexInRange">First index to get the data from</param>
        /// <param name="lastIndexInRange">Last index to get the data from</param>
        public TimeSpan GetTotalUsageTimesBeetweenIndexes(IEnumerable<Measure> sortedMeasure, int firstIndexInRange, int lastIndexInRange)
            => TimeSpan.FromSeconds(GetUsageTimesBeetweenMeasures(sortedMeasure.Skip(firstIndexInRange).Take(lastIndexInRange - firstIndexInRange + 1)).Sum(t => t.TotalSeconds));

        /// <summary>
        /// Compute the usage time beetween each measure of the provided enumerable
        /// </summary>
        /// <param name="sortedMeasures">Enumerable of <see cref="Measure"/> sorted by <see cref="Measure.Time"/> in ascending order</param>
        /// <returns> An enumerable of <see cref="TimeSpan"/> shorter than sortedMeasure by 1. Zero if the pump was off, the time beetween two measures if the pump was on </returns>
        public IEnumerable<TimeSpan> GetUsageTimesBeetweenMeasures(IEnumerable<Measure> sortedMeasure)
            => sortedMeasure.Zip(sortedMeasure.Skip(1), (first, second) => first.IsOn ? second.Time - first.Time : TimeSpan.Zero);
    }
}
