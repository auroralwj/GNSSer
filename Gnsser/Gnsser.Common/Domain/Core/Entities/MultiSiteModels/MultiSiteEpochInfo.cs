//2016.04.24, czs, edit in hongqing, 多测站历元信息
//2018.11.01, czs, edit in hmx, 增加网解观测向量

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust; 
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Domain
{
    /// <summary>
    /// 多测站单历元信息
    /// </summary>
    public class MultiSiteEpochInfo : BaseDictionary<string, EpochInformation>, ISiteSatObsInfo
    {
        /// <summary>
        /// 多站，同历元数据。
        /// </summary>
        public MultiSiteEpochInfo(bool IsRequireSameSats)
        {
            this.Name = "多站同历元数据";
            this.IsRequireSameSats = IsRequireSameSats;
        }

        #region 属性
        /// <summary>
        /// 历元列表
        /// </summary>
        public List<Time> Epoches => new List<Time>() { ReceiverTime };
        /// <summary>
        /// 记录已经移除的卫星编号
        /// </summary>
        public List<SatelliteNumber> RemovedPrns
        {
            get
            {
                List<SatelliteNumber> list = new List<SatelliteNumber>();
                foreach (var epochInfo in this)
                {
                    if (list.Count == 0) { list = epochInfo.RemovedPrns; continue; }

                    list.AddRange(epochInfo.RemovedPrns);
                }
                list = list.Distinct().ToList();
                list.Sort();
                return list;
            }
        }
        /// <summary>
        /// 是否需要相同的卫星
        /// </summary>
        public bool IsRequireSameSats { get; set; }
        /// <summary>
        /// 一天中的编号。依据主数据源历元的时间间隔和时间计算得出。
        /// </summary>
        public int EpochIndexOfDay { get { return BaseEpochInfo.EpochIndexOfDay; } }
        /// <summary>
        /// 历元
        /// </summary>
        public Time ReceiverTime { get { return BaseEpochInfo.ReceiverTime; } }
        /// <summary>
        /// 基准测站的观测信息
        /// </summary>
        public EpochInformation BaseEpochInfo { get; set; }
        /// <summary>
        /// 基准测站
        /// </summary>
        public string BaseSiteName { get => BaseEpochInfo.SiteName; }
        /// <summary>
        /// 除了基站信息的另一个测站信息，如果只有两个，则是另一个。
        /// </summary>
        public EpochInformation OtherEpochInfo { get { return ListExceptBase[0]; } set { ListExceptBase[0] = value; } }

        /// <summary>
        /// 其它历元信息，除了基准信息
        /// </summary>
        public List<EpochInformation> ListExceptBase { get { return List.FindAll(m => !m.Equals(BaseEpochInfo)); } }

        /// <summary>
        /// 列表。
        /// </summary>
        public List<EpochInformation> List { get => Data.Values.ToList(); }// { var list = new List<EpochInformation>(Data.Values); return list; } }
        /// <summary>
        /// 共同的启用的卫星
        /// </summary>
        public List<SatelliteNumber> EnabledPrns
        {
            get
            {
                List<SatelliteNumber> list = new List<SatelliteNumber>();
                foreach (var epochInfo in this)
                {
                    if (list.Count == 0) { list = epochInfo.EnabledPrns; continue; }
                    if (IsRequireSameSats)
                    {
                        list = SatelliteNumberUtils.GetCommons(epochInfo.EnabledPrns, list);
                    }
                    else
                    {
                        var newPrns = SatelliteNumberUtils.GetNews(  epochInfo.EnabledPrns,list);
                        list.AddRange(newPrns);
                    }
                }
                list.Sort();
                return list;
            }
        }

        /// <summary>
        /// 启用卫星的数量
        /// </summary>
        public int EnabledSatCount { get { return EnabledPrns.Count; } }

        /// <summary>
        /// 不稳定的卫星，通常为具有周跳的卫星。
        /// </summary>
        public List<SatelliteNumber> UnstablePrns
        {
            get
            {
                List<SatelliteNumber> unstablePrns = new List<SatelliteNumber>();
                foreach (var item in this)
                {
                    foreach (var prn in item.UnstablePrns)
                    {
                        if (!unstablePrns.Contains(prn))
                        {
                            unstablePrns.Add(prn);
                        }
                    }
                }
                return unstablePrns;
            }
        }

        /// <summary>
        /// 所有出现过的卫星编号
        /// </summary>
        public List<SatelliteNumber> TotalPrns
        {
            get
            {
                List<SatelliteNumber> totalPrns = new List<SatelliteNumber>();
                foreach (var item in this)
                {
                    foreach (var prn in item.TotalPrns)
                    {
                        if (!totalPrns.Contains(prn))
                        {
                            totalPrns.Add(prn);
                        }
                    }
                }
                return totalPrns;
            }
        }

        /// <summary>
        /// 流动站名称列表
        /// </summary>
        public List<String> RovSiteNames
        {
            get
            {
                var baseName = this.BaseEpochInfo.SiteName;
                var rovNames = new List<String>();
                foreach (var item in ListExceptBase)
                {
                    rovNames.Add(item.SiteName);
                }
                return rovNames;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 基线向量坐标
        /// </summary>
        /// <param name="fromSite"></param>
        /// <param name="toSite"></param>
        /// <returns></returns>
        public XYZ GetBaseLineVector(string fromSite, string toSite)
        {
            var siteTo = this.Get(toSite);
            if (siteTo == null) { return XYZ.Zero; }

            var siteFrom = this.Get(fromSite);
            if (siteFrom == null) { return XYZ.Zero; }

            return siteFrom.SiteInfo.EstimatedXyz - siteTo.SiteInfo.EstimatedXyz;
        }

        /// <summary>
        /// 去除没有卫星
        /// </summary>
        public void RemoveSatWithoutEphemeris()
        {
            foreach (var item in this)
            {
                item.RemoveNoEphemeris();
            }
        }
        /// <summary>
        /// 移除曾经标记为不稳定的标签。
        /// </summary>
        public void RemoveUnStableMarkers()
        {
            foreach (var item in this)
            {
                item.RemoveUnStableMarkers();
            }
        }

        /// <summary>
        /// 指定卫星是否具有周跳，任一测站有，则有。
        /// </summary>
        /// <param name="prn">卫星编号</param>
        /// <param name="siteName">测站名称</param>
        /// <returns></returns>
        public bool HasCycleSlip(SatelliteNumber prn, string siteName)
        {
            if (this.Contains(siteName)) return this[siteName].HasCycleSlip(prn);
            return true;
        }

        /// <summary>
        /// 如果有一个观测值具有周跳，则认为这颗有周跳。
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public bool HasCycleSlip(SatelliteNumber prn)
        {
            foreach (var item in this)
            {
                if (item.HasCycleSlip(prn)) return true;
            }
            return false;
        }

        /// <summary>
        /// 禁用其它卫星
        /// </summary>
        /// <param name="prns"></param>
        public void DisableOthers(List<SatelliteNumber> prns) { foreach (var info in this) { info.DisableOthers(prns); } }
        /// <summary>
        /// 可用卫星系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes => (from prn in EnabledPrns select prn.SatelliteType).Distinct().ToList();
        /// <summary>
        /// 启用指定卫星
        /// </summary>
        /// <param name="prns"></param>
        internal void Enable(List<SatelliteNumber> prns) { foreach (var info in this) { info.Enable(prns); } }
        /// <summary>
        /// 禁用指定卫星
        /// </summary>
        /// <param name="prns"></param>
        public void Disable(List<SatelliteNumber> prns)
        {
            if (prns.Count == 0) return;
            foreach (var item in this) { item.Disable(prns); }
        }

        /// <summary>
        /// 禁用不同的卫星
        /// </summary>
        public void DisableDifferSats()
        {
            List<SatelliteNumber> differs = new List<SatelliteNumber>();
            var first = this.First;
            foreach (var info in this)
            {
                if (info == first) continue;

                differs.AddRange(SatelliteNumberUtils.GetDiffers(first.EnabledPrns, info.EnabledPrns));
            }
            Disable(differs);
        }
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ReceiverTime + " " + base.ToString() + " "
                + ",EnabledPrns: " + this.EnabledSatCount
                + String.Format(new EnumerableFormatProvider(), "({0:,})", this.EnabledPrns);
        }

        #region 平差向量方法
        #region  两个测站单基线
        /// <summary>
        /// 本历元两个站的单差向量,相位观测值，浮动站站心残差减去参考站
        /// </summary>
        /// <param name="freqType">频率</param>
        /// <param name="prns">参与计算的卫星</param>
        /// <param name="basePrn">基础卫星，所有卫星都与它做差,将其编号排列在第一位</param>
        public Vector GetTwoSiteSinglePhaseDifferResidualVector(FrequenceType freqType, List<SatelliteNumber> prns, SatelliteNumber basePrn)
        {
            var refEpoch = this.BaseEpochInfo;
            var rovEpoch = this.OtherEpochInfo;

            Vector vector = new Vector(prns.Count);
            int satIndex = 0;
            double rover = rovEpoch[basePrn].GetRawPhaseRangeResidual(freqType);
            double refer = refEpoch[basePrn].GetRawPhaseRangeResidual(freqType);
            double baseDiffer = rover - refer;
            vector[satIndex++] = baseDiffer; //基础卫星放在第一个

            foreach (var prn in prns)
            {
                if (prn.Equals(basePrn)) { continue; }

                rover = rovEpoch[prn].GetRawPhaseRangeResidual(freqType);
                refer = refEpoch[prn].GetRawPhaseRangeResidual(freqType);
                var differ = rover - refer;
                vector[satIndex++] = differ;
            }
            return vector;
        }

        /// <summary>
        /// 本历元两个站的伪距双差残差向量
        /// </summary>
        /// <param name="freqType"></param>
        /// <param name="prns"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public Vector GetTwoSiteDoubleRangeDifferResidualVector(FrequenceType freqType, List<SatelliteNumber> prns, SatelliteNumber basePrn)
        {
            Vector singleDifferVector = GetTwoSiteSingleRangeDifferResidualVector(freqType, prns, basePrn);
            double firstSatResidual = singleDifferVector[0];//基准星放在第一位
            Vector vector = new Vector(prns.Count - 1);
            for (int i = 0; i < vector.Count; i++)  //后续减去第一个
            {
                vector[i] = singleDifferVector[i + 1] - firstSatResidual;
            }
            return vector;
        }

        /// <summary>
        /// 本历元两个站的伪距单差残差向量
        /// </summary>
        /// <param name="freqType"></param>
        /// <param name="prns"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public Vector GetTwoSiteSingleRangeDifferResidualVector(FrequenceType freqType, List<SatelliteNumber> prns, SatelliteNumber basePrn)
        {
            var refEpoch = this.BaseEpochInfo;
            var rovEpoch = this.OtherEpochInfo;

            Vector vector = new Vector(prns.Count);
            int satIndex = 0;
            double rover = rovEpoch[basePrn].GetRawRangeResidual(freqType);
            double refer = refEpoch[basePrn].GetRawRangeResidual(freqType);
            double baseDiffer = rover - refer;
            vector[satIndex++] = baseDiffer; //基础卫星放在第一个

            foreach (var prn in prns)
            {
                if (prn.Equals(basePrn)) { continue; }

                rover = rovEpoch[prn].GetRawRangeResidual(freqType);
                refer = refEpoch[prn].GetRawRangeResidual(freqType);
                var differ = rover - refer;
                vector[satIndex++] = differ;
            }
            return vector;
        }

        /// <summary>
        /// 本历元两个站的载波双差残差向量.
        /// </summary>
        /// <param name="freqType">频率</param> 
        /// <param name="prns">参与计算的卫星</param>
        /// <param name="basePrn">基础卫星，所有卫星都与它做差，将其编号排列在第一位</param>
        /// <returns></returns>
        public Vector GetTwoSiteDoublePhaseDifferResidualVector(FrequenceType freqType, List<SatelliteNumber> prns, SatelliteNumber basePrn)
        {
            Vector singleDifferVector = GetTwoSiteSinglePhaseDifferResidualVector(freqType, prns, basePrn);
            double firstSatResidual = singleDifferVector[0];//基准星放在第一位
            Vector vector = new Vector(prns.Count - 1);
            for (int i = 0; i < vector.Count; i++)  //后续减去第一个
            {
                vector[i] = singleDifferVector[i + 1] - firstSatResidual;
            }
            return vector;
        }
        /// <summary>
        /// 本历元两个站的双差残差向量.
        /// </summary>
        /// <param name="obsType">观测值</param>
        /// <param name="approxType">近似值</param>
        /// <param name="prns">参与计算的卫星</param>
        /// <param name="basePrn">基础卫星，所有卫星都与它做差，将其编号排列在第一位</param>
        /// <returns></returns>
        public Vector GetTwoSiteDoubleDifferResidualVector(SatObsDataType obsType, SatApproxDataType approxType, List<SatelliteNumber> prns, SatelliteNumber basePrn)
        {
            Vector singleDifferVector = GetTwoSiteSingleDifferResidualVector(obsType, approxType, prns, basePrn);
            double firstSatResidual = singleDifferVector[0];//基准星放在第一位
            Vector vector = new Vector(prns.Count - 1);
            for (int i = 0; i < vector.Count; i++)  //后续减去第一个
            {
                vector[i] = singleDifferVector[i + 1] - firstSatResidual;
            }
            return vector;
        }

        /// <summary>
        /// 本历元两个站的单差向量,浮动站站心残差减去参考站
        /// </summary>
        /// <param name="obsType">观测值</param>
        /// <param name="approxType">近似值</param>
        /// <param name="prns">参与计算的卫星</param>
        /// <param name="basePrn">基础卫星，所有卫星都与它做差,将其编号排列在第一位</param>
        public Vector GetTwoSiteSingleDifferResidualVector(SatObsDataType obsType, SatApproxDataType approxType, List<SatelliteNumber> prns, SatelliteNumber basePrn)
        {
            var refEpoch = this.BaseEpochInfo;
            var rovEpoch = this.OtherEpochInfo;

            Vector vector = new Vector(prns.Count);
            int satIndex = 0;
            double rover = rovEpoch[basePrn].GetResidual(obsType, approxType);
            double refer = refEpoch[basePrn].GetResidual(obsType, approxType);
            double baseDiffer = rover - refer;
            vector[satIndex++] = baseDiffer; //基础卫星放在第一个

            foreach (var prn in prns)
            {
                if (prn.Equals(basePrn)) { continue; }

                rover = rovEpoch[prn].GetResidual(obsType, approxType);
                refer = refEpoch[prn].GetResidual(obsType, approxType);
                var differ = rover - refer;
                vector[satIndex++] = differ;
            }
            return vector;
        }
        #endregion

        #region 差分网解

        #region  站间差分网解【算法校验通过】
        /// <summary>
        /// 历元的多站网解双差残差向量.
        /// 站间双差。【算法校验通过】
        /// 对于双差，站间和星间相同。
        /// </summary>
        /// <param name="obsType">观测值</param>
        /// <param name="approxType">近似值</param>
        /// <param name="baseSiteName">基准测站</param>
        /// <param name="basePrn">基础卫星，所有卫星都与它做差，将其编号排列在第一位</param>
        /// <returns></returns>
        public Vector GetNetDoubleDifferResidualVectorBeweenSites(SatObsDataType obsType, SatApproxDataType approxType, string baseSiteName, SatelliteNumber basePrn)
        {
            List<SatelliteNumber> prns = this.EnabledPrns;
            Vector singleDifferVector = GetNetDifferResidualVectorBetweenSites(obsType, approxType, baseSiteName, basePrn);
            int leftSiteCount = this.Count-1;  //余下的测站数量
            int leftSatCount = this.EnabledSatCount - 1;//预下的卫星数量
            int obsCount = leftSiteCount * leftSatCount;
            Vector vector = new Vector(obsCount);//相比单差，每个测站，少一个基准星观测量，被差分了。

            int newIndex = 0;
            int rovSiteIndex = 0;//
            foreach (var site in this)
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, baseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                //每个测站第一个为基准卫星
                var refSatIndexOfCurrentSite = rovSiteIndex * this.EnabledSatCount;
                double refVal = singleDifferVector[refSatIndexOfCurrentSite];
                int rovSatIndex = 0;
                foreach (var prn in prns)//对同一颗卫星作差，才能消除卫星钟差
                {
                    if (prn.Equals(basePrn)) { continue; }

                    int rovIndex = refSatIndexOfCurrentSite + 1 + rovSatIndex;
                    var differ = singleDifferVector[rovIndex] - refVal;
                    vector[newIndex] = differ;
                    vector.ParamNames[newIndex] = "(" + singleDifferVector.ParamNames[rovIndex] + ")-(" + singleDifferVector.ParamNames[rovSiteIndex] + ")";
                    //----------------------check-----------------
                    if ( false) //【算法校验通过】
                    {
                        var checkVal = this.GetDoubleDifferResidual(siteName, baseSiteName, prn, basePrn, obsType, approxType, false);
                        var dif = checkVal - differ;
                        if (dif != 0)
                        {
                            int ii = 0;
                            log.Error("与检核数据不相等！" + dif);
                        }
                    }
                    //-----------------end check -------------------
                    newIndex++;
                    rovSatIndex++;
                }
                rovSiteIndex++;
            }

            return vector;
        }

        /// <summary>
        /// 历元的站间单差向量,每个浮动站站星残差依次减去参考站，单差后消除了卫星钟差，适用于单差星形网解基线。
        /// 卫星数量不变，测站数量少一。
        /// 站间单差（同一卫星对不同测站作差）。基准站信息被差分了，体现的都是流动站信息。 
        /// 参数顺序：按照测站顺序变量，基准星排在各个测站的第一个。【算法核对无误】
        /// </summary>
        /// <param name="obsType">观测值</param>
        /// <param name="approxType">近似值</param>
        /// <param name="baseSiteName">基准测站</param>
        /// <param name="basePrn">基础卫星，所有卫星都与它做差,将其编号排列在第一位</param>
        public Vector GetNetDifferResidualVectorBetweenSites(SatObsDataType obsType, SatApproxDataType approxType, string baseSiteName,  SatelliteNumber basePrn)
        {
            List<SatelliteNumber> prns = this.EnabledPrns;
            var refSite = this.BaseEpochInfo;
            if (!String.IsNullOrWhiteSpace(baseSiteName))
            {
                refSite = this[baseSiteName];
            }
            int obsCount = (prns.Count  * (this.Count- 1));//少一个基准测站的观测值，被差分掉了。
            Vector vector = new Vector(obsCount);
            int satIndex = 0;
            double refer = refSite[basePrn].GetResidual(obsType, approxType);
            foreach (var site in this)
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, baseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }
                
                //测站对基准卫星作差，并放在第一个（主要方便双差与其作差）
                double rover = site[basePrn].GetResidual(obsType, approxType);
                double baseDiffer = rover - refer;
                vector[satIndex] = baseDiffer;

                vector.ParamNames[satIndex] = site.SiteName + "-" + baseSiteName + "_" + basePrn;
                satIndex++;

                //其它星的单差
                foreach (var prn in prns)//对同一颗卫星作差，才能消除卫星钟差
                {
                    if (prn.Equals(basePrn)) { continue; }

                    rover = site[prn].GetResidual(obsType, approxType);
                    refer = refSite[prn].GetResidual(obsType, approxType);
                    var differ = rover - refer;

                    //---------------------check----------------------
                    if (false) //以下校验核对无误，2018.11.02，czs, hmx
                    {
                        var checkVal = this.GetDifferResidualBetweenSites(siteName,baseSiteName, prn, obsType, approxType);
                        var dif = checkVal - differ;
                        if(dif != 0)
                        {
                            int ii = 0;
                            log.Error("与检核数据不相等！" + dif);
                        }
                    }
                    //-----------------end check -------------------

                    vector[satIndex] = differ;
                    vector.ParamNames[satIndex] = site.SiteName + "-" + baseSiteName + "_" + prn;
                    satIndex++;
                }
            }
            return vector;
        }
        #endregion

        #region 星间差分网解
        /// <summary>
        /// 历元的多站双差残差向量.
        /// 星间双差。【算法校验通过】
        /// 对于双差，站间和星间相同。
        /// </summary>
        /// <param name="obsType">观测值</param>
        /// <param name="approxType">近似值</param>
        /// <param name="baseSiteName">基准测站</param>
        /// <param name="basePrn">基础卫星，所有卫星都与它做差，将其编号排列在第一位</param>
        /// <returns></returns>
        public Vector GetDoubleDifferResidualVectorBeweenSats(SatObsDataType obsType, SatApproxDataType approxType, string baseSiteName, SatelliteNumber basePrn)
        {
            var refSite = this.BaseEpochInfo;
            if (!String.IsNullOrWhiteSpace(baseSiteName))
            {
                refSite = this[baseSiteName];
            }

            List<SatelliteNumber> prns = this.EnabledPrns;
            Vector singleDifferVector = GetNetDifferResidualVectorBeweenSats(obsType, approxType, baseSiteName, basePrn);
            var leftSiteCount = (this.Count - 1);
            var leftSatCount = (this.EnabledSatCount - 1);
            var obsCount = leftSiteCount * leftSatCount;//相比星间单差，每个卫星测站，少一个基准测站观测量，被差分了。
            Vector vector = new Vector(obsCount);

            int newIndex = 0; 
            foreach (var site in this)
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, baseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                //测站对基准卫星作差，并放在第一个（主要方便双差与其作差）
                double rover = site[basePrn].GetResidual(obsType, approxType);

                int baseIndex = 0;
                foreach (var prn in prns)//对同一颗卫星作差，才能消除卫星钟差
                {
                    if (prn.Equals(basePrn)) { continue; }
                    int rovIndex = newIndex + leftSatCount;
                    var differ = singleDifferVector[rovIndex] - singleDifferVector[baseIndex];
                    vector[newIndex] = differ;
                    vector.ParamNames[newIndex] = "(" + singleDifferVector.ParamNames[rovIndex] + ")-(" + singleDifferVector.ParamNames[baseIndex] + ")";
                     
                    //-------------------- check --------------------
                    if (false)
                    {
                        var checkVal = this.GetDoubleDifferResidual(siteName, baseSiteName, prn, basePrn, obsType, approxType, false);
                        var dif = checkVal - differ;
                        if (dif != 0)
                        {
                            int ii = 0;
                            log.Error("与检核数据不相等！" + dif);
                        }
                    }                    
                    //-----------------end check -------------------

                    baseIndex++; 
                    newIndex++;
                }
            }


            //-------------------- check --------------------
            //第二种遍历法，已经验证，完全相同。208.11.01， czs， hmx
            if (false)
            {
                newIndex = 0;
                for (int i = 0; i < leftSiteCount; i++)
                {
                    for (int j = 0; j < leftSatCount; j++)
                    {
                        int baseIndex = j;
                        int rovIndex = newIndex + leftSatCount;
                        var differ = singleDifferVector[rovIndex] - singleDifferVector[baseIndex];
                        var ddddd = vector[newIndex] - differ;
                        if (ddddd != 0)
                        {
                            log.Error("与检核数据不相等！" + ddddd);
                            throw new Exception("不相等啊！");
                        }
                        vector[newIndex] = differ;

                        vector.ParamNames[newIndex] = "(" + singleDifferVector.ParamNames[rovIndex] + ")-(" + singleDifferVector.ParamNames[baseIndex] + ")";
                        newIndex++;
                    }
                }
                //-----------------end check -------------------
            }

            return vector;
        }

        /// <summary> 
        /// 星间单差(以卫星为基准，对测站进行做差，需要指定一个基准测站，可以差分掉测站钟差，适用于定轨计算)。
        /// 基准测站在第一梯队，双差时，将其差分掉。
        /// 以一颗基准星的EpochSat为参考，纷纷与其作差。
        /// 参数顺序：按照测站顺序变量，基准星排在各个测站的第一个。【算法校验通过】
        /// </summary>
        /// <param name="obsType">观测值</param>
        /// <param name="approxType">近似值</param>
        /// <param name="baseSiteName">基准测站</param>
        /// <param name="basePrn">基础卫星，所有卫星都与它做差,将其编号排列在第一位</param>
        public Vector GetNetDifferResidualVectorBeweenSats(SatObsDataType obsType, SatApproxDataType approxType, string baseSiteName, SatelliteNumber basePrn)
        {
            var refSite = this.BaseEpochInfo;
            if (!String.IsNullOrWhiteSpace(baseSiteName))
            {
                refSite = this[baseSiteName];
            }
            List<SatelliteNumber> prns = this.EnabledPrns;

            int obsCount = (prns.Count - 1) * (this.Count);//少一个卫星的观测值，被差分掉了。
            Vector vector = new Vector(obsCount);
            int satIndex = 0;
            //为了方便双差使用，基准测站的放在第一位，双差时，再将其差分掉   
            double refer = refSite[basePrn].GetResidual(obsType, approxType); 
            foreach (var prn in prns)//对同一颗卫星作差，才能消除其钟差
            {
                if (prn.Equals(basePrn)) { continue; }

                var rover = refSite[prn].GetResidual(obsType, approxType); 
                var differ = rover - refer;
                vector[satIndex] = differ;
                vector.ParamNames[satIndex] = baseSiteName + "_" + prn + "-" + basePrn;
                satIndex++;
            }
            
            foreach (var site in this)
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, baseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                //首先各个测站对基准卫星作差，并放在第一个（主要方便双差与其作差）
                double baseVal = site[basePrn].GetResidual(obsType, approxType); 
                foreach (var prn in prns)//对同一颗卫星作差，才能消除其钟差
                {
                    if (prn.Equals(basePrn)) { continue; }

                    var rover = site[prn].GetResidual(obsType, approxType);

                    var differ = rover - baseVal;

                    //--------------------check---------------------
                    if(false)//2018.11.02, czs, edit in hmx, 算法校验通过
                    {
                        var singleVal = this.GetDifferResidualBetweenSats(siteName, prn, basePrn, obsType, approxType);
                        var dif = differ - singleVal;
                        if (dif != 0)
                        {
                            int ii = 0;
                            log.Error("与检核数据不相等！" + dif);
                        }
                    }
                    //-----------------end check -------------------

                    vector[satIndex] = differ;
                    vector.ParamNames[satIndex] = site.SiteName + "_" + prn + "-" + basePrn;
                    satIndex++;
                }
            } 
            return vector;
        }
        #endregion

        #region  单个差分
        /// <summary>
        /// 对两个测站两个卫星残差做双差。【算法校验通过】
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="baseSiteName"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="SatObsDataType"></param>
        /// <param name="SatApproxDataType"></param>
        /// <param name="isBeweenSiteOrSatFirst">站间差分，否则星间差分为先</param>
        /// <returns></returns>
        public double GetDoubleDifferResidual(
            string siteName, string baseSiteName,
            SatelliteNumber prn, SatelliteNumber basePrn,
            SatObsDataType SatObsDataType, SatApproxDataType SatApproxDataType, bool isBeweenSiteOrSatFirst = true)
        {
            var rovSite = this[siteName];
            var refSite = this[baseSiteName];
            var rovSiteRovPrn = rovSite[prn].GetResidual(SatObsDataType, SatApproxDataType);
            var rovSiteRefPrn = rovSite[basePrn].GetResidual(SatObsDataType, SatApproxDataType);
            var refSiteRovPrn = refSite[prn].GetResidual(SatObsDataType, SatApproxDataType);
            var refSiteRefPrn = refSite[basePrn].GetResidual(SatObsDataType, SatApproxDataType);
            //站间差分
            var rovSiteDiffer = rovSiteRovPrn - refSiteRovPrn; //流动星的流动站与参考星单差
            var reSiteDiffer = rovSiteRefPrn - refSiteRefPrn; //参考星的流动站与参考星单差
            //星间差分
            var rovSatDiffer = rovSiteRovPrn - rovSiteRefPrn;//流动站的流动星与参考星单差
            var refSatDiffer = refSiteRovPrn - refSiteRefPrn;//参考站的流动星与参考星单差

            //星间差分的双差
            var siteFirstDoubleDiffer = rovSiteDiffer - reSiteDiffer;

            //站间差分的双差
            var satFirstDoubleDiffer = rovSatDiffer - refSatDiffer;

            //------------------check--------------------------
            if (false)//【算法校验通过】
            {
                if(siteFirstDoubleDiffer != satFirstDoubleDiffer)
                {
                    log.Error("双差算法校验出错！");
                }
            }
            //-----------------end check -------------------

            if (isBeweenSiteOrSatFirst)
            {
                return siteFirstDoubleDiffer;
            }
            return satFirstDoubleDiffer;
        }


        /// <summary>
        /// 同一测站两个卫星残差做双差。星间单差。
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="SatObsDataType"></param>
        /// <param name="SatApproxDataType"></param>
        /// <returns></returns>
        public double GetDifferResidualBetweenSats(
            string siteName,
            SatelliteNumber prn, SatelliteNumber basePrn,
            SatObsDataType SatObsDataType, SatApproxDataType SatApproxDataType)
        {
            var rovSite = this[siteName];
            var rovSiteRovPrn = rovSite[prn].GetResidual(SatObsDataType, SatApproxDataType);
            var rovSiteRefPrn = rovSite[basePrn].GetResidual(SatObsDataType, SatApproxDataType);
            //星间差分
            var rovSatDiffer = rovSiteRovPrn - rovSiteRefPrn;//流动站的流动星
             
            return rovSatDiffer;
        }


        /// <summary>
        ///  同一卫星 对两个测站残差做差。站间单差。
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="baseSiteName"></param>
        /// <param name="prn"></param>
        /// <param name="SatObsDataType"></param>
        /// <param name="SatApproxDataType"></param>
        /// <returns></returns>
        public double GetDifferResidualBetweenSites(
            string siteName, string baseSiteName,
            SatelliteNumber prn, SatObsDataType SatObsDataType, SatApproxDataType SatApproxDataType)
        {
            var rovSite = this[siteName];
            var refSite = this[baseSiteName];
            var rovSiteRovPrn = rovSite[prn].GetResidual(SatObsDataType, SatApproxDataType);
            var refSiteRovPrn = refSite[prn].GetResidual(SatObsDataType, SatApproxDataType);
            //站间差分
            var rovSiteDiffer = rovSiteRovPrn - refSiteRovPrn; //同一星的流动站与参考星单差  
            return rovSiteDiffer; 
        }
        #endregion
        #endregion
        #endregion

        #endregion


        public string GetTabTitles()
        {
            throw new NotImplementedException();
        }

        public string GetTabValues()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 具有星历的卫星列表
        /// </summary>
        /// <returns></returns>
        public List<EpochSatellite> GetEpochSatWithEphemeris()
        {
            List<EpochSatellite> sats = new List<EpochSatellite>();
            foreach (var item in this)
            {
                sats.AddRange(item.GetEpochSatWithEphemeris()); 
            }
            return sats;
        } 
    }
}