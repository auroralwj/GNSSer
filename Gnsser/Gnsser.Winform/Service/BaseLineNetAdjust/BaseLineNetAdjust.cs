//2018.12.04, czs, create in hmx, 网平差单独移出来

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;
using Geo.IO;
using System.IO;
using Geo.Coordinates;
using Geo.Times;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;

namespace Gnsser
{
    /// <summary>
    /// 基线网计算结果
    /// </summary>
    public class BaselineNetResult : AdjustResult
    {
        public BaselineNetResult(BaseLineNetManager BaseLineNet, List<string> sites, AdjustResultMatrix resultMatrix)
            : base(resultMatrix)
        {
            this.BaseLineNet = BaseLineNet;
            this.ResultMatrix = resultMatrix;
            this.StdDev = this.ResultMatrix.StdDev;
            //提取改正数
            SetCorrection(sites, resultMatrix);

            //计算改正结果 
            SetCorrected(BaseLineNet, sites);


        }
        #region  初始化
        //计算改正结果 
        private void SetCorrected(BaseLineNetManager BaseLineNet, List<string> sites)
        {
            Corrected = new Dictionary<string, RmsedXYZ>();
            foreach (var name in sites)
            {
                var appXyz = BaseLineNet.GetApproxXyz(name);
                var xyz = appXyz + Correction[name];
                Corrected[name] = xyz;
            }
        }

        //提取改正数
        private void SetCorrection(List<string> sites, AdjustResultMatrix resultMatrix)
        {
            Correction = new Dictionary<string, RmsedXYZ>();
            int i = 0;
            var Estimated = resultMatrix.Estimated;
            var stdDevOfEst = resultMatrix.StdOfEstimatedParam;
            foreach (var name in sites)
            {
                var estXyz = new XYZ();
                var x = Estimated[i + 0];
                var y = Estimated[i + 1];
                var z = Estimated[i + 2];
                var xyz = new XYZ(x, y, z);

                var rmsx = (stdDevOfEst[i + 0]);
                var rmsy = (stdDevOfEst[i + 1]);
                var rmsz = (stdDevOfEst[i + 2]);
                var rms = new XYZ(rmsx, rmsy, rmsz);

                Correction[name] = new RmsedXYZ(xyz, rms);
                i = i + 3;
            }
        }
        #endregion

        #region  核心属性
        /// <summary>
        /// 平差结果
        /// </summary>
        public AdjustResultMatrix ResultMatrix { get; set; }
        /// <summary>
        /// 平差标准差
        /// </summary>
        public double StdDev { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public BaseLineNetManager BaseLineNet { get; set; }
        /// <summary>
        /// 估值坐标
        /// </summary>
        public Dictionary<string, RmsedXYZ> Corrected { get; set; }
        /// <summary>
        /// 估值坐标
        /// </summary>
        public Dictionary<string, RmsedXYZ> Correction { get; set; }
        #endregion
        /// <summary>
        /// 获取平差后的基线网
        /// </summary>
        /// <returns></returns>
        public AdjustedBaseLineNetManager GetAdjustedBaseLineNetManager()
        {
            AdjustedBaseLineNetManager manager = new AdjustedBaseLineNetManager();
            foreach (var kv in this.BaseLineNet.KeyValues)
            {
                var  net = manager.GetOrCreate(kv.Key);
                foreach (var item in kv.Value.KeyValues)
                {
                    var refCoord = Corrected[item.Key.RefName];
                    var rovCoord = Corrected[item.Key.RovName];

                    net[item.Key] = new AdjustedBaseLine(item.Value, refCoord, rovCoord, StdDev);
                }
            } 
            return manager;
        }

        #region 输出转换，显示
        public ObjectTableStorage BuildObjectTable()
        {
            ObjectTableStorage resultTable = new ObjectTableStorage("平差结果");
            foreach (var kv in this.Corrected)
            {
                var name = kv.Key;
                var corredXyz = kv.Value;
                var corr = this.Correction[name];

                resultTable.NewRow();
                var row = GetObjectRow(name, corredXyz, corr.Value);
                resultTable.AddItem(row);
            }

            return resultTable;
        }

        public Dictionary<String, Object> GetObjectRow(string name, RmsedXYZ corredXyz, XYZ Dxyz)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();

            row[ParamNames.Name] = name;
            row[ParamNames.X] = corredXyz.Value.X;
            row[ParamNames.Y] = corredXyz.Value.Y;
            row[ParamNames.Z] = corredXyz.Value.Z;
            row[ParamNames.RmsX] = corredXyz.Rms.X;
            row[ParamNames.RmsY] = corredXyz.Rms.Y;
            row[ParamNames.RmsZ] = corredXyz.Rms.Z;
            row[ParamNames.Dx] = Dxyz.X;
            row[ParamNames.Dy] = Dxyz.Y;
            row[ParamNames.Dz] = Dxyz.Z;
            var geoCood = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(corredXyz.Value);

            row[ParamNames.Lon] = geoCood.Lon;
            row[ParamNames.Lat] = geoCood.Lat;
            row[ParamNames.Height] = geoCood.Height;


            return row;
        }
        /// <summary>
        /// 获取向量改正数对象表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetVectorCorrectionTable()
        {
            var manager = GetAdjustedBaseLineNetManager();
            return manager.GetObjectTable(); 
        }
        #endregion
    }


