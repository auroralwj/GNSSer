//2017.07.25, czs, create in hongqing, 定位报表生成器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO; 
using System.Text; 
using System.ComponentModel;
using System.Data;  
using Geo.Coordinates;
using Geo;
using Geo.Algorithm;
using Geo.IO;
using System.Net; 
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Algorithm.Adjust;

namespace Gnsser
{
    /// <summary>
    /// 报表的EL标签
    /// </summary>
    public class ReportElMarker
    {
        /// <summary>
        /// 题目
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        /// 设置信息
        /// </summary>
        public const string ConfigInfo = "ConfigInfo"; 
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string CalculationTime = "CalculationTime";
        /// <summary>
        /// 第一个表
        /// </summary>
        public const string FirstTable = "FirstTable";
        /// <summary>
        /// 第二个表
        /// </summary>
        public const string SecondTable = "SecondTable";
        /// <summary>
        /// 第二个表
        /// </summary>
        public const string SecondTableTitle = "SecondTableTitle";
        /// <summary>
        /// 内容区域
        /// </summary>
        public const string Content = "Content"; 

    }
    
    /// <summary>
    /// GNSS HTML 报表生成器
    /// </summary>
    public class HtmlReportBuilder   : AbstractBuilder<string>
    { 
        Log log = new Geo.IO.Log(typeof(HtmlReportBuilder));
        /// <summary>
        /// 报表生成器
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="context"></param>
        public HtmlReportBuilder(GnssProcessOption Option, DataSourceContext context)
        {
            this.Context = context;
            this.Option = Option;
            this.Results = new List<SimpleGnssResult>();
            this.Data = new System.Collections.Generic.Dictionary<string, string>();
            this.ResultBuilder = new GnssResultBuilder(Option, Context);
            SetValue(ReportElMarker.Title, "计算结果报表");
            OutputPath = Path.Combine(Option.OutputDirectory, "GNSSerResultReport_" + Geo.Utils.DateTimeUtil.GetDateTimePathStringNow()+".html");

            Geo.Utils.FileUtil.CheckOrCreateDirectory(Path.GetDirectoryName(OutputPath));
        }
        #region  属性
        /// <summary>
        /// 报表标题
        /// </summary>
        public string Title { get { return Data["Title"]; } set { SetValue("Title", value); } }
        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutputPath { get; set; }
        /// <summary>
        /// 结果生成器
        /// </summary>
        public GnssResultBuilder ResultBuilder { get; set; }
       /// <summary>
       /// 所有的结果，并不是一个文件的结果。
       /// </summary>
        public   List<SimpleGnssResult> Results { get; set; }

        public List<RmsedXYZ> GetRmsedXYZs()
        {
            List<RmsedXYZ> list = new List<RmsedXYZ>();

            if ( Results ==null) { return list; }

            foreach (var item in Results)
            {
                if (item is BaseGnssResult)
                {
                    var xyz = ((BaseGnssResult)item).EstRmsedXYZ;
                    list.Add(xyz);
                }

            }
            return list;
        }
        public List<SimplePositionResult> GetSimplePositionResults()
        {
            List<SimplePositionResult> list = new List<SimplePositionResult>();

            if ( Results ==null) { return list; }

            foreach (var item in Results)
            { 
                list.Add(((BaseGnssResult)item).GetSimplePositionResult());
            }
            return list;
        }
        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
        /// <summary>
        /// 模型路径
        /// </summary>
        string ModelPath { get { return Setting.GnsserConfig.PositionReportModel; } }
        /// <summary>
        /// 计算选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 数据上下文
        /// </summary>
        public DataSourceContext Context { get; set; }
        #endregion

