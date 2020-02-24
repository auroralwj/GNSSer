//2014.08.30, czs, edit, 开始重构
//2014.08.31, czs, edit, 重构于 西安 到 沈阳 的航班上，春秋航空。
//2015.10.25, czs, eidt in pengzhou, Ppp 重命名为 IonoFreePpp
//2016.01.31, czs, edit in hongqing, 修复精度，简化流程
//2016.03.10, czs, edit in hongqing, 重构设计

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Service;
using Gnsser.Checkers;
using Geo.Times;
using Gnsser.Filter;

using System.IO;
using Gnsser.Data;

namespace Gnsser.Service
{
    //包含4类参数，测站位置（x,y,z），钟差（Cdt），对流程天顶距延迟(zpd)和非整的整周模糊度（N）。

    /// PPP 计算核心方法。 Kalmam滤波。
    /// 观测量的顺序是先伪距观测量，后载波观测量，观测量的总数为卫星数量的两倍。
    /// 参数数量为卫星数量加5,卫星数量对应各个模糊度，5为3个位置量xyz，1个接收机钟差量，1个对流程湿分量。
    /// <summary>
    /// 精密单点定位。
    /// 此处采用观测值残差向量计算。
    /// 条件：必须是双频观测，且观测卫星数量大于5个。
    /// 参考：Jan Kouba and Pierre Héroux. GPS Precise Point Positioning Using IGS Orbit Products[J].2000,sep
    /// </summary>
    public class IonoFreePppOld : SingleSiteGnssSolver
    {
        #region 构造函数

        /// <summary>
        /// 最简化的构造函数，可以多个定位器同时使用的参数，而不必多次读取
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public IonoFreePppOld(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "无电离层组合PPP";
            this.BaseParamCount = 5;
            if (PositionOption.ApproxDataType == SatApproxDataType.ApproxPseudoRangeOfTriFreq || PositionOption.ApproxDataType == SatApproxDataType.ApproxPhaseRangeOfTriFreq)
            { this.MatrixBuilder = new IonoFreePppOfTriFreqMatrixBuilder(PositionOption); }
            else { this.MatrixBuilder = new IonoFreePppMatrixBuilder(PositionOption); }
            this.SUM_MWs = new Dictionary<SatelliteNumber, MWInfo>();
        }
        #endregion
        /// <summary>
        /// 宽巷整数部分
        /// </summary>
        public ObjectTableStorage IntValueTableOfWL { get; set; }
        /// <summary>
        /// 窄巷的整数部分
        /// </summary>
        public ObjectTableStorage IntValueTableOfNL { get; set; }
        /// <summary>
        /// 窄巷的小数部分
        /// </summary>
        public ObjectTableStorage FractionValueTableOfNL { get; set; }
        FcbOfUpdFile  FcbFile { get; set; }

        /// <summary>
        /// 逐历元更新的，每颗卫星的MW值
        /// </summary>
        private Dictionary<SatelliteNumber, MWInfo> SUM_MWs { get; set; }
        public class MWInfo
        {
            public double sum { get; set; }
            public double total { get; set; }
        }

        List<XYZ> newxyz = new List<XYZ>();
        public Dictionary<Time, XYZ> newres = new Dictionary<Time, XYZ>();
        /// <summary>
        /// 独立计算
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public override SingleSiteGnssResult CaculateIndependent(EpochInformation material)
        {
            if (RangePositioner != null)
            {
                var posResult = RangePositioner.CurrentProduct;

            }


            this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));

            if (Adjustment.Estimated == null) return null;

