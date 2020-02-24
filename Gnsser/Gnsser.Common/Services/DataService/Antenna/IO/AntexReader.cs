//2014.05.22, Cui Yang, created,改进自GPSTk。
//2014.07.22, czs, Refactoring，去掉了大量冗余代码.
//2014.08.18, czs, edit，模块化程度提高，改 抛出异常 为返回null。
//2015.04.13, cy, edit, 修复了一个致命错误，找不到Antenna信息时，不应该直接返回null，先判断是否存在对应NONE类型的天线，返回不考虑天线罩的天线。
//2017.10.31, lly, edit in zz, 天线修改为igs14.atx，修复标签截断字符不足的错误
//2018.05.02, czs, edit in hmx, 天线罩读取纠错
//2018.08.01, czs, edit in HMX, 重构，分别采用缓存存储卫星和测站的天线

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Times;
using Geo.Utils;
using Gnsser.Data.Rinex;
using Geo.Times;
using Geo;
using Geo.IO;

namespace Gnsser.Data
{

    /// 参加：
    /// rcvr_ant.tab: Offical IGS naming conventions for GNSS equipment
    /// antex13.txt: ANTEX format definition
    /// igs05_wwww.atx: Absolute IGS phase center corrections for satellite and receiver antennas. Field 'wwww'represents GPS week of last file change
    /// igs05.atx: Link to latest igs05_wwww.atx file
    /// igs01.atx: Relative IGS phase center corrections for satellite and receiver antennas
    /// <summary>
    /// 天线文件读取和查找获取.
    /// </summary>
    public class AntexReader
    {
        Log log = new Log(typeof(AntexReader));

        /// <summary>
        /// 天线文件读取器的默认构造函数。
        /// </summary>
        /// <param name="fileName">Antex satData file to read</param>
        public AntexReader(string fileName)
        {
            this.FilePath = fileName;
            this.SiteAntennas = new BaseDictionary<string, Antenna>("测站天线缓存器");
            this.SatAntennas = new BaseDictionary<string, BaseDictionary<TimePeriod, Antenna>>("卫星天线缓存器", 
                new Func<string, BaseDictionary<TimePeriod, Antenna>>(k=>new BaseDictionary<TimePeriod, Antenna>()));

            this.Header = ReadHeader(fileName);
        }

        #region 属性
        /// <summary>
        /// 测站天线，通过类型获取，无时间相关。用于存储已经搜寻过的天线数据，下一次使用时，可以直接获取之。
        /// </summary>
        private BaseDictionary<string, Antenna> SiteAntennas { get; set; }
        /// <summary>
        /// 卫星相关的天线和时间相关。
        /// </summary>
        private BaseDictionary<string, BaseDictionary<TimePeriod, Antenna>> SatAntennas{ get; set; }
        /// <summary>
        /// 头部信息。
        /// </summary>
        public AntennaHeader Header { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath { get; set; }
        #endregion

        #region 基本的文件读取,采用的静态方法
        
        /// <summary>
        /// 获取文件中所有的天线对象。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static AntennaFile ReadFile(string fileName)
        {
            AntennaFile file = new AntennaFile();

            file.Header = ReadHeader(fileName);

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                RinexUtil.SkipHeader(sr);

                while ((line = sr.ReadLine()) != null)
                {
                    Antenna Antenna = ParseAntenna(line, sr, file.Header);
                    file.Antennas.Add(Antenna);
                }
            }

            return file;
        }

        #region 解析头文件
        /// <summary>
        /// 读取头文件。
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        private static AntennaHeader ReadHeader(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                AntennaHeader header = new AntennaHeader();
                //Read one line from file
                string line = sr.ReadLine();
                //Get label. remove trailing and leading blanks
                string label = line.Substring(60).Trim();

                while (label != AntexLabel.END_OF_HEADER)
                {
                    switch (label)
                    {
                        case AntexLabel.ANTEX_VERSION_SYST:
                            header.Version = Convert.ToDouble(line.Substring(0, 8));
                            char sys = line[20];
                            header.SatelliteSystem = (SatelliteSystem)Enum.Parse(typeof(SatelliteSystem), sys.ToString());
                            break;
                        //Process PCV type line
                        case AntexLabel.PCV_TYPE_REFANT:
                            ParsePcvType(header, line);
                            break;
                        case AntexLabel.COMMENT:
                            header.Comments.Add(line.Substring(0, 60).Trim());
                            break;
                        default: break;
                    }
                    line = sr.ReadLine();
                    label = line.Substring(60).Trim();
                }
                return header;
            }
        }

