//2015.01.28, czs, create from VB in pengzhou, 转换自VB代码。
//2015.10.21,double,correct code in line 564."if (MainForm.Default.NtripParam.ManualLon > 0)" to "if (MainForm.Default.NtripParam.ManualLat > 0)"
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using Geo.IO;


namespace Gnsser.Ntrip
{
    /// <summary>
    /// 本地设备。
    /// </summary>
    public class LocalGps
    {
        ILog log = new Log(typeof(LocalGps));
        public int LastFixQuality = -1;
        public int LastSatsTracked = 0;
        public string LastStationID = "0";
        public string LastCorrAge = "-";
        public double LastHDOP = 0;
        public double LastVDOP = 0;
        public double LastPDOP = 0;
        public double LastAltitude = -1;
        public double LastHeading = 0;
        public double LastSpeed = 0;
        public double LastSpeedSmoothed = 0;
        private double[] RecentSpeeds = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public string RecordQueue = "";

        public string Disp2 = "age";
        public string Disp3 = "speed-mph-smoothed";


        //age
        //hdop
        //vdop
        //pdop
        //elevation-feet
        //elevation-meters
        //speed-mph
        //speed-mph-smoothed
        //speed-kmh
        //speed-kmh-smoothed
        //heading

        //Speed and Heading satData require RMC sentences
        //VDOP and PDOP satData require GSA sentences ??

        /// <summary>
        /// 处理NMEA数据,从GPS接收机返回的数据？ 2016.01.24,czs,
        /// </summary>
        /// <param name="x"></param>
        public void ProcessNmeaData(string x)
        {
            //GPRMC (Required) contains time, lat, lon, speed, heading, date - We use RMC for **ALL** of the logging and steering commands
            //GPGGA (Required) contains fix quality, # of sats tracked, HDOP, and Altitude. Only useful to tell us when we lose sat signal.
            //GPGSV (Not Required) contains location and signal strength about up to 4 of the satellites in view. This is just used to display what sats are where.
            //GPGSA (Not Required) contains fix type, sat PRNs used, PDOP, HDOP, and VDOP. This just compliments GPGSV.

            int charlocation = x.LastIndexOf("$"); //Find location of the last $
            if (charlocation == -1 | charlocation + 1 > x.Length - 5)
            {
                return; //no $ found or not enough satData left
            }
            x = x.Substring(charlocation + 1 - 1); //drop characters before the $

            charlocation = x.IndexOf("*"); //Find location of prevObj *
            if (charlocation == -1)
            {
                return; //no * found
            }
            if (x.Length < charlocation + 3) //there aren't 2 characters after the *
            {
                return;
            }
            else if (x.Length > charlocation + 3) //there is extra satData after the *
            {
                x = x.Substring(0, charlocation + 3); //remove the extra satData after 2 chars after the *
            }
            if (x.Length < 8)
            {
                return; //not enough satData left
            }

            string[] aryNMEALine = x.Split('*');
            //lets see if the checksum matches the stuff before the astrix
            if (CalculateChecksum(aryNMEALine[0].Replace("$", "")) == aryNMEALine[1])
            {
                //Checksum matches, send it to the respective subroutine for processing.
                if (aryNMEALine[0].Substring(0, 6) == "$GPRMC")
                {
                    ProcessGPRMC(aryNMEALine[0]);
                }
                if (aryNMEALine[0].Substring(0, 6) == "$GPGGA")
                {
                  //  OldReceiverForm.MostRecentGGA = x;
                    ProcessGPGGA(aryNMEALine[0]);
                    //if (Gnsser.Ntrip.OldReceiverForm.Default.NtripParam.WriteNMEAToFile)
                    //{
                    //    RecordLine(x);
                    //}
                }
                if (aryNMEALine[0].Substring(0, 6) == "$GPGSA")
                {
                    ProcessGPGSA(aryNMEALine[0]);
                }
            }
        }

