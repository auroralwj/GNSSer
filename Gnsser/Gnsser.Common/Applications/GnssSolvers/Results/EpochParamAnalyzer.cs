//2018.11.10, czs, create in hmx, 提取单独的结果分析类

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Geo.Common;
using Gnsser.Checkers;

namespace Gnsser
{

    /// <summary>
    /// 历元参数分析者器
    /// </summary>
    public class EpochParamAnalyzer
    {
        Log log = new Log(typeof(EpochParamAnalyzer));
        /// <summary>
        /// 历元参数分析器
        /// </summary>
        /// <param name="ParamNames"></param>
        /// <param name="SequentialEpochCount"></param>
        /// <param name="MaxDiffer">判断是否收敛的标准</param>
        /// <param name="MaxAllowedConvergenceTime">判断是否合限，包括收敛时间，收敛后偏差和最大允许RMS</param>
        /// <param name="KeyLabelCharCount"></param>
        /// <param name="MaxAllowedDifferAfterConvergence">判断是否合限，包括收敛时间，收敛后偏差和最大允许RMS</param>
        /// <param name="MaxAllowedRms">判断是否合限，包括收敛时间，收敛后偏差和最大允许RMS</param>
        public EpochParamAnalyzer(
            List<string> ParamNames,
            int SequentialEpochCount=20,
            double MaxDiffer=0.1,
            double MaxAllowedConvergenceTime=240,
            int KeyLabelCharCount=4,
            double MaxAllowedDifferAfterConvergence =0.1,
            double MaxAllowedRms=0.1)
        { 
            this.MaxAllowedDifferAfterConvergence = MaxAllowedDifferAfterConvergence;
            this.MaxAllowedConvergenceTime = MaxAllowedConvergenceTime;
            this.KeyLabelCharCount = KeyLabelCharCount;
            this.ParamNames = ParamNames;
            this.MaxAllowedRms = MaxAllowedRms;
            this.SequentialEpochCount = SequentialEpochCount;
            this.MaxDiffer = MaxDiffer;
            ConvergenceResult = new BaseConcurrentDictionary<string, BaseDictionary<string, TimePeriod>>("结果", (key) => new BaseDictionary<string, TimePeriod>());
            EpochParamDataTables = new BaseConcurrentDictionary<string, ObjectTableStorage>();
        }

