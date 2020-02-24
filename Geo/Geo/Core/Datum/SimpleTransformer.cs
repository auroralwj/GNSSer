//2015.04.15, czs, create in numu shuangliao, 增加注释


using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// 简单的坐标转换。
    /// 假设 坐标的单位一致，坐标轴平行（方向可能相反）。
    /// 需要转换的是平移，放缩以及坐标翻转。
    /// </summary>
    public class SimpleTransformer : ICoordTransformer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourseCoord"></param>
        /// <param name="targetCoord"></param>
        public SimpleTransformer(XYZ sourseCoord, XYZ targetCoord)
        {
            this.Scale = 1;
            this.DifferCoord =targetCoord - sourseCoord;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="differCoord"></param>
        public SimpleTransformer(double scale, XYZ differCoord)
        {
            this.Scale = scale;
            this.DifferCoord = differCoord;
        }


        /// <summary>
        /// 尺度因子
        /// </summary>
        public double Scale { get; set; }
        /// <summary>
        /// 坐标差
        /// </summary>
        public XYZ DifferCoord { get; set; }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public XYZ Trans(XYZ old)
        {
            double x = (Scale) * old.X + DifferCoord.X;
            double y = (Scale) * old.Y + DifferCoord.Y;
            double z = (Scale) * old.Z + DifferCoord.Z;
            return new XYZ(x, y, z);
        }




    }
}
