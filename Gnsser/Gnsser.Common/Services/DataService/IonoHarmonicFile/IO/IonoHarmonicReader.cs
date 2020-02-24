//2018.05.25, czs, create in HMX, CODE电离层球谐函数

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gnsser.Times;
using Geo.Times;
using Geo;
using Geo.IO;
using Geo.Utils;
using Geo.Coordinates;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{
    /// <summary>
    /// 电离层 记录部分，不适用于遍历，更适用于一次性读取。
    /// 遍历的话，先全读取数据部分，遍历的只是最后RMS部分。
    /// </summary>
    public class IonoHarmonicReader //: Geo.IO.AbstractTimedStreamReader<IonoSection>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        public IonoHarmonicReader(string filePath)
            //: base(filePath)
        {
            FilePath = filePath;
        } 
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 头部信息。
        /// </summary>
        public IonoHarmonicHeader Header { get; set; }

        /// <summary>
        ///  读取RINEX文件的头文件。
        /// </summary>
        /// <param name="reader">数据流</param>
        /// <returns></returns>
        public IonoHarmonicHeader ReadHeader(StreamReader reader)
        {
            //if (!Geo.Utils.FileUtil.IsValid(path)) { return null; }
            IonoHarmonicHeader header = new IonoHarmonicHeader();
            //header.FileName = Path.GetFileName(path); 
            int lineIndex = 0;
            string line = null;
            while ((line = ReadNextNoNullLine(reader)) != null)
            {
                lineIndex++;
                if (line.StartsWith(IonoHarmonicHeaderLabel.COEFFICIENTS)) break;
                if (line.StartsWith(IonoHarmonicHeaderLabel.DEGREE)) break;

                string strVal = GetValueString(line);

                if (line.StartsWith(IonoHarmonicHeaderLabel.MODEL_NUMBER_STATION_NAME))
                {
                    header.ModelNumberStationName = strVal;
                    continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.MODEL_TYPE))
                {
                    header.ModelType = strVal; continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.MAXIMUM_DEGREE_OF_SPHERICAL_HARMONICS))
                {
                    header.MaxDegree = Geo.Utils.StringUtil.ParseInt(strVal); continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.MAXIMUM_ORDER))
                {
                    header.MaxOrder = Geo.Utils.StringUtil.ParseInt(strVal); continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.EVELOPMENT_WITH_RESPECT_TO))
                {
                    line = ReadNextNoNullLine(reader);
                    strVal = GetValueString(line);
                    header.IsGeographicalOrGeomeagnetic = Geo.Utils.StringUtil.ParseInt(strVal) == 1;

                    line = ReadNextNoNullLine(reader);
                    strVal = GetValueString(line);
                    header.IsMeanOrTruePosOfTheSun = Geo.Utils.StringUtil.ParseInt(strVal) == 1;

                    continue;
                }

                if (line.StartsWith(IonoHarmonicHeaderLabel.MAPPING_FUNCTION))
                {
                    header.MappingFunction = (IonoMappingFunction) Geo.Utils.StringUtil.ParseInt(strVal); continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.HEIGHT_OF_SINGLE_LAYER_AND_ITS_RMS_ERROR))
                {
                    RmsedNumeral rmsedVal = ParseRmedNumeral(strVal);
                    header.LayerHeight = rmsedVal; continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.COORDINATES_OF_EARTH_CENTERED_DIPOLE_AXIS))
                {
                    line = ReadNextNoNullLine(reader);
                    strVal = GetValueString(line);
                    line = ReadNextNoNullLine(reader);
                    strVal = GetValueString(line); continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.PERIOD_OF_VALIDITY))
                {
                    line = ReadNextNoNullLine(reader);
                    strVal = GetValueString(line);
                    var start = Time.Parse(strVal);
                    line = ReadNextNoNullLine(reader);
                    strVal = GetValueString(line);
                    var end = start;
                    if ( !String.IsNullOrWhiteSpace( strVal))
                    {
                        end = Time.Parse(strVal);
                    }
                    header.ValidPeroid = new TimePeriod(start, end); continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.LATITUDE_BAND_COVERED))
                {
                    line = ReadNextNoNullLine(reader);
                    strVal = GetValueString(line);
                    var min = Geo.Utils.StringUtil.ParseDouble(strVal);
                    line = ReadNextNoNullLine(reader);
                    strVal = GetValueString(line);
                    var max = Geo.Utils.StringUtil.ParseDouble(strVal);
                    header.LatSpan = new NumerialSegment(min, max);
                    continue;
                }
                if (line.StartsWith(IonoHarmonicHeaderLabel.ADDITIONAL_INFORMATION))
                {
                    while ((line = ReadNextNoNullLine(reader)) != null)
                    {
                        if (line.StartsWith(IonoHarmonicHeaderLabel.COEFFICIENTS)) break;
                        if (line.StartsWith(IonoHarmonicHeaderLabel.DEGREE)) break;
                        strVal = GetValueString(line);

                        if (line.StartsWith(IonoHarmonicHeaderLabel.NUMBER_OF_CONTRIBUTING_STATIONS))
                        {
                            header.NumOfStations = Geo.Utils.StringUtil.ParseInt(strVal); continue;
                        }
                        if (line.StartsWith(IonoHarmonicHeaderLabel.NUMBER_OF_CONTRIBUTING_SATELLITES))
                        {
                            header.NumOfSatellites = Geo.Utils.StringUtil.ParseInt(strVal); continue;
                        }
                        if (line.StartsWith(IonoHarmonicHeaderLabel.ELEVATION_CUT_OFF_ANGLE))
                        {
                            header.ElevationCutOff = Geo.Utils.StringUtil.ParseDouble(strVal); continue;
                        }
                        if (line.StartsWith(IonoHarmonicHeaderLabel.MAXIMUM_TEC_AND_ITS_RMS))
                        {
                            header.MaxTec =  ParseRmedNumeral(strVal);
                            continue;
                        }  
                    }
                    if (line.StartsWith(IonoHarmonicHeaderLabel.COEFFICIENTS)) break;
                    if (line.StartsWith(IonoHarmonicHeaderLabel.DEGREE)) break;
                } 
            }
            return header;
        }


        public IonoHarmonicFile IonoFile { get; set; }
        /// <summary>
        /// 读取文件
        /// </summary> 
        /// <returns></returns>
        public IonoHarmonicFile ReadAll(bool isSkipContent = false)
        {
            IonoHarmonicFile file = new IonoHarmonicFile();
            file.Name = Path.GetFileName(FilePath);
            Time startTime = Time.MaxValue;
            Time endTime = Time.MinValue;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    var sect = ReadSection(line, reader);
                    file.Add(sect);

                    if (startTime > sect.Time) { startTime = sect.Time; }
                    if (endTime < sect.Time) { endTime = sect.Time; }
                }
            }
            file.TimePeriod = new BufferedTimePeriod(startTime, endTime, 60 * 60);//外推一小时

            return file;
        }
        Time startTime = Time.MaxValue; 

        /// <summary>
        /// 读取历元对象
        /// </summary> 
        /// <param name="line"></param>
        /// <param name="StreamReader"></param>
        /// <returns></returns>
        private IonoHarmonicSection ReadSection( string line, StreamReader StreamReader)
        { 
            IonoHarmonicSection ionoHarmonicSection = new IonoHarmonicSection(); 
            ionoHarmonicSection.Header = ReadHeader(StreamReader);
            while(!IsStartOfRecord(line))
            {
                line = StreamReader.ReadLine();
            }
            //读取数据内容
            while ((line = StreamReader.ReadLine()) != null)
            {
                if (String.IsNullOrWhiteSpace(line)) { continue; }
                if (line.Contains(IonoHarmonicHeaderLabel.StartLine))
                {
                    break;
                }

                var items = Geo.Utils.StringUtil.SplitByBlank(line);
                int degree = Geo.Utils.StringUtil.ParseInt(items[0]);
                int order = Geo.Utils.StringUtil.ParseInt(items[1]);
                double val = Geo.Utils.StringUtil.ParseDouble(items[2]);
                double rms = Geo.Utils.StringUtil.ParseDouble(items[3]);

                ionoHarmonicSection.GetOrCreate(degree).Set(order, new RmsedNumeral(val, rms));        
            }
            return ionoHarmonicSection; 
        }
        /// <summary>
        /// 是否是记录的开始，下一条即数字
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static bool IsStartOfRecord(string line)
        {
            if ( String.IsNullOrWhiteSpace( line)) return false;
            return line.Contains("DEGREE");
        }
  

        #region 静态工具方法 

        private static RmsedNumeral ParseRmedNumeral(string strVal)
        {
            var items = strVal.Split(' ');
            var val = Geo.Utils.StringUtil.ParseDouble(items[0]);
            double rms = 0;
            if (items.Length > 1)
            {
                rms = Geo.Utils.StringUtil.ParseDouble(items[1]);
            }
            var rmsedVal = new RmsedNumeral(val, rms);
            return rmsedVal;
        }

        /// <summary>
        /// 如果没有，返回空字符串
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string GetValueString(string line)
        {
            string strVal = "";
            var keyValue = line.Split(':');
            if (keyValue.Length > 1) { strVal = keyValue[1].Trim(); }

            return strVal;
        }

        /// <summary>
        /// 读取下一行有内容的行，非空行，空白行。
        /// 如果返回“”，表示已经结束。
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string ReadNextNoNullLine(StreamReader reader)
        {
            string line = reader.ReadLine();
            if (!String.IsNullOrWhiteSpace(line))
            {
                return line;
            }

            if (line == null) { return "";  }//null表示已经结束。
            return ReadNextNoNullLine(reader);
        } 
  
        #endregion
    }     
}