        #region 方法
        private void BuildDic()
        {
            StringBuilder mainSb = new StringBuilder();
            mainSb.AppendLine("------------------------ 计算配置信息 --------------------- ");
            mainSb.AppendLine("共：" + Results.Count(m=>m!=null) + " 个计算结果。");
            mainSb.AppendLine(ResultBuilder.BuildOptionInfo(this.Option));

            SetValue(ReportElMarker.ConfigInfo, mainSb.ToString());
            SetValue(ReportElMarker.CalculationTime, Geo.Utils.DateTimeUtil.GetFormatedDateTimeNow());

            //First Table
            mainSb = new StringBuilder();
            if (Results.Count > 0)
            {
                var firstResult = Results.First();
                if (firstResult is SingleSiteGnssResult)
                {
                    BuildGeneraInfoTableOfSingleSite(mainSb, firstResult as SingleSiteGnssResult);
                }else if (firstResult is IWithEstimatedBaseline)
                {
                    BuildGeneraInfoTableOfBaseLine(mainSb, firstResult as IWithEstimatedBaseline);
                }else   if (firstResult is IWithEstimatedBaselines)
                {
                    BuildGeneraInfoTableOfMultiSites(mainSb, firstResult);
                }
            }
            SetValue(ReportElMarker.FirstTable, mainSb.ToString());

            //第二个表，外符合精度, 第二表格无存在作用了。
            mainSb = new StringBuilder();
           if(false) BuildAccuceEvalTableOfSingleSite(mainSb, Results);
            if (mainSb.Length > 0)
            {
                SetValue(ReportElMarker.SecondTableTitle, "精度估算(mm)");
            }
            else
            { 
                SetValue(ReportElMarker.SecondTableTitle, "");
            }
            SetValue(ReportElMarker.SecondTable, mainSb.ToString());


            mainSb = new StringBuilder();
            if (HasResults())
            {
                int i = 1;
                mainSb.AppendLine("------------------------ 测站信息 --------------------- ");
                foreach (var result in this.Results)
                {
                    if (result == null) { continue; }
                    mainSb.AppendLine(i + "");
                    var finalInfo = ResultBuilder.BuildResultInfo(result);
                    mainSb.AppendLine(finalInfo);
                    
                    if (result.HasParamAccuracyInfos)
                    {
                        mainSb.AppendLine("参数收敛时间与精度估计：");
                        mainSb.AppendLine(result.ParamAccuracyInfos.ToReadableText());
                    }

                    i++;
                }
            }
            this.SetValue(ReportElMarker.Content, mainSb.ToString());
        }

        #region 表格生成

        /// <summary>
        /// 单站结果
        /// </summary>
        /// <param name="mainSb"></param>
        /// <param name="Results"></param>
        private void BuildAccuceEvalTableOfSingleSite(StringBuilder mainSb, List<SimpleGnssResult> Results)
        {

            if (Results == null || Results.Count == 0) { return; }
            var first = Results[0];
            if (!first.HasParamAccuracyInfos) { return; }

            var sb = new StringBuilder();
            sb.Append(BuildTableHeaderCell("Num"));
            sb.Append(BuildTableHeaderCell("Name"));

            foreach (var kv in first.ParamAccuracyInfos.KeyValues)
            {
                sb.Append(BuildTableHeaderCell(kv.Key));
            }
            //sb.Append(BuildTableHeaderCell("IsOk"));

            var headerRow = BuildTableRow(sb.ToString());
            mainSb.Append(headerRow);

            sb = new StringBuilder();
            int i = 1;
            foreach (var result in Results)
            {
                if (result.HasParamAccuracyInfos)
                {
                    sb.Append(BuildTableCell(i + ""));

                    sb.Append(BuildTableCell(result.Name));

                    foreach (var kv in result.ParamAccuracyInfos.KeyValues)
                    {
                        var val = kv.Value.RmsValue.Value;
                        var isOk =  result.ParamAccuracyInfos.IsOk(kv.Value);
                        var str = (val * 1e3).ToString("0.000");
                        if (!isOk)
                        {
                            str = "<b class='red'>" + str + "</b>";
                        }
                        sb.Append(BuildTableCell(str));
                    }
                    var row = BuildTableRow(sb.ToString());
                    mainSb.Append(row);
                    sb.Clear();

                }
                i++;
            }
        }


