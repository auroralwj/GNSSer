//2018.11.28, czs, create in hmx, GNSS 接收机

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser
{
    //相对精度25万以上

    /// <summary>
    ///  GNSS 接收机
    /// </summary>
    public class GnssReceiver
    {
       

    }


    /// <summary>
    /// GNSS 类型等级
    /// </summary>
    public enum GnssGradeType
    {
        AA,
        A,
        B,
        C,
        D,
        E,
        No,
        Unknown
    }
    /// <summary>
    /// 转换工具
    /// </summary>
    public class GnssGradeTypeHelper
    {
        /// <summary>
        /// 转换结果类型
        /// </summary>
        /// <param name="LevelGradeType"></param>
        /// <returns></returns>
        public static ResultState GradeToResultState(GnssGradeType LevelGradeType)
        {
            ResultState resultState = ResultState.Unknown;
            switch (LevelGradeType)
            {
                case GnssGradeType.AA:
                case GnssGradeType.A:
                    resultState = ResultState.Good;
                    break;
                case GnssGradeType.B:
                case GnssGradeType.C:
                    resultState = ResultState.Acceptable;
                    break;
                case GnssGradeType.D:
                case GnssGradeType.E:
                    resultState = ResultState.Warning;
                    break;
                case GnssGradeType.No:
                    resultState = ResultState.Bad;
                    break;
                case GnssGradeType.Unknown:
                    resultState = ResultState.Unknown;
                    break;
                default:
                    resultState = ResultState.Unknown;
                    break;
            }
            return resultState;
        }
    }
    /// <summary>
    ///GNSS接收机标称精度水平和垂直,封装了一些工具
    /// </summary>
    public class GnssReveiverNominalAccuracy
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="levelFixed"></param>
        /// <param name="verticalFixed"></param>
        /// <param name="levelCoeef"></param>
        /// <param name="verticalCoeef"></param>
        public GnssReveiverNominalAccuracy(double levelFixed, double verticalFixed, double levelCoeef, double verticalCoeef)
            :this(new LevelVertical(levelFixed, verticalFixed), new LevelVertical(levelCoeef, verticalCoeef))
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FixedValue">固定误差，单位为毫米 mm</param>
        /// <param name="CoefOfProportion">注意单位为百万分之一米（ppm）</param>
        public GnssReveiverNominalAccuracy(LevelVertical FixedValue, LevelVertical CoefOfProportion)
        {
            this.FixedValue = FixedValue;
            this.CoefOfProportion = CoefOfProportion;
        }
         
        /// <summary>
        /// 固定误差
        /// </summary>
        public LevelVertical FixedValue { get; set; }
        /// <summary>
        /// 比例误差系数，
        /// </summary>
        public LevelVertical CoefOfProportion { get; set; }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FixedValue + " " + CoefOfProportion;
        }
        /// <summary>
        /// 当前参数设置下的接收机限差，单位毫米
        /// </summary>
        /// <param name="distanceMeter">距离，米</param>
        /// <returns></returns>
        public LevelVertical GetToleranceErrorMilliMeter(double distanceMeter)
        {
            var level = GetToleranceErrorMilliMeter(FixedValue.Level, CoefOfProportion.Level, distanceMeter);
            var vertical = GetToleranceErrorMilliMeter(FixedValue.Vertical, CoefOfProportion.Vertical, distanceMeter);
            return new LevelVertical(level, vertical);
        }
        #region 静态工具方法
        /// <summary>
        /// 获取当前精度所处等级
        /// </summary>
        /// <param name="closureErrorMeter">闭合差，单位：米</param>
        /// <param name="distanceMeter">距离，单位：米</param>
        /// <returns></returns>
        public static GnssGradeType GetGnssGrade(double closureErrorMeter, double distanceMeter)
        {
            closureErrorMeter = Math.Abs(closureErrorMeter); 

            var grades = Enum.GetNames(typeof(GnssGradeType));
            foreach (var grade in grades)//顺序从小到达
            {
                GnssGradeType gnssGrade = Geo.Utils.EnumUtil.Parse<GnssGradeType>(grade);
                var tolerance = GnssReveiverNominalAccuracy.GetGnssGradeToleranceMeter(gnssGrade, distanceMeter);
                if (closureErrorMeter < tolerance)
                {
                    return gnssGrade;
                }
            }
            return GnssGradeType.No;
        }

        /// <summary>
        /// GNSS网的精度限差, 单位米
        /// </summary>
        /// <param name="gnssGrade">GNSS网等级</param>
        /// <param name="distanceMeter">基线长度，单位米</param>
        /// <returns>GNSS网的精度限差, 单位米</returns>
        public static double GetGnssGradeToleranceMeter(GnssGradeType gnssGrade, double distanceMeter)
        {
            double fixedError = 3;
            double coefOfProportion = 1;
            switch (gnssGrade)
            {
                case GnssGradeType.AA:
                    fixedError = 3;
                    coefOfProportion = 0.01;
                    break;
                case GnssGradeType.A:
                    fixedError = 5;
                    coefOfProportion = 0.1;
                    break;
                case GnssGradeType.B:
                    fixedError = 8;
                    coefOfProportion = 1;
                    break;
                case GnssGradeType.C:
                    fixedError = 10;
                    coefOfProportion = 5;
                    break;
                case GnssGradeType.D:
                    fixedError = 10;
                    coefOfProportion = 10;
                    break;
                case GnssGradeType.E:
                    fixedError = 10;
                    coefOfProportion = 20;
                    break;
                default:
                    break;
            }
           
            var mm = GnssReveiverNominalAccuracy.GetToleranceErrorMilliMeter(fixedError, coefOfProportion, distanceMeter);
            return mm * 0.001; //单位转换为米
        }
        
        /// <summary>
        ///  GNSS 测量精度计算公式
        /// </summary> 
        /// <param name="gnssReveiverType">接收机类型</param>
        /// <param name="distanceMeter">基线长度</param>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public static LevelVertical GetReceiverToleranceError(GnssReveiverType gnssReveiverType, double distanceMeter, string version = null)
        {
            GnssReveiverNominalAccuracy accuracy = GetNominalAccuracyOfGnssReveiver(gnssReveiverType,  version);

            return accuracy.GetToleranceErrorMilliMeter(distanceMeter);
        }

        /// <summary>
        /// 获取接收机标称精度，静态。是否应该写入文件！！！
        /// </summary>
        /// <param name="gnssReveiverType">接收机类型</param> 
        /// <param name="version">版本</param>
        /// <returns></returns>
        public static GnssReveiverNominalAccuracy GetNominalAccuracyOfGnssReveiver(GnssReveiverType gnssReveiverType,  string version = null)
        { 
            var hasVersion = !String.IsNullOrWhiteSpace(version);
            switch (gnssReveiverType)
            {
                case GnssReveiverType.Laica:
                    return new GnssReveiverNominalAccuracy(new LevelVertical(5, 10), new LevelVertical(0.5, 0.5));
                case GnssReveiverType.Trimle://R10 为3mm水平，5700高程10mm
                    if (hasVersion)
                    {
                        if (version.Contains("R10"))
                        {
                            return new GnssReveiverNominalAccuracy(new LevelVertical(3, 5), new LevelVertical(0.5, 0.5));
                        }
                        else if (version.Contains("5700"))
                        {
                            return new GnssReveiverNominalAccuracy(new LevelVertical(5, 10), new LevelVertical(1, 1));
                        }
                    }
                    return new GnssReveiverNominalAccuracy(new LevelVertical(5, 5), new LevelVertical(0.5, 1));
                case GnssReveiverType.Ashetech:
                    if (hasVersion)
                    {
                        if (version.Contains("Z-12") || version.Contains("M-12"))
                        {
                            return new GnssReveiverNominalAccuracy(new LevelVertical(5, 10), new LevelVertical(1, 1));
                        }
                    }
                    return new GnssReveiverNominalAccuracy(new LevelVertical(5, 10), new LevelVertical(0.5, 0.5));
                case GnssReveiverType.Topcon:
                    return new GnssReveiverNominalAccuracy(new LevelVertical(3, 5), new LevelVertical(0.5, 0.5));
                case GnssReveiverType.ZhongHaiDa:
                    return new GnssReveiverNominalAccuracy(new LevelVertical(3, 5), new LevelVertical(0.5, 1));
                case GnssReveiverType.HuaCe:
                    return new GnssReveiverNominalAccuracy(new LevelVertical(5, 10), new LevelVertical(1, 2));
                case GnssReveiverType.NanFang:
                    return new GnssReveiverNominalAccuracy(new LevelVertical(3, 5), new LevelVertical(1, 1));
                default:
                    return new GnssReveiverNominalAccuracy(new LevelVertical(5, 10), new LevelVertical(0.5, 0.5));
            }
        }
        
        /// <summary>
        /// GNSS 测量精度计算公式,返回标准差，㎜
        /// </summary>
        /// <param name="fixedErrorMiniMeter">固定误差，按照国标，单位毫米</param>
        /// <param name="coefOfProportion">比例误差系数，按照国标，单位百万分之一</param>
        /// <param name="distanceMeter">基线距离，单位 米</param>
        /// <returns></returns>
        public static double GetToleranceErrorMilliMeter(double fixedErrorMiniMeter, double coefOfProportion, double distanceMeter)
        {
            distanceMeter *= 1000;//转换为毫米
            var val = Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(fixedErrorMiniMeter, coefOfProportion * distanceMeter * 10e-6);
            return val;
        }
        #endregion
    }

    /// <summary>
    /// 水平和垂直
    /// </summary>
    public class LevelVertical
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Level"></param>
        /// <param name="Vertical"></param>
        public LevelVertical(double Level, double Vertical)
        {
            this.Level = Level;
            this.Vertical = Vertical;
        }
        /// <summary>
        /// 水平
        /// </summary>
        public double Level { get; set; }
        /// <summary>
        /// 垂直
        /// </summary>
        public double Vertical { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get => Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(Level, Vertical); }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Level  + ", " + Vertical;
        }
    }

    /// <summary>
    /// 常见GNSS接收机类型
    /// </summary>
    public enum GnssReveiverType
    {
        Laica,
        Trimle,
        Ashetech,
        Topcon,
        ZhongHaiDa,
        HuaCe,
        NanFang
    }


}
