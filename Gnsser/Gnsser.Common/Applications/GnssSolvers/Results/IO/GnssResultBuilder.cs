//2017.09.06, czs, create in honqing, GNSS 结果构造器，按照GNSS结构构建。
//2017.10.26, czs, edit in honging, 丰富内容，计划用于每次平差结果输出
//2018.10.16, czs, edit in hmx, 梳理了文件名称

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser;
using Geo.Referencing;
using Geo.Utils;
using Gnsser.Checkers;
using System.Linq;


namespace Gnsser
{

    /// <summary>
    /// GNSS 结果构造器，按照GNSS结构构建。
    /// </summary>
    public class GnssResultBuilder : AbstractBasicProcess
    {
        Log log = new Log(typeof(GnssResultBuilder));
        /// <summary>
        /// GNSS 结果构造器
        /// </summary> 
        /// <param name="context"></param>
        /// <param name="Option"></param>
        public GnssResultBuilder(GnssProcessOption Option, DataSourceContext context)
        {
            this.Context = context;
            this.TableTextManager = new ObjectTableManager(Option.OutputDirectory);
            this.AioAdjustFileBuilder = new AioAdjustFileBuilder(Option.OutputDirectory);
            this.AdjustEquationFileBuilder = new AdjustEquationFileBuilder(Option.OutputDirectory);
            this.Option = Option;
            this.OutputMinInterval = this.Option.OutputMinInterval;
            var fileName = Path.Combine(Setting.TempDirectory, Geo.Utils.DateTimeUtil.GetDateTimePathStringNow() + ".sp3");
            Sp3Writer = new Sp3Writer(fileName, null);
            ResultFileNameBuilder = new ResultFileNameBuilder(Option.OutputDirectory);
        }

        /// <summary>
        ///  GNSS 结果构造器
        /// </summary>
        /// <param name="TableTextManager"></param>
        /// <param name="AioAdjustFileBuilder"></param>
        /// <param name="Option"></param>
        /// <param name="context"></param>
        /// <param name="AdjustEquationFileBuilder"></param>
        public GnssResultBuilder(
            ObjectTableManager TableTextManager,
            AioAdjustFileBuilder AioAdjustFileBuilder,
            AdjustEquationFileBuilder AdjustEquationFileBuilder,
            GnssProcessOption Option,
            DataSourceContext context)
        {
            this.Context = context;
            this.AdjustEquationFileBuilder = AdjustEquationFileBuilder;
            this.TableTextManager = TableTextManager;
            this.AioAdjustFileBuilder = AioAdjustFileBuilder;
            this.Option = Option;
            this.OutputMinInterval = this.Option.OutputMinInterval;
            var fileName = Path.Combine(Setting.TempDirectory, Geo.Utils.DateTimeUtil.GetDateTimePathStringNow() + ".sp3");
            Sp3Writer = new Sp3Writer(fileName, null);
            PrevEpoch = Time.MinValue;

            this.EpochParamAnalyzer = new EpochParamAnalyzer(new List<string>(this.Option.AnalysisParamNames),
              this.Option.SequentialEpochCountOfAccuEval,
             this.Option.MaxDifferOfAccuEval, this.Option.MaxAllowedConvergenceTime,
             this.Option.KeyLabelCharCount, this.Option.MaxAllowedDifferAfterConvergence, this.Option.MaxAllowedRmsOfAccuEval);
            ResultFileNameBuilder = new ResultFileNameBuilder(Option.OutputDirectory);
        }

        #region 属性 

        ResultFileNameBuilder ResultFileNameBuilder { get; set; }
        /// <summary>
        /// 结果分析器
        /// </summary>
        public EpochParamAnalyzer EpochParamAnalyzer { get; set; }
        /// <summary>
        /// 表格输出管理器
        /// </summary>
        public ObjectTableManager TableTextManager { get; set; }

        /// <summary>
        /// GNSS计算选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 数据上下文
        /// </summary>
        public DataSourceContext Context { get; set; }
        /// <summary>
        /// 平差文件
        /// </summary>
        public AioAdjustFileBuilder AioAdjustFileBuilder { get; set; }
        /// <summary>
        /// 平差文件
        /// </summary>
        AdjustEquationFileBuilder AdjustEquationFileBuilder { get; set; }
        #endregion
        Sp3Writer Sp3Writer { get; set; }
        /// <summary>
        /// 上一个已经成功输出的历元
        /// </summary>
        private Time PrevEpoch { get; set; }
        /// <summary>
        /// 上一个结果
        /// </summary>
        public SimpleGnssResult PrevResult { get; set; }
        /// <summary>
        /// 输出最小间隔,单位：秒。
        /// </summary>
        public double OutputMinInterval { get; set; }

        #region 方法

        #region  文字信息描述
        /// <summary>
        /// 构建最终信息
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string BuildFinalInfo(SimpleGnssResult result)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--------------- 计算配置信息 -------------------");
            sb.Append(BuildOptionInfo(Option));
            sb.AppendLine("--------------- 测站详情 -------------------");
            sb.Append(BuildResultInfo(result));

            return sb.ToString();
        }
        /// <summary>
        /// 追加配置信息
        /// </summary> 
        public string BuildOptionInfo(GnssProcessOption Option)
        {
            StringBuilder sb = new StringBuilder();
            //配置选项
            AppendLine(sb, "平差方法", Option.AdjustmentType);
            AppendLine(sb, "定位类型", Option.PositionType);
            AppendLine(sb, "近似数据", Option.ApproxDataType);
            AppendLine(sb, "观测数据", Option.ObsDataType);
            AppendLine(sb, "最大均方差限差", Option.MaxStdDev);


            string str = "";
            foreach (var item in Option.CycleSlipDetectSwitcher)
            {
                if (item.Value)
                    str += item.Key + ", ";
            }
            AppendLine(sb, "周跳探测方法", Option.CycleSlipDetectSwitcher.Count == 0 ? "默认" : str);

            return sb.ToString();
        }

