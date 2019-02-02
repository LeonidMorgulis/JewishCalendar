using System;
using System.Collections.Generic;

using JewishCalendar.Engine;
using JewishCalendar.Utils;

namespace JewishCalendar.Engine
{
    public class HolidayCalculation
    {
        public bool IsIsraeliDaylightSavingsTime(CalendarDate date, CalendarEngine engine)
        {
            // Get Jewish year of Yom Kippur in the passed Gregorian year
            int a = engine.absoluteFromGregorianDate(new CalendarDate(31, 12, date.Year));
            CalendarDate jewishCurrent = engine.jewishDateFromAbsolute(a);

            // Get Last Friday before 2nd of April
            int dstBegin = engine.absoluteFromGregorianDate(new CalendarDate(2, 4, date.Year)); // 2 April
            dstBegin--; // get the day before 2nd of April
            while (engine.GetWeekday(dstBegin) != 5) // gets the weekday, 5 = Friday
                dstBegin--; // counts to the previous day until Friday

            // Get Sunday between Rosh Hashana and Yom Kippur
            // Take the first Sunday on or after 3rd of Tishri
            int dstEnd = engine.absoluteFromJewishDate(new CalendarDate(3, 7, jewishCurrent.Year));
            while (engine.GetWeekday(dstEnd) != 0) // gets the weekday, 0 = Sunday
                dstEnd++; // counts to the next day until Sunday

            // Check if the current date is between the start and end date ...
            int currentDate = engine.absoluteFromGregorianDate(date);
            if (currentDate >= dstBegin && currentDate < dstEnd)
                return true;
            else
                return false;
        }

        public int GetWeekdayOfHebrewDate(int hebDay, int hebMonth, int hebYear, CalendarEngine engine)
        {
            int absDate = engine.absoluteFromJewishDate(new CalendarDate(hebDay, hebMonth, hebYear));
            return absDate % 7;
        }

