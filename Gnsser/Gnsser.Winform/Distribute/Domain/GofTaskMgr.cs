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
    public class GofTaskMgr : SingleFileManager
    {
        //核心存储
        XmlDocument doc = new XmlDocument();

        /// <summary>
        ///默认构造函数
        /// </summary>
        public GofTaskMgr(string filePath)
            : base(filePath)
        { 
        } 
         

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
        public void AddRange(List<GofTask> tasks)
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
        public void Add(GofTask node)
        {
            node.Id = this.doc.DocumentElement.ChildNodes.Count;
            XmlDocument newdoc = new XmlDocument();
            newdoc.LoadXml(node.ToXml());
            XmlElement e = newdoc.DocumentElement;

            var newNode = doc.ImportNode( e, true);
            doc.DocumentElement.AppendChild(newNode);
        }

        /// <summary>
        /// 编辑一个项目
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">顺序序号，从 0 开始</param>
        public void Edit(GofTask node, int index)
        {
            XmlDocument newdoc = new XmlDocument();
            newdoc.LoadXml(node.ToXml());
            XmlElement e = newdoc.DocumentElement;
            doc.DocumentElement.ChildNodes[index].InnerXml = e.InnerXml; 
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
        public  List<GofTask> GetAllTasks()
        {
             List<GofTask> list = new List<GofTask>();
            foreach (XmlNode item in doc.DocumentElement.ChildNodes)
            {
                var task = GofTask.ParseXml(item.OuterXml);
                list.Add(task); 
            }
            return list;
        }
         
    }
}