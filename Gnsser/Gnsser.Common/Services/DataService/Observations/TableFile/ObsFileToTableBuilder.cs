//2017.04.22, czs, create in hongqing, 表生成器

using System;
using System.Collections.Generic;
using System.Text;
using Geo;
using Gnsser;
using Geo.IO;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{
    /// <summary>
    /// 表生成器 
    /// </summary>
    public class ObsFileToTableBuilder  
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public ObsFileToTableBuilder()
        {

        }

        /// <summary>
        /// 读取并构建
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private ObjectTableStorage Build(string Path)
        {
            var source = new RinexObsFileReader(Path);
            ObjectTableStorage table = new ObjectTableStorage();
            table.Name = System.IO.Path.GetFileName(Path) + FileNames.TextExcelFileExtension;
            foreach (var epochInfo in source)
            {
                ObsToRow(table, epochInfo);
            }
            return table;
        }
        /// <summary>
        /// 直接转换
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ObjectTableStorage Build(RinexObsFile file)
        { 
            ObjectTableStorage table = new ObjectTableStorage();
            table.Name = file.SiteInfo.SiteName + file.StartTime.DayOfYear.ToString("000") + file.StartTime.Hour.ToString("0") + "."+file.StartTime.SubYear + "O" + FileNames.TextExcelFileExtension;
            foreach (var epochInfo in file)
            {
                ObsToRow(table, epochInfo);
            }
            return table;
        }
        /// <summary>
        /// 转换一个历元
        /// </summary>
        /// <param name="table"></param>
        /// <param name="epochInfo"></param>
        private static void ObsToRow(ObjectTableStorage table, RinexEpochObservation epochInfo)
        {
            table.NewRow();
            //加下划线，确保排序为第一个
            table.AddItem("Epoch", epochInfo.ReceiverTime.DateTime);
            foreach (var sat in epochInfo)
            {
                foreach (var phase in sat)
                {
                    var satprn = phase.Key;
                    var val = phase.Value.Value;
                    if (Geo.Utils.DoubleUtil.IsValid(val))
                    {
                        table.AddItem(sat.Prn + "_" + phase.Key, val);
                    }
                }
            }
            table.EndRow();
        }

    }

}