        /// <summary>
        /// 追加一个测量结果信息
        /// </summary>
        /// <param name="simpleResult"></param> 
        public string BuildResultInfo(SimpleGnssResult simpleResult)
        {
            StringBuilder sb = new StringBuilder();
            if (simpleResult is BaseGnssResult)
            {
                BaseGnssResult result = (BaseGnssResult)simpleResult;
                //数据源信息
                AppendLine(sb, "数据源", result.Material.Name);

                if (result.Material is EpochInformation)
                {
                    var epochInfo = result.Material as EpochInformation;
                    var ObsInfo = epochInfo.ObsInfo;
                    var SiteInfo = epochInfo.SiteInfo;
                    BuildObsInfo(sb, ObsInfo);
                    BuildSiteInfo(sb, SiteInfo);
                    BuildContextInfo(sb, Context);
                    BuildAdjustInfo(sb, result.ResultMatrix);

                }

                if (result.HasEstimatedXyz)
                {
                    AppendLine(sb, "最终估值坐标：", result.EstimatedXyz);
                    AppendLine(sb, "最终估值坐标RMS：", result.EstimatedXyzRms);
                    AppendLine(sb, "大地坐标：", result.GeoCoord);
                }

            }
            return sb.ToString();
        }

        /// <summary>
        /// 追加测站信息
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="Adjustment"></param>
        private void BuildAdjustInfo(StringBuilder sb, AdjustResultMatrix Adjustment)
        {
            sb.AppendLine("--------------  最后历元平差信息  ---------------");
            AppendLine(sb, "观测数量", Adjustment.ObsMatrix.ObsCount);
            AppendLine(sb, "参数数量", Adjustment.ParamCount);
            AppendLine(sb, "单位权中误差", Adjustment.StdDev);
            AppendLine(sb, "参数估值", Adjustment.Estimated);
        }
        /// <summary>
        /// 追加测站信息
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="SiteInfo"></param>
        private void BuildSiteInfo(StringBuilder sb, ISiteInfo SiteInfo)
        {
            AppendLine(sb, "接收机编号", SiteInfo.ReceiverNumber);
            AppendLine(sb, "接收机类型", SiteInfo.ReceiverType);
            AppendLine(sb, "接收机版本", SiteInfo.ReceiverVersion);
            AppendLine(sb, "测站标识名称", SiteInfo.SiteName);
            AppendLine(sb, "测站标识编号", SiteInfo.MarkerNumber);
            AppendLine(sb, "天线距离测站点偏差HEN", SiteInfo.Hen);
            AppendLine(sb, "近似坐标", SiteInfo.ApproxXyz);
        }

        /// <summary>
        /// 追加测站信息
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="context"></param>
        private void BuildContextInfo(StringBuilder sb, DataSourceContext context)
        {
            if (context == null) { return; }

            AppendLine(sb, "钟差", context.SimpleClockService);
            AppendLine(sb, "星历", context.EphemerisService);
            AppendLine(sb, "ERP", context.ErpDataService);
            AppendLine(sb, "天线", context.AntennaDataSource);

      

        }

        /// <summary>
        /// 追加观测信息
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="ObsInfo"></param>
        private void BuildObsInfo(StringBuilder sb, IObsInfo ObsInfo)
        {
            AppendLine(sb, "采样间隔（s）", ObsInfo.Interval);
            AppendLine(sb, "观测者", ObsInfo.Observer);
            AppendLine(sb, "观测机构", ObsInfo.ObserverAgence);
            AppendLine(sb, "观测时段", ObsInfo.TimePeriod);
            AppendLine(sb, "系统类型", Geo.Utils.EnumerableUtil.ToString<SatelliteType>(ObsInfo.SatelliteTypes));

            foreach (var item in ObsInfo.ObsCodeTypes)
            {
                AppendLine(sb, item.Key + "", Geo.Utils.EnumerableUtil.ToString(item.Value));
            }
            AppendLine(sb, "历元数量", ObsInfo.Count);
        }

        /// <summary>
        /// 追加一行。
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="title"></param>
        /// <param name="val"></param>
        private static void AppendLine(StringBuilder sb, string title, object val)
        {
            var str = val + "";
            if (val != null && !String.IsNullOrWhiteSpace(str))
            {
                sb.AppendLine(title + "：" + str);
            }
        }

        #endregion

