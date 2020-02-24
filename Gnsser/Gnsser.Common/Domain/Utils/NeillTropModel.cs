//2014.05.22, Cui Yang, created

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Geo;
using Geo.Times; 

namespace Gnsser
{
    /// <summary>
    /// Tropospheric model based in the Neill mapping functions.
    /// 
    /// This model uses the mapping functions developed by A.E. Niell and
    /// published in Neill, A.E., 1996, 'Global Mapping Functions for the 
    /// Atmosphere Delay of Radio Wavelengths,' J. Geophys. Res., 101, 
    /// pp. 3227-3246 (also see IERS TN 32).
    /// 
    /// The coefficients of the hydrostatic mapping function depend on the
    /// latitude and height above sea level of the receiver station, and on
    /// the secondOfWeek of the year. On the other hand, the wet mapping function depends only on latitude.
    /// 
    /// This mapping is independent from surface meteorology, while having 
    /// comparable accuracy and precision to those that require such satData.
    /// This characteristic makes this model very useful, and it is
    /// implemented in geodetic software such as JPL's Gipsy/OASIS.
    /// 
    /// A typical way to use this model follows:
    ///
    /// @obsCode
    ///   NeillTropModel neillTM;
    ///   neillTM.setReceiverLatitude(lat);
    ///   neillTM.setReceiverHeight(height);
    ///   neillTM.setDayOfYear(doy);
    /// @endcode
    ///
    /// Once all the basic model parameters are set (latitude, height and
    /// secondOfWeek of year), then we are able to compute the tropospheric correction
    /// as a function of elevation:
    ///
    /// @obsCode
    ///   trop = neillTM.correction(elevation);
    /// @endcode
    ///
    /// @warning The Neill mapping functions are defined for elevation
    /// angles down to 3 degrees.
    /// </summary>
    public class NeillTropModel 
    {
        double NeillHeight = 0.0;
        double NeillLat = 0.0;  //degree
        int NeillDoy = 0;
        bool ValidHeight = false;
        bool ValidLat = false;
        bool Valid; //true only if current model parameters are valid
        // private bool Valid = false;

        bool ValidDoy = true;
        /// <summary>
        /// Default constructor
        /// </summary>
        public NeillTropModel()
        {
            ValidHeight = false;
            ValidLat = false;
            ValidDoy = false;
        }


        /// Constructor to create a Neill trop model providing just the
        /// height of the receiver above mean sea level. The other
        /// parameters must be set with the appropriate set methods before
        /// calling correction methods.
        /// 
        /// @param ht   Height of the receiver above mean sea level, in
        ///             meters.
        public NeillTropModel(double ht)
        {
            SetReceiverHeight(ht);
        }

        /// <summary>
        /// Constructor to create a Neill trop model providing the height of
        /// the receiver above mean sea level (as defined by ellipsoid model), its latitude and the secondOfWeek of year.
        /// </summary>
        /// <param name="ht">大地高 ，单位 米 Height of the receiver above mean sea level, in meters.</param>
        /// <param name="lat"> 单位 度 Latitude of receiver, in degrees.</param>
        /// <param name="doy">Day of year.</param>
        public NeillTropModel(double ht, double lat, int doy)
        {
            SetReceiverHeight(ht);
            SetReceiverLatitude(lat);
            SetDayOfYear(doy);
        }

        /// <summary>
        /// Constructor to create a Neill trop model providing the position of the receiver and current time.
        /// </summary>
        /// <param name="RX"> Receiver position. 经纬度高程格式</param>
        /// <param name="time"></param>
        public NeillTropModel(GeoCoord RX, Time time)
        {
            SetReceiverHeight(RX.Height);
            SetReceiverLatitude(RX.Lat);
            SetDayOfYear(time);
        }

        // Parameters borrowed from Saastamoinen tropospheric model
        // Constants for wet mapping function
        static double[] NeillWetA = new double[5] { 0.00058021897, 0.00056794847, 0.00058118019, 0.00059727542, 0.00061641693 };
        static double[] NeillWetB = new double[5] { 0.0014275268, 0.0015138625, 0.0014572752, 0.0015007428, 0.0017599082 };
        static double[] NeillWetC = new double[5] { 0.043472961, 0.046729510, 0.043908931, 0.044626982, 0.054736038 };
        // constants for dry mapping function
        static double[] NeillDryA = new double[5] { 0.0012769934, 0.0012683230, 0.0012465397, 0.0012196049, 0.0012045996 };
        static double[] NeillDryB = new double[5] { 0.0029153695, 0.0029152299, 0.0029288445, 0.0029022565, 0.0029024912 };
        static double[] NeillDryC = new double[5] { 0.062610505, 0.062837393, 0.063721774, 0.063824265, 0.064258455 };
        static double[] NeillDryA1 = new double[5] { 0.0, 0.000012709626, 0.000026523662, 0.000034000452, 0.000041202191 };
        static double[] NeillDryB1 = new double[5] { 0.0, 0.000021414979, 0.000030160779, 0.000072562722, 0.00011723375 };
        static double[] NeillDryC1 = new double[5] { 0.0, 0.000090128400, 0.000043497037, 0.00084795348, 0.0017037206 };


