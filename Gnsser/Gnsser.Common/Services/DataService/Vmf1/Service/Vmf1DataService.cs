//2015.1.27,lly,VMF1对流层模型
//2015.05.12, czs, edit, 整理

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// 对流层的VMF1模型
    /// </summary>
    public class Vmf1DataService : FileBasedService<List<Vmf1Value>>, IDictionaryClass<string, List<Vmf1Value>>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vfmFilePath"></param>
        public Vmf1DataService(FileOption vfmFilePath)
            : base(vfmFilePath)
        {
            data = new Dictionary<string, List<Vmf1Value>>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vfmFilePath"></param>
        public Vmf1DataService(string  vfmFilePath)
            : base(vfmFilePath)
        {
            data = new Dictionary<string, List<Vmf1Value>>();
        }


        //  string path = "E:\\Code\\Gnsser2015.02.01-CuiY\\Gnsser\\Gnsser.Winform\\bin\\Debug\\Data\\GNSS\\Common\\2013001.vmf1_g";

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public List<Vmf1Value> Get(string stationName, Time date)
        {
            //按周存放
            if (!Contains(stationName))
            {
                TimeBasedFilePathBuilder builder = new TimeBasedFilePathBuilder(this.Option.FilePath);
                var path = builder.Get(date);
                if (File.Exists(path.FilePath))
                {
                    this.Vmf1Value = new Vmf1FileReader(path.FilePath).Read().GetStaInfo(stationName);
                    Set(stationName, Vmf1Value);
                }
                else
                {
                    //采用null，进行标记
                    Geo.IO.Log.GetLog(this).Error("没有Vmf1文件，请下载到，" + path.FilePath);
                    Set(stationName, null);
                }
            }

            this.Vmf1Value = Get(stationName);
            if (this.Vmf1Value == null) return new List<Vmf1Value>();

            return this.Vmf1Value;


            //Vmf1File  vmf1file = null;

            //string keyPrev = "VMF1" + stationName;

            //vmf1file = new Vmf1FileReader(path).Read();

            //satData[keyPrev] = vmf1file;

            //return vmf1file.GetStaInfo(stationName);
        }

        List<Vmf1Value> Vmf1Value = null;

        Dictionary<string, List<Vmf1Value>> data;

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Add(string key, List<Vmf1Value> val)
        {
            data.Add(key, val);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Set(string key, List<Vmf1Value> val)
        {
            data[key] = val;
        }
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return data.ContainsKey(key);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<Vmf1Value> Get(string key)
        {
            return data[key];
        }
        /// <summary>
        /// 移除一个
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (data.ContainsKey(key)) data.Remove(key);
        }
        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<List<Vmf1Value>> GetEnumerator()
        {
            return data.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return data.Values.GetEnumerator();
        }

        /// <summary>
        /// 数量。
        /// </summary>
        public int Count { get { return data.Count; } }
        /// <summary>
        /// 检索器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<Vmf1Value> this[string key]
        {
            get
            {
                return data[key];
            }
            set
            {

                data[key] = value;
            }
        }


        public void Clear()
        {
            data.Clear();
        }

        public void Dispose()
        {
            Clear();
        }

        public List<string> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public List<List<Vmf1Value>> Values
        {
            get { throw new NotImplementedException(); }
        }


        public List<Vmf1Value> GetOrCreate(string key)
        {
            throw new NotImplementedException();
        }
    }
}
