//2015.09.28, czs, create in xi'an hongqing, Gnsser文件读取通用接口
//2015.11.07, czs, create in D5181 达州到成都动车，提取对象读写通用接口

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Data;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// Xml读取通用接口。本类采用流式读取，实现了枚举接口 IEnumerable 和 IEnumerator。
    /// </summary>
    public abstract class EntityFileReader<T> : Geo.IO.IEntityFileReader<T> 
        where T : IOrderedProperty
    {
        #region 构造函数与初始化
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EntityFileReader()
        {
            this.CurrentIndex = -1;
            EnumCount = int.MaxValue / 2;
        }
        Log log = new Log(typeof(EntityFileReader<T>));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="metaFilePathOrDirectory">元数据文件或者元数据文件存放的目录。若不指定，则自动寻找文件目录下的元数据，若没有则调用默认的元数据，若还没有则报错。</param>
        public EntityFileReader(string filePath, string metaFilePathOrDirectory = null)
        { 
            Init(filePath, metaFilePathOrDirectory);
            Name = InputPath;
        } 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="Gmetadata"></param>
        public EntityFileReader(string filePath, Gmetadata Gmetadata)
        {
            Name = InputPath;
            Init(filePath, Gmetadata);
        }

        /// <summary>
        /// 初始化，等同于构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public void Init(string filePath, Gmetadata Gmetadata)
        {
            this.InputPath = filePath;
            this.Metadata = Gmetadata;
            EnumCount = int.MaxValue / 2;

            this.BaseDirectory = Path.GetDirectoryName(filePath);
            if (Gmetadata == null)
            {
                Init(filePath, "");
            }
            else { Init(Gmetadata); }
            
        }
        /// <summary>
        /// 初始化，等同于构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePathOrDirectory"></param>
        /// <returns></returns>
        public void Init(string filePath, string metaFilePathOrDirectory)
        {
            this.InputPath = filePath;
            this.BaseDirectory = Path.GetDirectoryName(filePath);
            var extention = Path.GetExtension(filePath);
            EnumCount = int.MaxValue / 2;

            if (String.IsNullOrWhiteSpace(metaFilePathOrDirectory))
            {
                //默认的元数据位置，按照优先级挨个检查
                List<string> metaFilePathes = new List<string>();
                //1.先检查当前文件夹
                var dir = Path.GetDirectoryName(filePath);
                var metaName = extention.Replace(".", "") + ".gmeta";
                var path = Path.Combine(dir, metaName);
                metaFilePathes.Add(path);
                //2.检查程序默认位置
                dir = System.AppDomain.CurrentDomain.BaseDirectory ;                 
                var path2 = Path.Combine(dir,"Data\\Gmeta", metaName);
                metaFilePathes.Add(path2);

                foreach (var item in metaFilePathes)
                {
                    if (File.Exists(item))
                    {
                        this.Metadata = new GmetaReader(item).Read();
                        log.Info("采用 Gmeta 文件:" + item);
                        break;
                    }
                }
            }
            if( this.Metadata ==null)//读取程序默认的元数据
            {
                this.Metadata = GetDefaultMetadata();
                log.Info("没有提供外部 Gmeta 文件，使用系统默认格式。" + filePath);
            }

            Init(Metadata); 
        }

        /// <summary>
        /// 初始化工作
        /// </summary>
        /// <param name="Gmetadata"></param>
        protected virtual void Init(Gmetadata Gmetadata)
        {
            this.ItemSpliters = Gmetadata.ItemSplliter;
            this.IsPropertyUnitChanged = this.Metadata.IsPropertyUnitChanged;
            this.DestPropertyUnits = Metadata.DestPropertyUnits;
            this.PropertyUnits = Metadata.PropertyUnits;
            this.StartIndex = Metadata.StartRowIndex;

            PropertyIndexes = new Dictionary<string, int>();
            int i = 0;
            foreach (var name in this.Metadata.PropertyNames)
            {
                PropertyIndexes.Add(name.Trim(), i++);
            }
            //采用默认序列
            if (PropertyIndexes.Count == 0)
            {
                var defaultObj = Activator.CreateInstance<T>();// typeof(TLineClass);

                var names = defaultObj.OrderedProperties;
                this.Metadata.PropertyNames = names.ToArray();
                i = 0;
                foreach (var name in this.Metadata.PropertyNames)
                {
                    PropertyIndexes.Add(name.Trim(), i++);
                }  
            }

            InitStreamReader();
        }
        /// <summary>
        /// 设置数据流
        /// </summary>
        protected virtual void InitStreamReader()
        {
            this.EnumCount = Int32.MaxValue / 2;
            this.CurrentIndex = -1;
        }
       #endregion

        #region 属性
        public bool IsCancel { get; set; }
        /// <summary>
        /// 当前编号，从 0 开始。只有读取了有效内容才递增。
        /// </summary>
        public int CurrentIndex { get; set; }
        /// <summary>
        /// 起始编号，从0开始。
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// 遍历数量，默认为最大值的一半。
        /// </summary>
        public int EnumCount { get; set; }
        /// <summary>
        /// 最大的循环编号
        /// </summary>
        public int MaxEnumIndex { get { return StartIndex + EnumCount; } }
        /// <summary>
        /// 设置遍历数量
        /// </summary>
        /// <param name="StartIndex"></param>
        /// <param name="EnumCount"></param>
        public void SetEnumIndex(int StartIndex, int EnumCount) { this.StartIndex = StartIndex; this.EnumCount = EnumCount; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性单位是否需要转换
        /// </summary>
        public bool IsPropertyUnitChanged { get; set; }

        public Dictionary<string, Unit> PropertyUnits { get; set; }

        public Dictionary<string, Unit> DestPropertyUnits { get; set; } 

        /// <summary>
        /// 参数类型
        /// </summary>
        public Type EntityType { get { return typeof(T); } }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string InputPath { get; set; }

        /// <summary>
        /// 元数据，描述数据的数据。
        /// 应该包含 Name X Y Z
        /// </summary>
        public Gmetadata Metadata { get; set; }
        /// <summary>
        /// 分隔符
        /// </summary>
        public string[] ItemSpliters { get; set; }
        /// <summary>
        /// 属性名称对应的编号
        /// </summary>
        public Dictionary<String, int> PropertyIndexes { get; set; }
        /// <summary>
        /// 基础目录，如果是相对路径，则按照此目录拼接。
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// 上一个全参数行。用于简化的数据文件。后续的就不用再填充相同数据。
        /// </summary>
        public T PreviousObject { get; set; }
        #endregion

        #region 便利方法，检索
        /// <summary>
        /// 读取指定的对象。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T ReadAt(int index)
        {
            if (CurrentIndex == index) { return Current; }

            if (CurrentIndex < index)
            {
               // for (int i = CurrentIndex; i <= index; i++)
                while(CurrentIndex < index)
                {
                    if (!this.MoveNext()) { throw new ArgumentException("参数可能越界了！CurrentIndex:" + CurrentIndex + ", index:" + index); }
                }
                return Current;
            }
            //CurrentIndex > index
            this.Reset();
            return ReadAt(index);
        }

        /// <summary>
        /// 读取指定的数量
        /// </summary>
        /// <param name="startIndex">起始编号（含）,若越界，则报错。</param>
        /// <param name="count">数量。若数据不足，则只返回已有数量</param>
        /// <returns></returns>
        public List<T> Read(int startIndex, int count = 1)
        {
            List<T> list = new List<T>();
            if (count < 1) { return list; }

            var first = ReadAt(startIndex);
            list.Add(first);
            while (CurrentIndex < startIndex + count - 1)
            {
                if (!this.MoveNext()) { break; }

                list.Add(Current);
            }  
            return list;
        }
        /// <summary>
        /// 返回数据表。算法有待优化。??2015.09.28
        /// </summary>
        /// <returns></returns>
        public DataTable ReadToRawTable()
        {
            DataTable table = new DataTable();
            List<T> list = ReadAll();
            var cols = Geo.Utils.ObjectUtil.GetPropertyNames(typeof(T));
            //头部
            foreach (var item in cols)
            {
                table.Columns.Add(item);
            }
            //表格
            foreach (var item in list)
            {
                DataRow row = table.NewRow();
                int i = 0;
                foreach (var col in cols)
                {
                    //算法有待优化。??2015.09.28
                    var val = Utils.ObjectUtil.GetPropertyValue<T>(item, col);
                    row[i] = val;
                    i++;
                }

                table.Rows.Add(row);
            }
            return table;
        }
        /// <summary>
        /// 一次性读取所有，适合小文件。
        /// </summary>
        /// <returns></returns>
        public List<T> ReadAll()
        {
            this.Reset();

            List<T> list = new List<T>();
            foreach (var item in this)
            {
                list.Add(item);
            }
            return list;
        }
        #endregion

        #region 核心转换 

        /// <summary>
        /// 获取默认的元数据。没有排序。
        /// </summary>
        /// <returns></returns>
        public virtual Gmetadata GetDefaultMetadata()
        {
            Gmetadata data = Gmetadata.NewInstance;
            data.PropertyNames = Activator.CreateInstance<T>().OrderedProperties.ToArray();
            //var names = Utils.ObjectUtil.GetPropertyNames(typeof(TLineClass));

            //foreach (var key in names)
            //{
            //    data.PropertyNames = names.ToArray();
            //}
            return data;
            //   throw new NotImplementedException(Path.GetExtension(FilePath) + " 暂时不支持这种格式，请手动指定格式元数据！");
        }
         
        #endregion

        #region IEnumerator 
        /// <summary>
        /// 当前
        /// </summary>
        public T Current { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName { get { return this.EntityType.Name; } }

        /// <summary>
        /// 根据输入字符串，和数据类型，以及上一个对象数值，获取新的属性值。
        /// 若失败，则返回null。
        /// </summary>
        /// <param name="valString"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual object GetOrParseValueString(string valString, string propertyName)
        {
            var propertyInfo = EntityType.GetProperty(propertyName);
            return ParseString(valString, propertyInfo);
        }
        /// <summary>
        /// 解析字符串为对象
        /// </summary>
        /// <param name="valString"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        protected virtual object ParseString(string valString, System.Reflection.PropertyInfo propertyInfo)
        {
            valString = valString.Trim();
            object newValue = null;
            var propertyType = (propertyInfo.PropertyType);

            if (String.IsNullOrWhiteSpace(valString) && (PreviousObject != null))
            {
                newValue = propertyInfo.GetValue(PreviousObject, null);
            }
            else//赋值，需要判断参数类型
            {
                newValue = Geo.Utils.ObjectUtil.ParseValue(valString, propertyType);
            }
            return newValue;
        }


        /// <summary>
        /// 移动到下一个。
        /// </summary>
        /// <returns></returns>
        public abstract bool MoveNext();



        /// <summary>
        /// 重置。先释放资源，再初始化。
        /// </summary>
        public virtual void Reset()
        {
            CurrentIndex = -1;
            Dispose();
            InitStreamReader();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// 当前对象
        /// </summary>
        object IEnumerator.Current  { get { return this.Current; }  }
        #endregion

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion 
    
    
       // public string InputPath { get; set; }

        public List<T2> ReadToModels<T2>() where T2 : class
        {
            List<T2> list = new List<T2>();
            this.Reset();
            var items = this.ReadAll();
            foreach (var item in items)
            {
                if (item is T2)
                { list.Add(item as T2); }
            }
            return list;
        }
         
    }
}
