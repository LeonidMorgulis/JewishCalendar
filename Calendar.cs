using System.Collections.Generic;

using JewishCalendar.Utils;
using JewishCalendar.Engine;
using System;

namespace JewishCalendar
{
    public class Calendar
    {
        /// <summary>
        /// Returns JahrZeit in a jewish year (from gregorian date)
        /// </summary>
        /// <param name="gregorianDateOfDeath">Gregorian date of death</param>
        /// <param name="hebYear">Jewish year</param>
        /// <returns>CalendarDate object</returns>
        public CalendarDate GetJahrZeitFromGregorianDateInJewishYear(DateTime gregorianDateOfDeath, int hebYear)
        {
            CalendarDate hebrDateOfDeath = GetHebrewDateFromGregorianDate(gregorianDateOfDeath);
            return GetJahrZeitFromHebrewDateInJewishYear(hebrDateOfDeath, hebYear);
        }

        /// <summary>
        /// Returns JahrZeit in a jewish year (from hebrew date)
        /// </summary>
        /// <param name="hebrewDateOfDeath">Hebrew date of death</param>
        /// <param name="targetYear">Jewish year</param>
        /// <returns>CalendarDate object</returns>
        public CalendarDate GetJahrZeitFromHebrewDateInJewishYear(CalendarDate hebrewDateOfDeath, int targetYear)
        {

            CalendarDate hebrDate = new CalendarDate(hebrewDateOfDeath.Day, hebrewDateOfDeath.Month, hebrewDateOfDeath.Year);

            if (!IsLeapYear(targetYear) && hebrewDateOfDeath.Month == 13)
            {
                hebrDate = new CalendarDate(hebrewDateOfDeath.Day, 12, targetYear);
            }
            else
            {
                hebrDate = hebrewDateOfDeath;
            }
            return GetGregorianDateFromHebrewDate(hebrDate);
        }

        /// <summary>
        /// Returns true if the jewish year is a leap year
        /// </summary>
        /// <param name="hebYear">Jewish year</param>
        /// <returns>True or false</returns>
        public bool IsLeapYear(int hebYear)
        {
            CalendarEngine engine = new CalendarEngine();
            return engine.IsHebrewLeapYear(hebYear);
        }

        /// <summary>
        /// Converts a gregorian date into a hebrew date
        /// </summary>
        /// <param name="gregorianDate">Gregorian date</param>
        /// <returns>CalendarDate object</returns>
        public CalendarDate GetHebrewDateFromGregorianDate(DateTime gregorianDate)
        {
            CalendarDate calDate = new CalendarDate(gregorianDate.Day, gregorianDate.Month, gregorianDate.Year);
            CalendarEngine engine = new CalendarEngine();
            int absolute = engine.absoluteFromGregorianDate(calDate);
            CalendarDate hebrewDate = engine.jewishDateFromAbsolute(absolute);
            return hebrewDate;
        }

        /// <summary>
        /// Converts a hebrew date into a gregorian date
        /// </summary>
        /// <param name="hebrewDate">Hebrew date</param>
        /// <returns>CalendarDate object</returns>
        public CalendarDate GetGregorianDateFromHebrewDate(CalendarDate hebrewDate)
        {
            CalendarEngine engine = new CalendarEngine();
            int absolute = engine.absoluteFromJewishDate(hebrewDate);
            CalendarDate gregorianDate = engine.gregorianDateFromAbsolute(absolute);
            return gregorianDate;
        }

        /// <summary>
        /// Returns a weekday from absolute date
        /// </summary>
        /// <param name="absDate">Absolute date</param>
        /// <returns>Weekday</returns>
        public int GetWeekday(int absDate)
        {
            CalendarEngine engine = new CalendarEngine();
            return engine.GetWeekday(absDate);
        }

        public CalendarTime GetSunrise(int month, int day, int year, Location location)
        {
            AstronomicalCalculations astroCalc = new AstronomicalCalculations();
            return astroCalc.GetSunrise(month, day, year, location);
        }

        public int GetWeekdayOfHebrewDate(int hebDay, int hebMonth, int hebYear)
        {
            CalendarEngine engine = new CalendarEngine();
            HolidayCalculation holCalc = new HolidayCalculation();
            return  holCalc.GetWeekdayOfHebrewDate(hebDay, hebMonth, hebYear, engine);
        }

        public bool IsIsraeliDaylightSavingsTime(CalendarDate hebrewDate)
        {
            CalendarEngine engine = new CalendarEngine();
            HolidayCalculation holCalc = new HolidayCalculation();
            return holCalc.IsIsraeliDaylightSavingsTime(hebrewDate, engine);
        }

        public List<string> GetJewishHolidaysForHebrewDate(CalendarDate gdate, bool diaspora)
        {
            CalendarEngine engine = new CalendarEngine();
            HolidayCalculation holCalc = new HolidayCalculation();
            return holCalc.GetHolidayForDate(gdate, engine, diaspora);
        }

        public string GetJewishMonthName(int month, int year)
        {
            CalendarEngine engine = new CalendarEngine();
            return engine.getJewishMonthName(month, year);
        }
    }
}
