//2016.01.17，czs, created

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo
{
    /// <summary>
    /// 树形节点接口
    /// </summary>
    public interface ITreeNode : Namable{
        /// <summary>
        /// 是否具有主节点
        /// </summary>
        bool HasChild{get;}
    }

    /// <summary>
    /// 树形节点接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeNode<T> : ITreeNode
    where T : ITreeNode
    {
        /// <summary>
        /// 自己诶单
        /// </summary>
        List<T> Children{get;}
        /// <summary>
        /// 转换为所有的节点，包含本身，本身在第一个节点。
        /// </summary>
        List<T> ToList();
    }

    /// <summary>
    /// 名称.封装了一个Name属性。实质上就是一个string对象。
    /// </summary> 
    public class BaseTreeNode<T> : Named, ITreeNode<T>
    where T : BaseTreeNode<T>
    {
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public BaseTreeNode(string name = "")
            : base(name)
        {
            this.Children = new List<T>();
        }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<T> Children { get; set; }

        /// <summary>
        /// 是否具有子节点
        /// </summary>
        public bool HasChild
        {
            get { return Children != null && Children.Count > 0; }
        }

        /// <summary>
        /// 转换为所有的节点，包含本身，本身在第一个节点。
        /// </summary>
        public List<T> ToList()
        {
            List<T> list = new List<T>();
            list.Add((T)this);
            if (HasChild)
            {
                foreach (var item in Children)
                {
                    list.AddRange(item.ToList());
                }
            }
            return list;
        }
    }
}
