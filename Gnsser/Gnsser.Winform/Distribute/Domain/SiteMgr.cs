//2015.11.02, czs, create in hongqing,  基于Gnsser操作任务的分布式计算   

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
//using System.Windows.Forms;
using Gnsser.Interoperation.Bernese;
using Geo.Times;
using Geo;
using Gnsser.Api;
using Geo.IO;

namespace Gnsser.Winform
{
    /// <summary>
    /// 任务管理器。主要负责其存储。
    /// </summary>
    public class SiteMgr : SingleFileManager
    {
        //核心存储
        List<Site> data;//= new List<Site>();
        

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="FilePath"></param>
        public SiteMgr(string FilePath):base(FilePath)
        {

        }

        /// <summary>
        /// 测站集合
        /// </summary>
        public List<Site> Sites { get { return data; } }

        /// <summary>
        /// 加载
        /// </summary>
        public override void Load()
        {
            data = new LineFileReader<Site>(this.FilePath).ReadAll();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void Save()
        {
          var writer = new LineFileWriter<Site>(this.FilePath);//.Write()
          foreach (var item in this.data)
          {
              writer.Write(item);
          }
          writer.Close();
        }

        internal List<string> GetSiteNames()
        {
            List<string> names = new List<string>();
            foreach (var item in data)
            {
                names.Add(item.Name);
            }
            return names;
        }
    }
}
