using System;

namespace JewishCalendar.Engine
{
    class AstronomicalCalculations
    {
        private bool leap(int y)
        {
            if (y % 400 == 0)
                return true;
            if (y % 100 != 0)
            {
                if (y % 4 == 0)
                    return true;
            }
            return false;
        }
        private int doy(int d, int m, int y)
        {
            int[] monCount = { 0, 1, 32, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };
            if ((m > 2) && (leap(y)))
                return monCount[m] + d + 1;
            else
                return monCount[m] + d;
        }
        private double todec(int deg, int min)
        {
            return (deg + min / 60.0);
        }
        private double M(double x)
        {
            return (0.9856 * x - 3.251);
        }
        private double L(double x)
        {
            return (x + 1.916 * Math.Sin(0.01745 * x) + 0.02 * Math.Sin(2 * 0.01745 * x) + 282.565);
        }
        private double adj(double x)
        {
            return (-0.06571 * x - 6.620);
        }
        private double float_abs(double x)
        {
            if (x < 0.0)
                return (-x);
            else
                return (x);
        }
        private double[] suntime(int d, int m, int y,
                  int zendeg, int zenmin,
                  int londeg, int lonmin, int ew,
                  int latdeg, int latmin, int ns,
                  int tz,
                  int elevation)
        { // Elevation in meters
            if (zendeg == 90)
            {
                double earthRadiusInMeters = 6356.9 * 1000.0;
                double elevationAdjustment = RadianToDegree((Math.Acos(earthRadiusInMeters / (earthRadiusInMeters + elevation))));

                double z = zendeg + zenmin / 60.0;
                z += elevationAdjustment;
                zendeg = (int)Math.Floor(z);
                zenmin = (int)((z - Math.Floor(z)) * 60);
            }

            int day = doy(d, m, y);
            double cosz = Math.Cos(0.01745 * todec(zendeg, zenmin));

            double longitude = todec(londeg, lonmin);
            if (ew != 0)
                longitude *= -1;
            double lonhr = longitude / 15.0;
            double latitude = todec(latdeg, latmin);
            if (ns != 0)
            {
                latitude *= -1;
            }
            double coslat = Math.Cos(0.01745 * latitude);
            double sinlat = Math.Sin(0.01745 * latitude);

            double t_rise = day + (6.0 + lonhr) / 24.0;
            double t_set = day + (18.0 + lonhr) / 24.0;

            double xm_rise = M(t_rise);
            double xl_rise = L(xm_rise);
            double xm_set = M(t_set);
            double xl_set = L(xm_set);

            double a_rise = 57.29578 * Math.Atan(0.91746 * Math.Tan(0.01745 * xl_rise));
            double a_set = 57.29578 * Math.Atan(0.91746 * Math.Tan(0.01745 * xl_set));
            if (float_abs(a_rise + 360.0 - xl_rise) > 90.0)
                a_rise += 180.0;
            if (a_rise > 360.0)
                a_rise -= 360.0;

            if (float_abs(a_set + 360.0 - xl_set) > 90.0)
                a_set += 180.0;
            if (a_set > 360.0)
                a_set -= 360.0;

            double ahr_rise = a_rise / 15.0;
            double sindec = 0.39782 * Math.Sin(0.01745 * xl_rise);
            double cosdec = Math.Sqrt(1.0 - sindec * sindec);
            double h_rise = (cosz - sindec * sinlat) / (cosdec * coslat);

            double ahr_set = a_set / 15.0;
            sindec = 0.39782 * Math.Sin(0.01745 * xl_set);
            cosdec = Math.Sqrt(1.0 - sindec * sindec);
            double h_set = (cosz - sindec * sinlat) / (cosdec * coslat);

            if (float_abs(h_rise) <= 1.0)
                h_rise = 57.29578 * Math.Acos(h_rise);
            else
                return null; // NO_SUNRISE

            if (float_abs(h_set) <= 1.0)
                h_set = 57.29578 * Math.Acos(h_set);
            else
                return null; // NO_SUNSET
            double ut_rise = ((360.0 - h_rise) / 15.0) + ahr_rise + adj(t_rise) + lonhr;
            double ut_set = (h_rise / 15.0) + ahr_set + adj(t_set) + lonhr;

            double returnSunrise = ut_rise + tz;  // sunrise
            double returnSunset = ut_set + tz; // sunset
            double[] result = { returnSunrise, returnSunset };
            return result;
        }
        private int[] timeadj(double t)
        {
            if (t < 0)
                t += 24.0;

            int hour = (int)Math.Floor(t);
            int min = (int)Math.Floor((t - hour) * 60.0 + 0.5);

            if (min >= 60)
            {
                hour += 1;
                min -= 60;
            }

            if (hour > 24)
                hour -= 24;

            int[] result = { hour, min };
            return result;
        }