            return BuildResult();
        }

        /// <summary>
        /// 滤波计算
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="lastResult"></param>
        /// <returns></returns>
        public override SingleSiteGnssResult CaculateKalmanFilter(EpochInformation epochInfo, SingleSiteGnssResult lastResult)
        {
            if (RangePositioner != null)
            {
                var posResult = RangePositioner.CurrentProduct;

            }
            var result = base.CaculateKalmanFilter(epochInfo, lastResult) as PppResult;
            #region
            if (this.IsFixingAmbiguity && epochInfo.Time.Value.SecondsOfDay > epochInfo.ObsInfo.StartTime.SecondsOfDay + 120)//&& this.Adjustment.ParamCount>10000000
            {
                #region 读取自定义FCB文件
                string WLpath = Path.Combine(Setting.DataDirectory, @"GNSS\Attachment\2013001FCB\WLFCB.txt");
                Dictionary<SatelliteNumber, double> SDWL_FCB = new Dictionary<SatelliteNumber, double>();
                Dictionary<SatelliteNumber, double> SDNL_FCB = new Dictionary<SatelliteNumber, double>();
                using (var streamReader = new StreamReader(WLpath))
                {
                    var line = "";
                    Time time = Time.Default;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        SatelliteNumber prn = SatelliteNumber.Parse(line.Substring(0, 3));
                        var val = Double.Parse(line.Substring(4));
                        SDWL_FCB.Add(prn, val);
                    }
                }

                //???NLFCB.txt 文件头注释采用什么星历和钟差估计的FCB
                string NLpath = Path.Combine(Setting.DataDirectory, @"GNSS\Attachment\2013001FCB\NLFCB.txt");
                using (var streamReader = new StreamReader(NLpath))
                {
                    var line = "";
                    Time time = Time.Default;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        SatelliteNumber prn = SatelliteNumber.Parse(line.Substring(0, 3));
                        var val = Double.Parse(line.Substring(4));
                        SDNL_FCB.Add(prn, val);
                    }
                }
                #endregion

                //???此处采用G01，应改为通用
                SatelliteNumber MaxPrn = SatelliteNumber.Parse("G01");

                #region MW值逐历元平滑
                List<SatelliteNumber> SatWithCycleSlips = new List<SatelliteNumber>();
                //epochInfo.GetCycleSlipedPrns();
                Dictionary<SatelliteNumber, double> Smooth_MWs = new Dictionary<SatelliteNumber, double>();
                foreach (var sat in epochInfo.TotalPrns)
                {
                    if (SatWithCycleSlips.Contains(sat))//主要是针对 1.首次观测到某颗卫星 2.某颗卫星出现在第二个弧段，两个弧段的MW不相等&& epochInfo.Time.Value.Minute== 21&& epochInfo.Time.Value.Second ==30
                    {
                        if (SUM_MWs.ContainsKey(sat)) SUM_MWs.Remove(sat);
                        continue;
                    }

                    if (SUM_MWs.ContainsKey(sat))//累加
                    {
                        //SUM_MWs[sat].sum += epochInfo.MWs[sat];
                        SUM_MWs[sat].sum += epochInfo[sat].MwCycle;
                        SUM_MWs[sat].total += 1;
                    }
                    else //第一个没有周跳的历元
                    {
                        MWInfo tmpmw = new MWInfo();
                        //tmpmw.sum = epochInfo.MWs[sat];
                        tmpmw.sum = epochInfo[sat].MwCycle;
                        tmpmw.total = 1;
                        SUM_MWs.Add(sat, tmpmw);
                    }
                }

                //当前历元没有出现改课卫星时，要将其从SUM_MWs清除
                List<SatelliteNumber> tmpsats = new List<SatelliteNumber>(SUM_MWs.Keys);
                foreach (var sat in tmpsats)
                {
                    if (epochInfo.EnabledPrns.Contains(sat)) continue;
                    else
                    {
                        SUM_MWs.Remove(sat);
                    }
                }

                if (SUM_MWs.Count > 0)
                {

                    foreach (var sat in epochInfo.TotalPrns)
                    {
                        if (!SatWithCycleSlips.Contains(sat))
                        {
                            Smooth_MWs.Add(sat, SUM_MWs[sat].sum / SUM_MWs[sat].total);//宽巷模糊度的历元平滑值
                        }
                    }
                }
                #endregion
                #region 读取FCB文件
                //string path = "D:\\Data\\GNSS\\19240FCB\\sgg19240_igs_30.fcb";
                //FcbFileReader reader = new FcbFileReader(path);
                //FcbFile = reader.Read();

                ////对其到15min的FCB
                //double  tmp  = Math.Floor(epochInfo.Time.Value.SecondsOfDay / 900) * 900 + Math.Floor(epochInfo.Time.Value.SecondsOfWeek / 86400) *86400;
                //Time current = new Time(epochInfo.Time.Value.GpsWeek, tmp);

                //var data = FcbFile.FcbInfos;
                //var currentdata = data.FindAll(m => m.Time == current);
                #endregion
                #region 所有卫星的星间单差宽巷及窄巷FCB
                //选择高度角最大的那颗卫星

                //SatelliteNumber MaxPrn;
                //MaxPrn = epochInfo[0].Prn;
                //foreach (var item in epochInfo)
                //{
                //    if (epochInfo[MaxPrn].Polar.Elevation < item.Polar.Elevation)
                //    {
                //        MaxPrn = item.Prn;
                //    }
                //    else
                //    {
                //        continue;
                //    }
                //}
                ////及那颗卫星的FCB
                //double ReferenceNLFCB = 0.0;                
                //foreach(var item in currentdata)
                //{
                //    if(item.Prn == MaxPrn)
                //    {
                //        ReferenceNLFCB = item.Value;
                //    }
                //}
                ////星间单差FCB
                //Dictionary<SatelliteNumber, double> SDNL_FCB = new Dictionary<SatelliteNumber, double>();
                //foreach(var item in currentdata)
                //{
                //    SDNL_FCB.Add(item.Prn, item.Value - ReferenceNLFCB);
                //}

                //var wl = FcbFile.Header.WideLaneValue;
                //double ReferenceWLFCB = 0.0;
                //foreach (var item in wl.Data)
                //{
                //    if (item.Key == MaxPrn)
                //    {
                //        ReferenceWLFCB = item.Value.Value;
                //    }
                //}
                ////星间单差宽巷FCB
                //Dictionary<SatelliteNumber, double> SDWL_FCB = new Dictionary<SatelliteNumber, double>();
                //foreach (var item in wl.Data)
                //{
                //    SDWL_FCB.Add(item.Key, item.Value.Value - ReferenceWLFCB);
                //}
                #endregion


                #region 星间单差宽巷模糊度//星间单差宽巷模糊度,减去参考星,不包括出现周跳的卫星
                Dictionary<SatelliteNumber, double> SD_MW = new Dictionary<SatelliteNumber, double>();
                foreach (var item in Smooth_MWs)
                {
                    if (item.Key == MaxPrn) continue;
                    SD_MW.Add(item.Key, item.Value - Smooth_MWs[MaxPrn]);
                }
                #endregion

                #region 对MW值（宽巷模糊度）进行宽巷FCB纠正，注意是-,是减
                Dictionary<SatelliteNumber, double> correctedSD_MW = new Dictionary<SatelliteNumber, double>();
                foreach (var item in SD_MW)
                {
                    correctedSD_MW.Add(item.Key, item.Value - SDWL_FCB[item.Key]); //改正WLFCB
                }
                Dictionary<SatelliteNumber, double> fixedSD_MW = new Dictionary<SatelliteNumber, double>();
                foreach (var item in correctedSD_MW)
                {
                    if ((Math.Abs(correctedSD_MW[item.Key] - Math.Round(correctedSD_MW[item.Key])) <= 0.25))  //NLFCBs=0代表无效
                    {
                        fixedSD_MW.Add(item.Key, Math.Round(correctedSD_MW[item.Key]));
                    }
                }
                #endregion

                #region 对窄巷模糊度进行NLFCB改正,注意是-
                Dictionary<SatelliteNumber, double> floatAmb = new Dictionary<SatelliteNumber, double>();//存储无电离层浮点模糊度,有参考星 
                for (int i = 5; i < this.Adjustment.ParamCount; i++)
                {
                    SatelliteNumber sat = SatelliteNumber.Parse(this.Adjustment.ParamNames[i].Substring(0, 3));
                    //if (SatWithCycleSlips.Contains(sat)) continue;
                    floatAmb.Add(sat, this.Adjustment.Estimated[i, 0]);//估值   
                }
                Dictionary<SatelliteNumber, double> SD_floatAmb = new Dictionary<SatelliteNumber, double>();//存储星间单差无电离层浮点模糊度,删除参考星,单位：周
                foreach (var sat in floatAmb.Keys)
                {
                    if (sat == MaxPrn) continue;
                    SD_floatAmb.Add(sat, (floatAmb[sat] - floatAmb[MaxPrn]) / GnssConst.GPS_L1_WAVELENGTH);//
                }

                Dictionary<SatelliteNumber, double> SDNL_floatAmb = new Dictionary<SatelliteNumber, double>();//存储窄巷浮点模糊度,没有参考星，注意是- ??????????   
                foreach (var sat in fixedSD_MW.Keys)
                {
                    if (SDNL_FCB[sat] != 0)
                    {
                        double tmpnl = SD_floatAmb[sat] * (1575.42 + 1227.60) / 1575.42 - fixedSD_MW[sat] * 1227.60 / (1575.42 - 1227.60) - SDNL_FCB[sat];
                        SDNL_floatAmb.Add(sat, tmpnl);
                    }

                }
                #endregion

                #region LAMBDA固定窄巷模糊度
                if (SDNL_floatAmb.Count >= 4) //模糊度维度>4的情况才进行固定
                {
                    #region 星间单差窄巷模糊度协方差矩阵
                    IMatrix B = new ArrayMatrix(this.Adjustment.ParamNames.Count - 6, this.Adjustment.ParamNames.Count - 5, 0.0); //将非差模糊度转换为星间单差模糊度的旋转矩阵
                    int index = 0;//参考星的索引位置???,参考星不同，index不同
                    for (int i = 5; i < this.Adjustment.ParamNames.Count; i++)
                    {
                        SatelliteNumber sat = SatelliteNumber.Parse(this.Adjustment.ParamNames[i].Substring(0, 3));
                        if (sat == MaxPrn)
                        {
                            break;
                        }
                        else
                        {
                            index++;
                        }

                    }
                    for (int i = 0; i < this.Adjustment.ParamNames.Count - 6; i++)
                    {
                        if (i < index)
                        {
                            B[i, index] = -1; B[i, i] = 1;
                        }
                        if (i >= index)
                        {
                            B[i, index] = -1; B[i, i + 1] = 1;
                        }
                    }

                    IMatrix BT = B.Transposition;
                    IMatrix covaAmb = new ArrayMatrix(this.Adjustment.ParamNames.Count - 5, this.Adjustment.ParamNames.Count - 5, 0); //非差模糊度的协方差矩阵
                    for (int i = 0; i < this.Adjustment.ParamNames.Count - 5; i++)
                    {
                        for (int j = 0; j < this.Adjustment.ParamNames.Count - 5; j++)
                        {
                            covaAmb[i, j] = this.Adjustment.CovaOfEstimatedParam[i + 5, j + 5];
                        }
                    }
                    IMatrix SD_covaAmb = B.Multiply(covaAmb).Multiply(BT); //星间单差模糊度的协方差矩阵
                    IMatrix SDNL_covaAmb = new ArrayMatrix(this.Adjustment.ParamNames.Count - 6, this.Adjustment.ParamNames.Count - 6, 0.0); //星间单差窄巷模糊度的协方差矩阵
                    for (int i = 0; i < this.Adjustment.ParamNames.Count - 6; i++)
                    {
                        for (int j = 0; j < this.Adjustment.ParamNames.Count - 6; j++)
                        {
                            SDNL_covaAmb[i, j] = SD_covaAmb[i, j] * (1575.42 + 1227.60) / (1575.42 * GnssConst.GPS_L1_WAVELENGTH);//系数
                        }
                    }
                    #endregion

                    #region 部分模糊度固定，先将可以固定的星间单差窄巷模糊度 及其 协方差矩阵挑出来
                    List<string> old_ambpara = new List<string>();
                    foreach (var item in SD_floatAmb.Keys)
                    {
                        old_ambpara.Add(item.ToString());//所有的模糊度（除了参考星）
                    }
                    List<string> new_ambpara = new List<string>();
                    foreach (var item in SDNL_floatAmb.Keys)
                    {
                        new_ambpara.Add(item.ToString());//可以固定的模糊度（宽巷 < 0.25 && NLFCB != 0）
                    }
                    IMatrix newtrix = NamedMatrix.GetNewMatrix(new_ambpara, old_ambpara, SDNL_covaAmb.Array); //将可以固定的模糊度的协方差矩阵挑出来
                    #endregion

                    #region LAMBDA星间单差窄巷模糊度固定
                    IMatrix narrowLaneAmbiguity = new ArrayMatrix(SDNL_floatAmb.Count, 1, 0);
                    int ii = 0;
                    foreach (var item in SDNL_floatAmb.Values)
                    {
                        narrowLaneAmbiguity[ii, 0] = item;
                        ii++;
                    }
                    #region 降维，但是存在相等就麻烦了
                    double[] a = narrowLaneAmbiguity.GetCol(0).OneDimArray;
                    double[][] Q = newtrix.Array;
                    LlyLambda lambda = new LlyLambda(new_ambpara.Count, 2, Q, a);//LAMBDA算法
                    double[] N1 = new double[a.Length * 2]; for (int i = 0; i < a.Length; i++) N1[i] = Math.Floor(a[i] + 0.5);
                    double[] s = new double[2];
                    int info = 0;
                    info = lambda.getLambda(ref N1, ref s);
                    double ratio = s[1] / s[0];
                    #endregion
                    #endregion

                    //宽巷模糊度和窄巷模糊度固定后反求出的无电离层组合模糊度
                    Dictionary<SatelliteNumber, double> fixedIonoAmb = new Dictionary<SatelliteNumber, double>();
                    #region 求固定后的无电离层模糊度

                    if (ratio > 3)
                    {
                        if (N1.Length == newtrix.RowCount * 2)//没有出现降维
                        {
                            List<SatelliteNumber> validsats = new List<SatelliteNumber>(SDNL_floatAmb.Keys);
                            for (int i = 0; i < newtrix.RowCount; i++)
                            {
                                SatelliteNumber sat = validsats[i];
                                double currentWL = fixedSD_MW[sat];//该卫星的宽巷,注意是整数
                                double currentNL = N1[i];//该卫星的窄巷,注意是整数
                               // double freqOfN = 

                                double freqOfW = 1575.42 * 1227.60 / (1575.42 * 1575.42 - 1227.6 * 1227.60);
                                double freqOfN = 1575.42 / (1575.42 + 1227.60);
                                double currentIonoAmb = freqOfW * currentWL + freqOfN * currentNL + freqOfN * SDNL_FCB[sat];
                                fixedIonoAmb.Add(sat, currentIonoAmb);
                            }
                        }
                        IMatrix restrictMatrix = new ArrayMatrix(fixedIonoAmb.Count, this.Adjustment.ParamCount, 0); //约束矩阵
                        IMatrix W = new ArrayMatrix(fixedIonoAmb.Count, 1, 0);//W矩阵
                        ii = 0;
                        List<SatelliteNumber> totalsats = new List<SatelliteNumber>(epochInfo.EnabledPrns);
                        foreach (var sat in fixedIonoAmb.Keys)
                        {
                            int currentIndex = totalsats.IndexOf(sat);//获取索引值
                            restrictMatrix[ii, 5 + index] = -1;
                            restrictMatrix[ii, 5 + currentIndex] = 1;
                            W[ii, 0] = (SD_floatAmb[sat] - fixedIonoAmb[sat]) * GnssConst.GPS_L1_WAVELENGTH;
                            ii++;
                        }
                        #region 具有约束条件的参数平差 ???条件平差给不同的权，得到不同的结果
                        IMatrix A = restrictMatrix;
                        IMatrix AT = A.Transposition;
                        IMatrix AQAT = A.Multiply(this.Adjustment.CovaOfEstimatedParam).Multiply(AT);
                        IMatrix Inv_AQAT = AQAT.GetInverse();
                        IMatrix tmp1 = this.Adjustment.CovaOfEstimatedParam.Multiply(AT).Multiply(Inv_AQAT);
                        IMatrix tmp2 = tmp1.Multiply(W);
                        IMatrix X1 = new ArrayMatrix(this.Adjustment.ParamNames.Count, 1, 0);
                        for (int i = 0; i < this.Adjustment.ParamNames.Count; i++)
                        {
                            X1[i, 0] = this.Adjustment.Estimated[i, 0];
                        }
                        IMatrix new1_Estimated = X1.Minus(tmp2);//估值

                        IMatrix tmp3 = tmp1.Multiply(A).Multiply(this.Adjustment.CovaOfEstimatedParam);
                        IMatrix new_CovaOfEstimatedParam = this.Adjustment.CovaOfEstimatedParam.Minus(tmp3);//协方差矩阵
                        IMatrix P2 = new ArrayMatrix(fixedIonoAmb.Count, fixedIonoAmb.Count, 0);
                        for (int kk = 0; kk < fixedIonoAmb.Count; kk++)
                        {
                            P2[kk, kk] = 5 * Math.Pow(10, 6); //给FCB的一个权值，暂定5*10^6
                        }
                        IMatrix m1 = P2.GetInverse();
                        IMatrix m2 = m1.Plus(AQAT);
                        IMatrix m3 = m2.GetInverse();
                        IMatrix J = this.Adjustment.CovaOfEstimatedParam.Multiply(AT).Multiply(m3);
                        IMatrix M4 = J.Multiply(W);
                        IMatrix new2_Estimated = this.Adjustment.Estimated;
                        IMatrix new2_CovaOfEstimatedParam = this.Adjustment.CovaOfEstimatedParam;
                        if (Math.Sqrt(M4[0, 0] * M4[0, 0] + M4[1, 0] * M4[1, 0] + M4[2, 0] * M4[2, 0]) < 0.001)
                        {
                            new2_Estimated = this.Adjustment.Estimated;
                            new2_CovaOfEstimatedParam = this.Adjustment.CovaOfEstimatedParam;
                        }
                        else
                        {
                            new2_Estimated = X1.Minus(M4);
                            IMatrix m5 = J.Multiply(A).Multiply(this.Adjustment.CovaOfEstimatedParam);
                            new2_CovaOfEstimatedParam = this.Adjustment.CovaOfEstimatedParam.Minus(m5);
                        }
                        this.Adjustment.Estimated_PPPAR1 = new WeightedVector(new1_Estimated, new_CovaOfEstimatedParam);
                        this.Adjustment.Estimated_PPPAR2 = new WeightedVector(new2_Estimated, new2_CovaOfEstimatedParam);
                        #endregion
                    }
                    #endregion
                }
                #endregion

            }
            #endregion
            return result;
        }

        /// <summary>
        /// 模糊度长度固定部分。单位周
        /// </summary>
        /// <param name="intOfWL"></param>
        /// <param name="intOfNL"></param>
        /// <param name="fractionOfNL"></param>
        /// <returns></returns>
        public double GetFixedPppAmbi(double intOfWL, double intOfNL, double fractionOfNL)
        {
            double f1 = Frequence.GpsL1.Value;
            double f2 = Frequence.GpsL2.Value;
            var b = f1 / (f1 + f2) * (intOfNL - fractionOfNL) + f1 * f2 / (f1 * f1 - f2 * f2) * intOfWL;
            return b;

        }
        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            return new PppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }

}