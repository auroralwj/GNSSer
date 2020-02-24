//2013.04, czs, create, 基于Bernese的分布式计算
//2015.11.02, czs, edit in K998  成都到西安南列车, 基于Gnsser操作任务的分布式计算   

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using Gnsser.Interoperation.Bernese;
using Geo.Times;
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 任务管理器。主要负责其存储。
    /// </summary>
    public class TaskMgr : SingleFileManager
    {
        //核心存储
        XmlDocument doc = new XmlDocument();

        /// <summary>
        ///默认构造函数
        /// </summary>
        public TaskMgr()
            : this(Setting.GnsserConfig.TaskFilePath, Setting.GnsserConfig.SiteFilePath)
        { 
        }

        /// <summary>
        /// 任务文件路径
        /// </summary>
        /// <param name="taskFilePath"></param>
        public TaskMgr(string taskFilePath, string SiteFilePath) : base(taskFilePath)
        {
            this.siteManager = new SiteMgr(SiteFilePath);
        }

        SiteMgr siteManager;

        /// <summary>
        /// 从文件加载
        /// </summary>
        public override void Load()
        {
            if (!File.Exists(this.FilePath))
            {  //create one ；
                Geo.Utils.FileUtil.CheckOrCreateFile(this.FilePath, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Tasks></Tasks>");
                // this.Save();
            }
            doc.Load(FilePath);
         }         

        /// <summary>
        /// 批量添加任务
        /// </summary>
        /// <param name="tasks"></param>
        public void AddRange(List<Task> tasks)
        {
            foreach (var item in tasks)
            {
                Add(item);
            }
        }
        
        /// <summary>
        /// 添加任务到文件。
        /// </summary>
        /// <param name="node"></param>
        public void Add(Task node)
        {
            XmlElement e = doc.CreateElement("Task");
            XmlElement nid = doc.CreateElement("Id");
            nid.InnerText = node.Id + "";
            e.AppendChild(nid);
            XmlElement nName = doc.CreateElement("Name");
            nName.InnerText = node.Name;
            e.AppendChild(nName);
            XmlElement nNumber = doc.CreateElement("Campaign");
            nNumber.InnerText = node.Campaign.ToString();
            e.AppendChild(nNumber);
            XmlElement eIp = doc.CreateElement("OperationName");
            eIp.InnerText = node.OperationName.ToString();
            e.AppendChild(eIp); 
            XmlElement ePort = doc.CreateElement("Time");
            ePort.InnerText = node.Time.ToString();
            e.AppendChild(ePort);
            XmlElement f = doc.CreateElement("ResultFtp");
            f.InnerText = node.ResultFtp.ToString();
            e.AppendChild(f);

            XmlElement eSites = doc.CreateElement("Sites");
            e.AppendChild(eSites);

            foreach (Site s in node.Sites)
            {
                XmlElement eSite = doc.CreateElement("Site");
                eSite.InnerText = s.Name;
                eSites.AppendChild(eSite);
            }

            doc.DocumentElement.AppendChild(e);
        }

        /// <summary>
        /// 编辑一个项目
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        public void Edit(Task node, int index)
        {
            XmlNode item = doc.DocumentElement.ChildNodes[index];

            ///名字被更改
            item.SelectSingleNode("./Id").InnerText = node.Id + "";
            item.SelectSingleNode("./Name").InnerText = node.Name;  
            item.SelectSingleNode("./Campaign").InnerText = node.Campaign.ToString();
            item.SelectSingleNode("./Time").InnerText = node.Time.ToString();
            item.SelectSingleNode("./OperationName").InnerText = node.OperationName + ""; 
            item.SelectSingleNode("./ResultFtp").InnerText = node.ResultFtp + ""; 
            XmlNode eSites = item.SelectSingleNode("./Sites");
            eSites.RemoveAll();

            foreach (Site s in node.Sites)
            {
                XmlElement eSite = doc.CreateElement("Site");
                eSite.InnerText = s.Name;
                eSites.AppendChild(eSite);
            }
        }

        /// <summary>
        /// 删除指定编号项目
        /// </summary>
        /// <param name="index"></param>
        public void Delete(int index)
        {
            XmlNode item = doc.DocumentElement.ChildNodes[index];
            doc.DocumentElement.RemoveChild(item);
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        public override void Save()
        {
            doc.Save(FilePath);
        }
        /// <summary>
        /// 解析XML
        /// </summary>
        /// <param name="north"></param>
        /// <returns></returns>
        public  List<Task> GetAllTasks()
        {
             List<Task> list = new List<Task>();
            foreach (XmlNode item in doc.DocumentElement.ChildNodes)
            {
                Task node = new Task();
                node.Id = int.Parse(item.SelectSingleNode("./Id").InnerText);
                node.Name = item.SelectSingleNode("./Name").InnerText; 
                node.OperationName =   item.SelectSingleNode("./OperationName").InnerText;
                node.Campaign = item.SelectSingleNode("./Campaign").InnerText;
                node.Time = Time.Parse(item.SelectSingleNode("./Time").InnerText);
                node.ResultFtp = item.SelectSingleNode("./ResultFtp").InnerText;
                node.Sites = new List<Site>(); 
                XmlNodeList ls = item.SelectNodes("./Sites/Site");
                foreach (XmlNode s in ls)
                {
                    var siteName =s.InnerText.Trim();
                    if(String.IsNullOrEmpty(siteName)){continue;}

                    var site = siteManager.Sites.FirstOrDefault(m => String.Equals(m.Name, siteName));
                    if (site == null) { continue; }
                    node.Sites.Add(site);
                 }

                list.Add(node);
            }
            return list;
        }
         
    }
}