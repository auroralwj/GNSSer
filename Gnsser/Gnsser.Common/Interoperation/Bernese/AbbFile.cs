using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;

namespace Gnsser.Interoperation.Bernese
{
    /// <summary>
    /// Bernese Station nambe abbreviation fileB.
    /// </summary>
    public class AbbFile : Gnsser.Interoperation.Bernese.IBerFile
    {
        public AbbFile() { this.Name = "AbbFile"; }

        /// <summary>
        /// ABB 文件项目列表。
        /// </summary>
        public List<AbbItem> Items { get; set; }
        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 统计条目数量
        /// </summary>
        public int Count { get { return Items.Count; } }
        /// <summary>
        /// 添加一个子项，如果已经存在，则不添加。
        /// 若添加，还要保证ID2没有重复。
        /// </summary>
        /// <param name="key"></param>
        public void Add(AbbItem item)
        {
            if (!Items.Contains(item))
            {
                if (Items.Find(m => m.Id2 == item.Id2) != null)
                    item.Id2 = AbbItem.GenId2(Items, item.MakerName);
                Items.Add(item);
            }
        }


        /// <summary>
        /// 保存到文件。
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            File.WriteAllText(path, ToString());
        }
        /// <summary>
        /// PPP_021430                                                       17-JAN-13 10:24
        /// --------------------------------------------------------------------------------
        /// 
        /// Station name             4-ID    2-ID    Remark                            
        /// ****************         ****     **     ***************************************
        /// AIRA 21742S001           AIRA     AI     Added by SR updabb                     
        /// AJAC                     AJA0     AA     Added by SR updabb                     
        /// AJAC 10077M005           AJAC     AJ     Added by SR updabb       
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(StringUtil.FillSpace(Name, 60) + DateTime.Now);
            sb.AppendLine("--------------------------------------------------------------------------------");
            sb.AppendLine();
            sb.AppendLine("****************         ****     **     ***************************************");
            sb.AppendLine("Station oFileName             4-ID    2-ID    Remark                            ");
            foreach (var item in Items)
            {
                sb.AppendLine(item.ToAbbString());
            }
            return sb.ToString();
        }        
        
        /// <summary>
        /// 从包含O文件的文件夹中提取。
        /// </summary>
        /// <param name="oDir"></param>
        /// <returns></returns>
        public static AbbFile CreateFromODir(string oDir)
        {
            AbbFile file = new AbbFile();
            file.Items = new List<AbbItem>();
            string[] files = Directory.GetFiles(oDir, Setting.RinexOFileFilter);

            foreach (var item in files)
            {
                Data.Rinex.RinexObsFileHeader h = new Data.Rinex.RinexObsFileReader(item).GetHeader();
                //判断是否已经存在。
                string makerName = StringUtil.FillZero(h.SiteInfo.SiteName.ToUpper(), 4).Substring(0, 4);
                if (file.Items.Find(m => m.MakerName == makerName) != null) continue;

                AbbItem sta = new AbbItem(makerName, h.SiteInfo.MarkerNumber, file.Items);

                file.Items.Add(sta);
            }
            return file;
        }

        /// <summary>
        /// 合并两个 ABB 文件。
        /// </summary>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <returns></returns>
        public static AbbFile Merger(AbbFile one, AbbFile another)
        {
            AbbFile newOne = new AbbFile();
            newOne.Name = "Mergered";
            newOne.Items = new List<AbbItem>();
            newOne.Items.AddRange(one.Items);

            foreach (var item in another.Items)
                newOne.Add(item);

            return newOne;
        }
        /// <summary>
        /// 解析字符串。
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static AbbFile ParseText(string txt)
        {
            String[] lines = txt.Split(new char[] { '\r', '\n' },
                 StringSplitOptions.RemoveEmptyEntries);
            AbbFile file = new AbbFile();
            file.Items = new List<AbbItem>(); 
            int count = 0;
            foreach (var line in lines)
            {
                count++;
                if (count < 6) continue;

                AbbItem item = AbbItem.ParseLine(line);
                file.Items.Add(item);
            }
            return file;
        }
        /// <summary>
        /// 从文件中读取。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AbbFile Parse(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                AbbFile file = new AbbFile();
                file.Items = new List<AbbItem>();
                string line = null;
                int count = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    count ++;
                    if (count < 6) continue;

                    AbbItem item = AbbItem.ParseLine(line);
                    file.Items.Add(item);
                }
                return file;
            }
        }

    }
}