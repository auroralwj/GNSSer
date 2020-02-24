//2014.12.08, czs, create in jinxinliaomao shuangliao, 差分定位结果
//2016.10.10, czs, edit in hongqing, 双站多历元差分定位结果

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Geo.Algorithm;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using  Geo.Algorithm.Adjust;

namespace Gnsser.Service
{
    /// <summary>
    /// 差分定位结果。
    /// </summary>
    public class TwoSitePeriodDifferPositionResult : MultiSitePeriodGnssResult, IWithEstimatedBaseline// IEstimatedBaseline
    {
        /// <summary>
        /// 以观测信息进行初始化
        /// </summary>
        /// <param name="Adjustment">平差</param>
        /// <param name="BasePrn">基准星</param>
        /// <param name="PeriodDifferInfo">区间差分信息</param>
        /// <param name="lastPppResult">最后结果</param>
        public TwoSitePeriodDifferPositionResult(
             MultiSitePeriodInfo PeriodDifferInfo,
             AdjustResultMatrix Adjustment,
             SatelliteNumber BasePrn,
             GnssParamNameBuilder nameBuilder )
            : base(PeriodDifferInfo, Adjustment, nameBuilder)
        {
            this.BasePrn = BasePrn;

        }

        #region 属性 

        /// <summary>
        /// 卫星模糊度字典
        /// </summary>
        public Dictionary<SatelliteNumber, int> SatIntAmbiguities
        {
            get
            {
                Dictionary<SatelliteNumber, int> dic = new Dictionary<SatelliteNumber, int>();
                for (int i = 0; i < FloatAmbiguities.Count; i++)
			    {
			        var ambi = FloatAmbiguities[i];
                    var paramName = FloatAmbiguities.ParamNames[i];
                    SatelliteNumber prn =  NameBuilder.TryGetPrn(paramName);
                    dic[prn] = (int)ambi;
			    }
                return dic;
            }
        }


        /// <summary>
        /// 模糊度浮点解
        /// </summary>
        public Vector FloatAmbiguities
        {
            get
            { 
                var length = ResultMatrix.ParamNames.Count;
                Vector vector = new Vector(length - 3);
                int j=0; 
                for (int i = 0; i < length; i++)
                {
                    if (ResultMatrix.ParamNames[i].Contains(Gnsser.ParamNames.Lambda))
                    {
                        vector.SetDimension(1+j);
                        vector.Set(j, ResultMatrix.Estimated[i], ResultMatrix.ParamNames[i]);
                        j++;
                    }
                }

                return vector;
            }
        }
         
        #endregion


        /// <summary>
        /// 基线估值
        /// </summary>
        public IEstimatedBaseline GetEstimatedBaseline()
        {
            Vector correctionOfVector = this.ResultMatrix.Estimated;
            //估值向量，天线之间的向量？？
            var xyz = XYZ.Parse(correctionOfVector);
            var rms = XYZ.Parse(this.ResultMatrix.Estimated.GetRmsVector());
            var result = new EstimatedBaseline(this.MaterialObj.First.BaseEpochInfo, this.MaterialObj.First.OtherEpochInfo, new RmsedXYZ(xyz, rms), new Matrix(ResultMatrix.CovaOfEstimatedParam.SubMatrix(0, 3)), ResultMatrix.StdDev)
            {
                 ResultType = this.ResultMatrix.ResultType,
                GnssSolverType = this.NameBuilder.Option.GnssSolverType
            };

            return result;
        }

        #region  标准化输出
        /// <summary>
        /// 参数名称字符串 ParamNames
        /// </summary>
        /// <returns></returns>
        public override string GetTabTitles()
        {
            var baseLine = GetEstimatedBaseline(); 
            return baseLine.GetTabTitles();
        }

        /// <summary>
        /// 以制表符分开的值为 Adjustment.Corrected
        /// </summary>
        /// <returns></returns>
        public override string GetTabValues()
        {
            var baseLine = GetEstimatedBaseline(); 
            return baseLine.GetTabValues();
        }

        /// <summary>
        /// 差分定位结果。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Material.UnstablePrns != null && this.Material.UnstablePrns.Count > 0)
            {
                sb.Append("CycleSlip:");
                foreach (var item in this.Material.UnstablePrns)
                {
                    sb.Append(item.ToString());
                    sb.Append(" ");
                }
            }
            return base.ToString() + " " + sb.ToString();
        }
        #endregion
         
    }
}