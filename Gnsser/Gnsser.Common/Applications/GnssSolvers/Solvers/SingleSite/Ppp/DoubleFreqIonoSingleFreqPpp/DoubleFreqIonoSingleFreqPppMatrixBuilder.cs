//2018.07.05, czs, create in HMX, 双频电离层延迟改正，单频PPP

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
    ///双频电离层延迟改正，单频PPP
    /// </summary>
    public class DoubleFreqIonoSingleFreqPppMatrixBuilder : BasePppPositionMatrixBuilder
    {
        /// <summary>
        /// 通用单站单频计算，高度可配置 构造函数。
        /// </summary> 
        /// <param name="option">解算选项</param> 
        public DoubleFreqIonoSingleFreqPppMatrixBuilder(
             GnssProcessOption option)
            : base(option)
        {           
            ParamNameBuilder = new DoubleFreqIonoSingleFreqPppNameBuilder(option);//option中包含了几个系统    
        
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
        public override int ObsCount { get { return EnabledSatCount * 2; } }
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

                foreach (var sat in  this.CurrentMaterial.EnabledSats)
                {
                    var ionoDcbLenOfL1 =  sat.IonoLenOfL1ByDifferL1L2;
                    var prn = sat.Prn;
                    L[satIndex] = rangeA_Vector[satIndex] - ionoDcbLenOfL1;
                    L.ParamNames[satIndex] = prn + "_C1";

                    L[satIndex + EnabledSatCount] = phaseA_Vector[satIndex] + ionoDcbLenOfL1;
                    L.ParamNames[satIndex + EnabledSatCount] = prn + "_L1";


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
        /// <param name="factor">载波和伪距权逆阵因子（模糊度固定后，才采用默认值！）</param>
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
           // factor = 1.0e-4;//5.0e-5;1.0e-4//   //1 / 140 / 140; 5.1020408163265306122448979591837e-5

            int satCount = CurrentMaterial.EnabledSatCount;
            int row = satCount * 2;
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(row);

            double invFactorOfRange = 1;
            double invFactorOfPhase = Option.PhaseCovaProportionToRange;

            for (int i = 0; i < satCount; i++)
            {
                EpochSatellite e = this.CurrentMaterial[i];
                double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);

                inverseWeight[i][i] = inverseWeightOfSat * invFactorOfRange; 
                inverseWeight[i +  satCount][i + satCount] = inverseWeightOfSat * invFactorOfPhase; 
            }
            return new ArrayMatrix(inverseWeight);
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
                int rowCount =2 * satCount;
                double[][] A = Geo.Utils.MatrixUtil.Create(rowCount, this.ParamCount); 
                int rowOfRange = 0;
                int rowOfPhase = 0;//相位观测值对应系数行 
                int satIndex = 0;//卫星编号

                foreach (var prn in EnabledPrns)// 一颗卫星2行
                {               
                    rowOfPhase = rowOfRange + satCount;

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
                    A[rowOfRange][colIndex++] = 1.0;            //接收机钟差对应的距离 = clkError * 光速   //系统间钟差将加载此后   
                    A[rowOfRange][colIndex++] = wetMap;          //对流层湿延迟参数     
                    
                    //更新相同相位列 
                    for (int i = 0; i < colIndex; i++)
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


                    //硬件延迟差分参数  
                    var colOfIono = colIndex + (satIndex);
                    A[rowOfRange][colOfIono] = -1;//硬件延迟差分
                    A[rowOfPhase][colOfIono] = +1;//硬件延迟差分
                     
                    //最后设置载波相位系数
                    int colOfAmbi = colIndex  + satCount + satIndex;
                    A[rowOfPhase][colOfAmbi] = 1;//CurrentMaterial[row].FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性

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