        private int GetDegreesBelowHorizonAdd(int uMonth, int uDay, int uYear,
                      double fDegreesBelowHorizon,
                      Location location)
        {
            int iLatitude = location.getLatitude();
            int iLongitude = location.getLongitude();
            int iTimeZone = location.getTimeZone();
            int elevation = location.getElevation();

            int longitudeFlag, latitudeFlag;
            if (iLongitude < 0)
            {
                longitudeFlag = 0;
            }
            else
            {
                longitudeFlag = 1;
            }
            if (iLatitude < 0)
            {
                latitudeFlag = 1;
            }
            else
            {
                latitudeFlag = 0;
            }
            double[] returnTimes = suntime(uDay, uMonth, uYear, 90, 50,
                    (int)Math.Floor((double) Math.Abs(iLongitude / 100)),
                    (int)Math.Floor((double) Math.Abs(iLongitude % 100)), longitudeFlag,
                    (int)Math.Floor((double) Math.Abs(iLatitude / 100)),
                    (int)Math.Floor((double) Math.Abs(iLatitude % 100)), latitudeFlag,
                  iTimeZone, elevation);
            if (returnTimes != null)
            {
                int[] srTime = timeadj(returnTimes[1]);
                while (srTime[0] > 12)
                {
                    srTime[0] -= 12;
                }

                double db = fDegreesBelowHorizon + 90.0;
                int deghour = (int)Math.Floor(db);
                db = db - deghour;
                db *= 60.0;
                int degmin = (int)Math.Floor(db);
                returnTimes = suntime(uDay, uMonth, uYear,
                  deghour, degmin,
                    (int)(Math.Floor((double) Math.Abs(iLongitude / 100))),
                    (int)(Math.Floor((double) Math.Abs(iLongitude % 100))), longitudeFlag,
                    (int)(Math.Floor((double) Math.Abs(iLatitude / 100))),
                    (int)(Math.Floor((double) Math.Abs(iLatitude % 100))), latitudeFlag,
                  iTimeZone, elevation);
                if (returnTimes != null)
                {
                    int[] dbTime = timeadj(returnTimes[1]);
                    while (dbTime[0] > 12)
                    {
                        dbTime[0] -= 12;
                    }

                    int srTimeValue = srTime[0] * 60 + srTime[1];
                    int dbTimeValue = dbTime[0] * 60 + dbTime[1];
                    return dbTimeValue - srTimeValue;
                }
            }
            return -1;
        }

