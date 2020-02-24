//2013.04, czs, create, 基于Bernese的分布式计算
//2015.11.02, czs, edit in K998  成都到西安南列车, 基于Gnsser操作任务的分布式计算   

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Gnsser.Interoperation.Bernese;
using Geo.Times;
using System.IO;

namespace Gnsser.Winform
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public class BaseTask
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } 

    }
    

    /// <summary>
    /// 计算任务
    /// </summary>
    public class Task : BaseTask
    {     
        /// <summary>
        /// 工程
        /// </summary>
        public string Campaign { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }
        /// <summary>
        /// 操作文件名称
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 结果保存到FTP。
        /// </summary>
        public string ResultFtp { get; set; } 
        /// <summary>
        /// 待计算的测站。
        /// </summary>
        public List<Site> Sites { get; set; }
        /// <summary>
        /// 测站URLS
        /// </summary>
        public List<string> Urls
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var item in Sites)
                {
                    list.Add(item.Url);
                }

                return list;
            }
        }

        #region override
        public override bool Equals(object obj)
        {
            Task t = obj as Task;
            if (t == null) return false;

            return Name.Equals(t.Name) && Id.Equals(t.Id);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        public override string ToString()
        {
            string str = Name + "," + Campaign + "," + Time + "," + OperationName + "," + ResultFtp + "{";
            StringBuilder sb = new StringBuilder();
            //foreach (var text in Urls)
            //{
            //    sb.AppendLine(text);                
            //}

            return str + sb.ToString() + "}";
        }
        #endregion

        #region IO
        /// <summary>
        /// 生成XML字符串
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<Task>");
            sb.Append("<Id>");
            sb.Append(Id);
            sb.AppendLine("</Id>");
            sb.Append("<Name>");
            sb.Append(Name);
            sb.AppendLine("</Name>");
            sb.Append("<Campaign>");
            sb.Append(Campaign);
            sb.AppendLine("</Campaign>");
            sb.Append("<Time>");
            sb.Append(Time);
            sb.AppendLine("</Time>");
            sb.Append("<OperationName>");
            sb.Append(OperationName);
            sb.AppendLine("</OperationName>"); 
            sb.Append("<ResultFtp>");
            sb.Append(ResultFtp);
            sb.AppendLine("</ResultFtp>");

            sb.Append("<Urls>");
            if (Sites != null)
            foreach (var url in Sites)
            {
                sb.Append("<url>");
                sb.Append(url.Url);
                sb.AppendLine("</url>");
            }
            sb.AppendLine("</Urls>");

            sb.AppendLine("</Task>");
            return sb.ToString();
        }
        /// <summary>
        /// 解析XML字符串
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Task ParseXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement e = doc.DocumentElement;
            Task c = new Task();
            c.Id =int.Parse( e.SelectSingleNode("./Id").InnerText);
            c.Name = e.SelectSingleNode("./Name").InnerText;
            c.Campaign = e.SelectSingleNode("./Campaign").InnerText; 
            c.ResultFtp = e.SelectSingleNode("./ResultFtp").InnerText;
            c.Time =Time.Parse( e.SelectSingleNode("./Time").InnerText);
            c.OperationName = e.SelectSingleNode("./OperationName").InnerText;

            c.Sites = new List<Site>();
            XmlNodeList list = e.SelectNodes("./Urls/url");
            foreach (XmlNode item in list)
            {
                var site = new Site(item.InnerText); 
                c.Sites.Add(site);
            }

            return c;
        }
        #endregion


    }  
}
