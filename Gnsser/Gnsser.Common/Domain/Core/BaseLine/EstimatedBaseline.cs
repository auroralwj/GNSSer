//2018.11.09, czs, create in HMX, 一条基线的平差结果
//2018.11.10, czs, create in hmx, 增加基线组合类
//2018.11.30, czs, create in hmx, 实现IToTabRow接口，用于规范输出,合并定义新的 BaseLineNet
//2019.01.09, czs, edit in hmx, 增加大地方位角

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
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
using Geo.Times;
using AnyInfo.Graphs.Structure;


namespace Gnsser
{
    /// <summary>
    /// 一条基线的平差结果
    /// </summary>
    public class EstimatedBaseline : IEstimatedBaseline, IReadable, IObjectRow
    {
        /// <summary>
        /// 默认
        /// </summary>
        public EstimatedBaseline()
        {
            this.BaseLineName = new GnssBaseLineName();
            EstimatedVectorRmsedXYZ = new RmsedXYZ();
            StdDev = 1;
            ClosureError = 1;
        }
        /// <summary>
        /// 最简单的构造函数，支持从文本读取
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="refXYyz"></param>
        /// <param name="rovXYyz"></param>
        /// <param name="epoch"></param>
        /// <param name="EstimatedVector"></param>
        /// <param name="CovaMatrix"></param>
        /// <param name="stdDev"></param>
        public EstimatedBaseline(string Name, Time epoch, XYZ refXYyz, XYZ rovXYyz, RmsedXYZ EstimatedVector, Matrix CovaMatrix, double stdDev)
        {
            this.BaseLineName = new GnssBaseLineName(Name);
            this.CovaMatrix = CovaMatrix;
            this.ApproxXyzOfRef = refXYyz;
            this.ApproxXyzOfRov = rovXYyz;
            this.EstimatedVectorRmsedXYZ = EstimatedVector;
            this.Epoch = epoch;
            this.StdDev = stdDev;     
            
            this.CorrectionOfRov = new RmsedXYZ((EstimatedVector.Value - ApproxVector), EstimatedVector.Rms);
            this.ClosureError = 1;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refSite"></param>
        /// <param name="rovSite"></param>
        /// <param name="stdDev"></param>
        /// <param name="CovaMatrix"></param>
        /// <param name="RovEstmatedXYZCorrection"></param>
        public EstimatedBaseline(EpochInformation refSite, EpochInformation rovSite, RmsedXYZ RovEstmatedXYZCorrection, Matrix CovaMatrix, double stdDev)
            : this(refSite.SiteInfo, rovSite.SiteInfo, refSite.ReceiverTime, RovEstmatedXYZCorrection, CovaMatrix, stdDev)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="RefName"></param>
        /// <param name="RovName"></param>
        /// <param name="CovaMatrix"></param>
        /// <param name="stdDev"></param>
        /// <param name="RovEstmatedXYZCorrection"></param>
        public EstimatedBaseline(ISiteInfo RefName, 
            ISiteInfo RovName, Time epoch, RmsedXYZ RovEstmatedXYZCorrection, Matrix CovaMatrix, double stdDev)
        {
            this.CovaMatrix = CovaMatrix;
            this.SiteInfoOfRef = RefName;
            this.SiteInfoOfRov = RovName;
            this.Epoch = epoch;
            this.BaseLineName = new GnssBaseLineName(this.SiteInfoOfRov.SiteName, this.SiteInfoOfRef.SiteName);
            this.ApproxXyzOfRef = RefName.ApproxXyz;
            this.ApproxXyzOfRov = RovName.ApproxXyz;
            this.CorrectionOfRov = RovEstmatedXYZCorrection;
            this.EstimatedVectorRmsedXYZ = new RmsedXYZ(ApproxVector + CorrectionOfRov.Value, CorrectionOfRov.Rms);
            StdDev = stdDev;
            ClosureError = 1;
        }
         
        #region 属性

        /// <summary>
        /// 所在网络的时段,需要外部指定。
        /// </summary>
        public TimePeriod NetPeriod { get; set; }
        /// <summary>
        /// 时段,以历元为中心前后 10分钟
        /// </summary>
        public TimePeriod ApporxNetPeriod => new TimePeriod(Epoch - TimeSpan.FromMinutes(10), Epoch + TimeSpan.FromMinutes(10));

        /// <summary>
        /// 计算结果类型
        /// </summary>
        public  ResultType ResultType { get; set; }
        /// <summary>
        /// 算法类型
        /// </summary>
        public GnssSolverType GnssSolverType { get; set; }
        /// <summary>
        /// 基线闭合差长度，默认为1，单位：米
        /// </summary>
        public double ClosureError { get; set; }
        /// <summary>
        /// 方便属性
        /// </summary>
        public object Tag { get; set; }

        #region 核心输入属性
        /// <summary>
        /// XYZ  协方差阵, 3 * 3
        /// </summary>
        public Matrix CovaMatrix { get; set; }
        /// <summary>
        /// 单位权方差，标准差
        /// </summary>
        public double StdDev { get; set; }

        #region  核心的核心，一般只需要参考站坐标和估值向量即可
        /// <summary>
        /// 历元
        /// </summary>
        public Time Epoch { get; set; }
        /// <summary>
        /// 基线名称
        /// </summary>
        public GnssBaseLineName BaseLineName { get; set; }
        /// <summary>
        /// 估值向量
        /// </summary>
        public RmsedXYZ EstimatedVectorRmsedXYZ { get; set; }
        /// <summary>
        /// 参考站坐标，认为是真值。
        /// </summary>
        public XYZ ApproxXyzOfRef { get;  set; }
        #endregion

        /// <summary>
        /// 参考站信息
        /// </summary>
        public ISiteInfo SiteInfoOfRef { get; private set; }
        /// <summary>
        /// 流动站信息
        /// </summary>
        public ISiteInfo SiteInfoOfRov { get; private set; }
        
        #endregion

        /// <summary>
        /// 基线名称
        /// </summary>
        public string Name => BaseLineName.Name;
        /// <summary>
        /// 向量名称
        /// </summary>
        public string VectorName => BaseLineName.VectorName;
        /// <summary>
        /// 流动近似坐标是否可用。
        /// </summary>
        public bool IsApproxXyzOfRovAvailable { get => ApproxXyzOfRov != null; }
        /// <summary>
        /// 流动站估值坐标
        /// </summary>
        public XYZ EstimatedXyzOfRov { get => ApproxXyzOfRef + EstimatedVector; }
        /// <summary>
        /// 估值 XYZ 和 RMS
        /// </summary>
        public RmsedXYZ EstimatedRmsXyzOfRov   { get => new RmsedXYZ(EstimatedXyzOfRov, EstimatedVectorRmsedXYZ.Rms); }
        /// <summary>
        /// 流动站估值大地坐标
        /// </summary>
        public GeoCoord EstimatedGeoCoordOfRov { get => CoordTransformer.XyzToGeoCoord(EstimatedXyzOfRov); }
        /// <summary>
        /// 参考站大地坐标
        /// </summary>
        public GeoCoord GeoCoordOfRef { get => CoordTransformer.XyzToGeoCoord(this.ApproxXyzOfRef); }
        /// <summary>
        /// 估值向量
        /// </summary>
        public XYZ EstimatedVector { get => EstimatedVectorRmsedXYZ.Value; }
        /// <summary>
        /// 估值向量
        /// </summary>
        public ENU EstimatedVectorEnu { get => Geo.Coordinates.CoordTransformer.LocaXyzToEnu(EstimatedVector, ApproxXyzOfRef); }
 
        /// <summary>
        /// 大地方位角,单位度小数
        /// </summary>
        public double GeodeticAzimuth
        {
            get
            {
                return Geo.Coordinates.GeodeticUtils.BesselAzimuthAngle(this.ApproxXyzOfRef.Xyz, this.EstimatedXyzOfRov.Xyz);
            }
        }
        /// <summary>
        /// 大地线长，膨胀椭球，以基线中心计算。
        /// </summary>
        public double GeodeticLength
        {
            get
            { 
               return Geo.Coordinates.GeodeticUtils.BesselGeodeticLine(this.GeoCoordOfRef, this.EstimatedGeoCoordOfRov);
            }
        }
        /// <summary>
        /// 球面几何方位角,单位度小数
        /// </summary>
        public double SphereAzimuth
        {
            get
            {
                return Geo.Coordinates.CoordTransformer.GetAzimuthAngle(this.ApproxXyzOfRef, this.EstimatedXyzOfRov);
            }
        }

        #region 需要知道流动站估值坐标才能获取的属性
        /// <summary>
        /// 流动站近似值坐标。
        /// </summary>
        public XYZ ApproxXyzOfRov { get;  set; }
        /// <summary>
        /// 流动站坐标改正数。
        /// </summary>
        public RmsedXYZ CorrectionOfRov { get; private set; }
        /// <summary>
        /// 向量改正数ENU
        /// </summary>
        public ENU CorrectionOfRovEnu { get => Geo.Coordinates.CoordTransformer.LocaXyzToEnu(CorrectionOfRov.Value, ApproxXyzOfRov); }
        /// <summary>
        /// 初始向量
        /// </summary>
        public XYZ ApproxVector { get => ApproxXyzOfRov - ApproxXyzOfRef; }
        #endregion


        #endregion
        /// <summary>
        /// 此测站设置为此坐标,通常一个网中，相同测站坐标应该一致
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xyz"></param>
        public void SetSiteCoord(string name, XYZ xyz)
        {
            if (BaseLineName.RefName == name)
            {
                this.ApproxXyzOfRef = xyz;
            }
            else if (BaseLineName.RovName == name)
            {
                this.ApproxXyzOfRov = xyz;
            }
        }
        /// <summary>
        /// 反转基线，最简模式
        /// </summary>
        public IEstimatedBaseline ReversedBaseline
        {
            get
            {
                Matrix mat = this.CovaMatrix.GetReverse();
                 
                EstimatedBaseline reverse = new EstimatedBaseline(
                    this.BaseLineName.ReverseBaseLine.Name,
                    Epoch,
                    this.ApproxXyzOfRov, this.ApproxXyzOfRef, -this.EstimatedVectorRmsedXYZ, mat, StdDev
                    )
                {
                    ResultType = ResultType,
                    ClosureError = ClosureError,NetPeriod = NetPeriod,
                    GnssSolverType = this.GnssSolverType
                };
                return reverse;
            }
        }
        /// <summary>
        /// 获取近似坐标
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public XYZ GetApproxXyz(string siteName)
        {
            if (siteName == BaseLineName.RefName)
            {
                return ApproxXyzOfRef;
            }
            if (siteName == BaseLineName.RovName)
            {
                return ApproxXyzOfRov;
            }
            return null;
        }
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToReadableText();
            var sb = new StringBuilder();
            sb.AppendLine(   ToReadableText()); 
            //反向 
            sb.AppendLine("-----------反向---------"); 
            sb.AppendLine(this.ReversedBaseline.ToReadableText());
            return sb.ToString();
        }