        /// <summary>
        /// 单站结果
        /// </summary>
        /// <param name="mainSb"></param>
        /// <param name="firstObject"></param>
        private void BuildGeneraInfoTableOfSingleSite(StringBuilder mainSb, SingleSiteGnssResult firstObject)
        {
            var last = Results.Last();
            var sb = new StringBuilder();
            sb.Append(BuildTableHeaderCell("Num"));
            sb.Append(BuildTableHeaderCell("Name"));
            sb.Append(BuildTableHeaderCell("StdDev"));

            if (firstObject.EstimatedXyz != null)
            {
                sb.Append(BuildTableHeaderCell("X"));
                sb.Append(BuildTableHeaderCell("Y"));
                sb.Append(BuildTableHeaderCell("Z"));
                sb.Append(BuildTableHeaderCell("Lon"));
                sb.Append(BuildTableHeaderCell("Lat"));
                sb.Append(BuildTableHeaderCell("Height"));
                sb.Append(BuildTableHeaderCell("RmsE"));
                sb.Append(BuildTableHeaderCell("RmsN"));
                sb.Append(BuildTableHeaderCell("RmsU"));
            }

            if (last.HasParamAccuracyInfos )
            {
                foreach (var kv in firstObject.ParamAccuracyInfos.KeyValues)
                {
                    sb.Append(BuildTableHeaderCell(kv.Key));
                } 
            }


            sb.Append(BuildTableHeaderCell("Epoch"));

            var row = BuildTableRow(sb.ToString());
            mainSb.Append(row);

            var format = "G5";

            if (HasResults())
            {
                int i = 1;
                foreach (var item in this.Results)
                {
                    if (item == null) { continue; }
                    sb = new StringBuilder();
                    var result = item as SingleSiteGnssResult;
                    sb.Append(BuildTableCell(i));
                    sb.Append(BuildTableCell(result.Material.Name));
                    sb.Append(BuildTableCell(result.ResultMatrix.StdDev.ToString(format)));
                    if (result.EstimatedXyz != null)
                    {
                        sb.Append(BuildTableCell(result.EstimatedXyz.X.ToString("0.#####")));
                        sb.Append(BuildTableCell(result.EstimatedXyz.Y.ToString("0.#####")));
                        sb.Append(BuildTableCell(result.EstimatedXyz.Z.ToString("0.#####")));
                        sb.Append(BuildTableCell(new DMS(result.GeoCoord.Lon).ToReadableDms()));
                        sb.Append(BuildTableCell(new DMS(result.GeoCoord.Lat).ToReadableDms()));
                        sb.Append(BuildTableCell(result.GeoCoord.Height.ToString("0.#####")));

                        var tmsenu = CoordTransformer.LocaXyzToEnu(result.EstimatedXyzRms, result.ApproxXyz);
                        sb.Append(BuildTableCell(Math.Abs(tmsenu.E).ToString(format)));
                        sb.Append(BuildTableCell(Math.Abs(tmsenu.N).ToString(format)));
                        sb.Append(BuildTableCell(Math.Abs(tmsenu.U).ToString(format)));
                    }
                    if (last.HasParamAccuracyInfos)
                    { 
                        foreach (var kv in result.ParamAccuracyInfos.KeyValues)
                        {
                            var val = kv.Value.RmsValue.Value;
                            var isOk = result.ParamAccuracyInfos.IsOk(kv.Value);
                            var str = (val * 1e3).ToString("0.000");
                            if (!isOk)
                            {
                                str = "<b class='red'>" + str + "</b>";
                            }
                            sb.Append(BuildTableCell(str));
                        }
                    }

                    sb.Append(BuildTableCell(result.ReceiverTime));

                    var rowContent = BuildTableRow(sb.ToString());
                    mainSb.Append(rowContent);
                    mainSb.AppendLine();
                    i++;
                }
            }
        }
        /// <summary>
        /// 双站单基线结果
        /// </summary>
        /// <param name="mainSb"></param>
        /// <param name="firstObject"></param>
        private void BuildGeneraInfoTableOfBaseLine(StringBuilder mainSb, IWithEstimatedBaseline firstObject)
        {
            var last = Results.Last();
            var sb = new StringBuilder();
            sb.Append(BuildTableHeaderCell("Num"));
            sb.Append(BuildTableHeaderCell("Name"));
            sb.Append(BuildTableHeaderCell("StdDev"));

            sb.Append(BuildTableHeaderCell( ParamNames.ResultType));
            sb.Append(BuildTableHeaderCell("X"));
            sb.Append(BuildTableHeaderCell("Y"));
            sb.Append(BuildTableHeaderCell("Z"));
            sb.Append(BuildTableHeaderCell("Lon"));
            sb.Append(BuildTableHeaderCell("Lat"));
            sb.Append(BuildTableHeaderCell("Height"));
            sb.Append(BuildTableHeaderCell("RmsE"));
            sb.Append(BuildTableHeaderCell("RmsN"));
            sb.Append(BuildTableHeaderCell("RmsU"));
            if (last.HasParamAccuracyInfos)
            {
                foreach (var kv in last.ParamAccuracyInfos.KeyValues)
                {
                    sb.Append(BuildTableHeaderCell(kv.Key));
                }
            }
            sb.Append(BuildTableHeaderCell("Epoch"));

            var row = BuildTableRow(sb.ToString());
            mainSb.Append(row);

            var format = "G5";

            if (HasResults())
            {
                int i = 1;
                foreach (var result in this.Results)
                {
                    if (result == null) { continue; }
                    sb = new StringBuilder();
                    var baselineObj = result as IWithEstimatedBaseline;
                    var baseline = baselineObj.GetEstimatedBaseline();

                    sb.Append(BuildTableCell(i));
                    sb.Append(BuildTableCell(result.Name));
                    sb.Append(BuildTableCell(result.ResultMatrix.StdDev.ToString(format)));
                    sb.Append(BuildTableCell(result.ResultMatrix.ResultType.ToString()));
                    var EstimatedXyz = baseline.EstimatedRmsXyzOfRov.Value;
                    sb.Append(BuildTableCell(EstimatedXyz.X.ToString("0.#####")));
                    sb.Append(BuildTableCell(EstimatedXyz.Y.ToString("0.#####")));
                    sb.Append(BuildTableCell(EstimatedXyz.Z.ToString("0.#####")));

                    var geoCoord = baseline.EstimatedGeoCoordOfRov;
                    sb.Append(BuildTableCell(new DMS(geoCoord.Lon).ToReadableDms()));
                    sb.Append(BuildTableCell(new DMS(geoCoord.Lat).ToReadableDms()));
                    sb.Append(BuildTableCell(geoCoord.Height.ToString("0.#####")));

                    var tmsenu = CoordTransformer.LocaXyzToEnu(baseline.EstimatedRmsXyzOfRov.Rms, baseline.ApproxXyzOfRov);
                    sb.Append(BuildTableCell(Math.Abs(tmsenu.E).ToString(format)));
                    sb.Append(BuildTableCell(Math.Abs(tmsenu.N).ToString(format)));
                    sb.Append(BuildTableCell(Math.Abs(tmsenu.U).ToString(format)));
                    if (last.HasParamAccuracyInfos)
                    {
                        foreach (var kv in result.ParamAccuracyInfos.KeyValues)
                        {
                            var val = kv.Value.RmsValue.Value;
                            var isOk = result.ParamAccuracyInfos.IsOk(kv.Value);
                            var str = (val * 1e3).ToString("0.000");
                            if (!isOk)
                            {
                                str = "<b class='red'>" + str + "</b>";
                            }
                            sb.Append(BuildTableCell(str));
                        }
                    }

                    sb.Append(BuildTableCell(result.ReceiverTime));

                    var rowContent = BuildTableRow(sb.ToString());
                    mainSb.Append(rowContent);
                    mainSb.AppendLine();
                    i++;
                }
            }
        }
        /// <summary>
        /// 多站结果
        /// </summary>
        /// <param name="mainSb"></param>
        /// <param name="firstObject"></param>
        private void BuildGeneraInfoTableOfMultiSites(StringBuilder mainSb, SimpleGnssResult firstObject)
        {
            var last = Results.Last();
            var sb = new StringBuilder();
            sb.Append(BuildTableHeaderCell("Num"));
            sb.Append(BuildTableHeaderCell("Name"));
            sb.Append(BuildTableHeaderCell("StdDev"));
            sb.Append(BuildTableHeaderCell(ParamNames.ResultType));

            sb.Append(BuildTableHeaderCell("X"));
            sb.Append(BuildTableHeaderCell("Y"));
            sb.Append(BuildTableHeaderCell("Z"));
            sb.Append(BuildTableHeaderCell("Lon"));
            sb.Append(BuildTableHeaderCell("Lat"));
            sb.Append(BuildTableHeaderCell("Height"));
            sb.Append(BuildTableHeaderCell("RmsE"));
            sb.Append(BuildTableHeaderCell("RmsN"));
            sb.Append(BuildTableHeaderCell("RmsU"));
            if (last.HasParamAccuracyInfos)
            {
                foreach (var kv in firstObject.ParamAccuracyInfos.KeyValues)
                {
                    sb.Append(BuildTableHeaderCell(kv.Key));
                }
            }
            sb.Append(BuildTableHeaderCell("Epoch"));

            var row = BuildTableRow(sb.ToString());
            mainSb.Append(row);

            var format = "G5";

            if (HasResults())
            {
                int i = 1;
                var baselineObj = firstObject as IWithEstimatedBaselines;
                var baselines = baselineObj.GetEstimatedBaselines();
                foreach (var baseline in baselines)
                { 
                    sb = new StringBuilder();;

                    sb.Append(BuildTableCell(i));
                    sb.Append(BuildTableCell(baseline.Name));
                    sb.Append(BuildTableCell(firstObject.ResultMatrix.StdDev.ToString(format)));
                    sb.Append(BuildTableCell(firstObject.ResultMatrix.ResultType.ToString()));
                    var EstimatedXyz = baseline.EstimatedRmsXyzOfRov.Value;
                    sb.Append(BuildTableCell(EstimatedXyz.X.ToString("0.#####")));
                    sb.Append(BuildTableCell(EstimatedXyz.Y.ToString("0.#####")));
                    sb.Append(BuildTableCell(EstimatedXyz.Z.ToString("0.#####")));

                    var geoCoord = baseline.EstimatedGeoCoordOfRov;
                    sb.Append(BuildTableCell(new DMS(geoCoord.Lon).ToReadableDms()));
                    sb.Append(BuildTableCell(new DMS(geoCoord.Lat).ToReadableDms()));
                    sb.Append(BuildTableCell(geoCoord.Height.ToString("0.#####")));

                    var tmsenu = CoordTransformer.LocaXyzToEnu(baseline.EstimatedRmsXyzOfRov.Rms, baseline.ApproxXyzOfRov);
                    sb.Append(BuildTableCell(Math.Abs(tmsenu.E).ToString(format)));
                    sb.Append(BuildTableCell(Math.Abs(tmsenu.N).ToString(format)));
                    sb.Append(BuildTableCell(Math.Abs(tmsenu.U).ToString(format)));

                    if (last.HasParamAccuracyInfos)
                    {
                        foreach (var kv in firstObject.ParamAccuracyInfos.KeyValues)
                        {
                            var val = kv.Value.RmsValue.Value;
                            var isOk = firstObject.ParamAccuracyInfos.IsOk(kv.Value);
                            var str = (val * 1e3).ToString("0.000");
                            if (!isOk)
                            {
                                str = "<b class='red'>" + str + "</b>";
                            }
                            sb.Append(BuildTableCell(str));
                        }
                    }
                    sb.Append(BuildTableCell(firstObject.ReceiverTime));

                    var rowContent = BuildTableRow(sb.ToString());
                    mainSb.Append(rowContent);
                    mainSb.AppendLine();
                    i++;
                }
            }
        }

