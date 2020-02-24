// 2014.09.03, czs, 独立于Ppp.cs
// 2014.09.05, czs, edit, 实现 IAdjustMatrixBuilder 接口
//201?, cuiyang, edit, 实现非差非组合PPP
//2018.06.29, czs, 修改代码，重构，实现固定参考站的非差非组合PP

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
    /// 非组合精密单点定位矩阵生成类。
    /// </summary>
    public class UncombinedPppMatrixBuilderOld : BasePppPositionMatrixBuilder
    {
        /// <summary>
        /// 精密单点定位矩阵生成类 构造函数。
        /// </summary> 
        /// <param name="option">解算选项</param> 
        public UncombinedPppMatrixBuilderOld(
             GnssProcessOption option)
            : base(option)
        {
            ParamNameBuilder = new UncombinedPppParamNameBuilder(option);//option中包含了几个系统
        }

        #region 全局基础属性



        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return ParamNames.Count; } }  //4个参数分别是x y z trop,如果只有GPS，就加上一个钟差，两个系统，一个钟差+一个系统时间偏差      


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
                Vector L = new Vector(EnabledSatCount * 4);
                int satIndex = 0;
                XYZ staXyz = CurrentMaterial.SiteInfo.EstimatedXyz;

                Vector rangeA_Vector = CurrentMaterial.GetAdjustVector(SatObsDataType.PseudoRangeA);
                Vector rangeB_Vector = CurrentMaterial.GetAdjustVector(SatObsDataType.PseudoRangeB);


                Vector phaseA_Vector = CurrentMaterial.GetAdjustVector(SatObsDataType.PhaseRangeA, true);
                Vector phaseB_Vector = CurrentMaterial.GetAdjustVector(SatObsDataType.PhaseRangeB, true);

                foreach (var sat in CurrentMaterial.EnabledSats)
                {
                    //先伪距观测量
                    int rowIndexOfRangeA = satIndex;
                    int rowIndexOfRangeB = satIndex + EnabledSatCount;
                    int rowIndexOfPhaseA = satIndex + 2 * EnabledSatCount;
                    int rowIndexOfPhaseB = satIndex + 3 * EnabledSatCount;

                    L[rowIndexOfRangeA] = rangeA_Vector[satIndex];
                    L[rowIndexOfRangeB] = rangeB_Vector[satIndex];

                    L[rowIndexOfPhaseA] = phaseA_Vector[satIndex];
                    L[rowIndexOfPhaseB] = phaseB_Vector[satIndex];

                    L.ParamNames[rowIndexOfRangeA] = sat.Prn + "_P1";
                    L.ParamNames[rowIndexOfRangeB] = sat.Prn + "_P2";
                    L.ParamNames[rowIndexOfPhaseA] = sat.Prn + "_L1";
                    L.ParamNames[rowIndexOfPhaseB] = sat.Prn + "_L2"; 


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
            int satCount = CurrentMaterial.EnabledSatCount;
            int row = satCount * 4;
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(row);

            double invFactorOfRange = 1;
            double invFactorOfPhase = Option.PhaseCovaProportionToRange;

            int satIndex = 0;
            foreach (var sat in CurrentMaterial.EnabledSats)
            {
                double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);

                int rowIndexOfRangeA = satIndex;
                int rowIndexOfRangeB = satIndex + EnabledSatCount;
                int rowIndexOfPhaseA = satIndex + 2 * EnabledSatCount;
                int rowIndexOfPhaseB = satIndex + 3 * EnabledSatCount;

                inverseWeight[rowIndexOfRangeA][rowIndexOfRangeA] = inverseWeightOfSat * invFactorOfRange;
                inverseWeight[rowIndexOfRangeB][rowIndexOfRangeB] = inverseWeightOfSat * invFactorOfRange;

                inverseWeight[rowIndexOfPhaseA][rowIndexOfPhaseA] = inverseWeightOfSat * invFactorOfPhase;
                inverseWeight[rowIndexOfPhaseB][rowIndexOfPhaseB] = inverseWeightOfSat * invFactorOfPhase;

                satIndex++;
            }

            return new ArrayMatrix(inverseWeight);
        }

        #endregion

        #region 公共矩阵生成

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。当时多系统是，卫星排序为G E C
        /// </summary>
        /// <param name="epochInfo">观测信息</param>
        /// <param name="aprioriXyz">先验坐标</param>
        /// <returns></returns>
        public override Matrix Coefficient
        {
            get
            {
                Geo.Coordinates.GeoCoord geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(
                    CurrentMaterial.SiteInfo.EstimatedXyz, 
                    Geo.Coordinates.AngleUnit.Radian);

                //   Time time = EpochInfo.CorrectedTime;
                Time time = CurrentMaterial.ReceiverTime;

                NeillTropModel pTroModel = new NeillTropModel(geoCoord.Height, geoCoord.Lat * CoordConsts.RadToDegMultiplier, time.DayOfYear);

                int satCount = CurrentMaterial.EnabledSatCount;
                bool isFixedCoord = this.Option.IsFixingCoord;
                int sameColCount = IsFixingCoord ? 2 : 5;//x,y,z,cdt,trop
               
                int rowCount = satCount * 4;
                int colCount = sameColCount -1 + 3 * satCount + Option.SatelliteTypes.Count;
                double[][] A = Geo.Utils.MatrixUtil.Create(rowCount, colCount);

                int row = 0;//卫星编号
                int satIndex = 0;

                foreach (var sat in CurrentMaterial.EnabledSats)// 一颗卫星2行
                {
                    var prn = sat.Prn;
                    IEphemeris eph = sat.Ephemeris;
                    XYZ vector = eph.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;// this.ReceiverXyz;    
                    int colIndex = 0;
                    double wetMap = sat.WetMap;
                    double wetMap0 = sat.Vmf1WetMap;

                    #region 只有一个卫星导航系统
                    if (Option.SatelliteTypes.Count == 1)
                    {
                        double IonoCoeOfP2 = 0;
                        switch (Option.SatelliteTypes[0])
                        {
                            case SatelliteType.G:
                                IonoCoeOfP2 = GnssConst.CoeOfGPSIono;
                                break;
                            case SatelliteType.E:
                                IonoCoeOfP2 = GnssConst.CoeOfGalileoIono;
                                break;
                            case SatelliteType.C:
                                IonoCoeOfP2 = GnssConst.CoeOfBDIono;
                                break;
                        }
                        //------------P1--------------
                        if (!IsFixingCoord)
                        {
                            A[row][colIndex++] = -vector.CosX;
                            A[row][colIndex++] = -vector.CosY;
                            A[row][colIndex++] = -vector.CosZ;
                        }
                        A[row][colIndex++] = 1.0;            //接收机钟差对应的距离 = clkError * 光速
                        A[row][colIndex++] = wetMap;// DryWetM[1];   //A[row][3] = 299792458.0;//光速

                        int ionoColIndex = sameColCount + satIndex;
                        A[row][ionoColIndex] = 1;// P1的电离层参数的系数
                                                 
                        //------------P2--------------
                        int next = row + satCount;
                        for (int i = 0; i < sameColCount; i++) { A[next][i] = A[row][i];  } 
                        A[next][ionoColIndex] = IonoCoeOfP2;  //P2的电离层参数的系数

                        //------------L1--------------
                        next = row + 2 * satCount;
                        for (int i = 0; i < sameColCount; i++) { A[next][i] = A[row][i]; }
                        A[next][ionoColIndex] = -1;    // L1的电离层参数的系数
                        A[next][sameColCount + satCount + satIndex] = 1;// sat.FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性 //lam_L1
                       
                        //------------L2--------------
                        next = row + 3 * satCount;
                        for (int i = 0; i < sameColCount; i++) { A[next][i] = A[row][i]; }
                        A[next][sameColCount + satIndex] = -IonoCoeOfP2; // L2的电离层参数的系数
                        A[next][sameColCount + 2 * satCount + satIndex] = 1;// sat.FrequenceB.Frequence.WaveLength;  //L2模糊度,保持以周为单位,，但不具有整周特性 //lam_L2
                    }
                    #endregion

                    #region 两个卫星导航系统，默认有GPS
                    if (Option.SatelliteTypes.Count == 2)
                    {
                        if (prn.SatelliteType == SatelliteType.G)
                        {
                            A[row][0] = -vector.CosX;
                            A[row][1] = -vector.CosY;
                            A[row][2] = -vector.CosZ;
                            A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速
                            A[row][4] = 0;            //增加一个参数
                            A[row][5] = wetMap;// DryWetM[1];   //A[row][3] = 299792458.0;//光速
                            A[row][6 + satIndex] = 1;// P1的电离层参数的系数

                            int next = row + satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = GnssConst.CoeOfGPSIono;  //P2的电离层参数的系数

                            next = row + 2 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = -1;    // L1的电离层参数的系数
                            A[next][6 + satCount + satIndex] = 1;// sat.FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性

                            next = row + 3 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = -GnssConst.CoeOfGPSIono;  // L2的电离层参数的系数
                            A[next][6 + 2 * satCount + satIndex] = 1;// sat.FrequenceB.Frequence.WaveLength;  //L2模糊度,保持以周为单位,，但不具有整周特性
                        }

                        if (prn.SatelliteType == SatelliteType.E)
                        {
                            A[row][0] = -vector.CosX;
                            A[row][1] = -vector.CosY;
                            A[row][2] = -vector.CosZ;
                            A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速
                            A[row][4] = -1.0;            //增加一个参数
                            A[row][5] = wetMap;// DryWetM[1];   //A[row][3] = 299792458.0;//光速
                            A[row][6 + satIndex] = 1;// P1的电离层参数的系数

                            int next = row + satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = GnssConst.CoeOfGalileoIono;  //P2的电离层参数的系数

                            next = row + 2 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = -1;    // L1的电离层参数的系数
                            A[next][6 + satCount + satIndex] = 1;//sat.FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性

                            next = row + 3 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = -GnssConst.CoeOfGalileoIono;  // L2的电离层参数的系数
                            A[next][6 + 2 * satCount + satIndex] = 1; //sat.FrequenceB.Frequence.WaveLength;  //L2模糊度,保持以周为单位,，但不具有整周特性
                        }

                        if (prn.SatelliteType == SatelliteType.C)
                        {
                            A[row][0] = -vector.CosX;
                            A[row][1] = -vector.CosY;
                            A[row][2] = -vector.CosZ;
                            A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速
                            A[row][4] = -1.0;            //增加一个参数
                            A[row][5] = wetMap;// DryWetM[1];   //A[row][3] = 299792458.0;//光速
                            A[row][6 + satIndex] = 1;// P1的电离层参数的系数

                            int next = row + satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = GnssConst.CoeOfBDIono;  //P2的电离层参数的系数

                            next = row + 2 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = -1;    // L1的电离层参数的系数
                            A[next][6 + satCount + satIndex] = 1;// sat.FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性

                            next = row + 3 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6 + satIndex] = -GnssConst.CoeOfBDIono;  // L2的电离层参数的系数
                            A[next][6 + 2 * satCount + satIndex] = 1;// sat.FrequenceB.Frequence.WaveLength;  //L2模糊度,保持以周为单位,，但不具有整周特性
                        }

                    }
                    #endregion

                    #region 有三个卫星导航系统 GPS BDS Galileo
                    if (Option.SatelliteTypes.Count == 3)
                    {
                        if (prn.SatelliteType == SatelliteType.G)
                        {
                            A[row][0] = -vector.CosX;
                            A[row][1] = -vector.CosY;
                            A[row][2] = -vector.CosZ;
                            A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速
                            A[row][4] = 0;            //增加系统间时间偏差
                            A[row][5] = 0;           //增加系统间时间偏差
                            A[row][6] = wetMap;// DryWetM[1];   //A[row][3] = 299792458.0;//光速
                            A[row][7 + satIndex] = 1;// P1的电离层参数的系数

                            int next = row + satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = GnssConst.CoeOfGPSIono;  //P2的电离层参数的系数

                            next = row + 2 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = -1;    // L1的电离层参数的系数
                            A[next][7 + satCount + satIndex] = 1;// sat.FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性

                            next = row + 3 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = -GnssConst.CoeOfGPSIono;  // L2的电离层参数的系数
                            A[next][7 + 2 * satCount + satIndex] = 1;// sat.FrequenceB.Frequence.WaveLength;  //L2模糊度,保持以周为单位,，但不具有整周特性
                        }
                        if (prn.SatelliteType == SatelliteType.E)
                        {
                            A[row][0] = -vector.CosX;
                            A[row][1] = -vector.CosY;
                            A[row][2] = -vector.CosZ;
                            A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速
                            A[row][4] = -1.0;            //增加系统间时间偏差
                            A[row][5] = 0;           //增加系统间时间偏差
                            A[row][6] = wetMap;// DryWetM[1];   //A[row][3] = 299792458.0;//光速
                            A[row][7 + satIndex] = 1;// P1的电离层参数的系数

                            int next = row + satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = GnssConst.CoeOfGalileoIono;  //P2的电离层参数的系数

                            next = row + 2 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = -1;    // L1的电离层参数的系数
                            A[next][7 + satCount + satIndex] = 1;// sat.FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性

                            next = row + 3 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = -GnssConst.CoeOfGalileoIono;  // L2的电离层参数的系数
                            A[next][7 + 2 * satCount + satIndex] = 1;// sat.FrequenceB.Frequence.WaveLength;  //L2模糊度,保持以周为单位,，但不具有整周特性
                        }
                        if (prn.SatelliteType == SatelliteType.C)
                        {
                            A[row][0] = -vector.CosX;
                            A[row][1] = -vector.CosY;
                            A[row][2] = -vector.CosZ;
                            A[row][3] = -1.0;            //接收机钟差对应的距离 = clkError * 光速
                            A[row][4] = 0;            //增加系统间时间偏差
                            A[row][5] = -1.0;           //增加系统间时间偏差
                            A[row][6] = wetMap;// DryWetM[1];   //A[row][3] = 299792458.0;//光速
                            A[row][7 + satIndex] = 1;// P1的电离层参数的系数

                            int next = row + satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = GnssConst.CoeOfBDIono;  //P2的电离层参数的系数

                            next = row + 2 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = -1;    // L1的电离层参数的系数
                            A[next][7 + satCount + satIndex] = 1;// sat.FrequenceA.Frequence.WaveLength;  //L1模糊度,保持以周为单位,，但不具有整周特性

                            next = row + 3 * satCount;
                            A[next][0] = A[row][0];
                            A[next][1] = A[row][1];
                            A[next][2] = A[row][2];
                            A[next][3] = A[row][3];
                            A[next][4] = A[row][4];
                            A[next][5] = A[row][5];
                            A[next][6] = A[row][6];
                            A[next][7 + satIndex] = -GnssConst.CoeOfBDIono;  // L2的电离层参数的系数
                            A[next][7 + 2 * satCount + satIndex] = 1;// sat.FrequenceB.Frequence.WaveLength;  //L2模糊度,保持以周为单位,，但不具有整周特性
                        }

                    }
                    #endregion

                    row++;
                    satIndex++;
                }
                return new Matrix(A);
            }
        }


        #endregion


    }//end class
}