//2014.11.07, czs, create in numu, 二维盒子，由坐标组成的盒子。

using System;
using System.Collections.Generic;

namespace Geo.Coordinates
{
    /// <summary>
    ///  二维盒子，由坐标组成的盒子。
    ///  一个方方正正的盒子。
    /// </summary>
    /// <typeparam name="TCoord">坐标类型</typeparam>
    public interface IBox<TCoord> where TCoord : INumeralIndexing , ICloneable, new ()
    {
        /// <summary>
        /// 盒子中心坐标
        /// </summary>
        TCoord Center { get; } 
        /// <summary>
        /// 是否包含另一个盒子
        /// </summary>
        /// <param name="box">另一个盒子</param>
        /// <returns></returns>
        bool Contains(IBox<TCoord> box);
        /// <summary>
        /// 是否包含一条线段
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        bool Contains(LineSegment<TCoord> box);
        /// <summary>
        /// 是否好汉指定坐标
        /// </summary>
        /// <param name="xy">坐标</param>
        /// <returns></returns>
        bool Contains(TCoord xy); 
        /// <summary>
        /// 求交集
        /// </summary>
        /// <param name="other">另一个盒子</param>
        /// <returns></returns>
        IBox<TCoord> And(IBox<TCoord> other);
        /// <summary>
        /// 求与另一个盒子的并集。
        /// </summary>
        /// <param name="bbox">另一个盒子</param>
        /// <returns></returns>
        IBox<TCoord> Expands(IBox<TCoord> bbox);
        /// <summary>
        /// 是否与另一个盒子相交
        /// </summary>
        /// <param name="box">另一个盒子</param>
        /// <returns></returns>
        bool IntersectsWith(IBox<TCoord> box);
        /// <summary>
        /// 按照倍数扩展
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        IBox<TCoord> Expands(double times); 
        /// <summary>
        /// 盒子的高度
        /// </summary>
        double Height { get; }
        /// <summary>
        /// 盒子宽度
        /// </summary>
        double Width { get; }
        /// <summary>
        /// 是否为空
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// 左下角
        /// </summary>
        TCoord LeftBottom { get; }
        /// <summary>
        /// 左上角
        /// </summary>
        TCoord LeftTop { get; }
        /// <summary>
        /// 右下角
        /// </summary>
        TCoord RightBottom { get; }
        /// <summary>
        /// 右上角
        /// </summary>
        TCoord RightTop { get; } 
        /// <summary>
        /// 最大的水平数值
        /// </summary>
        double MaxHorizontal { get; set; }
        /// <summary>
        /// 最小的水平数值
        /// </summary>
        double MinHorizontal { get; set; }
        /// <summary>
        /// 最大的竖直数值
        /// </summary>
        double MaxVertical { get; set; }
        /// <summary>
        /// 最小的竖直数值
        /// </summary>
        double MinVertical { get; set; }
        /// <summary>
        /// 最上面的线段
        /// </summary>
        TwoDLineSegment<TCoord> Top { get; }
        /// <summary>
        /// 最下边的线段
        /// </summary>
        TwoDLineSegment<TCoord> Bottom { get; }
        /// <summary>
        /// 最左边的线段
        /// </summary>
        TwoDLineSegment<TCoord> Left { get; }
        /// <summary>
        /// 最右边的线段
        /// </summary>
        TwoDLineSegment<TCoord> Right { get; }
        /// <summary>
        /// 与指定线段是否相交。
        /// </summary>
        /// <param name="line">线段</param>
        /// <returns></returns>
        TCoord GetIntersectPoint(TwoDLineSegment<TCoord> line);

        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="trans">坐标转换接口</param>
        /// <returns></returns>
        bool Transform(ICoordTransformer trans);
        /// <summary>
        /// 获取线段与盒子的切点，如果相离则没有切点。
        /// </summary>
        /// <param name="line">线段</param>
        /// <returns></returns>
        List<TCoord> GetIntersectPoints(TwoDLineSegment<TCoord> line);
    }
}
