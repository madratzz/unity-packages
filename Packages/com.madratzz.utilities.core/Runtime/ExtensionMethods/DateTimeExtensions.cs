using System;

namespace ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static bool IsInRange(this DateTime currentDate, DateTime beginDate, DateTime endDate)
        {
            return ( currentDate >= beginDate && currentDate <= endDate );
        }

        public static bool IsLessThan(this DateTime currentDate, DateTime referenceDate)
        {
            return (currentDate <= referenceDate);
        }

        public static bool IsGreaterThan(this DateTime currentDate, DateTime referenceDate)
        {
            return ( currentDate >= referenceDate);
        }

        // Example
        //     var monday = DateTime.Now.ThisWeekMonday();
        // var friday = DateTime.Now.ThisWeekFriday();
        //
        // If (DateTime.Now.IsInRange(monday, friday) {
        //     ...do something...
        // }

        public static DateTime? ToDateTime(this string s)
        {
            DateTime dtr;
            var tryDtr = DateTime.TryParse(s, out dtr);
            return (tryDtr) ? dtr : new DateTime?();
        }

        //     Example
        // you can test the result like this:
        //         var dt = TextBox1.Text.ToDateTime();
        //     if (dt == null) {
        //         throw new Exception("Your datetime was invalid");
        //     }
        //     else {
        // this method takes a normal/non-nullable DateTime
        // as its parameter.
        //         DoSomething(dt.Value);
        //     }

        /// <summary>
        /// Get the elapsed time since the input DateTime
        /// </summary>
        /// <param name="input">Input DateTime</param>
        /// <returns>Returns a TimeSpan value with the elapsed time since the input DateTime</returns>
        /// <example>
        /// TimeSpan elapsed = dtStart.Elapsed();
        /// </example>
        /// <seealso cref="ElapsedSeconds()"/>
        public static TimeSpan Elapsed(this DateTime input)
        {
            return DateTime.Now.Subtract(input);
        }

        public static DateTime AddTime(this DateTime date, int hour, int minutes)
        {
            return date + new TimeSpan(hour, minutes, 0);
        }

        public static TimeSpan TimeElapsed(this DateTime date)
        {
            return DateTime.Now - date;
        }

        public static string LengthOfTimeShort(this DateTime date)
        {
            TimeSpan lengthOfTime = DateTime.Now.Subtract(date);
            if (lengthOfTime.Minutes == 0)
                return lengthOfTime.Seconds.ToString() + "s";
            else if (lengthOfTime.Hours == 0)
                return lengthOfTime.Minutes.ToString() + "m";
            else if (lengthOfTime.Days == 0)
                return lengthOfTime.Hours.ToString() + "h";
            else
                return lengthOfTime.Days.ToString() + "d";
        }

        public static string LengthOfTimeTillShort(this DateTime date)
        {
            TimeSpan lengthOfTime = date.Subtract(DateTime.Now);//DateTime.Now.Subtract(date);
            if (lengthOfTime.Minutes == 0)
                return lengthOfTime.Seconds.ToString() + "s";
            else if (lengthOfTime.Hours == 0)
                return lengthOfTime.Minutes.ToString() + "m";
            else if (lengthOfTime.Days == 0)
                return lengthOfTime.Hours.ToString() + "h";
            else
                return lengthOfTime.Days.ToString() + "d";
        }

        public static string LengthOfTimeTill(this DateTime date)
        {
            TimeSpan lengthOfTime = date.Subtract(DateTime.Now);//DateTime.Now.Subtract(date);
            string timeString="";


            if (lengthOfTime.Days > 0)
            {
                //timeString = lengthOfTime.ToString(@"dd\.hh\:mm");
                //timeString = $"D{lengthOfTime.Days} {lengthOfTime.Hours}:{lengthOfTime.Minutes}";
                timeString = $"{lengthOfTime.Days}D {lengthOfTime.Hours}H:{lengthOfTime.Minutes}M";
            }
            else if(lengthOfTime.Days<=0)
            {
                //timeString = lengthOfTime.ToString(@"hh\:mm\:ss");
                // timeString = $"{lengthOfTime.Hours}:{lengthOfTime.Minutes}:{lengthOfTime.Seconds}";
                timeString = $"{lengthOfTime.Hours:D2}:{lengthOfTime.Minutes:D2}:{lengthOfTime.Seconds:D2}";
                //timeString = $"{lengthOfTime.Hours}: {lengthOfTime.Minutes}:{lengthOfTime.Seconds}";

            }
            return timeString;
        }

        #region DateTime Floor, Mid, Ceiling

        static public DateTime DateTimeFloor(this DateTime dt, TimeInterval Interval)
        {
            return WorkMethod(dt, 0L, Interval);
        }
        static public DateTime DateTimeMidpoint(this DateTime dt, TimeInterval Interval)
        {
            return WorkMethod(dt, 2L, Interval);
        }
        static public DateTime DateTimeCeiling(this DateTime dt, TimeInterval Interval)
        {
            return WorkMethod(dt, 1L, Interval);
        }
        static public DateTime DateTimeCeilingUnbounded(this DateTime dt, TimeInterval Interval)
        {
            return WorkMethod(dt, 1L, Interval).AddTicks(-1);
        }
        static public DateTime DateTimeRound(this DateTime dt, TimeInterval Interval)
        {
            if (dt >= WorkMethod(dt, 2L, Interval))
                return WorkMethod(dt, 1L, Interval);
            else
                return WorkMethod(dt, 0L, Interval);
        }
        public enum TimeInterval : long
        {
            YearFromJanuary = 120L,
            YearFromFebruary = 121L,
            YearFromMarch = 122L,
            YearFromApril = 123L,
            YearFromMay = 124L,
            YearFromJune = 125L,
            YearFromJuly = 126L,
            YearFromAugust = 127L,
            YearFromSeptember = 128L,
            YearFromOctober = 129L,
            YearFromNovember = 130L,
            YearFromDecember = 131L,
            HalfYearFromJanuary = 60L,
            HalfYearFromFebruary = 61L,
            HalfYearFromMarch = 62L,
            HalfYearFromApril = 63L,
            HalfYearFromMay = 64L,
            HalfYearFromJune = 65L,
            QuarterYearFromJanuary = 30L,
            QuarterYearFromFebruary = 31L,
            QuarterYearFromMarch = 32L,
            BiMonthlyFromJanuary = 20L,
            BiMonthlyFromFebruary = 21L,
            OneMonth = 10L,
            OneWeekFromSunday = 1L,
            OneWeekFromMonday = 2L,
            OneWeekFromTuesday = 3L,
            OneWeekFromWednesday = 4L,
            OneWeekFromThursday = 5L,
            OneWeekFromFriday = 6L,
            OneWeekFromSaturday = 7L,
            OneDay = TimeSpan.TicksPerDay,
            TwelveHour = TimeSpan.TicksPerHour * 12L,
            SixHour = TimeSpan.TicksPerHour * 6L,
            ThreeHour = TimeSpan.TicksPerHour * 3L,
            OneHour = TimeSpan.TicksPerHour,
            HalfHour = TimeSpan.TicksPerMinute * 30L,
            QuarterHour = TimeSpan.TicksPerMinute * 15L,
            OneMinute = TimeSpan.TicksPerMinute,
            HalfMinute = TimeSpan.TicksPerSecond * 30L,
            QuarterMinute = TimeSpan.TicksPerSecond * 15L,
            OneSecond = TimeSpan.TicksPerSecond,
            TenthOfASecond = TimeSpan.TicksPerSecond / 10L,
            HundrethOfASecond = TimeSpan.TicksPerSecond / 100L,
            ThousandthOfASecond = TimeSpan.TicksPerSecond / 1000L
        }
        static private DateTime WorkMethod(DateTime dt, long ReturnType, TimeInterval Interval)
        {
            long Interval1 = (long)Interval;
            long TicksFromFloor = 0L;
            int IntervalFloor = 0;
            int FloorOffset = 0;
            int IntervalLength = 0;
            DateTime floorDate;
            DateTime ceilingDate;

            if (Interval1 > 132L) //Set variables to calculate date for time interval less than one day.
            {
                floorDate = new DateTime(dt.Ticks - (dt.Ticks % Interval1), dt.Kind);
                if (ReturnType != 0L)
                    TicksFromFloor = Interval1 / ReturnType;
            }
            else if (Interval1 < 8L) //Set variables to calculate date for time interval of one week.
            {
                IntervalFloor = (int)(Interval1) - 1;
                FloorOffset = (int)dt.DayOfWeek * -1;
                floorDate = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Kind).AddDays(-(IntervalFloor > FloorOffset ? FloorOffset + 7 - IntervalFloor : FloorOffset - IntervalFloor));
                if (ReturnType != 0L)
                    TicksFromFloor = TimeSpan.TicksPerDay * 7L / ReturnType;
            }
            else //Set variables to calculate date for time interval one month or greater.
            {
                IntervalLength = Interval1 >= 130L ? 12 : (int)(Interval1 / 10L);
                IntervalFloor = (int)(Interval1 % IntervalLength);
                FloorOffset = (dt.Month - 1) % IntervalLength;
                floorDate = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, dt.Kind).AddMonths(-(IntervalFloor > FloorOffset ? FloorOffset + IntervalLength - IntervalFloor : FloorOffset - IntervalFloor));
                if (ReturnType != 0L)
                {
                    ceilingDate = floorDate.AddMonths(IntervalLength);
                    TicksFromFloor = (long)ceilingDate.Subtract(floorDate).Ticks / ReturnType;
                }
            }
            return floorDate.AddTicks(TicksFromFloor);
        }
        #endregion

        public static int CompareWithoutMinutes(this DateTime source, DateTime toCompare)
        {
            source = new DateTime(source.Year, source.Month, source.Day, source.Hour, 0, 0);
            toCompare = new DateTime(toCompare.Year, toCompare.Month, toCompare.Day, toCompare.Hour, 0, 0);

            return source.CompareTo(toCompare);
        }

        /// <summary>
        /// Gest the elapsed seconds since the input DateTime
        /// </summary>
        /// <param name="input">Input DateTime</param>
        /// <returns>Returns a Double value with the elapsed seconds since the input DateTime</returns>
        /// <example>
        /// Double elapsed = dtStart.ElapsedSeconds();
        /// </example>
        /// <seealso cref="Elapsed()"/>
        public static Double ElapsedSeconds(this DateTime input)
        {
            return DateTime.Now.Subtract(input).TotalSeconds;
        }

        public static int ToEpoch(this DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)timeSpan.TotalSeconds;
            //Console.WriteLine(secondsSinceEpoch);
            return secondsSinceEpoch;
        }

        public static DateTime ToDateTime(int epochTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epochTime);
            DateTime dateTime = dateTimeOffset.UtcDateTime;