        #region 属性
        /// <summary>
        /// 连续历元数量，统计收敛
        /// </summary>
        public int SequentialEpochCount { get; set; }
        /// <summary>
        /// 最大的偏差，小于此才认为收敛
        /// </summary>
        public double MaxDiffer { get; set; }
        /// <summary>
        /// 最大允许的的RMS，超出则标记not ok
        /// </summary>
        public double MaxAllowedRms { get; set; }
        /// <summary>
        /// 允许最大的收敛时间，超出认为不收敛
        /// </summary>
        public double MaxAllowedConvergenceTime { get; set; }
        /// <summary>
        /// 收敛后允许的最大偏差。超出则标记超限。
        /// </summary>
        public double MaxAllowedDifferAfterConvergence { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public int KeyLabelCharCount { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<string> ParamNames { get; set; }
        #endregion

        #region 批量结果存储
        /// <summary>
        /// 结果，文件名：参数名：收敛时间
        /// </summary>
        BaseConcurrentDictionary<string, BaseDictionary<string, TimePeriod>> ConvergenceResult { get; set; }
        /// <summary>
        /// 原始数据表
        /// </summary>
        BaseConcurrentDictionary<string, ObjectTableStorage> EpochParamDataTables { get; set; }
        #endregion

        #region  单独提取
        /// <summary>
        /// 参数精度信息提取
        /// </summary>
        /// <param name="rawEpochParamTable"></param>
        /// <returns></returns>
        public ParamAccuracyInfoManager GetParamAccuracyInfos(ObjectTableStorage rawEpochParamTable)
        {
            int count = 0;
            foreach (var paranName in rawEpochParamTable.ParamNames)
            {
                if (this.ParamNames.Contains(paranName))
                {
                    count++;
                    break;
                }
            }
            if(count == 0)
            {
                log.Warn("估计参数的名称错了。");
                return null;
            }

            var epochParamTable = rawEpochParamTable.Clone();//克隆
      
            epochParamTable = GetParamsOnlyTable(epochParamTable); 

            string fileKey = BuildFileKey(epochParamTable.Name);
            epochParamTable.Name = fileKey;

            ParamAccuracyInfoManager accuracyInfos = new ParamAccuracyInfoManager(  
             MaxAllowedConvergenceTime,
             MaxAllowedDifferAfterConvergence ,
             MaxAllowedRms);
            //收敛行
            var ConvergeceDic = CalculateConvergeceTimePeriods(epochParamTable);
            foreach (var kv in ConvergeceDic.KeyValues)
            {
                var obj = accuracyInfos.GetOrCreate(kv.Key);
                obj.ConvergenceTimePeriod = kv.Value;
            }

            //移除收敛前的行
            var ConvergencedTable = GetConvergencedTable(epochParamTable, ConvergeceDic);

            //先作差，以最后历元为基准
            var referLastValConvergencedTable = epochParamTable.GetTableAllColMinusLastRow();

            //求最大偏差，
            var maxAbsDiffers = referLastValConvergencedTable.GetMaxAbsValue(this.ParamNames);
            foreach (var kv in ConvergeceDic.KeyValues)
            {
                var obj = accuracyInfos.GetOrCreate(kv.Key);
                obj.MaxAbsDiffer = maxAbsDiffers[kv.Key];// kv.Value;
            }

            //以最后一个为参考与其作差然后求RMS
            var rmses = referLastValConvergencedTable.GetResidualRms();
            foreach (var kv in ConvergeceDic.KeyValues)
            {
                var obj = accuracyInfos.GetOrCreate(kv.Key);
                obj.RmsValue = rmses[kv.Key];// kv.Value; kv.Value;
            }
            return accuracyInfos;
        }

        #endregion

        #region 批量计算增加
        static object locker = new object();
        /// <summary>
        /// 批量计算，增加一个，最后统计
        /// </summary>
        /// <param name="inputPath"></param>
        public void Add(string inputPath)
        {
            var table = ObjectTableReader.Read(inputPath);
            Add(table);
        }

        /// <summary>
        /// 批量计算，增加一个，最后统计
        /// </summary>
        /// <param name="table"></param>
        public void Add(ObjectTableStorage table)
        {
            string fileKey = BuildFileKey(table.Name);

            var newTable = GetParamsOnlyTable(table);
            newTable.Name = fileKey;

            //一个文件一个。
            var dic = CalculateConvergeceTimePeriods(newTable);
            lock (locker)
            {
                ConvergenceResult[fileKey]= dic;
                EpochParamDataTables[fileKey] = newTable; //存起来分析结果
            }  
        }
        #endregion


        #region 批量计算结果汇总
        /// <summary>
        /// 聚类收敛结果统计表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetTotalFileParamConvergenceTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("收敛结果统计表");

            foreach (var fileResult in ConvergenceResult.Data)
            {
                var fileName = fileResult.Key;
                var data = EpochParamDataTables.Get(fileName);
                var totalMinutes = ((Time)data.LastIndex - (Time)data.FirstIndex) / 60.0;
                var maxTimeSpan = Math.Min(MaxAllowedConvergenceTime, totalMinutes);

                table.NewRow();
                table.AddItem("Name", fileName);
                double maxTime = 0;
                bool isOK = true;
                foreach (var epochResult in fileResult.Value.KeyValues)
                {
                    table.AddItem(epochResult.Key, epochResult.Value.End);
                    var spanMinute = epochResult.Value.TimeSpan.TotalMinutes;
                    table.AddItem(epochResult.Key + "_Span", Double.Parse(spanMinute.ToString("0.000")));

                    maxTime = Math.Max(spanMinute, maxTime);
                }
                table.AddItem("MaxTime", Double.Parse(maxTime.ToString("0.000")));
                isOK = maxTime < MaxAllowedConvergenceTime;

                table.AddItem("IsOk", isOK);
                if (!isOK)
                {
                    log.Warn(fileName + " ,收敛时间超限 " + maxTime + " > " + maxTimeSpan);
                }
            }
            return table;
        }

        /// <summary>
        /// 所有文件的精度结果统计表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetTotalFileParamRmsTable()
        {
            ObjectTableStorage result = new ObjectTableStorage("参数 RMS");
            RmsedNumeralDictionary rmsedNumerals = new RmsedNumeralDictionary();
            foreach (var fileResult in ConvergenceResult.Data)
            {
                var fileName = fileResult.Key;
                var convergencePeriods = fileResult.Value;
                var epochParamTable = EpochParamDataTables.Get(fileName);

                ExtractRmsInfoAddToTableRow(ref result, epochParamTable, convergencePeriods);
            }
            return result;
        }
        #endregion

        #region 算法细节
        /// <summary>
        ///获取收敛后的数据表，即删除收敛前的内容。
        /// </summary>
        /// <param name="epochParamTable"></param>
        /// <param name="ConvergeceDic"></param>
        /// <returns></returns>
        private ObjectTableStorage GetConvergencedTable(ObjectTableStorage epochParamTable, BaseDictionary<string, TimePeriod> ConvergeceDic)
        {
            RemoveConvergencedRows(ref epochParamTable, ConvergeceDic);
            return epochParamTable;
        }
        private BaseDictionary<string, TimePeriod> CalculateConvergeceTimePeriods(ObjectTableStorage epochParamTable)
        {
            BaseDictionary<string, TimePeriod> dic = new BaseDictionary<string, TimePeriod>();
            string fileKey = epochParamTable.Name;
            //提取最后的结果作为比较依据
            Dictionary<string, double> finalValuesForCompare = new Dictionary<string, double>();
            foreach (var paramName in ParamNames)
            {
                finalValuesForCompare[paramName] = Geo.Utils.ObjectUtil.GetNumeral(epochParamTable.GetLastValue(paramName));
            }
            bool isAllOk = false;
            var indexName = epochParamTable.GetIndexColName();
            TimeNumeralWindowDataManager windowManager = new TimeNumeralWindowDataManager(SequentialEpochCount);
            int converedCount = 0; //已经收敛的数量
            foreach (var row in epochParamTable.BufferedValues)
            {
                var index = (Time)row[indexName];
                foreach (var item in row)
                {
                    var paramName = item.Key; 
                    if (ParamNames.Contains(paramName))
                    {
                        if (dic.Contains(paramName)) { continue; } //存在即收敛!!!

                        var window = windowManager.GetOrCreate(paramName);
                        window.IsSetTheVeryFirstKey = true;
                        var val = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                        if (!Geo.Utils.DoubleUtil.IsValid(val)) { continue; }

                        window.Add(index, val);
                        if (window.IsFull)
                        {
                            var lastVal = finalValuesForCompare[paramName];
                            var differ = window.GetMaxDifferReferTo(lastVal);
                            if (Math.Abs(differ.Value) <= MaxDiffer) // 已经收敛
                            {
                                var convergeEpoch = window.FirstKey;
                                //differ.Key.Tag = window.TheVeryFirstKey;
                                dic[paramName] = new TimePeriod((Time)epochParamTable.FirstIndex, convergeEpoch);
                                converedCount++;
                            }
                        }
                    }
                }
                isAllOk = converedCount == ParamNames.Count;
                if (isAllOk) //所有都收敛了，不用再计算
                {
                    break;
                }
            }

            //查找收敛的参数
            if (!isAllOk)//表示遍历完成也没有收敛
            {
                var notConvered = Geo.Utils.ListUtil.GetDifferences(ParamNames, dic.Keys);
                foreach (var paramName in notConvered)
                {
                    dic[paramName] = new TimePeriod((Time)epochParamTable.FirstIndex, Time.MaxValue);
                    log.Warn(fileKey + " 的 " + paramName + " 没有收敛!");
                }
            }
            log.Info("计算完成：" + epochParamTable.Name);
            return dic;
        }
        /// <summary>
        /// 移除其它参数
        /// </summary>
        /// <param name="epochParamTable"></param>
        /// <returns></returns>
        private  ObjectTableStorage  GetParamsOnlyTable(ObjectTableStorage epochParamTable)
        {
            return epochParamTable.GetTable(epochParamTable.Name, this.ParamNames);

            //var otherCols = Geo.Utils.ListUtil.GetDifferences(epochParamTable.ParamNames, this.ParamNames);
            //otherCols.Remove("Epoch");
            //epochParamTable.RemoveCols(otherCols);
            //return epochParamTable;
        }

        /// <summary>
        /// 表名称
        /// </summary>
        /// <param name="inputPath"></param>
        /// <returns></returns>
        private string BuildFileKey(string inputPath)
        {
            return Geo.Utils.StringUtil.SubString(Path.GetFileName(inputPath), 0, KeyLabelCharCount);
        }
        
        /// <summary>
        /// 提取到表行
        /// </summary>
        /// <param name="result"></param>
        /// <param name="epochParamTable"></param>
        /// <param name="convergencePeriods"></param>
        private void ExtractRmsInfoAddToTableRow(ref ObjectTableStorage result, ObjectTableStorage epochParamTable,  BaseDictionary<string, TimePeriod> convergencePeriods)
        {
            Dictionary<string, double> maxAbsDiffers;
            Dictionary<string, RmsedNumeral> rmses;

            RemoveConvergencedRows(ref epochParamTable, convergencePeriods);

            //先作差，以最后历元为基准
            var minusedTab = epochParamTable.GetTableAllColMinusLastRow();

            //求最大偏差，
            maxAbsDiffers = minusedTab.GetMaxAbsValue();

            //以最后一个为参考与其作差然后求RMS
            rmses = minusedTab.GetResidualRms();
            
            string fileName = epochParamTable.Name;

            AddParamRmsAsTableRow(result, maxAbsDiffers, rmses, fileName);

        } 

        /// <summary>
        /// 移除收敛前的行
        /// </summary>
        /// <param name="epochParamTable"></param>
        /// <param name="convergencePeriods"></param>
        private static void RemoveConvergencedRows(ref ObjectTableStorage epochParamTable, BaseDictionary<string, TimePeriod> convergencePeriods)
        {
            //移除初始未收敛的历元
            foreach (var epochResult in convergencePeriods.KeyValues)
            {
                var colName = epochResult.Key;
                var indexOf = epochParamTable.GetRowIndexOfIndexCol(epochResult.Value.End);
                if (indexOf == -1) { continue; }

                epochParamTable.RemoveRowsOfCol(colName, 0, indexOf);
            }
        }

        /// <summary>
        /// 增加参数精度信息到表行
        /// </summary>
        /// <param name="result"></param>
        /// <param name="maxDiffers"></param>
        /// <param name="rmses"></param>
        /// <param name="fileName"></param>
        private void AddParamRmsAsTableRow(ObjectTableStorage result, Dictionary<string, double> maxDiffers, Dictionary<string, RmsedNumeral> rmses, string fileName)
        {
            result.NewRow();
            result.AddItem("Name", fileName);
            var isOk = true;
            double topMaxDiffer = 0;
            foreach (var item in rmses)
            {
                var key = item.Key;
                result.AddItem(item.Key, item.Value);
                if (item.Value.Value > MaxAllowedRms)
                {
                    isOk = false;
                    log.Warn(fileName + " " + item.Key + " ,有历元 RMS 超限 " + item.Value + " > " + MaxAllowedRms);
                }

                double maxDiffer = 99999.99;
                if (maxDiffers.ContainsKey(key))
                {
                    maxDiffer = maxDiffers[item.Key];
                }
                topMaxDiffer = Math.Max(maxDiffer, topMaxDiffer);
                result.AddItem("MaxDifferOf" + item.Key, Double.Parse(maxDiffer.ToString("0.000")));
                if (isOk)
                {
                    isOk = (maxDiffer) < this.MaxAllowedDifferAfterConvergence;
                    if (!isOk)
                    {
                        log.Warn(fileName + " " + item.Key + " ,有历元偏差超限 " + maxDiffer + " > " + MaxAllowedDifferAfterConvergence);
                    }
                }
            }
            result.AddItem("TopMaxDiffer", topMaxDiffer);
            result.AddItem("IsOk", isOk);
        }

        #endregion
    }
    
