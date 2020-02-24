//2017.10.20, czs, create in hongqing, 对流层模型改正伪距定位
//2018.06.07, czs, edit in HMX, 增加参考站固定

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
    /// 对流层模型改正伪距定位矩阵生成类。基础变量数量，此处为5个 dx,dy,dz,dClk, wetTrop
    /// </summary>
    public class RangePositionWithTropParamMatrixBuilder : BasePppPositionMatrixBuilder
    {
        /// <summary>
        /// 对流层模型改正伪距定位 构造函数。
        /// </summary> 
        /// <param name="option">解算选项</param> 
        public RangePositionWithTropParamMatrixBuilder(
             GnssProcessOption option)
            : base(option)
        {
            ParamNameBuilder = new RangePositionWithTropParamNameBuilder(option);//option中包含了几个系统                        
        }

        #region 全局基础属性
        /// <summary>
        /// 系统类型数量
        /// </summary>
        public int SysTypeCount { get { return Option.SatelliteTypes.Count; } }
        /// <summary>
        /// 系统类型
        /// </summary>
        public SatelliteType BaseType { get { return Option.SatelliteTypes[0]; } }


        /// <summary>
        /// 观测数量
        /// </summary>
        public override int ObsCount { get { return CurrentMaterial.EnabledSatCount; } }
 
        #endregion
         
        #region 创建观测信息
 
        /// <summary>
        /// 残差
        /// </summary>
        public override WeightedVector Observation
        {
            get
            {
                Vector obs = new Vector(this.ObsCount);
                Vector app = new Vector(this.ObsCount);
                int i = 0;
                foreach (var sat in this.CurrentMaterial.EnabledSats)
                {
                    //是否指定观测类型
                    var obsObj = sat.GetDataValue(Option.ObsDataType);
                    if (obsObj != null && obsObj.Value != 0)
                    {
                        obs[i] = obsObj.CorrectedValue;
                    }
                    else//最低定位条件，确保获取一个定位结果。
                    {
                        obs[i] = sat.AvailablePseudoRange.CorrectedValue; //近似值 
                    }
                    obs.ParamNames[i] = sat.Prn + "";
                    i++;
                }
                var vector = new AdjustVector(obs, app);
                var cova = BulidInverseWeightOfObs();
                return new WeightedVector(vector, cova);
            }
        }
        /// <summary>
        /// 观测量的权逆阵，一个对角阵。 
        /// </summary>  
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs( )
        { 
            int satCount = CurrentMaterial.EnabledSatCount;
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(satCount);

            for (int i = 0; i < satCount; i++)
            {
                EpochSatellite e = this.CurrentMaterial[i];
                inverseWeight[i][i] = SatWeightProvider.GetInverseWeightOfRange(e); 
            }
            return new ArrayMatrix(inverseWeight);
        }

        /// <summary>
        ///  自由项D，B0等等。则参数平差中，满足满足 l = L - (AX0 + D).
        ///  此处，FreeVector = (AX0 + D)。
        /// </summary>
        public override Vector FreeVector
        {
            get
            {
                Vector app = new Vector(this.ObsCount);
                int i = 0;
                foreach (var sat in this.CurrentMaterial.EnabledSats)
                {
                    var obsObj = sat.GetDataValue(Option.ApproxDataType);
                    if (obsObj != null && obsObj.Value != 0)
                    {
                        app[i] = obsObj.CorrectedValue;
                    }
                    else//最低定位条件，确保获取一个定位结果。
                    {
                        app[i] = sat.AvailableApproxPseudoRange.CorrectedValue; //近似值 
                    }
                    app.ParamNames[i] = sat.Prn + "";

                    i++;
                }
                return app;
            }
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
               
                foreach (var prn in EnabledPrns)// 一颗卫星1行
                {
                    int paramIndex = 0;

                    XYZ vector = CurrentMaterial[prn].EstmatedVector;

                    double wetMap = CurrentMaterial[prn].WetMap;
                    double wetMap0 = CurrentMaterial[prn].Vmf1WetMap;

                    #region  公共参数

                    if (!Option.IsFixingCoord)
                    {
                        A[rowOfRange][paramIndex++] = -vector.CosX;
                        A[rowOfRange][paramIndex++] = -vector.CosY;
                        A[rowOfRange][paramIndex++] = -vector.CosZ;
                    }

                    A[rowOfRange][paramIndex++] = 1.0;            //接收机钟差对应的距离 = clkError * 光速   //系统间钟差将加载此后   
                    A[rowOfRange][paramIndex++] = wetMap;          //对流层湿延迟参数     
            
                    #endregion

                    #region 卫星私有参数，需要加上 satIndex
                    //设置多系统时间参数
                    #region 两个卫星导航系统，默认第一个系统为基准
                    if (SysTypeCount == 2)
                    { 
                        if (prn.SatelliteType == BaseType)
                        {
                            A[rowOfRange][paramIndex++] = 0;    
                        }else 
                        {
                            A[rowOfRange][paramIndex++] = -1.0;    
                        }                         
                    }
                    #endregion

                    #region 有三个卫星导航系统
                    if (SysTypeCount == 3)
                    {
                        if (prn.SatelliteType == BaseType)
                        {
                            A[rowOfRange][paramIndex++] = 0;            //增加系统间时间偏差
                            A[rowOfRange][paramIndex++] = 0;           //增加系统间时间偏差           
                        }
                        else if (prn.SatelliteType == Option.SatelliteTypes[1])
                        {
                            A[rowOfRange][paramIndex++] = -1.0;            //增加系统间时间偏差
                            A[rowOfRange][paramIndex++] = 0;           //增加系统间时间偏差 
                        }
                        else if (prn.SatelliteType == Option.SatelliteTypes[2])
                        {
                            A[rowOfRange][paramIndex++] = 0;            //增加系统间时间偏差
                            A[rowOfRange][paramIndex++] = -1.0;           //增加系统间时间偏差 
                        }
                    }
                    #endregion

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