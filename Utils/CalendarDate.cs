using System;

namespace JewishCalendar.Utils
{
    public class CalendarDate
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public CalendarDate(int day, int month, int year)
        {
            Day = day; 
            Month = month; 
            Year = year;
        }

        public bool AreDatesEqual(CalendarDate date)
        {
            if ((Day == date.Day) &&
                (Month == date.Month) &&
                (Year == date.Year))
                return true;
            else
                return false;
        }

        public int GetHashCode()
        {
            return (Year - 1583) * 366 + Month * 31 + Day;
        }

        public String ToString()
        {
            return String.Format("{0}.{1}.{2}", Day, Month, Year);
        } 
    }
}
