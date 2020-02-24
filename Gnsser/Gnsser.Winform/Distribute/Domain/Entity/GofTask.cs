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
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 计算任务
    /// </summary>
    public class GofTask : BaseTask
    {
        #region 构造函数
        /// <summary>
        /// GOF文件直接初始化
        /// </summary>
        /// <param name="flow"></param>
        public GofTask(OperationFlow flow)
        {
            this.Name = flow.FileName;
            this.OperationInfos = flow.Data;

            this.Params = new Dictionary<string, string>();
            foreach (var item in OperationInfos)
            {
                var paramName = Path.GetFileName(item.ParamFilePath);
                Params[paramName] = File.ReadAllText(item.ParamFilePath);
            } 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public GofTask()
        {
            this.OperationInfos = new List<OperationInfo>();
            this.Params = new Dictionary<string, string>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        public GofTask(string gofFilePath)
        {
            this.OperationInfos = new OperationInfoReader(gofFilePath).ReadAll();
            this.Params = new Dictionary<string, string>();
            foreach (var item in OperationInfos)
            {
                var paramName = Path.GetFileName(item.ParamFilePath);
                Params[paramName] = File.ReadAllText(item.ParamFilePath); 
            } 
        }
        #endregion

        #region 属性
        /// <summary>
        /// 操作信息
        /// </summary>
        public List<OperationInfo> OperationInfos { get; set; }
        /// <summary>
        /// 参数，直接以字符串表示。
        /// </summary>
        public Dictionary<string, string> Params { get; set; }
        #endregion

        /// <summary>
        /// 将列表中的操作信息提取为字符串。
        /// </summary>
        /// <returns></returns>
        public string GetGofFileContent()
        {
            MemoryStream stream = new MemoryStream();
            OperationInfoWriter writer = new OperationInfoWriter(stream, null);

            foreach (var item in OperationInfos)
            {
                writer.Write(item.ToString());
            }

            writer.StreamWriter.Flush();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            var txt = reader.ReadToEnd();
            return txt;
        }
        /// <summary>
        /// 将列表中的操作信息提取为字符串。
        /// </summary>
        /// <returns></returns>
        public string GetParamFileString()
        {
            MemoryStream stream = new MemoryStream();
            OperationInfoWriter writer = new OperationInfoWriter(stream, null);

            foreach (var item in Params)
            {
                writer.Write(item.ToString());
            }

            writer.StreamWriter.Flush();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            var txt = reader.ReadToEnd();
            return txt;
        }

        #region override
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            GofTask t = obj as GofTask;
            if (t == null) return false;

            return Name.Equals(t.Name) && Id.Equals(t.Id);
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = Name + "," + OperationInfos.Count;
            return str;
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

            sb.Append("<OperationInfos>");
            foreach (var OperationInfo in OperationInfos)
            {
                sb.Append("<OperationInfo>");
                sb.Append("<Name>");
                sb.Append(OperationInfo.Name);
                sb.AppendLine("</Name>");
                sb.Append("<OperationName>");
                sb.Append(OperationInfo.OperationName);
                sb.AppendLine("</OperationName>");
                sb.Append("<DependsString>");
                sb.Append(OperationInfo.DependsString);
                sb.AppendLine("</DependsString>");
                sb.Append("<ParamFilePath>");
                sb.Append(OperationInfo.ParamFilePath);
                sb.AppendLine("</ParamFilePath>");
                sb.AppendLine("</OperationInfo>");
            }
            sb.Append("</OperationInfos>");

            sb.Append("<Params>");
            foreach (var para in Params)
            {
                sb.Append("<Param>");
                sb.Append("<Name>");
                sb.Append(para.Key);
                sb.AppendLine("</Name>");
                sb.Append("<Content>");
                sb.Append(para.Value);
                sb.AppendLine("</Content>");
                sb.AppendLine("</Param>");
            }
            sb.Append("</Params>");


            sb.AppendLine("</Task>");
            return sb.ToString();
        }
        /// <summary>
        /// 解析XML字符串
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static GofTask ParseXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement e = doc.DocumentElement;
            GofTask task = new GofTask();
            task.Id = int.Parse(e.SelectSingleNode("./Id").InnerText);
            task.Name = e.SelectSingleNode("./Name").InnerText;
            task.OperationInfos = new List<OperationInfo>();
            XmlNodeList list = e.SelectNodes("./OperationInfos/OperationInfo");
            foreach (XmlNode item in list)
            {
                OperationInfo OperationInfo = new Geo.OperationInfo
                {
                    Name = item.SelectSingleNode("./Name").InnerText,
                    OperationName = item.SelectSingleNode("./OperationName").InnerText,
                    DependsString = item.SelectSingleNode("./DependsString").InnerText,
                    ParamFilePath = item.SelectSingleNode("./ParamFilePath").InnerText,
                };
                task.OperationInfos.Add(OperationInfo);
            }


            XmlNodeList paramlist = e.SelectNodes("./Params/Param");
            foreach (XmlNode item in paramlist)
            {
                var name = item.SelectSingleNode("./Name").InnerText;
                var content = item.SelectSingleNode("./Content").InnerText;

                task.Params.Add(name, content); 
            }


            return task;
        }
        #endregion
    }  
}
