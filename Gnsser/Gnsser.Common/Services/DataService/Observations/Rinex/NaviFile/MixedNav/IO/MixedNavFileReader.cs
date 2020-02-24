
//2017.07.23,czs, create in hongqing,  混合类型的导航文件

using System;
using Gnsser.Times;
using System.Collections.Generic;
using Geo.Times; 
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// Rinex 混合导航文件读取器。
    /// </summary>
    public class MixedNavFileReader
    {
        /// <summary>
        /// 初始化一个读取器。
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public MixedNavFileReader(string fileName)
        {
            this.RinexFileName = fileName;
            if (! File.Exists( RinexFileName)) throw new ArgumentException("路径不可为空！", "RinexFileName");
        }

        /// <summary>
        /// 当前文件路径。
        /// </summary>
        public string RinexFileName { get; set; }
         

        #region 导航文件的的读取
        /// <summary>
        /// GNSS导航文件的读取
        /// </summary>
        /// <param name="navFilePath"></param>
        /// <returns></returns>
        public MixedNavFile ReadGnssNavFlie()
        {
            MixedNavFile f = new MixedNavFile();
            f.SetHeader(ParamNavFileReader.ReadHeader(RinexFileName));
            //测试版本
            if (f.Header.Version == 0)
            {
                using (StreamReader sr = new StreamReader(RinexFileName, UnicodeEncoding.UTF8))
                {
                    RinexUtil.SkipHeader(sr);
                }
            }

            using (StreamReader sr = new StreamReader(RinexFileName, UnicodeEncoding.UTF8))
            {
                RinexUtil.SkipHeader(sr);

                if (f.Header.Version < 3.0)
                {
                    while (sr.Peek() != -1)
                    {
                        EphemerisParam record = ParamNavFileReader.ReadRecordV2(sr, f.Header);
                        f.Add(record);
                    }
                }
                else if (f.Header.Version >= 3.0 && f.Header.Version < 4.0)
                {
                    while (sr.Peek() != -1)
                    {
                        string line = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) { continue; }
                        SatClockBias recordHeader = ParamNavFileReader.ParseFirstLineV3(line, f.Header);
                        //轨道参数
                        var satType = recordHeader.Prn.SatelliteType;
                        if (EphemerisUtil.IsEphemerisParam(satType))
                        {
                            var record = new EphemerisParam(recordHeader);

                            ParamNavFileReader.ReadRecordBodyV3(sr, record);

                            f.Add(record);
                        }
                        else
                        {
                            var record2 = new GlonassNavRecord(recordHeader); 
                            GlonassNaviFileReader.ParseFirstLineV3(line, record2);
                            GlonassNaviFileReader.ReadRecordBodyV3(sr, record2);
                            f.Add(record2);
                        }

                    }
                }
            }
            
            return f;
        }
       
        #endregion
    }
}