        public bool isValid() { return Valid; }

        /// <summary>
        /// Compute and return the full tropospheric delay. 
        /// The receiver height, latitude and Day oy Year must has been set before using the appropriate constructor or the provided methods.
        /// </summary>
        /// <param name="elevation"> Elevation of satellite as seen at receiver,in degrees</param>
        /// <returns></returns>
        public double Correction(double elevation)
        {
            if (!Valid)
            {
                if (!ValidLat)
                {
                    throw new Exception("Invalid Neill trop model: Rx Latitude");
                }

                if (!ValidHeight)
                {
                    throw new Exception("Invalid Neill trop model: Rx Height");
                }

                if (!ValidDoy)
                {
                    throw new Exception("Invalid Neill trop model: day of year");
                }
            }


            // Neill mapping functions work down to 3 degrees of elevation
            if (elevation < 3.0)
            {
                return 0.0;
            }

            double map_dry = Dry_Mapping_Function(elevation);
            double map_wet = Wet_Mapping_Function(elevation);

            // Compute tropospheric delay

            double tropDelay = (Dry_Zenith_Delay() * map_dry) +
                            (Wet_Zenith_Delay() * map_wet);
            return tropDelay;
        }




        // Compute and return the zenith delay for the dry component of
        // the troposphere.  
        public double Dry_Zenith_Delay()
        {
            if (!Valid)
            {
                throw new Exception("Invalid model");
            }

            // Note: 1.013*2.27 = 2.29951
            double ddry = (2.29951 * Math.Exp((-0.000116 * NeillHeight)));

            return ddry;
        }

        // Compute and return the zenith delay for the wet component of
        // the troposphere.
        public double Wet_Zenith_Delay()
        {
            if (!Valid)
            {
                throw new Exception("Invalid model");
            }
            return 0.1;
        }

        /// <summary>
        /// Compute and return the mapping function for dry component of the troposphere.
        /// </summary>
        /// <param name="elevation"> Elevation of satellite as seen at receiver, in degrees</param>
        /// <returns></returns>
        public double Dry_Mapping_Function(double elevation)
        {
            if (!Valid)
            {
                if (!ValidLat)
                {
                    throw new Exception("Invalid Neill trop model: Rx Latitude");
                }

                if (!ValidHeight)
                {
                    throw new Exception("Invalid Neill trop model: Rx Height");
                }

                if (!ValidDoy)
                {
                    throw new Exception("Invalid Neill trop model: day of year");
                }
            }

            if (elevation < 3.0)
            {
                return 0.0;
            }

            double lat, t, ct;

            lat = Math.Abs(NeillLat);         // degrees
            t = NeillDoy - 28.0;  // mid-winter

            if (NeillLat < 0.0)              // southern hemisphere
            {
                t += 365.25 / 2;
            }

            t *= 360.0 / 365.25;            // funcKeyToDouble to degrees
            ct = Math.Cos(t * CoordConsts.DegToRadMultiplier);

            double a, b, c;
            if (lat < 15.0)
            {
                a = NeillDryA[0];
                b = NeillDryB[0];
                c = NeillDryC[0];
            }
            else if (lat < 75.0)      // coefficients are for 15,30,45,60,75 deg
            {
                int i = (int)(lat / 15.0) - 1;
                double frac = (lat - 15.0 * (i + 1)) / 15.0;
                a = NeillDryA[i] + frac * (NeillDryA[i + 1] - NeillDryA[i]);
                b = NeillDryB[i] + frac * (NeillDryB[i + 1] - NeillDryB[i]);
                c = NeillDryC[i] + frac * (NeillDryC[i + 1] - NeillDryC[i]);

                a -= ct * (NeillDryA1[i] + frac * (NeillDryA1[i + 1] - NeillDryA1[i]));
                b -= ct * (NeillDryB1[i] + frac * (NeillDryB1[i + 1] - NeillDryB1[i]));
                c -= ct * (NeillDryC1[i] + frac * (NeillDryC1[i + 1] - NeillDryC1[i]));
            }
            else
            {
                a = NeillDryA[4] - ct * NeillDryA1[4];
                b = NeillDryB[4] - ct * NeillDryB1[4];
                c = NeillDryC[4] - ct * NeillDryC1[4];
            }

            double se = Math.Sin(elevation * CoordConsts.DegToRadMultiplier);
            double map = (1.0 + a / (1.0 + b / (1.0 + c))) / (se + a / (se + b / (se + c)));

            a = 0.0000253;
            b = 0.00549;
            c = 0.00114;
            map += (NeillHeight / 1000.0) *
                   (1.0 / se - ((1.0 + a / (1.0 + b / (1.0 + c))) / (se + a / (se + b / (se + c)))));

            return map;

        }




