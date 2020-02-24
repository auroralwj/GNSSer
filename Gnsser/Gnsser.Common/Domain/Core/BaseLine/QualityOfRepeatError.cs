//2018.11.09, czs, create in HMX, 一条基线的平差结果
//2018.11.10, czs, create in hmx, 增加基线组合类
//2018.11.30, czs, create in hmx, 实现IToTabRow接口，用于规范输出,合并定义新的 BaseLineNet
//2019.01.16, czs, edit in hmx, 重新设计质量类

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using System.Linq;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Geo;
using AnyInfo.Graphs.Structure;
using Geo.Times;
using AnyInfo.Graphs;

namespace Gnsser
{

    /// <summary>
    /// 基线误差类型
    /// </summary>
    public enum BaseLineErrorType
    {
        /// <summary>
        /// 三角形闭合差
        /// </summary>
        三角形闭合差,
        /// <summary>
        /// 复测基线较差
        /// </summary>
        复测基线较差
    }

    /// <summary>
    /// 复测基线较差管理器
    /// </summary>
    public class RepeatErrorQualityManager : BaseDictionary<GnssBaseLineName, QualityOfRepeatError>
    {
         

    }


    /// <summary>
    /// 抽象类
    /// </summary>
    public  class QualityBaseLine : IObjectRow
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QualityBaseLine()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseLine"></param>
        /// <param name="ClosureError"></param>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        public   QualityBaseLine(EstimatedBaseline baseLine, RmsedXYZ ClosureError, GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
        {
            Init(baseLine, ClosureError, GnssReveiverNominalAccuracy);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="baseLine"></param>
        /// <param name="ClosureError"></param>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        public void Init(EstimatedBaseline baseLine, RmsedXYZ ClosureError, GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
        {
            this.ClosureError = ClosureError;
            this.EstimatedBaseline = baseLine;
            this.GnssReveiverNominalAccuracy = GnssReveiverNominalAccuracy;
        }


        #region 核心属性
        /// <summary>
        /// 闭合差，可以是长度较差，也可以是三角形闭合差
        /// </summary>
        public RmsedXYZ ClosureError { get; set; }
        /// <summary>
        /// 基线
        /// </summary>
        public EstimatedBaseline EstimatedBaseline { get; set; }
        /// <summary>
        /// 接收机标称精度
        /// </summary>
        public GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy { get; set; }
        #endregion

        #region  便捷属性
        /// <summary>
        /// 国标规定，三角形闭合差系数 Math.Sqrt(3) / 5.0;
        /// 按照GB/T 18314-2009，B、C级复测基线长度较差应满足：Wx<= 2 √2 σ 
        /// </summary>
        public virtual double ClousureErrorFactor => 2 * Math.Sqrt(2); 
        /// <summary>
        /// 名称
        /// </summary>
        public GnssBaseLineName BaseLineName => EstimatedBaseline.BaseLineName;

        /// <summary>
        /// 基线长度
        /// </summary>
        public virtual double AverageLength => EstimatedBaseline.EstimatedVector.Length;
        /// <summary>
        /// 三维距离偏差
        /// </summary>
        public double LengthError => ClosureError.Value.Length;
        /// <summary>
        /// 距离PPM
        /// </summary>
        public double LengthPpm => (LengthError / AverageLength) / 1e-6;
        /// <summary>
        /// 水平长度误差
        /// </summary>
        public double LevelLengthError => ClosureErrorEnu.LevelLength;
        /// <summary>
        /// 水平PPM
        /// </summary>
        public double LevelPPm => (LevelLengthError / AverageLength) / 1e-6;
        /// <summary>
        /// 垂直长度误差
        /// </summary>
        public double VerticalLengthError => Math.Abs(ClosureErrorEnu.U);
        /// <summary>
        /// 垂直PPm
        /// </summary>
        public double VerticalPPm => (VerticalLengthError / AverageLength) / 1e-6;

        /// <summary>
        ///  基线 较差类型
        /// </summary>
        public virtual BaseLineErrorType BaseLineErrorType => BaseLineErrorType.复测基线较差; 
        /// <summary>
        /// 误差限差，毫米
        /// </summary>
        public LevelVertical ToleranceMilliMeter => GnssReveiverNominalAccuracy.GetToleranceErrorMilliMeter(AverageLength);
        /// <summary>
        /// 水平GNSS等级
        /// </summary>
        public GnssGradeType LevelGradeType => GnssReveiverNominalAccuracy.GetGnssGrade(LevelLengthError, AverageLength);
        /// <summary>
        /// 距离GNSS等级
        /// </summary>
        public GnssGradeType LengthGradeType => GnssReveiverNominalAccuracy.GetGnssGrade(LengthError, AverageLength);
        /// <summary>
        /// 垂直GNSS等级
        /// </summary>
        public GnssGradeType VerticalGradeType => GnssReveiverNominalAccuracy.GetGnssGrade(VerticalLengthError, AverageLength);
        /// <summary>
        /// 水平是否合限
        /// </summary>
        public bool LevelIsOk => (LevelLengthError <= ClousureErrorFactor * ToleranceMilliMeter.Level / 1000.0);
        /// <summary>
        /// 垂直是否合限
        /// </summary>
        public bool VerticalIsOk => ( VerticalLengthError <= ClousureErrorFactor * ToleranceMilliMeter.Vertical / 1000.0);
        /// <summary>
        /// 距离长度是否合限
        /// </summary>
        public bool LengthIsOk => (LengthError <= ClousureErrorFactor * ToleranceMilliMeter.Length / 1000.0);

        /// <summary>
        /// ENU 方向的闭合差
        /// </summary>
        public ENU ClosureErrorEnu => Geo.Coordinates.CoordTransformer.LocaXyzToEnu(ClosureError.Value, EstimatedBaseline.ApproxXyzOfRef);
        /// <summary>
        /// 结果状态
        /// </summary>
        public virtual ResultState ResultState
        {
            get
            {
                return GnssGradeTypeHelper.GradeToResultState(this.LengthGradeType);
            }
        }

        #endregion

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, Object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();

            row["基线"] = this.BaseLineName;
            row["基线距离"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.AverageLength, "0.0000"));

