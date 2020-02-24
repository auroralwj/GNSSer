//2017.10, lly, create in zz, 对流层服务
//2017.11.10, czs, edit in hongqing, 服务重构合并

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Data;
using System.IO;

namespace Gnsser
{
    /// <summary>
    /// 对流层服务
    /// </summary>
    public class TropAugmentInfoReader
    {
        Log log = new Log(typeof(TropAugmentInfoReader));
        /// <summary>
        /// 对流层服务
        /// </summary>
        /// <param name="filePath"></param>
        public TropAugmentInfoReader(string filePath)
        {
            this.FilePath = filePath;
        }
        string FilePath { get; set; }

        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public List<TropAugmentValue> read()
        {
            List<TropAugmentValue> TropAugmentValues = new List<TropAugmentValue>();
            if (!File.Exists(FilePath)) {
                log.Error("尝试读取对流层增强文件，但指定路径不存在啊！ " + FilePath);
                return TropAugmentValues; 
            }

            bool isEnd = true;
            using (StreamReader sr = new StreamReader(FilePath))
            {
                while (isEnd)
                {
                    string line = sr.ReadLine();
                    if (line == null || line == "")
                        break;
                    if (line.Length > 255)
                    {
                        throw new Exception("Line too long");
                    }
                    if (line.Length == 0)
                    {
                        isEnd = false;
                    }
                    Time GpsTime = Time.MinValue;
                    GpsTime = Time.Parse(line.Substring(0, 19));
                    double zwd = double.Parse(line.Substring(20, 15));
                    TropAugmentValue tropaugval = new TropAugmentValue(GpsTime, zwd);
                    TropAugmentValues.Add(tropaugval);
                }
            }
            log.Info("对流层增强文件读取成功！共" + TropAugmentValues .Count + " 条，"+ FilePath);
            return TropAugmentValues;
        }
    }
}
