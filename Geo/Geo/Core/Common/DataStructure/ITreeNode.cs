
//2015.06.10, czs, refactor in namu,提取抽象接口 ITreeNode


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
    /// 标识是一个节点，可以具有父亲，父可以为Null
    /// </summary>
    public interface IIntNode<TParent> : INode<int, TParent, int?>, IIntId, IChildOfNullableInt
    {
        //where TParent : IIntId  
    }
    /// <summary>
    /// 标识是一个节点，可以具有父亲，父可以为Null
    /// </summary>
    public interface IStringNode<TParent> : INode<string, TParent, string>, IStringId, IChild<string>
    {
        //where TParent : IIntId  
    }

    /// <summary>
    /// 标识是一个节点，可以具有父亲，父可以为Null
    /// </summary>
    public interface INode<Tkey, TParent, TParentKey> : Identifiable<Tkey>, IChild<TParentKey> 
    {
        /// <summary>
        /// 父节点
        /// </summary>
        TParent Parent { get; set; }
    }


    public interface IStringTreeNode<TNode> : ITreeNode<int, TNode, String>
      where TNode : IStringTreeNode<TNode>
    {

    }

    public interface IIntTreeNode<TNode> : ITreeNode<int, TNode, int>
      where TNode : IIntTreeNode<TNode>
    {

    }

    /// <summary>
    /// 树形节点接口,具有相同类型的子节点和父节点
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public interface ITreeNode<TKey, TNode, TParentKey> : INode<TKey, TNode, TParentKey>, Namable
        where TNode : ITreeNode<TKey, TNode, TParentKey> 
    {
        #region 核心属性
        /// <summary>
        /// 子节点结合
        /// </summary>
        List<TNode> Children { get; set; }
        #endregion

        #region 常用属性
        /// <summary>
        /// 深度
        /// </summary>
        int Depth { get; }
        /// <summary>
        /// 是否有子节点
        /// </summary>
        bool HasChild { get; }
        /// <summary>
        /// 是否有父节点
        /// </summary>
        bool HasParent { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 获取没有子节点的节点。
        /// </summary>
        /// <returns></returns>
        IList<TNode> GetNoChildNodes();
        /// <summary>
        /// 获取所有的节点，含本身。
        /// </summary>
        /// <returns></returns>
        List<TNode> GetNodeList();
        /// <summary>
        /// 获取所有的名称列表
        /// </summary>
        /// <returns></returns>
        List<string> GetNodeNames();
        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        string GetPath();
        /// <summary>
        /// 导出为文本
        /// </summary>
        /// <param name="spliter"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        string GetTreeText(string spliter = "\t", int depth = 1);
        #endregion
    }
}
