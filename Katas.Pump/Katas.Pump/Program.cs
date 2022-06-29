using Katas.Pump;

var manager = new PumpManager();

var m1 = new Measure(new DateTime(2022, 1, 20, 8, 0, 2), false);
var m2 = new Measure(new DateTime(2022, 1, 20, 11, 54, 48), true); // T
var m3 = new Measure(new DateTime(2022, 1, 20, 8, 30, 56), true); // T
var m4 = new Measure(new DateTime(2022, 1, 20, 7, 40, 5), true); // T
var m5 = new Measure(new DateTime(2022, 1, 20, 10, 29, 27), true); // T
var m6 = new Measure(new DateTime(2022, 1, 20, 8, 24, 30), false);
var m7 = new Measure(new DateTime(2022, 1, 20, 5, 34, 12), true);
var m8 = new Measure(new DateTime(2022, 1, 20, 13, 28, 56), true);

var measures = new Measure[] { m1, m2, m3, m4, m5, m6, m7, m8 };

var start = new DateTime(2022, 1, 20, 6, 0, 0);
var end = new DateTime(2022, 1, 20, 12, 0, 0);

TimeSpan totalUsageTime = manager.GetPumpUsageByDateTimeRange(measures, start, end);

Console.WriteLine($"Total usage time : {totalUsageTime}");
Console.WriteLine($"Total usage time : {(m1.Time - m4.Time) + (m5.Time - m3.Time) + (m2.Time - m5.Time) + (end - m2.Time)}");
