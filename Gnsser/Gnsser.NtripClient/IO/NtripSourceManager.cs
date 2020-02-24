//2016.10.15, czs & double, create in hongqing, Ntrip配置文件读取器
//2016.12.10, czs, create in hongqing, Ntrp 管理器增加数据挂载点的列表


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Geo;
using System.IO;


namespace Gnsser.Ntrip
{



    //2016.10.15, czs & double, create in hongqing, Ntrip数据流播发服务器。
    /// <summary>
    /// Ntrip数据流播发服务器。
    /// </summary>
    public class NtripCaster : Named
    {

        public NtripCaster()
        {

        }

        /// <summary>
        /// 版本
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 数据源表路径。
        /// </summary>
        public string SourceTablePath { get; set; }
        /// <summary>
        /// 获取属性字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Object> GetAttibuteDic()
        {
            var dic = new Dictionary<string, Object>();
            dic.Add("Name", Name);
            dic.Add("Host", Host);
            dic.Add("Port", Port);
            dic.Add("User", UserName);
            dic.Add("Pass", Password);
            dic.Add("Version", Version);
            dic.Add("SourceTable", Name);
            return dic;
        }
    }


    /// <summary>
    /// Ntrip配置文件读取器
    /// </summary>
    public class NtripSourceManager
    {
        public NtripSourceManager(string settingPath)
        {
            this.settingPath = settingPath;
            this.NtripCasters = this.GetNtripCasters();
            this.CurrentCasterSites = this.GetCasterSites(); 
            this.DownloadSourceTables();
            this.SourceTables = LoadSourceTables();
            this.NtripSourceTables = LoadNtripSourceTables();
        }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        string settingPath { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public Dictionary<string, NtripCaster> NtripCasters { get; set; }
        /// <summary>
        /// 数据源测站
        /// </summary>
       public  Dictionary<string, List<string>> CurrentCasterSites { get; set; }
       public Dictionary<string, SourceTable> SourceTables { get; set; }
       public Dictionary<string, NtripSourceTable> NtripSourceTables { get; set; }

        /// <summary>
        /// 当前的数据挂载点
        /// </summary>
        /// <returns></returns>
       public List<NtripStream> GetCurretnNtripStreams()
       {
           List<NtripStream> list = new List<NtripStream>();

           foreach (var item in CurrentCasterSites)
           {
               if (!NtripSourceTables.ContainsKey(item.Key)) { continue; }
               
               var table = NtripSourceTables[item.Key];
               var streams = table.GetNtripStreams();
               foreach (var kv in item.Value)
               {
                   if (streams.ContainsKey(kv))
                   {
                       list.Add(streams[kv]);
                   }
               }
           }


           return list;
       }


        /// <summary>
        /// 读取数据源表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, SourceTable> LoadSourceTables()
        {
            var tables = new Dictionary<string, SourceTable>();
            foreach (var item in this.NtripCasters.Values)
            {
                var local = Path.Combine(Setting.ApplicationDirectory, item.SourceTablePath);
                var localBk = local + ".bak";
                SourceTable table = null;
                if (File.Exists(local))
                {
                    table = SourceTable.Load(local);
                }
                else if (File.Exists(localBk))
                {
                    table = SourceTable.Load(localBk);
                }

                if (table != null)
                {
                    tables.Add(item.Name, table);
                }
            }
            return tables;
        }     
        
        /// <summary>
        /// 读取数据源表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, NtripSourceTable>  LoadNtripSourceTables()
        {
            var tables = new Dictionary<string, NtripSourceTable>();
            foreach (var item in this.NtripCasters.Values)
            {
                var local = Path.Combine(Setting.ApplicationDirectory, item.SourceTablePath);
                var table = NtripSourceTable.Load(local);
                tables.Add(item.Name, table);
            }
            return tables;
        }

        /// <summary>
        /// 下载或更新数据源表
        /// </summary>
        public void DownloadSourceTables()
        {
            foreach (var item in this.NtripCasters.Values)
            {
                bool isDown = true;
                var host = item.Host;
                if (!host.ToLower().Contains("http://")) { host = "http://" + host; }
                var url = host + ":" + item.Port;
                var local = Path.Combine(Setting.ApplicationDirectory, item.SourceTablePath);
                var localBak = local + ".bak";
                if (File.Exists(local))
                {
                    FileInfo info = new FileInfo(local);
                    //如果超过30天，则更新表
                    if (info.CreationTime < DateTime.Now - TimeSpan.FromDays(30))
                    {
                        if (File.Exists(localBak)) { File.Delete(localBak); }

                        File.Move(local, localBak);

                        isDown = true;
                    }
                    else { isDown = false; }
                }
                if (isDown)
                {
                    Geo.Utils.NetUtil.Download(url, local);    
                }            
            }
        }
        #region  IO
        #region 读取
        /// <summary>
        /// 读取配置文件
        /// </summary>
        public Dictionary<string, List<string>> GetCasterSites()
        {
            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>(); 
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(settingPath);
            XmlNode node = xmlDoc.SelectSingleNode("./Setting/Current/SelectedCasters");
            foreach (XmlNode casterNode in node.ChildNodes)
            {
                var casterName = casterNode.SelectSingleNode("Name").InnerText.Trim();
                var sitesNode= casterNode.SelectSingleNode("Sites");
                List<string> sites = new List<string>();
                foreach (XmlNode siteNode in sitesNode.ChildNodes)
                {
                    sites.Add(siteNode.SelectSingleNode("Name").InnerText.Trim());
                }
                data.Add(casterName, sites);
            }
            return data;

        }


           /// <summary>
        /// 读取配置文件
        /// </summary>
        public  Dictionary<string, NtripCaster> GetNtripCasters()
        {
            //try
            //{
            Dictionary<string, NtripCaster> data = new Dictionary<string, NtripCaster>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(settingPath);
            XmlNode node = xmlDoc.SelectSingleNode("./Setting/NtripStream/Casters");
            foreach (XmlNode casterNode in node.ChildNodes)
            {
                NtripCaster caster = new NtripCaster()
                {
                     Name = casterNode.SelectSingleNode("Name").InnerText.Trim(),
                     Host = casterNode.SelectSingleNode("Host").InnerText.Trim(),
                     Port = Int32.Parse(casterNode.SelectSingleNode("Port").InnerText.Trim()),
                     UserName = casterNode.SelectSingleNode("User").InnerText.Trim(),
                     Password = casterNode.SelectSingleNode("Pass").InnerText.Trim(),
                     SourceTablePath = casterNode.SelectSingleNode("SourceTable").InnerText.Trim(),
                     Version = casterNode.SelectSingleNode("Version").InnerText.Trim()
                };
                data.Add(caster.Name, caster);
            }
            return data;

        }

        #endregion

        #region 写入

       

        /// <summary>
        /// 读取配置文件
        /// </summary>
        public void SaveCurrentCasterSites()
        {
            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
            XmlDocument doc = new XmlDocument();
            doc.Load(settingPath);
            XmlNode node = doc.SelectSingleNode("./Setting/Current/SelectedCasters");
            //node.ChildNodes.a

            node.RemoveAll();
            foreach (var item in CurrentCasterSites)
            {
                var caster = Geo.Utils.XmlUtil.CreateElementWithChild(doc, "Caster", "Name", item.Key); 
                node.AppendChild(caster);

                var sites = Geo.Utils.XmlUtil.CreateElement(doc, "Sites");
                caster.AppendChild(sites);

                foreach (var siteName in item.Value)
                { 
                    var siteNode = Geo.Utils.XmlUtil.CreateElementWithChild(doc, "Site", "Name", siteName);
                    sites.AppendChild(siteNode);
                }     
            }

            doc.Save(settingPath);   
        } 
        /// <summary>
        /// 读取配置文件
        /// </summary>
        public void  SaveNtripCasters()
        {
            Dictionary<string, NtripCaster> data = new Dictionary<string, NtripCaster>();
            XmlDocument doc = new XmlDocument();
            doc.Load(settingPath);
            XmlNode node = doc.SelectSingleNode("./Setting/NtripStream/Casters");
            node.RemoveAll();

            foreach (var item in NtripCasters)
            {
                var dic = item.Value.GetAttibuteDic();
                var caster = Geo.Utils.XmlUtil.CreateElementWithChild(doc, "Caster", dic); 
                node.AppendChild(caster);

            }
            doc.Save(settingPath);    
        }




        #endregion

        #endregion
    }
} 