// 2014.12.06, czs, create in jinxinliaomao shuangliao, 运动状态方程卡尔曼滤波

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
using Geo.Algorithm;

namespace Gnsser.Service
{
    /// <summary>
    /// 运动状态方程卡尔曼滤波
    /// ??状态体现在预测值？？？
    /// </summary>
    public class MotionMatrixBuilder : BaseAdjustMatrixBuilder
    {
        public MotionMatrixBuilder(double interval)
        {
            this.interval = interval;
        }

        //int satCount;
        //int obsCount;

        /// <summary>
        /// 时间间隔，单位：秒
        /// </summary>
        public double interval { get; set; }

        public override WeightedVector AprioriParam
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 状态方程噪声向量Ω(k-1)的系数阵
        /// </summary>
        public override Matrix Coefficient
        {
            get
            {
                IMatrix matrix = ArrayMatrix.Diagonal( 6, 6, 1);
                matrix[0, 3] = interval;
                matrix[0, 4] = interval;
                matrix[0, 5] = interval; 

                return new Matrix( matrix);
            }
        }

        public override WeightedVector Observation
        {
            get { throw new NotImplementedException(); }
        }

        //public override IMatrix Transfer
        //{
        //    get
        //    { 
        //        IMatrix matrix = ArrayMatrix.Diagonal(6, 6, 1);
        //        matrix[0, 3] = interval;
        //        matrix[0, 4] = interval;
        //        matrix[0, 5] = interval;

        //        return matrix;
        //    }
        //} 

        public override List<string> ParamNames
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsParamsChanged
        {
            get { throw new NotImplementedException(); }
        }
    }
}
