//2015.01.18, czs, create in namu,  配置文件内容
//2015.09.26, czs, edit in hongqing, BaseDictionary<string, TypedConfigItem<TValue>>
//2016.02.10, czs, edit in hongqing,  增加默认数据，应对变化
//2018.03.19, czs, edti in hmx, 与分组配置文件合并，提取抽象类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Common;
using System.IO;
using Geo.IO;

namespace Geo.IO
{
    /// <summary>
    /// 配置文件内容.
    /// 通常以#为注释，以=为赋值符号，以行为单位的配置文件。
    /// </summary>
    public abstract class TypedConfig<TItem,TKey, TValue> : BaseDictionary<TKey, TItem>
        where TItem : TypedConfigItem<TKey, TValue>
        //  where TKey : IComparable<TKey>
    {
        protected const string DataSource = "DataSource";

        /// <summary>
        ///配置文件内容
        /// </summary> 
        public TypedConfig():base("配置文件")
        { 
            this.Comments = new List<string>(); 

            IsChangSaved = true;
        }

        /// <summary>
        /// 采用已有列表初始化
        /// </summary>
        /// <param name="ConfigItems"></param>
        /// <param name="Comments"></param>
        public TypedConfig(IDictionary<TKey, TItem> ConfigItems, List<String> Comments) :base(ConfigItems)
        { 
            this.Comments = Comments;
            IsChangSaved = true;
        }

        #region 核心存储变量 
        /// <summary>
        /// 整个文档的注释
        /// </summary>
        public List<String> Comments { get; set; }
        #endregion

        /// <summary>
        /// 指示是否保存了改变。
        /// </summary>
        public bool IsChangSaved{ get; set; }
        /// <summary>
        /// 添加全文注释
        /// </summary>
        /// <param name="comment">注释内容</param>
        public void AddComment(string comment)
        {
            if (!String.IsNullOrWhiteSpace(comment))
            { this.Comments.Add(comment); IsChangSaved = false; }
        } 

        /// <summary>
        /// 获取指定变量的注释，如果没有注释，则返回空 String.Empty .
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public string GetComment(TKey name)
        {
            if (this.Contains(name)) { return this.Data[name].Comment; }
            return String.Empty;
        }
        /// <summary>
        /// 直接以对象形式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="comment"></param>
        public void SetObj(TKey name, Object val, string group, string comment = "")
        {
            TValue valu = Parse(val);
            Set(name, valu, group, comment);
        }

        /// <summary>
        /// 设置一个变量的值。如果存在则只改变其值，如果不存在则添加。
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="val">数值</param>
        /// <param name="group">分组</param>
        /// <param name="comment">参数非空则添加，原来的将被替换</param>
        public void Set(TKey name, TValue val, string group = "Default",  string comment = null)
        {
            var item = GetOrCreate(name);
            item.Value = val;
            item.Group = group;
            item.Comment = comment ?? "Create in " + Geo.Utils.DateTimeUtil.GetFormatedDateTimeNow();

            //base.Set(name, val);

            //if (this.Contains(name))
            //{ 
            //    var item = Get(name);
            //    item.Value = val;
            //    if (!String.IsNullOrWhiteSpace(comment))
            //    {
            //        item.Comment = comment;
            //    }
            //}
            //else
            //{
            //    this.Data[name] = CreateItem(name, val, "Default", comment);
            //}
            IsChangSaved = false;
        }

        public override TItem Create(TKey key)
        {
            return CreateItem(key, default(TValue));
        }

        protected abstract TItem CreateItem(TKey Name, TValue Value, string group = "Default", string Comment = "");

        /// <summary>
        /// 设置变量值，如果已经有了，则直接覆盖。
        /// </summary>
        /// <param name="ConfigItem">项目</param>
        public void Set(TItem ConfigItem)
        {
            base.Set(ConfigItem.Name, ConfigItem);
            //this.data[ConfigItem.Name] = ;
            IsChangSaved = false;
        }
        /// <summary>
        /// 获取字符串数组
        /// </summary>
        /// <param name="key"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public string[] GetArray(TKey key, string spliter = ",")
        {
            var str = Get(key);
            if (str == null) return new string[] { };
            return Geo.Utils.StringUtil.Split(str.Value.ToString(), ',');
        }
        /// <summary>
        /// 保存字符串数组
        /// </summary>
        /// <param name="key"></param>
        /// <param name="array"></param>
        /// <param name="spliter"></param>
        public void SetArray(TKey key, string [] array, string group="Default", string spliter = ",")
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in array)
            {
                if(i !=0){sb.Append(spliter); }
                sb.Append(item);
                i++;
            }