        #endregion

        private bool HasResults()
        {
            return Results != null && Results.Count > 0;
        }

        #region html  标签生成
        /// <summary>
        /// row
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string BuildTableRow(string val) { return HtmlTrStart() + val + HtmlTrEnd(); }
        /// <summary>
        ///th
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string BuildTableHeaderCell(string val) { return HtmlThStart() + val + HtmlThEnd(); }
        /// <summary>
        /// cell
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string BuildTableCell(object val) { return HtmlTdStart() + val + HtmlTdEnd(); }
        /// <summary>
        /// td
        /// </summary>
        /// <returns></returns>
        public string HtmlTdStart() { return BuidHtmlStart("td"); }
        /// <summary>
        /// td
        /// </summary>
        /// <returns></returns>
        public string HtmlTdEnd() { return BuidHtmlEnd("td"); }
        /// <summary>
        /// tr
        /// </summary>
        /// <returns></returns>
        public string HtmlTrStart() { return BuidHtmlStart("tr"); }
        /// <summary>
        /// tr
        /// </summary>
        /// <returns></returns>
        public string HtmlTrEnd() { return BuidHtmlEnd("tr"); }
        /// <summary>
        /// th
        /// </summary>
        /// <returns></returns>
        public string HtmlThEnd() { return BuidHtmlEnd("th"); }
        /// <summary>
        /// th
        /// </summary>
        /// <returns></returns>
        public string HtmlThStart() { return BuidHtmlStart("th"); }
        /// <summary>
        /// 构建html起始标签
        /// </summary>
        /// <param name="markerName"></param>
        /// <returns></returns>
        public string BuidHtmlStart(string markerName)
        {
            return "<" + markerName + ">";
        }
        /// <summary>
        /// 构建html结束标签
        /// </summary>
        /// <param name="markerName"></param>
        /// <returns></returns>
        public string BuidHtmlEnd(string markerName)
        {
            return "</" + markerName + ">";
        }
        #endregion

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetValue(string key, string val)
        {
            this.Data[key] = val;
        }
        /// <summary>
        /// 统计一个结果
        /// </summary>
        /// <param name="result"></param>
        public void Add(SimpleGnssResult result)
        {
            if (result == null) { return; }
            Results.Add(result);
        }
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override string Build()
        {  
            BuildDic();

            string model = File.ReadAllText(ModelPath);
         
            foreach (var item in this.Data)
            {
                var key = "${" + item.Key + "}";
                 model = model.Replace(key, item.Value);
            }
         

            return model;
        }

        /// <summary>
        /// 构建且写入
        /// </summary>
        public void BuildAndWriteToFile()
        {
            if (Option.IsOutputAdjustMatrix) { ResultBuilder.WriteAdjustMatrixText(); }

            var model = Build();
            File.WriteAllText(OutputPath, model);
        }
        /// <summary>
        /// 构建并打开
        /// </summary>
        public void BuildWriteAndOpen(bool isOpen)
        {
            BuildAndWriteToFile();
           if(isOpen)  Geo.Utils.FileUtil.OpenFile(this.OutputPath);
        }
        #endregion
    }

    /// <summary>
    /// 简单的定位结果，用于获取结果
    /// </summary>
    public class SimplePositionResult
    {
        public string Name { get; set; }
        public string Epoch { get; set; }
        public string GeoCoord { get; set; }
        public string Xyz { get; set; }
        public string RmsXyz { get; set; }

    }
}