    /// <summary>
    /// 参数精度信息
    /// </summary>
    public class ParamAccuracyInfoManager : BaseDictionary<string, ParamAccuracyInfo> , IReadable, IToTabRow
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MaxAllowedConvergenceTime">判断是否合限，包括收敛时间，收敛后偏差和最大允许RMS</param>
        /// <param name="MaxAllowedDifferAfterConvergence">判断是否合限，包括收敛时间，收敛后偏差和最大允许RMS</param>
        /// <param name="MaxAllowedRms">判断是否合限，包括收敛时间，收敛后偏差和最大允许RMS</param>
        public ParamAccuracyInfoManager(
            double MaxAllowedConvergenceTime,
            double MaxAllowedDifferAfterConvergence,
            double MaxAllowedRms)
        {
            this.MaxAllowedConvergenceTime = MaxAllowedConvergenceTime;
            this.MaxAllowedDifferAfterConvergence = MaxAllowedDifferAfterConvergence; 
            this.MaxAllowedRms = MaxAllowedRms;
        }

        /// <summary>
        /// 最大允许的的RMS，超出则标记not ok
        /// </summary>
        public double MaxAllowedRms { get; set; }
        /// <summary>
        /// 允许最大的收敛时间，超出认为不收敛,分钟
        /// </summary>
        public double MaxAllowedConvergenceTime { get; set; }
        /// <summary>
        /// 收敛后允许的最大偏差。超出则标记超限。
        /// </summary>
        public double MaxAllowedDifferAfterConvergence { get; set; }

