using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDayCounting
{
    public class BusinessDayCounter
    {
        private static List<DayOfWeek> weekEnds = new List<DayOfWeek>(new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday });

        /// <summary>
        /// TASK ONE:
        /// Calculates the number of weekdays in between two dates.
        /// </summary>
        /// <remarks>
        /// Weekdays are Monday, Tuesday, Wednesday, Thursday, Friday.
        /// The returned count should not include either firstDate or secondDate - e.g. between Monday 07-Oct-2013 and Wednesday 09-Oct-2013 is one weekday.
        /// If secondDate is equal to or before firstDate, return 0.
        /// </remarks>
        /// <param name="firstDate">The first date.</param>
        /// <param name="secondDate">The second date.</param>
        /// <returns>Number of weekdays</returns>
        public static int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            int numWeekDays = 0;

            if (firstDate.AddDays(1) >= secondDate) return 0;
            
            firstDate = firstDate.AddDays(1);

            while (!firstDate.DayOfWeek.Equals(DayOfWeek.Monday))
            {
                if (!weekEnds.Contains(firstDate.DayOfWeek))
                {
                    numWeekDays++;
                }
                firstDate = firstDate.AddDays(1);
            }
			
            while (!secondDate.DayOfWeek.Equals(DayOfWeek.Monday))
            {
                if (!weekEnds.Contains(secondDate.DayOfWeek))
                {
                    numWeekDays++;
                }
                secondDate = secondDate.AddDays(-1);
            }
            TimeSpan span = secondDate - firstDate;
            int weeks = span.Days / 7;

            return weeks*5 + numWeekDays;
        }

        /// <summary>
        /// TASK TWO:
        /// Calculates the number of business days in between two dates.
        /// </summary>
        /// <remarks>
        /// Business days are Monday, Tuesday, Wednesday, Thursday, Friday, but excluding any dates which appear in the supplied list of public holidays.
        /// The returned count should not include either firstDate or secondDate - e.g. between Monday 07-Oct-2013 and Wednesday 09-Oct-2013 is one weekday.
        /// If secondDate is equal to or before firstDate, return 0.
        /// </remarks>
        /// <param name="firstDate">The first date.</param>
        /// <param name="secondDate">The second date.</param>
        /// <param name="publicHolidays">List of public holidays.</param>
        /// <returns>Number of business days</returns>
        public static int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays)
        {
            int weekDays = WeekdaysBetweenTwoDates(firstDate, secondDate);

            foreach(var holiday in publicHolidays)
            {
                if (!weekEnds.Contains(holiday.DayOfWeek) && holiday > firstDate && holiday < secondDate)
                    weekDays--;
            }

            return weekDays;
        }

        /// <summary>
        /// TASK THREE:
        /// Calculates the number of business days in between two dates. Same as part 2 but now takes a list of custom PublicHoliday definition rather than a list of DateTimes
        /// </summary>
        /// <remarks>
        /// Business days are Monday, Tuesday, Wednesday, Thursday, Friday, but excluding any dates which appear in the supplied list of public holidays.
        /// The returned count should not include either firstDate or secondDate - e.g. between Monday 07-Oct-2013 and Wednesday 09-Oct-2013 is one weekday.
        /// If secondDate is equal to or before firstDate, return 0.
        /// </remarks>
        /// <param name="firstDate">The first date.</param>
        /// <param name="secondDate">The second date.</param>
        /// <param name="publicHolidays">List of public holidays.</param>
        /// <returns>Number of business days</returns>
        public static int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<PublicHoliday> publicHolidays)
        {
            //Simply call the above function to count all the weekdays, 
            //then for each public holiday, count-- if the holiday is in the interval AND on a weekday
            int weekDays = WeekdaysBetweenTwoDates(firstDate, secondDate);

            //Almost identical to the DateTime version except for an extra wrapper to turn each PublicHoliday definition into a list of DateTimes.
            //While it might appear O(n^2), this is still O(n), where n is the number of generated dates. The outer loop is only required because 
            //of the more efficient mechanism of storing public holiday dates. This makes it O(n) where n is the number of resultant public holidays inside the interval
            foreach (var holiday in publicHolidays)
            {
                foreach (var day in holiday.GetDatesInInterval(firstDate, secondDate)){
                    if (!weekEnds.Contains(day.DayOfWeek))
                        weekDays--;
                }
            }

            return weekDays;
        }
    }
}
