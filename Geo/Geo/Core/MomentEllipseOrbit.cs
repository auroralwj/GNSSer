//2016.06.04, czs,create in hongqing, 椭圆轨道

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;

namespace Geo
{
   
    /// <summary>
    /// 椭圆轨道.某一时刻平面坐标系中的轨道椭圆，
    /// 可以通过此旋转得到天球和地球坐标系。
    /// </summary>
    public class MomentEllipseOrbit
    {
        /// <summary>
        /// 默认构造函数。当前时刻的运动状态。
        /// </summary>
        /// <param name="MeanAnomaly">参考时刻τ卫星的平近点角,弧度</param>
        /// <param name="deltaTime">与参考时刻之差</param>
        /// <param name="PlaneEllipse">椭圆轨道</param>
        public MomentEllipseOrbit(double MeanAnomaly, double deltaTime, PlaneEllipse PlaneEllipse)
            : this(MeanAnomaly + PlaneEllipse.MeanAngularVelocity * deltaTime, PlaneEllipse)
        {
        }

        /// <summary>
        /// 默认构造函数。当前时刻的运动状态。
        /// </summary>
        /// <param name="MeanAnomaly">本时刻卫星的平近点角,弧度</param>
        /// <param name="PlaneEllipse">椭圆轨道</param>
        public MomentEllipseOrbit(double MeanAnomaly, PlaneEllipse PlaneEllipse)
        {
            this.PlaneEllipse = PlaneEllipse;
            this.MeanAnomaly = MeanAnomaly;
        }

        #region 核心属性
        /// <summary>
        /// 平近点角 M
        /// </summary>
        public double MeanAnomaly { get; set; }
        #endregion

        /// <summary>
        /// 偏近点角.偏近点角是过椭圆上的任意一点，垂直于椭圆半长轴，交半长轴外接圆的点到原点的直线与半长轴所成夹角。
        /// 在椭圆的参数方程x=acosθ ， y=bsinθ中，参数角θ即为偏近点角。
        /// 在天体力学中，偏近点角可用来描述极径
        /// </summary>
        public double EccentricAnomaly { get { return KeplerEqForEccAnomaly(MeanAnomaly, PlaneEllipse.e); } }
        /// <summary>
        /// 真近点角
        /// </summary>
        public double TrueAnomaly { get { return Math.Atan(Math.Sqrt((1 + PlaneEllipse.e) / (1 - PlaneEllipse.e)) * Math.Tan(EccentricAnomaly / 2)) * 2; } }
        /// <summary>
        /// 位置向径
        /// </summary>
        public double VectorLength { get { return PlaneEllipse.a * (1 - PlaneEllipse.e * Math.Cos(EccentricAnomaly)); } }
        
        /// <summary>
        /// 极坐标,高度角为0。原点在焦点，起始轴指向近地点，逆时针为正，单位弧度。
        /// </summary>
        public Polar Polar { get { return new Polar(VectorLength, TrueAnomaly, AngleUnit.Radian); } }

        /// <summary>
        /// 卫星在平面直角坐标系中的运动状态。
        /// 空间直角坐标，Z轴为0,.原点在焦点，X轴指向近地点，Y轴指向卫星运动方向，与X垂直，右手坐标系。
        /// </summary>
        public MotionState PlaneMotionState
        {
            get
            {
                double E = EccentricAnomaly;
                double r = VectorLength;
                double a = PlaneEllipse.a;
                double x = a * (Math.Cos(E) - PlaneEllipse.e);
                double y = a * Math.Sqrt(1 - PlaneEllipse.ee) * Math.Sin(E);
                var pos = new XYZ(x, y);

                //计算速度 
                double n = PlaneEllipse.n; 
                double aaanr = (a * a * n) / r;
                double vx = aaanr * (-Math.Sin(E));
                double vy = aaanr * Math.Sqrt(1 - PlaneEllipse.ee) * Math.Cos(E);
                var velocity = new XYZ(vx, vy, 0);

                return new MotionState(pos, velocity);
            }
        }

        /// <summary>
        /// 在天球坐标系中的位置
        /// </summary>
        /// <param name="argumentOfPerigee">近升角距</param>
        /// <param name="inclination">轨道倾角</param>
        /// <param name="rightAscensionOfAscendingNode">升交点赤经</param>
        /// <returns></returns>
        public MotionState GetCelestialMotionState(double argumentOfPerigee, double inclination, double rightAscensionOfAscendingNode)
        {
            return PlaneMotionState.RotateZ(-argumentOfPerigee).RotateX(-inclination).RotateZ(-rightAscensionOfAscendingNode);
        }
        /// <summary>
        /// 平面椭圆
        /// </summary>
        public PlaneEllipse PlaneEllipse { get; set; }
        /// <summary>
        /// 开普勒方程 由平近点角 M 和 离心率 e 计算 偏近点角E
        /// solve for eccentric anomaly given mean anomaly and orbital eccentricity
        /// use simple fixed point iteration of kepler's equation
        /// </summary>
        /// <param name="em">rad</param>
        /// <returns></returns>
        public static double KeplerEqForEccAnomaly(double em, double e)
        {
            double ecca, ecca0;           //*** iterates of eccentric anomaly
            //*** initialize eccentric anomaly
            ecca = em + e * Math.Sin(em);

            //*** exit only on convergence
            int counter = 0;
            do
            {
                ecca0 = ecca;
                ecca = em + e * Math.Sin(ecca0);
                counter++;
            } while (Math.Abs((ecca - ecca0) / ecca) > 1.0e-14 && counter < 20);
            return ecca;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("M:" + MeanAnomaly);
            sb.Append(", "); 
            sb.Append(this.PlaneEllipse);
            return sb.ToString();
        }
    }

}