        /// <summary>
        /// 可读
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("基线：" + Name + ", 向量：" + VectorName);
            if (IsApproxXyzOfRovAvailable) sb.AppendLine("流动站近似坐标： " + ApproxXyzOfRov);
            sb.AppendLine("流动站估值坐标： " + EstimatedXyzOfRov);
            if (IsApproxXyzOfRovAvailable) sb.AppendLine("初始向量 XYZ： " + ApproxVector.ToString() + ", 长度： " + Geo.Utils.StringUtil.GetReadableDistance(ApproxVector.Length));

            sb.Append("算法：" + this.GnssSolverType);
            sb.AppendLine("， 解类型：" + this.ResultType);

            sb.Append("估值向量 XYZ： " + EstimatedVector.ToString() + ", 长度： " + Geo.Utils.StringUtil.GetReadableDistance(EstimatedVector.Length));
        
            if (IsApproxXyzOfRovAvailable)
            {
                var correcVector = CorrectionOfRov.Value;// (EstimatedVector - ApproxVector);
                sb.AppendLine("，向量改正数 XYZ： " + correcVector.ToString() + ", 长度： " + Geo.Utils.StringUtil.GetReadableDistance(correcVector.Length));
            } 

            sb.Append("估值向量 ENU： " + EstimatedVectorEnu + ", 长度： " + Geo.Utils.StringUtil.GetReadableDistance(EstimatedVectorEnu.Length) 
                + ", 水平距离：" + Geo.Utils.StringUtil.GetReadableDistance(EstimatedVectorEnu.LevelLength));
            if (IsApproxXyzOfRovAvailable)
            {
                var coEnu = CorrectionOfRovEnu;
                sb.AppendLine("，向量改正数 ENU： " + coEnu.ToString() + ", 长度： " + Geo.Utils.StringUtil.GetReadableDistance(coEnu.Length));
                var azimuth = GeodeticAzimuth;
                sb.Append("大地方位角： " + azimuth + " = " + new DMS(azimuth).ToReadableDms());
                var aveHeight = (GeoCoordOfRef.Height + this.EstimatedGeoCoordOfRov.Height) / 2.0;
                sb.AppendLine(", 大地线长（投影高程 " + Geo.Utils.StringUtil.GetReadableDistance(aveHeight) + "）： " + Geo.Utils.StringUtil.GetReadableDistance(GeodeticLength));

                azimuth = this.SphereAzimuth;
                sb.AppendLine("球面方位角： " + azimuth + " = " + new DMS(azimuth).ToReadableDms());

            }
            sb.AppendLine("闭合差：" + this.ClosureError);
            if (SiteInfoOfRef != null && SiteInfoOfRov != null)
            {
                sb.AppendLine("天线：" + SiteInfoOfRef.SiteName
                  + ", " + SiteInfoOfRef.Antenna
                 + "->" + SiteInfoOfRov.SiteName
                  + ", " + SiteInfoOfRov.Antenna
                  );
            }

