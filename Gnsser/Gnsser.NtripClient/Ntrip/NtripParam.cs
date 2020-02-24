//2015.01.29, czs, create in pengzhou, NTRIP 访问参数。

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Gnsser.Ntrip
{
    /// <summary>
    /// NTRIP 协议类型
    /// </summary>
    public enum ProtocolType
    {
        TcpIp = 0,
        RTCM3 = 1
    }
    /// <summary>
    /// 接收机类型
    /// </summary>
    public enum ReceiverType
    {
        Unkown = 0,
        /// <summary>
        /// 诺瓦泰
        /// </summary>
        Novatel = 1
    }

    /// <summary>
    /// NTRIP 访问参数。一次访问一个设置。
    /// </summary>
    public class NtripOption : Geo.IOption
    {
        public NtripOption()
        {
            ProtocolType = Ntrip.ProtocolType.RTCM3;
            this.AllConfigs = new Dictionary<string, string>();
            Username = "tubjn";
            Password = "1014jn1988";
            ManualLon = 111;
            ManualLat = 40;
            IsWriteToLocal = true;
            this.LocalDirectory = @"D:\Temp";
            ObsFileEpochCount = 5 * 60;
            IsRequiresGGA = true;
        }
        #region  属性

        /// <summary>
        /// 是否需要GGA方式（需要本地坐标）。
        /// </summary>
        public bool IsRequiresGGA { get; set; }
        /// <summary>
        /// 一个文件的历元数量
        /// </summary>
        public int ObsFileEpochCount { get; set; }
        /// <summary>
        /// 是否写到本地
        /// </summary>
        public bool IsWriteToLocal { get; set; }
        public ProtocolType ProtocolType { get; set; }
        /// <summary>
        /// 接收机类型
        /// </summary>
        public ReceiverType ReceiverType { get; set; }

        public string ReceiverCorrDataType { get; set; }
        /// <summary>
        /// 本地目录
        /// </summary>
        public string LocalDirectory { get; set; }

        /// <summary>
        /// 数据流转发站
        /// </summary>
        public string CasterName { get; set; }
        /// <summary>
        /// 数据流转发站
        /// </summary>
        public string CasterIp { get; set; }
        /// <summary>
        /// 数据流转发站 的端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 挂载点
        /// </summary>
        public string PreferredMountPoint { get; set; }
        //public string ReceiverCorrDataType { get; set; }
        /// <summary>
        /// 手动指定的经度
        /// </summary>
        public double ManualLon { get; set; }
        /// <summary>
        /// 手动指定的纬度
        /// </summary>
        public double ManualLat { get; set; }
        /// <summary>
        /// 是否启用手动坐标
        /// </summary>
        public bool UseManualGGA { get; set; }
        public bool WriteNMEAToFile { get; set; }
        public bool NTRIPShouldBeConnected { get; set; }
        public bool SerialShouldBeConnected { get; set; }
        public bool WriteEventsToFile { get; set; }
        public int ReceiverMessageRate { get; set; }
        public string AudioFile { get; set; }
        public Dictionary<String, string> AllConfigs { get; set; }

        //public bool SerialShouldBeConnected = false;
        public int SerialPort = 1;
        public int SerialSpeed = 9600;
        public int SerialDataBits = 8;
        public int SerialStopBits = 1;

        #endregion

        #region  IO
        public void SaveNTRIPSettings(string targetfile)
        {
            string ntripsettings = "NTRIP Caster=" + this.CasterIp + "\r\n";
            ntripsettings += "NTRIP Caster Port=" + this.Port + "\r\n";
            ntripsettings += "NTRIP Username=" + this.Username + "\r\n";
            ntripsettings += "NTRIP Password=" + this.Password + "\r\n";
            ntripsettings += "NTRIP MountPoint=" + this.PreferredMountPoint + "\r\n";
            ntripsettings += "NTRIP Use Manual GGA=" + this.UseManualGGA + "\r\n";
            ntripsettings += "NTRIP Manual Latitude=" + this.ManualLat + "\r\n";
            ntripsettings += "NTRIP Manual Longitude=" + this.ManualLon + "\r\n";
            ntripsettings += "NTRIP Should be Connected=" + this.NTRIPShouldBeConnected + "\r\n";
            ntripsettings += "Serial Should be Connected=" + this.SerialShouldBeConnected + "\r\n";

            StringBuilder sb = new StringBuilder(ntripsettings);

            //  sb.AppendLine("Display Center="  +  this.dis);

            //Display Right=
            sb.AppendLine("Write Events to File=" + this.WriteEventsToFile);
            sb.AppendLine("Write NMEA to File=" + this.WriteNMEAToFile);
            sb.AppendLine("Receiver Type=" + (int)this.ReceiverType);
            sb.AppendLine("Serial Port Speed=" + this.SerialSpeed);
            sb.AppendLine("Serial Port Data Bits=" + this.SerialDataBits);
            sb.AppendLine("Serial Port Stop Bits=" + this.SerialStopBits); 

            File.WriteAllText(targetfile, ntripsettings);
        }
        /// <summary>
        /// 加载设置。文本行模式
        /// </summary>
        /// <param name="ntripconfigfile"></param>
        /// <returns></returns>
        public static NtripOption LoadNtripConfig(string ntripconfigfile)
        {
            NtripOption NtripParam = new NtripOption() { ConfigPath = ntripconfigfile };

            var lines = File.ReadAllLines(ntripconfigfile);
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("#")) continue;

                var pair = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length < 2 ) continue;

                var value = pair[1];

                NtripParam.AllConfigs.Add(pair[0].Trim(), value);


                switch (pair[0].Trim().ToLower())
                {
                    case "ntrip caster":
                        NtripParam.CasterIp = value;
                        break;
                    case "ntrip caster port":
                        NtripParam.Port = int.Parse(value);
                        break;
                    case "ntrip username":
                        NtripParam.Username = value;
                        break;
                    case "ntrip password":
                        NtripParam.Password = value;
                        break;
                    case "ntrip mountpoint":
                        NtripParam.PreferredMountPoint = value;
                        break;
                    case "serial should be connected":
                        NtripParam.SerialShouldBeConnected = bool.Parse(value);
                        break;
                    case "ntrip should be connected":
                        NtripParam.NTRIPShouldBeConnected = bool.Parse(value);
                        break;
                    case "ntrip manual longitude":
                        NtripParam.ManualLon = Double.Parse(value);
                        break;
                    case "ntrip manual latitude":
                        NtripParam.ManualLat = Double.Parse(value);
                        break;
                    case "ntrip use manual gga":
                        NtripParam.UseManualGGA = bool.Parse(value);
                        break;
                    default: break;
                }
            }
            NtripParam. InitFromConfigs();
            return NtripParam;
        }
        
        private void InitFromConfigs()
        {
            foreach (var item in this.AllConfigs)
            {
                var value = item.Value;
                switch (item.Key.ToLower())
                {
                    case "serial should be connected": 
                            this.SerialShouldBeConnected = bool.Parse(value); 
                        break;
                    case "serial port number": 
                         this.SerialPort = int.Parse(value);// (portnumber > 0 & portnumber < 1025)("串口应该在 1 到 1024 之间."); 
                        break;
                    case "serial port speed": 
                        this.SerialSpeed = int.Parse(value);//("指定的串口速度应该在 2400 到 115200 之间."); 
                        break;
                    case "serial port data bits":
                        if (value == "7")
                        {
                            this.SerialDataBits = 7;
                        }
                        else if (value == "8")
                        {
                            this.SerialDataBits = 8;
                        }
                        else
                        {
                            throw new Exception("指定串口数据位应该是 7 或 8.");
                        }
                        break;
                    case "serial port stop bits":
                        if (value == "0")
                        {
                            this.SerialStopBits = 0;
                        }
                        else if (value == "1")
                        {
                            this.SerialStopBits = 1;
                        }
                        else
                        {
                            throw new Exception("Specified Serial Port Stop bits should be 0 or 1.");
                        }
                        break;


                    case "protocol":
                        if (value.ToLower() == "rawtcpip")
                        {
                            this.ProtocolType = ProtocolType.TcpIp; 
                        }
                        else
                        {
                            this.ProtocolType = ProtocolType.RTCM3; 
                        }
                        break;



                    case "ntrip should be connected":
                        if (value.ToLower() == "true")
                        {
                            this.NTRIPShouldBeConnected = true;
                        }
                        break;
                    case "ntrip use manual gga":
                        if (value.ToLower() == "true")
                        {
                            this.UseManualGGA = true;
                        }
                        break;
                    case "ntrip only send gga once":
                        break;
                    //If LCase(value) = "yes" Then NTRIPResendGGAEvery10Seconds = False
                    case "ntrip manual latitude":
                        if (Information.IsNumeric(value))
                        {
                            double inlat = double.Parse(value);
                            if (inlat > -90 & inlat < 90)
                            {
                                this.ManualLat = inlat;
                            }
                            else
                            {
                                throw new Exception("Specified NTRIP Manual Latitude should be between -90 and 90.");
                            }
                        }
                        else
                        {
                            throw new Exception("Specified NTRIP Manual Latitude should be numeric.");
                        }
                        break;
                    case "ntrip manual longitude":
                        if (Information.IsNumeric(value))
                        {
                            double inlon = double.Parse(value);
                            if (inlon > -180 & inlon < 180)
                            {
                                this.ManualLon = inlon;
                            }
                            else
                            {
                                throw new Exception("Specified NTRIP Manual Longitude should be between -180 and 180.");
                            }
                        }
                        else
                        {
                            throw new Exception("Specified NTRIP Manual Longitude should be numeric.");
                        }
                        break;

                    case "audio alert file":
                        AudioFile = value;
                        break;

                    case "write events to file":
                        if (value.ToLower() == "true")
                        {
                            WriteEventsToFile = true;
                        }
                        break;
                    case "write nmea to file": 
                            WriteNMEAToFile = bool.Parse(value); 
                        break;

                    //case "display center":
                    //    GPS.Disp2 = value.ToLower();
                    //    break;
                    //case "display right":
                    //    GPS.Disp3 = value.ToLower();
                    //    break;

                    case "receiver type":
                        switch (value.ToLower())
                        {
                            case "novatel":
                                ReceiverType = (ReceiverType)1;
                                break;
                            default:
                                ReceiverType = (ReceiverType)0;
                                break;
                        }
                        break;

                    case "receiver correction format":
                        switch (value.ToLower())
                        {
                            case "rtcm":
                                ReceiverCorrDataType = "RTCM";
                                break;
                            case "rtcmv3":
                                ReceiverCorrDataType = "RTCMV3";
                                break;
                            case "cmr":
                                ReceiverCorrDataType = "CMR";
                                break;
                            case "rtca":
                                ReceiverCorrDataType = "RTCA";
                                break;
                            case "omnistar":
                                ReceiverCorrDataType = "OMNISTAR";
                                break;
                            default:
                                ReceiverCorrDataType = "NOVATEL";
                                break;
                        }
                        break;

                    case "receiver message rate":
                        switch (value)
                        {
                            case "5":
                                ReceiverMessageRate = 5;
                                break;
                            case "10":
                                ReceiverMessageRate = 10;
                                break;
                            default:
                                ReceiverMessageRate = 1;
                                break;
                        }
                        break;


                    default:
                        //This will be blank if no settings were loaded
                        //throw new Exception("无效设置，Just FYI, the \"" + prn.Key + "\" keyPrev in the settings file isn\'t valid, so it was skipped.");
                        break;
                }
            }
        }

        public string ConfigPath { get; set; }

        public void SaveSetting(string key1, string value1, string key2 = "", string value2 = "", string key3 = "", string value3 = "")
        {
            NtripOption.SaveSetting(ConfigPath, key1, value1, key2, value2, key3, value3);
        }
        ///
        public static void SaveSetting(string ntripconfigFile, string key1, string value1, string key2 = "", string value2 = "", string key3 = "", string value3 = "")
        {
            if (!File.Exists(ntripconfigFile)) //File doesn't exist. Create it.
            {
                System.IO.StreamWriter fn = new System.IO.StreamWriter(System.IO.File.Open(ntripconfigFile, System.IO.FileMode.Create));
                fn.WriteLine("# This is the NTRIP Client settings file. You need to use the format \"Key=Value\" for all settings.");
                fn.WriteLine("# Any line that starts with a # symbol will be ignored.");
                fn.WriteLine("");
                fn.Close();
            }


            string[] keyvalpair = new string[2];
            System.IO.StreamReader oRead = System.IO.File.OpenText(ntripconfigFile);
            string linein = "";
            string newfile = "";
            bool foundkey1 = false;
            bool foundkey2 = false;
            bool foundkey3 = false;

            while (oRead.Peek() != -1)
            {
                linein = oRead.ReadLine().Trim();
                if (linein.Length < 3)
                {
                    //Line is too short
                    newfile += linein;
                }
                else if (Strings.Asc(linein) == 35)
                {
                    //Line starts with a #
                    newfile += linein;
                }
                else if (linein.IndexOf("=") + 1 < 2)
                {
                    //There is no equal sign in the string
                    newfile += linein;
                }
                else
                {
                    keyvalpair = Strings.Split(linein, "=", 2, (Microsoft.VisualBasic.CompareMethod)0);
                    if (keyvalpair[0].Trim().ToLower() == key1.ToLower())
                    {
                        //Found the right keyPrev, update it.
                        newfile += keyvalpair[0] + "=" + value1;
                        foundkey1 = true;
                    }
                    else if (key2.Length > 0 && keyvalpair[0].Trim().ToLower() == key2.ToLower())
                    {
                        newfile += keyvalpair[0] + "=" + value2;
                        foundkey2 = true;
                    }
                    else if (key3.Length > 0 && keyvalpair[0].Trim().ToLower() == key3.ToLower())
                    {
                        newfile += keyvalpair[0] + "=" + value3;
                        foundkey3 = true;
                    }
                    else
                    {
                        newfile += linein;
                    }
                }
                newfile += "\r\n";
            }
            oRead.Close();

            if (!foundkey1)
            {
                newfile += key1 + "=" + value1 + "\r\n";
            }
            if (key2.Length > 0 && !foundkey2)
            {
                newfile += key2 + "=" + value2 + "\r\n";
            }
            if (key3.Length > 0 && !foundkey3)
            {
                newfile += key3 + "=" + value3 + "\r\n";
            }


            System.IO.StreamWriter sWriter = new System.IO.StreamWriter(ntripconfigFile);
            sWriter.Write(newfile);
            sWriter.Flush();
            sWriter.Close();
        }
        #endregion
    }
}