        #region 逐历元输出信息
        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="result"></param>
        public void AddEpochResult(ISiteSatObsInfo epoch, SimpleGnssResult result)
        {
            //结果采样率过滤
            var interval = Math.Abs(epoch.ReceiverTime - PrevEpoch);

            if (interval < this.OutputMinInterval)
            {
                return;
            }
            PrevEpoch = epoch.ReceiverTime;

            //总开关
            if (!Option.IsOutputResult && !Option.IsOutputEpochResult) { return; }

            var Adjustment = result.ResultMatrix;
            var ParamNames = result.ParamNames;

            //各历元信息
            #region  默认逐历元输出

            //参数值
            if (Option.IsOutputEpochParam) AddEpochParam(epoch, result);

            // RMS
            if (Option.IsOutputEpochParamRms) AddParamRms(epoch, result);

            //钟跳
            if (Option.IsOutputJumpClockFile)
            {
                CheckOrAddClockJump(epoch, result);
            }

            //第二参数
            if (result.ResultMatrix.SecondEstimated != null)
            {
                AddSecondParam(epoch, result);
                AddSecondParamRms(epoch, result);
            }

            //DOP
            if (this.Option.IsOutputEpochDop)
            {
                AddDop(epoch, result);
            }

            //两路滤波，模糊度固定解
            AddAmbFixed(epoch, result);

            //动态定位，且坐标未固定，则添加
            if (this.Option.IsOutputEpochCoord || (this.Option.PositionType == PositionType.动态定位 && !this.Option.IsFixingCoord))
            {
                AddEpochCoord(epoch, result);
            }
            #endregion

            #region 选择性历元输出
            #region 平差文件
            if (this.Option.IsOutputAdjust)
            {
                AioAdjustFileBuilder.AddAdjustment(Adjustment);
            }
            if (this.Option.IsOutputObsEquation)
            {
                AdjustEquationFileBuilder.AddAdjustment(Adjustment);
            }
            if (this.Option.IsOutputAdjustMatrix)
            {
                if (results == null) { results = new List<SimpleGnssResult>(); }
                results.Add(result);
            }

            #endregion

            #region 电离层产品
            if (this.Option.IsOutputIono)
            {
                BuildIonoResult(epoch, Adjustment, ParamNames);
            }

            #endregion

            #region 对流层产品
            if (this.Option.IsOutputWetTrop && Geo.Utils.StringUtil.Contanis(ParamNames, Gnsser.ParamNames.WetTropZpd, true) && epoch.ReceiverTime.SecondsOfDay % 300 == 0)
            {
                var table = TableTextManager.GetOrCreate(epoch.Name + "_" + Gnsser.ParamNames.WetTropZpd);
                table.NewRow();
                table.AddItem("Epoch", epoch.ReceiverTime);

                EpochInformation epochInfo = epoch as EpochInformation;
                //var ionoResult = Adjustment.Estimated.GetAll(Gnsser.ParamNames.WetTrop);
                //天顶对流层总延迟,epochInfo[0].WetMap_ZTD=1
                table.AddItem("Trop1", epochInfo[0].AppriorTropDelay + epochInfo[0].WetMap_ZTD * result.ResultMatrix.Estimated[4, 0]);
                table.AddItem("Trop2", epochInfo[1].WetMap_ZTD * result.ResultMatrix.Estimated[4, 0]);
                table.AddItem("Trop3", result.ResultMatrix.Estimated[4, 0]);
                //epoch[0].AppriorTropDelay + epoch[0].WetMap_ZTD *
                //foreach (var item in ionoResult)
                //{
                //    table.AddItem(item.Key, item.Value.Value);
                //}
            }
            #endregion

            #region  历元卫星观测信息
            if (Option.IsOutputEpochSatInfo)
            {
                AddSiteEpochSatInfo(epoch);
            }
            #endregion
            #endregion

            if (Option.IsOutputObservation)
            {
                AddObservation(epoch, result);
            }
            if (Option.IsOutputResidual)
            {
                this.AddResidual(epoch, result);
            }

            //轨道产品输出
            if (result is IOrbitResult)
            {
                var ephs = ((IOrbitResult)result).EphemerisResults;

                Sp3Section sp3Records = new Sp3Section(ephs.First.Original.Time);
                foreach (var eph in ephs)
                {
                    sp3Records.Add(eph.Original.Prn, eph.Corrected);
                } 
                Sp3Writer.Write(sp3Records); 
            }


            this.PrevResult = result;
        }
        /// <summary>
        /// 平差矩阵
        /// </summary>
        List<SimpleGnssResult> results { get; set; }
        /// <summary>
        ///  写平差矩阵文本信息
        /// </summary> 
        public void WriteAdjustMatrixText()
        {
            if (results == null || results.Count == 0) { return; }
            Geo.Utils.FileUtil.CheckOrCreateDirectory(this.Option.OutputDirectory);
            var obj = results[results.Count - 1];
            var epoch = obj.ReceiverTime;
            var name = obj.Name;
            string marker = "_" + (int)DateTime.Now.TimeOfDay.TotalSeconds + "";
            string path = Path.Combine(this.Option.OutputDirectory, name + "_" + epoch.ToDateString() + "_平差矩阵" + marker + ".adjust.txt");



            //常数项，没有按照卫星对应输出。
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var result in results)
            {
                sb.AppendLine(result.ResultMatrix.ObsMatrix.ToReadableText());

                i++;
            }

