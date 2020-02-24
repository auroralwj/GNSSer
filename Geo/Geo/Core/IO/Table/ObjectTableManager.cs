//2016.03.22, czs, created in hongqing, 基于卫星结果的管理器 
//2016.03.29, czs, edit in hongqing, 名称修改为 NamedValueTableManager
//2016.05.09, czs, create in hongqing, 表数据管理器
//2016.08.05, czs, edit in fujian yongan, 重构
//2016.10.03, czs, eidt in hongqing, 增加实时输出选项，以减少内存消耗
//2017.03.08, czs, edit in hongqing, 增加了大量并行化数据表处理方法
//2019.02.20, czs, edit in hongqing, TableObjectManager 更名为 ObjectTableManager

using Geo.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace Geo
{

    /// <summary>
    /// 表数据管理器。
    /// </summary>
    public class ObjectTableManager : BaseDictionary<string, ObjectTableStorage>
    {
        ILog log = new Log(typeof(ObjectTableManager));
        /// <summary>
        /// 采用字典数据直接初始化
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="OutputBufferCount">缓存数量，达到后将写入文件</param>
        /// <param name="directory">存储目录</param>
        /// <param name="Name">表名称</param>
        public ObjectTableManager(IDictionary<string, ObjectTableStorage> dic, int OutputBufferCount = 10000, string directory = null, string Name = "未命名")
            : this(OutputBufferCount, directory, Name)
        {
            this.SetData( dic);
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OutputBufferCount">缓存数量，达到后将写入文件</param>
        /// <param name="directory">存储目录</param>
        /// <param name="Name">表名称</param>
        public ObjectTableManager(string directory, int OutputBufferCount = 10000, string Name = "未命名")
            : this(OutputBufferCount, directory, Name)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OutputBufferCount">缓存数量，达到后将写入文件</param>
        /// <param name="directory">存储目录</param>
        /// <param name="Name">表名称</param>
        public ObjectTableManager(int OutputBufferCount = 10000, string directory = null, string Name = "未命名")//, bool IsRealTimeOutput)
        {
            this.OutputBufferCount = OutputBufferCount;
            this.Name = Name;
            if (directory == null)
            {
                directory = Setting.TempDirectory;
            }
            this.OutputDirectory = directory;
            if (String.IsNullOrWhiteSpace(directory)) { Geo.Utils.FileUtil.CheckOrCreateDirectory(directory); }

            log.Debug("新建了表管理器 " + this.Name + " " + this.OutputDirectory);
        }


        /// <summary>
        ///按照当前路径和缓存大小，建立一个新管理器。
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
        public ObjectTableManager CreateNew(string newName = "未命名")
        {
            return new ObjectTableManager(this.OutputBufferCount, this.OutputDirectory, newName);
        }
        /// <summary>
        /// 以当前目录和缓存数量创建一个新的管理器
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public ObjectTableManager CreateNew(IDictionary<string, ObjectTableStorage> dic, string newName = "未命名")
        {
            return new ObjectTableManager(dic, this.OutputBufferCount, this.OutputDirectory, newName);
        }
        #region 属性
        /// <summary>
        /// 后缀
        /// </summary>
        public static string TableExtention = Setting.TextTableFileExtension;// ".xls";
        /// <summary>
        /// 输出缓存数量大小
        /// </summary>
        public int OutputBufferCount { get; set; }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get;  set; }
        #endregion

        #region 方法
        /// <summary>
        /// 检索器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override ObjectTableStorage this[string key]
        {
            get
            {
                var name = key;
                if (!this.Contains(key))
                {
                    name += TableExtention;
                }
                return Data[key];
            }
            set
            {
                this.Set(key, value);
            }
        }
        /// <summary>
        /// 逆序所有行
        /// </summary>
        public void ReverseAllRows()
        {
            Parallel.ForEach<ObjectTableStorage>(this, item =>
            {
                item.ReverseRows();
            });
        }

        /// <summary>
        /// 将所有的列归算到指定的区间。
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="period"></param>
        /// <param name="reference"></param>
        public void ReductValuesTo(double period = 1, double reference = 0.5)
        {
            Parallel.ForEach<ObjectTableStorage>(this, item =>
            {
                item.ReductValuesTo(period, reference);
            });
        }
        /// <summary>
        /// 清空存储。
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            ClearParamNames();
        }
        /// <summary>
        /// 清空已经注册的名称列表
        /// </summary>
        public void ClearParamNames()
        {
            foreach (var item in this) { item.Clear(); }
        }
        /// <summary>
        /// 添加一个表，以表名为关键字。
        /// </summary>
        /// <param name="table"></param>
        public void Add(ObjectTableStorage table) { this.Add(table.Name, table); }
        /// <summary>
        /// 添加一个表。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public override void Add(string key, ObjectTableStorage val)
        {
            base.Add(key, val);
            if (String.IsNullOrWhiteSpace(val.Name))
            {
                val.Name = key;
            }
        }


        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override bool Contains(string tableName){
            if (base.Contains(tableName)) { return true; }
            if (base.Contains(tableName + TableExtention)) return true;
            return false;
        }

        /// <summary>
        /// 创建一个表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override ObjectTableStorage Create(string tableName)
        {
            AddTable(tableName); return this[tableName];
        }
        /// <summary>
        /// 新建，添加一个数据表,自动设置存储路径。
        /// </summary>
        /// <param name="tableName">表名称，以此写为文件，如 Params.xls;如果没有xls后缀，系统将自动添加一个。</param>
        public ObjectTableStorage AddTable(string tableName)
        {
            var table = new ObjectTableStorage(tableName);
            Add(tableName, table);
            return table;
        }
        /// <summary>
        /// 添加表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        public void AddTable(string tableName, ObjectTableStorage table)
        {
            table.Name = tableName;
            Add(tableName, table);
        }
        /// <summary>
        /// 添加表
        /// </summary> 
        /// <param name="table"></param>
        public void AddTable(ObjectTableStorage table)
        {
            Add(table.Name, table);
        }
        /// <summary>
        /// 添加表
        /// </summary> 
        /// <param name="tables"></param>
        public void AddTable(IEnumerable<ObjectTableStorage> tables)
        {
            foreach (var table in tables)
            {
                Add(table);
            }
        }

        #region 写文件
        /// <summary>
        /// 写入AllInOne文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="headerMarker"></param>
        /// <param name="spliter"></param>
        public void WriteAsOneFile(string fileName = null, string headerMarker = "#", string spliter = "\t")
        {
            if (this.Count == 0) { return; }

            if (fileName == null) { fileName = this.Name + ".aio"; }
            if (!TableExtention.Equals(Path.GetExtension(fileName), StringComparison.CurrentCultureIgnoreCase))
            {
                fileName += TableExtention;
            }

            var path = Path.Combine(OutputDirectory, fileName);

            Stream stream = new FileStream(path, FileMode.Create);

            WriteAsOneFile(stream, headerMarker, spliter);
        }
        /// <summary>
        /// 写入AllInOne文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="headerMarker"></param>
        /// <param name="spliter"></param>
        /// <param name="encoding"></param>
        public void WriteAsOneFile(Stream stream, string headerMarker = "#", string spliter = "\t", Encoding encoding = null)
        {
            if(encoding == null) { encoding = Encoding.Default; }

            using (StreamWriter writer = new System.IO.StreamWriter(stream, encoding))
            {
                var length = GetMaxTableRowCount();
                for (int i = -1; i < length; i++)
                {
                    foreach (var table in this)
                    {
                        if (table.RowCount == 0) { continue; }

                        if (table.RowCount > i)
                        {
                            if (i == -1)
                            {
                                writer.WriteLine(headerMarker + table.Name + spliter + table.ToSplitedTitleString(spliter));
                                continue;
                            }
                            var row = table.BufferedValues[i];

                            writer.WriteLine(table.Name + spliter + ObjectTableWriter.ToSplitedValueString(row, "", spliter, "G", table.ParamNames));
                        }
                    }
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// 获取最长表行
        /// </summary>
        /// <returns></returns>
        public int GetMaxTableRowCount()
        {
            int row = 0;
            foreach (var table in this)
            {
                if (table.RowCount > row) { row = table.RowCount; }
            }
            return row;
        }
        

        /// <summary>
        /// 采用默认文件名，写入并关闭数据流
        /// </summary>
        public void WriteAllToFileAndCloseStream()
        {
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var storage = this[item];
                storage.Name = item;//名称避免不同
                WriteTable(storage);
            });
        }

        /// <summary>
        /// 采用默认文件名，写入,并清空缓存。
        /// </summary>
        /// <param name="exceptClear">不清空的一个</param>
        public void WriteAllToFileAndClearBuffer(ObjectTableStorage exceptClear =null)
        {
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var storage = this[item];
                storage.Name = item;//名称避免不同
                WriteTable(storage);

                if (exceptClear != storage)
                {
                    storage.Clear();
                }
            });
        }
        
        /// <summary>
        /// 写一个表
        /// </summary>
        /// <param name="storage"></param>
        public void WriteTable(ObjectTableStorage storage)
        {
            if (storage.RowCount == 0) { log.Info(storage.Name + " 没有数据，不输出。"); return; }
            var directory = this.OutputDirectory;
            WriteTable(storage, directory);
        }
        /// <summary>
        /// 写一个表
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="directory"></param>
        public static void WriteTable(ObjectTableStorage storage, string directory)
        {
            var OutputPath = Path.Combine(directory, storage.Name);
            if (!OutputPath.EndsWith(TableExtention, StringComparison.CurrentCultureIgnoreCase))
            {
                OutputPath += TableExtention;
            }

            var writer = new ObjectTableWriter(OutputPath);
            writer.Write(storage);
            writer.CloseStream();
        }
        #endregion


        /// <summary>
        /// 替换指定字符
        /// </summary>
        /// <param name="oldStr"></param>
        /// <returns></returns>
        public ObjectTableManager GetReplacedColNameTable(string oldStr, string newstr = "")
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var newTable = this[item].GetReplacedColNameTable(oldStr, newstr);
                dic.TryAdd(newTable.Name, newTable);
            });
            return new ObjectTableManager(dic, this.OutputBufferCount, OutputDirectory);
        }
        /// <summary>
        /// 替换指定字符
        /// </summary>
        /// <param name="tobeRemoved"></param>
        /// <returns></returns>
        public ObjectTableManager GetRepalcedColNameTable(string[] tobeRemoved)
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var newTable = this[item].RemoveColString(tobeRemoved);
                dic.TryAdd(newTable.Name, newTable);
            });
            return new ObjectTableManager(dic, this.OutputBufferCount, OutputDirectory);
        }
        #endregion
        /// <summary>
        /// 合并到一个表格
        /// </summary>
        /// <param name="newTableName"></param>
        /// <returns></returns>
        public ObjectTableStorage Combine(string newTableName = null)
        {
            if (String.IsNullOrWhiteSpace(newTableName)) { newTableName = this.Name; }
            ObjectTableStorage table = new ObjectTableStorage(newTableName);
            foreach (var tab in this)
            {
                foreach (var item in tab.BufferedValues)
                {
                    table.NewRow();
                    table.AddItem(item);
                }
            }
            return table;
        }
        /// <summary>
        /// 完全平滑
        /// </summary>
        /// <param name="maxError">最大误差</param>
        /// <param name="isAsWhole">是否全局考虑</param>
        /// <param name="namePostfix">表后缀</param>
        /// <param name="isReplaceNamePosfix">是否移除之前的后缀</param>
        /// <returns></returns>
        public ObjectTableManager GetSmoothedTable(double maxError, bool isAsWhole = true, string namePostfix = "Smoothed", bool isReplaceNamePosfix = true)
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var newTable = this[item].GetSquentionAjustSmoothedTable(maxError, isAsWhole, namePostfix, isReplaceNamePosfix);
                dic.TryAdd(newTable.Name, newTable);
            });
            var mwTables = new ObjectTableManager(dic, this.OutputBufferCount, OutputDirectory);
            return mwTables;
        }
        /// <summary>
        /// 将数据规划到一个区间。可能出现的问题是：如果在周期边界，可能出现分离现象。
        /// </summary>
        /// <param name="period"></param>
        /// <param name="referenceVal"></param>
        /// <param name="isInReversedOrder"></param>
        /// <param name="nameSuffix"></param>
        /// <returns></returns>
        public ObjectTableManager GetPeriodPipeFilterTable(double period = 1, double referenceVal = 0.5, bool isInReversedOrder = true, string nameSuffix = "InPeriodPipe", bool isReplacePrevPosfix = false)
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var key = item;// +"-" + subtractor;
                var table = this[item];
                if (table.RowCount != 0)
                {
                    var newTable = table.GetPeriodPipeFilterTable(period, referenceVal, isInReversedOrder, nameSuffix, isReplacePrevPosfix);
                    dic.TryAdd(newTable.Name, newTable);
                }
            });
            return new ObjectTableManager(dic, this.OutputBufferCount, this.OutputDirectory);
        }
        /// <summary>
        /// 获取表格，如果匹配失败，则分割后继续比较，分隔符在 TableNameHelper 中指定，通常为下划线“_”
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override ObjectTableStorage Get(string key)
        {
            var table = base.Get(key);
            if (table == null)
            {
                foreach (var item in this.Data)
                {
                    if (TableNameHelper.IsPrefixEquals(item.Key, key)) { return item.Value; }
                }
            }
            return table;
        }
        /// <summary>
        /// 对同名列进行统计，返回表。
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetValidDataCount(Func<double, bool> Condition, string tableName = "ValidDataCount")
        {
            ObjectTableStorage table = new ObjectTableStorage(tableName);

            foreach (var item in this)
            {
                table.NewRow();
                var name = TableNameHelper.ParseName(item.Name);
                table.AddItem("Name", name);
                var dic = item.GetValidDataCount(Condition);
                table.AddItem(dic);
                table.EndRow();
            }
            return table;
        }
        /// <summary>
        /// 获取减去一个列的新表。
        /// </summary>
        /// <param name="colNameOfSubtractor"></param>
        /// <returns></returns>
        public ObjectTableManager GetNewByMinusCol(string colNameOfSubtractor,string tablePostfixName = "-ColName", bool isReplacePrevPosfix = false)
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var table = this[item].GetTableByMinusCol(colNameOfSubtractor, tablePostfixName, isReplacePrevPosfix);
                dic.TryAdd(table.Name, table);
            });
            var mwTables = new ObjectTableManager(dic, this.OutputBufferCount, OutputDirectory);

            return mwTables;
        }
        /// <summary>
        /// 设置统一的IndexColName
        /// </summary>
        /// <param name="indexColName"></param>
        public void SetIndexColName(string indexColName)
        {
            foreach (var item in this)
            {
                item.IndexColName = indexColName;
            }
        }
        /// <summary>
        /// 获取包含所有表行的Index列。
        /// </summary>
        /// <returns></returns>
        public List<Object> GetIndexesOfAllTables() { return GetIndexesOfAllTables<object>(); }
        /// <summary>
        /// 获取包含所有表行的Index列。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetIndexesOfAllTables<T>()
        {
            var indexName = this.First.GetIndexColName();
            List<T> indexes = new List<T>();
            int i = -1;
            foreach (var item in this.Data)
            {
                i++;

                var oneIndexe = item.Value.GetIndexValues<T>();
                if (i == 0)
                {
                    indexes.AddRange(oneIndexe);
                }

                indexes = Geo.Utils.ListUtil.Emerge<T>(indexes, oneIndexe);
            }
            indexes.Sort();
            return indexes;
        }
        /// <summary>
        /// 获取包含所有表列名称。
        /// </summary> 
        /// <returns></returns>
        public List<string> GetColNamesOfAllTables()
        {
            var indexName = this.First.GetIndexColName();
            List<string> names = new List<string>();
            int i = -1;
            foreach (var item in this.Data)
            {
                i++;

                var oneNames = item.Value.ParamNames;
                if (i == 0)
                {
                    names.AddRange(oneNames);
                }

                names = Geo.Utils.ListUtil.Emerge<String>(names, oneNames);
            }
            // names.Sort();
            return names;
        }
        /// <summary>
        /// 将相同列合并到一个表格中，标题以不同表名称命名。
        /// 提取所有表格的检索和所有表的名称作为新表的行和列。老表名称为新表的列，新表的列为老表的名称。
        /// </summary> 
        /// <param name="tableNamePostfix"></param>
        /// <returns></returns>
        public ObjectTableManager GetSameColAssembledTableManager(string tableNamePostfix = "")
        {
            var indexes = GetIndexesOfAllTables();
            var colNames = GetColNamesOfAllTables();

            return GetSameColAssembledTableManager(indexes, colNames, tableNamePostfix);
        }
        /// <summary>
        /// 将相同列合并到一个表格中，标题以不同表名称命名。
        /// 以其中一个表格为基准。老表名称为新表的列，新表的列为老表的名称。
        /// </summary> 
        /// <param name="baseTableName"></param>
        /// <param name="tableNamePostfix"></param>
        /// <returns></returns>
        public ObjectTableManager GetSameColAssembledTableManager(string baseTableName, string tableNamePostfix = "")
        {
            if (String.IsNullOrWhiteSpace(baseTableName))
            {
                baseTableName = this.FirstKey;
            }
            var baseTable = this[baseTableName];
            var colNames = baseTable.ParamNames;
            var indexes = baseTable.GetIndexValues();
            return GetSameColAssembledTableManager(indexes, colNames, tableNamePostfix);
        }
        /// <summary>
        ///  将相同列合并到一个表格中，标题以不同表名称命名。
        /// </summary>
        /// <param name="indexes"></param>
        /// <param name="colNames"></param>
        /// <param name="tableNamePostfix"></param>
        /// <returns></returns>
        public ObjectTableManager GetSameColAssembledTableManager(List<object> indexes, List<string> colNames, string tableNamePostfix = "")
        {
            var indexColName = this.GetIndexColName();
            ObjectTableManager newManger = new ObjectTableManager(this.OutputDirectory);
            //建立表格
            foreach (var oldColName in colNames)
            {
                if (indexColName.Equals(oldColName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                var newTableName = oldColName;
                if (!String.IsNullOrWhiteSpace(tableNamePostfix)) { newTableName = TableNameHelper.BuildName(oldColName, tableNamePostfix, true); }

                newManger.AddTable(newTableName);
            }
            //填充数据
            foreach (var indexVal in indexes)//逐历元
            {
                foreach (var oldColName in colNames) //逐新表
                {
                    if (indexColName.Equals(oldColName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                    var newTable = newManger.Get(oldColName);

                    newTable.NewRow();

                    newTable.AddItem(indexColName, indexVal);
                    foreach (var oldTableName in this.Keys) // 逐新表列, 逐老表
                    {
                        var oldTable = this[oldTableName];

                        if (!oldTable.ParamNames.Contains(oldColName))
                        {
                            continue;
                        }

                        var val = oldTable[indexVal, oldColName];
                        if (val != null)
                        {
                            var colName = TableNameHelper.ParseName(oldTableName);
                            newTable.AddItem(colName, val);
                        }
                    }
                    newTable.EndRow();
                }
            }
            return newManger;
        }
        /// <summary>
        /// 获取检索列名称
        /// </summary>
        /// <returns></returns>
        public string GetIndexColName()
        {
            foreach (var item in this)
            {
                var indexColName = item.GetIndexColName();
                if (indexColName != null) { return indexColName; }
            }
            return null;
        }
        /// <summary>
        /// 移除空行。编号列由数据也要移除。
        /// </summary>
        public void RemoveEmptyRows()
        {
            Parallel.ForEach<ObjectTableStorage>(this, item =>
            {
                item.RemoveEmptyRows();
            });
        }
        /// <summary>
        /// 移除注册数据少于指定百分比的行.
        /// </summary>
        /// <param name="percentageOrCount"></param>
        /// <param name="isPercentage"></param>
        public void RemoveRowsWithRegistDataLessThan(double percentageOrCount, bool isPercentage = true)
        {
            Parallel.ForEach<ObjectTableStorage>(this, item =>
            {
                item.RemoveRowsWithRegistDataLessThan(percentageOrCount, isPercentage);
            });
        }
        /// <summary>
        /// 同步索引,删除参数中没有的行。
        /// </summary>
        /// <param name="intWideLaneValues"></param>
        public void SynchronizeIndexes(ObjectTableManager intWideLaneValues)
        {
            Parallel.ForEach<string>(this.Keys, item =>
            {
                if (intWideLaneValues.Contains(item))
                {
                    this[item].SynchronizeIndexes(intWideLaneValues[item]);
                }
            });
        }

        public void RemoveRows(int from, int count)
        {
            Parallel.ForEach<ObjectTableStorage>(this, item =>
            {
                item.RemoveRows(from, count);
            });
        }
        /// <summary>
        /// chengfa
        /// </summary>
        /// <param name="multiplier"></param>
        /// <param name="isReplacePrevPosfix"></param>
        /// <param name="nameSuffix"></param>
        /// <returns></returns>
        public ObjectTableManager GetNewTableByMultiply(double multiplier, bool isReplacePrevPosfix = false, string nameSuffix = "MultiMultiplier")
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var table = this[item].GetTableByMultiply(multiplier, isReplacePrevPosfix, nameSuffix);
                dic.TryAdd(table.Name, table);
            });
            var mwTables = new ObjectTableManager(dic, this.OutputBufferCount, OutputDirectory);
            return mwTables;
        }
        /// <summary>
        /// DiviDivisor
        /// </summary>
        /// <param name="divisor"></param>
        /// <param name="isReplacePrevPosfix"></param>
        /// <param name="nameSuffix"></param>
        /// <returns></returns>
        public ObjectTableManager GetNewTableByDivision(double divisor, bool isReplacePrevPosfix = false, string nameSuffix = "DiviDivisor")
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var table = this[item].GetNewTableByDivision(divisor, isReplacePrevPosfix, nameSuffix);
                dic.TryAdd(table.Name, table);
            });
            var mwTables = new ObjectTableManager(dic, this.OutputBufferCount, OutputDirectory);
            return mwTables;
        }
        /// <summary>
        /// 对两个表相同位置的数据进行计算，并返回新表。
        /// </summary>
        /// <param name="tableBManager"></param>
        /// <param name="Func"></param>
        /// <param name="postfixOfTableName"></param>
        /// <returns></returns>
        public ObjectTableManager HandleSameCellFloatCellValue(ObjectTableManager tableBManager, Func<double, double, double> Func, string postfixOfTableName)
        {
            return HandleSameNumeralCellValue(this, tableBManager, Func, postfixOfTableName);
        }
        /// <summary>
        /// 对两个表相同位置的数据进行计算，并返回新表。
        /// </summary>
        /// <param name="tableAManager"></param>
        /// <param name="tableBManager"></param>
        /// <param name="Func"></param>
        /// <param name="postfixOfTableName"></param>
        /// <returns></returns>
        static public ObjectTableManager HandleSameNumeralCellValue(ObjectTableManager tableAManager, ObjectTableManager tableBManager, Func<double, double, double> Func, string postfixOfTableName)
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach<ObjectTableStorage>(tableAManager, tableA =>
            {
                var tableB = tableBManager.Get(tableA.Name);
                if (tableB != null)
                {
                    var newTableName = tableA.Name;
                    if (!String.IsNullOrWhiteSpace(postfixOfTableName))
                    {
                        newTableName += "_" + postfixOfTableName;
                    }
                    var newTable = tableA.HandleSameNumeralCellValue(tableB, Func, newTableName);
                    dic.TryAdd(newTable.Name, newTable);
                }
            });
            var mwTables = new ObjectTableManager(dic, tableAManager.OutputBufferCount, tableAManager.OutputDirectory);

            return mwTables;
        }
        /// <summary>
        /// 移除指定列为空的行。
        /// </summary>
        /// <param name="basePrn"></param>
        public void RemoveEmptyRowsOf(string basePrn)
        {
            Parallel.ForEach(this, (item) =>
            {
                item.RemoveEmptyRowsOf(basePrn);
            });
        }
        /// <summary>
        /// 移除所有空列
        /// </summary>
        public void RemoveEmptyCols()
        {
            Parallel.ForEach(this, (item) =>
            {
                item.RemoveEmptyCols();
            });
        }

        /// <summary>
        /// 读取返回表集合。移除下划线以后的字符串。
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <param name="nameSplitter"></param>
        /// <returns></returns>
        public static ObjectTableManager Read(string[] inputPathes, string nameSplitter="_")
        {
            new Log(typeof(ObjectTableManager)).Info("准备并行读取 " + inputPathes.Length + " 个文件！");
            DateTime start = DateTime.Now;
            var data = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach(inputPathes, (path) =>
            {
                if (!File.Exists(path)) { return; }

                ObjectTableReader reader = new ObjectTableReader(path);

                var storage = reader.Read();
                var key = Path.GetFileNameWithoutExtension(path);
                var fistDivChar = key.LastIndexOf(nameSplitter);
                var name = key;
                if (fistDivChar != -1)
                {
                    name = name.Substring(0, fistDivChar);
                }
                storage.Name = name;
                data.TryAdd(storage.Name, storage);

            });


            var tableManager = new ObjectTableManager(data);
            tableManager.OutputDirectory = Path.GetDirectoryName(inputPathes[0]);

            var span = DateTime.Now - start;
            new Log(typeof(ObjectTableManager)).Info("并行读取了" +tableManager.Count + " 个文件，耗时：" + span);

            return tableManager;
        }

        /// <summary>
        /// 通过减去指定小数后，四舍五入取整。
        /// </summary>
        /// <param name="fractionTables"></param>
        /// <param name="tableNamePostfix"></param>
        /// <param name="isReplacePrevPosfix"></param>
        /// <returns></returns>
        public ObjectTableManager GetIntByMinusAndRound(ObjectTableManager fractionTables, string tableNamePostfix = "Int", bool isReplacePrevPosfix = false)
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            Parallel.ForEach(this, tableA =>
            {
                var tableB = fractionTables.Get(tableA.Name);
                if (tableB == null) { return ; }
                var table = tableA.GetIntByMinusAndRound(tableB,tableNamePostfix, isReplacePrevPosfix);
                dic.TryAdd(table.Name, table);
            });
            var mwTables = new ObjectTableManager(dic, this.OutputBufferCount, OutputDirectory);
            return mwTables; 
        }

        /// <summary>
        /// 追加列，返回新表
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="appdenColPostfix"></param>
        /// <param name="tableNamePostfix"></param>
        /// <param name="isReplacePrevPosfix"></param>
        /// <returns></returns>
        public ObjectTableManager GetAppendColTables(ObjectTableManager tables, string appdenColPostfix, string tableNamePostfix = "Combined", bool isReplacePrevPosfix = false)
        {
            var newTableManager = new ObjectTableManager(this.OutputDirectory, this.OutputBufferCount, this.Name + "_append_" + tables.Name + "_" + appdenColPostfix);
            int i = -1;
            foreach (var tableA in this) //便利PPP星间单差，一次一颗卫星
            {
                i++;
                var tableB = tables.Get(tableA.Name);
                if (tableB == null) { continue; }

                var newTable = tableA.GetAppendColTable(tableB, appdenColPostfix, tableNamePostfix, isReplacePrevPosfix);

                newTableManager.Add(newTable);
            }
            return newTableManager;
        }
        /// <summary>
        /// 所有表格，最后一行除了检索的数值的平均数,如果该列最后一行没有，则追溯到有的那行。
        /// </summary>
        /// <param name="ignoreRowCountLessThan">忽略行数量少于此的列,不含</param>
        /// <returns></returns>
        public Dictionary<string, double[]> GetAverageValsOfLastRow(int ignoreRowCountLessThan = 10)
        {
            Dictionary<string, double[]> dic = new Dictionary<string, double[]>();
            foreach (var table in this.Data)
            {
                if (table.Value.RowCount == 0 || table.Value.ColCount < 2) { continue; }

                dic[table.Key] = table.Value.GetAverageOfLastValueWithStdDevOfAllCols(ignoreRowCountLessThan);
            }
            return dic;
        } 
        /// <summary>
        ///  以最后数据为依据，超过指定数值的滤掉。
        /// </summary>
        /// <param name="radius"></param>
        public void PipeFilterWithLastValues(double radius)
        {
            foreach (var item in this)
            {
                item.PipleFilterWithLastValues(radius);
            }
        }

        /// <summary>
        /// 获取所有表格的最后数据组成新表
        /// </summary>
        /// <param name="name">表名称</param>
        /// <param name="ignoreRowCountLessThan">忽略行数量少于此的列,不含</param> 
        /// <param name="period"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public ObjectTableStorage GetFractionOfLastValueOfAllCols(string name, int ignoreRowCountLessThan = 10, double period = 1.0, double reference = 0.5)
        {
            var newTable = new ObjectTableStorage(name);
            var PeriodFilterManager = new PeriodPipeFilterManager(period, reference);
            foreach (var table in this)
            {
                var siteName = TableNameHelper.ParseName(table.Name);
                newTable.NewRow();
                newTable.AddItem("Name", siteName);// 第一列为名称

                var lastValues = table.GetLastValueOfAllCols(ignoreRowCountLessThan);
                foreach (var colName in lastValues.Keys) //每个测站对应一行
                {
                    var initVal = lastValues[colName];
                    //结果统一到一个周期区间
                    // lly修改，这个地方直接取每一列的小数部分即可，不用再进行区间的变化
                    //var val = PeriodFilterManager.GetOrCreate(siteName).Filter(initVal);
                    //newTable.AddItem(colName, val);                    
                    newTable.AddItem(colName, initVal);
                }
                newTable.EndRow();
            }

            return newTable;
        }

        /// <summary>
        /// 返回一个非空的表。失败返回null。
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetOne()
        {
            foreach (var item in this)
            {
                if (item.RowCount == 0) { continue; }
                return item;
            }
            return null;
        }
        /// <summary>
        /// 比较获取第一个最小的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetFirstIndexValue<T>() where T : IComparable<T>
        {
            T min = this.First.GetFirstIndexValue<T>();
            foreach (var table in this)
            {
                var other = table.GetFirstIndexValue<T>();
                if (other.CompareTo(min) < 0) { min = other; }
            }
            return min;
        }
        /// <summary>
        /// 比较获取第一个最大的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetLastIndexValue<T>() where T : IComparable<T>
        {
            T max = this.First.GetLastIndexValue<T>();
            foreach (var table in this)
            {
                var other = table.GetLastIndexValue<T>();
                if (other.CompareTo(max) > 0) { max = other; }
            }
            return max;
        }
        /// <summary>
        /// 替换表名
        /// </summary>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        public void ReplaceTableName(string oldStr, string newStr)
        {
            var newDic = new Dictionary<string, ObjectTableStorage>();
            foreach (var item in this)
            {
                item.Name = item.Name.Replace(oldStr, newStr);
                newDic.Add(item.Name, item);
            } 
            this.SetData(newDic);
        }
        /// <summary>
        /// 移除所有数据列的前面的数据。从有数据时才开始计算。
        /// </summary>
        /// <param name="count"></param>
        public void EmptyFrontValueOfCols(int count)
        {
            Parallel.ForEach(this, (item) =>
          {
              item.EmptyFrontValueOfCols(count);
          });
        }
        /// <summary>
        /// 获取其中一段
        /// </summary>
        /// <param name="fromIndexValue"></param>
        /// <param name="toIndexValue"></param>
        /// <returns></returns>
        public ObjectTableManager GetSub(object fromIndexValue, object toIndexValue)
        {
            var dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            var namePostfix = fromIndexValue + "->" + toIndexValue;
            Parallel.ForEach<string>(this.Keys, item =>
            {
                var table = this[item].GetSub(fromIndexValue, toIndexValue, namePostfix, false);
                if (table != null)
                {
                    dic.TryAdd(table.Name, table);
                }
            });
            var mwTables = this.CreateNew(dic, this.Name + "子表 ");
            return mwTables;
        }
        /// <summary>
        /// 移除行数量少于指定的表,移除列数量少于指定的表
        /// </summary>
        /// <param name="minRowCount"></param>
        /// <param name="minColCount"></param>
        /// <param name="Condition"></param>
        public void RemoveTableDataCountLessThan(int minRowCount, int minColCount, Func<double, bool> Condition =null)
        {
            if (Condition == null) { Condition = m => true; }
            foreach (var item in this.Keys)
            {
                var rowCount = this[item].RowCount;
                if (rowCount < minRowCount)
                {
                    this.Remove(item);
                    log.Debug("移除了表 " + item + " 行数 " + rowCount);
                }
                else
                {
                    this[item].RemoveRowCountLessThan(minRowCount, Condition);//每个表的列少于指定行则移除。
                    var colCount = this[item].ColCount;
                    if (colCount < minColCount)
                    {
                        this.Remove(item);
                        log.Debug("移除了表 " + item + " 列数 " + colCount);
                    }
                }
            }
        }
        /// <summary>
        /// 把检索列填满
        /// </summary>
        public void FillIndexes()
        {
            foreach (var item in this)
            {
                item.FillIndexes();
            }
        }
        /// <summary>
        /// 移除每列开始部分的数据
        /// </summary>
        /// <param name="count"></param>
        /// <param name="maxBreakCountInSegment">忽略断裂数</param>
        public void RemoveStartRowOfEachSegment(int count, int maxBreakCountInSegment= 2)
        {
            foreach (var item in this)
            {
                item.RemoveStartRowOfEachSegment(count, maxBreakCountInSegment);
            }

        }
        /// <summary>
        /// 移除列，最后值偏离计算中心
        /// </summary>
        /// <param name="offCenter"></param>
        /// <param name="loop"></param> 
        public void RemoveColWithLastValOffCenter(double offCenter, bool loop = true)
        {
            foreach (var item in this)
            {
                if (item.ColCount <= 1) { continue; }
                item.RemoveColWithLastValOffCenter(offCenter, loop);
            }
        }
        /// <summary>
        /// 同步表，删除多余列
        /// </summary>
        /// <param name="tableObjectManager"></param>
        public void SynchronizeTable(ObjectTableManager tableObjectManager)
        {
            List<string> toremoveTable = new List<string>();
            foreach (var item in this)
            {
                var table = (tableObjectManager.Get(item.Name));
                if(table != null)
                {
                    item.SynchronizeCol(table);
                }
                else
                {
                    toremoveTable.Add(item.Name);
                }
            }
            if (toremoveTable.Count > 0)
            {
                this.Remove(toremoveTable);
                log.Debug(this.Name + " 与 " + tableObjectManager.Name + " 同步移除了 " + toremoveTable.Count + " 个表");
            }
        }
        /// <summary>
        /// 移除列数量小于指定数的表。
        /// </summary>
        /// <param name="minColCount">最小列数量（不含）</param>
        /// <returns></returns>
        public int RemoveTableOfColCountLessThan(int minColCount)
        {
            int count = 0;
            foreach (var item in Keys)
            {
                if (Get(item).ColCount < minColCount)
                {
                    Remove(item); count++;
                }
            }
            return count;
        }
        /// <summary>
        /// 移除列中不平坦的数据，只保留出现率最多的数，即所有列的行的内容应该为相同的数据。
        /// </summary>
        public void RemoveMinorityValueOfEachCol()  {   ParallelHandleTable(new Action<ObjectTableStorage>(table => table.RemoveMinorityValueOfEachCol()));   }
        /// <summary>
        /// 并行处理表格。
        /// </summary>
        /// <param name="action"></param>
        public void ParallelHandleTable(Action<ObjectTableStorage> action)  {   Parallel.ForEach(this, action);   }
        /// <summary>
        /// 统计所有数字表格数量。
        /// </summary>
        /// <returns></returns>
        public int GetCountOfNumeralCell()
        {
            int count = 0;
            foreach (var item in this )
            {
              count += item.GetNumeralValueCount();
            } 

             //ParallelHandleTable(new Action<TableObjectStorage>(table => table.HandleNumeralCell((a,b,c)=>{ count ++; }))); 
            return count;
        }
        static object locker = new object();
        /// <summary>
        /// 如果内存中已经有，则直接返回，如果磁盘有，则读取，否则创建一个新表。
        /// 表添加到管理器中。
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ObjectTableStorage GetOrReadOrCreateTableAndAppendAsync(string tableName)
        {
            lock (locker)
            {
                if (this.Contains(tableName)) { return this[tableName]; }

                ObjectTableStorage product = null;
                string dcbPath = Path.Combine(this.OutputDirectory, tableName + TableExtention);
                if (File.Exists(dcbPath)) { using (var reader = new ObjectTableReader(dcbPath)) { product = reader.Read(); } }
                else { product = new ObjectTableStorage(tableName); }

                this.Add(product);

                return product;
            }
        }
        /// <summary>
        /// 如果磁盘有，则读取，否则创建一个新表。
        /// 表不添加到管理器中。
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ObjectTableStorage ReadOrCreateTable(string tableName)
        {
            ObjectTableStorage product = null;
            string dcbPath = Path.Combine(this.OutputDirectory, tableName + TableExtention);
            if (File.Exists(dcbPath)) { using (var reader = new ObjectTableReader(dcbPath)) { product = reader.Read(); } }
            else { product = new ObjectTableStorage(tableName); }
            return product;
        }


    }
    /// <summary>
    /// 表名称帮助器
    /// </summary>
    public class TableNameHelper
    {
        /// <summary>
        /// 表名称分割
        /// </summary>
        static public string TableSpliter = "_";

        /// <summary>
        /// 两个名称的前缀是否相等。
        /// </summary>
        /// <param name="nameA"></param>
        /// <param name="nameB"></param>
        /// <param name="StringComparison"></param>
        /// <returns></returns>
        public static bool IsPrefixEquals(string nameA, string nameB, StringComparison StringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            var a = ParseName(nameA);
            var b = ParseName(nameB);
            return a.Equals(b, StringComparison);
        }

        /// <summary>
        /// 解析名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isTrimSpliter"></param>
        /// <returns></returns>
        public static string ParseName(string name, bool isTrimSpliter = true)
        {
            if (!isTrimSpliter) { return name; }

            int indexOfSpliter = name.IndexOf(TableSpliter);
            if (indexOfSpliter == -1) { return name; }
            return name.Substring(0, indexOfSpliter);
        }

        /// <summary>
        /// 构建新名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="namePostfix"></param>
        /// <param name="isReplacePrevPosfix"></param>
        /// <returns></returns>
        public static string BuildName(string name, string namePostfix, bool isReplacePrevPosfix = true)
        {
            var oldName = name;
            if (isReplacePrevPosfix)
            {
              oldName =  ParseName(name);
            }
            if (!String.IsNullOrWhiteSpace(namePostfix))
            {
                return oldName + TableSpliter + namePostfix;
            }
            return oldName; 
        }


    }
}