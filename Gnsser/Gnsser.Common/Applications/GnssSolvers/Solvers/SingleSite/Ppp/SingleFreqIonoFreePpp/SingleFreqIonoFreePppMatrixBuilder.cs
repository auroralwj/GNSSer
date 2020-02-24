//2017.09.14, czs, create in hongqing, 单频消电离层组合

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
using Geo.Algorithm;
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
    /// 单频消电离层组合
    /// </summary>
    public class SingleFreqIonoFreePppMatrixBuilder : BasePppPositionMatrixBuilder
    {
        /// <summary>
        ///单频消电离层组合 构造函数。
        /// </summary> 
        /// <param name="option">解算选项</param> 
        public SingleFreqIonoFreePppMatrixBuilder(
             GnssProcessOption option)
            : base(option)
        {
            ParamNameBuilder = new SingleFreqIonoFreePppNameBuilder(option);//option中包含了几个系统    
        
        }

        #region 全局基础属性 
        /// <summary>
        /// 系统类型数量
        /// </summary>
        public int SysTypeCount { get { return Option.SatelliteTypes.Count; } }
        /// <summary>
        /// 基础类型
        /// </summary>
        public SatelliteType BaseType { get { return    this.GnssParamNameBuilder.BaseSatType; } }
        /// <summary>
        /// 观测量
        /// </summary>
        public override int ObsCount { get { return EnabledSatCount; } }
        
        #endregion 

        #region 创建观测信息

        /// <summary>
        /// 具有权值的观测值。
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
                IMatrix inverseWeightOfObs = BulidInverseWeightOfObs();
                WeightedVector deltaObs = new WeightedVector(ObservationVector, inverseWeightOfObs);
                return deltaObs;
            }
        }

        /// <summary>
        /// 观测值。
        /// 自由项 l，观测值减去先验值或估计值。
        /// 常数项，观测误差方程的常数项,或称自由项
        /// </summary>
        public virtual IVector ObservationVector
        {
            get
            {
                Vector L = new Vector(ObsCount);
                int satIndex = 0;
                XYZ staXyz = CurrentMaterial.SiteInfo.EstimatedXyz;
                //单频，则默认都为A频率
                Vector rangeA_Vector = CurrentMaterial.GetAdjustVector(SatObsDataType.PseudoRangeA); 
                SatObsDataType ObsDataType = Gnsser.SatObsDataType.PhaseRangeA;
                if (Option.IsLengthPhaseValue)//如果是以周为载波单位
                {
                    ObsDataType = Gnsser.SatObsDataType.PhaseA;
                }

                Vector phaseA_Vector = CurrentMaterial.GetAdjustVector(ObsDataType, true); 

                foreach (var item in  this.EnabledPrns)
                {
                    var rangeA = rangeA_Vector[satIndex];
                    var phaseRangeA = phaseA_Vector[satIndex];

                    L[satIndex] = 0.5 * (rangeA + phaseRangeA);
                    L.ParamNames[satIndex] = item + "_LC";
                    satIndex++;
                }

                return L;
            }
        }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。按照先伪距，后载波的顺序排列。
        /// 标准差由常数确定，载波比伪距标准差通常是 1:100，其方差比是 1:10000.
        /// 1.0/140*140=5.10e-5
        /// </summary>  
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        { 
            int row = CurrentMaterial.EnabledSatCount;
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(row);

            double invFactorOfRange = 1; 

            for (int i = 0; i < row; i++)
            {
                EpochSatellite e = this.CurrentMaterial[i];
                double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);
                inverseWeight[i][i] = inverseWeightOfSat * invFactorOfRange;  
            }
            return new Matrix(inverseWeight);
        }

        #endregion

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
                int rowCount = satCount;
                double[][] A = Geo.Utils.MatrixUtil.Create(rowCount, this.ParamCount); 
                int rowOfRange = 0;
                int satIndex = 0;//卫星编号

                foreach (var prn in EnabledPrns)// 一颗卫星2行
                {               
                    XYZ vector = CurrentMaterial[prn].EstmatedVector;

                    double wetMap = CurrentMaterial[prn].WetMap;
                    double wetMap0 = CurrentMaterial[prn].Vmf1WetMap;
                     
                    #region  公共参数
                    int colIndex = 0;
                    if (!Option.IsFixingCoord)
                    {
                        A[rowOfRange][colIndex++] = -vector.CosX;
                        A[rowOfRange][colIndex++] = -vector.CosY;
                        A[rowOfRange][colIndex++] = -vector.CosZ;
                    }
                    A[rowOfRange][colIndex++] = -1.0;            //接收机钟差对应的距离 = clkError * 光速   //系统间钟差将加载此后   
                    A[rowOfRange][colIndex++] = wetMap;          //对流层湿延迟参数     
                    
                    #endregion

                    #region 卫星私有参数，需要加上 satIndex
                    //设置多系统时间参数
                    #region 两个卫星导航系统，默认第一个系统为基准
                    if (SysTypeCount == 2)
                    { 
                        if (prn.SatelliteType == BaseType)
                        {
                            A[rowOfRange][colIndex] = 0;    
                        }else 
                        {
                            A[rowOfRange][colIndex] = -1.0;    
                        }
                        colIndex++;
                    }
                    #endregion

                    #region 有三个卫星导航系统
                    if (SysTypeCount == 3)
                    {
                        if (prn.SatelliteType == BaseType)
                        {
                            A[rowOfRange][colIndex] = 0;            //增加系统间时间偏差
                            A[rowOfRange][colIndex+1] = 0;           //增加系统间时间偏差           
                        }
                        else if (prn.SatelliteType == Option.SatelliteTypes[1])
                        {
                            A[rowOfRange][colIndex] = -1.0;            //增加系统间时间偏差
                            A[rowOfRange][colIndex+1] = 0;           //增加系统间时间偏差 
                        }
                        else if (prn.SatelliteType == Option.SatelliteTypes[2])
                        {
                            A[rowOfRange][colIndex] = 0;            //增加系统间时间偏差
                            A[rowOfRange][colIndex+1] = -1.0;           //增加系统间时间偏差 
                        }
                        colIndex++;
                        colIndex++;
                    }
                    #endregion
                                        
                    //最后设置载波相位系数 
                    A[rowOfRange][colIndex + satIndex] = 0.5; //L1模糊度,保持以米周为单位,

                    #endregion
                    rowOfRange++;
                    satIndex++;
                }
                return new Matrix(A);
            }
        }
         
        #endregion


    }//end class
}