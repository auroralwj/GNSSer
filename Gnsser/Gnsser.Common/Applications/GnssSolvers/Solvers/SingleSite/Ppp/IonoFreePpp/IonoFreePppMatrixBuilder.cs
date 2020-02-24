// 2014.09.03, czs, 独立于Ppp.cs
// 2014.09.05, czs, edit, 实现 IAdjustMatrixBuilder 接口
//2016.10.10, czs, edit, 保持模糊度以米单位，多系统下具有可比性，
//2018.06.03, lly & czs, edit in zz & hmx, 修改伽利略频率，按照RINEX的顺序 1 5 7 8 6
//2018.09.26, czs, edit in hmx, 多系统重新设计

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
    public class IonoFreePppMatrixBuilder : BasePppPositionMatrixBuilder
    {
        /// <summary>
        /// 先构造，再设置历元值。
        /// </summary>
        /// <param name="option">解算选项</param> 
        public IonoFreePppMatrixBuilder(
            GnssProcessOption option)
            : base(option)
        {
            ParamNameBuilder = new IonoFreePppParamNameBuilder(option);
        }

        #region 公共矩阵生成

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。
        /// </summary> 
        /// <returns></returns>
        public override Matrix Coefficient
        {
            get
            {
                //return CoefficientOld;
                int satCount = EnabledSatCount;
                int rowCount = satCount * 2;
                int colCount = this.ParamCount;
                //待计算系统数量
                int sysTypeCount = SatTypeCount;
                var rovSysTypes = SatelliteTypes;
                if (sysTypeCount > 1)
                {
                    sysTypeCount = Math.Min(SatelliteTypes.Count, this.CurrentMaterial.SatelliteTypes.Count);
                    rovSysTypes = Geo.Utils.ListUtil.GetCommons(SatelliteTypes, this.CurrentMaterial.SatelliteTypes);
                    rovSysTypes.Remove(BaseSatType);//移除基准，只保留需要参数的系统
                }

                //相位模糊度系数，可以为米和周，按照设置文件决定。
                double coeefOfPhase = CheckAndGetCoeefOfPhase();
                Matrix A = new Matrix(rowCount, colCount);
                int rangeRow = 0;//卫星编号
                int satIndex = 0;
                int colIndex = 0;
                foreach (var sat in CurrentMaterial.EnabledSats)// 一颗卫星2行
                {
                    var prn = sat.Prn;
                    int phaseRow = rangeRow + satCount;

                    XYZ vector = sat.EstmatedVector;

                    double wetMap = sat.WetMap;
                    colIndex = 0;
                    if (!Option.IsFixingCoord)
                    { 
                        A[rangeRow, 0] = -vector.CosX;
                        A[rangeRow, 1] = -vector.CosY;
                        A[rangeRow, 2] = -vector.CosZ; 

                        A[phaseRow, 0] = A[rangeRow, 0];
                        A[phaseRow, 1] = A[rangeRow, 1];
                        A[phaseRow, 2] = A[rangeRow, 2];
                        colIndex = 3;
                    } 
                    A[rangeRow, colIndex] = 1.0;            //接收机钟差对应的距离 = clkError * 光速
                    A[phaseRow, colIndex] = A[rangeRow, colIndex];
                    colIndex++;

                    if (sysTypeCount == 1 || IsSameTimeSystemInMultiGnss)// 只有一个卫星系统,或所有星历采用一个时间基准
                    {
                        A[rangeRow, colIndex] = wetMap;// DryWetM[1];   //A[row,3] = 299792458.0;//光速
                        A[phaseRow, colIndex] = A[rangeRow, 4];
                        colIndex++;
                        A[phaseRow, colIndex + satIndex] = coeefOfPhase;   //模糊度系数
                    }
                    else//系统之间增加钟差参数
                    {
                        int baseColIndex = colIndex;
                        if (prn.SatelliteType != BaseSatType)
                        {
                            var k = rovSysTypes.IndexOf(prn.SatelliteType);//按照顺序,0 为基准

                            A[rangeRow, baseColIndex + k] = 1.0;
                            A[phaseRow, baseColIndex + k] = 1.0;
                        }
                        int next = baseColIndex + rovSysTypes.Count;
                        A[rangeRow, next] = wetMap;// 对流层湿分量天顶映射函数
                        A[phaseRow, next] = wetMap;

                        next++;
                        A[phaseRow, next + satIndex] = coeefOfPhase;//模糊度系数

                    }
                    rangeRow++;
                    satIndex++;
                }
                A.RowNames = this.BuildObsNames();
                A.ColNames = this.GnssParamNameBuilder.Build();

                return A;
            }
        }
         
        #endregion
    }//end class
}