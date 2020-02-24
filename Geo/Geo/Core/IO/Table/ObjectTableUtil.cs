//2016.03.22, czs, created in hongqing, 基于卫星结果的管理器 
//2016.03.29, czs, edit in hongqing, 名称修改为 NamedValueTableManager
//2016.08.05, czs, edit in fujian yongan, 重构
//2016.10.03, czs, edit in hongqing,增加缓存结果数量控制，便于控制内存大小
//2016.10.19, czs, edit in hongqing, 增加一些计算分析功能
//2016.10.26, czs, edit in hongqing, 表格值从字符串修改为 Object，减少转换损失
//2018.05.27, czs, create in HMX, 参数数值表格管理器算工具

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Gnsser; 
using Geo.Utils;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Common;
using Geo;
using Geo.Times;
using System.Threading.Tasks;
using Geo.Algorithm;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 拟合数据类型
    /// </summary>
    public enum FitDataValueType
    {
        /// <summary>
        /// 原始数据
        /// </summary>
        Raw,
        /// <summary>
        /// 结果数据
        /// </summary>
        Result,
        /// <summary>
        /// Rms
        /// </summary>
        Rms
    }
    /// <summary>
    /// 参数数值表格管理器算工具
    /// </summary> 
    public static class ObjectTableUtil
    {
        static Log log = new Log(typeof(ObjectTableUtil));

        /// <summary>
        /// 返回一个数字对齐后的表格
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetAlignedTable(ObjectTableStorage table)
        {
            var newTableName = TableNameHelper.BuildName(table.Name, "_数字对齐");
            ObjectTableStorage newTable = table.Clone();
            ObjectTableStorage newTable2 = table.Clone();
            newTable.Name = newTableName;

            newTable2.UpdateAllByMinusCol(newTable.FirstNumberalColName);
            //求列平均
            var aves = newTable2.GetAverages();
            foreach (var item in aves)
            {
                newTable.UpdateColumnByMinus(item.Key, item.Value);
            }
            return newTable;
        }

        /// <summary>
        /// 获取表格统计信息，包括最大值、最小值、平均值等。
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetStatisticsTable(ObjectTableStorage table)
        {
            DateTime start = DateTime.Now;

            var newTableName = TableNameHelper.BuildName(table.Name, "_统计信息");
            ObjectTableStorage storage = new ObjectTableStorage(newTableName);

            string indexColName = table.GetIndexColName();

            storage.NewRow();
            storage.AddItem("Name", "RowCount");
            storage.AddItem("Value", table.RowCount + "");

            storage.NewRow();
            storage.AddItem("Name", "ColCount");
            storage.AddItem("Value", table.ColCount + "");

            storage.NewRow();
            storage.AddItem("Name", "ValidCount");
            storage.AddItem("Value", table.GetValidDataCountOfTable() + "");
            storage.AddItem("Note", "Total Cell: " + (table.RowCount * table.ColCount));

            storage.NewRow();
            var maxCel = table.GetMax();
            storage.AddItem("Name", "MaxValue");
            storage.AddItem("Value", maxCel.Value + "");
            storage.AddItem("Note", maxCel.ToString());

            storage.NewRow();
            var minCel = table.GetMin(0);
            storage.AddItem("Name", "MinValue");
            storage.AddItem("Value", minCel.Value + "");
            storage.AddItem("Note", minCel.ToString());

            storage.NewRow();
            storage.AddItem("Name", "AveOfValidCell");
            var ave = new AverageValue(table.GetAverageWithRmse());
            storage.AddItem("Value", ave.Value);
            storage.AddItem("Note", ave.ToString());

            foreach (var colName in table.ParamNames)
            {
                if(indexColName == colName) { continue; }
                var count = table.GetValidRowCount(colName);
                storage.NewRow();
                storage.AddItem("Name", "CountOf_" + colName);
                storage.AddItem("Value", count);
                storage.AddItem("Note",  count * 1.0 / table.RowCount); 
            }

            var aves = table.GetAveragesWithStdDev();
            foreach (var item in aves)
            {
                var a = new AverageValue(item.Value);
                storage.NewRow();
                storage.AddItem("Name", "AveOf_" + item.Key);
                storage.AddItem("Value", a.Value);
                storage.AddItem("Note", a.ToString());
            }

            var absAves = table.GetAbsAveragesWithStdDev();
            foreach (var item in absAves)
            {
                var a = new AverageValue(item.Value);
                storage.NewRow();
                storage.AddItem("Name", "AbsAveOf_" + item.Key);
                storage.AddItem("Value", a.Value);
                storage.AddItem("Note", a.ToString());
            }

            var timespan = DateTime.Now - start;
            var str = "统计时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";

            log.Info(str);

            return storage;
        }

        /// <summary>
        /// 获取表格 平均值和绝对平均值等。
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetAverageTable(ObjectTableStorage table)
        {
            DateTime start = DateTime.Now;

            var newTableName = TableNameHelper.BuildName(table.Name, "_平均值");
            ObjectTableStorage storage = new ObjectTableStorage(newTableName);

            string indexColName = table.GetIndexColName();

            var aves = table.GetAveragesWithStdDev();
            foreach (var item in aves)
            {
                var a = new AverageValue(item.Value);
                storage.NewRow();
                storage.AddItem("Name", "AveOf_" + item.Key);
                storage.AddItem("Value", a.Value);
                storage.AddItem("Rms", a.StdDev);
            }

            var absAves = table.GetAbsAveragesWithStdDev();
            foreach (var item in absAves)
            {
                var a = new AverageValue(item.Value);
                storage.NewRow();
                storage.AddItem("Name", "AbsAveOf_" + item.Key);
                storage.AddItem("Value", a.Value);
                storage.AddItem("Rms", a.StdDev);
            }

            var timespan = DateTime.Now - start;
            var str = "统计时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";

            log.Info(str);

            return storage;
        }
        /// <summary>
        /// 获取残差中误差（均方差估值）表格， 所有列数值认为是残差，返回其平方中数的平方根。
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetResidualRmseTable(ObjectTableStorage table)
        {
            DateTime start = DateTime.Now;

            var newTableName = TableNameHelper.BuildName(table.Name, "_残差中误差");
            ObjectTableStorage storage = new ObjectTableStorage(newTableName);

            string indexColName = table.GetIndexColName();
             
            var rmse = table.GetResidualRmse();
            foreach (var item in rmse)
            { 
                storage.NewRow();
                storage.AddItem("Name", "RmseOf_" + item.Key);
                storage.AddItem("Value", item.Value[0]);
                storage.AddItem("Count", item.Value[1]);
            } 

            var timespan = DateTime.Now - start;
            var str = "计算时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";

            log.Info(str);

            return storage;
        }

        /// <summary>
        /// 多项式拟合滑动平均求拟合偏差
        /// </summary>
        /// <param name="table"></param>
        /// <param name="windowSize"></param> 
        /// <param name="polyFitOrder"></param>
        /// <param name="breakCount"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetTableOfFitError(ObjectTableStorage table, int windowSize = 10, int polyFitOrder = 2, double breakCount = 5)
        {
            DateTime start = DateTime.Now;
            TypedIndexHelper indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(windowSize, breakCount * indexHelper.Interval);

            var name = TableNameHelper.BuildName(table.Name, "_拟合精度估计_窗口" + windowSize + "_拟合次数" + polyFitOrder);
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();
            // int i = -1; 
            for (int i = 0; i < table.RowCount; i++)
            {
                var dic = table.BufferedValues[i];
                var indexValue = dic[indexColName];

                bool isIndexColumnAddedOfThisRow = false; //每一行确定该行有值，才添加索引
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == (indexColName)) { continue; }
                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }

                    var window = windowManager.GetOrCreate(key);

                    double k = indexHelper.GetDifferKeyValue(indexValue, i);
                    window.Add(k, (double)val);

                    if (window.IsFull)
                    {
                        var aveError = window.GetFitErrorWindow(polyFitOrder).AbsAverageValue;
                        window.Clear(); //清空

                        if (!isIndexColumnAddedOfThisRow)
                        {
                            storage.NewRow();
                            storage.AddItem(indexColName, indexValue);
                            isIndexColumnAddedOfThisRow = true;
                        }

                        storage.AddItem(key, aveError);
                    }
                }
            }

            var timespan = DateTime.Now - start;
            var str = "全列拟合时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";
            str += "滑动窗口: " + windowSize + " ,拟合次数: " + Geo.Utils.StringUtil.ToString(polyFitOrder);

            log.Info(str);

            return storage;
        }

        /// <summary>
        /// 拟合所有数值列,通过同一表中的不同阶次拟合，对比
        /// </summary>
        /// <param name="table"></param>
        /// <param name="polyFitOrders"></param>
        /// <param name="breakCount"></param>
        /// <param name="showFitDataValueTypes">是否显示原始数据</param>
        /// <returns></returns>
        public static ObjectTableStorage GetPolyfitColTableWithDiffOrders(ObjectTableStorage table,
            List<int> polyFitOrders,
             List<FitDataValueType> showFitDataValueTypes,
            double breakCount = 5)
        {
            if(showFitDataValueTypes == null) { showFitDataValueTypes = new List<FitDataValueType>() { FitDataValueType.Result };  }
            bool isShowRaw = showFitDataValueTypes.Contains(FitDataValueType.Raw);

            DateTime start = DateTime.Now;

            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(table.RowCount, breakCount * indexHelper.Interval);
            var name = TableNameHelper.BuildName(table.Name, "_拟合值_拟合次数" + Geo.Utils.StringUtil.ToString(polyFitOrders));
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();

            //提前加入缓存，利于拟合
            int bufferIndex = 0;
            foreach (var dic in table.BufferedValues)
            {
                var indexValue = dic[indexColName];
                double k = indexHelper.GetDifferKeyValue(indexValue, bufferIndex);

                foreach (var item in dic)
                {
                    if (String.Equals(item.Key, indexColName)) { continue; }
                    var objVal = item.Value;
                    if (Geo.Utils.StringUtil.IsDecimalOrNumber(objVal))
                    {
                        var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                        if (val != null && !double.IsNaN((double)val))
                        {
                            windowManager.GetOrCreate(item.Key).Add(k, (double)val);
                        }
                    }
                }
                bufferIndex++;
            };
            var polyfits = new BaseDictionary<string, LsPolyFit>();
            foreach (var item in windowManager.Data)
            {
                foreach (var order in polyFitOrders)
                {
                    var key = item.Key + "_Order" + order;
                    var fitter = item.Value.BuildPolyfitter(order);
                    polyfits.Add(key, fitter);
                    var info = key + "： " + fitter.ToString();
                    new Log(typeof(ObjectTableUtil)).Info(info);
                }
            }

            int i = -1;
            foreach (var dic in table.BufferedValues)
            {
                i++;
                var indexValue = dic[indexColName];

                bool isIndexColumnAddedOfThisRow = false; //每一行,确定该行有值，才添加索引
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == indexColName) { continue; }
                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }

                    //此行有值，是否添加了检索。
                    if (!isIndexColumnAddedOfThisRow)
                    {
                        storage.NewRow();
                        storage.AddItem(indexColName, indexValue);
                        isIndexColumnAddedOfThisRow = true;
                    }
                    if (isShowRaw)
                    {
                        storage.AddItem(key, objVal);
                    }

                    foreach (var order in polyFitOrders)
                    {
                        var key2 = key + "_Order" + order;
                        if (!polyfits.Contains(key2)) { continue; }

                        var polyfit = polyfits[key2];

                        double k = indexHelper.GetDifferKeyValue(indexValue, i);

                        var polyFitVal = polyfit.GetY(k);
                        storage.AddItem(key2, polyFitVal);
                    }
                }
            }

            var timespan = DateTime.Now - start;
            var str = "全列拟合时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";
            str += "拟合次数: " + Geo.Utils.StringUtil.ToString(polyFitOrders);

            log.Info(str);
            return storage;
        }

        /// <summary>
        /// 拟合所有数值列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="polyFitOrder"></param>
        /// <param name="breakCount"></param>
        /// <param name="isShowRaw">是否显示原始数据</param>
        /// <returns></returns>
        public static ObjectTableStorage GetPolyfitColTable(ObjectTableStorage table, int polyFitOrder,
             List<FitDataValueType> showFitDataValueTypes, double breakCount = 5)
        {
            if (showFitDataValueTypes == null) { showFitDataValueTypes = new List<FitDataValueType>() { FitDataValueType.Result}; }
            bool isShowRaw = showFitDataValueTypes.Contains(FitDataValueType.Raw); 

            DateTime start = DateTime.Now;

            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(table.RowCount, breakCount * indexHelper.Interval);

            var name = TableNameHelper.BuildName(table.Name, "_全列_拟合次数" + polyFitOrder);
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();

            //提前加入缓存，利于拟合
            int bufferIndex = 0;
            foreach (var dic in table.BufferedValues)
            {
                var indexValue = dic[indexColName];
                double k = indexHelper.GetDifferKeyValue(indexValue, bufferIndex);

                foreach (var item in dic)
                {
                    if (String.Equals(item.Key, indexColName)) { continue; }
                    var objVal = item.Value;
                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);

                    if (val == null || double.IsNaN((double)val)) { continue; }
                    windowManager.GetOrCreate(item.Key).Add(k, (double)val);
                }
                bufferIndex++;
            };
            BaseDictionary<string, LsPolyFit> polyfits = new BaseDictionary<string, LsPolyFit>();
            foreach (var item in windowManager.Data)
            {
                polyfits.Add(item.Key, item.Value.BuildPolyfitter(polyFitOrder));
            }


            int i = -1;
            foreach (var dic in table.BufferedValues)
            {
                i++;
                var indexValue = dic[indexColName];

                bool isIndexColumnAddedOfThisRow = false; //每一行,确定该行有值，才添加索引
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == (indexColName) || !polyfits.Contains(key)) { continue; }
                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }


                    double k = indexHelper.GetDifferKeyValue(indexValue, i);

                    var polyfit = polyfits[key];
                    var polyFitVal = polyfit.GetY(k);
                    //此行有值，是否添加了检索。
                    if (!isIndexColumnAddedOfThisRow)
                    {
                        storage.NewRow();
                        storage.AddItem(indexColName, indexValue);
                        isIndexColumnAddedOfThisRow = true;
                    }

                    if (isShowRaw)
                    {
                        storage.AddItem(key, objVal);
                    }
                    storage.AddItem(key + "_Fit", polyFitVal);
                }
            }

            var timespan = DateTime.Now - start;
            var str = "全列拟合时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";
            str += "拟合次数: " + Geo.Utils.StringUtil.ToString(polyFitOrder);

            log.Info(str);

            return storage;
        }

        /// <summary>
        /// 对数值列进行分段的滑动窗口多项式拟合
        /// </summary>
        /// <param name="table"></param>
        /// <param name="polyfitCount"></param> 
        /// <param name="polyFitOrder"></param>
        /// <param name="breakCount"></param> 
        /// <param name="colNames"></param>
        /// <param name="showFitDataValueTypes"></param>
        /// <param name="polyfitType">是否重叠部分窗口，改进的分段拟合，兼顾滑动与分段特点</param> 
        /// <param name="marginCount">仅当isOverlaped为true时起作用</param>
        /// <param name="overlapCount">仅当isOverlaped为true时起作用</param>
        /// <returns></returns>
        public static ObjectTableStorage GetPolyFittedTable(ObjectTableStorage table,
            List<int> polyFitOrder,
            int polyfitCount,
            PolyfitType polyfitType,
             List<FitDataValueType> showFitDataValueTypes,
            double breakCount = 5,
            List<string> colNames = null,
            int marginCount = 0,
            int overlapCount = 0)
        {
            if (showFitDataValueTypes == null) { showFitDataValueTypes = new List<FitDataValueType>() { FitDataValueType.Result }; }
            bool isShowRaw = showFitDataValueTypes.Contains(FitDataValueType.Raw);
            bool isAppdenRmse = showFitDataValueTypes.Contains(FitDataValueType.Rms);

            DateTime start = DateTime.Now;

            var startOfPolyBuildCount = PolyfitBuilder.BuildCount;

            MovingWindowCounter countCal = new MovingWindowCounter(polyfitCount, polyfitType, marginCount, overlapCount);
            log.Info(countCal.ToString());

            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(countCal.WindowSize, breakCount * indexHelper.Interval);

            if(colNames == null){
                colNames = table.ParamNames;
            }

            var name = TableNameHelper.BuildName(table.Name, polyfitType + "窗口_" + polyfitCount + "_拟合次数" + Geo.Utils.StringUtil.ToString(polyFitOrder));
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();
            var maxOrder = polyFitOrder.Max();
            var BufferedStream = new BufferedStreamService<Dictionary<string, object>>(table.BufferedValues, countCal.BufferSize);
            //提前加入缓存，利于拟合
            SetupBuffer(BufferedStream, windowManager, indexColName, indexHelper);
            int i = -1;
            foreach (var dic in BufferedStream)
            {
                if (dic == null) { continue; }
                i++;
                var indexValue = dic[indexColName];

                bool isIndexColumnAddedOfThisRow = false; //每一行,确定该行有值，才添加索引
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == (indexColName) || !colNames.Contains(key)) { continue; }                   

                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }

                    var window = windowManager.GetOrCreate(key);

                    double k = indexHelper.GetDifferKeyValue(indexValue, i);
                    //window.Add(k, (double)val);

                    if (window.Count > maxOrder)
                    {
                        //此行有值，是否添加了检索。
                        if (!isIndexColumnAddedOfThisRow)
                        {
                            storage.NewRow();
                            storage.AddItem(indexColName, indexValue);
                            isIndexColumnAddedOfThisRow = true;
                        }
                        if (isShowRaw)
                        {
                            storage.AddItem(key, val);
                        }

                        foreach (var order in polyFitOrder)
                        {
                            var polyFitVal = window.GetSectedMovWindowPolyfitValue(order, k, polyfitCount, countCal.MarginCount, true, countCal.OverlapCount);

                            var keyName = key + "_Fit" + order;
                            storage.AddItem(keyName, polyFitVal.Value);
                            if (isAppdenRmse) { storage.AddItem(keyName + "_Rms", polyFitVal.Rms); }
                        }
                    }
                }
            }

            var PolyBuildCount = PolyfitBuilder.BuildCount - startOfPolyBuildCount;


            var timespan = DateTime.Now - start;
            var str = "分段窗口拟合时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";
            str += "拟合器生成次数：" + PolyBuildCount + " , 拟合数量: " + polyfitCount + ",拟合次数: " + Geo.Utils.StringUtil.ToString(polyFitOrder);

            log.Info(str);


            return storage;
        }


        /// <summary>
        /// 对数值列进行滑动窗口多项式拟合
        /// </summary>
        /// <param name="table"></param>
        /// <param name="polyfitCount"></param> 
        /// <param name="polyFitOrder"></param>
        /// <param name="breakCount"></param> 
        /// <returns></returns>
        public static ObjectTableStorage GetPolyFittedTableWithMoveWindowAndDifferOrders(ObjectTableStorage table, List<int> polyFitOrder,  int polyfitCount, List<FitDataValueType> showFitDataValueTypes, double breakCount = 5)
        {
             
            DateTime start = DateTime.Now;
            if(showFitDataValueTypes == null) { showFitDataValueTypes = new List<FitDataValueType>() { FitDataValueType.Result }; }
            bool isShowRaw = showFitDataValueTypes.Contains(FitDataValueType.Raw);
            bool isAppdenRmse = showFitDataValueTypes.Contains(FitDataValueType.Rms);

            var startOfPolyBuildCount = PolyfitBuilder.BuildCount;
            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(polyfitCount, breakCount * indexHelper.Interval);

            var name = TableNameHelper.BuildName(table.Name, "_拟合值_滑动窗口" + polyfitCount + "_拟合次数" + Geo.Utils.StringUtil.ToString(polyFitOrder));
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();
            var maxOrder = polyFitOrder.Max();
            var BufferedStream = new BufferedStreamService<Dictionary<string, object>>(table.BufferedValues, polyfitCount / 2);
            //提前加入缓存，利于拟合
            SetupBuffer(BufferedStream, windowManager, indexColName, indexHelper);
            int i = -1;
            foreach (var dic in BufferedStream)
            {
                if (dic == null) { continue; }
                i++;
                var indexValue = dic[indexColName];

                bool isIndexColumnAddedOfThisRow = false; //每一行,确定该行有值，才添加索引
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == (indexColName)) { continue; }
                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }

                    var window = windowManager.GetOrCreate(key);
                    double k = indexHelper.GetDifferKeyValue(indexValue, i);

                    if (window.Count <= maxOrder) { continue; }

                    //此行有值，是否添加了检索。
                    if (!isIndexColumnAddedOfThisRow)
                    {
                        storage.NewRow();
                        storage.AddItem(indexColName, indexValue);
                        isIndexColumnAddedOfThisRow = true;
                    }
                    if (isShowRaw)
                    {
                        storage.AddItem(key, val);
                    }

                    foreach (var order in polyFitOrder)
                    {
                        var polyFitVal = window.GetPolyFitValue(k, order, polyfitCount);
                        var keyName = key + "_Fit" + order;
                        storage.AddItem(keyName, polyFitVal.Value);
                        if (isAppdenRmse) { storage.AddItem(keyName + "_Rms", polyFitVal.Rms); }
                    }
                }
            }

            var PolyBuildCount = PolyfitBuilder.BuildCount - startOfPolyBuildCount;

            var timespan = DateTime.Now - start;
            var str = "滑动窗口拟合时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";
            str += "拟合器生成次数：" + PolyBuildCount + " ,滑动窗口: " + polyfitCount + ",拟合次数: " + Geo.Utils.StringUtil.ToString(polyFitOrder);

            log.Info(str);


            return storage;
        }

        /// <summary>
        /// 对数值列进行滑动窗口多项式拟合
        /// </summary>
        /// <param name="table"></param>
        /// <param name="windowSize"></param> 
        /// <param name="polyFitOrder"></param>
        /// <param name="showFitDataValueTypes"></param>
        /// <param name="breakCount"></param>
        /// <param name="isBuffered"></param>
        /// <param name="colNames"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetPolyFittedTableWithMoveWindow(
            ObjectTableStorage table,
            List<FitDataValueType> showFitDataValueTypes,
            int windowSize = 10,
            int polyFitOrder = 2, 
            List<string> colNames = null,
            double breakCount = 5, 
            bool isBuffered = true)
        {
            if (showFitDataValueTypes == null) { showFitDataValueTypes = new List<FitDataValueType>() { FitDataValueType.Result }; }
            bool isShowRaw = showFitDataValueTypes.Contains(FitDataValueType.Raw);
            bool isAppdenRmse = showFitDataValueTypes.Contains(FitDataValueType.Rms);

            DateTime start = DateTime.Now;

            var startOfPolyBuildCount = PolyfitBuilder.BuildCount;
            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(windowSize, breakCount * indexHelper.Interval);

            if (colNames == null) { colNames = table.ParamNames; }

            var name = TableNameHelper.BuildName(table.Name, "_拟合值_滑动窗口" + windowSize + "_拟合次数" + polyFitOrder);
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();
             
            int bufferSize = isBuffered ? 0 : windowSize / 2;
            log.Info(table.Name + " 移动窗口拟合的缓存大小：" + bufferSize + ", 有缓存可避免边缘拟合");
            var BufferedStream = new BufferedStreamService<Dictionary<string, object>>(table.BufferedValues, bufferSize);

            //提前加入缓存，利于拟合
            SetupBuffer(BufferedStream, windowManager, indexColName, indexHelper);

            int i = -1;
            foreach (var dic in BufferedStream)
            {
                if(dic == null) { continue; }

                i++;
                var indexValue = dic[indexColName];

                bool isIndexColumnAddedOfThisRow = false; //每一行,确定该行有值，才添加索引
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == (indexColName) || !colNames.Contains(key)) { continue; }

                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }

                    var window = windowManager.GetOrCreate(key);
                    double k = indexHelper.GetDifferKeyValue(indexValue, i);

                    if (window.Count <= polyFitOrder) { continue; }
                    var polyFitVal = window.GetPolyFitValue(k, polyFitOrder, windowSize);

                    //此行有值，是否添加了检索。
                    if (!isIndexColumnAddedOfThisRow)
                    {
                        storage.NewRow();
                        storage.AddItem(indexColName, indexValue);
                        isIndexColumnAddedOfThisRow = true;
                    }

                    if (isShowRaw)
                    {
                        storage.AddItem(key, val);
                    }

                    storage.AddItem(key + "_Fit", polyFitVal.Value);
                    if (isAppdenRmse) { storage.AddItem(key + "_Rms", polyFitVal.Rms); }
                }
            }
            var PolyBuildCount = PolyfitBuilder.BuildCount - startOfPolyBuildCount;


            var timespan = DateTime.Now - start;
            var str = "滑动窗口拟合时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";
            str += "拟合器生成次数：" + PolyBuildCount + " ,滑动窗口: " + windowSize + ",拟合次数: " + polyFitOrder;

            log.Info(str);

            return storage;
        }

        /// <summary>
        /// 自适应线性拟合
        /// </summary>
        /// <param name="table"></param>
        /// <param name="windowSize"></param>
        /// <param name="colNames"></param>
        /// <param name="breakCount"></param>
        /// <param name="showFitDataValueTypes"></param>
        /// <param name="bufferSize">缓存大小，0为没有，若小于0，则默认为一半</param> 
        /// <returns></returns>
        public static ObjectTableStorage GetAdaptiveLinearFitTable(
            ObjectTableStorage table,
            int windowSize,    
            List<FitDataValueType> showFitDataValueTypes,
            int bufferSize = -1,
            List<string> colNames = null,
            int breakCount = 5)
        {

            if (showFitDataValueTypes == null) { showFitDataValueTypes = new List<FitDataValueType>() { FitDataValueType.Result }; }
            bool isShowRawValue = showFitDataValueTypes.Contains(FitDataValueType.Raw);
            bool isAppdenRmse = showFitDataValueTypes.Contains(FitDataValueType.Rms);
            DateTime start = DateTime.Now;

            var startOfPolyBuildCount = PolyfitBuilder.BuildCount;
            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(windowSize, breakCount * indexHelper.Interval);

            if (colNames == null) { colNames = table.ParamNames; }

            var name = TableNameHelper.BuildName(table.Name, "_自适应线性滑动窗口" + windowSize);
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();

            bufferSize = bufferSize < 0 ? windowSize / 2 : bufferSize;

            var BufferedStream = new BufferedStreamService<Dictionary<string, object>>(table.BufferedValues, bufferSize);
            log.Info(table.Name +" 自适应线性拟合的缓存大小：" + bufferSize + ", 有缓存可避免边缘拟合");
            //提前加入缓存，利于拟合
            SetupBuffer(BufferedStream, windowManager, indexColName, indexHelper);

            int i = -1;
            foreach (var dic in BufferedStream)
            {
                if (dic == null) { continue; }
                i++;
                var indexValue = dic[indexColName];

                bool isIndexColumnAddedOfThisRow = false; //每一行,确定该行有值，才添加索引
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == (indexColName) || !colNames.Contains(key)) { continue; }

                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }

                    var window = windowManager.GetOrCreate(key);
                    double k = indexHelper.GetDifferKeyValue(indexValue, i);

                    if (window.Count <= 1) { continue; }
                    var polyFitVal = window.GetAdaptiveLinearFitValue(k, windowSize, isAppdenRmse);

                    //此行有值，是否添加了检索。
                    if (!isIndexColumnAddedOfThisRow)
                    {
                        storage.NewRow();
                        storage.AddItem(indexColName, indexValue);
                        isIndexColumnAddedOfThisRow = true;
                    }

                    //原始数据
                    if (isShowRawValue)
                    {
                        storage.AddItem(key, val);
                    }

                    storage.AddItem(key + "_Fit", polyFitVal.Value);
                    if (isAppdenRmse) { storage.AddItem(key + "_Rms", polyFitVal.Rms); }
                }
            }
            var PolyBuildCount = PolyfitBuilder.BuildCount - startOfPolyBuildCount;


            var timespan = DateTime.Now - start;
            var str = "自适应线性窗口拟合时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";
            str += "拟合器生成次数：" + PolyBuildCount + " ,滑动窗口: " + windowSize ;

            log.Info(str);

            return storage;


        }

        /// <summary>
        /// 获取没有拟合粗差的表
        /// </summary>
        /// <param name="table"></param>
        /// <param name="windowSize"></param>
        /// <param name="errorTimes"></param>
        /// <param name="polyFitOrder"></param>
        /// <param name="breakCount"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetTableOfNoFitGrossError(ObjectTableStorage table, int windowSize = 6, double errorTimes = 3.0, int polyFitOrder = 2, double breakCount = 5)
        {
            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(windowSize, breakCount * indexHelper.Interval);

            var name = TableNameHelper.BuildName(table.Name, "_拟合去粗差_窗口" + windowSize + "_最大误差被数" + errorTimes);
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();
            // int i = -1; 
            for (int i = 0; i < table.RowCount; i++)
            {
                var dic = table.BufferedValues[i];
                var indexValue = dic[indexColName];
                storage.NewRow();
                storage.AddItem(indexColName, indexValue);
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == (indexColName)) { continue; }
                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }

                    var window = windowManager.GetOrCreate(key);
                    double k = indexHelper.GetDifferKeyValue(indexValue, i);
                    //window.Add(k, (double)val);

                    //if (window.IsFull)
                    {
                        // bool isGross = window.IsLastOverLimited(polyFitOrder, errorTimes);

                        bool isOk = window.PolyfitCheckAddOrClear(k, (double)val, polyFitOrder, errorTimes);
                        if (isOk)
                        {
                            storage.AddItem(key, val);
                        }
                        else
                        {
                            var msg = "添加失败或发现粗差：" + key + ", " + indexValue + ", " + objVal;
                            Log.GetLog(typeof(ObjectTableUtil)).Warn(msg);
                            if (key == "L1")
                            {
                                int iii = 0;
                            }
                        }
                    }
                }

                storage.EndRow();
            }

            return storage;
        }

        /// <summary>
        /// 循环剔除粗差
        /// </summary>
        /// <param name="table"></param>
        /// <param name="windowSize"></param>
        /// <param name="errorTimes"></param>
        /// <param name="breakCount"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetTableOfNoAveGrossErrorLoopUtil(ObjectTableStorage table, int windowSize, double errorTimes, double breakCount = 5)
        {
            var rmses = table.GetAveragesWithRms(table.ParamNames);
            ObjectTableStorage result = GetTableOfNoAveGrossError(table, windowSize, errorTimes, breakCount);

            var afterrmses = result.GetAveragesWithRms(table.ParamNames);
            foreach (var item in rmses)
            {
                if(afterrmses.ContainsKey(item.Key))
                {
                    if(item.Value.Value != afterrmses[item.Key].Value)
                    {
                        return GetTableOfNoAveGrossErrorLoopUtil(result, windowSize, errorTimes, breakCount);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取没有平均粗差的表
        /// </summary>
        /// <param name="table"></param>
        /// <param name="windowSize"></param>
        /// <param name="errorTimes"></param>
        /// <param name="breakCount"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetTableOfNoAveGrossError(ObjectTableStorage table, int windowSize = 6, double errorTimes = 3.0, double breakCount = 5)
        {
            Log log = new Log(typeof(ObjectTableUtil));
            var isFullWindow = windowSize >= table.ColCount;
            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(windowSize, int.MaxValue);// 求平均，不用连续 breakCount * indexHelper.Interval);
            var name = TableNameHelper.BuildName(table.Name, "_平均去粗差_窗口" + windowSize + "_最大误差被数" + errorTimes);
            ObjectTableStorage storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();

            var bufferedStream = new BufferedStreamService<Dictionary<string, object>>(table.BufferedValues, windowSize / 2);
            int index = -1;
            bufferedStream.MaterialInputted += new MaterialEventHandler<Dictionary<string, object>>(dic =>
              {
                  index++;
                  foreach (var kv in dic)
                  {
                      var key = kv.Key;
                      if (key == (indexColName)) { continue; }
                      var objVal = kv.Value;


                      if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                      var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                      if (val == null || double.IsNaN((double)val)) { continue; }

                      windowManager.GetOrCreate(key).Add(index, (double)val);
                  }
              });

            //如果全窗口，则只计算一次,用此存储
            Dictionary<string, RmsedNumeral> fullAveDic = new Dictionary<string, RmsedNumeral>();

            foreach (var dic in bufferedStream)
            {
                if (isFullWindow && fullAveDic.Count == 0)//全窗口计算
                {
                    log.Info("采用全窗口均值计算。");
                    foreach (var colName in table.ParamNames)
                    {
                        if (colName == (indexColName)) { continue; }
                        fullAveDic[colName] = windowManager.GetOrCreate(colName).Average;
                    }
                }


                var indexValue = dic[indexColName];
                storage.NewRow();
                storage.AddItem(indexColName, indexValue);
                foreach (var kv in dic)
                {
                    var key = kv.Key;
                    if (key == (indexColName)) { continue; }
                    var objVal = kv.Value;


                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }
                    RmsedNumeral aveValue = null;
                    if (isFullWindow)
                    {
                        if (fullAveDic.ContainsKey(key))
                        {
                            aveValue = fullAveDic[key];
                        }
                    }
                    else  //实时计算
                    {
                        var window = windowManager.GetOrCreate(key);
                        aveValue = window.Average;
                    }
                    if (aveValue == null)
                    {
                        continue;
                    }

                    var differ = aveValue.Value - val;
                    var maxError = aveValue.Rms * errorTimes;
                    bool isOk = (Math.Abs((double)differ) <= maxError);

                    if (isOk)
                    {
                        storage.AddItem(key, val);
                    }
                    else
                    {
                        double exceedTimes = (double)(differ / aveValue.Rms);

                        var msg = "发现粗差：" + key + ", " + indexValue + ", 值："
                            + val + ", 平均值："
                            + aveValue.ToString("G5")
                            + ", 超出Rms倍数：" + exceedTimes.ToString("0.00")
                            + ", 允许倍数： " + errorTimes.ToString("0.00")
                            ;
                        log.Warn(msg);
                    }
                }

                storage.EndRow();
            }

            return storage;
        }
        /// <summary>
        /// 获取所有列分组平均，分组根据各列的连续性决定。
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="minCalcuCount"></param>
        /// <param name="maxBreakCountInSegment"></param>
        /// <param name=tableColName">表名称标识</param>
        /// <returns></returns>
        public static ObjectTableStorage GetGroupAverageTableOfCols(this ObjectTableManager tables, int minCalcuCount, int maxBreakCountInSegment, double maxDiffer, string tableColName)
        {
            var tablename = Namer.GetName(tables.Keys);
            ObjectTableStorage result = new ObjectTableStorage("AverageOf" + tablename);
           int index = 0;
            foreach (var table in tables)
            { 
               var name = Namer.GetFirstPart(table.Name);

                var vals = table.GetGroupedAverageValue(minCalcuCount, maxBreakCountInSegment, maxDiffer);
                vals.AddDetailRowsToTable(result, ref index, "Site", name);  
            }
            return result;

            //下面的算法使得index不连续编号
           ObjectTableManager mgr = new ObjectTableManager(tables.OutputDirectory);
            foreach (var table in tables)
            {
                var name = Namer.GetFirstPart(table.Name);
                mgr[name]=table.GetGroupedAverageTable(minCalcuCount, maxBreakCountInSegment, maxDiffer, tableColName, name);
            }
            return  mgr.Combine("AverageTable");
        }
        /// <summary>
        /// 计算每个分段的平均数返回详细表
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="minCalcuCount">最小计算数量</param>
        /// <param name="maxBreakCountInSegment">分段内允许的最大断裂数量</param>
        /// <param name="tableColName">列标题名称</param>
        /// <returns></returns>
        public static ObjectTableStorage GetGroupedAverageTable( this ObjectTableStorage table, int minCalcuCount, int maxBreakCountInSegment, double maxDiffer = 5, string tableColName = null, string tableColValue = null)
        {
            GroupedRmsValue<string, string> store = table.GetGroupedAverageValue(minCalcuCount, maxBreakCountInSegment, maxDiffer);

            if (tableColValue == null)
            {
                tableColValue = table.Name;
            }
            return store.GetDetailTable("各段求平均详细_" + table.Name, tableColName, tableColValue); 
        }
        /// <summary>
        /// 计算每个分段的平均数返回详细表
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="minCalcuCount">最小的计算数量</param>
        /// <param name="maxBreakCountInSegment">最大连续数量</param>
        /// <param name="maxDiffer">新数据最大偏差</param>
        /// <returns></returns>
        public static GroupedRmsValue<string, string> GetGroupedAverageValue(this ObjectTableStorage table, int minCalcuCount, int maxBreakCountInSegment, double maxDiffer=5)
        {
            var indexColName = table.GetIndexColName();
            NumeralWindowDataManager windowMgr = new NumeralWindowDataManager(table.RowCount, maxBreakCountInSegment);
            var store = new GroupedRmsValue<string, string>();

            int currentRowIndex = -1;
            Object currentIndex = null;
            foreach (var dic in table.BufferedValues)
            {
                currentRowIndex++;
                currentIndex = dic[indexColName];
                foreach (var item in dic)
                {
                    var colName = item.Key;
                    if (indexColName == item.Key) { continue; }
                    //初始化
                    NumeralWindowData window = GetOrCreateWindowData(windowMgr, currentIndex, colName);

                    //尝试获取数据
                    double val = 0;
                    if (Geo.Utils.ObjectUtil.IsNumerial(item.Value))
                    {
                        val = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                        if (!Geo.Utils.DoubleUtil.IsValid(val))
                        {
                            continue;
                        }
                    }
                    //如果有数据，
                    if (window.Count > 0)
                    {
                        //数量足够，且发生断裂 
                        if (window.IsIndexBreaked(currentRowIndex))
                        {
                            window = CheckAddAveValAndCreateNewWindow(windowMgr, store, currentIndex, colName, window, minCalcuCount);
                        }
                        else//没有断裂，则判断当前是否粗差或发生了周跳
                        {
                            var differ = Math.Abs(window.Last - val);
                            if (differ > maxDiffer)//这里只考虑如果当前发生了周跳，则后继也周跳的情况
                            { 
                                window = CheckAddAveValAndCreateNewWindow(windowMgr, store, currentIndex, colName, window, minCalcuCount);
                            }
                        }
                    }                   
                    
                    //增加
                    window.Add(currentRowIndex, val);
                    ((Pair<Object>)window.Tag).Second = currentIndex;//更新分段编号（时间） 
                }
            }

            //最后需要判断计算
            foreach (var item in windowMgr.Data)
            {
                //初始化
                var window = item.Value;
                var colName = item.Key;
                CheckAddAveValAndClean(store, colName, window, minCalcuCount); 
            }

            return store;
        }

        private static NumeralWindowData CheckAddAveValAndCreateNewWindow(
            NumeralWindowDataManager windowMgr, 
            GroupedRmsValue<string, string> store,
            object currentIndex,
            string colName,
            NumeralWindowData window,
            int minCalcuCount)
        {
            CheckAddAveValAndClean(store, colName, window, minCalcuCount);
            //重新生成
            window = CreateNewWindow(windowMgr, currentIndex, colName);
            return window;
        }

        private static void CheckAddAveValAndClean(GroupedRmsValue<string, string> store, string colName, NumeralWindowData window, int minCalcuCount)
        {
            if (window.Count >= minCalcuCount)
            {
                AddWindowAveToGroupStorageAndClean(store, window, colName);
            }
        }

        /// <summary>
        /// 计算窗口平均值到分组存储对象，并清空。
        /// </summary>
        /// <param name="store"></param>
        /// <param name="window"></param>
        /// <param name="colName"></param>
        private static void AddWindowAveToGroupStorageAndClean(GroupedRmsValue<string, string> store, NumeralWindowData window, string colName)
        {
            var sto = store.GetOrCreate(colName);
            string group = ((Pair<Object>)window.Tag).ToString(TimePeriod.Spliter);
            var ave = window.Average;
            sto.Add(group, ave);

            window.Clear();
        }

        /// <summary>
        /// 判断是否断裂
        /// </summary>
        /// <param name="window"></param>
        /// <param name="minCalcuCount"></param>
        /// <param name="maxDiffer"></param>
        /// <param name="currentRowIndex"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private static bool IsBreaked(NumeralWindowData window, int minCalcuCount, double maxDiffer, int currentRowIndex,  double val)
        {
            return window.Count >= minCalcuCount && (window.IsIndexBreaked(currentRowIndex) || Math.Abs(window.AverageValue - val) > maxDiffer);
        }

        /// <summary>
        /// 获取或创建
        /// </summary>
        /// <param name="windowMgr"></param>
        /// <param name="currentIndex"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        private static NumeralWindowData GetOrCreateWindowData(NumeralWindowDataManager windowMgr, object currentIndex, string colName)
        {
            NumeralWindowData window = null;
            if (windowMgr.Contains(colName))
            {
                window = windowMgr[colName];
            }
            else//生成
            {
                window = CreateNewWindow(windowMgr, currentIndex, colName);
            }

            return window;
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="windowMgr"></param>
        /// <param name="currentIndex"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        private static NumeralWindowData CreateNewWindow(NumeralWindowDataManager windowMgr, object currentIndex, string colName)
        {
            NumeralWindowData window = windowMgr.Create(colName);
            windowMgr[colName] = window;
            window.Tag = new Pair<Object>(currentIndex, currentIndex);//记住当前Index
            return window;
        }

        /// <summary>
        /// 计算每个分段的平均数
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="minCalcuCount">最小计算数量</param>
        /// <param name="maxBreakCountInSegment">分段内允许的最大断裂数量</param>
        /// <returns></returns>
        public static ObjectTableStorage GetAverageTableOfEachSegments(ObjectTableStorage table,int minCalcuCount, int maxBreakCountInSegment)
        {
            var indexColName = table.GetIndexColName();
            NumeralWindowDataManager windowMgr = new NumeralWindowDataManager(table.RowCount, maxBreakCountInSegment);
            var store = new GroupedRmsValue<string,string>();

            int i = -1;
            foreach (var dic in table.BufferedValues)
            {
                i++; 
                var index = dic[indexColName];
                foreach (var item in dic)
                {
                    var colName = item.Key;
                    if (indexColName == item.Key) { continue; }
                    //初始化
                    var window =  windowMgr.GetOrCreate(colName);
                    if (window.Count >= minCalcuCount && window.IsIndexBreaked(i))
                    {
                        var sto = store.GetOrCreate(colName);
                        string group = sto.Count + "";
                        var ave = window.Average;
                        sto.Add(group, ave);
                        //清空
                        window.Clear();
                    }

                    if (Geo.Utils.ObjectUtil.IsNumerial(item.Value))
                    {
                        var num = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                        if (Geo.Utils.DoubleUtil.IsValid(num))
                        {
                            window.Add(i, num);
                        }
                    }
                }
            }
            //最后需要计算
            foreach (var item in windowMgr.Data)
            {
                //初始化
                var window = item.Value;
                if (window.Count >= minCalcuCount)
                {
                    var sto = store.GetOrCreate(item.Key);
                    string group = sto.Count + "";
                    var ave = window.Average;
                    sto.Add(group, ave);
                }
            }


            return  store.GetTable("各段求平均_" + table.Name); 
        }

        /// <summary>
        /// 精度评估，对于指定窗口，采用不同的拟合阶次进行拟合后，对指定的列作差，然后统计平均误差。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="toBeFitColName"></param>
        /// <param name="polyfitCount"></param>
        /// <param name="polyfitType"></param>
        /// <param name="polyFitOrder"></param>
        /// <param name="trueOrRawValColName"></param>
        /// <param name="breakCount"></param>
        /// <returns></returns>
        public static ObjectTableStorage PolyfitAccuracyEvaluation(
            ObjectTableStorage table,
            string toBeFitColName,
            int polyfitCount,
            PolyfitType polyfitType,
            List<int> polyFitOrder,
            string trueOrRawValColName = null,//比较列,真值或原始值
            double breakCount = 5)
        {
            DateTime start = DateTime.Now;

            var startOfPolyBuildCount = PolyfitBuilder.BuildCount;

            if (trueOrRawValColName == null) { trueOrRawValColName = toBeFitColName; }

            var countCal = new MovingWindowCounter(polyfitCount, polyfitType);
            log.Info(countCal.ToString());

            var indexHelper = new TypedIndexHelper(table);
            var windowManager = new NumeralWindowDataManager<string, double>(countCal.GetWindowSize(table.RowCount), breakCount * indexHelper.Interval);


            var name = TableNameHelper.BuildName(table.Name, polyfitType + "窗口_" + polyfitCount + "_拟合次数" + Geo.Utils.StringUtil.ToString(polyFitOrder));

            var storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();
            var maxOrder = polyFitOrder.Max();
            var BufferedStream = new BufferedStreamService<Dictionary<string, object>>(table.BufferedValues, countCal.GetBufferSize(table.RowCount));
            //提前加入缓存，利于拟合
            SetupBuffer(BufferedStream, windowManager, indexColName, indexHelper);
            int i = -1;
            //整体拟合器
            BaseDictionary<string, LsPolyFit> wholeFitters = new BaseDictionary<string, LsPolyFit>();
            foreach (var row in BufferedStream)
            {
                if (row == null) { continue; }
                i++;
                var indexValue = row[indexColName];


                bool isIndexColumnAddedOfThisRow = false; //每一行,确定该行有值，才添加索引
                foreach (var kv in row)
                {
                    var key = kv.Key;
                    if (key == (indexColName) || key != toBeFitColName) { continue; }
                    var objVal = kv.Value;

                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }
                    var window = windowManager.GetOrCreate(key);

                    double k = indexHelper.GetDifferKeyValue(indexValue, i);
                    //window.Add(k, (double)val);

                    if (window.Count <= maxOrder) { continue; }

                    //此行有值，是否添加了检索。
                    if (!isIndexColumnAddedOfThisRow)
                    {
                        storage.NewRow();
                        storage.AddItem(indexColName, indexValue);
                        isIndexColumnAddedOfThisRow = true;
                    }
                    //增加比较列,真值或原始值
                    storage.AddItem(trueOrRawValColName, row[trueOrRawValColName]);

                    foreach (var order in polyFitOrder)
                    {
                        RmsedNumeral polyFitVal = null;
                        if (polyfitType == PolyfitType.IndependentWindow || polyfitType == PolyfitType.OverlapedWindow)
                        {
                            polyFitVal = window.GetSectedMovWindowPolyfitValue(order, k, polyfitCount, countCal.MarginCount, true, countCal.OverlapCount);
                        }
                        else if (polyfitType == PolyfitType.MovingWindow)//滑动平均
                        {
                            polyFitVal = window.GetPolyFitValue(k, order, polyfitCount);
                        }
                        else if (polyfitType == PolyfitType.WholeData)
                        {
                            LsPolyFit wholeFitter;
                            var fitKey = key + "_" + order;
                            if (wholeFitters.Contains(fitKey))
                            {
                                wholeFitter = wholeFitters[fitKey];
                            }
                            else
                            {
                                wholeFitter = window.BuildPolyfitter(order);
                                wholeFitters[fitKey] = wholeFitter;
                            }
                            polyFitVal = wholeFitter.GetRmsedY(k);
                        }

                        var keyName = key + "_Win_" + polyfitCount + "_Order_" + order;
                        storage.AddItem(keyName, polyFitVal.Value);
                    }
                }
            }
            //下面进行精度计算
            storage.UpdateAllByMinusCol(trueOrRawValColName);

            var PolyBuildCount = PolyfitBuilder.BuildCount - startOfPolyBuildCount;
            var timespan = DateTime.Now - start;
            var str = "分段窗口拟合时间: " + timespan.ToString() + " , 总秒数: " + timespan.TotalSeconds + " ,总分钟: " + timespan.TotalMinutes + "\r\n";
            str += "拟合器生成次数：" + PolyBuildCount + " , 拟合数量: " + polyfitCount + ",拟合次数: " + Geo.Utils.StringUtil.ToString(polyFitOrder);

            log.Info(str);

         //   var resultTable =  GetAverageTable(storage);
            var resultTable = GetResidualRmseTable(storage);
            resultTable.Name = polyfitType + "法对 " + toBeFitColName + "拟合后，相对于 " + trueOrRawValColName + " 的偏差平均值，其中窗口:" + polyfitCount + "_拟合次数:" + Geo.Utils.StringUtil.ToString(polyFitOrder);
            return resultTable;
        }

        private static void SetupBuffer(
            BufferedStreamService<Dictionary<string, object>> BufferedStream,
            NumeralWindowDataManager<string, double> windowManager,
            string indexColName,
            TypedIndexHelper indexHelper
           )
        {
            int bufferIndex = -1;
            BufferedStream.MaterialInputted += new MaterialEventHandler<Dictionary<string, object>>(m =>
            {
                bufferIndex++;
                var indexValue = m[indexColName];
                double k = indexHelper.GetDifferKeyValue(indexValue, bufferIndex);
                foreach (var item in m)
                {
                    if (String.Equals(item.Key, indexColName)) { continue; }
                    var objVal = item.Value;
                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(objVal)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(objVal, double.NaN);
                    if (val == null || double.IsNaN((double)val)) { continue; }

                    windowManager.GetOrCreate(item.Key).Add(k, (double)val);

                }
            });
        }

        /// <summary>
        /// 精度评估，对于指定窗口，采用不同的拟合阶次进行拟合后，对指定的列作差，然后统计平均误差。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="toBeFitColName"></param>
        /// <param name="polyfitCounts"></param>
        /// <param name="polyfitType"></param>
        /// <param name="polyFitOrder"></param>
        /// <param name="trueOrRawValColName"></param>
        /// <param name="breakCount"></param>
        /// <returns></returns>
        public static ObjectTableStorage PolyfitAccuracyEvaluation(
            ObjectTableStorage table,
            string toBeFitColName,
            List<int> polyfitCounts,
            PolyfitType polyfitType,
            List<int> polyFitOrder,
            string trueOrRawValColName = null,//比较列,真值或原始值
            double breakCount = 5)
        {
            DateTime start = DateTime.Now;
            var name = polyfitType + "法对 " + toBeFitColName + "拟合后，相对于 " + trueOrRawValColName + " 的偏差平均值，其中窗口:" + Geo.Utils.StringUtil.ToString(polyfitCounts) + ",拟合次数:" + Geo.Utils.StringUtil.ToString(polyFitOrder);
            ObjectTableStorage ResultTable = new ObjectTableStorage(name);
            List<string> keys = new List<string>();


            //Parallel.ForEach<int>(polyfitCounts, (polyfitCount) =>
            //{
            //    SingleAccurayEvaluation(table, toBeFitColName, polyfitType, polyFitOrder, trueOrRawValColName, breakCount, polyfitCount, ResultTable, keys);
            //});

            foreach (var polyfitCount in polyfitCounts)
            {
                SingleAccurayEvaluation(table, toBeFitColName, polyfitType, polyFitOrder, trueOrRawValColName, breakCount, polyfitCount, ResultTable, keys);
            }

            return ResultTable;
        }

        private static void SingleAccurayEvaluation(ObjectTableStorage table, string toBeFitColName, PolyfitType polyfitType, List<int> polyFitOrder, string trueOrRawValColName, double breakCount, int polyfitCount, ObjectTableStorage ResultTable, List<string> keys)
        {
            var aveTable = PolyfitAccuracyEvaluation(
                 table,
                 toBeFitColName,
                 polyfitCount,
                 polyfitType,
                 polyFitOrder,
                 trueOrRawValColName,//比较列,真值或原始值
                 breakCount);

            foreach (var row in aveTable.BufferedValues)
            {
                if (row.ContainsKey("Name") && keys.Contains(row["Name"].ToString()))
                {
                    continue;
                }
                else { keys.Add(row["Name"].ToString()); }

                ResultTable.NewRow();
                foreach (var item in row)
                {
                    ResultTable.AddItem(item.Key, item.Value);
                }
            }
        }
    }
}