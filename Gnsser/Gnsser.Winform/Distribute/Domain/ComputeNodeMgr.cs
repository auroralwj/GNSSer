//2013.04, czs, create, 基于Bernese的分布式计算
//2015.11.02, czs, edit in K998  成都到西安南列车, 基于Gnsser操作任务的分布式计算   

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 计算节点管理器
    /// </summary>
    public class ComputeNodeMgr<TTask> : SingleFileManager
    {
        //核心存储
        XmlDocument doc = new XmlDocument();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FilePath"></param>
        public ComputeNodeMgr(string FilePath) : base(FilePath)
        { 
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="FilePath"></param>
        public override void Load()
        {
            doc.Load(FilePath);
        } 

        /// <summary>
        /// 添加一个节点
        /// </summary>
        /// <param name="node"></param>
        public void Add(BaseComputeNode node)
        {
            XmlElement e = doc.CreateElement("Node");
            XmlElement nName = doc.CreateElement("Name");
            nName.InnerText = node.Name;
            e.AppendChild(nName);
            XmlElement nNumber = doc.CreateElement("Id");
            nNumber.InnerText = node.Id.ToString();
            e.AppendChild(nNumber);
            XmlElement eIp = doc.CreateElement("IP");
            eIp.InnerText = node.Ip;
            e.AppendChild(eIp);
            XmlElement ePort = doc.CreateElement("Port");
            ePort.InnerText = node.Port.ToString();
            e.AppendChild(ePort);
            XmlElement t = doc.CreateElement("TaskId");
            t.InnerText = node.GetTaskIdsString();
            e.AppendChild(t);
            XmlElement te = doc.CreateElement("Enabled");
            te.InnerText = node.Enabled+"";
            e.AppendChild(te);
            
            doc.DocumentElement.AppendChild(e);
        }
        /// <summary>
        /// 编辑指定节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        public void Edit(BaseComputeNode node, int index)
        {
            XmlNode item = doc.DocumentElement.ChildNodes[index];

            item.SelectSingleNode("./Name").InnerText = node.Name;
            item.SelectSingleNode("./Id").InnerText = node.Id.ToString();
            item.SelectSingleNode("./IP").InnerText = node.Ip;
            item.SelectSingleNode("./Port").InnerText = node.Port + "";
            item.SelectSingleNode("./TaskId").InnerText = node.GetTaskIdsString();
            item.SelectSingleNode("./Enabled").InnerText = node.Enabled + ""; 
        }
        /// <summary>
        /// 删除节点
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

        #region 静态工具方法
        /// <summary>
        /// 解析XML
        /// </summary>
        /// <param name="ComputeNodeFilePath"></param>
        /// <param name="TaskFilePath"></param>
        /// <returns></returns>
        public static List<BerComputeNode> LoadComputeNodes(string ComputeNodeFilePath, string TaskFilePath, string sitePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ComputeNodeFilePath);

            List<Task> tasks = new TaskMgr(TaskFilePath, sitePath).GetAllTasks();

            List<BerComputeNode> list = new List<BerComputeNode>();
            foreach (XmlNode item in doc.DocumentElement.ChildNodes)
            {
                BerComputeNode node = new BerComputeNode();
                node.Id = int.Parse(item.SelectSingleNode("./Id").InnerText);
                node.Name = item.SelectSingleNode("./Name").InnerText;
                node.Ip = item.SelectSingleNode("./IP").InnerText;
                node.Port = int.Parse(item.SelectSingleNode("./Port").InnerText);
                node.Enabled = Boolean.Parse(item.SelectSingleNode("./Enabled").InnerText);

                List<int> taskId = BerComputeNode.ParseTaskIdsString(item.SelectSingleNode("./TaskId").InnerText);
                try
                {
                    node.Tasks = tasks.FindAll(m => taskId.Contains(m.Id));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                list.Add(node);
            }
            return list;
        }

        /// <summary>
        /// 解析XML
        /// </summary>
        /// <param name="ComputeNodeFilePath"></param>
        /// <param name="TaskFilePath"></param>
        /// <returns></returns>
        public static List<GofComputeNode> LoadGofComputeNodes(string ComputeNodeFilePath, string TaskFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ComputeNodeFilePath);

            var tasks = new GofTaskMgr( TaskFilePath).GetAllTasks();

            List<GofComputeNode> list = new List<GofComputeNode>();
            foreach (XmlNode item in doc.DocumentElement.ChildNodes)
            {
                GofComputeNode node = new GofComputeNode();
                node.Id = int.Parse(item.SelectSingleNode("./Id").InnerText);
                node.Name = item.SelectSingleNode("./Name").InnerText;
                node.Ip = item.SelectSingleNode("./IP").InnerText;
                node.Port = int.Parse(item.SelectSingleNode("./Port").InnerText);
                node.Enabled = Boolean.Parse(item.SelectSingleNode("./Enabled").InnerText);

                List<int> taskId = BerComputeNode.ParseTaskIdsString(item.SelectSingleNode("./TaskId").InnerText);
                try
                {
                    node.Tasks = tasks.FindAll(m => taskId.Contains(m.Id));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                list.Add(node);
            }
            return list;
        }


        #endregion
    }


    public class GofComputeNodeMgr : ComputeNodeMgr<GofTask>
    {    
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FilePath"></param>
        public GofComputeNodeMgr(string FilePath) : base(FilePath)
        { 
        }

    }
    public class BerComputeNodeMgr : ComputeNodeMgr<Task>
    {        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FilePath"></param>
        public BerComputeNodeMgr(string FilePath)
            : base(FilePath)
        { 
        }
    }
}
