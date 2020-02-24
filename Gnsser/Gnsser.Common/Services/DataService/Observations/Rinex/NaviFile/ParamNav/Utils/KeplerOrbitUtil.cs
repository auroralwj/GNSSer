
//2016.05.31 , czs, ceate in hongqing, 简单6参数轨道星历


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Geo.Coordinates;

namespace Gnsser
{ 

    /// <summary>
    /// 开普勒6个轨道根数解算。
    /// 卫星无摄运动二体问题。
    /// </summary>
    public class KeplerOrbitUtil
    {
        /// <summary>
        /// 地球引力常数
        /// </summary>
        const double GM = GeoConst.GM;
        const double PI = GeoConst.PI;
        /// <summary>
        /// 通过两个时刻的位置，求轨道参数。
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        public static KeplerEphemerisParam GetKeplerEphemerisParamFromTwoPos(XYZ posA, XYZ posB, double deltaTime)
        {
            throw new NotImplementedException("通过两个时刻的位置，求轨道参数，算法未实现！2016.06.06.16");
            //2.6.1.1	计算轨道倾角i和升交点赤经Ω
            XYZ hXyz = posA.Cross(posB);
            double hLen = hXyz.Length;
            double r1 = posA.Length;
            double r2 = posB.Length;
            double vv = posB.Dot(posB);

            //计算轨道倾角
            double ABLen = Math.Sqrt(hXyz.X * hXyz.X + hXyz.Y * hXyz.Y);
            double i = Math.Atan2(ABLen, hXyz.Z);            
           // double i = Math.Acos(hXyz.Z / hLen); //表示在Z轴上的投影，与上结果计算一致
            //double includedAngle = hXyz.GetIncludedAngle(new XYZ(0, 0, 1));//轨道倾角为两个平面法向量的夹角

            //计算升交点赤经
            double Ω = Math.Atan2(hXyz.X, -hXyz.Y);

            //用面积比法求半通径
            double sinf2minusf1 = hXyz.Z  / ( r1 * r2 * Math.Cos(i) );
            return null;

            //KeplerEphemerisParam param = new KeplerEphemerisParam
            //{
            //    ArgumentOfPerigee = ww,
            //    Eccentricity = e,
            //    Inclination = i,
            //    LongOfAscension = Ω,
            //    MeanAnomaly = M,
            //    SqrtA = sqrtA
            //};
            //return param;
        }
        /// <summary>
        /// 计算开普勒根数
        /// </summary>
        /// <param name="MotionState"></param>
        /// <returns></returns>
        public static KeplerEphemerisParam GetKeplerEphemerisParam(MotionState MotionState) { return GetKeplerEphemerisParam(MotionState.Position, MotionState.Velocity); }
       /// <summary>
        /// 计算开普勒根数
       /// </summary>
       /// <param name="pos"></param>
       /// <param name="velocity"></param>
       /// <returns></returns>
        public static KeplerEphemerisParam GetKeplerEphemerisParam(XYZ pos, XYZ velocity)
        {
            //2.6.1.1	计算轨道倾角i和升交点赤经Ω
            XYZ hXyz = pos.Cross(velocity);
            double hLen = hXyz.Length;
            double r = pos.Length;
            double vv = velocity.Dot(velocity);
           
            //计算轨道倾角
            //double ABLen = Math.Sqrt(hXyz.X * hXyz.X + hXyz.Y * hXyz.Y);
            //double i = Math.Atan2(ABLen, hXyz.Z);            
            double i = Math.Acos(hXyz.Z / hLen); //表示在Z轴上的投影，与上结果计算一致
            //double includedAngle = hXyz.GetIncludedAngle(new XYZ(0, 0, 1));//轨道倾角为两个平面法向量的夹角

            //计算升交点赤经
            double Ω = Math.Atan2(hXyz.X, -hXyz.Y); 

            //2.6.1.2	计算轨道长半轴a，
            double a = 1 / (2 / r - vv / GM); //由活力公式计算
            double sqrtA = Math.Sqrt(a);
            double n = Math.Sqrt(GM) / (sqrtA * sqrtA * sqrtA);//平运动角速度n
            //离心率e和平近点交M
            double rdotv = pos.Dot(velocity);
            double eCosE = 1 - r / a;
            double eSinE = rdotv / Math.Sqrt(GM * a);
            double e = Math.Sqrt(eSinE * eSinE + eCosE * eCosE);//离心率e
            double E = Math.Atan2(eSinE, eCosE);//偏近点角E
            double M = E - eSinE; //平近点角

            //2.6.1.3	计算近升角距ω
            XYZ eXyz = velocity.Cross(hXyz) / GM - pos.UnitVector();
            double ww = Math.Atan2(eXyz.Z, (eXyz.Y * Math.Sin(Ω) + eXyz.X * Math.Cos(Ω)) * Math.Sin(i));

            double u = Math.Atan2(pos.Z,  (pos.Y * Math.Sin(Ω) + pos.X * Math.Cos(Ω)) * Math.Sin(i));
            double ff = u - ww;//真近点角

            KeplerEphemerisParam param = new KeplerEphemerisParam
            {
                ArgumentOfPerigee = ww,
                Eccentricity = e,
                Inclination = i,
                LongOfAscension = Ω,
                MeanAnomaly = M,
                SqrtA = sqrtA
            };
            return param;
        }

