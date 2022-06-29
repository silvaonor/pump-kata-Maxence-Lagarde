using System;
using Xunit;

namespace Katas.Pump.Test
{
    public class PumpManagerTests
    {
        private static readonly PumpManager _manager = new PumpManager();

        private static readonly DateTime _start = new DateTime(2022, 1, 20, 6, 0, 0);

        private static readonly DateTime _end = _start.AddHours(10);

        [Fact]
        public void GivenZeroMeasure_ReturnsZeroUsageTime()
        {
            TimeSpan expectedUsageTime = TimeSpan.Zero;
            Measure[] emptyMeasures = Array.Empty<Measure>();

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(emptyMeasures, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenSingleInRangeMeasure_WhenOff_ReturnsZeroUsageTime()
        {
            DateTime inRangeTime = _start;
            TimeSpan expectedUsageTime = TimeSpan.Zero;
            var inRangeMeasure = new Measure(inRangeTime, false);
            var singleMeasureArray = new Measure[1] { inRangeMeasure };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(singleMeasureArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenSingleInRangeMeasure_WhenOn_ReturnsUntilEndUsageTime()
        {
            DateTime inRangeTime = _start;
            TimeSpan expectedUsageTime = _end - inRangeTime;
            var inRangeMeasure = new Measure(inRangeTime, true);
            var singleMeasureArray = new Measure[1] { inRangeMeasure };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(singleMeasureArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenSingleOutOfRangeMeasure_WhenOn_ReturnsZeroUsageTime()
        {
            TimeSpan expectedUsageTime = TimeSpan.Zero;
            var outOfRangeTime = new DateTime(_start.Year, _start.Month, _start.Day, _start.Hour - 1, _start.Minute, _start.Second);
            var outOfRangeMeasure = new Measure(outOfRangeTime, true);
            var singleMeasureArray = new Measure[1] { outOfRangeMeasure };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(singleMeasureArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenSingleOutOfRangeMeasure_WhenOff_ReturnsZeroUsageTime()
        {
            TimeSpan expectedUsageTime = TimeSpan.Zero;
            var outOfRangeTime = new DateTime(_start.Year, _start.Month, _start.Day, _start.Hour - 1, _start.Minute, _start.Second);
            var outOfRangeMeasure = new Measure(outOfRangeTime, false);
            var singleMeasureArray = new Measure[1] { outOfRangeMeasure };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(singleMeasureArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenSingleInRangeMeasure_WhenOnAndStartGreaterThanEnd_ReturnsZeroUsageTime()
        {
            DateTime inRangeTime = _start;
            TimeSpan expectedUsageTime = TimeSpan.Zero;
            var inRangeMeasure = new Measure(inRangeTime, false);
            var singleMeasureArray = new Measure[1] { inRangeMeasure };
            DateTime greaterStart = _end.AddHours(1);

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(singleMeasureArray, greaterStart, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenTwoInRangeMeasure_WhenOnThenOff_ReturnsUsageTimeInBetweenOnly()
        {
            DateTime inRangeOnTime = _start;
            DateTime inRangeOffTime = _start.AddHours(1);
            var inRangeOnMeasure = new Measure(inRangeOnTime, true);
            var inRangeOffMeasure = new Measure(inRangeOffTime, false);
            TimeSpan expectedUsageTime = inRangeOffTime - inRangeOnTime;
            var twoMeasuresArray = new Measure[2] { inRangeOnMeasure, inRangeOffMeasure };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(twoMeasuresArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenTwoInRangeMeasure_WhenOffThenOn_ReturnsUsageUntilEndOnly()
        {
            DateTime inRangeOnTime = _start.AddHours(1);
            DateTime inRangeOffTime = _start;
            var inRangeOnMeasure = new Measure(inRangeOnTime, true);
            var inRangeOffMeasure = new Measure(inRangeOffTime, false);
            TimeSpan expectedUsageTime = _end - inRangeOnTime;
            var twoMeasuresArray = new Measure[2] { inRangeOnMeasure, inRangeOffMeasure };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(twoMeasuresArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenTwoInRangeMeasure_WhenOnThenOn_ReturnsUsageTimeInBetweenPlusUntilEnd()
        {
            DateTime inRangeOnTime1 = _start;
            DateTime inRangeOnTime2 = _start.AddHours(1);
            var inRangeOnMeasure1 = new Measure(inRangeOnTime1, true);
            var inRangeOnMeasure2 = new Measure(inRangeOnTime2, true);
            TimeSpan expectedUsageTime = (inRangeOnTime2 - inRangeOnTime1) + (_end - inRangeOnTime2);
            var twoMeasuresArray = new Measure[2] { inRangeOnMeasure1, inRangeOnMeasure2 };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(twoMeasuresArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenTwoInRangeMeasure_WhenOffThenOff_ReturnsZeroUsageTime()
        {
            DateTime inRangeOffTime1 = _start;
            DateTime inRangeOffTime2 = _start.AddHours(1);
            var inRangeOffMeasure1 = new Measure(inRangeOffTime1, false);
            var inRangeOffMeasure2 = new Measure(inRangeOffTime2, false);
            TimeSpan expectedUsageTime = TimeSpan.Zero;
            var twoMeasuresArray = new Measure[2] { inRangeOffMeasure1, inRangeOffMeasure2 };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(twoMeasuresArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }

        [Fact]
        public void GivenMultipleInRangeMeasureWithOutsiders_WhenAllPossibleCombinations_ReturnsFullUsageTime()
        {
            var inRangeOnMeasure1 = new Measure(_start.AddHours(1), true);
            var inRangeOffMeasure1 = new Measure(inRangeOnMeasure1.Time.AddHours(1), false);
            var inRangeOnMeasure2 = new Measure(inRangeOnMeasure1.Time.AddHours(2), true);
            var inRangeOnMeasure3 = new Measure(inRangeOnMeasure1.Time.AddHours(3), true);
            var inRangeOffMeasure2 = new Measure(inRangeOnMeasure1.Time.AddHours(4), false);
            var inRangeOffMeasure3 = new Measure(inRangeOnMeasure1.Time.AddHours(5), false);
            var inRangeOnMeasure4 = new Measure(inRangeOnMeasure1.Time.AddHours(6), true);
            var inRangeOnMeasure5 = new Measure(inRangeOnMeasure1.Time.AddHours(7), true);
            TimeSpan expectedUsageTime = (inRangeOffMeasure1.Time - inRangeOnMeasure1.Time)
                + (inRangeOffMeasure2.Time - inRangeOnMeasure2.Time)
                + (_end - inRangeOnMeasure4.Time);
            var twoMeasuresArray = new Measure[8]
            {
                inRangeOnMeasure1,
                inRangeOffMeasure1,
                inRangeOnMeasure2,
                inRangeOnMeasure3,
                inRangeOffMeasure2,
                inRangeOffMeasure3,
                inRangeOnMeasure4,
                inRangeOnMeasure5
            };

            TimeSpan actualUsageTime = _manager.GetPumpUsageByDateTimeRange(twoMeasuresArray, _start, _end);

            Assert.Equal(expectedUsageTime, actualUsageTime);
        }
    }
}