//2017.05.26, czs, create in hongqing, 建立单频PPP框架
//2017.05.26, cuiyang, edit in chongqing, 修改自非差非组合PPP，实现单频PPP
//2017.08.15, czs, edit in hongqing, 代码梳理
//2018.10.23, czs, create in hmx, 递归算法实现

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
    /// 非组合精密单点定位矩阵生成类。
    /// </summary>
    public class RecursiveSingleFreqPppMatrixBuilder : SingleFreqPppMatrixBuilder
    {
        /// <summary>
        /// 精密单点定位矩阵生成类 构造函数。
        /// </summary> 
        /// <param name="option">解算选项</param> 
        public RecursiveSingleFreqPppMatrixBuilder(
             GnssProcessOption option)
            : base(option)
        {
            this.BasicParamCount = 4;
            if (IsFixingCoord)
            {
                this.BasicParamCount = 1;
            } 
           
            ParamNameBuilder = new RecursiveSingleFreqPppMutabletParamNameBuilder(option);//option中包含了几个系统      
            this.SecondParamNameBuilder = new RecursiveSingleFreqPppConstParamNameBuilder(option);
        }
                 

        #region 公共矩阵生成

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。当时多系统是，卫星排序为G E C
        /// </summary> 
        /// <returns></returns>
        public override Matrix Coefficient
        {
            get
            {
                Geo.Coordinates.GeoCoord geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(CurrentMaterial.SiteInfo.EstimatedXyz, Geo.Coordinates.AngleUnit.Degree);
                NeillTropModel pTroModel = new NeillTropModel(geoCoord.Height, geoCoord.Lat,  CurrentMaterial.ReceiverTime.DayOfYear);

                int satCount = CurrentMaterial.EnabledSatCount;
                int rowCount = satCount * 2;
                double[][] A = Geo.Utils.MatrixUtil.Create(rowCount, this.ParamCount); 
                int rowOfRange = 0;
                int rowOfPhase = 0;//相位观测值对应系数行
                int colOfIono = 0;//电离层参数对应列
                int satIndex = 0;//卫星编号
                int colIndex = 0;

                foreach (var prn in EnabledPrns)// 一颗卫星2行
                {
                    colIndex = 0;
                    rowOfPhase = rowOfRange + satCount;

                    XYZ vector = CurrentMaterial[prn].EstmatedVector;

                    double wetMap = CurrentMaterial[prn].WetMap;
                    double wetMap0 = CurrentMaterial[prn].Vmf1WetMap;

                    #region  公共参数
                    if (!IsFixingCoord)
                    {
                        A[rowOfRange][colIndex++] = -vector.CosX;
                        A[rowOfRange][colIndex++] = -vector.CosY;
                        A[rowOfRange][colIndex++] = -vector.CosZ;
                    }

                    A[rowOfRange][colIndex++] = 1.0;            //接收机钟差对应的距离 = clkError * 光速   //系统间钟差将加载此后   
                    A[rowOfRange][colIndex++] = wetMap;          //对流层湿延迟参数     
                    
                    //更新相同相位列
                    for (int i = 0; i < 5; i++)
                    {
                        A[rowOfPhase][i] = A[rowOfRange][i];
                    }
                    #endregion

                    #region 卫星私有参数，需要加上 satIndex
                    //设置多系统时间参数
                    #region 两个卫星导航系统，默认第一个系统为基准
                    if (SysTypeCount == 2)
                    { 
                        if (prn.SatelliteType == BaseType)
                        {
                            A[rowOfRange][colIndex++] = 0;    
                        }else 
                        {
                            A[rowOfRange][colIndex++] = -1.0;    
                        }                         
                    }
                    #endregion

                    #region 有三个卫星导航系统
                    if (SysTypeCount == 3)
                    {
                        if (prn.SatelliteType == BaseType)
                        {
                            A[rowOfRange][colIndex++] = 0;            //增加系统间时间偏差
                            A[rowOfRange][colIndex++] = 0;           //增加系统间时间偏差           
                        }
                        else if (prn.SatelliteType == Option.SatelliteTypes[1])
                        {
                            A[rowOfRange][colIndex++] = -1.0;            //增加系统间时间偏差
                            A[rowOfRange][colIndex++] = 0;           //增加系统间时间偏差 
                        }
                        else if (prn.SatelliteType == Option.SatelliteTypes[2])
                        {
                            A[rowOfRange][colIndex++] = 0;            //增加系统间时间偏差
                            A[rowOfRange][colIndex++] = -1.0;           //增加系统间时间偏差 
                        }
                    }
                    #endregion


                    //电离层参数
                    colOfIono = this.BasicParamCount + SysTypeCount + satIndex;
                    A[rowOfRange][colOfIono] = 1;//电离层码延迟，波超前
                    A[rowOfPhase][colOfIono] = -1;


                    //最后设置载波相位系数
                    //int colOfAmbi = this.BasicParamCount + SysTypeCount + satCount + satIndex;
                    //A[rowOfPhase][colOfAmbi] = 1;//CurrentMaterial[row].FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性

                    #endregion
                    rowOfRange++;
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
                int colCount = this.SecondParamCount;// satCount;

                double[][] B = Geo.Utils.MatrixUtil.Create(rowCount, colCount);
                int row = 0;//卫星编号               
                foreach (var prn in EnabledPrns)// 一颗卫星2行
                {
                    int colIndex = row;
                    int next = row + satCount;
                //最后设置载波相位系数
                  //int colOfAmbi = this.BasicParamCount + SysTypeCount + satCount + row;
                    B[next][colIndex] = 1;

                    row++;
                }
                return new Matrix(B);
            }
        }
        #endregion


    }//end class
}