        public override ParamAccuracyInfo Create(string key)
        {
            return new ParamAccuracyInfo()
            {
                ParamName = key,
                MaxAbsDiffer = 99999.999,
                ConvergenceTimePeriod = new TimePeriod(Time.MinValue, Time.MaxValue),
                RmsValue = new RmsedNumeral()
            };
        }
        /// <summary>
        /// 是否合限，包括收敛时间，收敛后偏差和最大允许RMS
        /// </summary>
        /// <param name="accuracyInfo"></param>
        /// <returns></returns>
        public bool IsOk(ParamAccuracyInfo accuracyInfo)
        {
            return accuracyInfo.ConvergenceTimePeriod.Span / 60 <= MaxAllowedConvergenceTime
                && accuracyInfo.MaxAbsDiffer <= MaxAllowedDifferAfterConvergence
                && accuracyInfo.RmsValue.Value <= MaxAllowedRms;
        }

        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in this)
            {
                if (i > 0)
                {
                    sb.Append("\t");
                }
                sb.Append(item.ParamName);
                i++;
            }
            return sb.ToString();
        }

        public string GetTabValues()
        { 
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in this)
            {
                if (i > 0)
                {
                    sb.Append("\t");
                }
                sb.Append(item.RmsValue.Value);
                i++;
            }
            return sb.ToString();
        }

        public string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IsOK, ParamName, ConvergenceMinutes , MaxAbsDiffer, RmsValue(Count) , ConvergenceTimePeriod ");
            foreach (var item in this)
            {
                var isOk = IsOk(item);
                sb.Append(isOk?"Good" : "Bad");
                sb.Append(splitter);
                sb.AppendLine(item.ToReadableText(splitter));
            }
            return sb.ToString();
        }

    }
    /// <summary>
    /// 参数精度信息
    /// </summary>
    public class ParamAccuracyInfo: IReadable
    {
        public ParamAccuracyInfo()
        {
        } 
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// 收敛时段
        /// </summary>
        public TimePeriod ConvergenceTimePeriod { get; set; }
        /// <summary>
        /// 收敛分钟
        /// </summary>
        public double ConvergenceSpanMinutes { get => ConvergenceTimePeriod.TimeSpan.TotalMinutes; }
        /// <summary>
        /// 最大绝对值偏差
        /// </summary>
        public double MaxAbsDiffer { get; set; }
        /// <summary>
        /// RMS 
        /// </summary>
        public RmsedNumeral RmsValue { get; set; }
        /// <summary>
        /// 可读的文本
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ParamName);
            sb.Append(splitter);
            sb.Append(" ");
            sb.Append(ConvergenceSpanMinutes.ToString("G5"));
            sb.Append(splitter);
            sb.Append(" ");
            sb.Append(MaxAbsDiffer.ToString("G5"));
            sb.Append(splitter);
            sb.Append(" ");
            sb.Append(RmsValue.ToString("G5"));
            sb.Append(splitter);
            sb.Append(" ");
            sb.Append(ConvergenceTimePeriod);

            return sb.ToString();
        }
    }

}
