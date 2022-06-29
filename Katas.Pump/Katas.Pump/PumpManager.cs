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
            DisplayTimes(measures);
            for(int i = 0; i < measures.Length && measures[i].Time < end; ++i)
            {
                Measure measure = measures[i];
                if (measure.IsBetween(start, end))
                {
                    totalUsageTime += GetNextUsageTimeToAdd(measure.Time, lastMeasure);
                    lastMeasure = measure;
                }
            }
            totalUsageTime += GetNextUsageTimeToAdd(end, lastMeasure);
            return totalUsageTime;
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

        private TimeSpan GetNextUsageTimeToAdd(DateTime measureTime, Measure lastMeasure)
        {
            return lastMeasure != null && lastMeasure.IsOn
                ? (measureTime - lastMeasure.Time)
                : TimeSpan.Zero;
        }
    }
}