            return sb.ToString();
        }
        /// <summary>
        /// 用于表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();

            var enu = EstimatedVectorEnu;
            var rms = this.EstimatedVectorRmsedXYZ.Rms;

            row[ParamNames.Name] = this.Name;
            row[ParamNames.ResultType] = this.ResultType;
            row[ParamNames.Dx] = this.EstimatedVector.X;
            row[ParamNames.Dy] = this.EstimatedVector.Y;
            row[ParamNames.Dz] = this.EstimatedVector.Z;
            row[ParamNames.De] = this.EstimatedVectorEnu.E;
            row[ParamNames.Dn] = this.EstimatedVectorEnu.N;
            row[ParamNames.Du] = this.EstimatedVectorEnu.U;
            row[ParamNames.Distance] = this.EstimatedVectorEnu.Length;
            row[ParamNames.GeodeticAzimuth] = this.GeodeticAzimuth;
            row[ParamNames.ClousureError] = this.ClosureError;
            row[ParamNames.RefX] = this.ApproxXyzOfRef.X;
            row[ParamNames.RefY] = this.ApproxXyzOfRef.Y;
            row[ParamNames.RefZ] = this.ApproxXyzOfRef.Z;
            row[ParamNames.RovX] = this.ApproxXyzOfRov.X;
            row[ParamNames.RovY] = this.ApproxXyzOfRov.Y;
            row[ParamNames.RovZ] = this.ApproxXyzOfRov.Z;
            row[ParamNames.RmsX] = this.EstimatedVectorRmsedXYZ.Rms.X;
            row[ParamNames.RmsY] = this.EstimatedVectorRmsedXYZ.Rms.Y;
            row[ParamNames.RmsZ] = this.EstimatedVectorRmsedXYZ.Rms.Z;
            row[ParamNames.StdDev] = this.StdDev;
            row[ParamNames.Qx] = this.CovaMatrix[0, 0];
            row[ParamNames.Qxy] = this.CovaMatrix[0, 1];
            row[ParamNames.Qxz] = this.CovaMatrix[0, 2];
            row[ParamNames.Qy] = this.CovaMatrix[1, 1];
            row[ParamNames.Qyz] = this.CovaMatrix[1, 2];
            row[ParamNames.Qz] = this.CovaMatrix[2, 2];
            row[ParamNames.GnssSolverType] = this.GnssSolverType;
            row[ParamNames.Epoch] = this.Epoch;

