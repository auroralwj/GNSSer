//2017.11.08, czs, create in hongqing,单站单频多历元电离层参数化定位

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Models;
using Geo.Algorithm.Adjust;

namespace Gnsser.Service
{
    /// <summary>
    /// 单站单频多历元电离层参数化定位
    /// </summary>
    public class SingleSitePeriodParamedIonoMatrixBuilder : SingleSitePeroidMatrixBuilder 
    {
        #region 构造函数
        /// <summary>
        ///单站单频多历元电离层参数化定位
        /// </summary>  
        /// <param name="model">解算选项</param> 
        public SingleSitePeriodParamedIonoMatrixBuilder(
            GnssProcessOption model)
            : base(model)
        {
            this.ParamNameBuilder = new SingleSitePeriodParamedIonoParamNameBuilder(this.Option);
        }
        #endregion



        #region 全局基础属性
        /// <summary>
        /// 系统类型数量
        /// </summary>
        public int SysTypeCount { get { return Option.SatelliteTypes.Count; } }
        /// <summary>
        /// 基础类型
        /// </summary>
        public SatelliteType BaseType { get { return this.GnssParamNameBuilder.BaseSatType; } }
        /// <summary>
        /// 观测量
        /// </summary>
        public override int ObsCount { get { return 2 * EnabledSatCount * EpochCount; } }


        #endregion

        protected override WeightedVector CreateInitAprioriParam()
        {
            var init = base.CreateInitAprioriParam();
            int  i = 0;
            foreach (var item in init.ParamNames)
            {
                if (item.Contains(Gnsser.ParamNames.Ambiguity))
                {
                    var prn = SatelliteNumber.Parse(item);
                    var freq = this.CurrentMaterial[0][prn].FrequenceA;
                    init.Set(i, freq.ApproxAmbiguityLength, 10000);//100米精度
                }
                i++;
            }
            return init;
        }

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
                XYZ staXyz = CurrentMaterial.SiteInfo.EstimatedXyz;
                //单频，则默认都为A频率 
                SatObsDataType ObsDataType = Gnsser.SatObsDataType.PhaseRangeA;
                if (Option.IsLengthPhaseValue)//如果是以周为载波单位
                {
                    ObsDataType = Gnsser.SatObsDataType.PhaseA;
                } 
                var enabledPrns = this.CurrentMaterial.EnabledPrns; 
                int epochIndex = 0;
                foreach (var epoch in this.CurrentMaterial)
                {
                    int satIndex = 0;
                    int epochStartIndex = this.EnabledSatCount * 2 * epochIndex;//当前历元参数起始编号
                    foreach (var item in enabledPrns)
                    {
                        var rangeA = epoch[item].GetAdjustValue(SatObsDataType.PseudoRangeA);
                        var phaseRangeA = epoch[item].GetAdjustValue(ObsDataType);
                        int rangIndex = GetRangIndex(satIndex, epochStartIndex);
                        int phaseIndex =  GetPhaseIndex(satIndex, epochStartIndex);

                        L[rangIndex ] = rangeA;
                        L.ParamNames[rangIndex] = item + "_C1_" + epoch.ReceiverTime.ToShortTimeString();

                        L[phaseIndex] = phaseRangeA;
                        L.ParamNames[phaseIndex] = item + "_L1_" + epoch.ReceiverTime.ToShortTimeString(); 
                        
                        satIndex++;
                    }
                    epochIndex++;
                }

