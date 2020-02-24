using System;
using System.Text;
using Geo.Coordinates;

namespace Gnsser
{
    /// <summary>
    /// 6 轨道参数。
    /// </summary>
    public class OrbitParam
    {
        /// <summary>
        /// 轨道 6 根数默认构造函数。
        /// </summary>
        public OrbitParam() { }
        /// <summary>
        /// 轨道 6 根数构造函数。
        /// </summary>
        /// <param name="Inclination">斜率</param>
        /// <param name="LongOfAscension">升角点赤经</param>
        /// <param name="SemiMajor">长轴半径</param>
        /// <param name="Eccentricity">偏心率</param>
        /// <param name="PerigeeAngle">近升角距</param>
        /// <param name="EpochOfPerigee">近地点时刻</param> 
        public OrbitParam(
            double Inclination,
            double LongOfAscension,
            double SemiMajor,
            double Eccentricity,
            double PerigeeAngle,
            double EpochOfPerigee 
            )
        {
            this.Inclination = Inclination;
            this.LongOfAscension = LongOfAscension;
            this.SemiMajor = SemiMajor;
            this.Eccentricity = Eccentricity;
            this.ArgumentOfPerigee = PerigeeAngle;
            this.EpochOfPerigee = EpochOfPerigee; 
        }

        #region 确定椭圆大小形状
        /// <summary>
        /// 偏心率 e
        /// </summary>
        public double Eccentricity { get; set; }
        /// <summary>
        /// 轨道椭圆长半径 a
        /// </summary>
        public double SemiMajor { get; set; }
        #endregion

        #region  确定轨道平面位置
        /// <summary>
        /// 轨道倾角 time、eye
        /// </summary>
        public double Inclination { get; set; }
        /// <summary>
        ///  升角点赤经  big Omega 卫星由南到北经过赤道的交点的经度。
        /// </summary>
        public double LongOfAscension { get; set; }
        #endregion

        #region 轨道椭圆定向
        /// <summary>
        /// 近升角距 small omega ，近地点和升角点的角度,轨道椭圆定向
        /// </summary>
        public double ArgumentOfPerigee { get; set; }
        #endregion
        
        #region 卫星在椭圆上位置
        /// <summary>
        /// 卫星过近地点时刻 tao 卫星在椭圆上位置: 过近地点时刻τ
        /// Perigee: 1.近地点; 2.绕地运动的天体轨道上离地心最近点。
        /// </summary>
        public double EpochOfPerigee { get; set; }
        #endregion

        #region 其它参数
        /// <summary>
        /// 平近点角M，M是一个假设量，
        /// 如果卫星在轨道上运动的平均速度为n，则平近点角定义为：  M＝n（t-τ）
        /// </summary>
        public double MeanAnomaly { get; set; }
        /// <summary>
        /// 偏近点角 E
        /// </summary>
        public double EccentricAnomaly { get; set; }
        #endregion
        /// <summary>
        /// 开普勒方程计算。
        /// 采用简单迭代法求偏近点角 E
        /// </summary>
        public static double SimpleLoopAndSetEccentricAnomaly(double Eccentricity, double MeanAnomaly)
        {
            double e = Eccentricity;
            double E0 = MeanAnomaly;
            double E1 = MeanAnomaly + e * Math.Sin(E0);
            while (Math.Abs(E1 - E0) > 1e-14)
            {
                E0 = E1;
                E1 = MeanAnomaly + e * Math.Sin(E0);
            }
            return E1;
        }
        /// <summary>
        /// 开普勒方程计算。
        /// 微分迭代法,牛顿法，收敛更快。
        /// </summary>
        /// <param name="Eccentricity">偏心率e</param>
        /// <param name="MeanAnomaly">平</param>
        /// <returns></returns>
        public static double DifferentialLoopAndSetEccentricAnomaly(double Eccentricity, double MeanAnomaly)
        {
            double e = Eccentricity;
            double E0 = MeanAnomaly;//表示上一个 E
            double E1 = E0;
            double differ = Double.MaxValue;
            while (differ > 1.0e-14)
            {
                double dM = MeanAnomaly - E0 + e * Math.Sin(E0);

                E1 = E0 + dM / (1.0 - e * Math.Cos(E0));

                differ = Math.Abs(dM / (1.0 - e * Math.Cos(E1)));
                E0 = E1;//update
            } 
            return E1;
        }




        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Inclination:");
            sb.Append(Inclination);
            sb.Append(",");
            sb.Append("LongOfAscension:");
            sb.Append(LongOfAscension);
            sb.Append(",");
            sb.Append("SemiMajor:");
            sb.Append(SemiMajor);
            sb.Append(",");
            sb.Append("Eccentricity:");
            sb.Append(Eccentricity);
            sb.Append(",");
            sb.Append("PerigeeAngle:");
            sb.Append(ArgumentOfPerigee);
            sb.Append(",");
            sb.Append("EpochOfPerigee:");
            sb.Append(EpochOfPerigee);
            sb.Append(",");
            sb.Append("MeanAnonomaly:");
            sb.Append(MeanAnomaly);

            return sb.ToString();
        }

    }
}
