 //Interneuron synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;

namespace Interneuron.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool Between(this DateTime date, DateTime? fromDate, DateTime? toDate)
        {
            DateTime frmDate = fromDate.HasValue ? fromDate.Value.Date : DateTime.MinValue.Date;
            DateTime tillDate = fromDate.HasValue ? fromDate.Value.Date : DateTime.MaxValue.Date;

            return date.Date >= frmDate && date.Date <= tillDate;
        }
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return date.Date.AddDays(-1 * date.Day + 1);
        }

        public static DateTime GetWeekStartDate(this DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            var wkStartDate = default(DateTime);
            switch (dayOfWeek)
            {
                case DayOfWeek.Saturday:
                    wkStartDate = date.AddDays(-5);
                   break;
                case DayOfWeek.Sunday:
                    wkStartDate = date.AddDays(-6);
                    break;
                case DayOfWeek.Monday:
                    wkStartDate = date.AddDays(-7);
                    break;
                case DayOfWeek.Tuesday:
                    wkStartDate = date.AddDays(-8);
                     break;
                case DayOfWeek.Wednesday:
                    wkStartDate = date.AddDays(-9);
                     break;
                case DayOfWeek.Thursday:
                    wkStartDate = date.AddDays(-10);
                    break;
                case DayOfWeek.Friday:
                    wkStartDate = date.AddDays(-11);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return wkStartDate;
        }

        public static DateTime GetWeekEndDate(this DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            var wkEndDate = default(DateTime);
            switch (dayOfWeek)
            {
                case DayOfWeek.Saturday:
                    wkEndDate = date.AddDays(-1);
                    break;
                case DayOfWeek.Sunday:
                    wkEndDate = date.AddDays(-2);
                    break;
                case DayOfWeek.Monday:
                    wkEndDate = date.AddDays(-3);
                    break;
                case DayOfWeek.Tuesday:
                   wkEndDate = date.AddDays(-4);
                    break;
                case DayOfWeek.Wednesday:
                    wkEndDate = date.AddDays(-5);
                    break;
                case DayOfWeek.Thursday:
                    wkEndDate = date.AddDays(-6);
                    break;
                case DayOfWeek.Friday:
                    wkEndDate = date.AddDays(-7);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return wkEndDate;
        }

        public static int GetBusinessDays(DateTime firstDay, DateTime lastDay, List<DateTime> holidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            //if (firstDay > lastDay)
            //    throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            if (holidays.IsCollectionValid())
            {
                // subtract the number of bank holidays during the time interval
                foreach (DateTime bankHoliday in holidays)
                {
                    DateTime bh = bankHoliday.Date;
                    if (firstDay <= bh && bh <= lastDay)
                        --businessDays;
                }
            }

            return businessDays;
        }

        public static string GetFormattedString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString() : string.Empty;
        }

        public static string GetFormattedString(this DateTime? dateTime, string format)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(format) : string.Empty;
        }
    }
}
