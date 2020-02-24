//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法
//2015.02.08, 崔阳, 卫星钟差和精密星历若同时存在，则不可分割


using System;
using System.IO;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times;
using Gnsser.Correction;

namespace Gnsser
{
     
    /// <summary>
    /// 地球自转改正，改正卫星的位置和速度。
    /// </summary>
    public class EarthSagnacEphemerisReviser : EphemerisReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="receiverPos"></param>
        public EarthSagnacEphemerisReviser(XYZ receiverPos)
        {
            this.Name = "地球自转改正";
            this.ReceiverPos = receiverPos;
        }
        /// <summary>
        /// 接收机位置信息。
        /// </summary>
        public XYZ ReceiverPos { get; set; }

        public override bool Revise(ref IEphemeris eph)
        {
            //eph.XYZ = EphemerisUtil.CorrectEarthSagnac(eph.XYZ, ReceiverPos);
            //eph.XyzDot = EphemerisUtil.CorrectEarthSagnac(eph.XyzDot, ReceiverPos);


            // 卫星坐标的地球自转改正

            double len = (eph.XYZ - ReceiverPos).Length;
            double elapsedTime = len / GnssConst.LIGHT_SPEED;


            double sag = GnssConst.EARTH_ROTATE_SPEED * elapsedTime;
            double sx = Math.Cos(sag) * eph.XYZ.X + Math.Sin(sag) * eph.XYZ.Y;
            double sy = -Math.Sin(sag) * eph.XYZ.X + Math.Cos(sag) * eph.XYZ.Y;

            eph.XYZ.X = sx;
            eph.XYZ.Y = sy;


            sx = Math.Cos(sag) * eph.XyzDot.X + Math.Sin(sag) * eph.XyzDot.Y;
            sy = -Math.Sin(sag) * eph.XyzDot.X + Math.Cos(sag) * eph.XyzDot.Y;

            eph.XyzDot.X = sx;
            eph.XyzDot.Y = sy;



            return true;
        }
    }









}
