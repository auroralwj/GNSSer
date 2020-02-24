//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法

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
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;


namespace Gnsser
{

    /// <summary>
    /// 采用坐标迭代，计算卫星发射时间。
    /// </summary>
    public class EmissionEphemerisRolverWithCoord : EmissionEphemerisRolver
    {
        /// <summary>
        /// 构造函数。设置卫星星历，请在本类前执行 观测值的有效性检查与过滤。
        /// </summary>
        public EmissionEphemerisRolverWithCoord(IEphemerisService EphemerisService, DataSourceContext DataSouceProvider, EpochSatellite sat)
            : base(EphemerisService, DataSouceProvider, sat)
        {
            this.Name = "坐标迭代法";
        }

        public override IEphemeris Get(){
            SatelliteNumber prn = EpochSat.Prn;
            Ephemeris eph = null;
            XYZ receiverPos = EpochSat.SiteInfo.EstimatedXyz;
            Time receivingTime = EpochSat.RecevingTime;
            //2.1 坐标迭代法求解
            double transitTime = 0.075;//传输时间初始值，GEO卫星应该更长，但此处并不影响。
            Time transmitTime = receivingTime - transitTime;
            eph = EphemerisService.Get(prn, transmitTime);
            if (eph == null)
            {
                throw new NullReferenceException("没有找到 " + prn + " " + transmitTime + " 的星历");
            }


            double differ = Double.MaxValue;//亚纳秒级别则退出
            double DELTA = 1e-10;
            double tmp = transitTime;

            //  int count = 0;//测试，统计计算了多少次
            for (int i = 0; differ > DELTA && i < 5; i++) //一般循环一次，可满足精度
            {
                //由于第一次的时间求得的卫星坐标可能会很差（几十米即可带来较大时间误差），因此继续利用通过观测值求得的时间
                tmp = transitTime;
                transitTime = (eph.XYZ - receiverPos).Length / GnssConst.LIGHT_SPEED;
                transmitTime = receivingTime - transitTime;

                differ = Math.Abs(transitTime - tmp);

                eph = EphemerisService.Get(prn, transmitTime);
                //  count++;
            }

            //  System.IO.File.AppendAllText(@"D:\count.txt", count + "\t" + differ.ToString("E") + "\r\n");

           return eph;
        }
    }

}