        /// <summary>
        /// 解析PCV
        /// </summary>
        /// <param name="header"></param>
        /// <param name="line"></param>
        private static void ParsePcvType(AntennaHeader header, string line)
        {
            char pcvt = line[0];
            switch (pcvt)
            {
                case 'A':
                    header.PcvType = PcvType.Absolute;
                    break;
                case 'R':
                    header.PcvType = PcvType.Relative;
                    header.ReferenceAntena = line.Substring(20, 20).Trim();
                    if (header.ReferenceAntena == "")
                    {
                        header.ReferenceAntena = "AOAD/M_T";//default
                    }
                    header.ReferenceAntenaSerial = line.Substring(40, 20).Trim();
                    break;
                default:
                    throw new Exception("天线头部的 PCV 类型无效.");
            }
        }
        #endregion

        #region 从文件中解析一个天线段
        /// <summary>
        /// 解析一个天线段。
        ///  Fill most Antenna satData
        /// </summary>
        /// <param name="firstLine"></param>
        /// <param name="sr"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private static Antenna ParseAntenna(string firstLine, StreamReader sr, AntennaHeader header)
        {
            //These flags take care of "Valid From" and "Valid Until"
            bool validFromPresent = false;
            bool validUntilPresent = false;
            //Create 'Antenna' object to be returned
            Antenna antenna = CreateAntenna(firstLine);
            antenna.Header = header;

            //Read the rest of satData
            string line;
            //Read one line from file
            line = sr.ReadLine();
            string label = line.Substring(60).Trim();
            // Repeat until 'endOfAntenna' line
            while (label != AntexLabel.END_OF_ANTENNA)
            {
                //Process 'calibrationMehtod' line
                switch (label)
                {
                    case AntexLabel.METH_BY_DATE:
                        antenna.CalibrationMethod = (line.Substring(0, 20).Trim());
                        antenna.Agency = line.Substring(20, 20).Trim();
                        antenna.NumOfAntennas = line.Substring(40, 6).Trim();
                        antenna.Date = line.Substring(50, 10).Trim();
                        break;
                    case AntexLabel.DAZI:
                        antenna.DeltaAzimuth = (Convert.ToDouble(line.Substring(2, 6).Trim()));
                        break;
                    case AntexLabel.ZEN1_ZEN2_DZEN:
                        antenna.ZenithStart = (Convert.ToDouble(line.Substring(2, 6).Trim()));
                        antenna.ZenithEnd = (Convert.ToDouble(line.Substring(8, 6).Trim()));
                        antenna.DeltaZenith = (Convert.ToDouble(line.Substring(14, 6).Trim()));

                        break;
                    case AntexLabel.NUM_OF_FREQUENCIES:
                        antenna.NumOfFrequencies = (Convert.ToInt32(line.Substring(0, 6).Trim()));
                        break;
                    case AntexLabel.VALID_FROM:
                        Time valFrom = Time.Parse(line.Substring(0, 43));
                        antenna.ValidDateFrom = (valFrom);
                        validFromPresent = true;// Mark that we found "Valid From"

                        break;
                    case AntexLabel.VALID_UNTIL: // Get validity as Year, Month, Day, Hour, Min, Sec
                        Time valUntil = Time.Parse(line.Substring(0, 43));
                        antenna.ValidDateUntil = (valUntil);
                        // Mark that we found "Valid Until"
                        validUntilPresent = true;
                        break;
                    case AntexLabel.SINEX_CODE:  //Get antenna Sinex Code
                        antenna.SinexCode = line.Substring(0, 10).Trim();
                        break;
                    case AntexLabel.COMMENT:
                        antenna.AddComment(line.Substring(0, 60).Trim());
                        break;
                    case AntexLabel.START_OF_FREQUENCY://Get frequency indicator
                        label = ParseFrequency(sr, antenna, line);
                        break;
                    case AntexLabel.START_OF_FREQ_RMS://Get frequency indicator
                        label = ParseFrequencyRms(sr, antenna, ref line);
                        break;
                    default:
                        break;
                }
                //Read another line from file      
                line = sr.ReadLine();
                //Get current label
                label = line.Substring(60).Trim();
            }
            //Take care of "Valid From" field if it was not present
            if (!validFromPresent)
            {
                //Set as "DayTime: BEGINNING_OF_TIME" 
                antenna.ValidDateFrom = (Time.MinValue);
            }
            if (!validUntilPresent)
            {
                antenna.ValidDateUntil = (Time.MaxValue);
            }
            return antenna;
        }

