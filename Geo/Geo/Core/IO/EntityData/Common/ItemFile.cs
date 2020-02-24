//2015.06.05, czs, craete in namu, 文本文件

 using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;

namespace Geo.Data
{
   

    /// <summary>
    /// 项目文件
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class ItemFile<TItem> : IEnumerable<TItem> where TItem : IStringId 
    {
        public ItemFile()
        {
            this.Items = new List<TItem>();
        }
        /// <summary>
        /// 元素
        /// </summary>
        public List<TItem> Items { get; set; }

        public IEnumerator<TItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
