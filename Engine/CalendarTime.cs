using System;

namespace JewishCalendar.Engine
{
    public class CalendarTime
    {
        public CalendarTime(int hour, int min)
        {
            this.hour = hour; this.min = min;
        }

        public int getHour() { return hour; }
        public int getMin() { return min; }

        public void setHour(int hour) { this.hour = hour; }
        public void setMin(int min) { this.min = min; }

        public String formatTime12()
        {
            int hourModulo12 = hour % 12;
            if (hourModulo12 == 0)
                hourModulo12 = 12;

            String ampm;
            if (hour >= 12)
                ampm = "PM";
            else
                ampm = "AM";

            String hourStr, minStr;
            if (hourModulo12 < 10)
                hourStr = "0" + hourModulo12;
            else
                hourStr = hourModulo12.ToString();
            if (min < 10)
                minStr = "0" + min;
            else
                minStr = min.ToString();
            return hourStr + ":" + minStr + ampm;
        }

        public String formatTime24()
        {
            String hourStr, minStr;
            if (hour < 10)
                hourStr = "0" + hour;
            else
                hourStr = hour.ToString();
            if (min < 10)
                minStr = "0" + min;
            else
                minStr = min.ToString();
            return hourStr + ":" + minStr;
        }

        public static String formatTimeShaaZmanit(int value)
        {
            int hour = (int)(value / 60);
            int min = value % 60;
            String hourStr, minStr;
            if (hour < 10)
                hourStr = "0" + hour;
            else
                hourStr = hour.ToString();
            if (min < 10)
                minStr = "0" + min;
            else
                minStr = min.ToString();
            return hourStr + ":" + minStr;
        }

        public void addMinutes(int min)
        {
            this.min += min;
            while (this.min >= 60)
            {
                this.min -= 60;
                this.hour += 1;
            }
        }

        public void subtractMinutes(int min)
        {
            this.min -= min;
            while (this.min < 0)
            {
                this.min += 60;
                this.hour -= 1;
            }
        }

        private int hour, min;
    }
}
