//2016.03.22, czs, created in hongqing, 基于卫星结果的管理器 
//2016.03.29, czs, edit in hongqing, 名称修改为 NamedValueTableManager
//2016.08.05, czs, edit in fujian yongan, 重构
//2016.10.03, czs, edit in hongqing,增加缓存结果数量控制，便于控制内存大小
//2016.10.19, czs, edit in hongqing, 增加一些计算分析功能
//2016.10.26, czs, edit in hongqing, 表格值从字符串修改为 Object，减少转换损失
//2016.10.28, czs, edit in hongqing, 去掉了与输出相关的全局设置，后续可考虑输出单独分离成一个类。更名为TableObjectStorage
//2017.02.08, czs, edit in hongqing, 增加检索列的名称，增加列数据相减等方法
//2017.07.07, czs, edit in HMX, 增加一些统计分析功能
//2019.02.20, czs, edit in hongqing, TableObjectStorage 更名为 TableObjectStorage

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
using Geo.Algorithm;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 参数数值表格管理器，用于表格化输出参数。适合用于存储稀疏表格数据，可以做适当的数据统计和分析。
    /// 当达到指定缓存大小后，将写入文件。而文件最后一行为全部的参数。
    /// </summary>
    public class ObjectTableStorage : IDisposable
    {
        Log log = new Log(typeof(ObjectTableStorage));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称，也用于文件识别和存储</param>
        public ObjectTableStorage(string name = "未命名表")
        {
            if (name == "未命名表")
            {
                int a = 0;
            }


            // this.OutputPath = outputPath;
            this.Name = name;
            this.NameListManager = new NameListManager();
            this.BufferedValues = new List<Dictionary<string, Object>>();
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="table"></param>
        /// <param name="name"></param>
        public ObjectTableStorage(ObjectTableStorage table, string name = "未命名表")
        {  
            this.Name = name;
            this.NameListManager = new NameListManager();
            this.BufferedValues = new List<Dictionary<string, Object>>();
            foreach (var item in table.BufferedValues)
            {
                this.NewRow();                
                this.AddItem(item);
            }
        }

        /// <summary>
        /// 采用系统表初始化
        /// </summary>
        /// <param name="table"></param>
        public ObjectTableStorage(DataTable table)
        {
            this.NameListManager = new NameListManager();
            this.BufferedValues = new List<Dictionary<string, Object>>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                this.NewRow();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    this.AddItem(table.Columns[j].ColumnName, table.Rows[i][j]);
                }
                this.EndRow();
            }
        }

        #region 属性
        /// <summary>
        /// 第一个检索
        /// </summary>
        public Object FirstIndex { get { return FirstRow[GetIndexColName()]; } }
        /// <summary>
        /// 第一个检索
        /// </summary>
        public Object SecondIndex { get { return SecondRow[GetIndexColName()]; } }
        /// <summary>
        /// 最后一个检索
        /// </summary>
        public Object LastIndex { get { return LastRow[GetIndexColName()]; } }
        /// <summary>
        /// 第一行
        /// </summary>
        public Dictionary<string, Object> FirstRow { get { return this.BufferedValues[0]; } }
        /// <summary>
        /// 第2行
        /// </summary>
        public Dictionary<string, Object> SecondRow { get { return this.BufferedValues[1]; } }
        /// <summary>
        /// 最后一行
        /// </summary>
        public Dictionary<string, Object> LastRow { get { return this.BufferedValues[RowCount - 1]; } }
        string _name = null;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get => _name;
            set {
                if (_name == null) {
                    _name = value;
                }
                else if(_name != value)
                { 
                    _name = value;
                }
            }
        }
        /// <summary>
        /// 没有后缀名的名称
        /// </summary>
        public string NoExtensionName { get { return Path.GetFileNameWithoutExtension(Name); } }

        string _IndexColName { get; set; }
        /// <summary>
        /// 检索列名称。不返回 null
        /// </summary>
        public string IndexColName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_IndexColName))
                {
                    _IndexColName = this.ParamNames[0];
                }
                return _IndexColName;
            }
            set { _IndexColName = value; }
        }
        /// <summary>
        /// 获取检索列名称，如果没有指定，则采用第一列的名称。
        /// </summary>
        public string GetIndexColName()
        {
            if (String.IsNullOrEmpty(IndexColName))
            {
                if (this.ParamNames.Count == 0)
                {
                    return null;
                }
                return this.ParamNames[0];
            }
            return IndexColName;
        }
        /// <summary>
        /// 参数名称顺序
        /// </summary>
        public List<string> ParamNames { get { return NameListManager.Data; } }
        /// <summary>
        /// 获取一个非检索的列名称
        /// </summary>
        /// <returns></returns>
        public string GetColNameNotIndex()
        {
            var index = GetIndexColName();
            foreach (var item in this.ParamNames)
            {
                if (item != index)
                {
                    return item;
                }
            }
            return "";
        }

        #region 核心存储
        /// <summary>
        /// 名称列表管理器
        /// </summary>
        public NameListManager NameListManager { get; set; }
        /// <summary>
        /// 缓存的数据,核心存储
        /// </summary>
        public List<Dictionary<string, Object>> BufferedValues { get; set; }
        /// <summary>
        /// 当前行,保存在数据中的最新行。具体位置决定于采用的方法，如 Add or Insert 
        /// </summary>
        public Dictionary<string, Object> CurrentRow { get; set; }
        #endregion

        /// <summary>
        /// 参数数量。列数。
        /// </summary>
        public int ColCount { get { return this.ParamNames.Count; } }
        /// <summary>
        /// 行数。
        /// </summary>
        public int RowCount { get { return this.BufferedValues.Count; } }
        /// <summary>
        /// 最后一行的编号
        /// </summary>
        public int IndexOfLastRow { get { return RowCount - 1; } }

        #endregion

        #region 方法
        #region  索引
        /// <summary>
        /// 索引
        /// </summary>
        public Dictionary<string, Dictionary<object, object>> Indexes { get; set; }
        /// <summary>
        /// 是否建立了索引。
        /// </summary>
        public bool HasIndexes { get { return Indexes != null && Indexes.Count > 0; } }
        /// <summary>
        /// 手动建立索引
        /// </summary>
        public void BuildIndexes()
        {
            Indexes = new Dictionary<string, Dictionary<object, object>>();
            foreach (var name in this.ParamNames)
            {
                Indexes.Add(name, GetColObjectDic(name));
            }
        }

        /// <summary>
        /// 返回单元格数据，若无返回null,默认以第一个
        /// </summary>
        /// <param name="indexObject"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public object this[object indexObject, string colName]
        {
            get
            {
                var col = this[colName];

                if (col != null && col.ContainsKey(indexObject))
                {
                    return col[indexObject];
                }
                return null;
            }
        }
        /// <summary>
        ///获取值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="indexObject"></param>
        /// <param name="colName"></param>
        /// <param name="defaultValue">失败后，默认返回。</param>
        /// <returns></returns>
        public TValue GetValue<TValue>(object indexObject, string colName, TValue defaultValue = default(TValue))
        {
            var col = this[colName];
            
            if (col != null && col.ContainsKey(indexObject) && col[indexObject]!=null)
            {
                var obj = col[indexObject];
                if(obj is TValue)
                {
                    return (TValue)obj;
                }
                return defaultValue;
            } 
            return defaultValue;
        }

        /// <summary>
        /// 返回指定列的数据
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        private Dictionary<object, object> this[string colName]
        {
            get
            {
                if (!this.ParamNames.Contains(colName)) { return null; }
                if (!HasIndexes) { BuildIndexes(); }
                var col = Indexes[colName];
                return col;
            }
        }


        #endregion
        /// <summary>
        /// 第一个索引对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetFirstIndexValue<T>() { return (T)FirstIndex; }
        /// <summary>
        /// 最后一个索引对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetLastIndexValue<T>() { return (T)LastIndex; }

        #region 排序，转置
        /// <summary>
        /// 转置
        /// </summary>
        public ObjectTableStorage Transpose(string namePostfix = "Transpose", bool isReplacePrevPosfix = true)
        {
            var newName = TableNameHelper.BuildName(this.Name, namePostfix, isReplacePrevPosfix);
            var table = new ObjectTableStorage(newName);
            var indexes = this.GetIndexValues();
            var indexName = GetIndexColName();
            foreach (var oldColName in this.ParamNames)
            {
                var oldCols = this.GetColObjectDic(oldColName);
                if (indexName.Equals(oldColName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                table.NewRow();
                int i = -1;
                foreach (var oldCol in oldCols)
                {
                    i++;
                    var newColName = oldCol.Key;
                    var newColValue = oldCol.Value;

                    if (i == 0) { table.AddItem("Name", oldColName); }

                    table.AddItem(newColName, newColValue);
                }

                table.EndRow();
            }
            return table;
        }
        /// <summary>
        /// 尝试获取采样率
        /// </summary>
        /// <returns></returns>
        public double GetInterval<TKey>(Func<TKey, double> KeyToDouble =  null)
        {
            Geo.Utils.DoubleUtil.AutoSetKeyToDouble<TKey>(out KeyToDouble);

            return Math.Abs(  KeyToDouble((TKey)this.SecondIndex) - KeyToDouble((TKey)this.FirstIndex));
        }

        /// <summary>
        /// 根据输入的参考key，获取列中连续的数据。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="referKey"></param>
        /// <param name="colName"></param>
        /// <param name="KeyToDouble"></param>
        /// <param name="breakGap">若超过此，则认为断裂</param>
        /// <returns></returns>
        public NumeralWindowData<TKey> GetContinuousNumeralWindowData<TKey>(TKey referKey, string colName, Func<TKey, double> KeyToDouble, double breakGap = 5)
          where TKey : IComparable<TKey>
        {
            InteruptableData<TKey> data = GetInteruptableData(colName, KeyToDouble, breakGap);
            return data.GetNumeralWindowData(referKey);
        }

        /// <summary>
        ///  根据输入的参考key，获取列中连续的数据。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="KeyToDouble"></param>
        /// <param name="breakGap"></param>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public BaseDictionary<string, InteruptableData<TKey>> GetInteruptableData<TKey>(Func<TKey, double> KeyToDouble, double breakGap, List<string> paramNames = null)
            where TKey : IComparable<TKey>
        {
            var manager = new BaseDictionary<string, InteruptableData<TKey>>();
            if (paramNames == null) { paramNames = this.ParamNames; }
            foreach (var item in paramNames)
            {
                if (item == GetIndexColName()) { continue; }

                var dat = GetInteruptableData(item, KeyToDouble, breakGap);

                manager.Add(item, dat);
            }
            return manager;
        }


            /// <summary>
            ///  根据输入的参考key，获取列中连续的数据。
            /// </summary>
            /// <typeparam name="TKey"></typeparam>
            /// <param name="colName"></param>
            /// <param name="KeyToDouble"></param>
            /// <param name="breakGap"></param>
            /// <returns></returns>
       public InteruptableData<TKey> GetInteruptableData<TKey>(string colName, Func<TKey, double> KeyToDouble, double breakGap) where TKey : IComparable<TKey>
        {
            var dic = GetIndexColDic<TKey, double>(colName);
            var data = new InteruptableData<TKey>(dic, KeyToDouble, breakGap);
            return data;
        }


        /// <summary>
        /// 反转行
        /// </summary>
        public void ReverseRows() { this.BufferedValues.Reverse(); }
        /// <summary>
        /// 排序参数名称
        /// </summary>
        public void SortColumns() { this.ParamNames.Sort(); }

        #endregion

        /// <summary>
        /// 获取指定列的类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Type GetType(string name)
        {
            foreach (var dic in this.BufferedValues)
            {
                foreach (var item in dic)
                {
                    if (String.Equals(name, item.Key, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (item.Value != null)
                        {
                            return item.Value.GetType();
                        }
                    }
                }

            }
            return typeof(String);
        }

        #region 添加数据
        /// <summary>
        /// 新建一个行，并追加到末尾，如果当前行尚未写值，则不新建。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Object> NewRow()
        {
            if (CurrentRow == null || CurrentRow.Count > 0)
            {
                CurrentRow = new Dictionary<string, Object>();
                BufferedValues.Add(CurrentRow);
            }
            return CurrentRow;
        }

        /// <summary>
        /// 在指定行插入新行
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Dictionary<string, Object> InsertNewRow(int index)
        {
            if (CurrentRow == null || CurrentRow.Count > 0)
            {
                CurrentRow = new Dictionary<string, Object>();
                BufferedValues.Insert(index, CurrentRow);
            }
            return CurrentRow;
        }

        /// <summary>
        /// 新建一行。
        /// </summary>
        /// <param name="vector"></param>
        public void NewRow(IVector vector)
        {
            this.NewRow();
            AddItem(vector);
        }
        /// <summary>
        /// 新建一行。
        /// </summary>
        /// <param name="matrix"></param>
        public void NewRow(IMatrix matrix)
        {
            this.NewRow();
            AddItem(matrix);
        }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="matrix"></param>
        public void AddItem(IMatrix matrix)
        {
            //if (matrix.IsColNameAvailable)
            //{


            //}

            //if (matrix.IsRowNameAvailable)
            //{


            //}

            int index = 0;
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColCount; j++)
                {
                    this.AddItem(index++, matrix[i, j]);
                  //  this.AddItem(i + "_" + j, matrix[i, j]);
                }
            } 
        }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="vector"></param>
        public void AddItem(IVector vector)
        {
            if (vector == null) { return; }
            int i = 0;
            foreach (var item in vector)
            {
                var key = i + "";
                if (vector.ParamNames != null && vector.ParamNames.Count == vector.Count)
                {
                    key = vector.ParamNames[i];
                    if (key == null)
                    {
                        key = i + "";
                    }
                }
                this.AddItem(key, vector[i]);
                i++;
            }
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="vector"></param>
        public void AddItem(WeightedVector vector)
        {
            if (vector == null) { return; }
            int i = 0;
            var vals = vector.GetRmsedVector();
            foreach (var item in vector)
            {
                var key = i + "";
                if (vector.ParamNames != null && vector.ParamNames.Count == vector.Count)
                {
                    key = vector.ParamNames[i];
                    if (key == null)
                    {
                        key = i + "";
                    }
                }
                this.AddItem(key, vals.GetItem(i));
                i++;
            }
        }


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="dic"></param>
        public void AddItem(Dictionary<string, double[]> dic)
        {
            if (dic.Count == 0) { return; }

            var first = Geo.Utils.DictionaryUtil.GetFirstValue<string, double[]>(dic);

            int len = first.Length; ;
            for (int i = 0; i < len; i++)
            {
                this.NewRow();
                foreach (var item in dic) { this.AddItem(item.Key, item.Value[i]); }
                this.EndRow();
            }
        }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="dic"></param>
        public void AddItem(Dictionary<string, RmsedNumeral> dic)
        {
            if (dic.Count == 0) { return; }
            foreach (var item in dic)
            {
                this.AddItem(item.Key, item.Value.Value);
            }
            this.EndRow();
        }

        #region 添加字典数据结构
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="dic"></param>
        public void AddItem(Dictionary<string, int> dic) { foreach (var item in dic) { this.AddItem(item.Key, item.Value); } }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="dic"></param>
        public void AddItem(Dictionary<string, double> dic) { foreach (var item in dic) { this.AddItem(item.Key, item.Value); } } 
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="dic"></param>
        public void AddItem(Dictionary<string, object> dic) { foreach (var item in dic) { this.AddItem(item.Key, item.Value); } }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="dic"></param>
        public void AddItem(BaseDictionary<string, object> dic) { foreach (var item in dic.Keys) { this.AddItem(item, dic[item]); } }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="dic"></param>
        public void AddItem(BaseDictionary<string, double> dic) { foreach (var item in dic.Keys) { this.AddItem(item, dic[item]); } }

      
        /// <summary>
        /// 结束行，若当前行没有数据，则删除之。
        /// </summary> 
        public void EndRow()
        {
            if (CurrentRow.Count == 0)
            {
                //  CurrentRow = new Dictionary<string, Object>();
                BufferedValues.Remove(CurrentRow);
                CurrentRow = null;
            }
        }
        /// <summary>
        /// 添加一组项目
        /// </summary>
        /// <param name="names"></param>
        /// <param name="vals"></param>
        public void AddItem(string[] names, object[] vals)
        {
            int length = names.Length;
            for (int i = 0; i < length; i++)
            {
                this.AddItem(names[i], vals[i]);
            }
        }
        /// <summary>
        /// 添加一个项目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void AddItem(object name, object val) { AddItem(name + "", val); }
        /// <summary>
        /// 添加一个项目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void AddItem(string name, object val) { this.Regist(name); if(!CurrentRow.ContainsKey(name)) CurrentRow.Add(name, val); }
        /// <summary>
        /// 移除满足条件的格子，以NaN代替。
        /// </summary>
        /// <param name="removeCondition"></param>
        /// <param name="nameSuffix"></param>
        /// <returns></returns>
        public ObjectTableStorage RemoveCellWith(Func<double, bool> removeCondition, string nameSuffix ="RemovedSome")
        { 
            if (nameSuffix == "RemovedSome")
            {
                nameSuffix = removeCondition.ToString();
            }
            return HandleAllNumeralCellValue((key, NumeralVal) =>
            {
               if(removeCondition(NumeralVal))
                {
                    return double.NaN;
                }
                return NumeralVal;
            }, false, nameSuffix, true);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="remainOrNot">保留或排除</param>
        /// <returns></returns>
        public ObjectTableStorage Search(ConnectedNumCondition condition, bool remainOrNot)
        {
            return HandleAllNumeralCellValue((key, NumeralVal) =>
            {
                if (remainOrNot && condition.IsSatisfy(NumeralVal))
                {
                    return NumeralVal;
                }
                return double.NaN;
            }, false, "搜索结果", true);
        }


        #endregion
        #endregion

        #region 注册名称
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="names"></param>
        public void Regist(IEnumerable<string> names) { foreach (var item in names) { Regist(item); } }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="colName"></param>
        public void Regist(string colName) { if (!NameListManager.Contains(colName)) { NameListManager.Add(colName); } }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="mainName"></param>
        /// <param name="subName"></param>
        public void Regist(string mainName, string subName) { Regist(mainName, new List<string> { subName }); }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="mainName"></param>
        /// <param name="subNames"></param>
        public void Regist(string mainName, List<string> subNames) { if (!NameListManager.Contains(mainName)) { NameListManager.Add(mainName, subNames); } }
        #endregion


        #region 质量控制
        /// <summary>
        /// 以最后数据为依据，超过指定数值的滤掉。
        /// </summary>
        /// <param name="radius"></param>
        public void PipleFilterWithLastValues(double radius)
        {
            var lastVals = this.GetLastValueOfAllCols();
            foreach (var dic in lastVals)
            {
                PipleFilter(dic.Key, dic.Value, radius);
            }
        }


        /// <summary>
        /// 管道滤波，移除范围外的数据
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public void PipleFilter(string colName, double center, double radius)
        {
            double max = center + radius;
            double min = center - radius;
            foreach (var row in this.BufferedValues)
            {
                if (row.ContainsKey(colName))
                {
                    var val = Geo.Utils.ObjectUtil.GetNumeral(row[colName]);
                    if (Geo.Utils.DoubleUtil.IsValid(val))
                    {
                        if (val < min || val > max)
                        {
                            row.Remove(colName);
                        }
                    }
                }
            }
        }

        #endregion


        #region 计算、转换、统计分析

        #region 数据转换
        /// <summary>
        /// 将所有的列归算到指定的周期区间。
        /// </summary> 
        /// <param name="period"></param>
        /// <param name="reference">第一个参数的参考值，后续可更新为第一个的平均数</param>
        /// <param name="isUpdateRefer">是否更新参考值</param>
        public void ReductValuesTo(double period = 1, double reference = 0.5, bool isUpdateRefer = true)
        {
            var refer = reference;
            foreach (var item in this.ParamNames)
            {
                refer = ReductValuesTo(item, period, reference);
                if (isUpdateRefer) { reference = refer; }

            }
        }

        /// <summary>
        /// 将指定的列归算到指定的区间。
        /// 返回实际的归算值，可以作为下一个的参考值，但可能造成整体性偏差。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="period"></param>
        /// <param name="reference"></param>
        public double ReductValuesTo(string paramName, double period = 1, double reference = 0.5)
        {
            var vector = GetVector(paramName);
            if (vector.Count == 0) { return 0; }
            var ave = GetAverage(paramName);

            var PeriodFilter = new PeriodPipeFilter(period, reference);
            var differ = PeriodFilter.Filter(ave);
            var periodCount = PeriodFilter.LastCutingPeriodCount;
            this.UpdateColumnByMinus(paramName, periodCount);
            return differ;
        }
        #endregion

        #region 提取新表

        /// <summary>
        /// 获取平滑后的数据。所有当初白噪声，采用一维卡拉曼滤波平滑。
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetSquentionAjustSmoothedTable(double maxError = 2, bool isAsWhole = true, string namePostfix = "Smoothed", bool isReplaceNamePosfix = true)
        {
            var name = TableNameHelper.BuildName(this.Name, namePostfix, isReplaceNamePosfix);
            var storage = new ObjectTableStorage(name);
            string indexColName = GetIndexColName();
            var WideLaneFilterManager = new CyclicalNumerFilterManager(maxError, false);
            // int i = -1;
            for (int i = 0; i < this.RowCount; i++)
            {
                var dic = this.BufferedValues[i];
                storage.NewRow();
                storage.AddItem(indexColName, dic[indexColName]);
                foreach (var kv in dic)
                {
                    if (kv.Key == (indexColName)) { continue; }

                    var filter = WideLaneFilterManager.GetOrCreate(kv.Key);

                    if (Geo.Utils.StringUtil.IsDecimalOrNumber(kv.Value))
                    {
                        var val = Geo.Utils.DoubleUtil.TryParse(kv.Value);
                        if (val != null)
                        {
                            RmsedNumeral n = new RmsedNumeral((double)val, 1);
                            if (filter.SetRawValue(n).IsBufferNeeded)
                            {
                                var Buffers = new NumeralWindowData(GetBuffer(kv.Key, i + 1, 5));
                                filter.SetBuffer(Buffers);
                            }
                            filter.Calculate();
                            var smooth = 0.0;
                            if (isAsWhole)
                            {
                                smooth = filter.CurrentFilteredFraction.Value;
                            }
                            else
                            {
                                smooth = filter.CurrentFilteredValue.Value;
                            }

                            var key = kv.Key;
                            storage.AddItem(key, smooth);
                        }
                    }
                }

                storage.EndRow();
            }

            return storage;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="from"></param>
        /// <param name="isSkipNonNumerial"></param>
        /// <returns></returns>
        public List<double> GetBuffer(string paramName, int from, int count, bool isSkipNonNumerial = true, double defaultValue = Double.NaN)
        {
            return GetVector(paramName, from, count, isSkipNonNumerial, defaultValue);
        }
        /// <summary>
        /// 获取指定列的数据
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="fromIndexValue"></param>
        /// <param name="toIndexValue">含</param>
        /// <returns></returns>
        public Dictionary<object, object> GetColObjectDicByObjIndex(string colName, Object fromIndexValue = null, Object toIndexValue = null)
        {
          //  FillIndexes();

            var fromRowIndex = GetRowIndexOfIndexCol(fromIndexValue);
            if (fromRowIndex == -1) { return null; }
            var toRowIndex = GetRowIndexOfIndexCol(toIndexValue);
            if (toRowIndex == -1)
            {
                toRowIndex = this.RowCount - 1;
            }
            return GetColObjectDic(colName, fromRowIndex, toRowIndex - fromRowIndex + 1);
        }
        /// <summary>
        /// 克隆一个
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage Clone()
        {
            return new ObjectTableStorage(this, "Clone_" + this.Name);
        }
        /// <summary>
        /// 获取列对象字典，key为索引，Value为值。
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public BaseDictionary<TKey, TValue> GetIndexColDic<TKey, TValue>(string colName, int fromIndex = 0, int count = int.MaxValue)
        {
            BaseDictionary<TKey, TValue> dic = new BaseDictionary<TKey, TValue>(); 
            var indexColName = this.GetIndexColName();
            int endIndex = fromIndex + count; 
            endIndex = endIndex < this.RowCount ? endIndex : this.RowCount;
            for (int i = fromIndex; i < endIndex; i++)
            {
                var item = this.BufferedValues[i];
                if (item.ContainsKey(colName))
                {
                    var key = item[indexColName];
                    var val = item[colName];
                    if(!Geo.Utils.ObjectUtil.IsNumerial(val)) { continue; }

                    dic[(TKey)key] = (TValue)val;
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取列对象字典，key为索引，Value为值。
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Dictionary<object, object> GetColObjectDic(string colName, int fromIndex = 0, int count = int.MaxValue)
        {
            Dictionary<object, object> dic = new Dictionary<object, object>();
            var indexColName = this.GetIndexColName();
            int endIndex = fromIndex + count;
            //endIndex = endIndex < this.RowCount ? endIndex : this.RowCount - 1;
            //李林阳修改，以前是this.RowCount - 1
            endIndex = endIndex < this.RowCount ? endIndex : this.RowCount;
            for (int i = fromIndex; i < endIndex; i++)
            {
                var item = this.BufferedValues[i];
                if (item.ContainsKey(colName))
                {
                    var key = item[indexColName];
                    var val = item[colName];
                    dic[key] = val;
                }
            }
            return dic;
        }
        /// <summary>
        /// 增加一个白噪声列，用于测试
        /// </summary>
        public void AddNewWhiteNoiseCol(int seed = 1, int polyOrder = 2, string colName = null)
        {
            var tempName = colName ?? "WhiteNoise";
            for (int i = 0; i < int.MaxValue; i++)
            {
                tempName = "WhiteNoise" + i;
                if (!this.ParamNames.Contains(tempName))
                {
                    colName = tempName;
                    break;
                }
            }
            Random random = new Random(seed);

            double[] Params = new double[polyOrder+ 1];
            for (int i = 0; i < polyOrder+1; i++)
            {
                Params[i] = random.NextDouble();
            }

            PolyVal polyVal = new PolyVal(Params);
            log.Debug("PolyParams: " + Geo.Utils.StringUtil.ToString(Params));

            this.ParamNames.Add(colName);
            int k = -this.RowCount / 2;
            foreach (var row in this.BufferedValues)
            {
                row[colName] = polyVal.GetY(k * 0.01) + random.NextDouble();
                k++;
            } 
        }

        /// <summary>
        /// 获取列对象
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<object> GetColObjects(string colName)
        {
            var dic = new List<object>();
            //var indexColName = this.GetIndexColName();
            foreach (var item in this.BufferedValues)
            {
                if (item.ContainsKey(colName))
                {
                    //var keyPrev = key[indexColName];
                    var val = item[colName];
                    dic.Add(val);
                }
            }
            return dic;
        }
        /// <summary>
        /// 获取列,数值以字符串保存。
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<string> GetColStrings(string colName)
        {
            var dic = new List<string>();
            //var indexColName = this.GetIndexColName();
            foreach (var item in this.BufferedValues)
            {
                if (item.ContainsKey(colName))
                {
                    //var keyPrev = key[indexColName];
                    var val = item[colName] + "";
                    dic.Add(val);
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取指定行
        /// </summary>
        /// <param name="index"></param> 
        /// <returns></returns>
        public Dictionary<string, object> GetRow(int index)
        {
            return this.BufferedValues[index];
        }
        /// <summary>
        /// 获取指定行
        /// </summary>
        /// <param name="match"></param> 
        /// <returns></returns>
        public Dictionary<string, object> GetRow(Func<Dictionary<string, object>, bool> match)
        {
            foreach (var item in this.BufferedValues)
            {
                if (match(item)) { return item; }
            }
            return null;
        }
        /// <summary>
        /// 获取指定行
        /// </summary>
        /// <param name="fromIndex">包含</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetRows(int fromIndex, int count)
        {
            var buffer = this.BufferedValues.GetRange(fromIndex, count);
            return buffer;
        }
        /// <summary>
        /// 所有列，减去第一个有效数据。
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetTableAllColMinusFirstValid()
        {
            var fisrtVal = this.GetFirstValidValue();
            return GetTableAllColMinusOf(fisrtVal);
        }
        /// <summary>
        /// 所有列，减去最后一行数据。
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetTableAllColMinusLastRow()
        {
            var val = this.LastRow;
            return GetTableAllColMinusOf(val);
        }
        /// <summary>
        /// 所有列作差
        /// </summary>
        /// <param name="baseRow"></param>
        /// <returns></returns>
        public ObjectTableStorage GetTableAllColMinusOf(Dictionary<string, object> baseRow)
        {
            var storage = new ObjectTableStorage(this.Name);
            string indexColName = GetIndexColName();

            var numervalRow = Geo.Utils.DictionaryUtil.GetNumeralRow(baseRow);
            

            int i = -1;
            foreach (var row in this.BufferedValues)
            {
                i++;
                storage.NewRow();
                foreach (var kv in row)
                {
                    var colName = kv.Key;
                    if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }
                    if (!numervalRow.ContainsKey(kv.Key))
                    {
                        continue;
                    }
                    var refVal = numervalRow[kv.Key];
                    if ( !Geo.Utils.StringUtil.IsDecimalOrNumber(kv.Value)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(kv.Value);
                    if (val != null)
                    {
                        var differ = (double)val - refVal;
                        storage.AddItem(kv.Key, differ);
                    }
                }
            }
            return storage;
        }

        

        /// <summary>
        /// 所有列，减去最后一个有效数据。
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetTableAllColMinusLastValid()
        {   
            var storage = new ObjectTableStorage(this.Name);
            string indexColName = GetIndexColName();
             
            var fisrtVal =  this.GetLastValidValue();
            foreach (var row in this.BufferedValues)
            { 
                storage.NewRow();
                foreach (var kv in row)
                {
                    var colName = kv.Key;
                    if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }
                    var first = fisrtVal[kv.Key];
                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(first) || !Geo.Utils.StringUtil.IsDecimalOrNumber(kv.Value)) { continue; }

                    var val = Geo.Utils.DoubleUtil.TryParse(kv.Value);
                    var firtV = Geo.Utils.DoubleUtil.TryParse(first);
                    if (val != null && firtV != null)
                    {
                        var differ = (double)val - (double)firtV;
                        storage.AddItem(kv.Key, differ);
                    }
                }
            }
            return storage;
        }

        /// <summary>
        /// 采用递增整数更新索引列。
        /// </summary>
        public void UpdateIndexColValuesWithAscendingInteger()
        {
            var indexName = this.GetIndexColName();
            RemoveCol(indexName);
            InsertIndexCol(indexName);
        }

        /// <summary>
        /// 插入索引列
        /// </summary>
        /// <param name="indexName"></param>
        public void InsertIndexCol(string indexName = "Index")
        {
            int num = 0;
            while (this.ParamNames.Contains(indexName))
            {
                indexName += num ++;
            }
            num = 0;
            foreach (var row in this.BufferedValues)
            {
                row[indexName] = num++; 
            }
            this.NameListManager.Insert(0, indexName);
            this.IndexColName = indexName;
        }
        
        /// <summary>
        /// 表格。保留正小数
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetNewSamplingTable(int interval, string tableName=null)
        {
            if(tableName == null) { tableName = this.Name; }
            var storage = new ObjectTableStorage(this.Name);
            string indexColName = GetIndexColName();

            int i = 0;
            foreach (Dictionary<string, Object> row in this.BufferedValues)
            {
                if (i % interval == 0)
                {
                    storage.NewRow();
                    storage.AddItem(row);
                }
                i++;
            }
            return storage;
        }
        /// <summary>
        /// 只返回指定列的表格
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="titles"></param>
        /// <returns></returns>
        public ObjectTableStorage GetTable(string tableName, List<string> titles)
        {
            var storage = new ObjectTableStorage(tableName);
            string indexColName = GetIndexColName();

            foreach (Dictionary<string, Object> row in this.BufferedValues.ToList())
            {
                storage.NewRow();
                foreach (var kv in row)
                {
                    var colName = kv.Key;
                    if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }

                    if (titles.Contains(colName))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                    }
                }
            }
            return storage;
        }
        /// <summary>
        /// 表格。保留正小数
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetTable(string tableName, Func<double, double> convertValTo)
        {
            var storage = new ObjectTableStorage(tableName);
            string indexColName = GetIndexColName();

            foreach (Dictionary<string, Object> row in this.BufferedValues)
            {
                storage.NewRow();
                foreach (var kv in row)
                {
                    var colName = kv.Key;
                    if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }
                    var currentObj = kv.Value;
                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(currentObj)) { continue; }

                    var currentVal = Geo.Utils.DoubleUtil.TryParse(currentObj);
                    if (currentVal != null)
                    {
                        double val = (double)currentVal;
                      //  var newVal = Geo.Utils.DoubleUtil.GetRoundFraction(val);// - Math.Floor(val); 
                        var newVal = convertValTo(val);// - Math.Floor(val); 
                        storage.AddItem(kv.Key, newVal);
                    }
                }
            }
            return storage;
        }


        /// <summary>
        /// 表格。返回四舍五入的表格。
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetRoundTable()
        {
            var storage = new ObjectTableStorage(this.Name);
            string indexColName = GetIndexColName();

            foreach (Dictionary<string, Object> row in this.BufferedValues)
            {
                storage.NewRow();
                foreach (var kv in row)
                {
                    var colName = kv.Key;
                    if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }
                    var currentObj = kv.Value;
                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(currentObj)) { continue; }

                    var currentVal = Geo.Utils.DoubleUtil.TryParse(currentObj);
                    if (currentVal != null)
                    {
                        double val = Math.Round((double)currentVal); 
                        storage.AddItem(kv.Key, val);
                    }
                }
            }
            return storage;
        }
        /// <summary>
        /// 差分表格。后一行，减去前一行。如果前一行没有数据，则不减，该行值置0。
        /// 如果前行无数据，则不减。
        /// 编号以第二行为准。
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetTableDifferPrevValue()
        {
            var storage = new ObjectTableStorage(this.Name);
            string indexColName = GetIndexColName();

            Dictionary<string, Object> prevRow = null;
            foreach (Dictionary<string, Object> row in this.BufferedValues)
            {
                if (prevRow == null) { prevRow = row; continue; }

                storage.NewRow();
                foreach (var kv in row)
                {
                    var colName = kv.Key;
                    if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }
                    if (!prevRow.ContainsKey(kv.Key)) { continue; }
                    var prevObj = prevRow[kv.Key];
                    var currentObj = kv.Value;
                    if (!Geo.Utils.StringUtil.IsDecimalOrNumber(prevObj) || !Geo.Utils.StringUtil.IsDecimalOrNumber(currentObj)) { continue; }

                    var currentVal = Geo.Utils.DoubleUtil.TryParse(currentObj);
                    var prevVaL = Geo.Utils.DoubleUtil.TryParse(prevObj);
                    if (currentVal != null && prevVaL != null)
                    {
                        var differ = (double)currentVal - (double)prevVaL;
                        storage.AddItem(kv.Key, differ);
                    }
                }
                prevRow = row;
            }
            return storage;
        }
        /// <summary>
        /// 提取值表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetValueTable()
        {
            ObjectTableStorage result = new ObjectTableStorage("值_" + this.Name);
            foreach (var row in this.BufferedValues)
            {
                result.NewRow();
                foreach (var item in row)
                {
                    if (item.Value is RmsedNumeral)
                    {
                        double val = ((RmsedNumeral)item.Value).Value;
                        result.AddItem(item.Key, val);
                    }
                    else
                    {
                        result.AddItem(item.Key, item.Value);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 提取值表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetRmsValueTable()
        {
            ObjectTableStorage result = new ObjectTableStorage("Rms_" + this.Name);
            foreach (var row in this.BufferedValues)
            {
                result.NewRow();
                foreach (var item in row)
                {
                    if (item.Value is RmsedNumeral)
                    {
                        double val = ((RmsedNumeral)item.Value).Rms;
                        result.AddItem(item.Key, val);
                    }
                    else
                    {
                        result.AddItem(item.Key, item.Value);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 返回新的表通过减去指定的列。如果指定咧没有数据，则跳过
        /// </summary>
        /// <param name="substractorColName"></param>
        /// <param name="tablePostfixName"></param>
        /// <param name="isReplacePrevPosfix">是否移除当前表格的后缀</param>
        /// <returns></returns>
        public ObjectTableStorage GetTableByMinusCol(string substractorColName, string tablePostfixName = "-ColName", bool isReplacePrevPosfix = false, bool isUpdateKeyWithCol = false)
        {
            var name = this.Name;
            if (isReplacePrevPosfix)
            {
                name = TableNameHelper.ParseName(name);
            }
            if (tablePostfixName == "-ColName")
            {
                tablePostfixName = "-" + substractorColName;
            }
            name += tablePostfixName;

            var storage = new ObjectTableStorage(name);
            string indexColName = GetIndexColName();

            int i = -1;
            foreach (var row in this.BufferedValues)
            {
                i++;
                if (!row.ContainsKey(substractorColName)) { continue; }

                storage.NewRow();
                //storage.AddItem(indexColName, key[indexColName]);//下面已经添加
                var substractor = Geo.Utils.ObjectUtil.GetNumeral(row[substractorColName]);
                if (!Geo.Utils.DoubleUtil.IsValid(substractor)) { continue; }

                foreach (var kv in row)
                {
                    var colName = kv.Key;
                    if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }
                    //减去自身为 0 ，不再浪费资源。
                    if (colName.Equals(substractorColName)) { continue; }

                    if (Geo.Utils.StringUtil.IsDecimalOrNumber(kv.Value))
                    {
                        var val = Geo.Utils.DoubleUtil.TryParse(kv.Value);
                        if (val != null)
                        {
                            var differ = (double)val - substractor;
                            var key = kv.Key;
                            //if( isUpdateKeyWithCol   ) 
                                key+="-" + substractorColName;
                            storage.AddItem(key, differ);
                        }
                    }
                }
                storage.EndRow();
            }
            return storage;
        }

        #endregion

        #region 列的计算

        /// <summary>
        /// 所有行操作指定列。
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="numeralOperationType"></param>
        /// <param name="ifColNullThenNull"></param>
        public void UpdateAllByOperateCol(string colName, NumeralOperationType numeralOperationType, bool ifColNullThenNull =true)
        {
            switch (numeralOperationType)
            {
                case NumeralOperationType.加: UpdateAllByOperateCol(colName, DoubleUtil.Plus, ifColNullThenNull);
                    break;
                case NumeralOperationType.减: UpdateAllByOperateCol(colName,  DoubleUtil.Minus, ifColNullThenNull);
                    break;
                case NumeralOperationType.乘: UpdateAllByOperateCol(colName,  DoubleUtil.Multipy, ifColNullThenNull);
                    break;
                case NumeralOperationType.除: UpdateAllByOperateCol(colName,  DoubleUtil.Div, ifColNullThenNull);
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 减去指定列
        /// </summary>
        /// <param name="paramName"></param>
        public void UpdateAllByMinusCol(string paramName){UpdateAllByOperateCol(paramName, DoubleUtil.Minus);}

        /// <summary>
        /// 减去指定列
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="funcOper"></param>
        /// <param name="ifColNullThenNull">如果有一个为NULL， 则置 NULL </param>
        public void UpdateAllByOperateCol(string paramName, Func<double, double, double> funcOper, bool ifColNullThenNull=true)
        { 
            bool isSubstractorNull = false;
            var indexColName = this.GetIndexColName();
            foreach (var row in this.BufferedValues)
            {  
                double substractor = 0;
                if (row.ContainsKey(paramName)) { substractor = Geo.Utils.DoubleUtil.TryParse(row[paramName].ToString()); isSubstractorNull = false; }
                else { isSubstractorNull = true; }

                foreach (var key in this.ParamNames)
                {
                    if (!row.ContainsKey(key) || key == indexColName) { continue; }
                    var valObj = row[key];
                    if (Geo.Utils.StringUtil.IsDecimalOrNumber(valObj))
                    {
                        var val = Geo.Utils.DoubleUtil.TryParse(valObj);
                        if (val != null)
                        {
                            if (isSubstractorNull && ifColNullThenNull)
                            {
                                row[key] = null;
                                row.Remove(key);
                            }
                            else
                            {
                                row[key] = funcOper((double)val, substractor);
                            }

                        }
                    }
                }
            }
        }

         
        /// <summary>
        /// 数据操作。
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="val"></param>
        /// <param name="numeralOperationType"></param>
        public void UpdateAllBy(double val, NumeralOperationType numeralOperationType)
        {
            switch (numeralOperationType)
            {
                case NumeralOperationType.加: this.UpdateAllBy(val, DoubleUtil.Plus);
                    break;
                case NumeralOperationType.减: this.UpdateAllBy(val, DoubleUtil.Minus);
                    break;
                case NumeralOperationType.乘: this.UpdateAllBy(val, DoubleUtil.Multipy);
                    break;
                case NumeralOperationType.除: this.UpdateAllBy(val, DoubleUtil.Div);
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 操作指定列
        /// </summary>
        /// <param name="val"></param>
        /// <param name="funcOper"></param>
        public void UpdateAllBy(double val,  Func<double, double, double> funcOper)
        {
            foreach (var row in this.BufferedValues)
            {
                foreach (var key in this.ParamNames)
                {
                    if (!row.ContainsKey(key)) { continue; }
                    var valObj = row[key];
                    if (Geo.Utils.StringUtil.IsDecimalOrNumber(valObj))
                    {
                        var baseVal = Geo.Utils.DoubleUtil.TryParse(valObj);
                        if (baseVal != null)
                        {
                            row[key] = funcOper((double)baseVal, val);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 数据操作。
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="val"></param>
        /// <param name="numeralOperationType"></param>
        public void UpdateColumnBy(string colName, double val, NumeralOperationType numeralOperationType)
        {
            switch (numeralOperationType)
            {
                case NumeralOperationType.加: this.UpdateColumnBy(colName, val, DoubleUtil.Plus);
                    break;
                case NumeralOperationType.减: this.UpdateColumnBy(colName, val, DoubleUtil.Minus);
                    break;
                case NumeralOperationType.乘: this.UpdateColumnBy(colName, val, DoubleUtil.Multipy);
                    break;
                case NumeralOperationType.除: this.UpdateColumnBy(colName, val, DoubleUtil.Div);
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 更新参数值通过加上一个数。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="val"></param>
        public void UpdateColumnByPlus(string paramName, double val) { UpdateColumnBy(paramName, val, DoubleUtil.Plus); }


        /// <summary>
        /// 更新参数值通过加上一个数。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="val"></param>
        public void UpdateColumnByMinus(string paramName, double val) { UpdateColumnBy(paramName, val, DoubleUtil.Minus); } 


        /// <summary>
        /// 更新参数值通过加上一个数。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="val"></param>
        public void UpdateColumnByMultiply(string paramName, double val) { UpdateColumnBy(paramName, val, DoubleUtil.Multipy); }


        /// <summary>
        /// 更新参数值通过加上一个数。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="val"></param>
        /// <param name="funcOper"></param>
        public void UpdateColumnBy(string paramName, double val, Func<double, double, double> funcOper)
        {
            foreach (var item in this.BufferedValues)
            {
                if (item.ContainsKey(paramName)) {
                    var obj = Geo.Utils.ObjectUtil.GetNumeral(item[paramName]);
                    item[paramName] = funcOper(obj, val);
                }
            }
        } 
        /// <summary>
        /// 第一个数字的列名称
        /// </summary>
        public String FirstNumberalColName
        {
            get
            {
                var dic =  this.GetFirstValidValue();
                foreach (var item in this.ParamNames)
                {
                    if (dic.ContainsKey(item))
                    {
                        if (Geo.Utils.ObjectUtil.IsNumerial(dic[item]))
                        {
                            return item;
                        }
                    }
                }
                return null;
            }
        }
        #endregion

        #region 列的统计

        /// <summary>
        /// 统计数据频率
        /// </summary>
        /// <param name="fromVal"></param>
        /// <param name="step"></param>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public ObjectTableStorage GetFrequenceTable(double fromVal, double step, List<string> paramNames = null)
        {
            if(paramNames == null) { paramNames = this.ParamNames; }
            
            Dictionary<string, Dictionary<double, int>> data = new Dictionary<string, Dictionary<double, int>>();
            //提取
            var dataValue = GetVectorsAvailable();
            foreach (var kv in dataValue)
            {
                var vector = kv.Value;
                Dictionary<double, int>  frequence =  Geo.Utils.DoubleUtil.GetDataFrequence(vector, fromVal, step);
                data[kv.Key] = frequence; 
            }

            //所有频率提取
            var frequeces = new List<double>();
            foreach (var kv in data)
            {
                foreach (var item in kv.Value)
                {
                    if (!frequeces.Contains(item.Key))
                    {
                        frequeces.Add(item.Key);
                    }
                }
            }

            //排序
            frequeces.Sort();

            ObjectTableStorage result = new ObjectTableStorage("频域分析-" + this.Name);
            foreach (var freq in frequeces)
            {
                result.NewRow();
                result.AddItem("Frequence",  freq);
                foreach (var kv in data)
                {
                    if (kv.Value.ContainsKey(freq))
                    {
                        result.AddItem(kv.Key, kv.Value[freq]);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 整个表格的统计
        /// </summary>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public int GetValidDataCountOfTable(Func<double, bool> Condition = null)
        {
            var dic = GetValidDataCount(Condition);
            int count = 0;
            foreach (var item in dic)
            {
                count += item.Value;
            }
            return count;
        }
        /// <summary> 
        /// 对各列有效数据进行统计。
        /// 默认为统计有效数个数。
        /// </summary>
        /// <param name="Condition">条件方法</param>
        /// <returns></returns>
        public Dictionary<string, int> GetValidDataCount(Func<double, bool> Condition = null)
        {
            if(Condition == null)
            {
                Condition = new Func<double, bool>(val => val < Double.MaxValue);
            }
            var data = new Dictionary<string, int>();
            var indexColName = GetIndexColName();

            foreach (var colName in this.ParamNames)
            {
                if (colName == indexColName) { continue; }
                data[colName] = GetValidDataCount(colName, Condition);
            }
            return data;
        }
        /// <summary>
        /// 列有效数据进行统计。默认为统计有效数个数
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="Condition">条件方法</param>
        /// <returns></returns>
        public int GetValidDataCount(string colName, Func<double, bool> Condition = null)
        {
            if (Condition == null)
            {
                Condition = new Func<double, bool>(val => val < Double.MaxValue);
            }
            int count = 0;
            if (this.ParamNames.Contains(colName))
            {
                foreach (var item in this.BufferedValues)
                {
                    if (item.ContainsKey(colName)
                        && Geo.Utils.ObjectUtil.IsNumerial(item[colName])
                        && Condition(Geo.Utils.ObjectUtil.GetNumeral(item[colName] )) 
                        ){  
                       count++;
                    } 
                }
            }
            return count;
        }
        /// <summary>
        /// 对所有列的数据进行统计。必须指定条件。
        /// </summary>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetCount(Func<double, bool> Condition)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (var col in this.ParamNames)
            {
                if (!dic.ContainsKey(col)) { dic[col] = 0; }
            }
            HandleNumeralCell(new Action<string, int, double>((string col, int rowIndex, double val) =>
            {
                if (Condition(val)) { dic[col]++; }
            }));

            return dic;
        }

        /// <summary>
        /// 统计指定列满足条件的内容。
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public int GetCount(string colName, Func<double, bool> Condition)
        {
            int count = 0;
            HandleNumeralCell(new Action<string, int, double>((string col, int rowIndex, double val) =>
            {
                if (colName == col && Condition(val)) { count++; }
            }));
            return count;
        }
        /// <summary>
        /// 获取连续数量
        /// </summary>
        /// <typeparam name="TIndexType"></typeparam>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public Dictionary<string, List<Segment<TIndexType>>> GetSequentialCountOfAllCol<TIndexType>(Func<double, bool> Condition)
          where TIndexType : IComparable<TIndexType>
        {
            var dic = new Dictionary<string, List<Segment<TIndexType>>>();

            var indexColName = GetIndexColName();
            foreach (var item in this.ParamNames)
            {
                if (item == indexColName) continue;
                dic[item] = GetSequentialCount<TIndexType>(item, Condition);
            }
            return dic;
        }
        /// <summary>
        /// 对于有趋势性的线条，获取第一个接近某值的Cell。通过符号反向判断。
        /// 如果中途断开，则立即返回。
        /// </summary>
        /// <typeparam name="TIndexType"></typeparam>
        /// <param name="colName"></param>
        /// <param name="valu"></param>
        /// <param name="start"></param>
        /// <param name="isUpOrDown">朝上或朝下接近</param>
        /// <returns></returns>
        public TableCell GetFirstSlopeApproxTo<TIndexType>(string colName, double valu, TIndexType start, bool isUpOrDown = true)
        {
            var rowFrom = this.GetRowIndexOfIndexCol(start); 
            TableCell cell = new TableCell(){ ColName = colName};
            for (int i = rowFrom; i < this.RowCount; i++)
            {
                var val = GetNumeral(i, colName);
                if (Geo.Utils.DoubleUtil.IsValid(val))
                { 
                    if (isUpOrDown)//朝上
                    {
                        if( val - valu > 0) 
                            return cell;
                    }
                    else
                    {
                        if (valu - val > 0)
                            return cell;
                    }                     

                    cell.RowNumber = i;
                    cell.Value = val;//记录上一个。
                }
                else
                {
                    return cell;
                }
            }
            return cell;
        }

        /// <summary>
        /// 获取最接近某值的表格
        /// </summary>
        /// <typeparam name="TIndexType"></typeparam>
        /// <param name="colName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public TableCell GetCellApproxTo<TIndexType>(string colName, double valu,  TIndexType start, TIndexType end, bool lagerOrLower = true)
        {
            var rowFrom = this.GetRowIndexOfIndexCol(start);
            var rowTo = this.GetRowIndexOfIndexCol(end);
            if (rowTo == -1) { rowTo = this.RowCount - 1; }
            var minDistance = double.MaxValue;
            TableCell cell = new TableCell();
            for (int i = rowFrom; i <= rowTo; i++)
            {
                var val = GetNumeral(i, colName);
                if (Geo.Utils.DoubleUtil.IsValid(val) )
                {
                    var differ = val - valu;

                    if (lagerOrLower && differ >= 0)
                    {
                        if (differ <= minDistance)
                        {
                            minDistance = Math.Min(differ, minDistance);
                            cell.Value = val;
                            cell.ColName = colName;
                            cell.RowNumber = i;
                        }
                    }
                    else if( !lagerOrLower && differ < 0 )
                    {
                        if (differ <= minDistance)
                        {
                            minDistance = Math.Min(differ, minDistance);
                            cell.Value = val;
                            cell.ColName = colName;
                            cell.RowNumber = i;
                        }
                    }                  
                }
            }
            return cell;
        }
        /// <summary>
        /// 获取列区域内最大的值
        /// </summary>
        /// <typeparam name="TIndexType"></typeparam>
        /// <param name="colName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public double GetMaxValue<TIndexType>(string colName, TIndexType start, TIndexType end)
        {
            var rowFrom = this.GetRowIndexOfIndexCol(start);
            var rowTo = this.GetRowIndexOfIndexCol(end);

            return GetMaxValue(colName, rowFrom, rowTo);
        }
        /// <summary>
        /// 获取列区域内最大的值
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="refVal">参考数据，与其作差后比较</param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <returns></returns>
        public double GetMaxValue(string colName, double refVal = 0, int rowFrom = 0, int rowTo = int.MaxValue)
        {
            if (rowTo == -1 || rowTo >= this.RowCount) { rowTo = this.RowCount - 1; }
            var max = double.MinValue;
            for (int i = rowFrom; i <= rowTo; i++)
            {
                var val = GetNumeral(i, colName);
                if (Geo.Utils.DoubleUtil.IsValid(val))
                {
                    max = Math.Max(val- refVal, max);
                }
            }
            return max;
        }

        /// <summary>
        /// 获取列区域内最小的值
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="refVal">参考数据，与其作差后比较</param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <returns></returns>
        public double GetMinValue(string colName, double refVal = 0, int rowFrom = 0, int rowTo = int.MaxValue)
        {
            if (rowTo == -1 || rowTo >= this.RowCount) { rowTo = this.RowCount - 1; }
            var max = double.MaxValue;
            for (int i = rowFrom; i <= rowTo; i++)
            {
                var val = GetNumeral(i, colName);
                if (Geo.Utils.DoubleUtil.IsValid(val))
                {
                    max = Math.Min(val - refVal, max);
                }
            }
            return max;
        }
        /// <summary>
        /// 获取列区域内最大绝对值的值
        /// </summary>
        /// <param name="colNames"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <returns></returns>
        public Dictionary<string, double> GetMaxAbsValue(List<string> colNames=null, int rowFrom = 0, int rowTo = int.MaxValue)
        {
            if(colNames == null) { colNames = this.ParamNames; }
            //求最大偏差，
            var maxDiffers = new Dictionary<string, double>();
            foreach (var colName in colNames)
            { 
                maxDiffers[colName] = GetMaxAbsValue(colName,0, rowFrom, rowTo);
            }
            return maxDiffers;
        }

        /// <summary>
        /// 获取列区域内最大绝对值的值
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="rowFrom"></param>
        /// <param name="refVal">参考数据，与其作差后比较</param>
        /// <param name="rowTo"></param>
        /// <returns></returns>
        public double GetMaxAbsValue(string colName,double refVal = 0, int rowFrom = 0, int rowTo = int.MaxValue)
        {
            if (rowTo == -1 || rowTo >= this.RowCount) { rowTo = this.RowCount - 1; }
            var max = double.MinValue;
            for (int i = rowFrom; i <= rowTo; i++)
            {
                var val = GetNumeral(i, colName);
                if (Geo.Utils.DoubleUtil.IsValid(val))
                {
                    max = Math.Max(Math.Abs(val - refVal), max);
                }
            }
            return max;
        }

        /// <summary>
        /// 获取区域内最大的值
        /// </summary>
        /// <typeparam name="TIndexType"></typeparam>
        /// <param name="colName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public TableCell GetMaxCell<TIndexType>(string colName, TIndexType start, TIndexType end)
        {
            var rowFrom = this.GetRowIndexOfIndexCol(start);
            var rowTo = this.GetRowIndexOfIndexCol(end);
            if (rowTo == -1) { rowTo = this.RowCount - 1; }
            var max = double.MinValue;
            TableCell cell = new TableCell();
            for (int i = rowFrom; i <= rowTo; i++)
            {
                var val = GetNumeral(i, colName);
                if (Geo.Utils.DoubleUtil.IsValid(val)&&(val >= max ))
                { 
                    max = Math.Max(val, max);
                    cell.Value = max;
                    cell.ColName = colName;
                    cell.RowNumber = i; 
                }
            }
            return cell;
        } 

        /// <summary>
        /// 统计指定列满足条件,且连续的内容。
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public List<Segment<TIndexType>> GetSequentialCount<TIndexType>(string colName, Func<double, bool> Condition)
            where TIndexType : IComparable<TIndexType>
        {
            var indexColName = GetIndexColName();
            var list = new List<Segment<TIndexType>>();

            int lastRowIndex = 0;
            int currentStartIndex = 0;
            var seg = new Segment<TIndexType>();
            list.Add(seg);
            TIndexType indexVal = default(TIndexType);
            TIndexType lastIndexVal = default(TIndexType);
            HandleNumeralCell(new Action<string, int, double>((string col, int rowIndex, double val) =>
            {
                if (colName == col && Condition(val))
                {

                    indexVal = (TIndexType)GetIndexValue(rowIndex);
                    if (seg.Start == null || seg.Start.Equals(default(TIndexType))) { seg.Start = indexVal; currentStartIndex = rowIndex; }

                    if (lastRowIndex != rowIndex - 1 && currentStartIndex != rowIndex)
                    {
                        seg.End = lastIndexVal;
                        seg = new Segment<TIndexType>();
                        list.Add(seg);
                    }

                    lastIndexVal = indexVal;
                    lastRowIndex = rowIndex;
                }
            }));
            seg.End = lastIndexVal;
            return list;
        }

        /// <summary>
        /// 计算所有列的平均数，并且追加到追加到表格最后。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double> GetAverageAndAppendToTable()
        {
            var dic = this.GetAverages();
            this.NewRow();
            this.AddItem(dic);
            foreach (var name in this.CurrentRow.Keys)
            {
                if (name.ToUpper().Contains("NAME"))
                {
                    this.CurrentRow[name] = "Average";
                    break;
                }
            }

            this.EndRow();
            return dic;
        }
        /// <summary>
        /// 计算平均值和均方差，添加到表格尾部，并返回。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double[]> GetAveragesWithStdDevAndAppendToTable()
        {
            Dictionary<string, double[]> result = GetAveragesWithStdDev();
            if (RowCount == 0)
            {
                return result;
            }
            this.AddItem(result);
            var secondLast = this.BufferedValues[RowCount - 3];
            var keys = this.BufferedValues[0].Keys;
            foreach (var name in keys)
            {
                if (name.ToUpper().Contains("NAME"))
                {
                    secondLast[name] = "Average";
                    break;
                }
            }
            var last = this.BufferedValues[RowCount - 2];
            foreach (var name in keys)
            {
                if (name.ToUpper().Contains("NAME"))
                {
                    last[name] = "StdOrRms";
                    break;
                }
            }
            last = this.BufferedValues[RowCount - 1];
            foreach (var name in keys)
            {
                if (name.ToUpper().Contains("NAME"))
                {
                    last[name] = "Count";
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取整个表格有效数均值和中误差。
        /// </summary>
        /// <returns></returns>
        public double [] GetAverageWithRmse()
        {
            var dic = GetAveragesWithStdDev();
            double sumVal = 0;
            double sumVtpv = 0;
            int totalCount = 0;
            //分组平差再合并
            foreach (var item in dic)
            {
                int count =  (int)item.Value[2];
                sumVal +=  item.Value[0];
                sumVtpv += Math.Pow(item.Value[1], 2.0) * (count - 1);//恢复

                totalCount += count;
            }
            var ave = sumVal / totalCount;
            var rmse = Math.Sqrt(sumVtpv / (totalCount - 1));

            return new double[] { ave, rmse, totalCount };
        }


        /// <summary>
        /// 返回平均数和均方差，第二个为均方差，中误差。如果不是数字，则不返回。
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Dictionary<string, double[]> GetAveragesWithStdDev(IEnumerable<string> paramNames = null, int from = 0, int count = int.MaxValue)
        {
            Dictionary<string, double[]> result = new Dictionary<string, double[]>();
            if (paramNames == null) { paramNames = this.ParamNames; }

            foreach (var item in paramNames)
            {
                var val = GetAverageWithStdDev(item, from, count);
                if (Geo.Utils.DoubleUtil.IsValid(val[0]))
                {
                    result[item] = val;
                }
            }
            return result;
        }
        /// <summary>
        /// 返回平均数和均方差
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Dictionary<string, RmsedNumeral> GetAveragesWithRms(IEnumerable<string> paramNames = null, int from = 0, int count = int.MaxValue)
        {
            Dictionary<string, RmsedNumeral> result = new Dictionary<string, RmsedNumeral>();
            if (paramNames == null) { paramNames = this.ParamNames; }

            foreach (var item in paramNames)
            {
                var val = GetAverageWithRms(item, from, count);
                if(RmsedNumeral.IsValid(val))
                {
                    result[item] = val;
                }
            }
            return result;
        }

        /// <summary>
        /// 返回残差中误差字典，认为列值为残差
        /// </summary>
        /// <param name="paramNamesOfResidual"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Dictionary<string, double[]> GetResidualRmse(IEnumerable<string> paramNamesOfResidual = null, int from = 0, int count = int.MaxValue)
        {
            var result = new Dictionary<string, double[]>();
            if (paramNamesOfResidual == null) { paramNamesOfResidual = this.ParamNames; }

            foreach (var item in paramNamesOfResidual)
            {
                var val = GetResidualRmse(item, from, count);
                if (Geo.Utils.DoubleUtil.IsValid(val[0]))
                {
                    result[item] = val;
                }
            }
            return result;
        }
        /// <summary>
        /// 返回残差中误差字典，认为列值为残差
        /// </summary>
        /// <param name="paramNamesOfResidual"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Dictionary<string, RmsedNumeral> GetResidualRms(IEnumerable<string> paramNamesOfResidual = null, int from = 0, int count = int.MaxValue)
        {
            var result = new Dictionary<string, RmsedNumeral>();
            if (paramNamesOfResidual == null) { paramNamesOfResidual = this.ParamNames; }

            foreach (var item in paramNamesOfResidual)
            {
                var val = GetResidualRmse(item, from, count);
                if (Geo.Utils.DoubleUtil.IsValid(val[0]))
                {
                    result[item] = new RmsedNumeral(val[0], val[1]);
                }
            }
            return result;
        }

        /// <summary>
        /// 返回残差中误差字典，认为列值为残差
        /// </summary>
        /// <param name="paramNamesOfResidual"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Dictionary<string, double[]> GetAbsMean(IEnumerable<string> paramNamesOfResidual = null, int from = 0, int count = int.MaxValue)
        {
           var result = new Dictionary<string, double[]>();
            if (paramNamesOfResidual == null) { paramNamesOfResidual = this.ParamNames; }

            foreach (var item in paramNamesOfResidual)
            {
                var val = GetAbsMean(item, from, count);
                if (Geo.Utils.DoubleUtil.IsValid(val[0]))
                {
                    result[item] = val;
                }
            }
            return result;
        }
        /// <summary>
        ///  返回表，绝对值平均数和均方差，第二个为均方差，中误差。如果不是数字，则不返回。
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ObjectTableStorage GetAbsAveragesWithStdDevTable(IEnumerable<string> paramNames = null, int from = 0, int count = int.MaxValue)
        {
            var result = GetAbsAveragesWithStdDev( paramNames ,   from  ,  count);
            return BuildAverageTable(result, "绝对值平均数和均方差"); 
        }

        /// <summary>
        ///  返回表，绝对值平均数和均方差，第二个为均方差，中误差。如果不是数字，则不返回。
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ObjectTableStorage GetAveragesWithStdDevTable(IEnumerable<string> paramNames = null, int from = 0, int count = int.MaxValue)
        {
            var result = GetAveragesWithStdDev(paramNames, from, count);
            return BuildAverageTable(result, "平均数和均方差");
        }
       
      

        /// <summary>
        /// 求各行的平均
        /// </summary> 
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ObjectTableStorage GetRowAveragesWithStdDevTable(int from = 0, int count = int.MaxValue)
        { 
            Dictionary<string, double[]> dic = new Dictionary<string, double[]>();
            var indexName = this.GetIndexColName();
            int i = -1;
            foreach (var row in this.BufferedValues)
            {
                i++;
                if(i < from || i > count) { continue; }

                if (!row.ContainsKey(indexName)) { continue; }

                List<double> vals = new List<double>();
                foreach (var item in row)
                {
                    if(item.Key == indexName) { continue; }

                    if (Geo.Utils.ObjectUtil.IsNumerial(item.Value))
                    {
                        var val = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                        vals.Add(val);
                    }
                }
                var index = row[indexName].ToString();
                var ave = Geo.Utils.DoubleUtil.GetAverageWithStdDev(vals);
                dic[index] = ave; 
            }
            return BuildAverageTable(dic, "各行平均_" + this.Name); 
        }


        /// <summary>
        /// 求各行的平均
        /// </summary> 
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="errorTimes"></param>
        /// <returns></returns>
        public ObjectTableStorage GetRowWeightedAveragesWithStdDevTable(double errorTimes=3, int from = 0, int count = int.MaxValue)
        {
            Dictionary<object, RmsedNumeral> dic = GetRowWeightedAveragesWithStdDev(errorTimes, from, count);
            return BuildAverageTable(dic, "各行平均_" + this.Name);
        }

        public Dictionary<object, RmsedNumeral> GetRowWeightedAveragesWithStdDev(double errorTimes, int from, int count)
        {
            Dictionary<object, RmsedNumeral> dic = new Dictionary<object, RmsedNumeral>();
            var indexName = this.GetIndexColName();
            int i = -1;
            foreach (var row in this.BufferedValues)
            {
                i++;
                if (i < from || i > count) { continue; }

                if (!row.ContainsKey(indexName)) { continue; }

                List<double> vals = new List<double>();
                foreach (var item in row)
                {
                    if (item.Key == indexName) { continue; }

                    if (Geo.Utils.ObjectUtil.IsNumerial(item.Value))
                    {
                        var val = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                        vals.Add(val);
                    }
                }
                var index = row[indexName];
                var ave = Geo.Utils.DoubleUtil.WeightedAverageWithRms(vals, errorTimes);
                dic[index] = ave;
            }

            return dic;
        }
        /// <summary>
        /// 行抗差加权平均后，再进行序贯平差。
        /// </summary>
        /// <param name="errorTimes"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ObjectTableStorage GetRowAveragesAndSquentialAdjustTable(double errorTimes = 3, int from = 0, int count = int.MaxValue)
        {
            Dictionary<object, RmsedNumeral> dic = GetRowWeightedAveragesWithStdDev(errorTimes, from, count);

            var storage = new ObjectTableStorage("一维序贯平差_" + this.Name);

            var keys = new List<object>( dic.Keys);
            keys.Sort();

            Dictionary<object, RmsedNumeral> result = new Dictionary<object, RmsedNumeral>();

            string indexColName = GetIndexColName();
            var WideLaneFilterManager = new CyclicalNumerFilterManager(errorTimes, false);
            NumeralWindowData window = new NumeralWindowData(50);
            var bufferedStream = new BufferedStreamService<Object>(keys);
            bufferedStream.MaterialInputted += new MaterialEventHandler<object>(key =>
            {
                if (key == null) { return; }
                var val = dic[key];
                window.Add(val.Value);
            });
                var filter = WideLaneFilterManager.GetOrCreate("Key");
            foreach (var key in bufferedStream)
            {
                if (key == null) { continue; }
                var val = dic[key];

                filter.SetBuffer(window);
                var resultVal = filter.Filter(val);

                result.Add(key, resultVal);
            }

            return BuildAverageTable(result, "各行平均_" + this.Name);
        }

        private static ObjectTableStorage BuildAverageTable(Dictionary<object, RmsedNumeral> result, string name )
        {
            ObjectTableStorage table = new ObjectTableStorage(name);
            foreach (var item in result)
            {
                table.NewRow();
                table.AddItem("ParamName", item.Key);
                table.AddItem("Value", item.Value.Value);
                table.AddItem("Rms", item.Value.Rms);
            }
            return table;
        }
        private static ObjectTableStorage BuildAverageTable(Dictionary<string, double[]> result, string name )
        {
            ObjectTableStorage table = new ObjectTableStorage(name);
            foreach (var item in result)
            {
                table.NewRow();
                table.AddItem("ParamName", item.Key);
                table.AddItem("Value", item.Value[0]);
                table.AddItem("Rms", item.Value[1]);
            }
            return table;
        }

        /// <summary>
        /// 返回绝对值平均数和均方差，第二个为均方差，中误差。如果不是数字，则不返回。
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Dictionary<string, double[]> GetAbsAveragesWithStdDev(IEnumerable<string> paramNames = null, int from = 0, int count = int.MaxValue)
        {
            Dictionary<string, double[]> result = new Dictionary<string, double[]>();
            if (paramNames == null) { paramNames = this.ParamNames; }

            foreach (var item in paramNames)
            {
                var val = GetAbsAverageWithStdDev(item, from, count);
                if (Geo.Utils.DoubleUtil.IsValid(val[0]))
                {
                    result[item] = val;
                }
            }
            return result;
        }
        /// <summary>
        /// 返回平均数和均方差，第二个为均方差，中误差。如果不是数字，则不返回。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public double[] GetAbsAverageWithStdDev(string paramName, int from = 0, int count = int.MaxValue)
        {
            var list = GetVector(paramName, from, count, true);
            if (list.Count == 0) { return new double[] { double.NaN, double.NaN }; }
            return Geo.Utils.DoubleUtil.GetAbsAverageWithStdDev(list);
        }
        /// <summary>
        /// 返回平均数和均方差，第二个为均方差，中误差。如果不是数字，则不返回。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public double[] GetAverageWithStdDev(string paramName, int from = 0, int count = int.MaxValue)
        {
            var list = GetVector(paramName, from, count, true);
            if (list.Count == 0) { return new double[] { double.NaN, double.NaN }; }
            return Geo.Utils.DoubleUtil.GetAverageWithStdDev(list);
        }
        
        /// <summary>
        /// 返回平均数和均方差
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public RmsedNumeral GetAverageWithRms(string paramName, int from = 0, int count = int.MaxValue)
        {
            var list = GetVector(paramName, from, count, true);
            if (list.Count == 0) { return RmsedNumeral.NaN; }
            var ave = Geo.Utils.DoubleUtil.GetAverageWithRms(list);
            return ave;
        }
        /// <summary>
        /// 返回残差中误差,第二个数字为计算的数量
        /// </summary>
        /// <param name="colNameOfResidual"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public double[]  GetResidualRmse(string colNameOfResidual, int from = 0, int count = int.MaxValue)
        {
            var list = GetVector(colNameOfResidual, from, count, true);
            if (list.Count == 0) { return new double [] { double.NaN, double.NaN }; }
            return Geo.Utils.DoubleUtil.GetResidualRmse(list);
        }
        /// <summary>
        /// 返回绝对值平均值，第二个数字为计算的数量
        /// </summary>
        /// <param name="colNameOfResidual"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public double[] GetAbsMean(string colNameOfResidual, int from = 0, int count = int.MaxValue)
        {
            var list = GetVector(colNameOfResidual, from, count, true);
            if (list.Count == 0) { return new double [] { double.NaN, double.NaN }; }
            return Geo.Utils.DoubleUtil.GetAbsMean(list);
        }
        /// <summary>
        /// 获取平均数。如果不是数字，返回 NaN。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public double GetAverage(string paramName, int from = 0, int count = int.MaxValue)
        {
            var list = GetVector(paramName, from, count, true);
            if (list.Count == 0) { return double.NaN; }
            return Geo.Utils.DoubleUtil.Average(list);
        } 

        /// <summary>
        /// 获取平均值。如果不是数字，则不返回。
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Dictionary<string, double> GetAverages(IEnumerable<string> paramNames = null, int from = 0, int count = int.MaxValue)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            if (paramNames == null) { paramNames = this.ParamNames; }

            foreach (var item in paramNames)
            {
                var val = GetAverage(item, from, count);
                if (Geo.Utils.DoubleUtil.IsValid(val))
                {
                    result[item] = val;
                }
            }
            return result;
        }

        /// <summary>
        /// 查找所有列的最后值，并返回其平均值。不一定是同一行。
        /// </summary>
        /// <param name="ignoreRowCountLessThan">忽略行数量少于此的列,不含</param>
        /// <returns></returns>
        public double GetAverageOfLastValueOfAllCols(int ignoreRowCountLessThan = 10, double period = 1.0, double reference = 0.5)
        {
            var dic = GetLastValueOfAllCols(ignoreRowCountLessThan);
            var PeriodFilterManager = new PeriodPipeFilter(period, reference);
            List<double> list = new List<double>();
            foreach (var item in dic.Values)
            {
                var val = PeriodFilterManager.Filter(item);
                list.Add(val);
            }

            return Geo.Utils.DoubleUtil.Average(list);
        }

        /// <summary>
        /// 查找所有列的最后值，并返回其平均值。不一定是同一行。
        /// </summary>
        /// <param name="ignoreRowCountLessThan">忽略行数量少于此的列,不含</param>
        /// <returns></returns>
        public double[] GetAverageOfLastValueWithStdDevOfAllCols(int ignoreRowCountLessThan = 10, double period = 1.0, double reference = 0.5)
        {
            var dic = GetLastValueOfAllCols(ignoreRowCountLessThan);
            var PeriodFilterManager = new PeriodPipeFilter(period, reference);
            List<double> list = new List<double>();
            foreach (var item in dic.Values)
            {
                //var val = PeriodFilterManager.Filter(item);
                //list.Add(val);
                list.Add(item);
            }

            return Geo.Utils.DoubleUtil.GetAverageWithStdDev(list);
        }


        /// <summary>
        /// 查找所有列的最后值，并返回其平均值。不一定是同一行。
        /// </summary>
        /// <param name="ignoreRowCountLessThan">忽略行数量少于此的列,不含</param>
        /// <returns></returns>
        public Dictionary<string, double> GetLastValueOfAllCols(int ignoreRowCountLessThan = 1)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            var indexColName = GetIndexColName();
            List<double> vals = new List<double>();
            foreach (var name in this.ParamNames)
            {
                if (name == indexColName) { continue; }
                if (GetValidRowCount(name) < ignoreRowCountLessThan) { continue; }

                var obj = GetLastValue(name);

                var val = Geo.Utils.ObjectUtil.GetNumeral(obj);
                if (!Double.IsNaN(val))
                {
                    dic[name] = val;
                }
            }
            return dic;
        }

        /// <summary>
        /// 统计指定列有效数据（行）个数。
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public int GetValidRowCount(string colName)
        {
            return GetVector(colName, 0, this.RowCount, true).Count;
        }

        /// <summary>
        /// 获取指定列最后不为空的值。
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public Object GetLastValue(string colName)
        {
            int rowCount = this.RowCount;
            for (int i = rowCount - 1; i >= 0; i--)
            {
                var row = this.BufferedValues[i];
                if (row.ContainsKey(colName) && row[colName] != null) { return row[colName]; }
            }
            return null;
        }

        /// <summary>
        /// 获取某行的平均值。如果该列无数值，则不参与计算。
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public double GetAverageOfRow(int rowIndex)
        {
            var row = this.BufferedValues[rowIndex];

            Dictionary<string, double> result = new Dictionary<string, double>();
            var indexColName = GetIndexColName();
            List<double> vals = new List<double>();
            foreach (var item in row)
            {
                if (item.Key == indexColName) { continue; }
                if (Utils.ObjectUtil.IsNumerial(item.Value))
                {
                    var val = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                    if (!Double.IsNaN(val))
                    {
                        vals.Add(val);
                    }
                }
            }

            return Geo.Utils.DoubleUtil.Average(vals);
        }

        #endregion

        #region 提取向量
        /// <summary>
        /// 返回数字列，字典。键为索引。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName"></param>
        /// <param name="from">从第一行开始计算（含无效数字）</param>
        /// <param name="count">总行数，含无效数</param>
        /// <param name="defaultValue">默认数值，如果无效数值，则解析失败后以其填充</param>
        /// <param name="isSkipNonNumerial">是否忽略非浮点数数据</param>
        /// <returns></returns>
        public Dictionary<T, double> GetNumeralColDic<T>(string paramName, int from = 0, int count = int.MaxValue, bool isSkipNonNumerial = false, double defaultValue = Double.NaN)
        {
            if (!this.ParamNames.Contains(paramName)) { return new Dictionary<T, double>(); }
            var Vector = new Dictionary<T, double>(count);
            var indexName = this.GetIndexColName();
            //表内容 
            int i = -1;
            int endIndex = from + count;
            foreach (var values in BufferedValues.ToArray())
            {
                i++;
                if (i < from) { continue; }
                if (i >= endIndex) { break; }

                if (values.ContainsKey(paramName))
                {
                    var obj = values[paramName];
                    if (Geo.Utils.ObjectUtil.IsNumerial(obj))
                    {
                        var valu = Geo.Utils.ObjectUtil.GetNumeral(obj);
                        if (Geo.Utils.DoubleUtil.IsValid(valu))
                        {
                            var key = (T)values[indexName];
                            Vector.Add(key, valu);
                        }
                    }
                    else if (obj is RmsedNumeral)
                    {
                        var key = (T)values[indexName];
                        var val = (RmsedNumeral)obj;
                        Vector.Add(key, val.Value);
                    }
                    else if (!isSkipNonNumerial)
                    {
                        var key = (T)values[indexName];
                        Vector.Add(key, defaultValue);
                    }
                }
            }
            return Vector;
        }
        /// <summary>
        /// 返回数字列，字典。键为索引。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramNames"></param>
        /// <param name="from">从第一行开始计算（含无效数字）</param>
        /// <param name="count">总行数，含无效数</param>
        /// <param name="defaultValue">默认数值，如果无效数值，则解析失败后以其填充</param>
        /// <param name="isSkipNonNumerial">是否忽略非浮点数数据</param>
        /// <returns></returns>
        public Dictionary<T, Dictionary<string, double>> GetNumeralColDic<T>( IEnumerable<string> paramNames, int from = 0, int count = int.MaxValue, bool isSkipNonNumerial = false, double defaultValue = Double.NaN)
        {
            Dictionary<T, Dictionary<string, double>> result = new Dictionary<T, Dictionary<string, double>>(count);  
            var indexName = this.GetIndexColName();
            //表内容 
            int i = -1;
            int endIndex = from + count;
            foreach (var values in BufferedValues)
            {
                i++;
                if (i < from) { continue; }
                if (i >= endIndex) { break; }
                foreach (var paramName in paramNames)
                {
                    Dictionary<string, double> dic = new Dictionary<string, double>();
                    if (values.ContainsKey(paramName))
                    {
                        var obj = values[paramName];
                        if (Geo.Utils.ObjectUtil.IsNumerial(obj))
                        {
                            var valu = Geo.Utils.ObjectUtil.GetNumeral(obj);
                            if (Geo.Utils.DoubleUtil.IsValid(valu))
                            {
                                dic.Add(paramName, valu);
                            }
                        }
                        else if (obj is RmsedNumeral)
                        { 
                            var val = (RmsedNumeral)obj;
                            dic.Add(paramName, val.Value);
                        }
                        else if (!isSkipNonNumerial)
                        { 
                            dic.Add(paramName, defaultValue);
                        }
                    }
                    if(dic.Count > 0)
                    {
                        var key = (T)values[indexName];
                        result[key] = dic;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 返回列数据，变量数组.如果无法解析，则返回为空数组。
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="from">起始编号</param>
        /// <param name="count">截取数量</param>
        /// <param name="defaultValue">默认数值，如果无效数值，则解析失败后以其填充</param>
        /// <param name="isSkipNonNumerial">是否忽略非浮点数数据</param>
        /// <returns></returns>
        public List<double> GetVector(string paramName, int from = 0, int count = int.MaxValue, bool isSkipNonNumerial = false, double defaultValue = Double.NaN)
        {
            if (!this.ParamNames.Contains(paramName)) { return new List<double>(); }
            List<double> Vector = new List<double>(this.BufferedValues.Count);

            //表内容 
            int i = -1;
            int endIndex = from + count;
            foreach (var values in BufferedValues.ToArray())
            {
                i++;
                if (i < from) { continue; }
                if (i >= endIndex) { break; }

                if (values.ContainsKey(paramName))
                {
                    var obj = values[paramName];
                    if (Geo.Utils.ObjectUtil.IsNumerial(obj))
                    {
                        var valu = Geo.Utils.ObjectUtil.GetNumeral(obj);
                        if (Geo.Utils.DoubleUtil.IsValid(valu))
                        {
                            Vector.Add(valu);
                        }
                    }else if(obj is RmsedNumeral)
                    {
                        var val = (RmsedNumeral)obj;
                        Vector.Add(val.Value);
                    }
                    else if (!isSkipNonNumerial)
                    {
                        Vector.Add(defaultValue);
                    }
                }
                else if (!isSkipNonNumerial)
                {
                    Vector.Add(defaultValue);
                }
            }
            return Vector;
        }
        /// <summary>
        /// 遍历移除每一列非最大值
        /// </summary>
        /// <returns></returns>
        public int RemoveMinorityValueOfEachCol()
        {
            var indexCol = GetIndexColName();
            int count = 0;
            foreach (var item in this.ParamNames)
            {
                if (item == indexCol) { continue; }
               count = count + RemoveMinorityValueOfCol(item);
            }
            return count;
        }
        /// <summary>
        /// 只保留最大的数值，其它移除，返回移除数量
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public int RemoveMinorityValueOfCol(string colName)
        {
            var valueCount = GetNumeralValueCount(colName);
            var maxCountValue = valueCount.OrderByDescending(p => p.Value).First().Key;
            var count = 0;
            HandleNumeralRow(colName,(int i, object index, Dictionary<string, object> row, double val)=>
            {
                if (val != maxCountValue) { row.Remove(colName); count++; }
            });
            return count;
        }

        public int GetNumeralValueCount()
        {
            int count = 0;
            var index = this.GetIndexColName();
            foreach (var item in this.BufferedValues)
            {
                foreach (var cell in item)
                {
                    if (cell.Key == index) { continue; }

                    var val = (Geo.Utils.ObjectUtil.GetNumeral(cell.Value));
                    if (Geo.Utils.DoubleUtil.IsValid(val))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// 计算指定列数值出现的次数。 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public Dictionary<double, int> GetNumeralValueCount(string colName)
        {
            var dic = new Dictionary<double, int>();
            foreach (var item in this.BufferedValues)
            {
                if (item.ContainsKey(colName))
                { 
                    var val = (Geo.Utils.ObjectUtil.GetNumeral(item[colName]));
                    if(Geo.Utils.DoubleUtil.IsValid(val)){
                        if(!dic.ContainsKey(val)){ dic[val] = 0;}
                        dic[val]++;
                    }
                }
            }

            return dic;
        }

        
        /// <summary>
        /// 是否没有值，或值为空null或空白。。
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        static public bool IsEmptyObject(Dictionary<String, Object> dic, string colName)
        {
            if (!dic.ContainsKey(colName)) { return true; }
            var val = dic[colName];

            if (val == null) { return true; }
            if (String.IsNullOrWhiteSpace(val.ToString())) { return true; }
            return false;
        }
        /// <summary>
        /// 返回所有的可用数据(忽略不可用数据)，编号可能已经混乱。
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param> 
        /// <returns></returns>
        public Dictionary<string, List<double>> GetVectorsAvailable(int from = 0, int count = int.MaxValue)
        {
            return GetVectors(this.ParamNames, from, count, true);
        }

        /// <summary>
        /// 返回所有的数据
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="isSkipNonNumerial"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Dictionary<string, List<double>> GetVectors(int from = 0, int count = int.MaxValue, bool isSkipNonNumerial = false, double defaultValue = Double.NaN)
        {
            return GetVectors(this.ParamNames, from, count, isSkipNonNumerial,  defaultValue);
        }

        /// <summary>
        /// 返回表格数据,如果获取失败，则忽略。
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Dictionary<string, List<double>> GetVectors(IEnumerable<string> paramNames, int from = 0, int count = int.MaxValue, bool isSkipEmpty = false, double defaultValue = Double.NaN)
        {
            Dictionary<string, List<double>> dic = new Dictionary<string, List<double>>();
            var indexName = GetIndexColName();
            foreach (var item in paramNames)
            {
                if (indexName == item) { continue; }
                var vector = GetVector(item, from, count, isSkipEmpty, defaultValue);
                if (vector.Count > 0)
                {
                    dic[item] = vector;
                }
            }
            return dic;
        }
        #endregion
        #endregion

        #region 转换输出，外部显示

        /// <summary>
        /// 文本显示
        /// </summary>
        /// <returns></returns>
        public string ToTextTable()
        {
            StringBuilder writer = new StringBuilder();
            writer.AppendLine(ToSplitedTitleString());
            foreach (var item in BufferedValues)
            {
                writer.AppendLine(ToSplitedValueString(item));
            }
            return writer.ToString();
        }

        /// <summary>
        /// 行
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string ToLines(List<Vector> values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in values)
            {
                var line = ToSplitedValueString(item);
                sb.AppendLine(line);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public String ToSplitedValueString(Vector values)
        {
            Dictionary<string, Object> vals = new Dictionary<string, Object>();
            var names = values.ParamNames;
            for (int i = 0; i < values.Count; i++)
            {
                var name = names[i];
                vals[name] = values[i];
            }

            return ToSplitedValueString(vals);
        }
        /// <summary>
        /// 制表位分隔的题目
        /// </summary>
        /// <returns></returns>
        public String ToSplitedTitleString(string Spliter = "\t", bool isTotal = true)
        {
            var paramNames = GetTitleNames(isTotal);
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in paramNames)
            {
                if (i != 0) { sb.Append(Spliter); }
                sb.Append(item);
                i++;
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取文本表格
        /// </summary>
        /// <returns></returns>
        public string GetTextTable(string spliter = "\t", string defaultStr = " ", string NumeralValFormat = "0.00000")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ToSplitedTitleString(spliter));
            foreach (var item in BufferedValues.ToArray()) { sb.AppendLine(ToSplitedValueString(item, defaultStr, spliter, NumeralValFormat)); }
            return sb.ToString();
        }

        /// <summary>
        /// 返回数据表格，用于显示。
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="isDefaultEmpty">默认是否是空</param>
        /// <param name="isTotal">是否所有</param>
        /// <returns></returns>
        public DataTable GetDataTable(string tableName = "表格", bool isDefaultEmpty = false, bool isTotal = true)
        {
            DataTable table = new DataTable(tableName);
            //表名称
            var typedNames = GetNamedTypes();
            var paramNames = this.ParamNames;
            foreach (var item in typedNames) { table.Columns.Add(item.Name, item.Type); }
            //表内容
            foreach (var values in BufferedValues.ToArray())
            {
                int i = 0;
                object[] vals = new object[paramNames.Count];

                foreach (var name in typedNames)
                {
                    if (values.ContainsKey(name.Name)) { vals[i] = values[name.Name]; }
                    else if (isDefaultEmpty) { vals[i] = null; }
                    else { vals[i] = ObjectUtil.Default(name.Type); }
                    i++;
                }

               var row = table.Rows.Add(vals);
            }
            return table;
        }
        #endregion

         
        /// <summary>
        ///  输出为行
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public String ToSplitedValueString(Dictionary<string, double> values)
        {
            var vals = new Dictionary<string, Object>();
            foreach (var item in values)
            {
                vals[item.Key.ToString()] = item.Value;
            }

            return ToSplitedValueString(vals);
        }

        /// <summary>
        /// 制表符数值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="defaultValue"></param>
        /// <param name="Spliter"></param>
        /// <param name="NumeralFormat"></param>
        /// <param name="NumeralValFormat"></param>
        /// <param name="isTotal"></param>
        /// <returns></returns>
        public String ToSplitedValueString(Dictionary<string, Object> values, String defaultValue = " ", string Spliter = "\t", string NumeralFormat = "0.00000", string NumeralValFormat = "0.00000", bool isTotal = true)
        {
            Regist(values.Keys);

            var paramNames = GetTitleNames(isTotal);

            return ObjectTableWriter.ToSplitedValueString(values, defaultValue, Spliter, NumeralFormat, paramNames);
        }

        /// <summary>
        /// 取得所有标题。
        /// </summary>
        /// <param name="isTotal"></param>
        /// <returns></returns>
        public List<string> GetTitleNames(bool isTotal = true)
        {
            var paramNames = isTotal ? NameListManager.TotalNames : ParamNames;
            return paramNames;
        }


        #region 移除或清空数据

        /// <summary>
        /// 清空指定列开始的数据。
        /// </summary>
        /// <param name="count"></param>
        public void EmptyFrontValueOfCols(int count)
        {
            var indexColName = GetIndexColName();
            foreach (var colName in this.ParamNames)
            {
                if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                EmptyFrontValueOfCol(colName, count);
            }

        }
        /// <summary>
        /// 清空指定列开始的数据。
        /// </summary>
        /// <param name="count"></param>
        public void EmptyFrontValueOfCol(string colName, int count)
        {
            int i = -1;
            foreach (var row in this.BufferedValues)
            {
                if (row.ContainsKey(colName))
                {
                    i++;
                    if (i > count) { break; }
                    row[colName] = null;
                }

            }
        }
        /// <summary>
        /// 移除列名称中的字符串
        /// </summary>
        /// <param name="tobeRemoved"></param>
        /// <returns></returns>
        public ObjectTableStorage RemoveColString(string[] tobeRemoved)
        {
            var name = Geo.Utils.StringUtil.RemoveString(this.Name, tobeRemoved);
            ObjectTableStorage table = new ObjectTableStorage(name);

            foreach (var row in this.BufferedValues)
            {
                table.NewRow();
                foreach (var item in row)
                {
                    var key = Geo.Utils.StringUtil.RemoveString(item.Key, tobeRemoved);
                    table.AddItem(key, item.Value);
                }
                table.EndRow();
            }
            return table;
        }

        /// <summary>
        /// 移除列名称包含的字。
        /// </summary>
        /// <param name="oldStr"></param>
        public ObjectTableStorage GetReplacedColNameTable(string oldStr, string newStr)
        {
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var row in this.BufferedValues)
            {
                table.NewRow();
                foreach (var item in row)
                {
                    var key = item.Key.Replace(oldStr, newStr);
                    table.AddItem(key, item.Value);
                }
                table.EndRow();
            }
            return table;
        }

        /// <summary>
        /// 移除指定列
        /// </summary>
        /// <param name="colNames"></param>
        public void RemoveCols(IEnumerable< string> colNames)
        {
            foreach (var name in colNames)
            {
                NameListManager.Remove(name);
            }
            foreach (var item in this.BufferedValues)
            {
                foreach (var name in colNames)
                {
                    item.Remove(name);
                }
            }
        }
        /// <summary>
        /// 移除指定列
        /// </summary>
        /// <param name="colName"></param>
        public void RemoveCol(string colName)
        {
            NameListManager.Remove(colName);

            foreach (var item in this.BufferedValues)
            {
                item.Remove(colName);
            }
        }
        /// <summary>
        /// 移除指定列为空的行。
        /// </summary>
        /// <param name="colName"></param>
        public void RemoveEmptyRowsOf(string colName)
        {
            if (!this.ParamNames.Contains(colName))
            {
                this.Clear();
                return;
            }
            List<int> tobeRemoveIndexes = new List<int>();
            int i = -1;
            foreach (var item in this.BufferedValues)
            {
                i++;
                if (!item.ContainsKey(colName))
                {
                    tobeRemoveIndexes.Add(i);
                    continue;
                }
                var val = item[colName];
                if (string.IsNullOrWhiteSpace(val + "") || val.Equals(0.0))
                {
                    tobeRemoveIndexes.Add(i);
                }
            }
            RemoveRows(tobeRemoveIndexes);
        }
        /// <summary>
        /// 移除为空的行。
        /// </summary>
        public void RemoveEmptyRows()
        {
            List<int> tobeRemoveIndexes = new List<int>();
            var indexColName = GetIndexColName();
            int i = -1;
            foreach (var item in this.BufferedValues)
            {
                i++;
                bool isEmptyRow = true;
                foreach (var cell in item)
                {
                    if (cell.Key == indexColName)
                    {
                        continue;
                    }
                    if (!Geo.Utils.ObjectUtil.IsEmpty(cell.Value))
                    {
                        isEmptyRow = false;
                    }
                }
                if (isEmptyRow)
                {
                    tobeRemoveIndexes.Add(i);
                }
            }
            RemoveRows(tobeRemoveIndexes);
        }
        /// <summary>
        /// 移除注册数据少于指定百分比的行，含检索列
        /// </summary>
        /// <param name="percentageOrCount"></param>
        /// <param name="isPercentage"></param>
        public void RemoveRowsWithRegistDataLessThan(double percentageOrCount, bool isPercentage = true)
        {
            List<int> tobeRemoveIndexes = new List<int>();
            var indexColName = GetIndexColName();
            double colCount = this.ColCount;
            if (!isPercentage)
            {
                percentageOrCount /= colCount;
            }
            int i = -1;
            foreach (var item in this.BufferedValues)
            {
                i++;
                var validDataCount = Geo.Utils.DictionaryUtil.GetValidDataCount(item);
                var percent = 1.0 * validDataCount / colCount;
                if (percent < percentageOrCount)
                {
                    tobeRemoveIndexes.Add(i);
                }
            }
            RemoveRows(tobeRemoveIndexes);
        }
        /// <summary>
        ///  移除行，当指定列满足要求
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="val"></param>
        /// <param name="numeralCompareOperator"></param>
        public void RemoveRowsWithFilter(string colName, double val, NumeralCompareOperator numeralCompareOperator)
        {
            NumeralFilter numeralFilter = new Geo.NumeralFilter(val, numeralCompareOperator);
            RemoveRowsWithFilter(colName, numeralFilter.Filter);// new Func<double, bool>(numeralFilter.IsSatisfied));
        }

        /// <summary>
        /// 移除行，当对应指定列满足要求
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="funcFilter">过滤返回false的</param> 
        public void RemoveRowsWithFilter(string colName, Func<double, bool> funcFilter)
        {
            List<int> tobeRemoveIndexes = new List<int>();
            var indexColName = GetIndexColName();
            double colCount = this.ColCount; 
            int i = -1;
            foreach (var item in this.BufferedValues)
            {
                i++;
                if (!item.ContainsKey(colName)) continue;

                var obj = (item[colName]);
                if (Geo.Utils.ObjectUtil.IsNumerial(obj))
                {
                    var val = Geo.Utils.ObjectUtil.GetNumeral(obj);
                    if (funcFilter(val ))
                    {
                        tobeRemoveIndexes.Add(i);
                    }
                } 
            }
            RemoveRows(tobeRemoveIndexes);
        }


        /// <summary>
        ///  移除行，当列某满足要求
        /// </summary>
        /// <param name="val"></param>
        /// <param name="numeralCompareOperator"></param>
        public void RemoveRowsWithFilter(double val, NumeralCompareOperator numeralCompareOperator)
        {
            NumeralFilter numeralFilter = new Geo.NumeralFilter(val, numeralCompareOperator);
            RemoveRowsWithFilter(numeralFilter.Filter);// new Func<double, bool>(numeralFilter.IsSatisfied));
        }

        /// <summary>
        /// 移除行，当对应某列满足要求
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="funcFilter">过滤返回false的</param> 
        public void RemoveRowsWithFilter(Func<double, bool> funcFilter)
        {
            List<int> tobeRemoveIndexes = new List<int>();
            var indexColName = GetIndexColName();
            double colCount = this.ColCount;
            int i = -1;
            foreach (var dic in this.BufferedValues)
            {
                i++;
                foreach (var item in dic)
                {
                    var obj = item.Value;
                    if (Geo.Utils.ObjectUtil.IsNumerial(obj))
                    {
                        var val = Geo.Utils.ObjectUtil.GetNumeral(obj);
                        if (funcFilter(val))
                        {
                            tobeRemoveIndexes.Add(i);
                            break;
                        }
                    }
                }
            }
            RemoveRows(tobeRemoveIndexes);
        }

        /// <summary>
        /// 移除各列内，字段开始的行。
        /// </summary>
        /// <param name="removeCount"></param>
        /// <param name="maxBreakCountInSegment">段内允许的最大的断裂次数，否则认为是两段</param>
        public void RemoveStartRowOfEachSegment(int removeCount, int maxBreakCountInSegment)
        { 
            var indexColName = GetIndexColName(); 
            Dictionary<string, int> firstAppearIds = new Dictionary<string, int>();
            Dictionary<string, int> lastApearedIds = new Dictionary<string, int>();
            int i = -1;
            foreach (var dic in this.BufferedValues)
            {
                i++;
                List<string> toBeRemoves = new List<string>();
                foreach (var item in dic)
                {
                    if(indexColName == item.Key) { continue; }
                    //初始化
                    if (!firstAppearIds.ContainsKey(item.Key))
                    {
                        firstAppearIds[item.Key] = i; 
                    }
                    if (!lastApearedIds.ContainsKey(item.Key))
                    { 
                        lastApearedIds[item.Key] = i;
                    }

                    //判断是否断裂
                    var lastIndex = lastApearedIds[item.Key];
                    if(i - lastIndex > maxBreakCountInSegment)
                    {
                        firstAppearIds[item.Key] = i;
                    }

                    //判断是否移除
                     if ( i - firstAppearIds[item.Key] < removeCount)
                    {
                        toBeRemoves.Add(item.Key);
                    }
                      
                    //update
                    lastApearedIds[item.Key] = i;
                }

                //秋后算账
                foreach (var item in toBeRemoves)
                {
                    dic.Remove(item);
                }
            } 
        }


        /// <summary>
        /// 移除所有空行
        /// </summary>
        public void RemoveAllEmptyRows()
        { 
            var indexName = this.GetIndexColName();


            List<int> indexes = new List<int>();
            int i = -1;
            foreach (var row in BufferedValues)
            {
                i++;
                if (row.Count <= 1) {
                    indexes.Add(i);
                }
            }
            RemoveRows(indexes);
        }

        /// <summary>
        /// 移除所有出勤率太少的列
        /// </summary>
        /// <param name="minRatio"></param>
        public void RemoveAllEmptyCols(double minRatio = 0.9)
        { 
            var indexName = this.GetIndexColName();
            Dictionary<string, double> colCounts = new Dictionary<string, double>();
            foreach (var item in this.ParamNames) //初始化
            {
                colCounts[item] = 0;
            }
            int i = -1;
            foreach (var row in BufferedValues) //统计
            {
                i++;
                foreach (var item in row.Keys)
                {
                    if (!colCounts.ContainsKey(item))
                    {
                        colCounts[item] = 0;
                    }
                    colCounts[item]++;
                }
            }
            //判断
            List<string> colNames = new List<string>();
            foreach (var item in colCounts.Keys)
            {
               var ratio = colCounts[item] / RowCount;
                if(ratio < minRatio)
                {
                    colNames.Add(item);
                }
            }

            RemoveCols(colNames);
        }

        /// <summary>
        /// 移除指定行
        /// </summary>
        /// <param name="tobeRemoveIndexes"></param>
        public void RemoveRows(List<int> tobeRemoveIndexes)
        {
            //从后往前移除
            tobeRemoveIndexes.Sort();
            tobeRemoveIndexes.Reverse();
            foreach (var index in tobeRemoveIndexes)
            {
                if (this.BufferedValues.Count - 1 < index)
                {
                    continue;
                }
                BufferedValues.RemoveAt(index);
            }
        }
        /// <summary>
        /// 移除指定列的行内容
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        public void RemoveRowsOfCol(string colName, int from, int count)
        {
            if (from >= this.RowCount) { log.Warn(this.Name + " 没有足够的行来移除"); return; }

            var endIndex = from + count;
            if (endIndex > this.RowCount)
            {
                count = this.RowCount - from;
            }

            if (from < 0 || count <= 0) return;
            int i = 0;
            foreach (var item in BufferedValues)
            {
                if(i>=from && i<= endIndex)
                {
                    if (item.ContainsKey(colName)) { item.Remove(colName); }
                }
                if(i> endIndex) { break; }
                i++;
            } 
        }


        /// <summary>
        /// 移除指定行
        /// </summary>
        /// <param name="tobeRemoveIndexes"></param>
        public void RemoveRows(List<object> tobeRemoveIndexes)
        {
            foreach (var index in tobeRemoveIndexes)
            {
                var row = GetRow(index);
                this.BufferedValues.Remove(row); 
            }
        }

        /// <summary>
        /// 移除指定行
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        public void RemoveRows(int from, int count)
        {
            var endIndex = from + count;
            if (from > this.RowCount) { log.Warn(this.Name + " 没有足够的行来移除"); return; }
            if (endIndex > this.RowCount)
            {
                count = this.RowCount - from;
            }

            if (from < 0 || count <= 0) return;

            BufferedValues.RemoveRange(from, count);
        }
        /// <summary>
        /// 移除空列
        /// </summary>
        public void RemoveEmptyCols()
        {
            List<string> emptyNames = new List<string>();
            foreach (var name in this.ParamNames)
            {
                bool isEmptyCol = IsEmptyCol(name);

                if (isEmptyCol)
                {
                    emptyNames.Add(name);
                }
            }
            RemoveCols(emptyNames.ToArray());
        }
        /// <summary>
        /// 是否为空列
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsEmptyCol(string name)
        {
            bool isEmptyCol = true;
            foreach (var valdic in this.BufferedValues)
            {
                if (valdic.ContainsKey(name))
                {
                    if (!IsEmptyObject(valdic[name]))
                    {
                        isEmptyCol = false;
                        break;
                    }
                }
            }
            return isEmptyCol;
        }
        /// <summary>
        /// 表是否没有数据
        /// </summary>
        public bool IsEmpty { get { return this.RowCount == 0 || this.ColCount == 0; } }

        /// <summary>
        /// 数据值是否为空。默认浮点数0位空。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsEmptyObject(object obj)
        {
            if (String.IsNullOrWhiteSpace(obj + "") || obj.Equals(0.0))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 采用最简单的方法移除周跳。默认为数据应该变化很小，不超过一周。
        /// 可以有效移除陡峭的跳变。
        /// </summary>
        public void RemoveCycleSlipe(double lambda)
        {
            var indexName = this.GetIndexColName();
            foreach (var colName in this.ParamNames)
            {
                Dictionary<string, SimpleCycleSlipRemover> lastVal = new Dictionary<string, SimpleCycleSlipRemover>();
                int index = -1;
                foreach (var row in this.BufferedValues)
                {
                    index++;
                    foreach (var name in this.ParamNames)
                    {
                        var key = name;
                        if (key == indexName) { continue; }

                        var valObj = row[key];
                        //不处理非可计算数据
                        if (!Geo.Utils.ObjectUtil.IsNumerial(valObj)) { continue; }
                        var val = (double)valObj;

                        //初始化，判断
                        if (!lastVal.ContainsKey(key))
                        {
                            lastVal[key] = new SimpleCycleSlipRemover(lambda);
                        }

                        row[key] = lastVal[key].GetVal(val);
                    }
                }
            }
        }
        #endregion

        #region 通用接口方法
        /// <summary>
        /// 清空重新来过
        /// </summary>
        public void Clear() { this.ParamNames.Clear(); BufferedValues.Clear(); }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            NameListManager.Dispose();
            this.BufferedValues.Clear();
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name + " , Row: " + RowCount + " × Col:" + ColCount;
        }

        #endregion

        #region  遍历并计算单元数据后，返回新表
        /// <summary>
        /// 获取检索列表
        /// </summary>
        /// <returns></returns>
        public List<Object> GetIndexValues(bool ifToDateTime = false)
        {
            string indexColName = GetIndexColName();
            List<Object> list = new List<object>();
            if(indexColName == null) { return list; }

            for (int i =0; i< RowCount; i++)
            {
                var row = this.BufferedValues[i];
                if (!row.ContainsKey(indexColName)) { continue; }

                var index = row[indexColName];
                 list.Add(index);                
            }
            return list;
        }
        /// <summary>
        ///返回索引名称字符串列表
        /// </summary>
        /// <returns></returns>
        public List<String> GetIndexStrings()
        {
            string indexColName = GetIndexColName();
            List<String> list = new List<String>();
            foreach (var row in this.BufferedValues)
            {
                list.Add(row[indexColName] + "");
            }
            return list;
        }
        /// <summary>
        /// 获取检索列表
        /// </summary>
        /// <returns></returns>
        public List<T> GetIndexValues<T>()
        {
            return GetColValues<T>( GetIndexColName()); 
        }
         
        /// <summary>
        /// 获取列值，所无，则以默认值替代
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public List<T> GetColValues<T>(string colName, T defaultValue = default(T))
        {
            List<T> list = new List<T>(); 
            foreach (var row in this.BufferedValues)
            {
                if (row.Count == 0) { continue; }
                if (row.ContainsKey(colName))
                {
                    var obj = (row[colName]);
                    list.Add((T)(obj));
                }
                else
                {
                    list.Add(defaultValue);
                }
            }
            return list;
        }
        /// <summary>
        /// 对所有列有效数据进行统计。
        /// </summary>
        /// <param name="Condition">条件</param>
        /// <returns></returns>
        public Dictionary<TIndexType, int> GetEachRowCount<TIndexType>(Func<double, bool> Condition)
        {
            string indexColName = GetIndexColName();
            var data = new Dictionary<TIndexType, int>();

            foreach (var row in this.BufferedValues)
            {
                var index = (TIndexType)row[indexColName];
                int count = 0;
                foreach (var item in row)
                {
                    var val = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                    if (Geo.Utils.DoubleUtil.IsValid(val) && Condition(val))
                    {
                        count++;
                    }
                }
                data[index] = count;
            }
            return data;
        }

        /// <summary>
        /// 将数据规划到一个区间。可能出现的问题是：如果在周期边界，可能出现分离现象。
        /// 此处采用了二次判断方法，避免该情况。
        /// </summary>
        /// <param name="period"></param>
        /// <param name="referenceVal"></param>
        /// <param name="isInReversedOrder"></param>
        /// <param name="nameSuffix"></param>
        /// <returns></returns>
        public ObjectTableStorage GetPeriodPipeFilterTable(double period = 1, double referenceVal = 0, bool isInReversedOrder = true, string nameSuffix = "InPeriodPipe", bool isReplacePrevPosfix = false)
        {
            if (nameSuffix == "InPeriodPipe")
            {
                nameSuffix = "InPeriod" + period;
            }
            var mgr = new PeriodPipeFilterManager(period, referenceVal);
            var table = HandleAllNumeralCellValue(this, (key, NumeralVal) =>
            {
                return mgr.GetOrCreate(key).Filter(NumeralVal);
            }, isInReversedOrder, nameSuffix, isInReversedOrder);

            //统计，最后数的最大差是否大于等于周期。如果是，则重新换参考数据进行。
            var maxdiffer = table.GetMaxDifferOfLastValue();
            if (maxdiffer < period * 0.51) { return table; }

            var differ = period / 2.0;
            var newMgr = new PeriodPipeFilterManager(period, referenceVal - differ);
            var newTable = HandleAllNumeralCellValue(table, (key, NumeralVal) =>
            {
                return newMgr.GetOrCreate(key).Filter(NumeralVal);
            }, isInReversedOrder, nameSuffix, isInReversedOrder);


            return newTable;
        }
        /// <summary>
        /// 获取最后一行的最相差值。
        /// </summary>
        /// <returns></returns>
        public double GetMaxDifferOfLastValue()
        {
            var row = this.GetLastValueOfAllCols();
            return Geo.Utils.DoubleUtil.MaxDiffer(row.Values);
        }

        /// <summary>
        /// 对除了检索的所有数据乘以一个数，并返回新表。
        /// </summary>
        /// <param name="isReplacePrevPosfix">是否替换以前的后缀名</param>
        /// <param name="multiplier"></param>
        /// <param name="nameSuffix"></param>
        /// <returns></returns>
        public ObjectTableStorage GetTableByMultiply(double multiplier, bool isReplacePrevPosfix = false, string nameSuffix = "MultiMultiplier")
        {
            if (nameSuffix == "MultiMultiplier")
            {
                nameSuffix = "Multi" + multiplier.ToString("0.000");
            }
            return HandleAllNumeralCellValue(this, (key, NumeralVal) =>
            {
                return NumeralVal * multiplier;
            }, false, nameSuffix, isReplacePrevPosfix);
        }

        /// <summary>
        /// 所有数字除以一个数字,返回新表
        /// </summary>
        /// <param name="isReplacePrevPosfix">是否替换以前的后缀名</param>
        /// <param name="divisor"></param>
        /// <param name="nameSuffix"></param>
        /// <returns></returns>
        public ObjectTableStorage GetNewTableByDivision(double divisor, bool isReplacePrevPosfix = false, string nameSuffix = "DiviDivisor")
        {
            if (nameSuffix == "DiviDivisor")
            {
                nameSuffix = "Divi" + divisor.ToString("0.000");
            }
            return HandleAllNumeralCellValue(this, (key, NumeralVal) =>
            {
                return NumeralVal / divisor;
            }, false, nameSuffix, isReplacePrevPosfix);
        }
        /// <summary>
        /// 所有可数字减去一个数字,返回新表
        /// </summary>
        /// <param name="isReplacePrevPosfix">是否替换以前的后缀名</param>
        /// <param name="substractor"></param>
        /// <param name="nameSuffix"></param>
        /// <returns></returns>
        public ObjectTableStorage GetTableByMinus(double substractor, bool isReplacePrevPosfix = false, string nameSuffix = "MinusSubstractor")
        {
            if (nameSuffix == "MinusSubstractor")
            {
                nameSuffix = "Minus" + substractor.ToString("0.000");
            }
            return HandleAllNumeralCellValue((key, NumeralVal) =>
            {
                return NumeralVal - substractor;
            }, false, nameSuffix, isReplacePrevPosfix);
        } 

        /// <summary>
        /// 获取整数。通过减去一个表并四舍五入取整。
        /// </summary>
        /// <param name="tableB"></param>
        /// <param name="tableNamePostfix"></param>
        /// <param name="isReplacePrevPosfix"></param>
        /// <returns></returns>
        public ObjectTableStorage GetIntByMinusAndRound(ObjectTableStorage tableB, string tableNamePostfix, bool isReplacePrevPosfix)
        {
              string name = TableNameHelper.BuildName(tableB.Name, tableNamePostfix, isReplacePrevPosfix);
              return HandleSameNumeralCellValue(tableB, (a, b) =>
              {
                  return Math.Round(a - b);
              }, name);
        }
        /// <summary>
        /// 将所有浮点数取为整数。
        /// </summary> 
        /// <param name="isRoundOrTrim"></param> 
        /// <param name="nameSuffix"></param> 
        /// <returns></returns>
        public ObjectTableStorage GetIntTable(bool isRoundOrTrim = true, string nameSuffix = "Int", bool isReplacePrevPosfix = false)
        {
            return HandleAllNumeralCellValue<int>(this, (key, NumeralVal) =>
            {
                if (isRoundOrTrim) { return (int)Math.Round(NumeralVal); }
                return (int)NumeralVal;
            }, false, nameSuffix, isReplacePrevPosfix);
        }
        /// <summary>
        /// 遍历处理除了检索外的所有数字。
        /// </summary>
        /// <param name="func"></param>
        /// <param name="nameSuffix"></param> 
        /// <param name="isInReversedOrder"></param> 
        /// <returns></returns>
        public ObjectTableStorage HandleAllNumeralCellValue(Func<string, double, double> func, bool isInReversedOrder = false, string nameSuffix = null, bool isReplacePrevPosfix = false)
        {
            return HandleAllNumeralCellValue(this, func, isInReversedOrder, nameSuffix, isReplacePrevPosfix);
        }
        /// <summary>
        /// 遍历处理除了检索外的所有数字。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="func"></param>
        /// <param name="nameSuffix"></param> 
        /// <returns></returns>
        public static ObjectTableStorage HandleAllNumeralCellValue(ObjectTableStorage table, Func<string, double, double> func, bool isInReversedOrder = false, string nameSuffix = null, bool isReplacePrevPosfix = false)
        {
            return HandleAllNumeralCellValue<double>(table, func, isInReversedOrder, nameSuffix, isReplacePrevPosfix);
        }
        /// <summary>
        /// 建立新表，遍历处理除了检索外的所有数字。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colName"></param>
        /// <param name="condition"></param>
        /// <param name="isSaveOrFilt">匹配上的是保存还是过滤</param>
        /// <param name="nameSuffix">名称后缀</param> 
        /// <param name="isInReversedOrder">是否逆序，遍历</param>
        /// <returns></returns>
        public static ObjectTableStorage FilterStringCol(ObjectTableStorage table, string colName, IStringCondition condition, bool isSaveOrFilt= true,  bool isInReversedOrder = false, string nameSuffix = null, bool isReplacePrevPosfix = false)
        {
            string name = TableNameHelper.BuildName(table.Name, nameSuffix, isReplacePrevPosfix);

            var result = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();

            var length = table.RowCount;
            for (int i = 0; i < length; i++)
            {
                int index = i;

                //若逆序，则从最后出发。
                if (isInReversedOrder) { index = length - i - 1; }

                Dictionary<string, Object> row = table.BufferedValues[index];
                if (isInReversedOrder) { result.InsertNewRow(0); }
                else { result.NewRow(); }

                if(row.ContainsKey(colName))
                {
                    var val = row[colName].ToString();

                    if (condition.IsSatisfy(val) && isSaveOrFilt)
                    {
                        result.AddItem(row);
                    }
                    else if(!isSaveOrFilt)
                    {
                        result.AddItem(row);
                    }
                } 
                result.EndRow();
            }
            return result;
        }
        /// <summary>
        /// 建立新表，遍历处理除了检索外的所有数字。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="func"></param>
        /// <param name="nameSuffix">名称后缀</param> 
        /// <param name="isInReversedOrder">是否逆序，遍历</param>
        /// <returns></returns>
        public static ObjectTableStorage HandleAllNumeralCellValue<T>(ObjectTableStorage table, Func<string, double, T> func, bool isInReversedOrder = false, string nameSuffix = null, bool isReplacePrevPosfix = false)
        {
            string name = TableNameHelper.BuildName(table.Name, nameSuffix, isReplacePrevPosfix);

            var storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();

            var length = table.RowCount;
            for (int i = 0; i < length; i++)
            {
                int index = i;

                //若逆序，则从最后出发。
                if (isInReversedOrder) { index = length - i - 1; }

                Dictionary<string, Object> row = table.BufferedValues[index];
                if (isInReversedOrder) { storage.InsertNewRow(0); }
                else { storage.NewRow(); }

                foreach (var kv in row)
                {
                    if (kv.Key.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }
                    var val = Geo.Utils.ObjectUtil.GetNumeral(kv.Value);
                    if (!Double.IsNaN(val))
                    {
                        var key = kv.Key;
                        var newVal = func(key, val);
                        if(newVal is Double)
                        { 
                            if (!Double.IsNaN( Convert.ToDouble( newVal)))
                            {
                                storage.AddItem(key, newVal);
                            }
                        }
                        else
                        {
                            storage.AddItem(key, newVal);
                        }
                    }
                }
                storage.EndRow();
            }
            return storage;
        }

        /// <summary>
        /// 对两个表相同位置的数据进行计算，并返回新表。
        /// </summary>
        /// <param name="tableB"></param>
        /// <param name="Func"></param>
        /// <param name="newTableName"></param>
        /// <returns></returns>
        public ObjectTableStorage HandleSameNumeralCellValue(ObjectTableStorage tableB, Func<double, double, double> Func, string newTableName = null)
        {
            return HandleSameNumeralCellValue(this, tableB, Func, newTableName);
        }
        /// <summary>
        /// 对两个表相同位置的数据进行计算，并返回新表。
        /// </summary>
        /// <param name="Func">封装的函数</param>
        /// <param name="tableA"></param>
        /// <param name="tableB"></param>
        /// <returns></returns>
        public static ObjectTableStorage HandleSameNumeralCellValue(ObjectTableStorage tableA, ObjectTableStorage tableB, Func<double, double, double> Func, string newTableName = null)
        {
            //首先互相同步，确保能算,首先同步其数据顺序和大小，避免大量检索时间耗费 
            tableA.SynchronizeIndexes(tableB);
            tableB.SynchronizeIndexes(tableA);

            var indexColName = tableA.GetIndexColName();
            if (String.IsNullOrWhiteSpace(newTableName))
            {
                newTableName = tableA.Name + "_" + tableB.Name;
            }
            var newTable = new ObjectTableStorage(newTableName);
            var rowCount = Math.Min( tableA.RowCount, tableB.RowCount);
            for (int i = 0; i < rowCount; i++)
            {
                newTable.NewRow();
                var rowA = tableA.GetRow(i);
                var rowB = tableB.GetRow(i);
                foreach (var dicA in rowA)
                {
                    var key = dicA.Key;
                    if (key == indexColName)
                    {
                        newTable.AddItem(indexColName, dicA.Value);
                        continue;
                    }
                    var valA = Geo.Utils.ObjectUtil.GetNumeral(dicA.Value);
                    var valB = Geo.Utils.DictionaryUtil.GetNumeral(rowB, key);
                    if (double.IsNaN(valA) || double.IsNaN(valB)) { continue; }

                    var newVal = Func(valA, valB); //计算

                    if (Geo.Utils.DoubleUtil.IsValid(newVal))
                    {
                        newTable.AddItem(key, newVal);
                    }
                }
                newTable.EndRow();

            }
            return newTable;
        }
        /// <summary>
        /// 遍历处理非空浮点数列。
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="Action"></param>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        public void HandleNumeralRow(string colName, Action<int,object, Dictionary<string, Object>, double> Action, int fromIndex = 0, int count = int.MaxValue)
        {
            HandleRow(colName, (int i, object index,Dictionary<string, object> row, object obj)=>
            {
                var val = (Geo.Utils.ObjectUtil.GetNumeral(obj)) ;
                if (Geo.Utils.DoubleUtil.IsValid(val))
                {
                    Action(i, index, row, val);
                }

            }, fromIndex, count);
        }

        /// <summary>
        /// 遍历非空数值的指定列
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        public void HandleRow(string colName, Action<int, object, Dictionary<string, Object>, object> Action, int fromIndex = 0, int count = int.MaxValue)
        {
            var length = this.RowCount;
            var all = fromIndex + count;
            var indeColName = this.GetIndexColName();
            for (int rowIndex = fromIndex; rowIndex < length && rowIndex < all; rowIndex++)
            {
                Dictionary<string, Object> row = this.BufferedValues[rowIndex];
                if (row.ContainsKey(colName))
                {
                    var val = row[colName];
                    var index = row[indeColName];
                    if (val == null) { continue; }

                    Action(rowIndex, index, row, val);
                }
            }
        }
        /// <summary>
        /// 遍历处理各行
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        public void HandleRow(Action<int, object, Dictionary<string, Object>> Action, int fromIndex = 0, int count = int.MaxValue)
        {
            var length = this.RowCount;
            var all = fromIndex + count;
            var indeColName = this.GetIndexColName();

            for (int rowIndex = fromIndex; rowIndex < length && rowIndex < all; rowIndex++)
            {
                Dictionary<string, Object> row = this.BufferedValues[rowIndex];

                var index = row[indeColName];
                Action(rowIndex, index, row);
            }
        }

        /// <summary>
        /// 遍历除了检索外的所有非空数值的指定列
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        public void HandleNumeralCell(Action<string, int, double> Action, int fromIndex = 0, int count = int.MaxValue)
        {
            var length = this.RowCount;
            var all = fromIndex + count;
            var  indexColName = this.GetIndexColName();
            for (int rowIndex = fromIndex; rowIndex < length && rowIndex < all; rowIndex++)
            {
                Dictionary<string, Object> row = this.BufferedValues[rowIndex];
                foreach (var item in row)
                {
                    if (item.Key == indexColName) { continue; }
                    var val = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                    if (Geo.Utils.DoubleUtil.IsValid(val))
                    {
                        Action(item.Key, rowIndex, val);
                    } 
                } 
            }
        }


        /// <summary>
        /// 遍历处理除了检索外的所有非空值
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        public void HandleCell(Action<string, int, object> Action, int fromIndex = 0, int count = int.MaxValue)
        {
            string indexColName = this.GetIndexColName();

            var length = this.RowCount;
            var all = fromIndex + count;
            for (int rowIndex = fromIndex; rowIndex < length && rowIndex < all; rowIndex++)
            {
                Dictionary<string, Object> row = this.BufferedValues[rowIndex];
                foreach (var kv in row)
                {
                    var colName = kv.Key;
                    if (colName.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    var val = kv.Value;
                    if (val == null) { continue; }

                    Action(colName, rowIndex, val);
                }
            }
        }

        #endregion



        #endregion
        /// <summary>
        /// 设置好元素值
        /// </summary>
        /// <param name="val"></param>
        /// <param name="formRowIndex"></param>
        /// <param name="fromColIndex"></param>
        /// <param name="toRowIndex"></param>
        /// <param name="toColIndex"></param>
        public void SetCellValues(double val, int formRowIndex, int fromColIndex, int toRowIndex = int.MaxValue, int toColIndex = int.MaxValue)
        {
            if (toRowIndex >= this.RowCount)
            {
                toRowIndex = this.RowCount - 1;
            }
            List<string> selectedNames = GetSelectedColNames(fromColIndex, ref toColIndex);

            for (int i = formRowIndex; i <= toRowIndex; i++)
            {
                var row = this.BufferedValues[i];
                foreach (var name in selectedNames)
                {
                    if (row.ContainsKey(name))
                    {
                        row[name] = val;
                    }
                }
            }
            log.Info("设置值"+ val +" 到 (" + formRowIndex + "," + fromColIndex + ") 到 (" + toRowIndex + "," + toColIndex + ") 共" + (toColIndex - fromColIndex + 1) * (toRowIndex - formRowIndex + 1) + " 条数据。");
        }


        /// <summary>
        /// 移除内容
        /// </summary>
        /// <param name="formRowIndex"></param>
        /// <param name="fromColIndex"></param>
        /// <param name="toRowIndex"></param>
        /// <param name="toColIndex"></param>
        public void RemoveCells(int formRowIndex, int fromColIndex, int toRowIndex = int.MaxValue, int toColIndex = int.MaxValue)
        {
            if (toRowIndex >= this.RowCount)
            {
                toRowIndex = this.RowCount - 1;
            }
            List<string> selectedNames = GetSelectedColNames(fromColIndex, ref toColIndex);

            for (int i = formRowIndex; i <= toRowIndex; i++)
            {
                var row = this.BufferedValues[i];
                foreach (var name in selectedNames)
                {
                    if (row.ContainsKey(name))
                    {
                        row.Remove(name);
                    }
                }
            }
            log.Info("移除了从(" + formRowIndex + "," + fromColIndex + ") 到 (" + toRowIndex + "," + toColIndex + ") 共" + (toColIndex - fromColIndex + 1) * (toRowIndex - formRowIndex + 1) + " 条数据。");
        }
        /// <summary>
        /// 获取选择的列名称。
        /// </summary>
        /// <param name="fromColIndex"></param>
        /// <param name="toColIndex"></param>
        /// <returns></returns>
        public List<string> GetSelectedColNames(int fromColIndex, ref int toColIndex)
        {
            if (toColIndex >= this.ColCount)
            {
                toColIndex = this.ColCount - 1;
            }
            List<string> selectedNames = new List<string>();
            for (int j = fromColIndex; j <= toColIndex; j++)
            {
                selectedNames.Add(this.ParamNames[j]);
            }

            return selectedNames;
        }

        /// <summary>
        /// 命名的类型集合。
        /// </summary>
        public List<NamedType> GetNamedTypes()
        {
            List<NamedType> nameTypes = new List<NamedType>();
            var paramNames = GetTitleNames();
            foreach (var item in paramNames)
            {
                var type = GetType(item);
                var namedType = new NamedType(item, type);
                nameTypes.Add(namedType);
            }
            return nameTypes;
        }
        /// <summary>
        /// 获取浮点数。错误则返回NaN
        /// </summary>
        /// <param name="i"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public double GetNumeral(int i, string colName)
        {
            var row = GetRow(i);
            return Geo.Utils.DictionaryUtil.GetNumeral(row, colName);
        }

        /// <summary>
        /// 获取索引指定行、列值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public object GetIndexValue(int i)
        {
            return this.BufferedValues[i][this.GetIndexColName()];
        }

        /// <summary>
        /// 获取索引的行号编号，从0开始。
        /// </summary>
        /// <param name="indexValue"></param>
        /// <returns></returns>
        public int GetRowIndexOfIndexCol(object indexValue)
        {
            var indexs = GetIndexValues();
            return indexs.IndexOf(indexValue);
        }
        /// <summary>
        /// 获取行,没有找到则返回null
        /// </summary>
        /// <param name="indexValue"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetRow(object indexValue)
        {
            var index = GetRowIndexOfIndexCol(indexValue);
            if (index == -1) { return null; }
            return GetRow(index);
        }

        //public event NumeralValueEventHandler NumeralValue
        /// <summary>
        /// 同步索引,删除参数中没有的行。
        /// </summary>
        /// <param name="tableObjectStorage"></param>
        public void SynchronizeIndexes(ObjectTableStorage tableObjectStorage)
        {
            int i = -1;
            var indexName = this.GetIndexColName();
            var indexes = this.GetIndexValues();
            List<int> tobeDeleteIndex = new List<int>();
            foreach (var row in tableObjectStorage.BufferedValues)
            {
                i++;
                if (!row.ContainsKey(indexName))
                {
                    tobeDeleteIndex.Add(i);
                }
                else
                {
                    var indexVal = row[indexName];//外有的

                    var index = indexes.IndexOf(indexVal);

                    if (index == -1)//这里没有，就要删除他们
                    {
                        tobeDeleteIndex.Add(i);
                    }
                }
            }
            tableObjectStorage.RemoveRows(tobeDeleteIndex);
        }

        /// <summary>
        /// 合并表格数据，返回新表，后表数据列追加。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="appdenColPostfix"></param>
        /// <param name="nameSuffix"></param> 
        /// <returns></returns>
        public ObjectTableStorage GetAppendColTable(ObjectTableStorage table, string appdenColPostfix, string nameSuffix = "Appended", bool isReplacePrevPosfix = false)
        {
            string name = this.Name;
            if (!String.IsNullOrEmpty(nameSuffix))
            {
                name = TableNameHelper.BuildName(this.Name, nameSuffix, isReplacePrevPosfix);
            }

            var storage = new ObjectTableStorage(name);

            string indexColName = table.GetIndexColName();

            int i = -1;
            foreach (var row in this.BufferedValues)
            {
                i++;
                var index = row[indexColName];
                storage.NewRow();
                foreach (var kv in row)
                {
                    if (kv.Key.Equals(indexColName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        storage.AddItem(kv.Key, kv.Value);
                        continue;
                    }

                    var val = Geo.Utils.ObjectUtil.GetNumeral(kv.Value);
                    if (!Double.IsNaN(val))
                    {
                        var key = kv.Key;

                        var appdenVal = table[index, key];
                        if (appdenVal == null) { continue; }

                        storage.AddItem(key, kv.Value);
                        storage.AddItem(key + "_" + appdenColPostfix, appdenVal);
                    }
                }
                storage.EndRow();
            }
            return storage;
        }

        /// <summary>
        /// 获取指定列区域的最大值的列和行号
        /// </summary> 
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        public TableCell GetMax(IComparable fromIndex, IComparable toIndex)
        {
            FillIndexes();
            var from = GetRowIndexOfIndexCol(fromIndex);
            var to = GetRowIndexOfIndexCol(toIndex);
            if (to == -1)
            {
                to = IndexOfLastRow;
            }
            return GetMax(from, to);
        }
        public TableCell GetMax(IComparable fromIndex, int count)
        {
            FillIndexes();
            var from = GetRowIndexOfIndexCol(fromIndex);
            var to = from + count; 
            return GetMax(from, to);
        }
        /// <summary>
        /// 获取指定列区域的最大值的列和行号
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public TableCell GetMax(int from=0, int to=int.MaxValue)
        {
            var cell = new TableCell();
            double currentMax = double.MinValue;
            HandleCell(new Action<string, int, object>(delegate(string colName, int rowIndex, object val)
            {
                var num = Geo.Utils.ObjectUtil.GetNumeral(val);
                if (Geo.Utils.DoubleUtil.IsValid(num))
                {
                    if (num > currentMax)
                    {
                        currentMax = num;
                        cell.Value = currentMax;
                        cell.ColName = colName;
                        cell.RowNumber = rowIndex;
                    }
                }

            }), from, to - from);
            return cell;
        }

        /// <summary>
        /// 获取指定列区域的最大值的列和行号
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public TableCell GetMin(int from=0, int to=int.MaxValue)
        {
            var cell = new TableCell();
            double currentMin = double.MaxValue;
            HandleCell(new Action<string, int, object>(delegate(string colName, int rowIndex, object val)
            {
                var num = Geo.Utils.ObjectUtil.GetNumeral(val);
                if (Geo.Utils.DoubleUtil.IsValid(num))
                {
                    if (num < currentMin)
                    {
                        currentMin = num;
                        cell.Value = currentMin;
                        cell.ColName = colName;
                        cell.RowNumber = rowIndex;
                    }
                }

            }), from, to - from);
            return cell;
        }

        /// <summary>
        /// 获取子表，共享 NameListManager
        /// </summary>
        /// <param name="fromIndexValue"></param>
        /// <param name="toIndexValue"></param>
        /// <param name="namePostfix"></param>
        /// <param name="isReplaseSuffixName"></param>
        /// <returns></returns>
        public ObjectTableStorage GetSub(object fromIndexValue, object toIndexValue, string namePostfix="sub", bool isReplaseSuffixName = true)
        {
            FillIndexes();

            var fromRowIndex = GetRowIndexOfIndexCol(fromIndexValue);
            if (fromRowIndex == -1) { return null; }
            var toRowIndex = GetRowIndexOfIndexCol(toIndexValue);
            if (toRowIndex == -1)
            {
                toRowIndex = this.RowCount - 1;
            }
            var newTableName = TableNameHelper.BuildName(this.Name, namePostfix, isReplaseSuffixName);
            var subDic = this.BufferedValues.GetRange(fromRowIndex, toRowIndex - fromRowIndex);

            var newTable = new ObjectTableStorage(newTableName)
            {
                BufferedValues = subDic
            };
            NameListManager NameListManager = new Geo.NameListManager();
            foreach (var item in this.NameListManager)
            {
                NameListManager.Add(item);
            }
            newTable.NameListManager = NameListManager;
            return newTable;
        }
        /// <summary>
        /// 各个列结果的和附在最后。并返回字典。
        /// </summary>
        public Dictionary<string, double> GetAndAppendSumToTable()
        {
            var sumDic = new Dictionary<string, double>();
            var vects = GetVectors(0, this.RowCount, true);
            var indexName = GetIndexColName();

            this.NewRow();
            if (ParamNames.Contains("Name")) { this.AddItem("Name", "Sum"); }
            foreach (var item in vects)
            {
                var key = item.Key;
                if (item.Key == indexName) { continue; }
                var sum = Geo.Utils.DoubleUtil.Sum(item.Value);
                this.AddItem(item.Key, sum);
                sumDic.Add(key, sum);
            }
            return sumDic;
        }
        #region 级联获取
        /// <summary>
        /// 获取基准列中，各个基准数值的关系列表
        /// </summary>
        /// <param name="baseCol"></param>
        /// <returns></returns>
        public Dictionary<string, double> GetCascadeTransferValues(string baseCol)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            var cols = this.GetColObjects(baseCol);
            foreach (var row in this.BufferedValues)
            {
                var baseName = row[baseCol] + "";
                string next = Geo.Utils.ListUtil.GetNext<object>(cols, baseName) + "";
                if (String.IsNullOrWhiteSpace(next))
                {
                    break;
                }
                if (row.ContainsKey(next))
                {
                    var key = next + "-" + baseName;
                    double val = Geo.Utils.ObjectUtil.GetNumeral(row[next]);
                    if (Geo.Utils.DoubleUtil.IsValid(val))
                    {
                        dic[key] = val;
                    }
                }
            }
            return dic;
        }


        /// <summary>
        /// 提取最基本的小数传递表,只保留基本传递列。
        /// </summary>
        /// <param name="baseColName">链表列的名称</param>
        /// <param name="originalTransTable">原始链表列数据</param>
        /// <returns></returns>
        public static ObjectTableStorage GetBasicFractionCascadeTransTable(ObjectTableStorage originalTransTable, string baseColName, string marker)
        {
            ObjectTableStorage table = new ObjectTableStorage("BasicTranfer_" + baseColName + marker);
            var baseColValues = Geo.Utils.ListUtil.GetNoRepeatList<string>(originalTransTable.GetColStrings(baseColName));

            int i = -1;
            foreach (var prevNode in baseColValues)
            {
                i++;
                if (i >= baseColValues.Count - 1) { continue; }
                //分别从上下两行，互相包含中提取
                var row = originalTransTable.GetLastRowWithValue(baseColName, prevNode);
                //获取下一个链节点名称
                string nextNode = baseColValues[i + 1];
                var nextRow = originalTransTable.GetFirstRowWithValue(baseColName, nextNode);

                //如果当前行具有下一节点的值，则保存到新表中
                double val = Geo.Utils.DictionaryUtil.GetNumeral(row, nextNode);
                var val3 = val;
                if (Geo.Utils.DoubleUtil.IsValid(val))
                {
                    PeriodPipeFilter filter = new PeriodPipeFilter(1, val);
                    double val2 = -1.0 * Geo.Utils.DictionaryUtil.GetNumeral(nextRow, prevNode);
                    val3 = filter.Filter(val2);
                }
                var valu = Geo.Utils.DoubleUtil.Average(val, val3);
                if (Geo.Utils.DoubleUtil.IsValid(val))
                {
                    table.NewRow();
                    table.AddItem(baseColName, prevNode);
                    //table.AddItem(prevNode, 0);
                    table.AddItem(nextNode, valu);
                }
            }
            //var last = baseColValues[baseColValues.Count - 1];
            //table.NewRow();
            //table.AddItem(baseColName, last);
            //table.AddItem(last, 0);
            return table;
        }

        /// <summary>
        /// 获取各列第一个有效值。
        /// </summary> 
        /// <returns></returns>
        private Dictionary<string, object> GetFirstValidValue()
        { 
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var name in this.ParamNames)
            {
                dic[name] = GetFirstValue(name);
            }
            return dic;
        }

        /// <summary>
        /// 获取各列最后一个有效值。
        /// </summary> 
        /// <returns></returns>
        private Dictionary<string, object> GetLastValidValue()
        { 
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var name in this.ParamNames)
            {
                dic[name] = GetLastValue(name);
            }
            return dic;
        }
        /// <summary>
        /// 获取包含指定列第一个值
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="rowValue"></param>
        /// <returns></returns>
        private object GetFirstValue(string colName)
        {
            var length = this.RowCount;
            foreach (var row in this.BufferedValues)
            {
                if (row.ContainsKey(colName) && row[colName] != null) { return row[colName]; }
            }
            return null;
        }
        /// <summary>
        /// 获取包含指定列和指定数值的第一行
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="rowValue"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetFirstRowWithValue(string colName, object rowValue)
        {
            var length = this.RowCount;
            foreach (var row in this.BufferedValues)
            {
                if (row.ContainsKey(colName)) { if (row[colName].Equals(rowValue)) { return row; } }
            }
            return null;
        }
        /// <summary>
        /// 获取包含指定列和指定数值的最后一行
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetLastRowWithValue(string colName, object rowValue)
        {
            var length = this.RowCount;
            for (int i = length - 1; i > -1; i--)
            {
                var row = this.BufferedValues[i];
                if (row.ContainsKey(colName))
                {
                    if (row[colName].Equals(rowValue)) { return row; }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取指定列最后一行是有效数值的数值
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        private object GetLastValidValue(string colName)
        {
            var length = this.RowCount;
            for (int i = length - 1; i > -1; i--)
            {
                var row = this.BufferedValues[i];
                if (row.ContainsKey(colName) && row[colName]!=null)
                {
                    var val = row[colName];
                    return val;
                  //  if (Geo.Utils.ObjectUtil.IsNumerial(currentVal)) { return Geo.Utils.ObjectUtil.GetNumeral(currentVal); } 
                }
            }
            return null;// Double.NaN;
        }


        /// <summary>
        /// 获取级联更新表。
        /// </summary>
        /// <param name="basicTansTable"></param> 
        /// <param name="nameSuffix"></param>
        /// <param name="isReplacePrevPosfix"></param>
        /// <returns></returns>
        public static ObjectTableStorage GetCascadePlusTransferTable(ObjectTableStorage basicTansTable, List<string> nodes, string nameSuffix = "CascadePlus", bool isReplacePrevPosfix = true)
        {
            var indexName = basicTansTable.GetIndexColName();
            if (nodes.Count <= 1) { return basicTansTable; }

            //逆序获取
            string name = TableNameHelper.BuildName(basicTansTable.Name + "-" + indexName, nameSuffix, isReplacePrevPosfix);
            var storage = new ObjectTableStorage(name);
            foreach (var fromNode in nodes)
            {
                storage.NewRow();
                storage.AddItem(indexName, fromNode);
                foreach (var toBaseNode in nodes)
                {
                    var val = GetCascadeTransferValue(basicTansTable, fromNode, toBaseNode);
                    if (Geo.Utils.DoubleUtil.IsValid(val))
                    {
                        storage.AddItem(toBaseNode, val);
                    }
                }
                storage.EndRow();
            }

            return storage;
        }

        /// <summary>
        /// 获取传递
        /// </summary>
        /// <param name="fromNode">传递到的节点</param>
        /// <param name="toBaseNode">基础节点，终点</param>
        /// <param name="initTransferValue"></param>
        /// <returns></returns>
        public static double GetCascadeTransferValue(ObjectTableStorage table, string fromNode, string toBaseNode, double initTransferValue = 0)
        {
            if (fromNode == toBaseNode) { return 0; }

            var row = table.GetRow(toBaseNode);
            if (row == null)
            { //获取失败
                return double.NaN;
            }
            //当前终节点可以推导的节点。
            var nextNode = Geo.Utils.DictionaryUtil.GetLastKey<string, object>(row);
            var transferValue = Geo.Utils.ObjectUtil.GetNumeral(row[nextNode]);

            initTransferValue += transferValue;
            if (nextNode == fromNode)//以及找到节点啦，直接返回
            {
                return initTransferValue;
            }
            //继续寻找
            return GetCascadeTransferValue(table, fromNode, nextNode, initTransferValue);
        }
        #endregion
        /// <summary>
        /// 移除有效数据（行数）少于指定数的列
        /// </summary>
        /// <param name="minRowCount"></param>
        /// <returns></returns>
        public int RemoveRowCountLessThan(int minRowCount, Func< double, bool> Condition)
        {
            var dic = this.GetValidDataCount(Condition );
            int count = 0;
            foreach (var item in dic)
            {
                if (item.Value < minRowCount)
                {
                    this.RemoveCol(item.Key);
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// 把检索列填满,避免空隙
        /// </summary>
        public void FillIndexes()
        {
            var indexName = this.GetIndexColName();
            if (FirstIndex.GetType() == typeof(DateTime))
            {
                var first = (DateTime)FirstIndex;
                var second = (DateTime)SecondIndex;
                var last = (DateTime)LastIndex;
                var interval = second - first;
                var totalSpan = last - first;
                //是否已满
                if (this.RowCount >= totalSpan.TotalSeconds / interval.TotalSeconds)
                {
                    return;
                }
                //检查并填充
                var indexes = this.GetIndexValues<DateTime>();
                int i = 0;
                for (var index = first; index < last; index = index + interval, i++)
                {
                    if (!indexes.Contains(index))
                    {
                        this.InsertNewRow(i);
                        this.AddItem(indexName, index);
                        this.EndRow();
                    }
                }
            }
            if (FirstIndex.GetType() == typeof(Int32))
            {
                var first = (Int32)FirstIndex;
                var second = (Int32)SecondIndex;
                var last = (Int32)LastIndex;
                var interval = second - first;
                var totalSpan = last - first;
                //是否已满
                if (this.RowCount >= totalSpan / interval)
                {
                    return;
                }
                //检查并填充
                var indexes = this.GetIndexValues<Int32>();
                int i = 0;
                for (var index = first; index < last; index = index + interval, i++)
                {
                    if (!indexes.Contains(index))
                    {
                        this.InsertNewRow(i);
                        this.AddItem(indexName, index);
                        this.EndRow();
                    }
                }
            } 
        }
        /// <summary>
        /// 移除最后值偏离中心的列。
        /// </summary>
        /// <param name="offCenter"></param>
        /// <param name="loop"></param>
        public void RemoveColWithLastValOffCenter(double offCenter, bool loop)
        {
            var ave = GetAverageOfLastValueOfAllCols();
            var indexCol = this.GetIndexColName();
            var toberemove = new List<string>();
            foreach (var item in this.ParamNames)
            {
                if (item == indexCol) { continue; }
                var valObj = GetLastValue(item);
                var val = Geo.Utils.ObjectUtil.GetNumeral(valObj);
                if (Geo.Utils.DoubleUtil.IsValid(val) && Math.Abs(val - ave) > offCenter)
                {
                    toberemove.Add(item);
                    log.Debug(this.Name + " 即将移除列 " + item + "，最后值 " + val.ToString("0.00##") + " 与均值偏离 " + (val - ave).ToString("0.00##"));
                }
            }
            if (toberemove.Count > 0) { this.RemoveCols(toberemove); }
        }
        /// <summary>
        /// 获取所有列最后数据的平均数
        /// </summary>
        public double GetAverageOfLastValueOfAllCols()
        {
            var lastVal = GetLastValueOfAllCols();
            return Geo.Utils.DoubleUtil.Average(lastVal.Values);
        }
        /// <summary>
        /// 同步表，删除多余列
        /// </summary>
        /// <param name="table"></param>
        public void SynchronizeCol(ObjectTableStorage table)
        {
            var differCol = Geo.Utils.ListUtil.GetDifferences<string>(this.ParamNames, table.ParamNames);
            foreach (var item in differCol)
            {
                this.RemoveCol(item);
            }
        }
        /// <summary>
        /// 移除不平坦的列，即所有列的行的内容应该为相同的数据。
        /// </summary>
        public void RemoveColsOfUneven()
        {
            var dicVal = GetVectorsAvailable();
            var indexName = this.GetIndexColName();
            foreach (var item in dicVal)
            {
                if (item.Key == indexName) { continue; }
                if (item.Value.Count == 0 || Geo.Utils.DoubleUtil.IsAllEquals(item.Value))
                {
                    this.RemoveCol(item.Key);
                }
            }
        }



        /// <summary>
        /// 当前减去下一个的数值，若已经到头，则返回 0 。
        /// 若分double 返回 NAN
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public double MinusNext<T>(string colName, T startTime)
        {
             var  rowInex =GetRowIndexOfIndexCol(startTime);
             var nextIndex = rowInex + 1;
             if (nextIndex >= this.RowCount) { return 0; }

             var result = this.GetNumeral(rowInex, colName) - this.GetNumeral(nextIndex, colName);
             return result;
        }

        public void NewRow(List<string> list)
        {
            this.NewRow();

            AddItem(list);
        }
        /// <summary>
        /// 增加列表，有的为true。
        /// </summary>
        /// <param name="list"></param>
        public void AddItem(List<string> list)
        {
            foreach (var item in list)
            {
                this.AddItem(item, true);
            }
        }

        /// <summary>
        ///根据三个关键字，解析为双键数据表
        /// </summary>
        /// <param name="keyAName"></param>
        /// <param name="keyBName"></param>
        /// <param name="valName"></param>
        /// <returns></returns>
        public TwoNumeralKeyAndValueDictionary GetNumeralKeyDic(string keyAName, string keyBName, string valName)
        {
            if (!this.FirstRow.ContainsKey(keyAName) || !this.FirstRow.ContainsKey(keyBName) || !this.FirstRow.ContainsKey(valName))
            {
                throw new ArgumentException("必须包含列名： " + keyAName + ", " + keyBName + ", " + valName + "。 ");
            }
            TwoNumeralKeyAndValueDictionary dic = new TwoNumeralKeyAndValueDictionary();
            foreach (var row in this.BufferedValues)
            {
                if (!row.ContainsKey(valName)) { continue; }

                double keyA = Geo.Utils.ObjectUtil.GetNumeral(row[keyAName]);
                double keyB = Geo.Utils.ObjectUtil.GetNumeral(row[keyBName]);
                double val = Geo.Utils.ObjectUtil.GetNumeral(row[valName]);
                dic.Set(keyA, keyB, val);
            }
            dic.Init();
            return dic;
        }
        /// <summary>
        ///解析为双键数据表，其它列为另一键值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TwoKeyDictionary<Time, string, double> GetTwoKeyDictionary(string key)
        {
            if (!this.FirstRow.ContainsKey(key))
            {
                throw new ArgumentException("必须包含列名： " + key + "。 ");
            }
            var dic = new TwoKeyDictionary<Time, string, double>();
            foreach (var row in this.BufferedValues)
            {
                var keyA = Time.Parse(row[key].ToString());

                foreach (var kv in row)
                {
                    if (kv.Key == key) { continue; }

                    double val = Geo.Utils.ObjectUtil.GetNumeral(row[kv.Key]);
                    dic.Set(keyA, kv.Key, val);
                } 
            }
         //   dic.Init();
            return dic;
        }
    } 
}