            SetObj(key, sb.ToString(), group);
        }



        #region 变量类型转换
        /// <summary>
        /// 直接返回字符串，如果没有，则初始化一个，设置值为空。而不会报错。
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns> 
        public string GetString(TKey name, object defaultValue=null)
        {
           var item =  GetOrCreate(name);
            if(item.Value == null || String.IsNullOrWhiteSpace( item.Value + ""))
            {
                TValue val = default(TValue);
                if (defaultValue != null)
                {
                    val = Parse(defaultValue);
                }
                item.Value = val;
            } 

            return item.Value + "";
        }
        /// <summary>
        /// 解析对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected virtual TValue Parse(object str)
        {
            if(str is TValue) { return (TValue)str; }
            return ParseString(str.ToString());
        }
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected abstract TValue ParseString(string str);

        /// <summary>
        /// 直接返回时间
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public DateTime GetDateTime(TKey name)
        {
            return DateTime.Parse(GetString(name, DateTime.Now));
        }

        /// <summary>
        /// 直接返回数值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public Double GetDouble(TKey name, double defaultValue = 0)
        { 
            return Double.Parse(GetString(name, defaultValue));
        }
        /// <summary>
        /// 直接返回数值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public int GetInt(TKey name, int defaultVal = 0)
        {
            return Int32.Parse(GetString(name, defaultVal));
        }
        /// <summary>
        /// 直接返回长整形
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public long GetLong(TKey name)
        {
            return Int64.Parse(GetString(name, 0));
        }
        /// <summary>
        /// 直接返回数值,如果没有则返回默认值，并设置配置文件
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public Boolean GetBool(TKey name, bool defaultVal = true)
        {
            var val = GetString(name, defaultVal);
            return Boolean.Parse(val);
        } 
        #endregion

        #region 非公开函数
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        protected string GetVal(TKey settingName, string defaultVal = "")
        {
            return this.GetString(settingName, defaultVal);
        }
        /// <summary>
        /// 设值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="settingName"></param>
        protected void SetVal(TValue value, TKey settingName, string group = "Default")
        {
            this.SetObj(settingName, value, group);
        } 
        /// <summary>
        /// 设值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="settingName"></param>
        protected void SetVal(Object value, TKey settingName, string group = "Default")
        {
            this.SetObj(settingName, value.ToString(), group);
        }
        /// <summary>
        /// 返回路径。相对于当前运行程序目录的路径。
        /// </summary>
        /// <param name="settingName">名称</param>
        /// <param name="defaultFileName">默认值</param>
        /// <returns></returns>
        protected string GetPath(TKey settingName, string defaultFileName = "DefaultFileName.txt")
        {
            String path = GetString(settingName, defaultFileName);
            if (string.IsNullOrWhiteSpace(path)) { path = defaultFileName; }

            return GeoLocalPath(path);
        }
        /// <summary>
        /// 把相对路径通过 BaseDirectory 拼接为本地路径。
        /// 如果不是相对路径，则直接返回。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GeoLocalPath(string path)
        {
            //是否包含了硬盘符号
            if (path.Contains(@":")) { return path; }
            //否则为相对路径。
            path = Path.Combine(BaseDirectory, path);
            if (Geo.Utils.FileUtil.IsDirectory(path))
            {
                Geo.Utils.FileUtil.CheckOrCreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 主目录，可以手动设置 
        /// </summary>
        static public string baseDirectory = null;
        /// <summary>
        /// 程序集启动目录
        /// </summary>
        static public string BaseDirectory
        {
            get
            {
                if (String.IsNullOrWhiteSpace(baseDirectory))
                    return AppDomain.CurrentDomain.BaseDirectory;
                return baseDirectory;
            }
            set
            {
                baseDirectory = value;
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        protected string GetConfigValue(TKey settingName)
        {
            return this.GetString(settingName);
        }
        /// <summary>
        /// 相对于当前运行程序目录的路径。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="settingName"></param>
        protected void SetPath(string value, TKey settingName, string group= DataSource)
        {
            string path = value.Replace(BaseDirectory, "");
            SetConfigVlue(settingName, path, group);
        }
        /// <summary>
        /// 设置 app.config 的 appSettings 项目
        /// </summary>
        /// <param name="settingName">名称</param>
        /// <param name="val">值</param>
        protected void SetConfigVlue(TKey settingName, string val, string group)
        {
            this.SetObj(settingName, val, group);
        }
        #endregion

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in this.GetData())
            {
              //  if (i != 0) sb.Append(";"); 
                sb.Append(item.Value.Name + "\t=\t" + item.Value.Value  );
                if (item.Value.Comment != null)
                {
                    sb.Append("\t # ");
                    sb.Append(item.Value.Comment); 
                }
                sb.AppendLine();

                i++;
            }
            return sb.ToString();
        }

    }
     
}
