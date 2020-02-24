//2017.06.14, czs, create in hongqing, FCB 数据服务
//2018.09.17, czs, edit in HMX, 发现武大产品为差分产品，每次基准星不一致，需要进行差分。


using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Times;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Times; 
using Geo.Common;

namespace Gnsser.Data
{ 
    /// <summary>
    /// 卫星DCB服务。
    /// </summary>
    public class FcbDataService : FileBasedService<FcbValue>, Geo.IService<FcbValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dcbFileDir">地址目录</param>
        public FcbDataService(string dcbFileDir) : base(dcbFileDir)
        {
            Files = new Dictionary<string, FcbFile>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dcbFileDir">地址目录</param>
        public FcbDataService(FileOption dcbFileDir) : base(dcbFileDir)
        {
            Files = new Dictionary<string, FcbFile>();
        }

        Dictionary<string, FcbFile> Files { get; set; }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="time"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public BaseDictionary<SatelliteNumber, RmsedNumeral> GetWLFcbOfBsd(Time time, SatelliteNumber basePrn)
        {
            WideLaneValue wl = GetWLFcb(time);
            if (wl == null) { return new BaseDictionary<SatelliteNumber, RmsedNumeral>(); }
            var dic = wl.GetBsdDic(basePrn); 
            return dic;
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public WideLaneValue GetWLFcb(Time time)
        {
            string fileName = CheckFileLoaded(time);
            WideLaneValue result = null;
            if (Files.ContainsKey(fileName))
            {
                var file = Files[fileName];
                if (file == null) { return null; }

                result = file.Header.WideLaneValue;
            }

            return result;
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="time"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, RmsedNumeral> GetNLFcbOfBsd(Time time, SatelliteNumber basePrn)
        {
            string fileName = CheckFileLoaded(time);
            Dictionary<SatelliteNumber, RmsedNumeral> result = null;
            if (Files.ContainsKey(fileName))
            {
                var file = Files[fileName];
                if (file == null) { return null; }

                result = file.GetFcbOfBsdDic(time, basePrn);
            }

            return result; 
        }
        /// <summary>
        /// 获取FCB
        /// </summary>
        /// <param name="time"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn">基准卫星</param>
        /// <returns></returns>
        public RmsedNumeral GetNLFcbOfBsdValue(Time time, SatelliteNumber prn, SatelliteNumber basePrn)
        {
            string fileName = CheckFileLoaded(time);
            RmsedNumeral result = null;
            if (Files.ContainsKey(fileName))
            {
                var file = Files[fileName];
                if (file == null) { return null; }

                result = file.GetFcbOfBsdValue(time, prn, basePrn);
            }

            return result;
        }

        private string CheckFileLoaded(Time time)
        {
            var fileName = BuildName(time);

            if (!Files.ContainsKey(fileName))
            {
                var path = Path.Combine(this.Option.FilePath, fileName);
                FcbFile file = null;
                if (!File.Exists(path))
                {
                    var dir = BuildDirectory(time);
                    path = Path.Combine(this.Option.FilePath, dir, fileName);
                }
                if (File.Exists(path))
                {
                    FcbFileReader reader = new FcbFileReader(path);
                    file = reader.Read();
                }
                Files.Add(fileName, file);
            }

            return fileName;
        }

        public string BuildDirectory(Time time)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(time.GpsWeek.ToString("0000")); 
            return sb.ToString();
        }

        /// <summary>
        /// sgg19260_igs.fcb
        /// </summary>
        /// <param name="time"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public string BuildName(Time time, string sourceName = "igs_30")
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append("sgg");
            sb.Append(time.GpsWeek);
            sb.Append((int)time.DayOfWeek);
            sb.Append("_");
            sb.Append(sourceName);
            sb.Append(".fcb");
            return sb.ToString();
        }

    }
  
}
