//2015.05.30, czs, create in namu,  WinForm树形工具
//2017.01.29, czs, edit in pengzhou tanmu, 增加级联选择
//2019.01.16, czs, edit in hmx, 改进子节点获取

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

using System.Windows.Forms;

namespace Geo.Utils
{
    /// <summary>
    /// WinForm树形工具
    /// </summary>
    public static class TreeNodeUtil
    {
        /// <summary>
        /// 选中所有父和子节点。
        /// </summary>
        /// <param name="node"></param>
        public static void CheckAllDirectRelativeNodes(TreeNode node)
        {
            var allParents = GetAllParents(node);
            CheckAllNodes(allParents);

            var subNodes = GetSubNodes(node);
            CheckAllNodes(subNodes);
        }
        /// <summary>
        /// 选中所有父和子节点。
        /// </summary>
        /// <param name="node"></param>
        public static void UnCheckSubNodes(TreeNode node)
        { 
            var subNodes = GetSubNodes(node);
            UnCheckAllNodes(subNodes);
        }


        /// <summary>
        /// 选中所有的节点
        /// </summary>
        /// <param name="nodes"></param>
        public static void CheckAllNodes(IEnumerable<TreeNode> nodes)
        {
            foreach (var item in nodes)
            {
                if (!item.Checked)
                {
                    item.Checked = true;
                }
            }
        }
        /// <summary>
        /// 取消所有的节点的选中状态
        /// </summary>
        /// <param name="nodes"></param>
        public static void UnCheckAllNodes(IEnumerable<TreeNode> nodes)
        {
            foreach (var item in nodes)
            {
                if (item.Checked)
                {
                    item.Checked = false;
                }
            }
        }

        /// <summary>
        /// 当前的 object.如果选择的是Obj则直接返回，如果不是，则找其下的第一个，如果都没有，则返回null。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="thisLevelOnly"></param>
        /// <returns></returns>
        public static T GetCurrentObject<T>(TreeNode node, bool thisLevelOnly=false) where T : class
        {
            if (node == null) return null;

            if (node.Tag is T) return (T)node.Tag;
            else if (!thisLevelOnly && node.Nodes.Count != 0)
            {
                return GetCurrentObject<T>(node.Nodes[0]);
            }

            return null;
        }



        public static TreeNode GetFormNodeByTag(TreeNode formNode, Object tag)
        {
            if (tag == null) return null;

            if (formNode.Tag.Equals(tag)) return formNode;

            foreach (TreeNode item in formNode.Nodes)
            {
                TreeNode n = GetFormNodeByTag(item, tag);
                if (n != null)
                    return n;
            }
            return null;
        }


        /// <summary>
        /// 设置当前选中节点，并咱开到该节点。
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="tag"></param>
        public static void SetSelectedNodeByTag(TreeView TreeView, object tag)
        {
            SetSelectedNodeByTag(tag, TreeView.Nodes, TreeView);
        }

        //递归查找
        private static void SetSelectedNodeByTag(object tag, TreeNodeCollection nodeCollect, TreeView TreeView)
        {
            foreach (TreeNode node in nodeCollect)
            {
                if (node.Tag.Equals(tag))
                {
                    TreeView.SelectedNode = node;
                    TreeNodeUtil.ExpandTo(node);
                    return;
                }
            }
            foreach (TreeNode node in nodeCollect)
            {
                SetSelectedNodeByTag(tag, node.Nodes, TreeView);
            }
        }
        /// <summary>
        /// 找到绑定选中的对象，并展开到该节点。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="treeView"></param>
        /// <param name="t"></param>
        public static TreeNode SelectAndExpandTot<T>(TreeView treeView, T t)
        {
            TreeNode node = FindTreeNodeByTag<T>(treeView.TopNode, t);//默认顶层为1个节点。

            ExpandTo(node);
            return node;
        }
        public static TreeNode FindTreeNodeByTag<T>(TreeNode node, T t)
        {
            if (node == null || t == null) return null;

            if (node.Tag is T && t.Equals(node.Tag)) return node;

            foreach (TreeNode n in node.Nodes)
            {
                TreeNode sub = FindTreeNodeByTag<T>(n, t);
                if (sub != null) return sub;
            }
            return null;
        }
        /// <summary>
        /// 展开到选定目录。并使其处于选中状态。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void ExpandTo(TreeNode node)
        {
            if (node == null) return;

            TreeNode tempNode = node;
            tempNode.Expand();
            while (tempNode.Parent != null)
            {
                tempNode = tempNode.Parent;
                tempNode.Expand();
            }
        }

