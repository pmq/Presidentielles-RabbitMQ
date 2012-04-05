using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TechDays.Extensions
{
    /// <summary>
    /// Méthodes d'extension
    /// </summary>
    public static class Extensions
    {
        #region Private Members
        /// <summary>
        /// 
        /// </summary>
        private static readonly Regex RegexHtmlTag = new Regex("<.*?>", RegexOptions.Compiled);
        /// <summary>
        /// Le 1er janvier 1970 à 0h00
        /// </summary>
        private static readonly DateTime Date1970 = new DateTime(1970, 1, 1);
        #endregion

        #region Sur l'objet string
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBase64(this string s)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(s);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimToOneSpace(this string s)
        {
            Regex r = new Regex(@"\s{2,}");
            return r.Replace(s, " ");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceAccentuedCharacters(this string s)
        {
            byte[] objBytes = System.Text.Encoding.GetEncoding(1251).GetBytes(s);
            return System.Text.Encoding.ASCII.GetString(objBytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Append(this string s, string value)
        {
            return string.Concat(s, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToLines(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return s.Split(new string[] { "\r\n", Environment.NewLine }, StringSplitOptions.None);
            }
            else
            {
                return new string[0];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string s, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlEncode(this string s)
        {
            return HttpUtility.UrlEncode(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlDecode(this string s)
        {
            return HttpUtility.UrlDecode(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HtmlStrip(this string s)
        {
            return RegexHtmlTag.Replace(s, string.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int WordCount(this string s)
        {
            return s.Split(' ').Count();
        }
        /// <summary>
        /// Teste si la chaîne est une adresse mail correcte
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            return string.Format(format, args);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            return string.Format(provider, format, args);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string Left(this string s, int position)
        {
            return s.Substring(0, position);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string Right(this string s, int position)
        {
            return s.Substring(s.Length - position);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ProperCase(this string s)
        {
            bool bFirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (var c in s)
            {
                switch (c)
                {
                    case ' ':
                    case '-':
                    case ',':
                    case '.':
                        sb.Append(c);
                        bFirst = true;
                        break;
                    default:
                        if (bFirst)
                        {
                            bFirst = false;
                            sb.Append(Char.ToUpper(c));
                        }
                        else
                        {
                            sb.Append(Char.ToLower(c));
                        }
                        break;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] ToUTF8ByteArray(this string s)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(s);
        }
        #endregion
        #region Sur tous les objets
        /// <summary>
        /// Teste l'existence d'un objet dans une collection
        /// </summary>
        /// <param name="o"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool In(this object o, IEnumerable c)
        {
            bool bRet = false;
            foreach (object ob in c)
            {
                if (ob.Equals(o))
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool In<T>(this T t, IEnumerable<T> c)
        {
            bool bRet = false;
            foreach (T item in c)
            {
                if (item.Equals(t))
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool In(this object o, params object[] items)
        {
            bool bRet = false;
            foreach (object item in items)
            {
                if (item.Equals(o))
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool In<T>(this T t, params T[] items)
        {
            bool bRet = false;
            foreach (T item in items)
            {
                if (item.Equals(t))
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool ContainsAny<T>(this T t, params T[] items) where T : ICollection<T>
        {
            bool bRet = false;
            foreach (T item in items)
            {
                if (t.Contains(item))
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }
        /// <summary>
        /// 	Determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj">The object to be compared.</param>
        /// <param name = "values">The values to compare with the object.</param>
        /// <returns></returns>
        public static bool EqualsAny<T>(this T obj, params T[] values)
        {
            return (Array.IndexOf(values, obj) != -1);
        }
        /// <summary>
        /// 	Determines whether the object is equal to none of the provided values.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj">The object to be compared.</param>
        /// <param name = "values">The values to compare with the object.</param>
        /// <returns></returns>
        public static bool EqualsNone<T>(this T obj, params T[] values)
        {
            return (obj.EqualsAny(values) == false);
        }

        /// <summary>
        /// 	Converts an object to the specified target type or returns the default value.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value">The value.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object value)
        {
            return value.ConvertTo(default(T));
        }
        /// <summary>
        /// 	Converts an object to the specified target type or returns the default value.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value">The value.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object value, T defaultValue)
        {
            if (value != null)
            {
                var targetType = typeof(T);

                if (value.GetType() == targetType) return (T)value;

                var converter = TypeDescriptor.GetConverter(value);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                        return (T)converter.ConvertTo(value, targetType);
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(value.GetType()))
                        return (T)converter.ConvertFrom(value);
                }
            }
            return defaultValue;
        }
        /// <summary>
        /// 	Converts an object to the specified target type or returns the default value. Any exceptions are optionally ignored.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value">The value.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <param name = "ignoreException">if set to <c>true</c> ignore any exception.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object value, T defaultValue, bool ignoreException)
        {
            if (ignoreException)
            {
                try
                {
                    return value.ConvertTo<T>();
                }
                catch
                {
                    return defaultValue;
                }
            }
            return value.ConvertTo<T>();
        }
        /// <summary>
        /// 	Determines whether the value can (in theory) be converted to the specified target type.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can be convert to the specified target type; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanConvertTo<T>(this object value)
        {
            if (value != null)
            {
                var targetType = typeof(T);

                var converter = TypeDescriptor.GetConverter(value);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                        return true;
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(value.GetType()))
                        return true;
                }
            }
            return false;
        }
        #endregion
        #region Sur l'objet DateTime
        #region Sur les jours
        /// <summary>
        /// 	Returns the first day of the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        /// <summary>
        /// 	Returns the first day of the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "dayOfWeek">The desired day of week.</param>
        /// <returns>The first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetFirstDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(1);
            return dt;
        }
        /// <summary>
        /// 	Returns the last day of the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The last day of the month.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
        }
        /// <summary>
        /// 	Returns the last day of the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "dayOfWeek">The desired day of week.</param>
        /// <returns>The date time</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetLastDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(-1);
            return dt;
        }
        /// <summary>
        /// 	Indicates whether the date is today.
        /// </summary>
        /// <param name = "dt">The date.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date is today; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsToday(this DateTime dt)
        {
            return (dt.Date == DateTime.Today);
        }
        #endregion
        #region Sur les heures
        /// <summary>
        /// Gets a DateTime representing midnight on the current date
        /// </summary>
        /// <param name="current">The current date</param>
        public static DateTime Midnight(this DateTime current)
        {
            DateTime midnight = new DateTime(current.Year, current.Month, current.Day);
            return midnight;
        }
        /// <summary>
        /// Gets a DateTime representing noon on the current date
        /// </summary>
        /// <param name="current">The current date</param>
        public static DateTime Noon(this DateTime current)
        {
            DateTime noon = new DateTime(current.Year, current.Month, current.Day, 12, 0, 0);
            return noon;
        }
        /// <summary>
        /// 	Sets the time on the specified DateTime value.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "hours">The hours to be set.</param>
        /// <param name = "minutes">The minutes to be set.</param>
        /// <param name = "seconds">The seconds to be set.</param>
        /// <returns>The DateTime including the new time value</returns>
        public static DateTime SetTime(this DateTime date, int hours, int minutes, int seconds)
        {
            return date.SetTime(new TimeSpan(hours, minutes, seconds));
        }
        /// <summary>
        /// 	Sets the time on the specified DateTime value.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "hours">The hours to be set.</param>
        /// <param name = "minutes">The minutes to be set.</param>
        /// <returns>The DateTime including the new time value</returns>
        public static DateTime SetTime(this DateTime date, int hours, int minutes)
        {
            return date.SetTime(new TimeSpan(hours, minutes, 0));
        }
        /// <summary>
        /// 	Sets the time on the specified DateTime value.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "time">The TimeSpan to be applied.</param>
        /// <returns>
        /// 	The DateTime including the new time value
        /// </returns>
        public static DateTime SetTime(this DateTime date, TimeSpan time)
        {
            return date.Date.Add(time);
        }
        /// <summary>
        /// Sets the time of the current date with millisecond precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        /// <param name="second">The second</param>
        /// <param name="millisecond">The millisecond</param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime current, int hour, int minute, int second, int millisecond)
        {
            DateTime atTime = new DateTime(current.Year, current.Month, current.Day, hour, minute, second, millisecond);
            return atTime;
        }
        /// <summary>
        /// 	Determines whether the time only part of two DateTime values are equal.
        /// </summary>
        /// <param name = "time">The time.</param>
        /// <param name = "timeToCompare">The time to compare.</param>
        /// <returns>
        /// 	<c>true</c> if both time values are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTimeEqual(this DateTime time, DateTime timeToCompare)
        {
            return (time.TimeOfDay == timeToCompare.TimeOfDay);
        }
        #endregion
        #region Sur les mois
        /// <summary>
        /// 	Returns the number of days in the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The number of days.</returns>
        public static int GetCountDaysOfMonth(this DateTime date)
        {
            var nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }
        #endregion
        #region Jour de la semaine
        /// <summary>
        /// 	Gets the first day of the week using the current culture.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            return date.GetFirstDayOfWeek(null);
        }
        /// <summary>
        /// 	Gets the first day of the week using the specified culture.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            while (date.DayOfWeek != firstDayOfWeek)
                date = date.AddDays(-1);

            return date;
        }
        /// <summary>
        /// 	Gets the last day of the week using the current culture.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetLastDayOfWeek(this DateTime date)
        {
            return date.GetLastDayOfWeek(null);
        }
        /// <summary>
        /// 	Gets the last day of the week using the specified culture.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetLastDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            return date.GetFirstDayOfWeek(cultureInfo).AddDays(6);
        }
        /// <summary>
        /// 	Gets the next occurence of the specified weekday within the current week using the current culture.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example>
        /// 	<code>
        /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetWeeksWeekday(this DateTime date, DayOfWeek weekday)
        {
            return date.GetWeeksWeekday(weekday, null);
        }
        /// <summary>
        /// 	Gets the next occurence of the specified weekday within the current week using the specified culture.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The calculated date.</returns>
        /// <example>
        /// 	<code>
        /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetWeeksWeekday(this DateTime date, DayOfWeek weekday, CultureInfo cultureInfo)
        {
            var firstDayOfWeek = date.GetFirstDayOfWeek(cultureInfo);
            return firstDayOfWeek.GetNextWeekday(weekday);
        }
        /// <summary>
        /// 	Gets the next occurence of the specified weekday.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example>
        /// 	<code>
        /// 		var lastMonday = DateTime.Now.GetNextWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetNextWeekday(this DateTime date, DayOfWeek weekday)
        {
            while (date.DayOfWeek != weekday)
                date = date.AddDays(1);
            return date;
        }
        /// <summary>
        /// 	Gets the previous occurence of the specified weekday.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example>
        /// 	<code>
        /// 		var lastMonday = DateTime.Now.GetPreviousWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetPreviousWeekday(this DateTime date, DayOfWeek weekday)
        {
            while (date.DayOfWeek != weekday)
                date = date.AddDays(-1);
            return date;
        }
        /// <summary>
        /// 	Determines whether the date only part of twi DateTime values are equal.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "dateToCompare">The date to compare with.</param>
        /// <returns>
        /// 	<c>true</c> if both date values are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDateEqual(this DateTime date, DateTime dateToCompare)
        {
            return (date.Date == dateToCompare.Date);
        }
        #endregion
        #region Semaine
        /// <summary>
        /// 	Indicates whether the specified date is a weekend (Saturday or Sunday).
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date is a weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek.EqualsAny(DayOfWeek.Saturday, DayOfWeek.Sunday);
        }
        /// <summary>
        /// 	Adds the specified amount of weeks (=7 days gregorian calendar) to the passed date value.
        /// </summary>
        /// <param name = "date">The origin date.</param>
        /// <param name = "value">The amount of weeks to be added.</param>
        /// <returns>The enw date value</returns>
        public static DateTime AddWeeks(this DateTime date, int value)
        {
            return date.AddDays(value * 7);
        }
        #endregion
        #region Année
        ///<summary>
        ///	Get the number of days within that year.
        ///</summary>
        ///<param name = "year">The year.</param>
        ///<returns>the number of days within that year</returns>
        /// <remarks>
        /// 	Contributed by Michael T, http://stackoverflow.com/users/190249/michael-t
        /// </remarks>
        public static int GetDays(int year)
        {
            var first = new DateTime(year, 1, 1);
            var last = new DateTime(year + 1, 1, 1);
            return GetDays(first, last);
        }
        ///<summary>
        ///	Get the number of days within that date year.
        ///</summary>
        ///<param name = "date">The date.</param>
        ///<returns>the number of days within that year</returns>
        /// <remarks>
        /// 	Contributed by Michael T, http://stackoverflow.com/users/190249/michael-t
        /// </remarks>
        public static int GetDays(this DateTime date)
        {
            return GetDays(date.Year);
        }
        ///<summary>
        ///	Get the number of days between two dates.
        ///</summary>
        ///<param name = "fromDate">The origin year.</param>
        ///<param name = "toDate">To year</param>
        ///<returns>The number of days between the two years</returns>
        /// <remarks>
        /// 	Contributed by Michael T, http://stackoverflow.com/users/190249/michael-t
        /// </remarks>
        public static int GetDays(this DateTime fromDate, DateTime toDate)
        {
            return Convert.ToInt32(toDate.Subtract(fromDate).TotalDays);
        }
        #endregion
        #region Date Julien
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double ToJulian(this DateTime dt)
        {
            int a, b, m, y;

            m = dt.Month;
            y = dt.Year;

            if (m <= 2)
            {
                y--;
                m += 12;
            }

            if ((dt.Year < 1582) || ((dt.Year == 1582) && ((dt.Month < 9) || (dt.Month == 9 && dt.Day < 5))))
                b = 0;
            else
            {
                a = y / 100;
                b = 2 - a + (a / 4);
            }

            return (((long)(365.25 * (y + 4716))) + ((int)(30.6001 * (m + 1)))
                        + dt.Day + b - 1524.5) + ((dt.Second + 60L * (dt.Minute + 60L * dt.Hour)) / 86400.0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="jj"></param>
        /// <returns></returns>
        public static DateTime FromJulian(this DateTime dt, double jj)
        {
            int jour, mois, annee, heure, minute, seconde;
            double z, f, a, alpha, b, c, d, e;
            long ij;

            jj += 0.5;
            z = Math.Floor(jj);
            f = jj - z;

            if (z < 2299161.0)
                a = z;
            else
            {
                alpha = Math.Floor((z - 1867216.25) / 36524.25);
                a = z + 1 + alpha - Math.Floor(alpha / 4);
            }

            b = a + 1524;
            c = Math.Floor((b - 122.1) / 365.25);
            d = Math.Floor(365.25 * c);
            e = Math.Floor((b - d) / 30.6001);

            jour = (int)(b - d - Math.Floor(30.6001 * e) + f);
            mois = (int)((e < 14) ? (e - 1) : (e - 13));
            annee = (int)((mois > 2) ? (c - 4716) : (c - 4715));

            ij = (long)(((jj - Math.Floor(jj)) * 86400.0) + 0.5);
            heure = (int)(ij / 3600L);
            minute = (int)((ij / 60L) % 60L);
            seconde = (int)(ij % 60L);
            return new DateTime(annee, mois, jour, heure, minute, seconde);
        }
        #endregion
        #region Convertion en temps UNIX
        /// <summary>
        /// 	Get milliseconds of UNIX area. This is the milliseconds since 1/1/1970
        /// </summary>
        /// <param name = "datetime">Up to which time.</param>
        /// <returns>number of milliseconds.</returns>
        /// <remarks>
        /// 	Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
        /// </remarks>
        public static long GetMillisecondsSince1970(this DateTime datetime)
        {
            var ts = datetime.Subtract(Date1970);
            return (long)ts.TotalMilliseconds;
        }
        #endregion
        #region UTC Offset
        ///<summary>
        ///	Return System UTC Offset
        ///</summary>
        public static double UtcOffset
        {
            get { return DateTime.Now.Subtract(DateTime.UtcNow).TotalHours; }
        }
        /// <summary>
        /// 	Converts a DateTime into a DateTimeOffset using the local system time zone.
        /// </summary>
        /// <param name = "localDateTime">The local DateTime.</param>
        /// <returns>The converted DateTimeOffset</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime)
        {
            return localDateTime.ToDateTimeOffset(null);
        }
        /// <summary>
        /// 	Converts a DateTime into a DateTimeOffset using the specified time zone.
        /// </summary>
        /// <param name = "localDateTime">The local DateTime.</param>
        /// <param name = "localTimeZone">The local time zone.</param>
        /// <returns>The converted DateTimeOffset</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime, TimeZoneInfo localTimeZone)
        {
            localTimeZone = (localTimeZone ?? TimeZoneInfo.Local);

            if (localDateTime.Kind != DateTimeKind.Unspecified)
                localDateTime = new DateTime(localDateTime.Ticks, DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, localTimeZone);
        }
        #endregion
        #region Calcul d'âge
        /// <summary>
        /// 	Calculates the age based on today.
        /// </summary>
        /// <param name = "dateOfBirth">The date of birth.</param>
        /// <returns>The calculated age.</returns>
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            return CalculateAge(dateOfBirth, DateTime.Today);
        }
        /// <summary>
        /// 	Calculates the age based on a passed reference date.
        /// </summary>
        /// <param name = "dateOfBirth">The date of birth.</param>
        /// <param name = "referenceDate">The reference date to calculate on.</param>
        /// <returns>The calculated age.</returns>
        public static int CalculateAge(this DateTime dateOfBirth, DateTime referenceDate)
        {
            var years = referenceDate.Year - dateOfBirth.Year;
            if (referenceDate.Month < dateOfBirth.Month || (referenceDate.Month == dateOfBirth.Month && referenceDate.Day < dateOfBirth.Day))
                --years;
            return years;
        }
        #endregion
        #region IsBetween
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime date, DateTime dtStart, DateTime dtEnd)
        {
            return (date.CompareTo(dtStart) >= 0) && (date.CompareTo(dtEnd) <= 0);
        }
        #endregion
        #endregion
        #region Sur l'objet DateTimeOffset
        /// <summary>
        /// 	Indicates whether the date is today.
        /// </summary>
        /// <param name = "dto">The date.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date is today; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsToday(this DateTimeOffset dto)
        {
            return (dto.Date.IsToday());
        }
        /// <summary>
        /// 	Sets the time on the specified DateTimeOffset value using the local system time zone.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "hours">The hours to be set.</param>
        /// <param name = "minutes">The minutes to be set.</param>
        /// <param name = "seconds">The seconds to be set.</param>
        /// <returns>The DateTimeOffset including the new time value</returns>
        public static DateTimeOffset SetTime(this DateTimeOffset date, int hours, int minutes, int seconds)
        {
            return date.SetTime(new TimeSpan(hours, minutes, seconds));
        }
        /// <summary>
        /// 	Sets the time on the specified DateTime value using the local system time zone.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "time">The TimeSpan to be applied.</param>
        /// <returns>
        /// 	The DateTimeOffset including the new time value
        /// </returns>
        public static DateTimeOffset SetTime(this DateTimeOffset date, TimeSpan time)
        {
            return date.SetTime(time, null);
        }
        /// <summary>
        /// 	Sets the time on the specified DateTime value using the specified time zone.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "time">The TimeSpan to be applied.</param>
        /// <param name = "localTimeZone">The local time zone.</param>
        /// <returns>/// The DateTimeOffset including the new time value/// </returns>
        public static DateTimeOffset SetTime(this DateTimeOffset date, TimeSpan time, TimeZoneInfo localTimeZone)
        {
            var localDate = date.ToLocalDateTime(localTimeZone);
            localDate.SetTime(time);
            return localDate.ToDateTimeOffset(localTimeZone);
        }
        /// <summary>
        /// 	Converts a DateTimeOffset into a DateTime using the local system time zone.
        /// </summary>
        /// <param name = "dateTimeUtc">The base DateTimeOffset.</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc)
        {
            return dateTimeUtc.ToLocalDateTime(null);
        }
        /// <summary>
        /// 	Converts a DateTimeOffset into a DateTime using the specified time zone.
        /// </summary>
        /// <param name = "dateTimeUtc">The base DateTimeOffset.</param>
        /// <param name = "localTimeZone">The time zone to be used for conversion.</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc, TimeZoneInfo localTimeZone)
        {
            localTimeZone = (localTimeZone ?? TimeZoneInfo.Local);

            return TimeZoneInfo.ConvertTime(dateTimeUtc, localTimeZone).DateTime;
        }
        #endregion
        #region Sur un dictionaire
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            if (source.ContainsKey(key))
                return source[key];
            else
                return defaultValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            TValue result;
            if (!dictionary.TryGetValue(key, out result))
            {
                result = value;
                dictionary.Add(key, result);
            }
            return result;
        }
        #endregion
        #region Sérialsation XML
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static XmlDocument ToXml(this Object obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer xs = new XmlSerializer(t);
                xs.Serialize(sw, obj);

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(sw.ToString());

                return xd;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static XElement ToXElement(this Object obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer xs = new XmlSerializer(t);
                xs.Serialize(sw, obj);

                XElement el = XElement.Parse(sw.ToString());

                return el;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXmlString(this Object obj)
        {
            StringBuilder sb = new StringBuilder();
            using (XmlTextWriter xw = new XmlTextWriter(new StringWriter(sb)))
            {
                Type t = obj.GetType();
                XmlSerializer xs = new XmlSerializer(t);
                xw.Formatting = Formatting.Indented;
                xs.Serialize(xw, obj);

                return sb.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static T FromXmlString<T>(this string xmlData)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xmlData))
            {
                Object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="el"></param>
        /// <returns></returns>
        public static T FromXElement<T>(this XElement el)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(el.ToString()))
            {
                Object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        public static T FromXml<T>(this XmlDocument x)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(x.OuterXml))
            {
                Object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }
        #endregion
        #region Sur des objets de type byte[]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string UTF8ByteArrayToString(this byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        #endregion
        #region Sur l'objet XElement
        /// <summary>
        /// Converti un XmlNode en XElement
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static XElement GetXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }
            return xDoc.Root;
        }
        /// <summary>
        /// Converti un XElement en XmlNode
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="s"></param>
        public static void AddCDATA(this XElement x, string s)
        {
            x.Add(new XCData(s));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static bool Exists(this IEnumerable<XElement> elements, string elementName)
        {
            return elements.Any(x => x.Name == elementName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool Exists(this IEnumerable<XAttribute> attributes, string attributeName)
        {
            return attributes.Any(x => x.Name == attributeName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <returns></returns>
        public static string StringValue(this XElement el, string sTag)
        {
            string s;
            try
            {
                var els = el.Element(sTag);
                if (els != null)
                {
                    s = els.Value;
                }
                else
                {
                    s = String.Empty;
                }
            }
            catch
            {
                s = String.Empty;
            }
            return s;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <param name="sDefault"></param>
        /// <returns></returns>
        public static string StringValue(this XElement el, string sTag, string sDefault)
        {
            string s;
            try
            {
                var els = el.Element(sTag);
                if (els != null)
                {
                    s = els.Value;
                }
                else
                {
                    s = sDefault;
                }
            }
            catch
            {
                s = sDefault;
            }
            return s;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <param name="sDefault"></param>
        /// <returns></returns>
        public static string StringAttValue(this XElement el, string sAtt, string sDefault)
        {
            XAttribute a;
            string s;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    s = a.Value;
                }
                else
                {
                    s = sDefault;
                }
            }
            catch
            {
                s = sDefault;
            }
            return s;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <returns></returns>
        public static string StringAttValue(this XElement el, string sAtt)
        {
            XAttribute a;
            string s;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    s = a.Value;
                }
                else
                {
                    s = String.Empty;
                }
            }
            catch
            {
                s = String.Empty;
            }
            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <param name="nbi"></param>
        /// <param name="nDefault"></param>
        /// <returns></returns>
        public static int IntValue(this XElement el, string sTag, NumberFormatInfo nbi, int nDefault)
        {
            int n;
            try
            {
                if (!Int32.TryParse(el.Element(sTag).Value, NumberStyles.Any, nbi, out n))
                {
                    n = nDefault;
                }
            }
            catch
            {
                n = nDefault;
            }
            return n;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <param name="nDefault"></param>
        /// <returns></returns>
        public static int IntValue(this XElement el, string sTag, int nDefault)
        {
            int n;
            try
            {
                XElement elt = el.Element(sTag);
                if (elt != null)
                {
                    if (!Int32.TryParse(elt.Value, out n))
                    {
                        n = nDefault;
                    }
                }
                else
                {
                    n = nDefault;
                }
            }
            catch
            {
                n = nDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <returns></returns>
        public static int IntValue(this XElement el, string sTag)
        {
            int nDefault = 0;
            int n;
            try
            {
                XElement elt = el.Element(sTag);
                if (elt != null)
                {
                    if (!Int32.TryParse(elt.Value, out n))
                    {
                        n = nDefault;
                    }
                }
                else
                {
                    n = nDefault;
                }
            }
            catch
            {
                n = nDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <param name="nDefault"></param>
        /// <returns></returns>
        public static int IntAttValue(this XElement el, string sAtt, int nDefault)
        {
            XAttribute a;
            int n;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    if (!Int32.TryParse(a.Value, out n))
                    {
                        n = nDefault;
                    }
                }
                else
                {
                    n = nDefault;
                }
            }
            catch
            {
                n = nDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <returns></returns>
        public static int IntAttValue(this XElement el, string sAtt)
        {
            int nDefault = 0;
            XAttribute a;
            int n;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    if (!Int32.TryParse(a.Value, out n))
                    {
                        n = nDefault;
                    }
                }
                else
                {
                    n = nDefault;
                }
            }
            catch
            {
                n = nDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <param name="bDefault"></param>
        /// <returns></returns>
        public static bool BoolValue(this XElement el, string sTag, bool bDefault)
        {
            bool bRet;
            try
            {
                XElement elt = el.Element(sTag);
                if (elt != null)
                {
                    if (!Boolean.TryParse(elt.Value, out bRet))
                    {
                        bRet = bDefault;
                    }
                }
                else
                {
                    bRet = bDefault;
                }
            }
            catch
            {
                bRet = bDefault;
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <returns></returns>
        public static bool BoolValue(this XElement el, string sTag)
        {
            bool bDefault = false;
            bool bRet;
            try
            {
                XElement elt = el.Element(sTag);
                if (elt != null)
                {
                    if (!Boolean.TryParse(elt.Value, out bRet))
                    {
                        bRet = bDefault;
                    }
                }
                else
                {
                    bRet = bDefault;
                }
            }
            catch
            {
                bRet = bDefault;
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <param name="bDefault"></param>
        /// <returns></returns>
        public static bool BoolAttValue(this XElement el, string sAtt, bool bDefault)
        {
            XAttribute a;
            bool bRet;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    if (!Boolean.TryParse(a.Value, out bRet))
                    {
                        bRet = bDefault;
                    }
                }
                else
                {
                    bRet = bDefault;
                }
            }
            catch
            {
                bRet = bDefault;
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <returns></returns>
        public static bool BoolAttValue(this XElement el, string sAtt)
        {
            bool bDefault = false;
            XAttribute a;
            bool bRet;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    if (!Boolean.TryParse(a.Value, out bRet))
                    {
                        bRet = bDefault;
                    }
                }
                else
                {
                    bRet = bDefault;
                }
            }
            catch
            {
                bRet = bDefault;
            }
            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <param name="fDefault"></param>
        /// <returns></returns>
        public static double DoubleValue(this XElement el, string sTag, double fDefault)
        {
            double n;
            try
            {
                XElement elt = el.Element(sTag);
                if (elt != null)
                {
                    if (!Double.TryParse(elt.Value, out n))
                    {
                        n = fDefault;
                    }
                }
                else
                {
                    n = fDefault;
                }
            }
            catch
            {
                n = fDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <param name="ci"></param>
        /// <param name="fDefault"></param>
        /// <returns></returns>
        public static double DoubleValue(this XElement el, string sTag, CultureInfo ci, double fDefault)
        {
            double n;
            try
            {
                XElement elt = el.Element(sTag);
                if (elt != null)
                {
                    if (!Double.TryParse(elt.Value, NumberStyles.Any, ci, out n))
                    {
                        n = fDefault;
                    }
                }
                else
                {
                    n = fDefault;
                }
            }
            catch
            {
                n = fDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <returns></returns>
        public static double DoubleValue(this XElement el, string sTag)
        {
            double fDefault = 0;
            double n;
            try
            {
                XElement elt = el.Element(sTag);
                if (elt != null)
                {
                    if (!Double.TryParse(elt.Value, out n))
                    {
                        n = fDefault;
                    }
                }
                else
                {
                    n = fDefault;
                }
            }
            catch
            {
                n = fDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sTag"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static double DoubleValue(this XElement el, string sTag, CultureInfo ci)
        {
            double fDefault = 0;
            double n;
            try
            {
                if (!Double.TryParse(el.Element(sTag).Value, NumberStyles.Any, ci, out n))
                {
                    n = fDefault;
                }
            }
            catch
            {
                n = fDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <param name="fDefault"></param>
        /// <returns></returns>
        public static double DoubleAttValue(this XElement el, string sAtt, double fDefault)
        {
            XAttribute a;
            double n;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    if (!Double.TryParse(a.Value, out n))
                    {
                        n = fDefault;
                    }
                }
                else
                {
                    n = fDefault;
                }
            }
            catch
            {
                n = fDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <param name="ci"></param>
        /// <param name="fDefault"></param>
        /// <returns></returns>
        public static double DoubleAttValue(this XElement el, string sAtt, CultureInfo ci, double fDefault)
        {
            XAttribute a;
            double n;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    if (!Double.TryParse(a.Value, NumberStyles.Any, ci, out n))
                    {
                        n = fDefault;
                    }
                }
                else
                {
                    n = fDefault;
                }
            }
            catch
            {
                n = fDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <returns></returns>
        public static double DoubleAttValue(this XElement el, string sAtt)
        {
            double fDefault = 0;
            XAttribute a;
            double n;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    if (!Double.TryParse(a.Value, out n))
                    {
                        n = fDefault;
                    }
                }
                else
                {
                    n = fDefault;
                }
            }
            catch
            {
                n = fDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static double DoubleAttValue(this XElement el, string sAtt, CultureInfo ci)
        {
            double fDefault = 0;
            XAttribute a;
            double n;
            try
            {
                a = el.Attribute(sAtt);
                if (a != null)
                {
                    if (!Double.TryParse(a.Value, NumberStyles.Any, ci, out n))
                    {
                        n = fDefault;
                    }
                }
                else
                {
                    n = fDefault;
                }
            }
            catch
            {
                n = fDefault;
            }
            return n;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sAtt"></param>
        /// <param name="Value"></param>
        public static void SetAttribute(this XElement el, string sAtt, object Value)
        {
            XAttribute a;
            a = el.Attribute(sAtt);
            if (a == null)
            {
                el.Add(new XAttribute(sAtt, Value));
            }
            else
            {
                a.SetValue(Value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <param name="sFilename"></param>
        public static void SaveIso_8859_1(this XElement el, string sFilename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.GetEncoding("iso-8859-1");
            using (var xw = XmlWriter.Create(sFilename, settings))
            {
                el.Save(xw);
            }
        }
        #endregion
        #region Sur un entier
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="nBit"></param>
        /// <returns></returns>
        public static bool IsBitSet(this int n, int nBit)
        {
            return (n & nBit) == nBit;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="numberToCompare"></param>
        /// <returns></returns>
        public static bool IsMultipleOf(this int i, int numberToCompare)
        {
            return i % numberToCompare == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static int LowestCommonMultiple(int lhs, int rhs)
        {
            int max = lhs > rhs ? rhs : lhs;

            int lcm = -1;
            for (int i = 2; i <= max; i++)
            {
                if (lhs.IsMultipleOf(i) && rhs.IsMultipleOf(i))
                {
                    lcm = i;
                    break;
                }
            }

            return lcm;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static int GreatestCommonDivisor(int lhs, int rhs)
        {
            int i = 0;
            while (true)
            {
                i = lhs % rhs;
                if (i == 0)
                {
                    return rhs;
                }

                lhs = rhs;
                rhs = i;
            }
        }
        #endregion
        #region Sur un double
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToFraction(this double d)
        {
            string s = d.ToString(CultureInfo.InvariantCulture);

            if (!s.Contains('.'))
            { return s; }

            int wholeNumber = int.Parse(s.Substring(0, s.IndexOf('.')));

            int length = s.Substring(s.IndexOf('.') + 1).Count();
            int numerator = int.Parse(s.Substring(s.IndexOf('.') + 1));
            int denominator = (int)System.Math.Pow((double)10.0, (double)(length));

            int gcd = GreatestCommonDivisor(numerator, denominator);

            return string.Concat(wholeNumber, ' ', numerator / gcd, '/', denominator / gcd);
        }
        #endregion

        #region Sur les enum
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string str) where T : struct
        {
            return (T)Enum.Parse(typeof(T), str);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string str, bool ignoreCase) where T : struct
        {
            return (T)Enum.Parse(typeof(T), str, ignoreCase);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
        #endregion
        #region Sur les char
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Repeat(this char c, int count)
        {
            return new string(c, count);
        }
        #endregion
        #region Sur un Stream
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T BinaryDeserialize<T>(this Stream stream)
        {
            return (T)new BinaryFormatter().Deserialize(stream);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="item"></param>
        public static void BinarySerialize<T>(this Stream stream, T item)
        {
            new BinaryFormatter().Serialize(stream, item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        public static void CopyTo(this Stream origin, Stream destination)
        {
            byte[] buffer = new byte[32768];
            while (true)
            {
                int read = origin.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                destination.Write(buffer, 0, read);
            }
        }
        /// <summary>
        /// 	Opens a StreamReader using the default encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The stream reader</returns>
        public static StreamReader GetReader(this Stream stream)
        {
            return stream.GetReader(null);
        }
        /// <summary>
        /// 	Opens a StreamReader using the specified encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <returns>The stream reader</returns>
        public static StreamReader GetReader(this Stream stream, Encoding encoding)
        {
            if (stream.CanRead == false)
                throw new InvalidOperationException("Stream does not support reading.");

            encoding = (encoding ?? Encoding.Default);
            return new StreamReader(stream, encoding);
        }
        /// <summary>
        /// 	Opens a StreamWriter using the default encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The stream writer</returns>
        public static StreamWriter GetWriter(this Stream stream)
        {
            return stream.GetWriter(null);
        }
        /// <summary>
        /// 	Opens a StreamWriter using the specified encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <returns>The stream writer</returns>
        public static StreamWriter GetWriter(this Stream stream, Encoding encoding)
        {
            if (stream.CanWrite == false)
                throw new InvalidOperationException("Stream does not support writing.");

            encoding = (encoding ?? Encoding.Default);
            return new StreamWriter(stream, encoding);
        }
        /// <summary>
        /// 	Reads all text from the stream using the default encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The result string.</returns>
        public static string ReadToEnd(this Stream stream)
        {
            return stream.ReadToEnd(null);
        }
        /// <summary>
        /// 	Reads all text from the stream using a specified encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <returns>The result string.</returns>
        public static string ReadToEnd(this Stream stream, Encoding encoding)
        {
            using (var reader = stream.GetReader(encoding))
                return reader.ReadToEnd();
        }
        /// <summary>
        /// 	Sets the stream cursor to the beginning of the stream.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The stream</returns>
        public static Stream SeekToBegin(this Stream stream)
        {
            if (stream.CanSeek == false)
                throw new InvalidOperationException("Stream does not support seeking.");

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        /// <summary>
        /// 	Sets the stream cursor to the end of the stream.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The stream</returns>
        public static Stream SeekToEnd(this Stream stream)
        {
            if (stream.CanSeek == false)
                throw new InvalidOperationException("Stream does not support seeking.");

            stream.Seek(0, SeekOrigin.End);
            return stream;
        }
        #endregion
        #region Sur une collection
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }
        #endregion
        #region Reflexion
        /// <summary>
        /// 	Dynamically invokes a method using reflection
        /// </summary>
        /// <param name = "obj">The object to perform on.</param>
        /// <param name = "methodName">The name of the method.</param>
        /// <param name = "parameters">The parameters passed to the method.</param>
        /// <returns>The return value</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static object InvokeMethod(this object obj, string methodName, params object[] parameters)
        {
            return InvokeMethod<object>(obj, methodName, parameters);
        }
        /// <summary>
        /// 	Dynamically invokes a method using reflection and returns its value in a typed manner
        /// </summary>
        /// <typeparam name = "T">The expected return data types</typeparam>
        /// <param name = "obj">The object to perform on.</param>
        /// <param name = "methodName">The name of the method.</param>
        /// <param name = "parameters">The parameters passed to the method.</param>
        /// <returns>The return value</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters)
        {
            var type = obj.GetType();
            var method = type.GetMethod(methodName);

            if (method == null)
                throw new ArgumentException(string.Format("Method '{0}' not found.", methodName), methodName);

            var value = method.Invoke(obj, parameters);
            return (value is T ? (T)value : default(T));
        }
        /// <summary>
        /// 	Dynamically retrieves a property value.
        /// </summary>
        /// <param name = "obj">The object to perform on.</param>
        /// <param name = "propertyName">The Name of the property.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return GetPropertyValue<object>(obj, propertyName, null);
        }
        /// <summary>
        /// 	Dynamically retrieves a property value.
        /// </summary>
        /// <typeparam name = "T">The expected return data type</typeparam>
        /// <param name = "obj">The object to perform on.</param>
        /// <param name = "propertyName">The Name of the property.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            return GetPropertyValue(obj, propertyName, default(T));
        }
        /// <summary>
        /// 	Dynamically retrieves a property value.
        /// </summary>
        /// <typeparam name = "T">The expected return data type</typeparam>
        /// <param name = "obj">The object to perform on.</param>
        /// <param name = "propertyName">The Name of the property.</param>
        /// <param name = "defaultValue">The default value to return.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static T GetPropertyValue<T>(this object obj, string propertyName, T defaultValue)
        {
            var type = obj.GetType();
            var property = type.GetProperty(propertyName);

            if (property == null)
                throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);

            var value = property.GetValue(obj, null);
            return (value is T ? (T)value : defaultValue);
        }
        /// <summary>
        /// 	Dynamically sets a property value.
        /// </summary>
        /// <param name = "obj">The object to perform on.</param>
        /// <param name = "propertyName">The Name of the property.</param>
        /// <param name = "value">The value to be set.</param>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            var type = obj.GetType();
            var property = type.GetProperty(propertyName);

            if (property == null)
                throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);
            if (!property.CanWrite)
                throw new ArgumentException(string.Format("Property '{0}' does not allow writes.", propertyName), propertyName);
            property.SetValue(obj, value, null);
        }
        /// <summary>
        /// 	Gets the first matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name = "T">The attribute type tp look for.</typeparam>
        /// <param name = "obj">The object to look on.</param>
        /// <returns>The found attribute</returns>
        public static T GetAttribute<T>(this object obj) where T : Attribute
        {
            return GetAttribute<T>(obj, true);
        }
        /// <summary>
        /// 	Gets the first matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name = "T">The attribute type tp look for.</typeparam>
        /// <param name = "obj">The object to look on.</param>
        /// <param name = "includeInherited">if set to <c>true</c> includes inherited attributes.</param>
        /// <returns>The found attribute</returns>
        public static T GetAttribute<T>(this object obj, bool includeInherited) where T : Attribute
        {
            var type = (obj as Type ?? obj.GetType());
            var attributes = type.GetCustomAttributes(typeof(T), includeInherited);
            if ((attributes.Length > 0))
                return (attributes[0] as T);
            return null;
        }
        /// <summary>
        /// 	Gets all matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name = "T">The attribute type tp look for.</typeparam>
        /// <param name = "obj">The object to look on.</param>
        /// <returns>The found attributes</returns>
        public static IEnumerable<T> GetAttributes<T>(this object obj) where T : Attribute
        {
            return GetAttributes<T>(obj);
        }
        /// <summary>
        /// 	Gets all matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name = "T">The attribute type tp look for.</typeparam>
        /// <param name = "obj">The object to look on.</param>
        /// <param name = "includeInherited">if set to <c>true</c> includes inherited attributes.</param>
        /// <returns>The found attributes</returns>
        public static IEnumerable<T> GetAttributes<T>(this object obj, bool includeInherited) where T : Attribute
        {
            return (obj as Type ?? obj.GetType()).GetCustomAttributes(typeof(T), includeInherited).OfType<T>().Select(attribute => attribute);
        }
        /// <summary>
        /// 	Determines whether the object is excactly of the passed generic type.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "obj">The object to check.</param>
        /// <returns>
        /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfType<T>(this object obj)
        {
            return obj.IsOfType(typeof(T));
        }
        /// <summary>
        /// 	Determines whether the object is excactly of the passed type
        /// </summary>
        /// <param name = "obj">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfType(this object obj, Type type)
        {
            return (obj.GetType().Equals(type));
        }
        /// <summary>
        /// 	Determines whether the object is of the passed generic type or inherits from it.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "obj">The object to check.</param>
        /// <returns>
        /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfTypeOrInherits<T>(this object obj)
        {
            return obj.IsOfTypeOrInherits(typeof(T));
        }
        /// <summary>
        /// 	Determines whether the object is of the passed type or inherits from it.
        /// </summary>
        /// <param name = "obj">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfTypeOrInherits(this object obj, Type type)
        {
            var objectType = obj.GetType();

            do
            {
                if (objectType.Equals(type))
                    return true;
                if ((objectType == objectType.BaseType) || (objectType.BaseType == null))
                    return false;
                objectType = objectType.BaseType;
            } while (true);
        }
        /// <summary>
        /// 	Determines whether the object is assignable to the passed generic type.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "obj">The object to check.</param>
        /// <returns>
        /// 	<c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo<T>(this object obj)
        {
            return obj.IsAssignableTo(typeof(T));
        }
        /// <summary>
        /// 	Determines whether the object is assignable to the passed type.
        /// </summary>
        /// <param name = "obj">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo(this object obj, Type type)
        {
            var objectType = obj.GetType();
            return type.IsAssignableFrom(objectType);
        }
        /// <summary>
        /// 	Gets the type default value for the underlying data type, in case of reference types: null
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value">The value.</param>
        /// <returns>The default value</returns>
        public static T GetTypeDefaultValue<T>(this T value)
        {
            return default(T);
        }
        /// <summary>
        /// 	Converts the specified value to a database value and returns DBNull.Value if the value equals its default.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value">The value.</param>
        /// <returns></returns>
        public static object ToDatabaseValue<T>(this T value)
        {
            return (value.Equals(value.GetTypeDefaultValue()) ? DBNull.Value : (object)value);
        }
        /// <summary>
        /// 	Cast an object to the given type. Usefull especially for anonymous types.
        /// </summary>
        /// <typeparam name = "T">The type to cast to</typeparam>
        /// <param name = "value">The object to case</param>
        /// <returns>
        /// 	the casted type or null if casting is not possible.
        /// </returns>
        /// <remarks>
        /// 	Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
        /// </remarks>
        public static T CastTo<T>(this object value)
        {
            return (T)value;
        }
        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <param name = "target">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// 
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <example>
        /// 	object someObject = GetSomeObject();
        /// 	if ( someObject.IsNull() ) { /* the someObject is null */ }
        /// 	else { /* the someObject is not null */ }
        /// </example>
        public static bool IsNull(this object target)
        {
            var ret = IsNull<object>(target);
            return ret;
        }
        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <typeparam name = "T">Type of target.</typeparam>
        /// <param name = "target">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// 
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <example>
        /// 	MyClass someObject = GetSomeObject();
        /// 	if ( someObject.IsNull() ) { /* the someObject is null */ }
        /// 	else { /* the someObject is not null */ }
        /// </example>
        public static bool IsNull<T>(this T target)
        {
            var result = ReferenceEquals(target, null);
            return result;
        }
        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <param name = "target">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// 
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <example>
        /// 	object someObject = GetSomeObject();
        /// 	if ( someObject.IsNotNull() ) { /* the someObject is not null */ }
        /// 	else { /* the someObject is null */ }
        /// </example>
        public static bool IsNotNull(this object target)
        {
            var ret = IsNotNull<object>(target);
            return ret;
        }
        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <typeparam name = "T">Type of target.</typeparam>
        /// <param name = "target">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// 
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <example>
        /// 	MyClass someObject = GetSomeObject();
        /// 	if ( someObject.IsNotNull() ) { /* the someObject is not null */ }
        /// 	else { /* the someObject is null */ }
        /// </example>
        public static bool IsNotNull<T>(this T target)
        {
            var result = !ReferenceEquals(target, null);
            return result;
        }
        /// <summary>
        /// 	If target is null, returns null.
        /// 	Othervise returns string representation of target using current culture format provider.
        /// </summary>
        /// <param name = "target">Target transforming to string representation. Can be null.</param>
        /// <example>
        /// 	float? number = null;
        /// 	string text1 = number.AsString();
        /// 
        /// 	number = 15.7892;
        /// 	string text2 = number.AsString();
        /// </example>
        /// <remarks>
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static string AsString(this object target)
        {
            return ReferenceEquals(target, null) ? null : string.Format("{0}", target);
        }
        /// <summary>
        /// 	If target is null, returns null.
        /// 	Othervise returns string representation of target using specified format provider.
        /// </summary>
        /// <param name = "target">Target transforming to string representation. Can be null.</param>
        /// <param name = "formatProvider">Format provider used to transformation target to string representation.</param>
        /// <example>
        /// 	CultureInfo czech = new CultureInfo("cs-CZ");
        /// 
        /// 	float? number = null;
        /// 	string text1 = number.AsString( czech );
        /// 
        /// 	number = 15.7892;
        /// 	string text2 = number.AsString( czech );
        /// </example>
        /// <remarks>
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static string AsString(this object target, IFormatProvider formatProvider)
        {
            var result = string.Format(formatProvider, "{0}", target);
            return result;
        }
        /// <summary>
        /// 	If target is null, returns null.
        /// 	Othervise returns string representation of target using invariant format provider.
        /// </summary>
        /// <param name = "target">Target transforming to string representation. Can be null.</param>
        /// <example>
        /// 	float? number = null;
        /// 	string text1 = number.AsInvariantString();
        /// 
        /// 	number = 15.7892;
        /// 	string text2 = number.AsInvariantString();
        /// </example>
        /// <remarks>
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static string AsInvariantString(this object target)
        {
            var result = string.Format(CultureInfo.InvariantCulture, "{0}", target);
            return result;
        }
        /// <summary>
        /// 	If target is null reference, returns notNullValue.
        /// 	Othervise returns target.
        /// </summary>
        /// <typeparam name = "T">Type of target.</typeparam>
        /// <param name = "target">Target which is maybe null. Can be null.</param>
        /// <param name = "notNullValue">Value used instead of null.</param>
        /// <example>
        /// 	const int DEFAULT_NUMBER = 123;
        /// 
        /// 	int? number = null;
        /// 	int notNullNumber1 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 123
        /// 
        /// 	number = 57;
        /// 	int notNullNumber2 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 57
        /// </example>
        /// <remarks>
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static T NotNull<T>(this T target, T notNullValue)
        {
            return ReferenceEquals(target, null) ? notNullValue : target;
        }
        /// <summary>
        /// 	If target is null reference, returns result from notNullValueProvider.
        /// 	Othervise returns target.
        /// </summary>
        /// <typeparam name = "T">Type of target.</typeparam>
        /// <param name = "target">Target which is maybe null. Can be null.</param>
        /// <param name = "notNullValueProvider">Delegate which return value is used instead of null.</param>
        /// <example>
        /// 	int? number = null;
        /// 	int notNullNumber1 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns random number from 10 to 20
        /// 
        /// 	number = 57;
        /// 	int notNullNumber2 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns 57
        /// </example>
        /// <remarks>
        /// 	Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static T NotNull<T>(this T target, Func<T> notNullValueProvider)
        {
            return ReferenceEquals(target, null) ? notNullValueProvider() : target;
        }
        #endregion


    }
}
