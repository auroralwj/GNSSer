//2013.04, czs, create, 基于Bernese的分布式计算
//2015.11.02, czs, eidt in K998 成都到西安南列车, 基于Gnsser操作任务的分布式计算   

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Winform
{
    /// <summary>
    /// 计算节点。
    /// </summary>
    public class BaseComputeNode
    {
        /// <summary>
        /// ID标识
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 是否启用，如果主机关闭，就不用启用了。
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 将任务集合以编号字符串显示
        /// </summary>
        /// <returns></returns>
        public virtual string GetTaskIdsString() { return ""; }

        #region override

        /// <summary>
        /// 字符串查看
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + "(IP：" + Ip + ")";
        }
        /// <summary>
        /// 相等否
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BaseComputeNode o = obj as BaseComputeNode;
            if (o == null) return false;

            return Ip == o.Ip;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Ip != null)
                return Ip.GetHashCode();
            else return 13;
        }
        #endregion
    }

    /// <summary>
    /// 计算节点，通常为一台计算机。
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    public class ComputeNode<TTask> : BaseComputeNode 
        where TTask : BaseTask
    { 
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public ComputeNode()
        {
            this.Tasks = new List<TTask>();
        }

        #region 属性
      
        
        /// <summary>
        /// 任务数量。
        /// </summary>
        public int TaskCount { get { if (Tasks != null) return Tasks.Count; return 0; } }
        /// <summary>
        /// 任务列表
        /// </summary>
        public List<TTask> Tasks { get; set; }
        #endregion

        #region override

        /// <summary>
        /// 字符串查看
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + "(IP：" + Ip + "，Tasks：" + TaskCount + ")";
        }
   
        #endregion
        
        #region IO 任务字符串解析
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<int> ParseTaskIdsString(string str)
        {
            string[] strs = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> list = new List<int>();
            foreach (var item in strs)
            {
                list.Add(int.Parse(item));
            }
            return list;
        }
        /// <summary>
        /// 将任务名称输出为字符串
        /// </summary>
        /// <returns></returns>
        public override string GetTaskIdsString()
        {
            string str = "";
            int i = 0;
            if (Tasks != null)
                foreach (var item in Tasks)
                {
                    if (i != 0) str += ",";
                    str += item.Id;
                    i++;
                }
            return str;
        }
        #endregion
    }


    /// <summary>
    /// 计算节点，通常为一台计算机。
    /// </summary>
    public class BerComputeNode : ComputeNode<Task>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public BerComputeNode()
        {
        }
    }

    /// <summary>
    /// 计算节点，通常为一台计算机。
    /// </summary>
    public class GofComputeNode : ComputeNode<GofTask>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public GofComputeNode()
        {
        }
    }
}
