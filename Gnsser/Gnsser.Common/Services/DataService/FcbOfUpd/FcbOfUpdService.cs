//2018.08.31, czs, create in HMX, GPS 窄巷服务器
//2018.09.12, czs, edit in hmx, 通用宽巷窄巷服务
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Geo.Times;
using Geo;


namespace Gnsser.Service
{
    /// <summary>
    /// 通用宽巷窄巷服务
    /// </summary>
    public class FcbOfUpdService : IService
    {
        /// <summary>
        ///相位未校准延迟的小数部分(Fraction Code Bias of Uncalibrated phase delay)
        /// </summary>
        /// <param name="filePath"></param>
        public FcbOfUpdService(string filePath)
        {
            this.FilePath = filePath;
            this.Name = Path.GetFileName(filePath);
             this.FcbFile = new FcbOfUpdReader(filePath).ReadToFile();
            //2018.01.01,北美
            //基准星 G22
            //G09	     G10	G16   	G03	    G14  	G01	    G07    	G32	    G31	    G23	    G26
            //0.42198	0.14305	0.04596	0.00156	0.87965	0.15598	0.28638	0.14857	0.39278	0.48305	0.55891

            //Name G32-G22 G31 - G22 G23 - G22 G26 - G22 G09 - G22 G10 - G22 G11 - G22 G16 - G22 G14 - G22 G03 - G22 G01 - G22 G07 - G22 G08 - G22
            //Average 0.15022 0.38986 0.00806 0.07711 - 0.01805    0.15474 - 0.18813    0.03407 0.87876 0.55041 0.13516 0.30273 - 0.0284
        }

        /// <summary>
        /// 文件
        /// </summary>
        public FcbOfUpdFile  FcbFile { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// BSD
        /// </summary>
        /// <param name="prns"></param>
        /// <param name="basePrn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, RmsedNumeral> GetBsdOfNarrowLane(List<SatelliteNumber> prns, SatelliteNumber basePrn, Time time)
        {
            Dictionary<SatelliteNumber, RmsedNumeral> result = new Dictionary<SatelliteNumber, RmsedNumeral>();
            foreach (var prn in prns)
            {
                result[prn] = GetBsdOfNarrowLane(prn, basePrn, time);
            } 
            return result;
        }

        /// <summary>
        /// BSD
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public RmsedNumeral GetBsdOfWideLane(SatelliteNumber prn, SatelliteNumber basePrn, Time time)
        {
            if (FcbFile == null) { return RmsedNumeral.NaN; }
            var fcb = this.GetWideLane(time);
            return fcb.GetBsdValue(prn, basePrn);
        }
        /// <summary>
        /// BSD
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public RmsedNumeral GetBsdOfNarrowLane(SatelliteNumber prn, SatelliteNumber basePrn, Time time)
        {
            if (FcbFile == null) { return RmsedNumeral.NaN; }
            var fcb = this.GetNarrowLane(time);
            return fcb.GetBsdValue(prn, basePrn);
        }
        /// <summary>
        /// 宽巷产品
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public FcbOfUpd GetWideLane(Time time)
        {
            if(FcbFile == null) { return null; }
            return   FcbFile.GetWideLane(time);
        }
        /// <summary>
        /// 窄巷产品
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public FcbOfUpd GetNarrowLane(Time time)
        {
            if (FcbFile == null) { return null; }
            return FcbFile.GetNarrowLane(time);

        }

    }
}