            return row;
        }

        /// <summary>
        /// 此将用于输出
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ParamNames.Name);
            sb.Append("\t");
            sb.Append(ParamNames.Dx);
            sb.Append("\t");
            sb.Append(ParamNames.Dy);
            sb.Append("\t");
            sb.Append(ParamNames.Dz);
            sb.Append("\t");
            sb.Append(ParamNames.De);
            sb.Append("\t");
            sb.Append(ParamNames.Dn);
            sb.Append("\t");
            sb.Append(ParamNames.Du);
            sb.Append("\t");
            sb.Append(ParamNames.ResultType);
            sb.Append("\t");
            sb.Append(ParamNames.Distance); 
            sb.Append("\t");
            sb.Append(ParamNames.GeodeticAzimuth); 
            sb.Append("\t");
            sb.Append(ParamNames.ClousureError); 
            sb.Append("\t");
            sb.Append(ParamNames.RefX);
            sb.Append("\t");
            sb.Append(ParamNames.RefY);
            sb.Append("\t");
            sb.Append(ParamNames.RefZ);
            sb.Append("\t");
            sb.Append(ParamNames.RovX);
            sb.Append("\t");
            sb.Append(ParamNames.RovY);
            sb.Append("\t");
            sb.Append(ParamNames.RovZ);
            sb.Append("\t");
            sb.Append(ParamNames.RmsX);
            sb.Append("\t");
            sb.Append(ParamNames.RmsY);
            sb.Append("\t");
            sb.Append(ParamNames.RmsZ);
            sb.Append("\t");
            sb.Append(ParamNames.StdDev);
            sb.Append("\t");
            sb.Append(ParamNames.Qx);//[0,0]//编号从小到大
            sb.Append("\t");
            sb.Append(ParamNames.Qxy);//[0,1]
            sb.Append("\t");
            sb.Append(ParamNames.Qxz);//[0,3]
            sb.Append("\t");
            sb.Append(ParamNames.Qy);//[1,1]
            sb.Append("\t");
            sb.Append(ParamNames.Qyz);//[1,2]
            sb.Append("\t");
            sb.Append(ParamNames.Qz);//[2,2]
            sb.Append("\t"); 
            sb.Append(ParamNames.GnssSolverType);//[2,2]
            sb.Append("\t"); 
            sb.Append(ParamNames.Epoch);
            return sb.ToString();
        } 
        /// <summary>
        /// 此将用于输出
        /// </summary>
        /// <returns></returns>
        public  string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Name);
            sb.Append("\t");
            sb.Append(this.EstimatedVector.X.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.EstimatedVector.Y.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.EstimatedVector.Z.ToString("0.00000"));
            var enu = EstimatedVectorEnu;
            sb.Append("\t");
            sb.Append(enu.E.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(enu.N.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(enu.U.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.ResultType.ToString());
            sb.Append("\t");
            sb.Append(enu.Length.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.GeodeticAzimuth.ToString("0.000000000"));
            sb.Append("\t");
            sb.Append(ClosureError.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.ApproxXyzOfRef.X.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.ApproxXyzOfRef.Y.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.ApproxXyzOfRef.Z.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.ApproxXyzOfRov.X.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.ApproxXyzOfRov.Y.ToString("0.00000"));
            sb.Append("\t");
            sb.Append(this.ApproxXyzOfRov.Z.ToString("0.00000"));
            
            var rms = this.EstimatedVectorRmsedXYZ.Rms;
            sb.Append("\t");
            sb.Append(rms.X.ToString("G5"));
            sb.Append("\t");
            sb.Append(rms.Y.ToString("G5"));
            sb.Append("\t");
            sb.Append(rms.Z.ToString("G5"));
            sb.Append("\t");
            sb.Append(this.StdDev.ToString("G5"));
            sb.Append("\t");
            sb.Append(CovaMatrix[0, 0].ToString("G5"));
            sb.Append("\t");
            sb.Append(CovaMatrix[0, 1].ToString("G5"));
            sb.Append("\t");
            sb.Append(CovaMatrix[0, 2].ToString("G5"));
            sb.Append("\t");
            sb.Append(CovaMatrix[1, 1].ToString("G5"));
            sb.Append("\t");
            sb.Append(CovaMatrix[1, 2].ToString("G5"));
            sb.Append("\t");
            sb.Append(CovaMatrix[2, 2].ToString("G5"));
            sb.Append("\t");
            sb.Append(GnssSolverType.ToString());
            sb.Append("\t");
            sb.Append(Epoch.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static EstimatedBaseline Parse(Dictionary<string, object> row)
        {
            var epoch = (Time)row[ParamNames.Epoch];
            Matrix matrix = new Matrix(new SymmetricMatrix(3, 0));
            var name = row[ParamNames.Name].ToString();
            double refx = Convert.ToDouble(row[ParamNames.RefX]);
            double refy = Convert.ToDouble(row[ParamNames.RefY]);
            double refz = Convert.ToDouble(row[ParamNames.RefZ]);
            double rovx = Convert.ToDouble(row[ParamNames.RovX]);
            double rovy = Convert.ToDouble(row[ParamNames.RovY]);
            double rovz = Convert.ToDouble(row[ParamNames.RovZ]);
            double x = Convert.ToDouble(row[ParamNames.Dx]);
            double y = Convert.ToDouble(row[ParamNames.Dy]);
            double z = Convert.ToDouble(row[ParamNames.Dz]);
            double rmsX = Convert.ToDouble(row[ParamNames.RmsX]);
            double rmsY = Convert.ToDouble(row[ParamNames.RmsY]);
            double rmsZ = Convert.ToDouble(row[ParamNames.RmsZ]);
            double stdDev = Convert.ToDouble(row[ParamNames.StdDev]);

            matrix[0, 0] = Convert.ToDouble(row[ParamNames.Qx]);
            matrix[0, 1] = Convert.ToDouble(row[ParamNames.Qxy]);
            matrix[0, 2] = Convert.ToDouble(row[ParamNames.Qxz]);
            matrix[1, 1] = Convert.ToDouble(row[ParamNames.Qy]);
            matrix[1, 2] = Convert.ToDouble(row[ParamNames.Qyz]);
            matrix[2, 2] = Convert.ToDouble(row[ParamNames.Qz]);

            var refXyz = new XYZ(refx, refy, refz);
            var rovXyz = new XYZ(rovx, rovy, rovz);
            RmsedXYZ rmsedXYZ = new RmsedXYZ(new XYZ(x, y, z), new XYZ(rmsX, rmsY, rmsZ));

            var resultType = ResultType.Unknown;
            if(row.ContainsKey(ParamNames.ResultType))
            {
                resultType = (ResultType)Enum.Parse(typeof(ResultType), row[ParamNames.ResultType].ToString());
            }
            var ClousureError = 1.0;
            if (row.ContainsKey(ParamNames.ClousureError))
            {
                ClousureError = Convert.ToDouble(row[ParamNames.ClousureError]);
            }
            GnssSolverType GnssSolverType = GnssSolverType.单历元单频双差;
            if (row.ContainsKey(ParamNames.GnssSolverType))
            {
                try//防止汉字不被支持
                {
                    GnssSolverType = (GnssSolverType)Enum.Parse(typeof(GnssSolverType), row[ParamNames.GnssSolverType].ToString());
                }catch(Exception ex) { }
            } 

            var obj = new EstimatedBaseline(name, epoch, refXyz, rovXyz, rmsedXYZ, matrix, stdDev)
            {
                ClosureError = ClousureError,
                ResultType = resultType,
                GnssSolverType = GnssSolverType
            };
            return obj;
        }

        /// <summary>
        /// 用于显示
        /// </summary>
        /// <returns></returns>
        public static ObjectTableStorage GetLineTable(List<EstimatedBaseline> EstimatedBaselines, string Name)
        {  
            ObjectTableStorage result = new ObjectTableStorage(Name);
            foreach (var item in EstimatedBaselines)
            {
                result.NewRow();
                result.AddItem(item.GetObjectRow());
            }
            return result; 
        }
    }
}