using System;

namespace JewishCalendar.Utils
{
    public static class Defs
    {
        public static int[] MONTHLASTDAY = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        public static String[] JEWISHMONTHNAME_LEAPYEAR = { "Nisan", "Iyar", "Sivan", "Tammuz", "Av", "Elul", "Tishri", "Heshvan", "Kislev", "Tevet", "Shevat", "Adar I", "Adar II" };
        public static String[] JEWISHMONTHNAME_NONLEAPYEAR = { "Nisan", "Iyar", "Sivan", "Tammuz", "Av", "Elul", "Tishri", "Heshvan", "Kislev", "Tevet", "Shevat", "Adar" };
        public static String[] GREGORIANMONTHNAME_DE = { "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };
        public static String[] GREGORIANWEEKDAY_EN = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        public static String[] GREGORIANWEEKDAY_DE = { "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag", "Sonntag" };
        public static String[] GREGORIANWEEKDAY_RU = { "понедельник", "вторник", "среда", "четверг", "пятница", "суббота", "воскресенье" };
        public static int MAXJEWISHYEARS = 6000;
    }
}
