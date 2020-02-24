
// 2015.05.23, cy ,组双差
//2016.10.25, cy, edit in zz, 修改判断基准星是否周跳

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm; 
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Times;
using Gnsser.Times;
using Gnsser.Service;
using Gnsser.Domain;
using Gnsser.Checkers;
using Gnsser.Models;
using Gnsser.Data.Rinex;

namespace Gnsser.Service
{
    /// <summary>
    /// 无电离层双差矩阵构造器
    /// </summary>
    public class IonFreeDoubleDifferMatrixBuilder : MultiSiteMatrixBuilder// BasePositionMatrixBuilder
    {
        /// <summary>
        /// 无电离层双差矩阵构造器
        /// </summary>
        /// <param name="option"></param>
        /// <param name="baseParamCount"></param>
        public IonFreeDoubleDifferMatrixBuilder(
            GnssProcessOption option, int baseParamCount)
            : base(option)
        {
            this.ParamNameBuilder = new IonoFreeDoubleDifferParamNameBuilder(option);
            this.BaseParamCount = baseParamCount;
            SatWeightProvider = new SatElevateWeightProvider(option);
        } 
        /// <summary>
        /// 参数是否改变。
        /// </summary>
        public override bool IsParamsChanged
        {
            get
            {
                return base.IsParamsChanged ||this.IsBaseSatUnstable;
            }
        }
        /// <summary>
        /// 参考站信息
        /// </summary>
        EpochInformation RefInfo { get { return CurrentMaterial.BaseEpochInfo; } }
        /// <summary>
        /// 流动站
        /// </summary>
        EpochInformation RovInfo { get { return CurrentMaterial.OtherEpochInfo; } }        


        /// <summary>
        /// 基础参数的总数，即除了模糊度的剩余参数的个数
        /// 长基线时是5，即三个坐标参数+两个对流层参数
        /// 短基线时是3，即三个坐标参数
        /// </summary>
        public int BaseParamCount { get; set; }
         
        /// <summary>
        /// 构建
        /// </summary>
        public override void Build()
        { 
            //本类所独有的
            GnssParamNameBuilder.BaseParamCount = BaseParamCount; 
             
            if (this.RefInfo[CurrentBasePrn].IsUnstable || this.RovInfo[CurrentBasePrn].IsUnstable)
            {
                IsBaseSatUnstable = true;
            }
            base.Build();
        }
         

