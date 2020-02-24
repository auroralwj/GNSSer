//2015.01.07, czs, edigt in namu, 周跳组合探测处理器，采用探测器标记是否具有周跳。
//2016.01.28, czs, edit in hongqing, 周跳探测增加时间判断器，避免重复探测周跳
//2016.10.12 double, edit in hongqing, 纠正逆序周跳探测错误，逆序的实质为探测前一个历元的周跳。
//2017.09.04, czs, edit in hongqing, 逆序周跳探测取消已有标记的影响，增加当前周跳判断 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Times;
using Geo;
using Geo.Utils;
using Geo.Common;

namespace Gnsser
{

    /// <summary>
    /// 反向周跳修正器。主要用于探测前几（5）次周跳误报。
    /// 逆序探测前提：1.默认为顺序探测已探测完成，而存在误探。2.逆序探测只负责取消误探。
    /// </summary>
    public class ReverseCycleSlipeReviser : EpochInfoReviser
    {
    
        /// <summary>
        /// 构造函数，采用GNSS计算配置构造。
        /// </summary>
        /// <param name="Option"></param>
        public ReverseCycleSlipeReviser(GnssProcessOption Option)
        {
            this.Name = "逆序周跳探测器";
            if (Option.MinFrequenceCount == 1)
            {
                Detector  = (CycleSlipDetectReviser.DefaultSingeFrequencyDetector(Option,true));

            }
            else if (Option.MinFrequenceCount == 2)
            {
                Detector = CycleSlipDetectReviser.DefaultDoubleFrequencyDetector(Option, true);
            }
            else
            {
                Detector = CycleSlipDetectReviser.DefaultTripleFrequencyDetector(true);

            }            

            this.MinNum = 10;
        }

        /// <summary>
        /// 构造函数，采用GNSS计算配置构造。
        /// </summary>
        /// <param name="Option"></param>
        public ReverseCycleSlipeReviser(SatObsDataType obsType, List<CycleSlipDetectorType> types, GnssProcessOption Option)
        {
            Detector = CycleSlipDetectReviser.CreateCycleSlipReviser(obsType, types, Option, true);

            this.MinNum = 10;
        }


        /// <summary>
        /// 最小需要计算的数
        /// </summary>
        public int MinNum { get; set; }
        /// <summary>
        ///逆向探测器
        /// </summary>
        public CycleSlipDetectReviser Detector { get; set; }
        /// <summary>
        /// 当前信息，不包含在缓存中。
        /// </summary>
        public EpochInformation CurrentEpcohInfo { get; set; }
        /// <summary>
        /// 探测并标记
        /// </summary>
        /// <param name="CurrentEpcohInfo"></param>
        /// <returns></returns>
        public override bool Revise(ref  EpochInformation CurrentEpcohInfo)
        {
            this.CurrentEpcohInfo = CurrentEpcohInfo;

            IWindowData<EpochInformation> buffers = Buffers;
            if (buffers == null || buffers.Count == 0) return true;
             
            if (buffers.First != null)
            {
                foreach (var sat in CurrentEpcohInfo)
                {
                    //逆序，只针对连续两次以上具有周跳的卫星进行探测。
                    var nextSat = buffers.First.Get(sat.Prn);
                    if (nextSat != null)
                    {
                        //若顺序探测，未发现周跳，则不标记其为周跳，避免误探
                        if (sat.IsUnstable && nextSat.IsUnstable) 
                        {
                            var sats = GetSats(buffers, sat, MinNum);

                            DetectOrMark(sats);
                        }
                    }
                }
            }
            return true;
        }
         
        /// <summary>
        /// 探测
        /// </summary>
        /// <param name="sats"></param>
        public void DetectOrMark(List<EpochSatellite> sats)
        {
            //顺序反转
            sats.Reverse();

            StringBuilder sb = new StringBuilder();
            int count = sats.Count;

            int prevSatIndex = -2;
            foreach (var sat in sats)
            {
                prevSatIndex++;
                if (prevSatIndex <= 0) { continue; }
                var prevSat = sats[prevSatIndex];

                bool hasCs = Detector.Detect(sat);
                //若发现当前历元误探，且上一历元周跳，则取消上一历元的周跳标记。
                if (!hasCs && sat.IsUnstable && prevSat.IsUnstable)
                {
                    sb.Append("," + prevSat.Time.Value.ToShortTimeString());
                    prevSat.IsUnstable = false;
                }

            }
            if (sb.Length > 0)
            {
                sb.Insert(0, "移除周跳标记：" + sats[0].Prn);
                log.Debug(sb.ToString());
            }
        }


        /// <summary>
        /// 探测
        /// </summary>
        /// <param name="sats"></param>
        public void DetectOrMarkOld(List<EpochSatellite> sats)
        {
            //顺序反转
            sats.Reverse();

            StringBuilder sb = new StringBuilder();

            int count = sats.Count;

            int i = 0;
            EpochSatellite prevSat = null;
            int prevSatIndex = 0;
            foreach (var sat in sats)
            {
                prevSatIndex = i - 1;
                //逆序时，实质是对其前一个进行探测，注意与顺序（探测下一个）的区别。
                if (!Detector.Detect(sat) && prevSatIndex >= 0)
                {
                    prevSat = sats[prevSatIndex];
                    if (prevSat.IsUnstable)//没有周跳
                    {
                        sb.Append("," + prevSat.Time.Value.ToShortTimeString());
                        prevSat.IsUnstable = false;
                    }
                }
                i++;

            }
            if (sb.Length > 0)
            {
                sb.Insert(0, "移除周跳标记：" + sats[0].Prn);
                log.Debug(sb.ToString());
            }
        }


        /// <summary>
        /// 获取指定的卫星
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="firstSat"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<EpochSatellite> GetSats(IBuffer<EpochInformation> infos, EpochSatellite firstSat, int number)
        {
            List<EpochSatellite> sats = new List<EpochSatellite>();
            sats.Add(firstSat);
            int i = 0;
            foreach (var item in infos)
            {
                if (i >= number) { break; }
                if (item == null) continue;

                var sat = item.Get(firstSat.Prn);
                if (sat != null)
                {
                    sats.Add(sat);
                }
                i++;
            }
            return sats;
        }
    }

    //2017.09.04, czs, create in hongqing, 通用逆序周跳探测器
    ///// <summary>
    ///// 通用逆序周跳探测器
    ///// </summary>
    //public class ReverseCycleSlipDetector : BaseCycleSlipDetector
    //{
    //    public ReverseCycleSlipDetector(ICycleSlipDetector deta)
    //    {

    //    }

    //    public override CycleSlipDetectorType DetectorType
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public override bool Detect(EpochSatellite epochSat)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}