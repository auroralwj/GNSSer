//2016.12.101, czs, create in hongqing, XmlUtil工具

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Geo.Utils
{
    /// <summary>
    /// XmlUtil工具
    /// </summary>
    public class XmlUtil
    {

        /// <summary>
        /// 创建一个节点和值
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        public static XmlElement CreateElement(XmlDocument doc, string name, string innerText = null)
        {
            var node = doc.CreateElement(name);
            if (innerText != null)
            {
                node.InnerText = innerText;
            }
            return node;
        }
        /// <summary>
        /// 创建一个节点和子节点
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <param name="childName"></param>
        /// <param name="childInnerText"></param>
        /// <returns></returns>
        public static XmlElement CreateElementWithChild(XmlDocument doc, string name, string childName, string childInnerText = null)
        {
            var node = doc.CreateElement(name);
            var child = doc.CreateElement(childName);
            if (childInnerText != null)
            {
                child.InnerText = childInnerText;
            }
            node.AppendChild(child);
            return node;
        }

        /// <summary>
        /// 创建一个节点和子节点
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <param name="childName"></param>
        /// <param name="childInnerText"></param>
        /// <returns></returns>
        public static XmlElement CreateElementWithChild(XmlDocument doc, string name, Dictionary<string, object> children)
        {
            var node = doc.CreateElement(name);
            foreach (var dic in children)
            {
                var child = doc.CreateElement(dic.Key);
                if (dic.Value != null)
                {
                    child.InnerText = dic.Value + "";
                }
                node.AppendChild(child);

            }

            return node;
        }



    }
}
