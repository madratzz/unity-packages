using System;
using System.Globalization;

namespace CustomUtilities
{
    public static class Utilities
    {
        public static DateTime TryParseDateTime(int epochTime)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, 0); //from start epoch time
            start = start.AddSeconds(epochTime); //add the seconds to the start DateTime
            return start;
        }

        public static int ConvertDateToEpoch(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)timeSpan.TotalSeconds;
            return secondsSinceEpoch;
        }

        public static string TimeFromSeconds(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            return time.ToString(@"mm\:ss");
        }

        public static double ToDouble(string val)
        {
            return val.Length == 0 || val == "-" ? 0.0f : double.Parse(val, CultureInfo.InvariantCulture);
        }

        public static float ToFloat(string val)
        {
            return val.Length == 0 || val == "-" ? 0f : float.Parse(val, CultureInfo.InvariantCulture);
        }

        public static int ToInt(string val)
        {
            return val.Length == 0 || val == "-" || string.IsNullOrEmpty(val) ? 0 : int.Parse(val.Split('.')[0], CultureInfo.InvariantCulture);
        }

        public static bool ToBool(string booleanString)
        {
            booleanString = booleanString.ToUpper();
            switch (booleanString)
            {
                case "TRUE":
                    return true;
                case "FALSE":
                    return false;
                case "1":
                    return true;
                case "0":
                    return false;
                default:
                    return false;
            }
        }
    }
}