        public List<String> GetHolidayForDate(CalendarDate gdate, CalendarEngine engine, bool diaspora)
        {
            int absDate = engine.absoluteFromGregorianDate(gdate);
            CalendarDate jewishDate = engine.jewishDateFromAbsolute(absDate);
            int hebDay = jewishDate.Day;
            int hebMonth = jewishDate.Month;
            int hebYear = jewishDate.Year;

            List<String> listHolidays = new List<String>();

            // Holidays in Nisan

            int hagadolDay = 14;
            while (GetWeekdayOfHebrewDate(hagadolDay, 1, hebYear, engine) != 6)
                hagadolDay -= 1;
            if (hebDay == hagadolDay && hebMonth == 1)
                listHolidays.Add("Shabat Hagadol");

            if (hebDay == 14 && hebMonth == 1)
                listHolidays.Add("Erev Pesach");
            if (hebDay == 15 && hebMonth == 1)
                listHolidays.Add("Pesach I");
            if (hebDay == 16 && hebMonth == 1)
            {
                if (diaspora)
                {
                    listHolidays.Add("Pesach II");
                }
                else
                {
                    listHolidays.Add("Chol Hamoed");
                }
            }
            if (hebDay == 17 && hebMonth == 1)
                listHolidays.Add("Chol Hamoed");
            if (hebDay == 18 && hebMonth == 1)
                listHolidays.Add("Chol Hamoed");
            if (hebDay == 19 && hebMonth == 1)
                listHolidays.Add("Chol Hamoed");
            if (hebDay == 20 && hebMonth == 1)
                listHolidays.Add("Chol Hamoed");
            if (hebDay == 21 && hebMonth == 1)
            {
                if (!diaspora)
                    listHolidays.Add("Pesach VII (Yizkor)");
                else
                    listHolidays.Add("Pesach VII");
            }
            if (hebDay == 22 && hebMonth == 1)
            {
                if (diaspora)
                    listHolidays.Add("Pesach VIII (Yizkor)");
            }

            // Yom Hashoah

            if (GetWeekdayOfHebrewDate(27, 1, hebYear, engine) == 5)
            {
                if (hebDay == 26 && hebMonth == 1)
                    listHolidays.Add("Yom Hashoah");
            }
            else if (hebYear >= 5757 && GetWeekdayOfHebrewDate(27, 1, hebYear, engine) == 0)
            {
                if (hebDay == 28 && hebMonth == 1)
                    listHolidays.Add("Yom Hashoah");
            }
            else
            {
                if (hebDay == 27 && hebMonth == 1)
                    listHolidays.Add("Yom Hashoah");
            }

            // Holidays in Iyar

            // Yom Hazikaron

            if (GetWeekdayOfHebrewDate(4, 2, hebYear, engine) == 5)
            { // If 4th of Iyar is a Thursday ...
                if (hebDay == 2 && hebMonth == 2) // ... then Yom Hazicaron is on 2th of Iyar
                    listHolidays.Add("Yom Hazikaron");
            }
            else if (GetWeekdayOfHebrewDate(4, 2, hebYear, engine) == 4)
            {
                if (hebDay == 3 && hebMonth == 2)
                    listHolidays.Add("Yom Hazikaron");
            }
            else if (hebYear >= 5764 && GetWeekdayOfHebrewDate(4, 2, hebYear, engine) == 0)
            {
                if (hebDay == 5 && hebMonth == 2)
                    listHolidays.Add("Yom Hazikaron");
            }
            else
            {
                if (hebDay == 4 && hebMonth == 2)
                    listHolidays.Add("Yom Hazikaron");
            }

            // Yom Ha'Azmaut

            if (GetWeekdayOfHebrewDate(5, 2, hebYear, engine) == 6)
            {
                if (hebDay == 3 && hebMonth == 2)
                    listHolidays.Add("Yom Ha'Atzmaut");
            }
            else if (GetWeekdayOfHebrewDate(5, 2, hebYear, engine) == 5)
            {
                if (hebDay == 4 && hebMonth == 2)
                    listHolidays.Add("Yom Ha'Atzmaut");
            }
            else if (hebYear >= 5764 && GetWeekdayOfHebrewDate(4, 2, hebYear, engine) == 0)
            {
                if (hebDay == 6 && hebMonth == 2)
                    listHolidays.Add("Yom Ha'Atzmaut");
            }
            else
            {
                if (hebDay == 5 && hebMonth == 2)
                    listHolidays.Add("Yom Ha'Atzmaut");
            }
            if (hebDay == 14 && hebMonth == 2)
                listHolidays.Add("Pesach Sheni");
            if (hebDay == 18 && hebMonth == 2)
                listHolidays.Add("Lag BaOmer");
            if (hebDay == 28 && hebMonth == 2)
                listHolidays.Add("Yom Yerushalayim");

            // Holidays in Sivan

            if (hebDay == 5 && hebMonth == 3)
                listHolidays.Add("Erev Shavuot");
            if (hebDay == 6 && hebMonth == 3)
            {
                if (diaspora)
                    listHolidays.Add("Shavuot I");
                else
                    listHolidays.Add("Shavuot (Yizkor)");
            }
            if (hebDay == 7 && hebMonth == 3)
            {
                if (diaspora)
                    listHolidays.Add("Shavuot II (Yizkor)");
            }

            // Holidays in Tammuz

            if (GetWeekdayOfHebrewDate(17, 4, hebYear, engine) == 6)
            {
                if (hebDay == 18 && hebMonth == 4)
                    listHolidays.Add("Fast of Tammuz");
            }
            else
            {
                if (hebDay == 17 && hebMonth == 4)
                    listHolidays.Add("Fast of Tammuz");
            }

            // Holidays in Av

            if (GetWeekdayOfHebrewDate(9, 5, hebYear, engine) == 6)
            {
                if (hebDay == 10 && hebMonth == 5)
                    listHolidays.Add("Fast of Av");
            }
            else
            {
                if (hebDay == 9 && hebMonth == 5)
                    listHolidays.Add("Fast of Av");
            }
            if (hebDay == 15 && hebMonth == 5)
                listHolidays.Add("Tu B'Av");

            // Holidays in Elul

            if (hebDay == 29 && hebMonth == 6)
                listHolidays.Add("Erev Rosh Hashana");

            // Holidays in Tishri

            if (hebDay == 1 && hebMonth == 7)
                listHolidays.Add("Rosh Hashana I");
            if (hebDay == 2 && hebMonth == 7)
                listHolidays.Add("Rosh Hashana II");
            if (GetWeekdayOfHebrewDate(3, 7, hebYear, engine) == 6)
            {
                if (hebDay == 4 && hebMonth == 7)
                    listHolidays.Add("Tzom Gedaliah");
            }
            else
            {
                if (hebDay == 3 && hebMonth == 7)
                    listHolidays.Add("Tzom Gedaliah");
            }
            if (hebDay == 9 && hebMonth == 7)
                listHolidays.Add("Erev Yom Kippur");
            if (hebDay == 10 && hebMonth == 7)
                listHolidays.Add("Yom Kippur (Yizkor)");
            if (hebDay == 14 && hebMonth == 7)
                listHolidays.Add("Erev Sukkot");
            if (hebDay == 15 && hebMonth == 7)
            {
                if (diaspora)
                    listHolidays.Add("Sukkot I");
                else
                    listHolidays.Add("Sukkot");
            }
            if (hebDay == 16 && hebMonth == 7)
            {
                if (diaspora)
                    listHolidays.Add("Sukkot II");
                else
                    listHolidays.Add("Chol Hamoed");
            }
            if (hebDay == 17 && hebMonth == 7)
                listHolidays.Add("Chol Hamoed");
            if (hebDay == 18 && hebMonth == 7)
                listHolidays.Add("Chol Hamoed");
            if (hebDay == 19 && hebMonth == 7)
                listHolidays.Add("Chol Hamoed");
            if (hebDay == 20 && hebMonth == 7)
                listHolidays.Add("Chol Hamoed");
            if (hebDay == 21 && hebMonth == 7)
                listHolidays.Add("Hoshana Raba");
            if (hebDay == 22 && hebMonth == 7)
            {
                if (!diaspora)
                {
                    listHolidays.Add("Shemini Atzereth (Yizkor)");
                    listHolidays.Add("Simchat Torah");
                }
                else
                {
                    listHolidays.Add("Shemini Atzereth (Yizkor)");
                }
            }
            if (hebDay == 23 && hebMonth == 7)
            {
                if (diaspora)
                    listHolidays.Add("Simchat Torah");
            }

            // Holidays in Kislev

            if (hebDay == 25 && hebMonth == 9)
                listHolidays.Add("Chanukka I");
            if (hebDay == 26 && hebMonth == 9)
                listHolidays.Add("Chanukka II");
            if (hebDay == 27 && hebMonth == 9)
                listHolidays.Add("Chanukka III");
            if (hebDay == 28 && hebMonth == 9)
                listHolidays.Add("Chanukka IV");
            if (hebDay == 29 && hebMonth == 9)
                listHolidays.Add("Chanukka V");

            // Holidays in Tevet

            if (hebDay == 10 && hebMonth == 10)
                listHolidays.Add("Fast of Tevet");

            if (engine.getLastDayOfJewishMonth(9, hebYear) == 30)
            {
                if (hebDay == 30 && hebMonth == 9)
                    listHolidays.Add("Chanukka VI");
                if (hebDay == 1 && hebMonth == 10)
                    listHolidays.Add("Chanukka VII");
                if (hebDay == 2 && hebMonth == 10)
                    listHolidays.Add("Chanukka VIII");
            }
            if (engine.getLastDayOfJewishMonth(9, hebYear) == 29)
            {
                if (hebDay == 1 && hebMonth == 10)
                    listHolidays.Add("Chanukka VI");
                if (hebDay == 2 && hebMonth == 10)
                    listHolidays.Add("Chanukka VII");
                if (hebDay == 3 && hebMonth == 10)
                    listHolidays.Add("Chanukka VIII");
            }

            // Holidays in Shevat

            if (hebDay == 15 && hebMonth == 11)
                listHolidays.Add("Tu B'Shevat");

            // Holidays in Adar (I)/Adar II

            int monthEsther;
            if (engine.IsHebrewLeapYear(hebYear))
                monthEsther = 13;
            else
                monthEsther = 12;

            if (GetWeekdayOfHebrewDate(13, monthEsther, hebYear, engine) == 6)
            {
                if (hebDay == 11 && hebMonth == monthEsther)
                    listHolidays.Add("Fast of Esther");
            }
            else
            {
                if (hebDay == 13 && hebMonth == monthEsther)
                    listHolidays.Add("Fast of Esther");
            }

            if (hebDay == 14 && hebMonth == monthEsther)
                listHolidays.Add("Purim");
            if (hebDay == 15 && hebMonth == monthEsther)
                listHolidays.Add("Shushan Purim");

            if (engine.IsHebrewLeapYear(hebYear))
            {
                if (hebDay == 14 && hebMonth == 12)
                    listHolidays.Add("Purim Katan");
                if (hebDay == 15 && hebMonth == 12)
                    listHolidays.Add("Shushan Purim Katan");
            }
            return listHolidays;
        }
    }
}
