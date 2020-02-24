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
    /// 采用多普勒频速度求卫星发射时刻。
    /// 由于当前大多数数据已经取消了多普勒频率观测值，而只能通过载波频率进行差分计算，
    /// 但是卫星第一次出现的时候由于缺乏数据，是不能差分的，因此不可以计算卫星星历，此时返回空!!!!
    /// </summary>
    public class EmissionEphemerisRolverWithDopplorSpeed : EmissionEphemerisRolver
    {
        /// <summary>
        /// 构造函数。设置卫星星历，请在本类前执行 观测值的有效性检查与过滤。
        /// </summary>
        public EmissionEphemerisRolverWithDopplorSpeed(IEphemerisService EphemerisService, DataSourceContext DataSouceProvider, EpochSatellite sat)
            : base(EphemerisService, DataSouceProvider, sat)
        {
            this.Name = "多普勒频率法";
        }

        public override IEphemeris Get()
        {
            //实质为计算发射时刻的系统时间, 只比较秒数 
            SatelliteNumber prn = EpochSat.Prn;
            Ephemeris eph = null;
            XYZ receiverPos = EpochSat.SiteInfo.EstimatedXyz;
            Time receivingTime = EpochSat.RecevingTime;

            //2.需要接收机钟差 
            if (EpochSat.Time.Correction != 0 && EpochSat.FrequenceA.DopplerShift != null)
            {
                //2.2 多普勒频移法
                eph = EphemerisService.Get(prn, receivingTime);
                double distance = (eph.XYZ - receiverPos).Length;
                double deltaT = distance / GnssConst.LIGHT_SPEED;

                //这里的距离差是接收时刻与发射时刻的位移差，所以应该减去。
                distance -= EpochSat.AverageDopplorSpeed * deltaT;
                deltaT = distance / GnssConst.LIGHT_SPEED;
                Time transmitTime = receivingTime - deltaT;
                eph = EphemerisService.Get(prn, transmitTime);
            }
           return  eph;
        }
    }
}