                return L;
            }
        }

        private int GetPhaseIndex(int satIndex, int epochStartIndex) { return satIndex + EnabledSatCount + epochStartIndex; }

        private static int GetRangIndex(int satIndex, int epochStartIndex) { return satIndex + epochStartIndex; }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。按照先伪距，后载波的顺序排列。
        /// 标准差由常数确定，载波比伪距标准差通常是 1:100，其方差比是 1:10000.
        /// 1.0/140*140=5.10e-5
        /// </summary>  
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            int row = ObsCount;
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(ObsCount);

            double invFactorOfRange = 1;
            double invFactorOfPhase = Option.PhaseCovaProportionToRange;
            int epochIndex = 0;
            foreach (var epoch in this.CurrentMaterial)
            {
                int satIndex = 0;
                int epochStartIndex = this.EnabledSatCount * 2 * epochIndex;//当前历元参数起始编号
                foreach (var prn in EnabledPrns)
                {
                    int rangIndex = GetRangIndex(satIndex, epochStartIndex);
                    int phaseIndex = GetPhaseIndex(satIndex, epochStartIndex);

                    EpochSatellite e = epoch[prn]; 

                    double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);

                    inverseWeight[rangIndex][rangIndex] = inverseWeightOfSat * invFactorOfRange;
                    inverseWeight[phaseIndex][phaseIndex] = inverseWeightOfSat * invFactorOfPhase;

                    satIndex++;
                }
                epochIndex++;
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
                NeillTropModel pTroModel = new NeillTropModel(geoCoord.Height, geoCoord.Lat, CurrentMaterial.ReceiverTime.DayOfYear);
                 
                double[][] A = Geo.Utils.MatrixUtil.Create(ObsCount, this.ParamCount);
                int row = 0;
                int epochIndex = 0;

                foreach (var epoch in this.CurrentMaterial)
                {   
                    row = 2 * EnabledSatCount * epochIndex;
                    int satIndex = 0;//卫星编号
                    foreach (var prn in EnabledPrns)// 一颗卫星2行
                    {
                        XYZ vector = epoch[prn].EstmatedVector;

                        double wetMap = epoch[prn].WetMap;
                        double wetMap0 = epoch[prn].Vmf1WetMap;
                        int next = row + EnabledSatCount;
                        #region  公共参数
                        int colIndex = 3;
                        if (!Option.IsFixingCoord)
                        {
                            A[row][0] = -vector.CosX;
                            A[row][1] = -vector.CosY;
                            A[row][2] = -vector.CosZ;

                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];       
                        }
                        //由于接收机钟的不稳定性，一个历元一个钟差                      
                        if (Option.IsFixingCoord) { colIndex = -3; }
                        colIndex = colIndex + epochIndex;
                        A[row][colIndex] = -1.0;
                        A[next][colIndex] = -1.0;            //接收机钟差对应的距离 = clkError * 光速   //系统间钟差将加载此后                             
 
                        //对流层的参数编号
                        colIndex = 3 + EpochCount;
                        if (Option.IsFixingCoord) { colIndex = -3; }
                        A[row][colIndex] = wetMap;          //对流层湿延迟参数     
                        A[next][colIndex] = wetMap; 
                        #endregion

                        #region 卫星私有参数，需要加上 satIndex
                        //设置多系统时间参数
                        #region 两个卫星导航系统，默认第一个系统为基准
                        if (SysTypeCount == 2)
                        {
                            colIndex++;
                            if (prn.SatelliteType == BaseType)
                            {
                                A[row][colIndex] = 0;
                                A[next][colIndex] = 0;
                            }
                            else
                            {
                                A[row][colIndex] = -1.0;
                                A[next][colIndex] = -1.0;
                            }
                        }
                        #endregion

                        #region 有三个卫星导航系统
                        if (SysTypeCount == 3)
                        {
                            colIndex++; ;
                            if (prn.SatelliteType == BaseType)
                            {
                                A[row][colIndex] = 0;            //增加系统间时间偏差
                                A[row][colIndex + 1] = 0;           //增加系统间时间偏差  
                                A[next][colIndex] = 0;            //增加系统间时间偏差
                                A[next][colIndex + 1] = 0;           //增加系统间时间偏差          
                            }
                            else if (prn.SatelliteType == Option.SatelliteTypes[1])
                            {
                                A[row][colIndex] = -1.0;            //增加系统间时间偏差
                                A[row][colIndex + 1] = 0;           //增加系统间时间偏差 
                                A[next][colIndex] = -1.0;            //增加系统间时间偏差
                                A[next][colIndex + 1] = 0;           //增加系统间时间偏差 
                            }
                            else if (prn.SatelliteType == Option.SatelliteTypes[2])
                            {
                                A[row][colIndex] = 0;            //增加系统间时间偏差
                                A[row][colIndex + 1] = -1.0;           //增加系统间时间偏差
                                A[next][colIndex] = 0;            //增加系统间时间偏差
                                A[next][colIndex + 1] = -1.0;           //增加系统间时间偏差 
                            }
                            colIndex++;
                            colIndex++;
                        }
                        #endregion

                        //电离层参数系数 
                        //        xyz    dt        wetTro      tsys          iono   
                        colIndex = 3 + EpochCount + 1 + (SysTypeCount - 1) + satIndex;
                        if (Option.IsFixingCoord) { colIndex = -3; }
                        A[row][colIndex] = -1; //L1电离层参数,
                        A[next][colIndex] = 1; //L1电离层参数,

                        //最后设置载波相位系数 
                        //        xyz    dt        wetTro    tsys             iono             sat
                        colIndex = 3 + EpochCount + 1 + (SysTypeCount - 1) + EnabledSatCount + satIndex;
                        if (Option.IsFixingCoord) { colIndex = -3; } 
                        A[next][colIndex] = 1; //L1模糊度,保持以米周为单位,

                        #endregion

                        row++;
                        satIndex++;
                    }
                    epochIndex++;
                }              
                return new Matrix(A);
            }
        }

        #endregion

        public override WeightedMatrix Transfer
        {
            get
            {
                return base.Transfer;
            }
        }

    }
}
