//2014.10.02， czs, create, 海鲁吐嘎查 通辽, 创建矩阵接口，为后继实现自我的、快速的、大规模的矩阵计算做准备

using System;

namespace Geo.Algorithm
{
    /// <summary>
    /// Geo 矩阵工厂接口。主要用于在指定对象内部创建矩阵。
    /// </summary>
    public interface IMatrixFactory
    { 
        /// <summary>
        /// 创建一个新的默认的矩阵。
        /// </summary>
        /// <returns></returns>
        IMatrix Create(int rowCount, int colCount);
        /// <summary>
        /// 以二维数组实例化一个矩阵
        /// </summary>
        /// <param name="array">二维数组</param>
        /// <returns></returns>
        IMatrix Create(double[][] array); 
    }



}
