//2014.09.12, czs, create, 卫星定权。
//2015.05.07, cy, 发现。待修改，Range 距离始终为1，错误

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 按照卫星高度和离地距离进行定权。
    /// </summary>
    public class SatElevateAndRangeWeightProvider : ISatWeightProvider
    {
        /// <summary>
        /// 按照卫星高度和离地距离进行定权，构造函数。
        /// </summary>
        public SatElevateAndRangeWeightProvider()
        {

        }

        /// <summary>
        /// 获取权值
        /// </summary>
        /// <param name="satellite">测站卫星向量对象</param>
        /// <returns></returns>
        public double GetWeight(EpochSatellite satellite)
        {
            //避免低高度角卫星的权太小，czs,2018.09.27, hmx
            double sinE =0.5 +0.5 * Math.Sin(satellite.Polar.Elevation * AngularConvert.DegToRadMultiplier);
            //距离始终为1，错误：2015.05.07，cy，发现。待修改,先替换为 satellite.ApproxXyzVector.Length
            //2015.05.10,czs, 已经找到原因，是因为后续验证错误，而源代码无误，已修改
            double range = satellite.EstmatedVector.Length;// satellite.Polar.Range;
            if (range == 0) {
                range = 1; 
            }
            GnssSystem sys =  GnssSystem.GetGnssSystem(satellite.Prn.SatelliteType);
           double factor = range / sys.ApproxRadius;

           double Weight =Math.Pow( sinE / factor, 2.0);          
            
            //防止测站坐标为 0 的情况
           if (Geo.Utils.DoubleUtil.IsValid(Weight)) { return Weight; }

           return 1; 
        } 

        /// <summary>
        /// 权逆，方差。
        /// </summary>
        /// <param name="satellite">测站卫星向量</param>
        /// <returns></returns>
        public double GetInverseWeightOfRange(EpochSatellite satellite)
        {
            double standardCova = 1;//标准差//伪距以10米为基准
            double invWeight = standardCova * 1.0 / GetWeight(satellite);
            return invWeight;// *100000;
        }

        /// <summary>
        /// 标准差估值
        /// </summary>
        /// <param name="satellite"></param>
        /// <returns></returns>
        public double GetStdDev(EpochSatellite satellite) { return Math.Sqrt(GetInverseWeightOfRange(satellite)); }
    }
}