        /// <summary>
        /// 处理GGA数据
        /// </summary>
        /// <param name="obsCodeode"></param>
        private void ProcessGPGGA(string code)
        {
            //This is a GGA line and has 11+ fields; check that we have at least 11
            //$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47
            //0     ,1     ,2       ,3,4        ,5,6,7 ,8  ,9   ,10,11  ,12
            //$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A

            double InLatitude = 0;
            double InLongitude = 0;

            string[] aryNMEAString = code.Split(',');
            if ((aryNMEAString.Length - 1) > 13) //we have at least 14 fields.
            {
                //Parse Lat/Lon
                if (aryNMEAString[2] != "" && aryNMEAString[3] != "" && aryNMEAString[4] != "" && aryNMEAString[5] != "")
                {
                    if (Information.IsNumeric(aryNMEAString[2]) && Information.IsNumeric(aryNMEAString[4]))
                    {
                        double snglat = (double.Parse(aryNMEAString[2])) / 100;
                        double snglatmins = snglat % 1;
                        snglat = snglat - snglatmins;
                        snglatmins = snglatmins * 100 / 60;
                        InLatitude = snglat + snglatmins;
                        if (aryNMEAString[3] == "S")
                        {
                            InLatitude = 0 - InLatitude;
                        }

                        double snglon = (double.Parse(aryNMEAString[4])) / 100;
                        double snglonmins = snglon % 1;
                        snglon = snglon - snglonmins;
                        snglonmins = snglonmins * 100 / 60;
                        InLongitude = snglon + snglonmins;
                        if (aryNMEAString[5] == "W")
                        {
                            InLongitude = 0 - InLongitude;
                        }
                    }
                    else
                    {
                        //non-numeric satData in gga message
                       throw new Exception( "Bad GGA data");
                    }
                }
                else
                {
                    //empty GGA message
                   ShowInfo("Empty GGA data");
                }

                if (Information.IsNumeric(aryNMEAString[6]) && Information.IsNumeric(aryNMEAString[7]) && Information.IsNumeric(aryNMEAString[8]) && Information.IsNumeric(aryNMEAString[9]))
                {
                    int InFixQuality = int.Parse(aryNMEAString[6]);
                    int InSatsTracked = int.Parse(aryNMEAString[7]);
                    double InHDOP = double.Parse(aryNMEAString[8]);
                    float InAltitude = float.Parse(aryNMEAString[9]);

                    if (!(InFixQuality == LastFixQuality) || !(InSatsTracked == LastSatsTracked)) //fix quality has changed
                    {
                        string gpstype = "";
                        string shorttype = "";
                        switch (InFixQuality)
                        {
                            case 1: //GPS fix (SPS)
                                gpstype = "GPS fix (No Differential Correction)";
                                shorttype = "GPS";
                                break;
                            case 2: //DGPS fix
                                gpstype = "DGPS";
                                shorttype = "DGPS";
                                break;
                            case 3: //PPS fix
                                gpstype = "PPS Fix";
                                shorttype = "PPS";
                                break;
                            case 4: //Real Time Kinematic
                                gpstype = "RTK";
                                shorttype = "RTK";
                                break;
                            case 5: //Float RTK
                                gpstype = "Float RTK";
                                shorttype = "FloatRTK";
                                break;
                            case 6: //estimated (dead reckoning) (2.3 feature)
                                gpstype = "Estimated";
                                shorttype = "Estimated";
                                break;
                            case 7: //Manual input mode
                                gpstype = "Manual Input Mode";
                                shorttype = "Manual";
                                break;
                            case 8: //Simulation mode
                                gpstype = "Simulation";
                                shorttype = "Simulation";
                                break;
                            default: //0 = invalid
                                gpstype = "Invalid";
                                shorttype = "Invalid";
                                break;
                        }

                       ShowInfo(shorttype + ":" + System.Convert.ToString(InSatsTracked));

                        if (!(InFixQuality == LastFixQuality))
                        {
                            if (LastFixQuality == -1) //This only happens on startup
                            {
                                LastFixQuality = 0;
                            }
                            else //This happens every time except at startup
                            {
                               // Gnsser.Ntrip.OldReceiverForm.Default.PlayAudioAlert();
                            }

                            string status = "";
                            if (InFixQuality == 5 & LastFixQuality == 4)
                            {
                                status = "Degraded";
                            }
                            else if (InFixQuality == 4 & LastFixQuality == 5)
                            {
                                status = "Increased";
                            }
                            else if (InFixQuality == 8 | LastFixQuality == 8)
                            {
                                status = "Changed";
                            }
                            else if (InFixQuality > LastFixQuality)
                            {
                                status = "Increased";
                            }
                            else
                            {
                                status = "Degraded";
                            }
                          //  Gnsser.Ntrip.OldReceiverForm.Default.LogEvent("GPS Fix Quality " + status + " from " + System.Convert.ToString(LastFixQuality) + " to " + System.Convert.ToString(InFixQuality) + " (" + gpstype + ")");
                            if (LastFixQuality == 4) //was RTK
                            {
                                //Gnsser.Ntrip.OldReceiverForm.Default.LogEvent("Correction Age was " + LastCorrAge);
                            }
                            if (LastHDOP == InHDOP)
                            {
                               // Gnsser.Ntrip.OldReceiverForm.Default.LogEvent("H-DOP unchanged at " + System.Convert.ToString(InHDOP));
                            }
                            else
                            {
                              //  Gnsser.Ntrip.OldReceiverForm.Default.LogEvent("H-DOP was " + System.Convert.ToString(LastHDOP) + " and now is " + System.Convert.ToString(InHDOP));
                            }//


                            LastFixQuality = InFixQuality;
                        }

                        if (!(InSatsTracked == LastSatsTracked))
                        {
                            //sat count has changed

                            if (InSatsTracked > LastSatsTracked)
                            {
                               // Gnsser.Ntrip.OldReceiverForm.Default.LogEvent("Number of Satellites tracked Increased from " + System.Convert.ToString(LastSatsTracked) + " to " + System.Convert.ToString(InSatsTracked));
                            }
                            else
                            {
                              //  Gnsser.Ntrip.OldReceiverForm.Default.LogEvent("Number of Satellites tracked Decreased from " + System.Convert.ToString(LastSatsTracked) + " to " + System.Convert.ToString(InSatsTracked));
                            }

                            LastSatsTracked = InSatsTracked;
                        }
                    }


                    if (!(InHDOP == LastHDOP)) //hdop has changed
                    {
                        LastHDOP = InHDOP;
                        if (Disp2 == "hdop")
                        {
                              ShowInfo( "HDOP:" + System.Convert.ToString(LastHDOP));
                        }
                        if (Disp3 == "hdop")
                        {
                              ShowInfo( "HDOP:" + System.Convert.ToString(LastHDOP));
                        }
                    }

                    if (!(InAltitude == LastAltitude)) //altitude has changed
                    {
                        LastAltitude = InAltitude;
                        if (Disp2 == "elevation-meters")
                        {
                              ShowInfo( Strings.Format(LastAltitude, "#.00") + "m");
                        }
                        if (Disp3 == "elevation-meters")
                        {
                              ShowInfo( Strings.Format(LastAltitude, "#.00") + "m");
                        }
                        if (Disp2 == "elevation-feet")
                        {
                            ShowInfo(Strings.Format(LastAltitude * 3.2808399, "#.0") + "\'");
                        }
                        if (Disp3 == "elevation-feet")
                        {
                              ShowInfo( Strings.Format(LastAltitude * 3.2808399, "#.0") + "\'");
                        }
                        if (InFixQuality == 4)
                        {
                          //  Gnsser.Ntrip.MainForm.Default.ElevNewE((decimal)InAltitude);
                        }
                    }
                }



                string inCorrAge = aryNMEAString[13];
                if (inCorrAge.Length == 0)
                {
                    inCorrAge = "N/A";
                }
                if (!(LastCorrAge == inCorrAge))
                {
                    LastCorrAge = inCorrAge;
                    if (Disp2 == "age")
                    {
                          ShowInfo( "Age:" + inCorrAge);
                    }
                    if (Disp3 == "age")
                    {
                      ShowInfo( "Age:" + inCorrAge);
                    }
                }


                string inStationID = aryNMEAString[14];
                if (inStationID.Length == 0)
                {
                    inStationID = "0";
                }
                if (!(LastStationID == inStationID))
                {
                  //  Gnsser.Ntrip.OldReceiverForm.Default.LogEvent("Correction Station ID changed from " + LastStationID + " to " + inStationID);
                    LastStationID = inStationID;
                  //  Gnsser.Ntrip.OldReceiverForm.Default.PlayAudioAlert();
                }
            }
        }

