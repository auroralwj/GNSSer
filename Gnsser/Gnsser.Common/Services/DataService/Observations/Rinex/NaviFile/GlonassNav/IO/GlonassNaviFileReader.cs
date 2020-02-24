//2014.12.19, czs, edit in namu, 添加注释，增加 V3 读取

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Times; 

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// Glonass导航文件的读取。
    /// 包含卫星编号，时刻，轨道根数等。
    /// </summary>
    public class GlonassNaviFileReader
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">路径名称</param>
        public GlonassNaviFileReader(string fileName)
        {
            this.FileName = fileName;
            if (!File.Exists(FileName)) throw new FileNotFoundException(FileName, "FileName");

            if (FileName == null) throw new ArgumentException("路径不可为空！", "FileName");
        }

        /// <summary>
        /// 当前文件路径。
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// GLONASS导航文件，适用于 Rinex V2 V3
        /// </summary>
        /// <returns></returns>
        public GlonassNavFile Read()
        {
            //NavFileHeader header = new NavigationFlieReader(FileName).ReadHeader();
            return ReadFileV2V3(FileName);
        }

        /// <summary>
        /// 读取GLONASS导航文件
        /// </summary>
        /// <param name="navFilePath">路径</param>
        /// <returns></returns>
        public static GlonassNavFile ReadFileV2V3(string navFilePath)
        {
            GlonassNavFile f = new GlonassNavFile();
            f.Header = ParamNavFileReader.ReadHeader(navFilePath); 

            //测试版本
            if (f.Header.Version == 0)
            {
                using (StreamReader sr = new StreamReader(navFilePath, UnicodeEncoding.UTF8))
                {
                    RinexUtil.SkipHeader(sr);
                    try
                    {
                        GlonassNaviFileReader.ReadRecordV2(sr, f.Header);
                        GlonassNaviFileReader.ReadRecordV2(sr, f.Header);
                        //  f.Header.Version = 2.0;
                    }
                    catch
                    {
                        f.Header.Version = 1.0;
                    }
                }
            }

            using (StreamReader sr = new StreamReader(navFilePath, UnicodeEncoding.UTF8))
            {
                RinexUtil.SkipHeader(sr);

                while (sr.Peek() != -1)
                {
                    GlonassNavRecord record = null;
                    if(f.Header.Version < 3 )
                        record = GlonassNaviFileReader.ReadRecordV2(sr, f.Header);
                    else if (f.Header.Version >= 3)
                        record = GlonassNaviFileReader.ReadRecordV3(sr, f.Header);
                    else
                    {
                        throw new ArgumentException(" 导航文件版本 " + f.Header.Version);
                    }
                    f.Add(record);
                }
            }
            return f;
        }

        /// <summary>
        /// 读取记录
        /// </summary>
        /// <param name="sr">数据流读取器</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        public static GlonassNavRecord ReadRecordV2(StreamReader sr, NavFileHeader header)
        { 
            GlonassNavRecord record = new GlonassNavRecord(); 
            string line = sr.ReadLine(); 
           GlonassNaviFileReader.ParseFirstLineV2(line, header, record);

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            double x = double.Parse(line.Substring(3, 19));
            double vx = double.Parse(line.Substring(22, 19));
            double vvx = double.Parse(line.Substring(41, 19));   
            record.Health = double.Parse(line.Substring(60, 19));   

            line = sr.ReadLine();
            line = line.Replace('D', 'E'); 
            double y = double.Parse(line.Substring(3, 19));
            double vy = double.Parse(line.Substring(22, 19));
            double vvy = double.Parse(line.Substring(41, 19));
            record.FrequencyNumber = double.Parse(line.Substring(60, 19));

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            double z = double.Parse(line.Substring(3, 19));
            double vz = double.Parse(line.Substring(22, 19));
            double vvz = double.Parse(line.Substring(41, 19));
            record.FrequencyNumber = double.Parse(line.Substring(60, 19));
            record.AgeOfOper = double.Parse(line.Substring(60, 19));
          
            record.XYZ = new XYZ(x,y,z) * 1000.0; // km -> m        
            record.XyzVelocity = new XYZ(vx, vy, vz) * 1000.0; // km -> m 
            record.XyzAcceleration = new XYZ(vvx, vvy, vvz) * 1000.0; // km -> m
            return record;
        }
        /// <summary>
        /// RINEX 3.0 记录读取。
        /// </summary>
        /// <param name="sr">数据流</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        public static GlonassNavRecord ReadRecordV3(StreamReader sr, NavFileHeader header)
        { 
            GlonassNavRecord record = new GlonassNavRecord();
            string line = sr.ReadLine();
            GlonassNaviFileReader.ParseFirstLineV3(line, record);

            return ReadRecordBodyV3(sr, record);
        }
        /// <summary>
        /// 读取记录体，V3。
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="record"></param> 
        /// <returns></returns>
        public static GlonassNavRecord ReadRecordBodyV3(StreamReader sr, GlonassNavRecord record)
        {
           string line = sr.ReadLine();
            line = line.Replace('D', 'E');
            double x = double.Parse(line.Substring(4, 19));
            double vx = double.Parse(line.Substring(23, 19));
            double ddx = double.Parse(line.Substring(42, 19));
            record.Health = double.Parse(line.Substring(61, 19));

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            double y = double.Parse(line.Substring(4, 19));
            double vy = double.Parse(line.Substring(23, 19));
            double ddy = double.Parse(line.Substring(42, 19));
            record.FrequencyNumber = double.Parse(line.Substring(61, 19));

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            double z = double.Parse(line.Substring(4, 19));
            double vz = double.Parse(line.Substring(23, 19));
            double ddz = double.Parse(line.Substring(42, 19));
            record.AgeOfOper = double.Parse(line.Substring(61, 19));

            record.XYZ = new XYZ(x, y, z) * 1000.0; // km -> m       
            record.XyzVelocity = new XYZ(vx, vy, vz) * 1000.0; // km -> m 
            record.XyzAcceleration = new XYZ(ddx, ddy, ddz) * 1000.0; // km -> m

            return record;
        }

        /// <summary>
        /// 分解第一行。V2
        /// </summary>
        /// <param name="line">行</param>
        /// <param name="header">头部</param>
        /// <param name="record">钟差</param>
        public static void ParseFirstLineV2(string line, NavFileHeader header, GlonassNavRecord record)
        {
            line = line.Replace('D', 'E');
            record.Prn = SatelliteNumber.Parse(line.Substring(0, 2), header.SatelliteType);
            record.Time = Time.Parse(line.Substring(2, 20));
            string val = line.Substring(22, 19);
            record.ClockBias = double.Parse(val);
            record.RelativeFrequencyBias = double.Parse(line.Substring(41, 19));
            record.MessageTime = double.Parse(line.Substring(60, 19));
        }
        /// <summary>
        /// 分解第一行。V3
        /// </summary>
        /// <param name="line">行</param>
        /// <param name="header">头部</param>
        /// <param name="record">钟差</param>
        public static void ParseFirstLineV3(string line, GlonassNavRecord record)
        {
            line = line.Replace('D', 'E');
            record.Prn = SatelliteNumber.Parse(line.Substring(0, 3));
            record.Time = Time.Parse(line.Substring(4, 19));
            string val = line.Substring(23, 19);
            record.ClockBias = double.Parse(val);
            record.RelativeFrequencyBias = double.Parse(line.Substring(42, 19));
            record.MessageTime = double.Parse(line.Substring(61, 19));
        }


        /// <summary>
        /// 直接返回头文件原纪录。
        /// </summary>
        /// <param name="rinexFileName">RINEX 文件路径</param>
        /// <returns></returns>
        public static string ReadText(string rinexFileName)
        {
            using (TextReader r = new StreamReader(rinexFileName, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                string line = null;
                while ((line = r.ReadLine()) != null)
                {
                    //中文字符支持
                    int nonAscCount = StringUtil.GetNonAscCount(line.Substring(0, 60 > line.Length ? line.Length : 60));
                    string headerLabel = line.Substring(60 - nonAscCount).TrimEnd();//header label 61-80
                    if (headerLabel.Contains(RinexHeaderLabel.END_OF_HEADER)) break;

                    sb.AppendLine(line);
                }
                return sb.ToString();
            }
        }
    }
}