        #region 解析细节
        /// <summary>
        ///  The section includes the rms values of  the phase center eccentricities and of the phase pattern values.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="antenna"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string ParseFrequencyRms(StreamReader sr, Antenna antenna, ref string line)
        {
            string label;
            string freqString = line.Substring(3, 3).Trim();

            RinexSatFrequency freq = RinexSatFrequency.Parse(freqString);

            // Read new line and extract label
            line = sr.ReadLine();
            label = line.Substring(60).Trim();
            //Repeat until 'endOfFreqRMS' line
            while (label != AntexLabel.END_OF_FREQ_RMS)
            {
                //Process this line
                if (label == AntexLabel.NORTH_EAST_UP)//Rms of the eccentricities (in milli meters)
                {
                    //Add antenna eccentricities RMS, as Meters
                    antenna.SetAntennaRmsEcc(freq, ParseNEU(line));
                }
                else
                {
                    //Check if this line contains "NOAZI" pattern RMS
                    if (line.Substring(3, 5).Trim() == "NOAZI") //表明不依赖方位角,如卫星
                        {
                            StringUtil.TrimFirstWord(ref line);
                            List<double> pcVec = ParseArrayLine(antenna, line);
                            antenna.SetAntennaNoAziRms(freq, pcVec);
                        }
                    else//依赖方位角,如测站
                    {
                            double azi = Convert.ToDouble(StringUtil.TrimFirstWord(ref line));
                            List<double> pcVec = ParseArrayLine(antenna, line);
                            antenna.SetAntennaPatternRms(freq, azi, pcVec); 
                    }
                }
                //Read new line and extract label
                line = sr.ReadLine();
                label = line.Substring(60).Trim();
            }

            return label;
        }

        /// <summary>
        /// 解析主体数据部分
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="antenna"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string ParseFrequency(StreamReader sr, Antenna antenna, string line)
        {
            var freqString = line.Substring(3, 3).Trim();
            var freq = RinexSatFrequency.Parse(freqString);
            // Read new line and extract label
            line = sr.ReadLine();
            var label = line.Substring(60).Trim();
            //Repeat until 'endOfFreq' line
            while (label != AntexLabel.END_OF_FREQUENCY)
            {
                //Eccentricities of the mean antenna phase center relative to the antenna reference point (ARP)(in millimeters).
                //平均天线相位中心相对于天线参考点（ARP）的偏心率（以毫米计）。
                if (label == AntexLabel.NORTH_EAST_UP)
                {
                    var neu = ParseNEU(line);
                    antenna.SetAntennaEcc(freq, neu);
                }
                else
                {
                    //Check if this line contains "NOAZI" pattern
                    if (line.Substring(3, 5).Trim() == "NOAZI")//常常为第一行：表明不依赖方位角,如卫星
                    {
                        StringUtil.TrimFirstWord(ref line);
                        List<double> pcVec = ParseArrayLine(antenna, line);
                        antenna.SetAntennaNoAziPattern(freq, pcVec);
                    }
                    else//依赖方位角,如测站
                    {
                        double azi = Convert.ToDouble(StringUtil.TrimFirstWord(ref line));
                        List<double> pcVec = ParseArrayLine(antenna, line);
                        antenna.SetAntennaPattern(freq, azi, pcVec);
                    }
                }
                //Read new line and extract label
                line = sr.ReadLine();
                label = line.Substring(60).Trim();
            }//End of While(label != endOfFreq)

            return label;
        }
        /// <summary>
        ///  Add antenna eccentricities or rms , as METERS
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static Geo.Coordinates.NEU ParseNEU(string line)
        {
            double north = Convert.ToDouble(line.Substring(0, 10).Trim()) / 1000.0;
            double east = Convert.ToDouble(line.Substring(10, 10).Trim()) / 1000.0;
            double upper = Convert.ToDouble(line.Substring(20, 10).Trim()) / 1000.0;
            Geo.Coordinates.NEU neu = new Geo.Coordinates.NEU(north, east, upper);
            return neu;
        }

