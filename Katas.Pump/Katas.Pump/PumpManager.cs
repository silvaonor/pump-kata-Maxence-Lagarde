namespace Katas.Pump
{
    public class PumpManager
    {
        /// <summary>
        /// Returns the pump's total usage time between the provided lower and upper time bounds.
        /// </summary>
        /// <param name="measures">Measures related to the current pump</param>
        /// <param name="start">Time range lower bound</param>
        /// <param name="end">Time range upper bound</param>
        /// <returns></returns>
        public TimeSpan GetPumpUsageByDateTimeRange(Measure[] measures, DateTime start, DateTime end)
        {
            TimeSpan totalUsageTime = TimeSpan.Zero;
            Measure lastMeasure = null!;
            QuickSort(measures, 0, measures.Length - 1);
            for(int i = 0; i < measures.Length && measures[i].Time < end; ++i)
            {
                Measure measure = measures[i];
                if (measure.IsBetween(start, end))
                {
                    totalUsageTime += GetUsageTimeFromAnteriorOpenPump(measures, start, i, measure);
                    totalUsageTime += GetNextUsageTime(measure.Time, lastMeasure);
                    lastMeasure = measure;
                }
            }
            totalUsageTime += GetNextUsageTime(end, lastMeasure);
            return totalUsageTime;
        }

        /// <summary>
        /// If an anterior "on" measure exists (i.e : a measure which was taken
        /// strictly before the <paramref name="start"/> time, with a status describing
        /// an open pump) and if this anterior measure strictly precedes the current 
        /// <paramref name="measure"/>, returns the elapsed time from the <paramref name="start"/>
        /// to the current <paramref name="measure"/>, else returns <see cref="TimeSpan.Zero"/>.
        /// </summary>
        /// <param name="sortedMeasures">Measures sorted by <see cref="Measure.Time"/> in ascending order</param>
        /// <param name="start">Start time</param>
        /// <param name="i">Current index in <paramref name="sortedMeasures"/> array</param>
        /// <param name="measure">Current measure</param>
        /// <returns></returns>
        private TimeSpan GetUsageTimeFromAnteriorOpenPump(Measure[] sortedMeasures, DateTime start, int i, Measure measure)
        {
            if (i > 0)
            {
                Measure previousMeasure = sortedMeasures[i - 1];
                if (previousMeasure.IsAnteriorOpenPump(start))
                    return GetNextUsageTime(measure.Time, new Measure(start, true));
            }
            return TimeSpan.Zero;
        }

        /// <summary>
        /// Applies a quick sort algorithm to the provided <paramref name="measures"/> array
        /// using a measure's time as comparator.
        /// </summary>
        /// <param name="measures">The provided measures array</param>
        /// <param name="lowestIndex">The array's currently used lowest index</param>
        /// <param name="highestIndex">The array's currently used highest index</param>
        private void QuickSort(Measure[] measures, int lowestIndex, int highestIndex)
        {
            if(lowestIndex < highestIndex)
            {
                int newPivot = Partition(measures, lowestIndex, highestIndex);
                QuickSort(measures, lowestIndex, newPivot - 1);
                QuickSort(measures, newPivot + 1, highestIndex);
            }
        }

        /// <summary>
        /// Partitions the provided measures array using its last element as pivot and returns
        /// the new pivot.
        /// </summary>
        /// <param name="measures">The provided measures array</param>
        /// <param name="lowestIndex">The array's currently used lowest index</param>
        /// <param name="highestIndex">The array's currently used highest index</param>
        /// <returns></returns>
        private int Partition(Measure[] measures, int lowestIndex, int highestIndex)
        {
            int i = lowestIndex - 1;
            Measure pivot = measures[highestIndex];
            for(int j = lowestIndex; j < highestIndex; ++j)
            {
                if(measures[j].CompareTo(pivot) == - 1)
                {
                    ++i;
                    Swap(measures, i, j);
                }
            }
            Swap(measures, i + 1, highestIndex);
            return i + 1;
        }

        /// <summary>
        /// Swaps the two elements at indexes <paramref name="i"/> and <paramref name="j"/>.
        /// </summary>
        /// <param name="measures">The provided measures array</param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void Swap(Measure[] measures, int i, int j)
        {
            Measure temp = measures[i];
            measures[i] = measures[j];
            measures[j] = temp;
        }

        /// <summary>
        /// Returns the next time span to add to the current total pump usage time.
        /// This method expects a <paramref name="measureTime"/> between the configured
        /// lower and upper bounds (both included).
        /// </summary>
        /// <param name="measureTime">Current measure</param>
        /// <param name="lastMeasure">Previous measure</param>
        /// <returns></returns>
        private TimeSpan GetNextUsageTime(DateTime measureTime, Measure lastMeasure)
        {
            return lastMeasure != null && lastMeasure.IsOn
                ? (measureTime - lastMeasure.Time)
                : TimeSpan.Zero;
        }

        /// <summary>
        /// Displays the formatted times of the provided measures array.
        /// </summary>
        /// <param name="measures">The provided measures array</param>
        private void DisplayTimes(Measure[] measures)
        {
            Console.Write("[");
            for(int i = 0; i < measures.Length; ++i)
            {
                Console.Write(measures[i].Time.ToString("dd/MM/yyyy HH:mm:ss"));
                if(i < measures.Length - 1)
                    Console.Write(", ");
            }
            Console.WriteLine("]");
        }
    }
}
