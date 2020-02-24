//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Correction;
using Gnsser.Correction;
using Geo;

namespace Gnsser.Domain
{ 
    /// <summary>
    /// 观测量，包含载波、伪距、多普勒，观测值得组合观测值等。
    /// 观测量由观测值和其改正组数成。
    /// </summary>
    public class ObservationWithCommonCorrection : Observation
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param>
        /// <param name="CommonCorrection">改正器，也可以后来赋值</param>
        public ObservationWithCommonCorrection(double val, NumerialCorrectionDic CommonCorrection, ObservationCode ObservationCode)
            : base(val, ObservationCode)
        {
            this.CommonCorrection = CommonCorrection;
        }
        /// <summary>
        /// 外部公共改正数。
        /// </summary>
        public NumerialCorrectionDic CommonCorrection { get; private set; }

        public override Double Correction
        {
            get
            {
                double all = TotalCorrection;
                if (CommonCorrection != null) all += CommonCorrection.TotalCorrection;
                return all;
            }
            set
            {
                throw new NotImplementedException("请使用 AddCorrection ");
            }
        }
    }
}