        private static List<double> ParseArrayLine(Antenna antenna, string line)
        {
            List<double> pcVec = new List<double>();
            for (double zen = antenna.ZenithStart; zen <= antenna.ZenithEnd; zen += antenna.DeltaZenith)
            {
                double value = Convert.ToDouble(StringUtil.TrimFirstWord(ref line));
                pcVec.Add(value / 1000.0);//Extract values (they are in milimeters) store values as meters
            }

            return pcVec;
        }

        /// <summary>
        /// 创建一个天线实例，采用第一行解析初始化。
        /// </summary>
        /// <param name="firstLine">第一行</param>
        /// <returns></returns>
        private static Antenna CreateAntenna(string firstLine)
        {
            Antenna antenna = new Antenna();
            antenna.AntennaType = (firstLine.Substring(0, 15).Trim()).ToUpper();
            antenna.Radome = (firstLine.Substring(16, 4).Trim());

            antenna.SerialOrSatFreqCode = (firstLine.Substring(20, 20).Trim());
            antenna.SatCode = firstLine.Substring(40, 10).Trim();
            antenna.CosparID = firstLine.Substring(50, 10).Trim();
            return antenna;
        }
        #endregion
        #endregion
        #endregion

        #region 天线获取读取到缓存
         
        /// <summary>
        /// 从指定的IGS天线类型中，获取天线数据。
        /// 首先判断缓存中是否具有该数据，如果没有则从文件中读取，如果也没有 则抛出异常，or 返回空 null？？
        ///  Method to get antenna satData from a given IGS model.
        /// </summary>
        /// <param name="model">20个字符或15以内，IGS天线名称或加上天线罩</param>
        /// <returns></returns>
        public virtual Antenna GetAntenna(string model)
        {
            model = model.ToUpper(); 
            string typeName = model.Trim();
            if (model.Length > 15) {//取前15个字
                typeName = model.Substring(0, 15).Trim();
            }

            string uRadome = "NONE"; 
            //天线类型如果小于17个字符，则认为没有天线罩 
            if (model.Length >= 20)
            {
                uRadome = Geo.Utils.StringUtil.SubString(model, 16, 4).Trim(); 
            }
            var key = StringUtil.FillSpaceRight(typeName, 15) + StringUtil.FillSpaceRight(uRadome, 4);
            if (SiteAntennas.Count > 0 && SiteAntennas.Contains(key))
            { 
                return SiteAntennas[key];
            }

            Antenna antenna = FindFromFile(typeName, uRadome);

            if(antenna == null && uRadome != "NONE")//如果没有找到，尝试使用NONE
            {
                log.Warn("没有找到天线 " + model + " 啊！😭😭😭 不着急，我们将尝试无罩信息替代！");
                antenna = FindFromFile(typeName, "NONE");
                if(antenna == null)
                {
                    log.Warn("完蛋了！ " + typeName + " NONE 也没有啊！😭😭😭 ！请尝试到 https://www.ngs.noaa.gov/ANTCAL/ 下载对应天线改正信息，并追加到.atx文件中！");
                }
                else
                {
                    log.Warn("找到！ " + typeName + " NONE 了！ 讲究用着吧，如果要求高，请尝试到 https://www.ngs.noaa.gov/ANTCAL/ 下载对应天线改正信息，并追加到.atx文件中！");
                }
            }


            SiteAntennas.Add(key, antenna);  //无论找到否，都添加，避免下次继续查找

            return antenna;
        }

