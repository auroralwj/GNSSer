//2013.04, czs, create, 基于Bernese的分布式计算
//2015.11.02, czs, edit in K998  成都到西安南列车, 基于Gnsser操作任务的分布式计算   
//2016.11.27, czs, edit in hongqing, 测试，调试

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Gnsser.Interoperation.Bernese;

namespace Gnsser.Winform
{


    /// <summary>
    /// 信息类型
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 传输的路径，多个路径以分号分隔，需要下载
        /// </summary>
        Path, 
        /// <summary>
        /// 任务
        /// </summary>
        Task,
        /// <summary>
        /// 字符串
        /// </summary>
        String
    }


    /// <summary>
    /// 远程任务类，用于指示远程通信消息。
    /// </summary>
    public class TelMsg
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType MsgType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string XmlContent { get; set; }
        /// <summary>
        /// 源地址
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 目标地址
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 转换为 XML
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement e = doc.CreateElement("TelMsg");
            doc.AppendChild(e);

            XmlElement eFrom = doc.CreateElement("From");
            eFrom.InnerText = From.ToString();
            e.AppendChild(eFrom);
            XmlElement eTo = doc.CreateElement("To");
            eTo.InnerText = To.ToString();
            e.AppendChild(eTo);
            XmlElement eMsgType = doc.CreateElement("MsgType");
            eMsgType.InnerText = MsgType.ToString();
            e.AppendChild(eMsgType);

            XmlElement eContent = doc.CreateElement("XmlContent");
            eContent.InnerXml = XmlContent;
            e.AppendChild(eContent);

            return doc.InnerXml;
        }

        /// <summary>
        /// 解析XML
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static TelMsg ParseXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement e = doc.DocumentElement;
            TelMsg c = new TelMsg();
            c.MsgType = (MsgType)Enum.Parse(typeof(MsgType), e.SelectSingleNode("./MsgType").InnerText);
            c.XmlContent = e.SelectSingleNode("./XmlContent").InnerXml;
            c.From = e.SelectSingleNode("./From").InnerXml;
            c.To = e.SelectSingleNode("./To").InnerXml;

            return c;
        }

    }
}
