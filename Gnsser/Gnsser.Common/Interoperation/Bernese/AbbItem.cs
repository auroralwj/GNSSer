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
    /// Station name             4-ID    2-ID    Remark                            
    /// ****************         ****     **     ***************************************
    /// </summary>
    public class AbbItem
    {   
        public AbbItem()
        {            
        }
        /// <summary>
        /// 构建一个对象。
        /// </summary>
        /// <param name="makerName"></param>
        /// <param name="markerNumber"></param>
        /// <param name="items">已经有的对象列表。要求新对象不可与之相同。</param>
        public AbbItem(string makerName, string markerNumber, List<AbbItem> items)
        {
            MakerName = makerName.ToUpper();
            MakerNumber = markerNumber == null ? "" : markerNumber.ToUpper();
            Id4 = MakerName;
            Id2 = GenId2(items, makerName);
            Remark = " Added by Geo"; //+ Path.GetFileName(text)                   
        }  
        /// <summary>
        /// 标记
        /// </summary>
        public string MakerName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string MakerNumber { get; set; }
        /// <summary>
        /// 4位ID
        /// </summary>
        public string Id4 { get; set; }
        /// <summary>
        /// 2位ID
        /// </summary>
        public string Id2 { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return MakerName.GetHashCode();
        }
        /// <summary>
        /// 值是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AbbItem o = obj as AbbItem;
            if (o == null) return false;

            return MakerName.Equals(o.MakerName);
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToAbbString();
        }
        /// <summary>
        /// 简略名
        /// </summary>
        /// <returns></returns>
        public string ToAbbString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                StringUtil.FillSpace(MakerName + " " + MakerNumber, 25)
                 + StringUtil.FillSpace(Id4, 4) + "     "
                 + StringUtil.FillSpace(Id2, 2) + "     "
                 + StringUtil.FillSpace(Remark, 39));
            return sb.ToString();
        }

        /// <summary>
        /// KARR 50139M001           KARR     KA     Added by SR updabb  
        /// KAYT                     KAYT     KY     Added by SR updabb    
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static AbbItem ParseLine(string line)
        {
            AbbItem item = new AbbItem();
            item.MakerName = line.Substring(0, 4);
            item.MakerNumber = line.Substring(5, 11).Trim();
            item.Id4 = line.Substring(25, 4);
            item.Id2 = line.Substring(34, 2);
            item.Remark = line.Substring(41).Trim();
            return item;
        }


        /// <summary>
        /// 生成一个独一无二的ID2.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="markerName"></param>
        /// <returns></returns>
        public static string GenId2(List<AbbItem> items, string markerName)
        {
            string name = markerName.ToUpper();
            string id2 = "";
            foreach (var item in name)
            {
                foreach (var item2 in name)
                {
                    id2 = (item.ToString() + item2.ToString()).ToUpper();
                    if (items.Find(m => m.Id2 == id2) == null)
                        return id2;
                }
            }

            foreach (var item in name)
            {
                for (int i = 0; i < 255; i++)
                {
                    char c = ((char)(i));

                    if (Char.IsNumber(c) || char.IsUpper(c))
                    {
                        id2 = (item.ToString() + c.ToString()).ToUpper();
                        if (items.Find(m => m.Id2 == id2) == null)
                            return id2;
                    }
                }
            }
            for (int j = 0; j < 255; j++)
            {
                char jc = (char)j;
                if (Char.IsNumber(jc) || char.IsUpper(jc))

                    for (int i = 0; i < 255; i++)
                    {
                        char c = ((char)(i));

                        if (Char.IsNumber(c) || char.IsUpper(c))
                        {
                            id2 = (jc.ToString() + c.ToString()).ToUpper();
                            if (items.Find(m => m.Id2 == id2) == null)
                                return id2;
                        }
                    }
            }
            return "XX";
        }

    }
}