        public CalendarTime GetSunrise(int uMonth, int uDay, int uYear, Location location)
        {
            int iLatitude = location.getLatitude();
            int iLongitude = location.getLongitude();
            int iTimeZone = location.getTimeZone();
            int elevation = location.getElevation();

            int longitudeFlag, latitudeFlag;
            if (iLongitude < 0)
            {
                longitudeFlag = 0;
            }
            else
            {
                longitudeFlag = 1;
            }
            if (iLatitude < 0)
            {
                latitudeFlag = 1;
            }
            else
            {
                latitudeFlag = 0;
            }
            double[] returnTimes = suntime(uDay, uMonth, uYear, 90, 50,
                    (int)(Math.Floor((double) Math.Abs(iLongitude / 100))),
                    (int)(Math.Floor((double) Math.Abs(iLongitude % 100))), longitudeFlag,
                    (int)(Math.Floor((double) Math.Abs(iLatitude / 100))),
                    (int)(Math.Floor((double) Math.Abs(iLatitude % 100))), latitudeFlag,
                    iTimeZone, elevation);
            if (returnTimes != null)
            {
                int[] returnTime = timeadj(returnTimes[0]);

                while (returnTime[0] > 12)
                {
                    returnTime[0] -= 12;
                }

                return new CalendarTime(returnTime[0], returnTime[1]);
            }
            else
            {
                return null;
            }
        }
        public CalendarTime GetSunriseDegreesBelowHorizon(int uMonth, int uDay, int uYear,
                            double fDegreesBelowHorizon,
                            Location location)
        {
            CalendarTime t = GetSunrise(uMonth, uDay, uYear, location);
            if (t != null)
            {
                int adding = GetDegreesBelowHorizonAdd(uMonth, uDay, uYear, fDegreesBelowHorizon, location);
                if (adding != -1)
                {
                    t.subtractMinutes(adding);
                    return t;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public CalendarTime GetSunset(int uMonth, int uDay, int uYear,
                 Location location)
        {
            int iLatitude = location.getLatitude();
            int iLongitude = location.getLongitude();
            int iTimeZone = location.getTimeZone();
            int elevation = location.getElevation();

            int longitudeFlag, latitudeFlag;
            if (iLongitude < 0)
            {
                longitudeFlag = 0;
            }
            else
            {
                longitudeFlag = 1;
            }
            if (iLatitude < 0)
            {
                latitudeFlag = 1;
            }
            else
            {
                latitudeFlag = 0;
            }

            double[] returnTimes = suntime(uDay, uMonth, uYear, 90, 50,
                    (int)(Math.Floor((double) Math.Abs(iLongitude / 100))),
                    (int)(Math.Floor((double) Math.Abs(iLongitude % 100))), longitudeFlag,
                    (int)(Math.Floor((double) Math.Abs(iLatitude / 100))),
                    (int)(Math.Floor((double) Math.Abs(iLatitude % 100))), latitudeFlag,
                    iTimeZone, elevation);
            if (returnTimes != null)
            {
                int[] returnTime = timeadj(returnTimes[1]);

                while (returnTime[0] < 12)
                    returnTime[0] += 12;

                return new CalendarTime(returnTime[0], returnTime[1]);
            }
            else
            {
                return null;
            }
        }

        public CalendarTime GetSunsetDegreesBelowHorizon(int uMonth, int uDay, int uYear,
                            double fDegreesBelowHorizon,
                            Location location)
        {
            CalendarTime t = GetSunset(uMonth, uDay, uYear, location);
            if (t != null)
            {
                int adding = GetDegreesBelowHorizonAdd(uMonth, uDay, uYear, fDegreesBelowHorizon, location);
                if (adding != -1)
                {
                    t.addMinutes(adding);
                    return t;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public CalendarTime GetProportionalHours(double value, CalendarTime sunrise, CalendarTime sunset)
        {
            if (sunrise == null || sunset == null)
                return null;
            double sr = sunrise.getHour() * 60 + sunrise.getMin();
            double ss = sunset.getHour() * 60 + sunset.getMin();
            double retval = sr + Math.Floor(((ss - sr) * value) / 12);
            int[] propResult = { (int)(Math.Floor(retval / 60)), (int)(retval % 60) };
            return new CalendarTime(propResult[0], propResult[1]);
        }

        public int GetShaaZmanit(CalendarTime sunrise, CalendarTime sunset)
        {
            double sr = sunrise.getHour() * 60 + sunrise.getMin();
            double ss = sunset.getHour() * 60 + sunrise.getMin();
            return (int)(Math.Floor((ss - sr) / 12));
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