        /// <summary>
        /// 计算坐标
        /// </summary>
        /// <param name="timeFromReffer"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static MotionState GetSatPos(double timeFromReffer, KeplerEphemerisParam param)
        {
            return GetSatPos(
                timeFromReffer,
                param.Inclination,
                param.LongOfAscension,
                param.SqrtA * param.SqrtA, 
                param.Eccentricity, 
                param.ArgumentOfPerigee, 
                param.MeanAnomaly);
        }
        /// <summary>
        /// 6个轨道根数计算卫星在天球坐标系中的位置，角度以弧度为单位。
        /// 本函数仅用于课程学习计算。
        /// </summary>
        /// <param name="deltaTime">距离参考时刻的时间差，单位秒</param>
        /// <param name="eye">轨道平面倾角,弧度</param>
        /// <param name="Ω">升交点赤经,弧度</param>
        /// <param name="a">轨道椭圆长半径的平方根</param>
        /// <param name="e">轨道椭圆离心率</param>
        /// <param name="w">近升角距,弧度</param>
        /// <param name="M0">参考时刻τ卫星的平近点角,弧度</param>
        public static MotionState GetSatPos(
            double deltaTime,
            double eye,
            double Ω,
            double a,
            double e,
            double w,
            double M0
            )
        {             
            double SqrtA = Math.Sqrt(a);

            //计算平均角速度n 
            double sqrtA3 = a * SqrtA;
            double n =  Math.Sqrt(GM)/sqrtA3;
            //2）计算平近点角M和偏近点角E
            double M = M0 + n * deltaTime;

            //偏近点角E, E=M+eSinE
            double E = OrbitUtil.KeplerEqForEccAnomaly(M, e);

            //计算卫星向径的模r
            double r = a * (1 - e * Math.Cos(E));

            //计算卫星在轨道平面直角坐标系中的坐标（x’,y’）
            double x = a * (Math.Cos(E) - e);
            double y = a * Math.Sqrt(1 - e * e) * Math.Sin(E);
            //2.5.1.2	卫星在天球坐标系中的位置
            XYZ plainXyz = new XYZ(x, y, 0);
            var celestialXyz = plainXyz.RotateZ(-w).RotateX(-eye).RotateZ(-Ω);
        
            //计算速度
            double aaanr = (a * a * n) / r;
             double vx = aaanr * (-Math.Sin(E));
            double vy = aaanr *  Math.Sqrt(1-e*e) * Math.Cos(E);
            XYZ plainVxy = new XYZ(vx, vy, 0 );
             var celestialVXyz = plainVxy.RotateZ(-w).RotateX(-eye).RotateZ(-Ω);

             MotionState state = new MotionState(celestialXyz, celestialVXyz);
            return state;
        }
    }
}