        /// <summary>
        /// 返回当前 TreeNode 及其子节点的 Tag 属性是 T 类型 的 T 实体集合。
        /// 如果不是 T 类型，则跳过。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<T> GetCurrentAndSubObjects<T>(TreeNode node)
        {
            List<T> list = new List<T>();
            List<TreeNode> nodes = GetSubNodes(node);
            foreach (TreeNode n in nodes)
            {
                if (n.Tag is T) list.Add((T)n.Tag);
            }
            return list;
        }

        /// <summary>
        /// 获取指定节点（含[可选]）下的所有节点。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isCurrentInclude"></param>
        /// <param name="maxDepth">从0开始</param>
        /// <returns></returns>
        public static List<TreeNode> GetSubNodes(TreeNode node, bool isCurrentInclude = true, int maxDepth = 10000)
        {
            List<System.Windows.Forms.TreeNode> list = new List<TreeNode>();
            if (node == null) return list;

            if (isCurrentInclude) { list.Add(node); }

            foreach (TreeNode n in node.Nodes) { list.AddRange(GetSubNodes(n,0, maxDepth)); }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="currentDepth">从0开始</param>
        /// <param name="maxDepth">最大深度，0代表当前</param>
        /// <returns></returns>
        private static List<TreeNode> GetSubNodes(TreeNode node, int currentDepth, int maxDepth = 100000)
        {
            List<System.Windows.Forms.TreeNode> list = new List<TreeNode>();
            if (node == null || currentDepth >= maxDepth) { return list; }

            currentDepth++;

            list.Add(node); //必然包含自身

            foreach (TreeNode n in node.Nodes) { list.AddRange(GetSubNodes(n, currentDepth, maxDepth)); }
            return list;
        }
        /// <summary>
        /// 返回所有的父节点。
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<TreeNode> GetAllParents(TreeNode node)
        { 
            List<TreeNode> nodes = new List<TreeNode>();
            if (node.Parent == null)
            {
                return nodes;
            }
            var parent = node.Parent;
            while (parent != null)
            {
                nodes.Add(parent);
                parent = parent.Parent;
            }

            return nodes;
        }

        /// <summary>
        ///  获取当前节点下的所有子节点（只包含第一级字节点）。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="treeNode"></param>
        /// <param name="isCurrentInclude">是否包括当前</param>
        /// <param name="maxDepth">最大深度，0代表当前</param>
        /// <returns></returns>
        public static List<T> GetCurrentSubObjects<T>(TreeNode treeNode, bool isCurrentInclude = true, int maxDepth = 100000)
        {
            var nodes =  GetSubNodes(treeNode, isCurrentInclude, maxDepth);

            List<T> list = new List<T>();
            foreach (TreeNode n in nodes)
            {
                if (n.Tag is T) list.Add((T)n.Tag);
            }
            return list;
        } 


        /// <summary>
        /// 深度
        /// </summary>
        static public int GetDepth(TreeNode node)
        {
            int count = 0;
            if (node.Nodes.Count != 0)
            {
                foreach (TreeNode item in node.Nodes)
                {
                    count += GetDepth(item);
                }
                count++;
            }
            return count;
        }

        /// <summary>
        /// 返回过滤后的节点，如果没有则为空。
        /// 如果子节点有，则父节点也保存。
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="isFuzzy"></param>
        /// <param name="containsOrNot"></param>
        /// <returns></returns>
        static public void Filter(TreeNode node,  string keyword, bool isFuzzy = true, bool containsOrNot = true)
        {
            List<TreeNode> okNodes = new List<TreeNode>();
            var chidren = GetNodeList(node);
            foreach (var item in chidren)
            {
                if (StringUtil.IsMatch(item.Text, keyword, containsOrNot, isFuzzy))
                {
                    okNodes.Add(item);
                }
            }

            //遍历，删除没有背景的

            var depth = GetDepth(node);
            for (int i = 0; i < depth; i++)//循环次数
            {
                var tailLeaves = GetNoChildNodes(node);
                foreach (var item in tailLeaves)
                {
                    if (!okNodes.Contains(item) && item.Parent != null)
                    {
                       //   key.Parent.Nodes.Remove(key);
                      item.Remove();
                    }
                }
            }
        }


        /// <summary>
        /// 获取没有子节点的节点，常用于从底到上进行删除.
        /// 如果没有则返回自身。
        /// 如果只包含一个则是他本身,反之也成立
        /// </summary>
        /// <param name="treeId"></param>
        /// <returns></returns>
        public static List<TreeNode> GetNoChildNodes(TreeNode node)
        {
            List<TreeNode> list = new List<TreeNode>();
            if (node.Nodes.Count == 0)
            {
                list.Add(node);
                return list;
            }
            else
            {
                foreach (TreeNode n in node.Nodes)
                {
                    list.AddRange(GetNoChildNodes(n));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取节点列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TreeView"></param>
        /// <param name="loop"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static List<TreeNode> GetNodeListWithTag<T>(TreeView TreeView, bool loop = true, int maxCount = 1000)
        {
            List<TreeNode> list = new List<TreeNode>();
            var all = GetNodeList(TreeView, loop, maxCount);
            foreach (var n in all)
            {
                if ((n.Tag is T))
                {
                    list.Add(n);
                }
            }
            return list;
        }


        /// <summary>
        /// 将所有的节点以列表形式返回
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="loop"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static List<TreeNode> GetNodeList(TreeView TreeView, bool loop = true, int maxCount = 1000)
        {
            List<System.Windows.Forms.TreeNode> nodes = new List<System.Windows.Forms.TreeNode>();
            foreach (TreeNode item in  TreeView.Nodes)
            {
                nodes.Add(item);
            }
            if (nodes.Count == 0) return nodes;

            var list = Geo.Utils.TreeNodeUtil.GetNodeList(nodes, loop, maxCount);
            return list;
        }

        public static List<System.Windows.Forms.TreeNode> GetNodeList(List<System.Windows.Forms.TreeNode> nodes, bool loop = true, int maxCount = 1000)
        {
            List<System.Windows.Forms.TreeNode> list = new List<System.Windows.Forms.TreeNode>();
            foreach (var item in nodes)
            {
                var ns = GetNodeList(item, loop, maxCount);
                list.AddRange(ns);
            }
            return list; 
        }
        /// <summary>
        /// 将节点以及子节点，以列表返回。
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<System.Windows.Forms.TreeNode> GetNodeList(System.Windows.Forms.TreeNode node, bool loop = true, int maxCount = 1000)
        {
            List<System.Windows.Forms.TreeNode> list = new List<System.Windows.Forms.TreeNode>();

            list.Add(node);

            if ( loop && node.Nodes != null && node.Nodes.Count > 0)
            {
                foreach (TreeNode item in node.Nodes)
                {
                    list.AddRange(GetNodeList(item, loop, maxCount));

                    if (list.Count >= maxCount) break;
                }
            }

            return list;
        }
        /// <summary>
        /// 展开与收缩
        /// </summary>
        /// <param name="topNode"></param>
        public static void TriggerExpand(TreeNode topNode)
        {
            if (topNode == null) { return; }
            if (topNode.IsExpanded) { topNode.Collapse(); }
            else { topNode.Expand(); }
        }
        /// <summary>
        /// 展开与收缩
        /// </summary>
        /// <param name="topNode"></param>
        public static void TriggerExpandAll(TreeNode topNode)
        {
            if (topNode == null) { return; }
            if (topNode.IsExpanded) { topNode.Collapse(); }
            else { topNode.ExpandAll(); }
        }

        /// <summary>
        /// 获取第一个Tag绑定的对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="loop"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static T GetFirstTag<T>(System.Windows.Forms.TreeNode node)
        {
            var ts = GetTags<T>(node, true, 10);
            if (ts.Count > 0)
            {
                return ts[0];
            }
            return default(T);
        }     
        
        /// <summary>
        /// 获取第一个Tag绑定的对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="loop"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static T GetFirstTag<T>(List<System.Windows.Forms.TreeNode> nodes)
        {
            var ts = GetTags<T>(nodes, true, 10);
            if (ts.Count > 0)
            {
                return ts[0];
            }
            return default(T);
        }

        /// <summary>
        /// 返回当前及其子节点 Tag 类型为指定类型的对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static List<T> GetTags<T>(List<System.Windows.Forms.TreeNode> nodes, bool loop =true, int maxCount = 1000)
        { 
            List<T> ts = new List<T>();
            foreach (var node in nodes)
            {
                var t = GetTags<T>(node,loop, maxCount);
                if (t != null && t.Count > 0)
                {
                    ts.AddRange(t);
                }
            }
            return ts;
        }


        /// <summary>
        /// 返回当前及其子节点 Tag 类型为指定类型的对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<T> GetTags<T>(System.Windows.Forms.TreeNode node, bool loop = true, int maxCount = 1000)
        {
            List<T> tags = new List<T>();
            List<System.Windows.Forms.TreeNode> list = GetNodeList(node, loop, maxCount);
            foreach (var item in list)
            {
                if (item.Tag is T)
                {
                    tags.Add((T)(item.Tag));
                    if (tags.Count >= maxCount) break;
                }
            }

            return tags;
        }

        /// <summary>
        /// 展开到指定的节点
        /// </summary>
        /// <param name="node"></param>
        public static void ExpandToNode(System.Windows.Forms.TreeNode node)
        {
            if (node == null) return;
            node.Expand();
            var parent = node.Parent;
            while (parent != null)
            {
                parent.Expand();
                parent = parent.Parent;
            }
        }
    }
}
