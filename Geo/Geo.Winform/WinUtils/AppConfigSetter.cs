//2012.02.07，

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Geo.Utils
{
    /// <summary>
    /// 对 App.Config 文件进行读写.可以添加值。
    /// 由于 http://msdn.microsoft.com/zh-cn/library/1xtk877y.aspx 2012.02.07 中写道：
    /// 托管代码可以使用 System.Configuration 命名空间中的类从配置文件中读取设置，但不向这些文件写入设置。
    /// 因此写本类。
    /// </summary>
    public class AppConfigSetter
    {
        /// <summary>
        /// 对  App.Config 文件进行读写.
        /// </summary>
        public AppConfigSetter()
        { //此处配置文件在程序目录下
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            this.Path = codeBase + ".config";            
        }
        /// <summary>
        /// 对  App.Config 文件进行读写.
        /// </summary>
        /// <param name="path">指定路径</param>
        public AppConfigSetter(string path)
        {
            this.Path = path;
        }
        XmlDocument xDoc = new XmlDocument();

        private string appName;

        /// <summary>
        /// 程序名称，以此来确定配置文件的路径。若不指定，则自动采用程序集名称。
        /// </summary>
        public string AppName
        {
            get { return appName; }
            set { appName = value.Trim();
            this.Path = FileUtil.GetAssemblyFolderPath() + appName + ".exe.config";   
            }
        }
        /// <summary>
        /// 配置文件路径。
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string SingleNodeName { get; set; }
        /// <summary>
        /// 设置app.config中的某个key的value.
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="AppKey">keyPrev</param>
        /// <param name="AppValue">value</param>
        public void SetValue(string nodeName, string AppKey, string AppValue)
        {
            this.SingleNodeName = nodeName;
            SetValue(AppKey, AppValue);        
        }
        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AppValue"></param>
        public void SetValue(string AppKey, string AppValue)
        {
            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            xNode = xDoc.SelectSingleNode(SingleNodeName);
            xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xElem1 != null)
            {
                xElem1.SetAttribute("value", AppValue);
            }
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                xNode.AppendChild(xElem2);
            }
        }
        /// <summary>
        /// 加载
        /// </summary>
        public void Open()
        {
            xDoc.Load(Path);
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            xDoc.Save(Path);
        }

    }
}
