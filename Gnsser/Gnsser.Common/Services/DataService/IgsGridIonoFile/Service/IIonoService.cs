 //2017.08.17, czs, create in hongqing, 电离层文件的读取与服务
 //2018.05.13, czs, edit in hmx, 修复倾斜电离层延迟计算算法


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;//
using Gnsser.Times;
using Gnsser.Service;
using Geo.Times;
using Geo;
using Geo.IO;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{
    /// <summary>
    /// 电离层文件服务接口
    /// </summary>
    public interface IIonoService : ITimedService<BufferedTimePeriod>
    {
        /// <summary>
        /// 模型高度
        /// </summary>
        double HeightOfModel { get; }
        /// <summary>
        /// 获取倾斜延迟距离
        /// </summary>
        /// <param name="time">历元</param>
        /// <param name="siteXyz">测站坐标</param>
        /// <param name="satXyz">卫星坐标</param>
        /// <param name="freq">频率</param>
        /// <returns></returns>
        double GetSlopeDelayRange(Time time, XYZ siteXyz, XYZ satXyz, double freq);

        /// <summary>
        /// 获取倾斜电离层电子数
        /// </summary>
        /// <param name="time">历元</param>
        /// <param name="siteXyz">测站坐标</param>
        /// <param name="satXyz">卫星坐标</param>
        /// <returns></returns>
        RmsedNumeral GetSlope(Time time, XYZ siteXyz, XYZ satXyz);

        /// <summary>
        /// 获取垂直方向电子数量
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geocentricLonlatDeg"></param>
        /// <returns></returns>
        RmsedNumeral Get(Time time, LonLat geocentricLonlatDeg);
        /// <summary>
        /// 计算倾斜方向的延迟电子数量
        /// </summary>
        /// <param name="receiverTime"></param>
        /// <param name="geocentricLonLat"></param>
        /// <param name="elevation"></param>
        /// <returns></returns>
        RmsedNumeral GetSlope(Time receiverTime, LonLat geocentricLonLat, double elevation);
    }

    /// <summary>
    /// 电离层文件服务接口
    /// </summary>
    public interface IGridIonoFileService : IIonoService
    {
        /// <summary>
        /// 获取当天测站DCB
        /// </summary>
        /// <param name="time"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        RmsedNumeral GetDcb(Time time, string name);

        /// <summary>
        /// 获取当天卫星DCB
        /// </summary>
        /// <param name="time"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        RmsedNumeral GetDcb(Time time, SatelliteNumber prn);
    }
}