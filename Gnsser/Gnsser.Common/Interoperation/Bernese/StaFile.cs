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
    /// Bernese STA 文件。
    /// </summary>
    public class StaFile : IBerFile
    {
        /// <summary>
        /// 项目
        /// </summary>
        public List<StaInfoItem> Items { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 统计条目数量
        /// </summary>
        public int Count { get { return Items.Count; } }
         /// <summary>
         /// 保存到路径
         /// </summary>
         /// <param name="path"></param>
        public void Save(string path)
        {
            File.WriteAllText(path, ToString());
        }

        /// <summary>
        /// 添加一个子项，如果已经存在，则不添加。 
        /// </summary>
        /// <param name="key"></param>
        public void Add(StaInfoItem item)
        {
            if (!Items.Contains(item))
            {
                Items.Add(item);
            }
        }

        /// <summary>
        ///   EXTART INFORMATION HEADER
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(StringUtil.FillSpace(Name + "  EXTART INFORMATION HEADER ", 60) + DateTime.Now);
            //  sb.AppendLine("EXAMPLE EXTART INFORMATION HEADER                                23-JAN-13 16:23");
            sb.AppendLine("--------------------------------------------------------------------------------");
            sb.AppendLine();
            sb.AppendLine("TYPE 001: RENAMING OF STATIONS");
            sb.AppendLine("------------------------------");
            sb.AppendLine();
            sb.AppendLine("STATION NAME          FLG          FROM                   TO         OLD STATION NAME      REMARK");
            sb.AppendLine("****************      ***  YYYY MM DD HH MM SS  YYYY MM DD HH MM SS  ********************  ************************");

            foreach (var item in Items)
            {
                sb.AppendLine(item.ToStaString());
            }

            sb.AppendLine();
            sb.AppendLine("TYPE 002: STATION INFORMATION");
            sb.AppendLine("-----------------------------");
            sb.AppendLine();
            sb.AppendLine("STATION NAME          FLG          FROM                   TO         RECEIVER TYPE         ANTENNA TYPE          REC #   ANT #    NORTH      EAST      UP      DESCRIPTION             REMARK");
            sb.AppendLine("****************      ***  YYYY MM DD HH MM SS  YYYY MM DD HH MM SS  ********************  ********************  ******  ******  ***.****  ***.****  ***.****  **********************  ************************");

            foreach (var item in Items)
            {
                sb.AppendLine(item.ToStaInfoString());
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("TYPE 003: HANDLING OF STATION PROBLEMS");
            sb.AppendLine("--------------------------------------");
            sb.AppendLine();
            sb.AppendLine("STATION NAME          FLG          FROM                   TO         REMARK");
            sb.AppendLine("****************      ***  YYYY MM DD HH MM SS  YYYY MM DD HH MM SS  ************************");
            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine("TYPE 004: STATION COORDINATES AND VELOCITIES (ADDNEQ)");
            sb.AppendLine("-----------------------------------------------------");
            sb.AppendLine("                                        RELATIVE CONSTR. POSITION     RELATIVE CONSTR. VELOCITY");
            sb.AppendLine("STATION NAME 1        STATION NAME 2        NORTH     EAST      UP        NORTH     EAST      UP");
            sb.AppendLine("****************      ****************      **.*****  **.*****  **.*****  **.*****  **.*****  **.*****");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("TYPE 005: HANDLING STATION TYPES");
            sb.AppendLine("--------------------------------");
            sb.AppendLine();
            sb.AppendLine("STATION NAME          FLG  FROM                 TO                   MARKER TYPE           REMARK");
            sb.AppendLine("****************      ***  YYYY MM DD HH MM SS  YYYY MM DD HH MM SS  ********************  ************************");

            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// 从 包含 O 文件的文件夹中读取、解析，并创建 StaFile 对象。
        /// </summary>
        /// <param name="oDir"></param>
        /// <returns></returns>
        public static StaFile CreateFromODir(string oDir)
        {
            StaFile file = new StaFile();
            file.Items = new List<StaInfoItem>();
            string[] files = Directory.GetFiles(oDir, Setting.RinexOFileFilter);

            foreach (var path in files)
            {
                Data.Rinex.RinexObsFileHeader h = new Data.Rinex.RinexObsFileReader(path).GetHeader();
                string name = h.MarkerName.Length > 4 ? h.MarkerName.Substring(0, 4).ToUpper() : h.MarkerName.ToUpper();
                //是否已经添加同名测站
                if (file.Items.Find(m => m.MakerName == name) != null) continue;
                StaInfoItem sta = new StaInfoItem(name, h.SiteInfo, path);
                file.Items.Add(sta);
            }
            return file;
        }

        /// <summary>
        /// 解析字符串。
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static StaFile ParseText(string txt)
        {
            //String[] lines = txt.Split(new char[] { '\r', '\north' },
            //     StringSplitOptions.RemoveEmptyEntries);
            StaFile file = new StaFile();
            file.Items = new List<StaInfoItem>();

            using (StreamReader r = new StreamReader(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(txt))))
            {
                string line = null;
                while ((line = r.ReadLine()) != null)
                {
                    //
                    //TYPE 002: STATION INFORMATION
                    //-----------------------------
                    //
                    //STATION NAME          FLG          FROM                   TO         RECEIVER TYPE         ANTENNA TYPE          REC #   ANT #    NORTH      EAST      UP      DESCRIPTION             REMARK
                    //****************      ***  YYYY MM DD HH MM SS  YYYY MM DD HH MM SS  ********************  ********************  ******  ******  ***.****  ***.****  ***.****  **********************  ************************
                    //AIRA 21742S001        001
                    //.............
                    //ZWEN 12330M001        001  1980 01 06 00 00 00  2099 12 31 00 00 00  AOA SNR-8000 ACT      AOAD/M_T                 279     342    0.0000    0.0000    0.0460                            From ZWEN1430.02O       
                    //
                    //
                    //TYPE 003: HANDLING OF STATION PROBLEMS
                    if (line.Contains("TYPE 002: STATION INFORMATION"))
                    {
                        line = r.ReadLine();
                        line = r.ReadLine();
                        line = r.ReadLine();
                        line = r.ReadLine();
                        while ((line = r.ReadLine()) != null)
                        {
                            if (line.Trim() == "" || line.Contains("TYPE 003")) break;
                            StaInfoItem item = StaInfoItem.ParseLine(line);
                            file.Items.Add(item);
                        }
                        break;
                    }
                }
            }

            return file;
        }

        /// <summary>
        /// 合并两个 STA 文件。
        /// </summary>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <returns></returns>
        public static StaFile Merger(StaFile one, StaFile another)
        {
            StaFile newOne = new StaFile();
            newOne.Name = "Mergered";
            newOne.Items = new List<StaInfoItem>();
            newOne.Items.AddRange(one.Items);

            foreach (var item in another.Items)
                newOne.Add(item);

            return newOne;
        }
         
    }

}