//2018.07.26, czs, create in HMX, 单历元双站双差
//2018.11.29, czs, edit in hmx, 实现 GetTabTitles ，用于输出

using System;
using System.Collections.Generic;
using Gnsser.Times;
using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.Times; 
using Geo.IO;

namespace Gnsser.Service
{
    /// <summary>
    /// 单历元双站双差
    /// </summary>
    public class DualSiteEpochDoubleDifferResult : MultiSiteGnssResult, IWithEstimatedBaseline
    {
        /// <summary>
        /// 单历元双站双差
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="adjust"></param>
        /// <param name="nameBuilder"></param>
        public DualSiteEpochDoubleDifferResult(MultiSiteEpochInfo epochInfo, AdjustResultMatrix adjust, GnssParamNameBuilder nameBuilder)
            : base(epochInfo, adjust, nameBuilder)
        {
            this.Option = nameBuilder.Option;
        }
        /// <summary>
        /// 定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }

        /// <summary>
        /// 此将用于输出
        /// </summary>
        /// <returns></returns>
        public override string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder(); 
            sb.Append(this.GetEstimatedBaseline().GetTabTitles()); 
            return sb.ToString();
        }
        /// <summary>
        /// 此将用于输出
        /// </summary>
        /// <returns></returns>
        public override string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetEstimatedBaseline().GetTabValues()); 
            return sb.ToString();
        }


        IEstimatedBaseline baseline;
        /// <summary>
        /// 基线估值
        /// </summary>
        public IEstimatedBaseline GetEstimatedBaseline() {
            if(baseline != null) { return baseline; }

            Vector correctionOfVector = this.ResultMatrix.Estimated;
            //估值向量，天线之间的向量？？
            var xyz = XYZ.Parse(correctionOfVector);
            var rms = XYZ.Parse(this.ResultMatrix.Estimated.GetRmsVector());
            var result = new EstimatedBaseline(this.MaterialObj.BaseEpochInfo, this.MaterialObj.OtherEpochInfo, new RmsedXYZ(xyz, rms),new Matrix(this.ResultMatrix.Estimated.InverseWeight.SubMatrix(0,3)), this.ResultMatrix.StdDev);
            result.ResultType = this.ResultMatrix.ResultType;
            result.GnssSolverType = Option.GnssSolverType;
            baseline = result;//保存

            return result;
        }
    }

}