//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Geo.Correction;
using Geo.IO;


namespace Gnsser.Correction
{
    //2018.09.06, czs, edit in hmx, 改正数的名称修改为枚举类型
    /// <summary>
    /// 改正数的名称
    /// </summary>
    public enum CorrectionNames
    {
        Iono,
        SatClockBiasDistance,
        Trop,
        DcbOfP1ToLc,
        DcbOfP2ToLc,
        DcbP1C1,
        DcbP2C2,
        PhaseSmoothRange,
        PhaseSmoothRangeA,
        PhaseSmoothRangeB,
        PhaseSmoothRangeC
        ///// <summary>
        ///// IOno
        ///// </summary>
        //public const string Iono = "Iono";
        ///// <summary>
        ///// SatClockBiasDistance
        ///// </summary>
        //public const string SatClockBiasDistance = "cdts";
        ///// <summary>
        ///// Trop
        ///// </summary>
        //public const string Trop = "Trop";
        ///// <summary>
        ///// P1DcbToLc
        ///// </summary>
        //public const string DcbOfP1ToLc = "DcbOfP1ToLc";
        ///// <summary>
        ///// P2DcbToLc
        ///// </summary>
        //public const string DcbOfP2ToLc = "DcbOfP2ToLc";
        ///// <summary>
        ///// DcbP1C1
        ///// </summary>
        //public const string DcbP1C1 = "DcbP1C1";
        ///// <summary>
        ///// DcbP2C2
        ///// </summary>
        //public const string DcbP2C2 = "DcbP2C2";
    }


    /// <summary>
    /// 卫星距离改正器基类,改正观测值本身。
    /// </summary>
    public abstract class AbstractSelfRangeCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 卫星距离改正器基类,改正观测值本身。
        /// </summary>
        public AbstractSelfRangeCorrector()
        {
            CorrectChianType = CorrectChianType.Self;
        }
    }
    /// <summary>
    ///  卫星距离改正器基类。
    /// </summary>
    public abstract class AbstractRangeCorrector : AbstractCorrector<double, EpochSatellite>, IRangeCorrector<EpochSatellite>, IRangeCorrector
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected Log log = new Log(typeof(AbstractRangeCorrector));

        /// <summary>
        /// 改正器类型
        /// </summary>
        public  CorrectChianType CorrectChianType { get; set; }
        /// <summary>
        /// 改正类型
        /// </summary>
        public CorrectionType CorrectionType { get; protected set; }
         /// <summary>
         /// 改正
         /// </summary>
         /// <param name="input"></param>
        public override abstract void Correct(EpochSatellite input);
    }

     
}