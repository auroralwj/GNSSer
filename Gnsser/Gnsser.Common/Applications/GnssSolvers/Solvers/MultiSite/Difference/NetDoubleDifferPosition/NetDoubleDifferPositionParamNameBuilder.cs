//2018.11.02, czs, create in HMX, 双差网解定位

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using  Geo.Algorithm.Adjust;
using Geo;

namespace Gnsser.Service
{

    /// <summary>
    /// 双差网解定位参数命名器
    /// </summary>
    public class NetDoubleDifferPositionParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NetDoubleDifferPositionParamNameBuilder(GnssProcessOption option)  :base(option)
        {
            this.IsEstimateTropWetZpd = option.IsEstimateTropWetZpd;
        }
        /// <summary>
        /// 是否估计对流层湿延迟参数。
        /// </summary>
        public bool IsEstimateTropWetZpd { get; set; }
        /// <summary>
        /// 原料对象
        /// </summary>
        MultiSiteEpochInfo Obj { get => (MultiSiteEpochInfo)this.Material; }
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            if (String.IsNullOrWhiteSpace(BaseSiteName))
            {
                BaseSiteName = Obj.First.SiteName;
            }

            List<string> paramNames = new List<string>(); 

            foreach (var site in this.Obj)
            {
                if (site.SiteName == BaseSiteName) { continue; }

                var names = this.GetSiteDxyz(site.SiteName);
                paramNames.AddRange(names);
            }
            string name = "";
            if (this.IsEstimateTropWetZpd)
            {
                //对流层
                name = GetSiteWetTropZpdName(BaseSiteName);
                paramNames.Add(name);

                foreach (var site in this.Obj)
                {
                    if (site.SiteName == BaseSiteName) { continue; }
                    name = GetSiteWetTropZpdName(site.SiteName);
                    paramNames.Add(name);
                }
            }
            //模糊度
            foreach (var site in this.Obj)
            {
                var siteName = site.SiteName;
                if (siteName == BaseSiteName) { continue; }

                foreach (var prn in this.EnabledPrns)
                {
                    if (BasePrn == prn) { continue; }
                    paramNames.Add(GetDoubleDifferAmbiParamName(siteName , prn));
                } 
            }
            return paramNames;
        }
        /// <summary>
        /// 生成卫星编号相关的参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public override string GetParamName(SatelliteNumber prn)
        {
            return prn.ToString() + "-" + BasePrn + Gnsser.ParamNames.PhaseLengthSuffix;
        }
         
    }



    /// <summary>
    /// 双差名称
    /// </summary>
    public class NetDoubleDifferName: NetDifferName
    {
        public NetDoubleDifferName()
        {

        }
        public NetDoubleDifferName(string RovName, string RefName, SatelliteNumber RovPrn, SatelliteNumber RefPrn  )
            :base(RovName, RefName)
        { 
            this.RefPrn = RefPrn;
            this.RovPrn = RovPrn;
        } 
        /// <summary>
        /// 基准星
        /// </summary>
        public SatelliteNumber RefPrn { get; set; }
        /// <summary>
        /// 流动星
        /// </summary>
        public SatelliteNumber RovPrn { get; set; }
        /// <summary>
        /// 是否有效，都有值。
        /// </summary>
        public override bool IsValid => RovName != null && RefName != null && RefPrn != SatelliteNumber.Default && RovPrn != SatelliteNumber.Default;

        /// <summary>
        /// 字符串显示, Rov-Ref_G02-G01
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(RovName))
            {
                sb.Append(RovName);
                sb.Append(Gnsser.ParamNames.Pointer);
            }
            if (!String.IsNullOrWhiteSpace(RefName))
            {
                sb.Append(RefName);
                sb.Append(Gnsser.ParamNames.Divider);
            }
            sb.Append(RovPrn + Gnsser.ParamNames.Pointer + RefPrn);
            return sb.ToString();

            return RovName + Gnsser.ParamNames.Pointer + RefName + Gnsser.ParamNames.Divider + RovPrn + Gnsser.ParamNames.Pointer + RefPrn;
        }
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public string ToString(string Suffix)
        {
            return ToString() + Suffix;
        }
        /// <summary>
        /// 简单判断是否差分参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static bool IsDifferParam(string paramName)
        {
            return paramName.Contains("_") && paramName.Contains("-");

        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static NetDoubleDifferName Parse(string str)
        {
            var items = str.Split(new string[] { Gnsser.ParamNames.Divider }, StringSplitOptions.RemoveEmptyEntries);
            //Gnsser.ParamNames.DoubleDifferAmbiguity
            var firstStr = items[0];
            var firsts = firstStr.Split(new string[] { Gnsser.ParamNames.Pointer }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length >= 3)
            {
                var prnStr = items[1]; 
                var prns = prnStr.Split(new string[] { Gnsser.ParamNames.Pointer }, StringSplitOptions.RemoveEmptyEntries); 

               var nameObj = new NetDoubleDifferName()
               {
                   RovName = firsts[0],
                   RefName = firsts[1],
                   RovPrn = SatelliteNumber.Parse(prns[0]),
                   RefPrn = SatelliteNumber.Parse(prns[1])
               };
                return nameObj;
            }
            else
            {
                var RovPrn = SatelliteNumber.Parse(firsts[0]);
                var RefPrn = SatelliteNumber.Parse(firsts[1]);

                var nameObj = new NetDoubleDifferName()
                { 
                    RovPrn = RovPrn,
                    RefPrn = RefPrn
                };
                return nameObj;

            }
        }
    }


    /// <summary>
    /// 差分名称
    /// </summary>
    public class NetDifferName
    {
        public NetDifferName()
        {

        }
        public NetDifferName(string RovName, string RefName)
        {
            this.RovName = RovName;
            this.RefName = RefName; 
        }
        /// <summary>
        /// 参考站名称
        /// </summary>
        public string RefName { get; set; }
        /// <summary>
        /// 流动站名称
        /// </summary>
        public string RovName { get; set; } 
        /// <summary>
        /// 是否有效，都有值。
        /// </summary>
        public virtual bool IsValid => RovName != null && RefName != null;

        /// <summary>
        /// 字符串显示, Rov-Ref_G02-G01
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(RovName))
            {
                sb.Append(RovName);
                sb.Append(Gnsser.ParamNames.Pointer);
            }
            if (!String.IsNullOrWhiteSpace(RefName))
            {
                sb.Append(RefName);
                sb.Append(Gnsser.ParamNames.Divider);
            } 
            return sb.ToString();

         }
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public string ToString(string Suffix)
        {
            return ToString() + Suffix;
        }
        /// <summary>
        /// 简单判断是否差分参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static bool IsDifferParam(string paramName)
        {
            return paramName.Contains("-");

        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static NetDifferName Parse(string str)
        {
            var items = str.Split(new string[] { Gnsser.ParamNames.Divider }, StringSplitOptions.RemoveEmptyEntries);
            //Gnsser.ParamNames.DoubleDifferAmbiguity
            var firstStr = items[0];
            var firsts = firstStr.Split(new string[] { Gnsser.ParamNames.Pointer }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length >= 3)
            {
                var nameObj = new NetDifferName()
                {
                    RovName = firsts[0],
                    RefName = firsts[1]
                };
                return nameObj;
            }
            else
            {
                var RovPrn = (firsts[0]);
                var RefPrn = (firsts[1]);

                var nameObj = new NetDifferName(RovPrn, RefPrn);
                return nameObj;

            }
        }
    }

}