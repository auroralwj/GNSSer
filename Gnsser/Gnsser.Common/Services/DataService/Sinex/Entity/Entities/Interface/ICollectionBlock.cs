using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Gnsser.Data.Sinex
{
    public interface ICollectionBlock<T> : IBlock where T : IBlockItem, new()
    {
        /// <summary>
        /// 包含元素的数量
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 是否具有元素。
        /// </summary>
        bool HasItems { get; }
        /// <summary>
        /// 包含的元素集合
        /// </summary>
        List<T> Items { get; set; }
        string ToString(string label);
    }
     
}