        public void ShowInfo(string msg)
        {
            log.Info(msg);
        }
        private void ProcessGPGSA(string code)
        {
            string[] aryNMEAString = code.Split(',');
            if ((aryNMEAString.Length - 1) >= 17) //we have at least 15 fields.
            {
                if (Information.IsNumeric(aryNMEAString[15]) && Information.IsNumeric(aryNMEAString[16]) && Information.IsNumeric(aryNMEAString[17]))
                {
                    float InPDOP = float.Parse(aryNMEAString[15]);
                    float InHDOP = float.Parse(aryNMEAString[16]);
                    float InVDOP = float.Parse(aryNMEAString[17]);

                    if (!(InHDOP == LastPDOP)) //pdop has changed
                    {
                        LastPDOP = InPDOP;
                        if (Disp2 == "pdop")
                        {
                              ShowInfo( "PDOP:" + System.Convert.ToString(LastPDOP));
                        }
                        if (Disp3 == "pdop")
                        {
                              ShowInfo( "PDOP:" + System.Convert.ToString(LastPDOP));
                        }
                    }
                    if (!(InHDOP == LastHDOP)) //hdop has changed
                    {
                        LastHDOP = InHDOP;
                        if (Disp2 == "hdop")
                        {
                              ShowInfo( "HDOP:" + System.Convert.ToString(LastHDOP));
                        }
                        if (Disp3 == "hdop")
                        {
                              ShowInfo( "HDOP:" + System.Convert.ToString(LastHDOP));
                        }
                    }
                    if (!(InVDOP == LastVDOP)) //vdop has changed
                    {
                        LastVDOP = InVDOP;
                        if (Disp2 == "vdop")
                        {
                              ShowInfo( "VDOP:" + System.Convert.ToString(LastVDOP));
                        }
                        if (Disp3 == "vdop")
                        {
                              ShowInfo( "VDOP:" + System.Convert.ToString(LastVDOP));
                        }
                    }
                }
            }
        }
        private void ProcessGPRMC(string code)
        {
            //This is a RMC line and has 9+ fields; check that it's active (good satData)
            //$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A
            //0     ,1     ,2,3       ,4,5        ,6,7    ,8    ,9     ,10   ,11

            string[] aryNMEAString = code.Split(',');

            if (aryNMEAString[7] != "")
            {
                if (Information.IsNumeric(aryNMEAString[7]))
                {
                    LastSpeed = (double)((double.Parse(aryNMEAString[7])) * 1.852); //Convert knots to km/h

                    double speedsum = LastSpeed;
                    for (var i = 0; i <= 8; i++)
                    {
                        RecentSpeeds[(int)i] = RecentSpeeds[i + 1];
                        speedsum += RecentSpeeds[(int)i];
                    }
                    RecentSpeeds[9] = LastSpeed;
                    LastSpeedSmoothed = speedsum / 10;

                    if (Disp2 == "speed-mph")
                    {
                          ShowInfo( Strings.Format(LastSpeed * 0.621371192, "0.0") + " MPH");
                    }
                    if (Disp3 == "speed-mph")
                    {
                          ShowInfo( Strings.Format(LastSpeed * 0.621371192, "0.0") + " MPH");
                    }
                    if (Disp2 == "speed-mph-smoothed")
                    {
                          ShowInfo( Strings.Format(LastSpeedSmoothed * 0.621371192, "0.0") + " MPH");
                    }
                    if (Disp3 == "speed-mph-smoothed")
                    {
                          ShowInfo( Strings.Format(LastSpeedSmoothed * 0.621371192, "0.0") + " MPH");
                    }

                    if (Disp2 == "speed-kmh")
                    {
                          ShowInfo( Strings.Format(LastSpeed, "0.0") + " km/h");
                    }
                    if (Disp3 == "speed-kmh")
                    {
                          ShowInfo( Strings.Format(LastSpeed, "0.0") + " km/h");
                    }
                    if (Disp2 == "speed-kmh-smoothed")
                    {
                          ShowInfo( Strings.Format(LastSpeedSmoothed, "0.0") + " km/h");
                    }
                    if (Disp3 == "speed-kmh-smoothed")
                    {
                          ShowInfo( Strings.Format(LastSpeedSmoothed, "0.0") + " km/h");
                    }
                }
            }

            if (aryNMEAString[8] != "")
            {
                if (Information.IsNumeric(aryNMEAString[8]))
                {
                    LastHeading = float.Parse(aryNMEAString[8]);
                    if (Disp2 == "heading")
                    {
                          ShowInfo( Strings.Format(LastHeading, "0.0") + System.Convert.ToString(Strings.Chr(176)));
                    }
                    if (Disp3 == "heading")
                    {
                          ShowInfo( Strings.Format(LastHeading, "0.0") + System.Convert.ToString(Strings.Chr(176)));
                    }
                }
            }
        }


