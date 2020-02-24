//2014.08.30, czs, edit, 开始重构
//2014.08.31, czs, edit, 重构于 西安 到 沈阳 的航班上，春秋航空。
//2015.10.25, czs, eidt in pengzhou, Ppp 重命名为 IonoFreePpp
//2016.01.31, czs, edit in hongqing, 修复精度，简化流程
//2016.03.10, czs, edit in hongqing, 重构设计
//2018.08.03, czs, edit in hmx, 动态定位实现

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
    public class IonoFreePppOld2 : SingleSiteGnssSolver
    {
        #region 构造函数

        /// <summary>
        /// 最简化的构造函数，可以多个定位器同时使用的参数，而不必多次读取
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public IonoFreePppOld2(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "无电离层组合PPP";
            this.BaseParamCount = 5;
            if (PositionOption.ApproxDataType == SatApproxDataType.ApproxPseudoRangeOfTriFreq || PositionOption.ApproxDataType == SatApproxDataType.ApproxPhaseRangeOfTriFreq)
            { this.MatrixBuilder = new IonoFreePppOfTriFreqMatrixBuilder(PositionOption); }
            else { this.MatrixBuilder = new IonoFreePppMatrixBuilder(PositionOption); }

            if (!Option.TopSpeedModel)
            {
                WideLaneBiasService = new WideLaneBiasService(Setting.GnsserConfig.IgnWideLaneFile);
                if (this.IsFixingAmbiguity)
                {
                    this.IsBaseSatelliteRequried = true;
                }
                SmoothMwProvider = new SmoothMwProvider(120, this.DataSourceContext.ObservationDataSource.ObsInfo.Interval * 5);
                if ( !this.Option.IsUseFixedParamDirectly  && (   this.IsFixingAmbiguity && this.DataSourceContext.GnsserFcbOfUpdService == null))
                {
                    throw new Exception("PPP模糊度固定，请设置FCB文件路径!");
                }
                NarrawLaneFcbService = this.DataSourceContext.GnsserFcbOfUpdService;// new FcbOfUpdService( Option.GnsserFcbFilePath);
            }
        }
        #endregion
        WideLaneBiasService WideLaneBiasService { get; set; }
        FcbOfUpdService NarrawLaneFcbService{ get; set; }

        SmoothMwProvider SmoothMwProvider { get; set; }


        /// <summary>
        /// 滤波计算
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="lastResult"></param>
        /// <returns></returns>
        public override SingleSiteGnssResult CaculateKalmanFilter(EpochInformation epochInfo, SingleSiteGnssResult lastResult)
        {
            //极速模式。
            if (Option.TopSpeedModel)
            {
                return base.CaculateKalmanFilter(epochInfo, lastResult);
            }

            epochInfo.RemoveIonoFreeUnavailable();
            if (epochInfo.Count < 2)
            {
                log.Error("卫星可用数量不足：" + epochInfo.Count);
                return null;
            }
            var result = base.CaculateKalmanFilter(epochInfo, lastResult) as PppResult;

            //外部模糊度文件直接固定
            if( Option.IsFixingAmbiguity && Option.IsUseFixedParamDirectly && File.Exists( Option.AmbiguityFilePath) && Option.IsUsingAmbiguityFile)
            {
                return result;
            }

            //var testBasePrn = new SatelliteNumber(12, SatelliteType.G);

            //平滑MW
            if (Option.IsFixingAmbiguity)
            {
                SmoothMwProvider.Add(epochInfo);
            }

            if (this.IsFixingAmbiguity
                //&& epochInfo.Contains(testBasePrn)
                && epochInfo.ReceiverTime.DateTime.TimeOfDay > TimeSpan.FromHours(2)
                )
            {

                //this.CurrentBasePrn = testBasePrn;

                var time = epochInfo.ReceiverTime;
                var SD_MW = SmoothMwProvider.GetDifferMwValue(CurrentBasePrn);//周为单位
                if (SD_MW.Count < 2)//MW精度不足
                {
                    return result;
                }

                var wmFcb = WideLaneBiasService.Get(time).GetMwDiffer(CurrentBasePrn);//用于模糊度固定，精度要求不高，一次任务获取一次即可

                SatWideNarrowValueManager wideNarrowValues = new SatWideNarrowValueManager(); //存储用于模糊度固定的宽窄项及对应的浮点产品
                //基本参数定义
                double maxMwRms = 0.3;         //MW 至少的平滑精度
                double maxDevOfInt = 0.2; //MW 取整时允许最大的偏差
                var f1 = Frequence.GetFrequenceA(CurrentBasePrn, time).Value;// 1575.42;
                var f2 = Frequence.GetFrequenceB(CurrentBasePrn, time).Value; //  1227.60;
                var wideWaveLen = Frequence.GetMwFrequence(CurrentBasePrn, time).WaveLength;
                var narrowWaveLen = Frequence.GetNarrowLaneFrequence(CurrentBasePrn, time).WaveLength;

                #region 对MW值（宽巷模糊度）进行固定 
                foreach (var item in SD_MW)
                {
                    //简单的质量控制
                    if (item.Value.Rms > maxMwRms && !Double.IsNaN(item.Value.Rms))
                    {
                        continue;
                    }
                    var f = wmFcb[item.Key]; //已经归算为和WM的定义相同，在[0-1]区间，满足 B=N+f, N=B-f.
                    if (f == double.NaN) { continue; }

                    var B = item.Value.Value;

                    var nearN = B - f;

                    var N = (int)Math.Round(nearN);//直接进行取整数 
                    if (Math.Abs(nearN - N) <= maxDevOfInt)  //fixedSD_MW=0代表无效
                    {
                        wideNarrowValues.GetOrCreate(item.Key).WideLane = new IntFractionNumber(f, N);//改正后的 WLFCB
                    }
                }
                #endregion

                //提取浮点解星间单差
                SetPppFloatAmbiguityLen(result, wideNarrowValues);

                #region 固定窄巷模糊度               //星间单差，窄巷模糊度     
                var tempCoeef = f2 / (f1 + f2);                          //算法1
                var tempCoeef2 = (f1 + f2) * 1e6 / GnssConst.LIGHT_SPEED;//算法2：单位转换,窄巷波长的倒数
                var tempCoeef3 = f2 / (f1 - f2);                         //算法2
                foreach (var kv in wideNarrowValues.Data)
                {
                    //  var nFcb = SDNL_FCB[kv];//获取窄巷的产品
                    var prn = kv.Key;
                    var satData = kv.Value;
                    var f = NarrawLaneFcbService.GetBsdOfNarrowLane(prn, CurrentBasePrn, time); //获取窄巷的产品f, 符合B=N+f
                    if (!RmsedNumeral.IsValid(f)) { continue; }

                    var wideInt = satData.WideLane.Int;//只需要整数部分，小数部分化为窄巷小数部分，不影响整体
                    var floatPppAmbiLen = satData.FloatAmbiguityLength;

                    var B = (floatPppAmbiLen - tempCoeef * wideWaveLen * wideInt) / narrowWaveLen;//算法1

                    #region 算法验证
                    var B2 = tempCoeef2 * floatPppAmbiLen - tempCoeef3 * wideInt;                 //算法2
                    var differ = B - B2;
                    if (differ > 0.0001)
                    {
                        int ii = 0;
                        throw new Exception("Error!");
                    }
                    #endregion

                    var nearN = B - f;
                    var N = (int)Math.Round(nearN.Value);//直接进行取整数 
                    if (Math.Abs(nearN.Value - N) <= maxDevOfInt)  //fixedSD_MW=0代表无效
                    {
                        satData.NarrowLane = new IntFractionNumber(f.Value, N);//改正后的 WLFCB
                    }
                }
                #endregion

                //提取协方差，采用Lambda去相关后固定模糊度

                #region LAMBDA固定窄巷模糊度 
                //提取已选择的模糊度协方差阵


                //#region 星间单差窄巷模糊度协方差矩阵
                //IMatrix B = new ArrayMatrix(this.Adjustment.ParamNames.Count - 6, this.Adjustment.ParamNames.Count - 5, 0.0); //将非差模糊度转换为星间单差模糊度的旋转矩阵
                //int index = 0;//参考星的索引位置???,参考星不同，index不同
                //for (int i = 5; i < this.Adjustment.ParamNames.Count; i++)
                //{
                //    SatelliteNumber sat = SatelliteNumber.Parse(this.Adjustment.ParamNames[i].Substring(0, 3));
                //    if (sat == MaxPrn)
                //    {
                //        break;
                //    }
                //    else
                //    {
                //        index++;
                //    }

                //}
                //for (int i = 0; i < this.Adjustment.ParamNames.Count - 6; i++)
                //{
                //    if (i < index)
                //    {
                //        B[i, index] = -1; B[i, i] = 1;
                //    }
                //    if (i >= index)
                //    {
                //        B[i, index] = -1; B[i, i + 1] = 1;
                //    }
                //}

                //IMatrix BT = B.Transposition;
                //IMatrix covaAmb = new ArrayMatrix(this.Adjustment.ParamNames.Count - 5, this.Adjustment.ParamNames.Count - 5, 0); //非差模糊度的协方差矩阵
                //for (int i = 0; i < this.Adjustment.ParamNames.Count - 5; i++)
                //{
                //    for (int j = 0; j < this.Adjustment.ParamNames.Count - 5; j++)
                //    {
                //        covaAmb[i, j] = this.Adjustment.CovaOfEstimatedParam[i + 5, j + 5];
                //    }
                //}
                //IMatrix SD_covaAmb = B.Multiply(covaAmb).Multiply(BT); //星间单差模糊度的协方差矩阵
                //IMatrix SDNL_covaAmb = new ArrayMatrix(this.Adjustment.ParamNames.Count - 6, this.Adjustment.ParamNames.Count - 6, 0.0); //星间单差窄巷模糊度的协方差矩阵
                //for (int i = 0; i < this.Adjustment.ParamNames.Count - 6; i++)
                //{
                //    for (int j = 0; j < this.Adjustment.ParamNames.Count - 6; j++)
                //    {
                //        SDNL_covaAmb[i, j] = SD_covaAmb[i, j] * (1575.42 + 1227.60) / (1575.42 * GnssConst.GPS_L1_WAVELENGTH);//系数
                //    }
                //}
                //#endregion

                //#region 部分模糊度固定，先将可以固定的星间单差窄巷模糊度 及其 协方差矩阵挑出来
                //List<string> old_ambpara = new List<string>();
                //foreach (var item in SD_floatAmb.Keys)
                //{
                //    old_ambpara.Add(item.ToString());//所有的模糊度（除了参考星）
                //}
                //List<string> new_ambpara = new List<string>();
                //foreach (var item in SDNL_floatAmb.Keys)
                //{
                //    new_ambpara.Add(item.ToString());//可以固定的模糊度（宽巷 < 0.25 && NLFCB != 0）
                //}
                //IMatrix newtrix = NamedMatrix.GetNewMatrix(new_ambpara, old_ambpara, SDNL_covaAmb.Array); //将可以固定的模糊度的协方差矩阵挑出来
                //#endregion






                #endregion


                StringBuilder sb = new StringBuilder();
                sb.Append("PPP模糊度固定，" + time + ", BasePrn : " + CurrentBasePrn + ", 其它：" + wideNarrowValues.Count + ", " + Geo.Utils.StringUtil.ToString(wideNarrowValues.Keys));

                log.Info(sb.ToString());

                //计算固定后的模糊度距离
                //已经固定的模糊度
                var fixedPppAmbi = new Dictionary<SatelliteNumber, double>();
                foreach (var kv in wideNarrowValues.Data)
                {
                    if (!kv.Value.IsValid)
                    {
                        continue;
                    }
                    var prn = kv.Key;
                    var satData = kv.Value;
                    var wideInt = satData.WideLane.Int;              //宽巷模糊度仍然采用整数
                    var floatNarrowAmbi = satData.NarrowLane.Value;  //浮点部分包括在窄巷硬件延迟中
                    var fixedAmbiLen = tempCoeef * wideWaveLen * wideInt + narrowWaveLen * floatNarrowAmbi;

                    satData.FixedAmbiguityLength = fixedAmbiLen;
                    //简单质量控制
                    if (satData.DifferOfAmbiguityLength < 0.5)
                    {
                        fixedPppAmbi[prn] = fixedAmbiLen;
                    }
                }

                #region 条件平差  模糊度固定
                return FixPppResult(result, fixedPppAmbi);
                #endregion
            }
            return result;
        }

        /// <summary>
        /// 模糊度固定解，条件平差。
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fixedPppAmbi"></param>
        private PppResult FixPppResult(PppResult result, Dictionary<SatelliteNumber, double> fixedPppAmbi)
        {
            if (fixedPppAmbi.Count < 2)
            {
                return result;
            }

            var floadParams = result.ResultMatrix.Estimated;
            var paramCount = floadParams.Count;
            int fixedCount = fixedPppAmbi.Count;
            Vector fixedVector = new Vector();//fixedCount + 1
            foreach (var item in fixedPppAmbi)
            {
                fixedVector.Add(item.Value, item.Key.ToString());
            }
            //构建控制阵
            Matrix constraintMatrix = new Matrix(fixedCount, paramCount);
            int baseColIndex = -1;// floadParams.ParamNames.IndexOf(this.CurrentBasePrn.ToString());
            for (int i = 5; i < paramCount; i++)
            {
                var sat = SatelliteNumber.Parse(floadParams.ParamNames[i]);
                if (sat == CurrentBasePrn)
                {
                    baseColIndex = i;
                    break;
                }
            }
            for (int colIndex = 5; colIndex < paramCount; colIndex++)
            {
                var sat = SatelliteNumber.Parse(floadParams.ParamNames[colIndex]);
                int rowIndex = fixedVector.ParamNames.IndexOf(sat.ToString());//列编号与固定值对应
                if (rowIndex == -1)
                {
                    continue;//没有，略过
                }
                constraintMatrix[rowIndex, baseColIndex] = -1;
                constraintMatrix[rowIndex, colIndex] = 1;
            }
            //条件平差
            AdjustObsMatrix obsMatrix = new AdjustObsMatrix();
            obsMatrix.SetCoefficient(constraintMatrix).SetObservation(floadParams).SetFreeVector(fixedVector);
            ConditionalAdjuster adjuster = new ConditionalAdjuster();
            var resultMatrix = adjuster.Run(obsMatrix);

            result.ResultMatrix.Estimated = resultMatrix.CorrectedObs;
            return result;
        }

        /// <summary>
        /// 提取浮点解星间单差
        /// </summary>
        /// <param name="result"></param>
        /// <param name="wideNarrowValues"></param>
        private void SetPppFloatAmbiguityLen(PppResult result, SatWideNarrowValueManager wideNarrowValues)
        {              
            var floatAmbLen = new Dictionary<SatelliteNumber, double>(); //存储星间单差无电离层浮点模糊度,略过参考星,单位：米
            var baseFloatAmbi = result.GetAmbiguityDistance(CurrentBasePrn);
            foreach (var prn in result.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }
                var floatAmbi = result.GetAmbiguityDistance(prn);  //浮点解，单位：米，是否应该需要周？ no need
                var differ = floatAmbi - baseFloatAmbi;
                wideNarrowValues.GetOrCreate(prn).FloatAmbiguityLength = differ;
            } 
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