            row["距离较差"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.LengthError, "G5"));
            row["距离ppm"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.LengthPpm, "G5"));
            row["距离等级"] = (this.LengthGradeType);
            row["距离合限"] = (this.LengthIsOk);

            var enu = this.ClosureErrorEnu;
            row[ParamNames.De] = (Geo.Utils.DoubleUtil.ToStringThenDouble(enu.E, "G5"));
            row[ParamNames.Dn] = (Geo.Utils.DoubleUtil.ToStringThenDouble(enu.N, "G5"));
            row[ParamNames.Du] = (Geo.Utils.DoubleUtil.ToStringThenDouble(enu.U, "G5"));

            row["ResultState"] = (this.ResultState);
            row["水平较差"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.LevelLengthError, "G5"));
            row["水平ppm"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.LengthPpm, "G5"));
            row["水平等级"] = (this.LevelGradeType);
            row["水平合限"] = (this.LevelIsOk);

            row["垂直较差"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.VerticalLengthError, "G5"));
            row["垂直ppm"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.VerticalPPm, "G5"));
            row["垂直等级"] = (this.VerticalGradeType);
            row["垂直合限"] = (this.VerticalIsOk);


            row[ParamNames.Dx] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.X, "G5"));
            row[ParamNames.Dy] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.Y, "G5"));
            row[ParamNames.Dz] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.Z, "G5"));

            row[ParamNames.RmsX] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.X, "G5"));
            row[ParamNames.RmsY] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.Y, "G5"));
            row[ParamNames.RmsZ] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.Z, "G5"));
            row["误差类型"] = (BaseLineErrorType);
            return row;
        }
    }
    

    /// <summary>
    /// 基线质量类,主要包括同步环闭合差和时段长度较差
    /// </summary>
    public class QualityOfRepeatError : QualityBaseLine, IComparable<QualityOfRepeatError>
    {
        /// <summary>
        /// 复测基线较差
        /// </summary>
        /// <param name="ClosureError"></param>
        /// <param name="baseLine"></param>
        /// <param name="GnssReveiverNominalAccuracy"></param> 
        public QualityOfRepeatError(EstimatedBaseline baseLine, RmsedXYZ ClosureError, GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
            :base(baseLine, ClosureError, GnssReveiverNominalAccuracy)
        {
            this.ClosureError = ClosureError;
            this.EstimatedBaseline = baseLine;
            this.GnssReveiverNominalAccuracy = GnssReveiverNominalAccuracy;
        }


        /// <summary>
        /// 国标规定，三角形闭合差系数 Math.Sqrt(3) / 5.0;
        /// 按照GB/T 18314-2009，B、C级复测基线长度较差应满足：Wx<= 2 √2 σ 
        /// </summary>
        public override double ClousureErrorFactor => 2 * Math.Sqrt(2);
        /// <summary>
        /// 结果状态
        /// </summary>
        public override ResultState ResultState
        {
            get
            {
                return GnssGradeTypeHelper.GradeToResultState(this.LengthGradeType);
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, Object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            row["基线"] = this.BaseLineName;
            row["Epoch"] = this.EstimatedBaseline.Epoch;
            row["基线距离"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.AverageLength, "0.0000"));

            row["距离较差"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.LengthError, "G5"));
            row["距离ppm"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.LengthPpm, "G5"));
            row["距离等级"] = (this.LengthGradeType);
            row["距离合限"] = (this.LengthIsOk);
            var enu = this.ClosureErrorEnu;
            row[ParamNames.De] = (Geo.Utils.DoubleUtil.ToStringThenDouble(enu.E, "G5"));
            row[ParamNames.Dn] = (Geo.Utils.DoubleUtil.ToStringThenDouble(enu.N, "G5"));
            row[ParamNames.Du] = (Geo.Utils.DoubleUtil.ToStringThenDouble(enu.U, "G5"));

            row["ResultState"] = (this.ResultState);


            row["水平较差"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.LevelLengthError, "G5"));
            row["水平ppm"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.LengthPpm, "G5"));
            row["水平等级"] = (this.LevelGradeType);
            row["水平合限"] = (this.LevelIsOk);

            row["垂直较差"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.VerticalLengthError, "G5"));
            row["垂直ppm"] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.VerticalPPm, "G5"));
            row["垂直等级"] = (this.VerticalGradeType);
            row["垂直合限"] = (this.VerticalIsOk);


            row[ParamNames.Dx] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.X, "G5"));
            row[ParamNames.Dy] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.Y, "G5"));
            row[ParamNames.Dz] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.Z, "G5"));

            row[ParamNames.RmsX] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.X, "G5"));
            row[ParamNames.RmsY] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.Y, "G5"));
            row[ParamNames.RmsZ] = (Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.Z, "G5"));
            row["误差类型"] = (BaseLineErrorType);
            return row;
        }

        /// <summary>
        ///比较闭合差总长度
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(QualityOfRepeatError other)
        {
            return this.ClosureError.Value.Length.CompareTo(other.ClosureError.Value.Length);
        }
    }

}