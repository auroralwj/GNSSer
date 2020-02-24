
//2015.05.28, czs, create in namu,  数据库模型  
//2015.06.10, czs, refactor in namu,提取抽象类 AbstractTreeNode
//2016.02.28, czs, refactor in hongiqng, 提取抽象接口


using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.Data.Entity;
using System.Globalization;
using Geo;
using Geo.Utils;

namespace Geo
{
    /// <summary>
    /// ID 为整型的树形节点
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public  class IntIdTreeNode<TNode> : AbstractTreeNode<int, TNode, int?>, Geo.IIntId, IChildOfNullableInt
       where TNode : IntIdTreeNode<TNode>{

           /// <summary>
           /// 设置父节点，同时设置父ID。
           /// </summary>
           /// <param name="parent"></param>
           public void SetParent(TNode parent)
           {
               parent.Children.Add(this as TNode);
               this.Parent = parent;
               this.ParentId = (parent.Id);
           }
    }
    /// <summary>
    /// ID 为String的树形节点
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public  class StringIdTreeNode<TNode> :  AbstractTreeNode<string, TNode, string>, Geo.IStringId
       where TNode : StringIdTreeNode<TNode>{

           /// <summary>
           /// 设置父节点，同时设置父ID。
           /// </summary>
           /// <param name="parent"></param>
           public void SetParent(TNode parent)
        {
               parent.Children.Add(this as TNode);
               this.Parent = parent;
               this.ParentId = (parent.Id);
           }
    }


    /// <summary>
    /// 通用树形节点，抽象树节点。
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public abstract class AbstractTreeNode<Tkey, TNode, TParentkey> : IdentifyNamed<Tkey>, IEnumerable<TNode>, ITreeNode<Tkey, TNode, TParentkey>
        where TNode : AbstractTreeNode<Tkey, TNode, TParentkey>
       
    {
        public AbstractTreeNode()
        {
            Children = new List<TNode>();
        }


        public virtual TParentkey ParentId { get; set; }

        [Display(Name = "父节点")]
        public virtual TNode Parent { get; set; }
 

        //[Display(Name = "子节点")]
        public virtual List<TNode> Children { get; set; }

        /// <summary>
        /// 从 0 开始。
        /// </summary>
        [Display(Name = "等级")]
        public virtual int Rank { get; set; }
        #region 重新

        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            TNode t = obj as TNode;
            if (t == null) return false;

            return (t.Id.Equals( Id) && t.Name == Name);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() * 19;// +(int)(ParentId) * 13;
        }
        #endregion


        #region 扩展属性

        /// <summary>
        /// 是否具有子节点
        /// </summary> 
        public bool HasChild { get { return (Children != null) && (Children.Count != 0); } }


        /// <summary>
        /// 是否具有父节点 通过 ParentId 判断
        /// </summary>

        public bool HasParent { get { return this.Parent != null; } }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth
        {
            get
            {
                int count = 0;
                if (this.HasChild)
                {
                    foreach (var item in this.Children)
                    {
                        count += item.Depth;
                    }
                    count++;
                }
                return count;
            }
        }

        #endregion

        #region 常用方法


        /// <summary>
        /// 获取所有的名称，含本节点。
        /// </summary>
        /// <returns></returns>
        public List<string> GetNodeNames()
        {
            List<string> names = new List<string>();
            foreach (var item in this)
            {
                names.Add(item.Name);
            }

            return names;
        }

        /// <summary>
        /// 所有的树叶节点(含本身)以列表形式返回。
        /// </summary>
        /// <returns></returns>
        public List<TNode> GetNodeList()
        {
            List<TNode> list = new List<TNode>();
            list.Add((TNode)this);

            if (this.HasChild)
            {
                foreach (var item in this.Children)
                {
                    list.AddRange(item.GetNodeList());
                }
            }
            return list;
        }

        /// <summary>
        /// 获取节点的路径。
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            string path = Name;
            TNode current = (TNode)this;
            List<Tkey> ids = new List<Tkey>();
            while (current.HasParent)
            {
                path = current.Parent.Name + "/" + path;
                current = current.Parent;

                if (ids.Contains((current.Id)))
                    throw new Exception("天啊！树中有回路！" + path);

                ids.Add((current.Id));
            }
            return path;
        }

        /// <summary>
        /// 获取没有子节点的节点，常用于从底到上进行删除.
        /// 如果没有则返回自身。
        /// 如果只包含一个则是他本身,反之也成立
        /// </summary>
        /// <param name="treeId"></param>
        /// <returns></returns>
        public IList<TNode> GetNoChildNodes()
        {
            List<TNode> list = new List<TNode>();
            if (!this.HasChild)
            {
                list.Add((TNode)this);
                return list;
            }
            else
            {
                foreach (TNode n in this.Children)
                {
                    list.AddRange(n.GetNoChildNodes());
                }
            }
            return list;
        }
        #endregion

        #region 重写 GetEnumerator
        public IEnumerator<TNode> GetEnumerator()
        {
            return GetNodeList().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetNodeList().GetEnumerator();
        }
        #endregion

        #region IO
        /// <summary>
        /// 以返回文本化的树形目录
        /// </summary>
        /// <returns></returns>
        public string GetTreeText(string spliter = "\t", int depth = 1)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            // sb.AppendLine();

            foreach (var item in this.Children)
            {
                for (int i = 0; i < depth; i++)
                {
                    sb.Append(spliter);
                }
                sb.Append(item.GetTreeText(spliter, depth + 1));
            }
            return sb.ToString();
        }
        #endregion

        #region IO
        /// <summary>
        /// 文本解析
        /// </summary>
        /// <param name="nodeLines"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static TNode ParseTreeText<TNode>(ICollection<string> nodeLines, string spliter = "\t")
            where TNode : AbstractTreeNode<Tkey, TNode, TParentkey>, new()
        {
            if (nodeLines.Count == 0) return null;

            //存储每一层次当前的节点
            Dictionary<int, TNode> current = new Dictionary<int, TNode>();
            int newDepth = 0;
            foreach (var line in nodeLines)
            {
                if (line.Trim() == "") continue;

                var n = new TNode { Name = line.Trim() };

                newDepth = StringUtil.StartCount(line, spliter);

                current[newDepth] = n;
                if (newDepth > 0)
                {
                    current[newDepth - 1].Children.Add(n); //当前深度的上一个
                }
            }

            return current[0];
        }
        #endregion
    }



     
}