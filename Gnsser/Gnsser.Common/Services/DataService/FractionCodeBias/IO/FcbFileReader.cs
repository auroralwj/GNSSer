//2017.06.14, czs, create in hongqing, FCB 数据服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Times;
using Geo.Utils;
using Gnsser;
using Geo.IO;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// FCB信息读取器
    /// </summary>
    public class FcbFileReader
    {
        static Log log = new Log(typeof(FcbFileReader));
        string FilePath; 
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public FcbFileReader(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// 读取头文件。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public FcbFileHeader ReadHeader(String path)
        {
            FcbFileHeader header = new FcbFileHeader();
            Time time = Time.Default;
            using (var streamReader = new StreamReader(path))
            {
                var line = "";
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Contains("END OF HEADER")) { break; }

                    var label = Geo.Utils.StringUtil.SubString(line, 60).Trim();
                    var content = Geo.Utils.StringUtil.SubString(line, 0, 60);
                    switch (label)
                    {
                        case FcbFileHeaderLabel.VERSION_TYPE:
                            header.Verion = Geo.Utils.StringUtil.ParseDouble(content, 0, 11);
                            header.DataType = Geo.Utils.StringUtil.SubString(content, 22, 10);
                            header.SatType = SatelliteTypeHelper.PareSatType(Geo.Utils.StringUtil.SubString(content, 49, 4));
                            break;
                        case FcbFileHeaderLabel.ANALYSIS_CENTER:
                            header.AnalysisCenter = content.Trim();
                            break;
                        case FcbFileHeaderLabel.COMMENT:


                            if (line.StartsWith("*"))
                            {
                                time = Time.Parse(Geo.Utils.StringUtil.SubString(line, 1, 27));
                                header.WideLaneValue = new WideLaneValue(time);
                            }
                            if (line.StartsWith("WL"))
                            {
                                SatelliteNumber prn = SatelliteNumber.Parse(line.Substring(4, 3));
                                var val = Double.Parse(line.Substring(14, 7));
                                var rms = Geo.Utils.StringUtil.ParseDouble(line, 25, 7);
                                var item = new FcbValue(prn, time, val, rms); 
                                header.WideLaneValue.Add(prn, new Geo.RmsedNumeral(val, rms));
                            } 

                            header.Comments.Add(content.Trim());
                            break;
                        case FcbFileHeaderLabel.Num_OF_SOLN_STA:
                            //egnore
                            break;
                        case FcbFileHeaderLabel.RUN_BY_DATE:
                            header.RunBy = content.Substring(0, 20);
                            break;
                        case FcbFileHeaderLabel.STA_NAME_LIST:
                            var sites = Geo.Utils.StringUtil.Split(content, ' ');
                            header.StationNames.AddRange(sites);
                            break;
                        case FcbFileHeaderLabel.SYS_EXT_PROD_APPLIED:
                            header.System = content.Substring(0, 1);
                            header.ExtProdApplied = Geo.Utils.StringUtil.SubString(content, 2);
                            break;
                        case FcbFileHeaderLabel.END_OF_HEADER:
                            return header;
                            break;
                        default:
                            log.Error("出现了不支持的标签 " + label);
                            break;
                    }
                }
                return header;
            }
        }

        /// <summary>
        /// 读取FCB信息。
        /// 由于信息文件较小，这里一次性读取完毕。
        /// </summary>
        /// <returns></returns>
        public FcbFile Read()
        {
            if (!File.Exists(FilePath))
            {
                log.Error("FCB文件不存在！" + FilePath);
                return null;
            }
         
            log.Info("载入：" + FilePath);
            FcbFileHeader header = ReadHeader(FilePath);
            FcbFile file = new FcbFile(header);
            using (var streamReader = new StreamReader(FilePath))
            {
                 var line = "";
                 SkipHeader(streamReader, line);
                 Time time = Time.Default;
                Geo.BaseDictionary<SatelliteNumber, FcbValue> currentEpochData = null;
                 while ((line = streamReader.ReadLine()) != null)
                 {                 
                     if (line.StartsWith("*")){
                         time = Time.Parse( Geo.Utils.StringUtil.SubString(line, 1));
                         currentEpochData = file.GetOrCreate(time);
                    }

                    if (line.StartsWith("P"))
                     {
                         SatelliteNumber prn = SatelliteNumber.Parse(line.Substring(1, 3));
                         var val = Double.Parse(line.Substring(23, 10));
                         var rms = Geo.Utils.StringUtil.ParseDouble(line,53);
                         var item = new FcbValue(prn,time, val, rms);
                        currentEpochData[item.Prn] = item;
                     } 
                 }
            }
            return file; 
        }
        /// <summary>
        /// 跳过头部
        /// </summary>
        /// <param name="streamReader"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private static StreamReader SkipHeader(StreamReader streamReader, string line)
        {
            while ((line = streamReader.ReadLine()) != null)
            {
                if (line.Contains("END OF HEADER")) { break; }
            }
            return streamReader;
        } 
    }//End SatDataReader
}