//            Debug.LogError("Time"+dateTime);
            return dateTime;
        }
        
        public static string GetRemainingTime(this DateTime date, DateTime endTime)
        {
            TimeSpan lengthOfTime = endTime.Subtract(DateTime.Now);
            string timeString="";


            if (lengthOfTime.Days > 0)
            {
                timeString = $"{lengthOfTime.Days}D {lengthOfTime.Hours}H";//:{lengthOfTime.Minutes}M";
            }
            else if(lengthOfTime.Days<=0 && !(lengthOfTime.Hours<=0))
            {
                timeString = $"{lengthOfTime.Hours}H {lengthOfTime.Minutes}M";//:{lengthOfTime.Seconds}S";
            }
            else if (lengthOfTime.Hours<=0)
            {
                timeString = $"{lengthOfTime.Minutes}M {lengthOfTime.Seconds}S";
            }
            else if (lengthOfTime.Seconds<=0 && lengthOfTime.Minutes<=0 && lengthOfTime.Hours<=0)
            {
                timeString = $"{00}M {00}S";
            }
            return timeString;
        }

        public static DateTime  TimeTillMidNight(int epochTime)
        {
            DateTime now = ToDateTime(epochTime);
            // Calculate the next midnight
            DateTime nextMidnight = now.Date.AddDays(1);
            // Calculate the end of the current day (11:59:59 PM)
            DateTime endOfDay = nextMidnight.AddSeconds(-1);

            return endOfDay;
        }

    }
}