        /// <summary>
        /// Compute and return the mapping function for wet component of the troposphere.
        /// </summary>
        /// <param name="elevation"> Elevation of satellite as seen at receiver, in degrees</param>
        /// <returns></returns>
        public double Wet_Mapping_Function(double elevation)
        {
            if (!Valid)
            {
                if (!ValidLat)
                {
                    throw new Exception("Invalid Neill trop model: Rx Latitude");
                }

                if (!ValidHeight)
                {
                    throw new Exception("Invalid Neill trop model: Rx Height");
                }

                if (!ValidDoy)
                {
                    throw new Exception("Invalid Neill trop model: day of year");
                }
            }

            if (elevation < 3.0)
            {
                return 0.0;
            }

            double a, b, c, lat;
            lat = Math.Abs(NeillLat);         // degrees
            if (lat < 15.0)
            {
                a = NeillWetA[0];
                b = NeillWetB[0];
                c = NeillWetC[0];
            }
            else if (lat < 75.0)          // coefficients are for 15,30,45,60,75 deg
            {
                int i = (int)(lat / 15.0) - 1;
                double frac = (lat - 15.0 * (i + 1)) / 15.0;
                a = NeillWetA[i] + frac * (NeillWetA[i + 1] - NeillWetA[i]);
                b = NeillWetB[i] + frac * (NeillWetB[i + 1] - NeillWetB[i]);
                c = NeillWetC[i] + frac * (NeillWetC[i + 1] - NeillWetC[i]);
            }
            else
            {
                a = NeillWetA[4];
                b = NeillWetB[4];
                c = NeillWetC[4];
            }

            double se = Math.Sin(elevation * CoordConsts.DegToRadMultiplier);
            double map = (1.0 + a / (1.0 + b / (1.0 + c))) / (se + a / (se + b / (se + c)));

            return map;
        }


        /// <summary>
        ///  This method configure the model to estimate the weather using
        /// height, latitude and secondOfWeek of year (DOY). It is called
        /// automatically when setting those parameters.
        /// </summary>
        public void SetWeather()
        {
            if (!ValidLat)
            {
                Valid = false;
                throw new Exception("NeillTropModel must have Rx latitude before computing weather");
            }
            if (!ValidDoy)
            {
                Valid = false;
                throw new Exception("NeillTropModel must have day of year before computing weather");
            }
            Valid = ValidHeight && ValidLat && ValidDoy;
        }

        /// <summary>
        /// In Neill tropospheric model, this is a dummy method kept here just for consistency,
        /// </summary>
        /// <param name="TProduct"></param>
        /// <param name="P"></param>
        /// <param name="H"></param>
        // public override void SetWeather(double TProduct, double P, double H)
        //{

        //}

        //public void SetWeather()
        //{ }

        /// <summary>
        /// Define the receiver height; this is required before calling correction() or any of the zenith_delay routines.
        /// </summary>
        /// <param name="ht"> Height of the receiver above mean sea level,in meters.</param>
        public void SetReceiverHeight(double ht)
        {
            NeillHeight = ht;
            ValidHeight = true;
            // Change the value of field "valid" if everything is already set
            Valid = ValidHeight && ValidLat && ValidDoy;
            // If model is valid, set the appropriate parameters
            if (Valid) SetWeather();
        }

        /// <summary>
        /// Define the receiver latitude; this is required before calling correction() or any of the zenith_delay routines.
        /// </summary>
        /// <param name="lat"> Latitude of receiver, in degrees.</param>
        public void SetReceiverLatitude(double lat)
        {
            NeillLat = lat;
            ValidLat = true;
            // Change the value of field "valid" if everything is already set
            Valid = ValidHeight && ValidLat && ValidDoy;
            // If model is valid, set the appropriate parameters
            if (Valid) SetWeather();
        }


        /// <summary>
        /// Set the time when tropospheric correction will be computed for,in days of the year.
        /// </summary>
        /// <param name="doy">Day of the year</param>
        public void SetDayOfYear(int doy)
        {
            if ((doy >= 1) && (doy <= 366))
            {
                ValidDoy = true;
            }
            else
            {
                ValidDoy = false;
            }

            NeillDoy = doy;

            // Change the value of field "valid" if everything is already set
            Valid = ValidHeight && ValidLat && ValidDoy;
            // If model is valid, set the appropriate parameters
            if (Valid) SetWeather();
        }



        /// <summary>
        /// Set the time when tropospheric correction will be computed for,in days of the year.
        /// </summary>
        /// <param name="time">Time object.</param>
        public void SetDayOfYear(Time time)
        {
            NeillDoy = time.DayOfYear;
            ValidDoy = true;
            // Change the value of field "valid" if everything is already set
            Valid = ValidHeight && ValidLat && ValidDoy;
            // If model is valid, set the appropriate parameters
            if (Valid) SetWeather();
        }


        /// <summary>
        /// Convenient method to set all model parameters in one pass.
        /// </summary>
        /// <param name="time">Time object.</param>
        /// <param name="rxPos">Receiver position object.</param>
        public void setAllParameters(Time time, GeoCoord rxPos)
        {
            NeillDoy = time.DayOfYear;
            ValidDoy = true;

            NeillLat = rxPos.Lat;
            ValidLat = true;
            NeillHeight = rxPos.Height;
            ValidHeight = true;

            Valid = ValidHeight && ValidLat && ValidDoy;
            // If model is valid, set the appropriate parameters
            if (Valid) SetWeather();
        }
    }
}
