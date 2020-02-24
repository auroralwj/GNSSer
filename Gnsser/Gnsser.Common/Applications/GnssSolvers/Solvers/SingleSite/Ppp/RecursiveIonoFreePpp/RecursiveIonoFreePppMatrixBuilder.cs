//2018.04.18, czs, create in hmx, 递归最小二乘法  
//2018.10.17, czs, edit in hmx, 发现系数阵未赋值，改正后得到了正常的结果

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common; 
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Gnsser.Models;
using Geo.Times; 
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 精密单点定位矩阵生成类。
    /// </summary>
    public class RecursiveIonoFreePppMatrixBuilder : BasePppPositionMatrixBuilder
    {
        /// <summary>
        /// 先构造，再设置历元值。
        /// </summary>
        /// <param name="option">解算选项</param> 
        public RecursiveIonoFreePppMatrixBuilder(
            GnssProcessOption option)
            : base(option)
        {
            this.ParamNameBuilder = new RecursiveIonoFreePppParamNameBuilder(option);
            this.SecondParamNameBuilder = new RecursiveIonoFreePppSecondParamNameBuilder(option);
        }

        #region 全局基础属性 

        /// <summary>
        /// 固定参数数量
        /// </summary>
        public override int SecondParamCount { get { return EnabledSatCount; } }

        #endregion
                 
        /// <summary>
        /// 参数平差系数阵 A n*m, m为可变参数个数，此处为 5 个，xyz, dt, zpd。
        /// 前一半行数为伪距观测量，后一半为载波观测量。
        /// </summary> 
        /// <returns></returns>
        public override Matrix Coefficient
        {
            get
            {
                int satCount = EnabledSatCount;
                int rowCount = satCount * 2;
                int colCount = 5;

                double[][] A = Geo.Utils.MatrixUtil.Create(rowCount, colCount);
                int rangeRow = 0;//卫星编号
                int satIndex = 0;

                foreach (var sat in CurrentMaterial.EnabledSats)// 一颗卫星2行
                {
                    XYZ vector = sat.EstmatedVector;

                    double wetMap = sat.WetMap;
                    double wetMap0 = sat.Vmf1WetMap;

                    A[rangeRow][0] = -vector.CosX;
                    A[rangeRow][1] = -vector.CosY;
                    A[rangeRow][2] = -vector.CosZ;
                    A[rangeRow][3] = 1.0;            //接收机钟差对应的距离 = clkError * 光速
                    A[rangeRow][4] = wetMap;// DryWetM[1];   //A[row][3] = 299792458.0;//光速


                    int phaseRow = rangeRow + satCount;

                    A[phaseRow][0] = A[rangeRow][0];
                    A[phaseRow][1] = A[rangeRow][1];
                    A[phaseRow][2] = A[rangeRow][2];
                    A[phaseRow][3] = A[rangeRow][3];
                    A[phaseRow][4] = A[rangeRow][4];


                    rangeRow++;
                    satIndex++;
                }
                return new Matrix(A);
            }
        }
        
        /// <summary>
        /// 固定参数（如模糊度）平差系数阵 B 。模糊度参数。
        /// 随着卫星数量的改变，参数将改变。
        /// </summary> 
        /// <returns></returns>
        public override Matrix SecondCoefficient
        {
            get
            {
                int satCount = EnabledSatCount;
                int rowCount = satCount * 2;
                int colCount = satCount;

                double[][] B = Geo.Utils.MatrixUtil.Create(rowCount, colCount);
                int row = 0;//卫星编号               
                foreach (var prn in EnabledPrns)// 一颗卫星2行
                {
                    int colIndex = row;
                    int next = row + satCount;
                    B[next][colIndex] = 1;

                    row++;
                }
                return new Matrix(B);
            }
        }
        
    }//end class
}