            File.AppendAllText(path, sb.ToString());
            results.Clear();
        }

        /// <summary>
        /// 构建电离层输出表格
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="Adjustment"></param>
        /// <param name="ParamNames"></param>
        private void BuildIonoResult(ISiteSatObsInfo epoch, Geo.Algorithm.Adjust.AdjustResultMatrix Adjustment, List<string> ParamNames)
        {
            if (epoch is EpochInformation) //这里只处理单站单历元情况
            {
                var epochInfo = epoch as EpochInformation;
                //电离层汇总
                var allInOneTable = TableTextManager.GetOrCreate(epoch.Name + "_All_" + Gnsser.ParamNames.Iono);
                allInOneTable.NewRow();
                allInOneTable.AddItem("Epoch", epoch.ReceiverTime);

                ObjectTableStorage tableIonoParam = null;
                ObjectTableStorage tableIf = null;
                ObjectTableStorage tableGridFile = null;
                ObjectTableStorage tableHarmoFile = null;
                ObjectTableStorage tableOfIonoParamService = null;

                //电离层参数
                if (Geo.Utils.StringUtil.Contanis(ParamNames, Gnsser.ParamNames.Iono, true))//参数化电离层文件
                {
                    tableIonoParam = TableTextManager.GetOrCreate(epoch.Name + "_Param_" + Gnsser.ParamNames.Iono);
                    tableIonoParam.NewRow();
                    tableIonoParam.AddItem("Epoch", epoch.ReceiverTime);

                    var ionoResult = Adjustment.Estimated.GetAll(Gnsser.ParamNames.Iono);
                    foreach (var item in ionoResult)
                    {
                        //斜距转换为垂距
                        //计算穿刺点
                        var prn = SatelliteNumber.Parse(item.Key);
                        var sat = epochInfo.Get(prn);
                        tableIonoParam.AddItem(prn.ToString(), item.Value.Value);
                    }
                }

                //双频电离层
                if (epochInfo.First.Count > 1)
                {
                    tableIf = TableTextManager.GetOrCreate(epoch.Name + "_IFofC_" + Gnsser.ParamNames.Iono);
                    tableIf.NewRow();
                    foreach (var sat in epochInfo.EnabledSats)
                    {
                        //斜距转换为垂距
                        //计算穿刺点
                        var prn = sat.Prn;
                        var ionXyz = sat.GetIntersectionXyz();
                        var geoCoordOfIono = CoordTransformer.XyzToGeoCoord(ionXyz);
                        var ionoFreeRange = sat.Combinations.IonoFreeRange.Value;
                        var rangeA = sat.FrequenceA.PseudoRange.Value;
                        var ionoError = rangeA - ionoFreeRange;

                        tableIf.AddItem("Epoch", epoch.ReceiverTime);
                        tableIf.AddItem(prn.ToString(), ionoError);
                    }
                }


                if (Context.IgsGridIonoFileService != null && Context.IgsGridIonoFileService.TimePeriod.Contains(epoch.ReceiverTime))
                {

                    tableGridFile = TableTextManager.GetOrCreate(epoch.Name + "_Grid_" + Gnsser.ParamNames.Iono);
                    tableGridFile.NewRow();
                    foreach (var sat in epochInfo.EnabledSats)
                    {
                        //斜距转换为垂距
                        //计算穿刺点
                        var prn = sat.Prn;
                        tableGridFile.AddItem("Epoch", epoch.ReceiverTime);

                        double val = IonoGridModelCorrector.GetGridModelCorrection(sat, FrequenceType.A, Context.IgsGridIonoFileService);
                        tableGridFile.AddItem(prn.ToString(), val);
                    }
                }

                if (Context.IgsCodeHarmoIonoFileService != null && Context.IgsCodeHarmoIonoFileService.TimePeriod.Contains(epoch.ReceiverTime))
                {

                    tableHarmoFile = TableTextManager.GetOrCreate(epoch.Name + "_Harmo_" + Gnsser.ParamNames.Iono);
                    tableHarmoFile.NewRow();
                    foreach (var sat in epochInfo.EnabledSats)
                    {
                        //斜距转换为垂距
                        //计算穿刺点
                        var prn = sat.Prn;
                        tableHarmoFile.AddItem("Epoch", epoch.ReceiverTime);

                        double val = IonoGridModelCorrector.GetGridModelCorrection(sat, FrequenceType.A, Context.IgsCodeHarmoIonoFileService);
                        tableHarmoFile.AddItem(prn.ToString(), val);
                    }
                }
                if (Context.IonoKlobucharParamService != null)
                {
                    var ionoParam = Context.IonoKlobucharParamService.Get(epoch.ReceiverTime);//
                    if (ionoParam != null)
                    {
                        tableOfIonoParamService = TableTextManager.GetOrCreate(epoch.Name + "_ParamModel_" + Gnsser.ParamNames.Iono);
                        tableOfIonoParamService.NewRow();
                        foreach (var sat in epochInfo.EnabledSats)
                        {
                            tableOfIonoParamService.AddItem("Epoch", epoch.ReceiverTime);

                            var val = IonoParamModelCorrector.GetCorrectorInDistance(sat, ionoParam);

                            tableOfIonoParamService.AddItem(sat.Prn.ToString(), val);
                        }
                    }
                }


                //保存到总表中
                foreach (var sat in epochInfo.EnabledSats)
                {
                    var prn = sat.Prn;
                    CheckAndAddIonoValueToMainTable(allInOneTable, tableIonoParam, prn, "Param");
                    CheckAndAddIonoValueToMainTable(allInOneTable, tableIf, prn, "IfofC");
                    CheckAndAddIonoValueToMainTable(allInOneTable, tableGridFile, prn, "Grid");
                    CheckAndAddIonoValueToMainTable(allInOneTable, tableHarmoFile, prn, "Harmo");
                    CheckAndAddIonoValueToMainTable(allInOneTable, tableOfIonoParamService, prn, "ParamModel");
                }
            }
        }

        internal void Clear()
        {
            this.TableTextManager.Clear();
            this.AioAdjustFileBuilder.Clear();
            this.AdjustEquationFileBuilder.Clear();
        }

        /// <summary>
        /// 保存结果到总表中
        /// </summary>
        /// <param name="allInOneTable"></param>
        /// <param name="tableIonoParam"></param>
        /// <param name="prn"></param>
        /// <param name="typeName"></param>
        private static void CheckAndAddIonoValueToMainTable(ObjectTableStorage allInOneTable, ObjectTableStorage tableIonoParam, SatelliteNumber prn, string typeName)
        {
            if (tableIonoParam != null)
            {
                if (tableIonoParam.CurrentRow.ContainsKey(prn.ToString()))
                {
                    allInOneTable.AddItem(prn + "_" + typeName, tableIonoParam.CurrentRow[prn.ToString()]);
                }
            }
        }

        #region 具体实现
        /// <summary>
        /// 添加测站卫星信息
        /// </summary>
        /// <param name="epoch"></param>
        private void AddSiteEpochSatInfo(ISiteSatObsInfo epoch)
        {
            EpochInformation epochInfo = null;
            if (epoch is EpochInformation)
            {
                epochInfo = epoch as EpochInformation;
            }
            else if (epoch is MultiSiteEpochInfo)
            {
                epochInfo = (epoch as MultiSiteEpochInfo).OtherEpochInfo;
            }
            else if (epoch is MultiSitePeriodInfo)
            {
                epochInfo = (epoch as MultiSitePeriodInfo)[0].OtherEpochInfo;
            }
            else if (epoch is PeriodInformation)
            {
                epochInfo = (epoch as PeriodInformation)[0];
            }


            if (epochInfo != null)
            {
                foreach (var sat in epochInfo.EnabledSats)
                {
                    var satTable = TableTextManager.GetOrCreate(epoch.Name + "_" + sat.Prn + "");
                    satTable.NewRow();
                    satTable.AddItem("Epoch", epoch.ReceiverTime);
                    var polar = sat.Polar;
                    satTable.AddItem("Elevation", polar.Elevation);
                    satTable.AddItem("Azimuth", polar.Azimuth);
                    satTable.AddItem("Distance", polar.Range);
                    foreach (var correction in sat.CommonCorrection.Corrections)
                    {
                        satTable.AddItem("通用_" + correction.Key, correction.Value);
                    }
                    foreach (var correction in sat.RangeOnlyCorrection.Corrections)
                    {
                        satTable.AddItem("仅伪距_" + correction.Key, correction.Value);
                    }
                    foreach (var correction in sat.PhaseOnlyCorrection.Corrections)
                    {
                        satTable.AddItem("仅载波距离_" + correction.Key, correction.Value);
                    }

                    foreach (var freq in sat)
                    {
                        foreach (var correction in freq.CommonCorrection.Corrections)
                        {
                            satTable.AddItem("频率_" + freq.FrequenceType + "_通用距离_" + correction.Key, correction.Value);
                        }
                        foreach (var correction in freq.RangeOnlyCorrection.Corrections)
                        {
                            satTable.AddItem("频率_" + freq.FrequenceType + "_仅伪距_" + correction.Key, correction.Value);
                        }
                        foreach (var correction in freq.PhaseOnlyCorrection.Corrections)
                        {
                            satTable.AddItem("频率_" + freq.FrequenceType + "_仅载波距离_" + correction.Key, correction.Value);
                        }

                        foreach (var obs in freq)
                        {
                            foreach (var subObs in obs)
                            {
                                foreach (var correction in subObs.Corrections)
                                {
                                    satTable.AddItem("观测值自身改正_频率_" + freq.FrequenceType + "_" + freq.PseudoRange.GnssCodeType + "_" + correction.Key, correction.Value);
                                }

                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 增加DOP结果
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="result"></param>
        private void AddDop(ISiteSatObsInfo epoch, SimpleGnssResult result)
        {
            //无测站信息，或固定参考站，
            if ((result is SingleSiteGnssResult) && result.ParamNames.Contains(Gnsser.ParamNames.Dx))
            {
                var singleSiteResult = result as SingleSiteGnssResult;
                var sysName = Geo.Utils.StringUtil.ToString(Option.SatelliteTypes, "_");
                var tableDOP = TableTextManager.GetOrCreate(epoch.Name + sysName + Setting.EpochDopFileExtension);
                tableDOP.NewRow();
                tableDOP.AddItem("Epoch", singleSiteResult.ReceiverTime);
                tableDOP.AddItem("TotalPrns", epoch.EnabledSatCount);
                tableDOP.AddItem("GDOP", singleSiteResult.DilutionOfPrecision.GDOP);
                tableDOP.AddItem("PDOP", singleSiteResult.DilutionOfPrecision.PDOP);
                tableDOP.AddItem("HDOP", singleSiteResult.DilutionOfPrecision.HDOP);
                tableDOP.AddItem("VDOP", singleSiteResult.DilutionOfPrecision.VDOP);
                tableDOP.EndRow();
            }
        }

        /// <summary>
        /// 模糊度固定解
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="result"></param>
        private void AddAmbFixed(ISiteSatObsInfo epoch, SimpleGnssResult result)
        {
            if (result.ResultMatrix.Estimated_PPPAR1 != null)
            {
                var tablenew = TableTextManager.GetOrCreate(epoch.Name + "PPPAR_Params");
                tablenew.NewRow();
                tablenew.AddItem("Epoch", epoch.ReceiverTime);//ToShortTimeString());
                tablenew.AddItem("dx", result.ResultMatrix.Estimated_PPPAR1[0]);
                tablenew.AddItem("dy", result.ResultMatrix.Estimated_PPPAR1[1]);
                tablenew.AddItem("dz", result.ResultMatrix.Estimated_PPPAR1[2]);
                tablenew.AddItem("clk", result.ResultMatrix.Estimated_PPPAR1[3]);
                tablenew.AddItem("trop", result.ResultMatrix.Estimated_PPPAR1[4]);
                tablenew.EndRow();

                var tablenew2 = TableTextManager.GetOrCreate(epoch.Name + "PPPAR_Params2");
                tablenew2.NewRow();
                tablenew2.AddItem("Epoch", epoch.ReceiverTime);//ToShortTimeString());
                tablenew2.AddItem("dx", result.ResultMatrix.Estimated_PPPAR2[0]);
                tablenew2.AddItem("dy", result.ResultMatrix.Estimated_PPPAR2[1]);
                tablenew2.AddItem("dz", result.ResultMatrix.Estimated_PPPAR2[2]);
                tablenew2.AddItem("clk", result.ResultMatrix.Estimated_PPPAR2[3]);
                tablenew2.AddItem("trop", result.ResultMatrix.Estimated_PPPAR2[4]);
                tablenew2.EndRow();
            }
        }   /// <summary>
            /// 增加参数RMS到存储表
            /// </summary>
            /// <param name="epoch"></param>
            /// <param name="result"></param>
        private void AddSecondParamRms(ISiteSatObsInfo epoch, SimpleGnssResult result)
        {
            var tableRms = TableTextManager.GetOrCreate(epoch.Name + Setting.EpochSecondParamRmsFileExtension);
            tableRms.NewRow();
            tableRms.AddItem("Epoch", epoch.ReceiverTime);

            tableRms.AddItem(result.ResultMatrix.StdOfSecondEstimatedParam);

            if (epoch.UnstablePrns.Count > 0 || epoch.RemovedPrns.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (epoch.UnstablePrns.Count > 0)
                    sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", epoch.UnstablePrns));

                if (epoch.RemovedPrns.Count > 0)
                    sb.Append(";" + String.Format(new EnumerableFormatProvider(), "{0}", epoch.RemovedPrns));

                tableRms.AddItem("CsOrRemoved", sb.ToString());
            }
            tableRms.EndRow();
        }
        /// <summary>
        /// 增加参数到存储表
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="result"></param>
        private void AddSecondParam(ISiteSatObsInfo epoch, SimpleGnssResult result)
        {
            var table = TableTextManager.GetOrCreate(epoch.Name + Setting.EpochSecondParamFileExtension );
            table.NewRow();
            table.AddItem("Epoch", epoch.ReceiverTime);//ToShortTimeString());

            //table.AddItem((IVector)result.Adjustment.Estimated);
            foreach (var item in result.ResultMatrix.SecondParamNames)
            {
                table.AddItem(item, result.ResultMatrix.SecondEstimated[item]);
            }
            if (epoch.UnstablePrns.Count > 0 || epoch.RemovedPrns.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (epoch.UnstablePrns.Count > 0)
                    sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", epoch.UnstablePrns));

                if (epoch.RemovedPrns.Count > 0)
                    sb.Append(";" + String.Format(new EnumerableFormatProvider(), "{0}", epoch.RemovedPrns));

                table.AddItem("CsOrRemoved", sb.ToString());
            }

            table.EndRow();


        }

        /// <summary>
        /// 添加钟跳结果
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="result"></param>
        private void CheckOrAddClockJump(ISiteSatObsInfo epoch, SimpleGnssResult result)
        {
            if (this.PrevResult == null) { return; }

            if (result is SingleSiteGnssResult && PrevResult is SingleSiteGnssResult)
            {
                var pre = PrevResult as SingleSiteGnssResult;
                var res = result as SingleSiteGnssResult;
                double delta = res.RcvClkError - pre.RcvClkError;

                if (Math.Abs(delta) > 1e-4 )
                {
                    log.Info(epoch .Name + "在 " + epoch.ReceiverTime +  "捕获到一个钟跳！与上历元偏差："+ delta + "，改正数(s)：" + res.RcvClkCorrection);

                    var table = TableTextManager.GetOrCreate(epoch.Name + "_ClockJump.cjump");
                    table.NewRow();
                    table.AddItem("Epoch", epoch.ReceiverTime);

                    table.AddItem("Value", delta.ToString("G9"));
                }                 
            }
        }

        /// <summary>
        /// 增加参数到存储表
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="result"></param>
        private void AddEpochParam(ISiteSatObsInfo epoch, SimpleGnssResult result)
        {
            var fileName = ResultFileNameBuilder.BuildEpochParamFileName(epoch.Name);
  


            var table = TableTextManager.GetOrCreate(fileName ); 
            table.NewRow();
            table.AddItem("Epoch", epoch.ReceiverTime);//ToShortTimeString());

            if (result is SingleSiteGnssResult 
                || result is IWithEstimatedBaselines
                || result is SingleSitePeriodInfoGnssResult 
                || result is IWithEstimatedBaseline)
            {
                XYZ appXyz = new XYZ();
                XYZ estXyz = new XYZ();
                if (result is SingleSiteGnssResult)
                {
                    var singleSiteResult = result as SingleSiteGnssResult;
                    appXyz = singleSiteResult.ApproxXyz;
                    estXyz = singleSiteResult.EstimatedXyz;
                }
                if (result is SingleSitePeriodInfoGnssResult)
                {
                    var period = result as SingleSitePeriodInfoGnssResult;
                    appXyz = period.MaterialObj[0].SiteInfo.ApproxXyz;
                }
                if (result is IWithEstimatedBaseline)
                {
                    var info = result as IWithEstimatedBaseline;
                    var baseLine = info.GetEstimatedBaseline();
                    estXyz = baseLine.EstimatedXyzOfRov;
                    appXyz = baseLine.ApproxXyzOfRov;
                }
                if (result is IWithEstimatedBaselines)
                {
                    var info = result as IWithEstimatedBaselines;
                    var baseLine = info.GetEstimatedBaselines().First;
                    estXyz = baseLine.EstimatedXyzOfRov;
                    appXyz = baseLine.ApproxXyzOfRov;
                }


                var dxyz = ((BaseGnssResult)result).XyzCorrection;// XYZ.Parse(result.Adjustment.Estimated);

                if (dxyz != null) //&& epoch.ReceiverTime.SecondsOfDay % 30 == 0
                {
                    ENU enu = null;
                    if (this.Option.IsUpdateEstimatePostition)
                    {
                        dxyz = estXyz - appXyz;

                        enu = CoordTransformer.XyzToEnu(estXyz, appXyz);
                    }
                    else
                    {
                        enu = CoordTransformer.LocaXyzToEnu(dxyz, appXyz);
                    }

                    table.AddItem(Gnsser.ParamNames.De, enu.E);
                    table.AddItem(Gnsser.ParamNames.Dn, enu.N);
                    table.AddItem(Gnsser.ParamNames.Du, enu.U);

                    if (this.Option.IsUpdateEstimatePostition || this.Option.PositionType == PositionType.动态定位)
                    { 
                        table.AddItem("EstX", estXyz.X);
                        table.AddItem("EstY", estXyz.Y);
                        table.AddItem("EstZ", estXyz.Z);
                    }

                }
            }
            
            #region 添加钟结果改正
            if (result is ClockEstimationResult)
            {
                var mEpochInfo = epoch as MultiSiteEpochInfo;
                List<string> paranames = result.ResultMatrix.ParamNames;
                int CountOfSat = epoch.EnabledSatCount * 2;
                foreach (var item in epoch.EnabledPrns)
                {
                    double qq = result.ResultMatrix.Corrected.CorrectedValue[paranames.IndexOf(item + "_" + ParamNames.SatClkErrDistance)];
                    table.AddItem(item.ToString(), qq);
                    int count = 0;
                    double time = 0;
                    Time EmissionSatClockTime = new Time();// Time.Default;
                    //Time EmissionSatClockTime1 = Time.Default;
                    foreach (var item2 in mEpochInfo)
                    {
                        foreach (var item3 in item2)
                        {
                            if (item3.Prn == item)
                            {
                                count++;
                                time += item3.Ephemeris.ClockBias - ((Ephemeris)item3.Ephemeris).RelativeCorrection;
                                //EmissionSatClockTime.TickTime.SecondTicks += item3.EmissionTime.TickTime.SecondTicks;

                                EmissionSatClockTime.TickTime += item3.EmissionTime.TickTime;
                                break;
                            }
                        }
                    }
                    double aa = qq / 0.3 + time * 1e9 / count;
                    double time1 = (EmissionSatClockTime.TickTime.SecondTicks + EmissionSatClockTime.TickTime.Fraction) / count;
                    EmissionSatClockTime.TickTime = SecondTime.FromSecond(time1);
                    table.AddItem(item + "EmissionSatClockTime", EmissionSatClockTime.ToString());
                    table.AddItem(item + "_Corrected", aa);
                }
            }
            #endregion

            //table.AddItem((IVector)result.Adjustment.Estimated);
            foreach (var name in result.ParamNames)
            {
                //if (item.Length == 6 && item.Substring(3, 3) == "_λN")
                //{
                //    table.AddItem(item.Substring(0, 3), result.Adjustment.Estimated[item]);
                //}
                //else
                {
                    table.AddItem(name, result.ResultMatrix.Estimated[name]);
                }
            }
            if (epoch.UnstablePrns.Count > 0 || epoch.RemovedPrns.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (epoch.UnstablePrns.Count > 0)
                    sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", epoch.UnstablePrns));

                if (epoch.RemovedPrns.Count > 0)
                    sb.Append(";" + String.Format(new EnumerableFormatProvider(), "{0}", epoch.RemovedPrns));

                table.AddItem("CsOrRemoved", sb.ToString());
            }

            table.AddItem(ParamNames.ResultType, result.ResultMatrix.ResultType);
            table.AddItem(ParamNames.StdDev, result.ResultMatrix.StdDev);

            table.EndRow();
        }
        
        /// <summary>
        /// 增加参数RMS到存储表
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="result"></param>
        private void AddParamRms(ISiteSatObsInfo epoch, SimpleGnssResult result)
        {
            var fileName = ResultFileNameBuilder.BuildEpochParamRmsFileName(epoch.Name);
            var tableRms = TableTextManager.GetOrCreate(fileName);
            tableRms.NewRow();
            tableRms.AddItem("Epoch", epoch.ReceiverTime);

            if (result is SingleSiteGnssResult || result is SingleSitePeriodInfoGnssResult)
            {
                if (result.ParamNames.Contains( ParamNames.Dx))
                {
                    XYZ xyz = new XYZ();
                    CovaedXyz covaedXyz = null;
                    if (result is SingleSiteGnssResult)
                    {
                        var singleSiteResult = result as SingleSiteGnssResult;
                        xyz = singleSiteResult.ApproxXyz;
                        covaedXyz = singleSiteResult.CovaedEstXyz;
                    }
                    if (result is SingleSitePeriodInfoGnssResult)
                    {
                        var period = result as SingleSitePeriodInfoGnssResult;
                        xyz = period.MaterialObj[0].SiteInfo.ApproxXyz;
                        covaedXyz = period.CovaedEstXyz;
                    }
                    var dxyz = ((BaseGnssResult)result).EstimatedXyzRms;// result.EstimatedXyzRms;// XYZ.Parse(result.Adjustment.Estimated);
                    if (dxyz != null && dxyz != XYZ.Zero)
                    {
                      // var tmsenu = CoordTransformer.LocaXyzToEnu(dxyz, xyz);
                         var cova = ((BaseGnssResult)result).CovaOfFirstThree;
                        var tmsenu = CoordTransformer.XyzToEnuRms(cova, xyz);

                        tableRms.AddItem(Gnsser.ParamNames.De, Math.Abs(tmsenu.E));
                        tableRms.AddItem(Gnsser.ParamNames.Dn, Math.Abs(tmsenu.N));
                        tableRms.AddItem(Gnsser.ParamNames.Du, Math.Abs(tmsenu.U));
                    }
                }
            }

            if (result is IWithEstimatedBaseline)
            {
                var info = result as IWithEstimatedBaseline;
                var baseLine = info.GetEstimatedBaseline();
                var xyz = baseLine.EstimatedXyzOfRov;
                var dxyz = ((BaseGnssResult)result).EstimatedXyzRms;

            //  var tmsenu = CoordTransformer.LocaXyzToEnu(dxyz, xyz);

                var cova = ((BaseGnssResult)result).CovaOfFirstThree;
                var tmsenu = CoordTransformer.XyzToEnuRms(cova, xyz);

                
                tableRms.AddItem(Gnsser.ParamNames.De, Math.Abs(tmsenu.E));
                tableRms.AddItem(Gnsser.ParamNames.Dn, Math.Abs(tmsenu.N));
                tableRms.AddItem(Gnsser.ParamNames.Du, Math.Abs(tmsenu.U));
            } 

            tableRms.AddItem(result.ResultMatrix.StdOfEstimatedParam);
            if (epoch.UnstablePrns.Count > 0 || epoch.RemovedPrns.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (epoch.UnstablePrns.Count > 0)
                    sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", epoch.UnstablePrns));

                if (epoch.RemovedPrns.Count > 0)
                    sb.Append(";" + String.Format(new EnumerableFormatProvider(), "{0}", epoch.RemovedPrns));

                tableRms.AddItem("CsOrRemoved", sb.ToString());
            }

            tableRms.AddItem(ParamNames.ResultType, result.ResultMatrix.ResultType);
            tableRms.AddItem(ParamNames.StdDev, result.ResultMatrix.StdDev);
            tableRms.EndRow();
        }

     
        /// <summary>
        /// 增加坐标到存储表，主要用于导航解算
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="gnssResult"></param>
        private void AddEpochCoord(ISiteSatObsInfo epoch, SimpleGnssResult gnssResult)
        {
            if (gnssResult is SingleSiteGnssResult)
            {
                var result = (SingleSiteGnssResult)gnssResult;
                var table = TableTextManager.GetOrCreate(epoch.Name + Setting.EpochCoordFileExtension);
                table.NewRow();
                table.AddItem("Epoch", epoch.ReceiverTime);//ToShortTimeString());

                var dxyz = result.XyzCorrection;// XYZ.Parse(result.Adjustment.Estimated);
                if (dxyz != null)
                {
                    var xyz = result.EstimatedXyz;
                    var geo = result.GeoCoord;
                    table.AddItem(Gnsser.ParamNames.X, xyz.X.ToString("0.0000"));
                    table.AddItem(Gnsser.ParamNames.Y, xyz.Y.ToString("0.0000"));
                    table.AddItem(Gnsser.ParamNames.Z, xyz.Z.ToString("0.0000"));
                    //精确到亚毫米
                    table.AddItem(Gnsser.ParamNames.Lon, geo.Lon.ToString("0.00000000"));
                    table.AddItem(Gnsser.ParamNames.Lat, geo.Lat.ToString("0.00000000"));
                    table.AddItem(Gnsser.ParamNames.Height, geo.Height.ToString("0.0000"));
                }
            }
            if (gnssResult is IEstimatedBaseline)
            {
                var result = (IEstimatedBaseline)gnssResult;
                var table = TableTextManager.GetOrCreate(epoch.Name + Setting.EpochCoordFileExtension);
                table.NewRow();
                table.AddItem("Epoch", epoch.ReceiverTime);//ToShortTimeString());

                var xyz = result.EstimatedXyzOfRov;//.EstimatedXyz;
                var geo = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(xyz);// result.GeoCoord;
                table.AddItem(Gnsser.ParamNames.X, xyz.X.ToString("0.0000"));
                table.AddItem(Gnsser.ParamNames.Y, xyz.Y.ToString("0.0000"));
                table.AddItem(Gnsser.ParamNames.Z, xyz.Z.ToString("0.0000"));
                //精确到亚毫米
                table.AddItem(Gnsser.ParamNames.Lon, geo.Lon.ToString("0.00000000"));
                table.AddItem(Gnsser.ParamNames.Lat, geo.Lat.ToString("0.00000000"));
                table.AddItem(Gnsser.ParamNames.Height, geo.Height.ToString("0.0000"));
            }
        }
        /// <summary>
        /// 输出观测残差
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="gnssResult"></param>
        private void AddObservation(ISiteSatObsInfo epoch, SimpleGnssResult gnssResult)
        {
            var fileName =  ResultFileNameBuilder.BuildEpochObsFileName(epoch.Name);
            var table = TableTextManager.GetOrCreate(fileName );
            table.NewRow();
            table.AddItem("Epoch", epoch.ReceiverTime);//ToShortTimeString());
            var names = gnssResult.ResultMatrix.ObsMatrix.Observation.ParamNames;
            int i = 0;
            var obs = gnssResult.ResultMatrix.ObsMatrix.Observation - gnssResult.ResultMatrix.ObsMatrix.FreeVector;
            // var obs = gnssResult.ResultMatrix.PostfitResidual;//.ObsMatrix.Observation - gnssResult.ResultMatrix.ObsMatrix.FreeVector;
            foreach (var item in obs)
            {
                string name = i + "";
                if(names != null) { name = names[i]??i+""; }
                table.AddItem(name, obs[i]);
                i++;
            } 
        }
        /// <summary>
        /// 输出算后残差
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="gnssResult"></param>
        private void AddResidual(ISiteSatObsInfo epoch, SimpleGnssResult gnssResult)
        {
            var fileName =  ResultFileNameBuilder.BuildEpochResidualFileName(epoch.Name);
            var table = TableTextManager.GetOrCreate(fileName );
            table.NewRow();
            table.AddItem("Epoch", epoch.ReceiverTime);//ToShortTimeString());
            var names = gnssResult.ResultMatrix.ObsMatrix.Observation.ParamNames;
            int i = 0;
            //var obs = gnssResult.ResultMatrix.ObsMatrix.Observation - gnssResult.ResultMatrix.ObsMatrix.FreeVector;
            var obs = gnssResult.ResultMatrix.PostfitResidual;//.ObsMatrix.Observation - gnssResult.ResultMatrix.ObsMatrix.FreeVector;
            foreach (var item in obs)
            {
                string name = i + "";
                if(names != null) { name = names[i]??i+""; }
                table.AddItem(name, obs[i]);
                i++;
            } 
        }
        /// <summary>
        /// run
        /// </summary>
        public override void Run()
        {
        }
        #endregion
        #endregion
        #endregion
        /// <summary>
        /// 完毕
        /// </summary>
        public override void Complete()
        {
            base.Complete();

            Sp3Writer.Flush();
            
            var keys = this.TableTextManager.Keys.FindAll(m => m.Contains(Setting.EpochParamFileExtension));
            if(keys.Count == 0) { return; }
            foreach (var item in keys)
            {
                var table = TableTextManager.Get(item);
                EpochParamAnalyzer.Add(table);
            }
            EpochParamAnalyzer.GetTotalFileParamConvergenceTable();

        } 
    }
}