    /// <summary>
    /// 平差基线网
    /// </summary>
    public class AdjustedBaseLineNetManager : BaseDictionary<BufferedTimePeriod, AdjustedBaseLineNet>
    {
        public override AdjustedBaseLineNet Create(BufferedTimePeriod key)
        {
            return new AdjustedBaseLineNet();
        }

        /// <summary>
        /// 获取向量改正数对象表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetObjectTable()
        {
            ObjectTableStorage result = new ObjectTableStorage();
            int i = 1;
            foreach (var kv in this.KeyValues)
            {
                foreach (var item in kv.Value.KeyValues)
                {
                    result.NewRow();
                    result.AddItem(ParamNames.Index, i++);
                    result.AddItem(ParamNames.TimePeriod, kv.Key);
                    result.AddItem(item.Value.GetObjectRow());
                }
            }
            return result;
        }

    }
    public class AdjustedBaseLineNet : BaseDictionary<GnssBaseLineName, AdjustedBaseLine>
    { 
    }

    /// <summary>
    /// 基线平差结果
    /// </summary>
    public class AdjustedBaseLine: IObjectRow
    {
        public AdjustedBaseLine(EstimatedBaseline baseline, RmsedXYZ RefCoord, RmsedXYZ RovCoord,double NetStdDev)
        {
            this.EstimatedBaseline = baseline;
            this.RefCoord = RefCoord;
            this.RovCoord = RovCoord;
            this.NetStdDev = NetStdDev;
            this.MaxAllowedStdDev = 3.0 * this.NetStdDev;//3倍中误差
        }
        /// <summary>
        /// 平差基线
        /// </summary>
        public EstimatedBaseline EstimatedBaseline { get; set; }
        /// <summary>
        /// 网络标准差
        /// </summary>
        public double NetStdDev { get; set; }
        /// <summary>
        /// 最大允许的标准差
        /// </summary>
        public double MaxAllowedStdDev { get; set; }
        /// <summary>
        /// 参考站坐标
        /// </summary>
        public RmsedXYZ RefCoord { get; set; }
        /// <summary>
        /// 流动站坐标
        /// </summary>
        public RmsedXYZ RovCoord { get; set; }
        /// <summary>
        /// 改正后的向量
        /// </summary>
        public RmsedXYZ CorrectedVector => RovCoord -RefCoord ;

        public XYZ VectorCorrection => CorrectedVector.Value - this.EstimatedBaseline.EstimatedVector;
        /// <summary>
        /// 估值向量
        /// </summary>
        public ENU VectorCorrectionEnu { get => Geo.Coordinates.CoordTransformer.LocaXyzToEnu(VectorCorrection, RefCoord.Value); }

        /// <summary>
        /// 是否 X 合限
        /// </summary>
        public bool IsXOk => Math.Abs(this.VectorCorrection.X) <= MaxAllowedStdDev;
        public bool IsYOk => Math.Abs(this.VectorCorrection.Y) <= MaxAllowedStdDev;
        public bool IsZOk => Math.Abs(this.VectorCorrection.Z) <= MaxAllowedStdDev;
        public bool IsEOk => Math.Abs(this.VectorCorrectionEnu.E) <= MaxAllowedStdDev;
        public bool IsNOk => Math.Abs(this.VectorCorrectionEnu.N) <= MaxAllowedStdDev;
        public bool IsUOk => Math.Abs(this.VectorCorrectionEnu.U) <= MaxAllowedStdDev;
        public bool IsLenOk => Math.Abs(this.VectorCorrection.Length) <= MaxAllowedStdDev;


        public Dictionary<string, Object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();

            row[ParamNames.Name] = EstimatedBaseline.Name;
            row[ParamNames.ResultType] = EstimatedBaseline.ResultType;
            var differ = VectorCorrection; 

            row[ParamNames.Dx] = differ.X;
            row[ParamNames.Dy] = differ.Y;
            row[ParamNames.Dz] = differ.Z;
            row[ParamNames.Length] = differ.Length;

            row["IsXOk"] = IsXOk;
            row["IsYOk"] = IsYOk;
            row["IsZOk"] = IsZOk;
            row["IsLenOk"] = IsLenOk;

            var enu = VectorCorrectionEnu;
            row[ParamNames.De] = enu.E;
            row[ParamNames.Dn] = enu.N;
            row[ParamNames.Du] = enu.U;
            
            row["IsEOk"] = IsEOk;
            row["IsNOk"] = IsNOk;
            row["IsUOk"] = IsUOk;

            var Corrected = CorrectedVector;
            row[ParamNames.RmsX] = Corrected.Rms.X;
            row[ParamNames.RmsY] = Corrected.Rms.Y;
            row[ParamNames.RmsZ] = Corrected.Rms.Z;

            row[ParamNames.StdDev] = NetStdDev;
            row[ParamNames.Distance] = EstimatedBaseline.EstimatedVector.Length;
              
            row[ParamNames.RefX] = RefCoord.Value.X;
            row[ParamNames.RefY] = RefCoord.Value.Y;
            row[ParamNames.RefZ] = RefCoord.Value.Z;
            row[ParamNames.RovX] = RovCoord.Value.X;
            row[ParamNames.RovY] = RovCoord.Value.Y;
            row[ParamNames.RovZ] = RovCoord.Value.Z;

            row[ParamNames.GnssSolverType] = EstimatedBaseline.GnssSolverType;
            row[ParamNames.Epoch] = EstimatedBaseline.Epoch;

            return row;
        }
    }

}
