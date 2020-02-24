//2016.06.04, czs,create in hongqing, 天球坐标系中椭圆轨道

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;

namespace Geo
{
    /// <summary>
    /// 天球坐标系中椭圆轨道。
    /// </summary>
    public class CelestialMomentEllipseOrbit
    {
        /// <summary>
        /// 通过运动状态初始化。
        /// </summary>
        /// <param name="MotionState">空间直角坐标系中的坐标和速度向量</param>
        public CelestialMomentEllipseOrbit(MotionState MotionState)
        {
            #region 由运动状态计算轨道6根数
            var pos = MotionState.Position;
            var velocity = MotionState.Velocity;
             //中间变量准备
            XYZ hXyz = pos.Cross(velocity);
            double hLen = hXyz.Length;
            double r = pos.Length;
            double vv = velocity.Dot(velocity);

            //计算轨道倾角             
            double i = Math.Acos(hXyz.Z / hLen); //表示在Z轴上的投影，与上结果计算一致

            //计算升交点赤经
            double Ω = Math.Atan2(hXyz.X, -hXyz.Y);

            //计算轨道长半轴a，
            double a = 1 / (2 / r - vv / GeoConst.GM); //由活力公式计算
            double sqrtA = Math.Sqrt(a);
            double n = Math.Sqrt(GeoConst.GM) / (sqrtA * sqrtA * sqrtA);//平运动角速度n

            //离心率e,偏近点角E ,平近点交M
            double rdotv = pos.Dot(velocity);
            double eCosE = 1 - r / a;
            double eSinE = rdotv / Math.Sqrt(GeoConst.GM * a);
            double e = Math.Sqrt(eSinE * eSinE + eCosE * eCosE);//离心率e
            double E = Math.Atan2(eSinE, eCosE);//偏近点角E 
            double M = E - eSinE; //平近点角
            
            //计算近升角距ω
            XYZ eXyz = velocity.Cross(hXyz) / GeoConst.GM - pos.UnitVector();
            double ω = Math.Atan2(eXyz.Z, (eXyz.Y * Math.Sin(Ω) + eXyz.X * Math.Cos(Ω)) * Math.Sin(i));

            double u = Math.Atan2(pos.Z, (pos.Y * Math.Sin(Ω) + pos.X * Math.Cos(Ω)) * Math.Sin(i));
            double ff = u - ω;//真近点角
            #endregion

            var momentEllipseOrbit = new MomentEllipseOrbit(M, new PlaneEllipse(a, e));
            Init(ω, i, Ω, momentEllipseOrbit);
        }
        /// <summary>
        /// 通过参数初始化
        /// </summary>
        /// <param name="argumentOfPerigee">近地点弧角ω</param>
        /// <param name="inclination">轨道倾角</param>
        /// <param name="rightAscensionOfAscendingNode">升交点赤经</param>
        /// <param name="MomentEllipseOrbit">时刻平面轨道</param>
        public CelestialMomentEllipseOrbit(double argumentOfPerigee, double inclination, double rightAscensionOfAscendingNode, MomentEllipseOrbit MomentEllipseOrbit)
        {
            Init(argumentOfPerigee, inclination, rightAscensionOfAscendingNode, MomentEllipseOrbit);
        }
        /// <summary>
        /// 通过参数初始化
        /// </summary>
        /// <param name="argumentOfPerigee">近地点弧角ω</param>
        /// <param name="inclination">轨道倾角</param>
        /// <param name="rightAscensionOfAscendingNode">升交点赤经</param>
        /// <param name="MomentEllipseOrbit">时刻平面轨道</param>
        private void Init(double argumentOfPerigee, double inclination, double rightAscensionOfAscendingNode, MomentEllipseOrbit MomentEllipseOrbit)
        {
            this.ArgumentOfPerigee = argumentOfPerigee;
            this.RightAscensionOfAscendingNode = rightAscensionOfAscendingNode;
            this.Inclination = inclination;
            this.MomentEllipseOrbit = MomentEllipseOrbit;
        }
        /// <summary>
        /// 当前时刻，卫星运动状态。
        /// </summary>
        public MotionState MotionState { get { return MomentEllipseOrbit.GetCelestialMotionState(ArgumentOfPerigee, Inclination, RightAscensionOfAscendingNode); } }
        /// <summary>
        /// 计算其它时刻的卫星运动状态。
        /// </summary>
        /// <param name="deltaTime">与标准时间差</param>
        /// <returns></returns>
        public MotionState GetMotionState(double deltaTime)
        {
            return new MomentEllipseOrbit(MomentEllipseOrbit.MeanAnomaly, deltaTime, MomentEllipseOrbit.PlaneEllipse).GetCelestialMotionState(ArgumentOfPerigee, Inclination, RightAscensionOfAscendingNode);
        }

        #region 核心属性
        /// <summary>
        /// 近升角距 ω
        /// </summary>
        public double ArgumentOfPerigee { get; set; }
        /// <summary>
        /// 轨道倾角 i
        /// </summary>
        public double Inclination { get; set; }
        /// <summary>
        /// 升交点赤经 Ω
        /// </summary>
        public double RightAscensionOfAscendingNode { get; set; }

        /// <summary>
        /// 轨道椭圆
        /// </summary>
        public MomentEllipseOrbit MomentEllipseOrbit { get; set; }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Ω:" + RightAscensionOfAscendingNode);
            sb.Append(", ");
            sb.Append("ω:" + ArgumentOfPerigee);
            sb.Append(", ");
            sb.Append("i:" + Inclination);
            sb.Append(", ");
            sb.Append(MomentEllipseOrbit);
            return sb.ToString();
        }
    }
}