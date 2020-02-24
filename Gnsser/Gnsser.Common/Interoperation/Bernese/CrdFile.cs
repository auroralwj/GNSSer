using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Interoperation.Bernese
{
    /// <summary>
    /// Bernese 坐标文件。
    /// </summary>
    public class CrdFile : IBerFile
    {
        /// <summary>
        /// 说明或其它信息。
        /// </summary>
        public List<string> Comments { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 时间字符串
        /// </summary>
        public string DateString { get; set; }

        /// <summary>
        /// 历元。
        /// </summary>
        public Time Epoch { get; set; }

        /// <summary>
        /// 基准
        /// </summary>
        public string Datum { get; set; }

        /// <summary>
        /// 坐标集合
        /// </summary>
        public List<CrdItem> Items { get; set; }

        /// <summary>
        /// 统计条目数量
        /// </summary>
        public int Count { get { return Items.Count; } }
        public void Add(CrdItem item)
        {
            if (!Items.Contains(item))
            {
                item.Num += 1;
                this.Items.Add(item);
            }
        } 

        /// <summary>
        /// 将另一个文件的内容添加进来，两个文件合并成一个文件。
        /// </summary>
        /// <param name="file"></param>
        /// <param name="strict"></param>
        /// <returns></returns>
        public void AddRange(CrdFile file, bool strict = false)
        {
            if (strict && (Datum != file.Datum || !this.Epoch.Equals(file.Epoch))) throw new ArgumentException("两个文件历元或基准不一样。");
            int num = this.Items.Count;
            foreach (var item in file.Items)
            {
                if (!Items.Contains(item))
                {
                    item.Num += num;
                    this.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 写入文件。
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {            
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(ToString());
            }
        }

        public  override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            string line = "";
            line = Label;
            line = StringUtil.FillSpace(line, 66);
            line += DateString;
            sb.AppendLine(line);
            sb.AppendLine("--------------------------------------------------------------------------------");
            line = "LOCAL GEODETIC DATUM: " + this.Datum;
            line = StringUtil.FillSpace(line, 40);
            line += "EPOCH: " + this.Epoch;
            sb.AppendLine(line);
            sb.AppendLine();
            sb.AppendLine("NUM  STATION NAME           X (M)          Y (M)          Z (M)     FLAG");
            sb.AppendLine();
            foreach (CrdItem coord in Items)
            {
                sb.AppendLine(coord.ToLine());
            }
            return sb.ToString();
        }
      
        /// <summary>
        /// 读取文本。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CrdFile ParseText(string text)
        {
            using (MemoryStream stream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(text)))
            {
                return Read(stream);
            }
        }

        /// <summary>
        /// 读取Bernese坐标文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static CrdFile Read(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return Read(stream);
            }
        }

        public static CrdFile Read(Stream stream)
        {
            CrdFile file = new CrdFile();
            if (file.Comments == null) file.Comments = new List<string>();

            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                //第一行
                //PPP_021430_143MATE                                               24-JAN-13 09:21
                line = reader.ReadLine();
                file.Label = line.Substring(0, 65).Trim();
                file.DateString = line.Substring(65).Trim();

                //第二行
                //--------------------------------------------------------------------------------
                line = reader.ReadLine();

                //第3行
                //LOCAL GEODETIC DATUM: IGS00             EPOCH: 2002-05-23 11:57:30
                line = reader.ReadLine();
                if (line.Contains("DATUM"))
                {
                    file.Datum = line.Substring(line.IndexOf(":") + 1, 15).Trim();
                }
                if (line.Contains("EPOCH"))
                {
                    file.Epoch = new Time(DateTime.Parse(line.Substring(line.IndexOf("EPOCH") + 6)));
                }

                while ((line = reader.ReadLine()) != null)
                {
                    //正式记录
                    if (line.StartsWith("NUM"))
                    {
                        file.Items = new List<CrdItem>();
                        line = reader.ReadLine();//空了一行。
                        while (line != null)
                        {
                            line = line.Trim();
                            if (line == "")
                            {
                                line = reader.ReadLine();
                                continue;
                            }

                            CrdItem coord = CrdItem.ParseLine(line);
                            file.Items.Add(coord);

                            line = reader.ReadLine();
                            if (line == "") break;//跳出
                        }

                        //末尾有些东西
                        while ((line = reader.ReadLine()) != null)
                        {
                            file.Comments.Add(line);

                        }
                    }
                }
                return file;
            }
        }
        /// <summary>
        /// 从O文件夹创建
        /// </summary>
        /// <param name="oDir"></param>
        /// <returns></returns>
        public static CrdFile CreateFromODir(string oDir)
        { 
            CrdFile file = new CrdFile()
            {
                Comments = new List<string>(),
                DateString = DateTime.Now.ToString(),
                Datum = "IGS00",
                Epoch = Time.Parse("2000-01-01 00:00:00"),
                Items = new List<CrdItem>(),
                Label = "IGS00 COORDINATES BASED ON SINEX O Files"
            };

            file.Items = new List<CrdItem>();
            string[] files = Directory.GetFiles(oDir, Setting.RinexOFileFilter);
            int num =1;
            foreach (var item in files)
            {
                Data.Rinex.RinexObsFileHeader h = new Data.Rinex.RinexObsFileReader(item).GetHeader();
                //判断是否已经存在。
                string makerName = StringUtil.FillZero( h.MarkerName.ToUpper(), 4).Substring(0,4);
                if (file.Items.Find(m => m.StationName == makerName) != null) continue;

                string code = h.SiteInfo.MarkerNumber;

                file.Items.Add(new CrdItem(num++, makerName, code, h.ApproxXyz));
            }

            return file;
        }

        /// <summary>
        /// 合并两个 ABB 文件。
        /// </summary>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <returns></returns>
        public static CrdFile Merger(CrdFile one, CrdFile another)
        {
            CrdFile newOne = new CrdFile();
            newOne.DateString = DateTime.Now + "";
            newOne.Datum = one.Datum;
            newOne.Epoch = one.Epoch;
            newOne.Label = "Merged";            
            newOne.Items = new List<CrdItem>();
            newOne.Items.AddRange(one.Items);

            foreach (var item in another.Items)
                newOne.Add(item);

            return newOne;
        }

    }

}
