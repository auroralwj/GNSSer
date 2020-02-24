//2016.04.27, czs create in hongqing,  名称列表管理器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 名称列表管理器,管理名称列表，名称下还有子名称，如坐标XYZ的X，Y，Z，数值和权值等。以此类推。
    /// 采用字典维护子名称。
    /// 注意：名称不可重复。
    /// </summary>
    public class NameListManager : BaseList<string>
    {
        /// <summary>
        /// 名称列表管理器
        /// </summary>
        public NameListManager()
        {
            this.SubNames = new Dictionary<string, List<string>>();
        }


        /// <summary>
        /// 名称
        /// </summary>
        public Dictionary<string, List<string>> SubNames { get; set; }

        /// <summary>
        /// 将名称及其子名称都显示为列表
        /// </summary>
        public List<string> TotalNames
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var mainName in this)
                {
                    list.Add(mainName);
                    if (SubNames.ContainsKey(mainName))
                    {
                        var subNames = SubNames[mainName];
                        foreach (var item in subNames)
                        {
                            list.Add(item);
                        }
                    }
                } 

                return list;
            }
        }

        public void Replace(string oldStr, string newStr)
        {
            var newSubNames = new Dictionary<string, List<string>>();
            foreach (var item in SubNames)
            {
                var subNames = new List<string>();
                newSubNames.Add(item.Key.Replace(oldStr, newStr), subNames);
                foreach (var sub in item.Value)
                {
                    subNames.Add(sub.Replace(oldStr, newStr));
                }
            } 
        }

        public override void Add(IEnumerable<string> val)
        {
            foreach (var item in val)
	        {
                Add(item);
	        }            
        }

        public override void Add(string name)
        { 
            Add(name, new List<string>());
        }
        public  void Add(string name, string subName)
        { 
            Add(name, new List<string>() { subName });
        }
        public  void Add(string name, List<string> subNames)
        {
            if (Contains(name)) return;

            base.Add(name);

            if (subNames.Count > 0)
            {
                SubNames.Add(name, subNames);
            }
        }

        public override void Clear()
        {
            base.Clear();
            SubNames.Clear();
        }
    }
}