        /// <summary>
        /// 观测值数量。
        /// </summary>
        public override int ObsCount { get { return (EnabledSatCount - 1) * 2; } }

        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return (EnabledSatCount - 1) + BaseParamCount; } } 



        #region 参数先验信息
        /// <summary>
        /// 创建先验信息
        /// </summary>  
        protected override WeightedVector CreateInitAprioriParam()
        {
            return PppMatrixHelper.GetInitDoubleAprioriParam(this.RovInfo.EnabledSatCount - 1 + BaseParamCount, BaseParamCount, this.Option.InitApproxXyzRms); 
        }
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
                deltaObs.ParamNames = GetObsNames();
                return deltaObs;
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
            int satCount = EnabledSatCount;
           
            //首先建立非差观测值的权逆阵，这里前一半是rov站的，后一半是ref站的
            int row = (satCount) * 4;
            DiagonalMatrix inverseWeight =new DiagonalMatrix(row);

            double invFactorOfRange = 1;
            double invFactorOfPhase = Option.PhaseCovaProportionToRange;

            for (int i = 0; i < satCount; i++)
            {
                EpochSatellite e = this.RovInfo.EnabledSats[i];
                double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);

                inverseWeight[i,i] = inverseWeightOfSat * invFactorOfRange;
                inverseWeight[i + satCount,i + satCount] = inverseWeightOfSat * invFactorOfPhase;

                e = this.RefInfo[i];
                inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);

                inverseWeight[i + 2 * satCount,i + 2 * satCount] = inverseWeightOfSat * invFactorOfRange;
                inverseWeight[i + 3 * satCount,i + 3 * satCount] = inverseWeightOfSat * invFactorOfPhase;
            }

            DiagonalMatrix undiffInverseWeigth = (inverseWeight);
            double[][] sigleCoeff = Geo.Utils.MatrixUtil.Create(satCount * 2, satCount * 4, 0f);
            for (int i = 0; i < 2 * satCount;i++ )
            {
                sigleCoeff[i][i] = 1; //流动站
                sigleCoeff[i][2 * satCount + i] = -1; //参考站
            }

            IMatrix sigleCoeffMatrix = new ArrayMatrix(sigleCoeff);
            IMatrix sigleInverseWeigth = sigleCoeffMatrix.Multiply(undiffInverseWeigth).Multiply(sigleCoeffMatrix.Transposition);
            double[][] doubleCoeff = Geo.Utils.MatrixUtil.Create((satCount - 1) * 2, satCount * 2, 0f);
            int baseindex = this.RovInfo.EnabledPrns.IndexOf(CurrentBasePrn);
            int satIndex = 0;  
           
            foreach (var item in RovInfo.EnabledSats)
            {
                if (item.Prn != CurrentBasePrn)
                {
                    doubleCoeff[satIndex][baseindex] = -1; //参考星  
                    int index = this.RovInfo.EnabledPrns.IndexOf(item.Prn);
                    doubleCoeff[satIndex ][index] = 1; //流动星
                    //载波
                    doubleCoeff[satIndex + satCount - 1][baseindex + satCount] = -1;
                    doubleCoeff[satIndex + satCount - 1][index + satCount] = 1;
                    satIndex++;
                }
            }

            IMatrix doubleCoeffMatrix = new ArrayMatrix(doubleCoeff);
            IMatrix doubleInverseWeigth = doubleCoeffMatrix.Multiply(sigleInverseWeigth).Multiply(doubleCoeffMatrix.Transposition);
            return doubleInverseWeigth;
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
                double[] L = new double[ObsCount];
                int satIndex = 0;
                //XYZ staXyz = EpochInfo.ApproxXyz;
                Vector refRangeVector = RefInfo.GetAdjustVector(SatObsDataType.IonoFreeRange);
                Vector rovRangeVector = RovInfo.GetAdjustVector(SatObsDataType.IonoFreeRange);



                //Vector refPhaseVector = RefInfo.GetRangeVector(SatObsDataType.AlignedIonoFreePhaseRange, true);
                //Vector rovPhaseVector = EpochInfo.GetRangeVector(SatObsDataType.AlignedIonoFreePhaseRange, true);
                Vector refPhaseVector = RefInfo.GetAdjustVector(SatObsDataType.IonoFreePhaseRange, true);
                Vector rovPhaseVector = RovInfo.GetAdjustVector(SatObsDataType.IonoFreePhaseRange, true);


                int baseSatIndex = RovInfo.EnabledPrns.IndexOf(CurrentBasePrn);

                int i = 0;
                foreach (var item in RovInfo.EnabledSats)
                {
                    if (item.Prn != CurrentBasePrn)
                    {
                        L[i] = rovRangeVector[satIndex] - refRangeVector[satIndex] - (rovRangeVector[baseSatIndex] - refRangeVector[baseSatIndex]);
                        L[i + (EnabledSatCount - 1)] = rovPhaseVector[satIndex] - refPhaseVector[satIndex] - (rovPhaseVector[baseSatIndex] - refPhaseVector[baseSatIndex]);
                        i++;
                    }
                    satIndex++;
                }

                return new Vector(L);
            }
        }
        /// <summary>
        /// 观测名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetObsNames()
        {
            var names = new string[ObsCount];
            int rangeRow = 0;
            int siteCount = CurrentMaterial.Count;
            int rangeRowCount = this.ObsCount / 2;

            foreach (var prn in this.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                int phaseRow = rangeRow + rangeRowCount;
                names[rangeRow] = GnssParamNameBuilder.GetDoubleDifferObsPCodeName(prn, CurrentBasePrn);
                names[phaseRow] = GnssParamNameBuilder.GetDoubleDifferObsLCodeName(prn, CurrentBasePrn);
                rangeRow++;
            }
            return new List<string>(names);
        }

        #endregion

        #region 公共矩阵生成

        /// <summary>
        /// 双差-参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。
        /// </summary>
        /// <returns></returns>
        public override Matrix Coefficient
        {
            get
            {
              //  Geo.Coordinates.GeoCoord geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(EpochInfo.ApproxXyz, Geo.Coordinates.AngleUnit.Radian);

                //   Time time = EpochInfo.CorrectedTime;
              //  Time time = EpochInfo.ReceiverTime;

              //NeillTropModel pTroModel = new NeillTropModel(geoCoord.Height, geoCoord.Lat *Geo.CoordConsts.RadToDegMultiplier, time.DayOfYear);

                int satCount =  EnabledSatCount;
                int rowCount = (satCount - 1) * 2;
                int colCount = (satCount - 1) + BaseParamCount;
                double[][] A = Geo.Utils.MatrixUtil.Create(rowCount, colCount);

                int row = 0;//卫星编号
                int satIndex = 0;
                
                XYZ baseVector = RovInfo[CurrentBasePrn].Ephemeris.XYZ - this.RovInfo.SiteInfo.ApproxXyz;
                
                double f1 = RovInfo[0].FrequenceA.Frequence.Value * 1E6;
               double f2 = RovInfo[0].FrequenceB.Frequence.Value * 1E6;
               double lam_LC = GnssConst.LIGHT_SPEED / (f1 + f2);
              
                double lam_L1 = GnssConst.LIGHT_SPEED / f1;
             
                foreach (var prn in EnabledPrns)// 一颗卫星2行
                {
                    if (prn != CurrentBasePrn)
                    {
                        IEphemeris sat = RovInfo[prn].Ephemeris;

                        XYZ vector = sat.XYZ - this.RovInfo.SiteInfo.ApproxXyz;// this.ReceiverXyz;
                        //对流层延迟，第一个是干分量的延迟量，第二个值是湿分量的映射函数系数
                        //double[] DryWetM = TropoCorrection.GetDryTropCorrectValue(time, sat.XYZ, this.EpochInfo.ApproxXyz);
                        //Polar p = EpochInfo[prn].Polar;

                        ////Scalar to hold satellite elevation
                        //double elevation = p.Elevation;// sat[GnssDataType.Elevation]; 
                        ////Compute tropospheric slant correction
                        //double tropoCorr = pTroModel.Correction(elevation);
                        //double dryZDelay = pTroModel.Dry_Zenith_Delay();
                        //double wetZDelay = pTroModel.Wet_Zenith_Delay();
                        //double dryMap = pTroModel.Dry_Mapping_Function(elevation);
                        //double wetMap = pTroModel.Wet_Mapping_Function(elevation);


                        if (BaseParamCount == 5)
                        {

                            double refwetMap = RefInfo[prn].WetMap - RefInfo[CurrentBasePrn].WetMap; //参考站
                            //    refwetMap = (EpochInfo[satelliteType].WetMap - RefInfo[satelliteType].WetMap) - (EpochInfo[BasePrn].WetMap - RefInfo[BasePrn].WetMap); //参考站

                            double rovWetMap = RovInfo[prn].WetMap - RovInfo[CurrentBasePrn].WetMap;

                            //    double wetMap0 = EpochInfo[satelliteType].Vmf1WetMap;

                            A[row][0] = -(vector.CosX - baseVector.CosX);
                            A[row][1] = -(vector.CosY - baseVector.CosY);
                            A[row][2] = -(vector.CosZ - baseVector.CosZ);
                            //  A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速  A[row][3] = 299792458.0;//光速
                            A[row][3] = rovWetMap;// DryWetM[1];   //流动站对流层湿延迟
                            A[row][4] = -refwetMap;// DryWetM[1];   //基准站对流层湿延迟



                            int next = row + (satCount - 1);

                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];

                            A[next][5 + satIndex] = lam_L1;//1;//模糊度,保持以周为单位,，但不具有整周特性
                        }

                        if (BaseParamCount == 4)
                        {

                            double refwetMap = RefInfo[prn].WetMap - RefInfo[CurrentBasePrn].WetMap; //参考站
                            //    refwetMap = (EpochInfo[satelliteType].WetMap - RefInfo[satelliteType].WetMap) - (EpochInfo[BasePrn].WetMap - RefInfo[BasePrn].WetMap); //参考站

                            double wetMap = RovInfo[prn].WetMap - RovInfo[CurrentBasePrn].WetMap;

                            //    double wetMap0 = EpochInfo[satelliteType].Vmf1WetMap;

                            A[row][0] = -(vector.CosX - baseVector.CosX);
                            A[row][1] = -(vector.CosY - baseVector.CosY);
                            A[row][2] = -(vector.CosZ - baseVector.CosZ);
                            //  A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速  A[row][3] = 299792458.0;//光速
                            A[row][3] = wetMap;// DryWetM[1];   //流动站对流层湿延迟
                            // A[row][4] = -refwetMap;// DryWetM[1];   //基准站对流层湿延迟



                            int next = row + (satCount - 1);

                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            // A[next][4] = A[row][4];

                            A[next][4 + satIndex] = lam_L1;//1;//模糊度,保持以周为单位,，但不具有整周特性
                        }

                        if (BaseParamCount == 3)
                        {

                            double refwetMap = RefInfo[prn].WetMap - RefInfo[CurrentBasePrn].WetMap; //参考站
                            //    refwetMap = (EpochInfo[satelliteType].WetMap - RefInfo[satelliteType].WetMap) - (EpochInfo[BasePrn].WetMap - RefInfo[BasePrn].WetMap); //参考站

                            double wetMap = RovInfo[prn].WetMap - RovInfo[CurrentBasePrn].WetMap;

                            //    double wetMap0 = EpochInfo[satelliteType].Vmf1WetMap;

                            A[row][0] = -(vector.CosX - baseVector.CosX);
                            A[row][1] = -(vector.CosY - baseVector.CosY);
                            A[row][2] = -(vector.CosZ - baseVector.CosZ);
                            //  A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速  A[row][3] = 299792458.0;//光速
                            //  A[row][3] = wetMap;// DryWetM[1];   //流动站对流层湿延迟
                            // A[row][4] = -refwetMap;// DryWetM[1];   //基准站对流层湿延迟



                            int next = row + (satCount - 1);

                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            //  A[next][3] = A[row][3];
                            // A[next][4] = A[row][4];

                            A[next][3 + satIndex] = lam_L1;//1;//模糊度,保持以周为单位,，但不具有整周特性
                        }


                        row++;
                        satIndex++;
                    }
                }
                if (A.Length == 0)
                {
                    return null;
                }
               // return new SparseMatrix(A);
                var coeff=  new Matrix(A);
                coeff.ColNames= this.ParamNames;
                coeff.RowNames = this.GetObsNames();
                return coeff;
            }
        }


        #endregion 

    }//end class
}
