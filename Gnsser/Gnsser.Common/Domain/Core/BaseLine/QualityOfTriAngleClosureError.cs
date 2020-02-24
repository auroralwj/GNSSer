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
    /// 闭合路径质量
    /// </summary>
    public class QualityOfTriAngleClosureError : QualityBaseLine, IComparable<QualityOfTriAngleClosureError>, IObjectRow
    {
        /// <summary>
        /// 闭合路径质量
        /// </summary> 
        /// <param name="ClosureNet"></param>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        public QualityOfTriAngleClosureError(BaseLineNet ClosureNet, GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
            :base(ClosureNet.First, ClosureNet.ClosureError, GnssReveiverNominalAccuracy)
        {
            this.BaseLineNet = ClosureNet;
            this.GnssReveiverNominalAccuracy = GnssReveiverNominalAccuracy; 
            this.ClosureError = BaseLineNet.ClosureError;
            //计算闭合差
            if (BaseLineNet.ClosureError == null)
            {
                BaseLineNet.CalculateSetAndGetClosureError();
                Init(ClosureNet.First, ClosureNet.ClosureError, GnssReveiverNominalAccuracy);
            }
        }


        /// <summary>
        ///  基线 较差类型
        /// </summary>
        public override BaseLineErrorType BaseLineErrorType => BaseLineErrorType.三角形闭合差; 
        /// <summary>
        ///平均长度，用于计算PPM
        /// </summary>
        public override double AverageLength => BaseLineNet.AverageLength;
        /// <summary>
        /// 如果是闭合差，则这是闭合网，否则是全网
        /// </summary>
        public BaseLineNet BaseLineNet { get; set; }
        /// <summary>
        /// 国标规定，三角形闭合差系数 Math.Sqrt(3) / 5.0;
        /// 按照GB/T 18314-2009，B、C级复测基线长度较差应满足：Wx<= 2 √2 σ 
        /// </summary>
        public override double ClousureErrorFactor => Math.Sqrt(3) / 5.0;
        /// <summary>
        /// 是否都合限
        /// </summary>
        public bool IsAllOk { get => LevelIsOk && VerticalIsOk; }
        /// <summary>
        /// 结果状态
        /// </summary>
        public override ResultState ResultState
        {
            get
            {
                return GnssGradeTypeHelper.GradeToResultState(LevelGradeType);
            }
        } 
        /// <summary>
        /// 添加到行
        /// </summary> 
        public override Dictionary<string, object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            row["基线"] = this.BaseLineName;
            row["平均距离"]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.AverageLength, "0.0000"));

            if (this.BaseLineErrorType == BaseLineErrorType.三角形闭合差)
            {
                row["闭合路径"]= this.BaseLineNet.GetPath();
                row[ParamNames.De]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureErrorEnu.E, "G5"));
                row[ParamNames.Dn]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureErrorEnu.N, "G5"));
                row[ParamNames.Du]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureErrorEnu.U, "G5"));

                row["距离差"]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.Length, "G5"));
                row["水平ppm"]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.LevelPPm, "G5"));
                row["水平等级"]=( this.LevelGradeType);
                row["水平合限"]=( this.LevelIsOk);
                row["垂直ppm"]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.VerticalPPm, "G5"));
                row["垂直等级"]=( this.VerticalGradeType);
                row["垂直合限"]=( this.VerticalIsOk);
            }

            if (this.BaseLineErrorType == BaseLineErrorType.复测基线较差)
            {
                row["距离ppm"]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.LengthError, "G5"));
                row["距离等级"]=( this.LevelGradeType);
                row["距离合限"]=( this.LengthIsOk);
            }

            row["ResultState"] = (this.ResultState); 
            row["误差类型"]=( BaseLineErrorType);

            row[ParamNames.Dx]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.X, "G5"));
            row[ParamNames.Dy]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.Y, "G5"));
            row[ParamNames.Dz]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Value.Z, "G5"));

            row[ParamNames.RmsX]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.X, "G5"));
            row[ParamNames.RmsY]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.Y, "G5"));
            row[ParamNames.RmsZ]=( Geo.Utils.DoubleUtil.ToStringThenDouble(this.ClosureError.Rms.Z, "G5"));

            return row;
        }
         

        /// <summary>
        ///比较闭合差总长度
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(QualityOfTriAngleClosureError other)
        {
            return this.ClosureError.Value.Length.CompareTo(other.ClosureError.Value.Length);
        }


        /// <summary>
        /// 三边闭合环检核，构建环路环检核结果
        /// </summary>
        public static ObjectTableStorage BuildSynchNetTrilateralCheckTResultable(
            string netName,
            List<QualityOfTriAngleClosureError> qualities,
            BufferedTimePeriod timeperiod = null, bool IsBadOnly = false)
        {
            //B-E网，GPS
            ObjectTableStorage lineTable = new ObjectTableStorage(timeperiod + "");
            foreach (var quality in qualities)
            {
                if (IsBadOnly && quality.IsAllOk)
                {
                    continue;
                }
                lineTable.NewRow();
                lineTable.AddItem("网编号", netName);
                //lineTable.AddItem("Index", i++); 

                lineTable.AddItem(quality.GetObjectRow());
                if (timeperiod != null)
                {
                    lineTable.AddItem("时段", timeperiod.ToString());
                }
            }

            return lineTable;
        }

        /// <summary>
        /// 正反向基线较差
        /// </summary>
        /// <param name="line"></param>
        /// <param name="reverseLine"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildReverseErrorTable(SiteObsBaseline line, SiteObsBaseline reverseLine)
        {
            ObjectTableStorage reverseErrorTable = null;
            if (line == null || reverseLine == null || line.EstimatedResult == null || reverseLine.EstimatedResult == null)
            {
                return null;
            }
            reverseErrorTable = new ObjectTableStorage(line.LineName + "正反向较差");
            reverseErrorTable.NewRow();
            var deltaXyz = line.EstimatedResult.EstimatedVector + reverseLine.EstimatedResult.EstimatedVector;

            reverseErrorTable.AddItem(ParamNames.Name, line.LineName);
            reverseErrorTable.AddItem(ParamNames.Dx, deltaXyz.X);
            reverseErrorTable.AddItem(ParamNames.Dy, deltaXyz.Y);
            reverseErrorTable.AddItem(ParamNames.Dz, deltaXyz.Z);
            reverseErrorTable.AddItem(ParamNames.Length, deltaXyz.Length);

            var enu = Geo.Coordinates.CoordTransformer.LocaXyzToEnu( deltaXyz, line.EstimatedResult.ApproxXyzOfRef);
            reverseErrorTable.AddItem(ParamNames.De, enu.E);
            reverseErrorTable.AddItem(ParamNames.Dn, enu.N);
            reverseErrorTable.AddItem(ParamNames.Du, enu.U);

            return reverseErrorTable;
        }
    }

}