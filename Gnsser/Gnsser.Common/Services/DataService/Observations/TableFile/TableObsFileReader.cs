//2017.04.21.16, czs, create in hongqing, 表读取器

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
    /// 表读取器 
    /// </summary>
    public class TableObsFileReader : AbstractEnumer<RinexEpochObservation> //: AbstractReader<RinexObsFile>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public TableObsFileReader(string path)//:base()
        {
            this.Path = path;
            this.TableToRinexObsFileBuilder = new TableToRinexObsFileBuilder();
        }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        ObjectTableStorage Table { get; set; }
        /// <summary>
        /// 转换器
        /// </summary>
        public TableToRinexObsFileBuilder TableToRinexObsFileBuilder { get; set; }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public RinexObsFile Read()
        {
            ObjectTableReader reader = new ObjectTableReader(Path);

            Table = reader.Read();

            RinexObsFile obsFile = new RinexObsFile();
            obsFile.Header = TableToRinexObsFileBuilder.BuidHeader(Table);
            var indexColName = Table.GetIndexColName();

            foreach (var row in Table.BufferedValues)
            {
                obsFile.Add(TableToRinexObsFileBuilder.BuildObs(row, indexColName));
            }
            return obsFile;
        }

        public override bool MoveNext()
        {
            if (base.MoveNext())
            {
                var indexColName = Table.GetIndexColName();
                var row = Table.BufferedValues[CurrentIndex];
                this.Current = TableToRinexObsFileBuilder.BuildObs(row, indexColName);
            }
            return false;
        }


        public override void Dispose()
        {
            if (Table != null) { Table.Dispose(); }
        }

        public override void Reset()
        {
            this.CurrentIndex = 0;
        }
    }

    /// <summary>
    /// 表格文件向RINEX观测对象的转换器
    /// </summary>
    public class TableToRinexObsFileBuilder
    {

      /// <summary>
        /// 构建观测历元。
        /// </summary>
        /// <param name="row"></param>
        /// <param name="indexColName"></param>
        /// <returns></returns>
        public RinexEpochObservation BuildObs(Dictionary<String, object> row, string indexColName)
        {
            var obs = new RinexEpochObservation();
            obs.ReceiverTime = new Geo.Times.Time((DateTime)row[indexColName]);
           foreach (var item in row)
           {
               var name = item.Key;
               if (name == indexColName) { continue; }

               var code = name.Substring(4);
               var prn = SatelliteNumber.Parse(name);

               if (!obs.Contains(prn)) { obs[prn] = new RinexSatObsData(prn); }
               var obsValue = new RinexObsValue((double)item.Value, code);
               obs[prn].Add(code, obsValue); 
           }
           return obs;
        }


        /// <summary>
        /// 构建头部信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public RinexObsFileHeader BuidHeader(ObjectTableStorage table)
        {
            var colNames = table.ParamNames;
            var indexName = table.GetIndexColName();
            var secondColName = colNames[1];


            var startEpochTime = (DateTime)table.FirstIndex;
            var endEpoch = (DateTime)table.LastIndex;
            var interval = (int)((DateTime)table.SecondIndex - startEpochTime).TotalSeconds;
            RinexObsFileHeader header = new RinexObsFileHeader();
            header.Interval = interval;
            header.StartTime = new Geo.Times.Time(startEpochTime);
            header.EndTime = new Geo.Times.Time(endEpoch);
            header.Version = secondColName.Length >= 7 ? 3.02 : 2.11;
            header.Comments.Add("Build from " + table.Name);
            var obsCodes = new Dictionary<SatelliteType, List<string>>();
            foreach (var name in colNames)
            {
                if (indexName == name) { continue; }
                var prn = SatelliteNumber.Parse(name);
               if(!obsCodes.ContainsKey(prn.SatelliteType)){obsCodes[prn.SatelliteType] = new List<string>();}
                  var list = obsCodes[prn.SatelliteType];

                var code = name.Substring(4);

                if (!list.Contains(code)) { list.Add(code); }
            }
            header.ObsCodes = obsCodes;
            header.MarkerType = obsCodes.Count > 1 ? "M" : header.SatelliteTypes[0]+"";
            header.SiteInfo.SiteName = table.Name.Substring(0, 4);


            return header;
        }


    }

}