        /// <summary>
        /// 获取天线数据，根据给定的序列号和时间获取。
        /// </summary>
        /// <param name="serial"> Antenna serial number, 如卫星的PRN</param>
        /// <param name="epoch">Validity epoch.</param>
        /// <returns></returns>
        public virtual Antenna GetAntenna(string serial, Time epoch)
        {
            //Get serial
            string uSerial = serial.ToUpper().Trim();

            if (SatAntennas.Contains(uSerial))
            {
                var satAntennas = SatAntennas.GetOrCreate(uSerial);
                foreach (var item in satAntennas)
                {
                    if (item.TimePeriod.Contains(epoch))
                    {
                        return item;
                    }
                } 
            }

            var antenna = FindFromFile(epoch, uSerial);

            if ( antenna != null && antenna.TimePeriod.Contains(epoch))
            {
                SatAntennas.GetOrCreate(uSerial).Add(antenna.TimePeriod, antenna); 
                return antenna;
            }

            // NULL,   避免重复读取，耗费时间
            SatAntennas.GetOrCreate(uSerial).Add( new TimePeriod(epoch - 6 * 3600, epoch +  6 * 3600), antenna);
            return antenna;
        }

        #region 从文件中查找
        /// <summary>
        /// 从天线文件查找一个,只负责查找
        /// </summary>
        /// <param name="uModel"></param>
        /// <param name="uRadome"></param>
        /// <returns></returns>
        private Antenna FindFromFile(string uModel, string uRadome = "NONE")
        {
            if (String.IsNullOrWhiteSpace(uRadome))
            {
                uRadome = "NONE";
            }

            Antenna antenna = null; 
            //Fla that signals if we found the antenna
            bool antennaFound = false;
           
            //read the satData stream/file from the beginning
            using (StreamReader sr = new StreamReader(FilePath))
            {
                //Repeat until antenna is found or End of file
                while (!antennaFound)
                {
                    string label = null;
                    string line = null;
                   
                    //lool for 'typeSerial' line
                    while (label != AntexLabel.TYPE_ERIAL_NO)
                    {
                        //Read one line from file
                        line = sr.ReadLine(); 
                        
                        if (line == null)
                        {
                            return antenna;
                        }
                        //修改截断错误
                        label = line.Substring(60).Trim();
                    }

                    //Check if model matches. Read only model, not radome//Check if radome matches
                    if (uModel == line.Substring(0, 15).Trim()
                        && line.Substring(16, 4).Trim() == uRadome)
                    {
                        //found the antenna,fill it with satData
                        antenna = ParseAntenna(line, sr, this.Header);
                        antennaFound = true;
                        return antenna;
                    }

                }//End of while(!=antennaFound)...
            }
           
            return antenna;
        }

        /// <summary>
        /// 通过 SERIAL 和时段查找，这个通常为卫星接收机。
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="uSerial">G01，E01d等</param>
        /// <returns></returns>
        private Antenna FindFromFile(Time epoch, string uSerial)
        {
            Antenna antenna = null;
            //read the satData stream/file from the beginning
            using (StreamReader sr = new StreamReader(FilePath))
            {
                //flag that signals if we found the antenna
                bool antennaFound = false;
                //Repeat until antenna is found or End of file
                while (!antennaFound)
                {
                    string label = null;
                    string line = null;
                    //lool for 'typeSerial' line
                    while (label != AntexLabel.TYPE_ERIAL_NO)
                    {
                        //Read one line from file
                        line = sr.ReadLine();
                        if (line == null) return null;
                        label = line.Substring(60).Trim();
                    }
                    //Check if serial matches
                    if (uSerial == line.Substring(20, 20).Trim())
                    {
                        //found the antenna,fill it with satData
                        antenna = ParseAntenna(line, sr, this.Header);

                        if (antenna.TimePeriod.Contains(epoch))
                        {  
                            antennaFound = true; 
                            return antenna;
                        }
                    }
                }
            }

            return antenna;
        }
        #endregion
        #endregion

        /// <summary>
        ///是否绝对标定
        /// </summary>
        public bool IsAbsolute
        {
            get
            {
                return this.Header.IsAbsolute;
            }
        }
    }
}