        private void RecordLine(string Line)
        {
            RecordQueue += Line + "\r\n";
            if (RecordQueue.Length > 8000)
            {
                WriteRecordingQueueToFile();
            }
        }

        public void WriteRecordingQueueToFile()
        {
            if (RecordQueue.Length > 0)
            {
                string nmeafolder = Application.StartupPath + "\\NMEA";
                if (!(new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.DirectoryExists(nmeafolder))
                {
                    try
                    {
                        (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.CreateDirectory(nmeafolder);
                    }
                    catch (Exception)
                    {
                    }
                }

                string nmeafile = nmeafolder + "\\" + System.Convert.ToString(DateAndTime.Year(DateTime.Now)) + Strings.Format(DateAndTime.Month(DateTime.Now), "00") + Strings.Format(DateAndTime.DatePart(DateInterval.Day, DateTime.Now, (Microsoft.VisualBasic.FirstDayOfWeek)Microsoft.VisualBasic.FirstDayOfWeek.Sunday, (Microsoft.VisualBasic.FirstWeekOfYear)Microsoft.VisualBasic.FirstWeekOfYear.Jan1), "00") + ".txt";
                try
                {
                    (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.WriteAllText(nmeafile, RecordQueue, true);
                    RecordQueue = ""; //Clear that out
                }
                catch (Exception)
                {
                }
            }
        }


        /// <summary>
        /// 生成 GPGGA 数据
        /// </summary>
        /// <returns></returns>
        public string GenerateGPGGAcode()
        {
            double posnum = 0;
            double minutes = 0;

            DateTime UTCTime = DateTime.UtcNow;

            //$GPGGA,052158,4158.7333,N,09147.4277,W,2,08,3.1,260.4,M,-32.6,M,,*79

            string mycode = "GPGGA,";
            if (UTCTime.Hour < 10)//  DateAndTime.Hour(UTCTime) < "10")
            {
                mycode = mycode + "0";
            }
            mycode = mycode + System.Convert.ToString(DateAndTime.Hour(UTCTime));
            if (UTCTime.Minute < 10)//DateAndTime.Minute(UTCTime) < "10")
            {
                mycode = mycode + "0";
            }
            mycode = mycode + System.Convert.ToString(DateAndTime.Minute(UTCTime));
            if (UTCTime.Second < 10)// DateAndTime.Second(UTCTime) < "10")
            {
                mycode = mycode + "0";
            }
            mycode = mycode + System.Convert.ToString(DateAndTime.Second(UTCTime));
            mycode = mycode + ",";

            posnum = 0;// (Math.Abs(OldReceiverForm.Default.NtripParam.ManualLat));
            minutes = posnum % 1;
            posnum = posnum - minutes;
            minutes = minutes * 60;
            posnum = (posnum * 100) + minutes;
            mycode = mycode + posnum.ToString("0000.00####");

            if (posnum > 0) //if (MainForm.Default.NtripParam.ManualLon > 0)
            {
                mycode = mycode + ",N,";
            }
            else
            {
                mycode = mycode + ",S,";
            }

           // posnum =  (Math.Abs(OldReceiverForm.Default.NtripParam.ManualLon));
            minutes = posnum % 1;
            posnum = posnum - minutes;
            minutes = minutes * 60;
            posnum = (posnum * 100) + minutes;
            mycode = mycode + posnum.ToString("00000.00####");

           // if (OldReceiverForm.Default.NtripParam.ManualLon > 0)
            {
                mycode = mycode + ",E,";
            }
          //  else
            {
                mycode = mycode + ",W,";
            }


            mycode = mycode + "4,10,1,200,M,1,M,";

            mycode = mycode + ((DateAndTime.Second(DateTime.Now) % 6) + 3) + ",0";
            mycode = "$" + mycode + "*" + CalculateChecksum(mycode); //Add checksum satData
            return mycode;
        }



        /// <summary>
        /// 计算校验码
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public string CalculateChecksum(string sentence)
        {
            // Calculates the checksum for a sentence
            // Loop through all chars to get a checksum
            char Character = (char)0;
            int Checksum = 0;
            foreach (char tempLoopVar_Character in sentence)
            {
                Character = tempLoopVar_Character;
                switch (Character)
                {
                    case '$':
                        break;
                    // Ignore the dollar sign
                    case '*':
                        // Stop processing before the asterisk
                        goto endOfForLoop;
                    default:
                        // Is this the prevObj value for the checksum
                        if (Checksum == 0)
                        {
                            // Yes. Set the checksum to the value
                            Checksum = Convert.ToByte(Character);
                        }
                        else
                        {
                            // No. XOR the checksum with this character's value
                            Checksum = Checksum ^ Convert.ToByte(Character);
                        }
                        break;
                }
            }
        endOfForLoop:
            // Return the checksum formatted as a two-character hexadecimal
            return Checksum.ToString("X2");
        }

    }

}
