//2015.01.29, czs, create in pengzhou, NTRIP 数据源表
//2016.10.16, czs, edit in hongqing,  加入详细信息
//2016.12.10, czs, edit in hongqing,  获取数据流字典

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo;

namespace Gnsser.Ntrip
{

    /**
     * The Ntrip Broadcaster maintains a ObsDataSource-table containing information on available Ntrip streams,
     * networks of Ntrip streams and Ntrip Broadcasters. The ObsDataSource-table is sent to an Ntrip Client on request.
     * Source-table records are dedicated to one of the following: 
     * Data Streams (record type STR), Casters (record type CAS), 
     * or Networks of streams (record type NET).
     * Source-table records of type STR contain the following satData ﬁelds: 
     * ‘mountpoint’, ‘identiﬁer’, ‘format’, ‘formatdetails’, ‘carrier’,
     * ‘nav-system’, ‘network’, ‘country’, ‘latitude’, ‘longitude’, ‘nmea’, 
     * ‘solution’, ‘generator’, ‘compr-encryp’, ‘authentication’, ‘fee’, 
     * ‘bitrate’, ‘misc’.
     * Source-tablerecordsoftypeNETcontainthefollowingdataﬁelds:
     * ‘identiﬁer’,‘operator’,‘authentication’, ‘fee’, ‘web-net’, ‘web-str’, 
     * ‘web-reg’, ‘misc’. 
     * Source-table records of type CAS contain the following satData ﬁelds: 
     * ‘host’, ‘port’, ‘identiﬁer’, ‘operator’, ‘nmea’, ‘country’, ‘latitude’, ‘longitude’, ‘misc’.
     */
    public class NtripSourceTable : BaseDictionary<SourceType, List< INtripSourceItem>>
    {
        /// <summary>
        /// 获取数据流字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, NtripStream> GetNtripStreams()
        {
            Dictionary<string, NtripStream> dic = new Dictionary<string, NtripStream>();
            var strems = Get(SourceType.Stream);
            foreach (var item in strems)
            {
                var stre = item as NtripStream;
                dic.Add(stre.Mountpoint, stre);
            } 

            return dic;
        }


        public override List<INtripSourceItem> Create(SourceType key)
        { 
            return new List<INtripSourceItem>();
        }

        public List<T> GetItems<T>( SourceType type)
            where T : INtripSourceItem
        {
            var list = Get(type);
            var streams = new List<T>();
            foreach (var item in list)
            {
                streams.Add((T)item );
            }

            return streams;
        }

        public static NtripSourceTable Load(string path)
        {
            var manager = new NtripSourceTable();
             
            using (StreamReader reader = new StreamReader(path, Encoding.Default))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                   
                    if (IsSourceLine(line))
                    {
                        INtripSourceItem item;
                        var first3Char = line.Substring(0,3);
                        var type = SourceTypeHelper.GetSourceType( first3Char);
                        switch (type)
                        {
                            case SourceType.Stream:
                                var NtripStreamItem = NtripStream.Parse(line);
                                NtripStreamItem.CasterName = Path.GetFileNameWithoutExtension(path).Replace("-table", "");
                                item = NtripStreamItem;
                                break;
                            case SourceType.Caster:
                                item = NtripCasterItem.Parse(line);
                                break;
                            case SourceType.Network:
                                item = NtripNetwork.Parse(line);
                                break;
                            default:
                                item = NtripCasterItem.Parse(line);
                                break;
                        }
                        manager.GetOrCreate(item.SourceType).Add(item);
                    } 
                }
            }
            return manager;
        }
        /// <summary>
        /// 判断是否数据源内容
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
       static public bool IsSourceLine(string line)
        {
            if (String.IsNullOrWhiteSpace(line) || line.Length < 3) { return false; }

            var first3Char = line.Substring(0, 3);
            return (SourceTypeHelper.IsSource(first3Char));
        }
    }

    /// <summary>
    /// Network
    /// </summary>
    public class NtripNetwork : Gnsser.Ntrip.INtripSourceItem
    {
        public SourceType SourceType { get { return Ntrip.SourceType.Network; } }
        public string Identiﬁer { get; set; }
        public string Operator { get; set; }
        public string Authentication { get; set; }
        public string Fee { get; set; }
        public string WebNet { get; set; }
        public string WebStr { get; set; }
        public string WebReg { get; set; }
        public string Misc { get; set; }
        public override string ToString()
        {
            return Identiﬁer;
        }
        public static NtripNetwork Parse(string line)
        {
            var item = new NtripNetwork();

            string[] fields = line.Split(';');
            string first = fields[0].ToUpper();
            if (first != "NET")
            {
                throw new NotSupportedException("字符串错误");
            }
            int i = 1;

            item.Identiﬁer = fields[i++];
            item.Operator = fields[i++];
            item.Authentication = fields[i++];
            item.Fee = fields[i++];
            item.WebNet = fields[i++];
            item.WebStr = fields[i++];
            item.WebReg = fields[i++];
            item.Misc = fields[i++];

            return item;
        }

    }

    /// <summary>
    /// CasterInfo
    /// </summary>
    public class NtripCasterItem : INtripSourceItem
    {
        public SourceType SourceType { get { return Ntrip.SourceType.Caster; } }

        public string Host { get; set; }
        public string Port { get; set; }
        public string Identiﬁer { get; set; }
        public string Operator { get; set; }
        public string Nmea { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Misc { get; set; }
        public override string ToString()
        {
            return Identiﬁer;
        }
        public static NtripCasterItem Parse(string line)
        {
            var item = new NtripCasterItem();

            string[] fields = line.Split(';');
            string first = fields[0].ToUpper();
            if (first != "CAS")
            {
                throw new NotSupportedException("字符串错误");
            }
            int i = 1;
            item.Host = fields[i++];
            item.Port = fields[i++];
            item.Identiﬁer = fields[i++];
            item.Operator = fields[i++];
            item.Nmea = fields[i++];
            item.Country = fields[i++];
            item.Latitude = fields[i++];
            item.Longitude = fields[i++];
            item.Misc = fields[i++];
            return item;
        }
    }

    //public class CasterNtripStream : NtripStream
    //{
    //    public string CasterName { get; set; }

    //}



    /// <summary>
    /// NtripStream
    /// </summary>
    public class NtripStream : INtripSourceItem
    {
        public string CasterName { get; set; }

        public SourceType SourceType { get { return Ntrip.SourceType.Stream; } }

        public string Mountpoint { get; set; }
        public string Identiﬁer { get; set; }
        public string Format { get; set; }
        public string FormatDetails { get; set; }
        public string Carrier { get; set; }
        public string NavSystem { get; set; }
        public string Network { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Nmea { get; set; }
        public string Solution { get; set; }
        public string Generator { get; set; }
        public string ComprEncryp { get; set; }
        public string Authentication { get; set; }
        public string Fee { get; set; }
        public string Bitrate { get; set; }
        public string Misc { get; set; }

        public override string ToString()
        {
            return Mountpoint;
        }

        public static NtripStream Parse(string line)
        {
            var item = new NtripStream();

            string[] fields = line.Split(';');
            string first = fields[0].ToUpper();
            if (first != "STR")
            {
                throw new NotSupportedException("字符串错误");
            }
            int i = 1;

            item.Mountpoint = fields[i++];
            item.Identiﬁer = fields[i++];
            item.Format = fields[i++];
            item.FormatDetails = fields[i++];
            item.Carrier = fields[i++];
            item.NavSystem = fields[i++];
            item.Network = fields[i++];
            item.Country = fields[i++];
            item.Latitude = fields[i++];
            item.Longitude = fields[i++];
            item.Nmea = fields[i++];
            item.Solution = fields[i++];
            item.Generator = fields[i++];
            item.ComprEncryp = fields[i++];
            item.Authentication = fields[i++];
            item.Fee = fields[i++];
            item.Bitrate = fields[i++];
            item.Misc = fields[i++];
            return item;
        }
    }



    /// <summary>
    /// 数据源表中的站点信息。
    /// </summary>
    public class CasterInfo
    {
        /// <summary>
        /// 名称，也是标示。
        /// </summary>
        public string Name { get; set; } 
         /// <summary>
        /// 数据源类型
        /// </summary>
        public SourceType SourceType { get; set; }
        public int Num11 { get; set; }
        /// <summary>
        /// 解析一行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static CasterInfo Parse(string line)
        { 
            CasterInfo CasterInfo = new Ntrip.CasterInfo();
            string[] fields = line.Split(';');
            string first = fields[0].ToUpper();
            if (first == "NET")
            {
                CasterInfo.SourceType = SourceType.Caster;
            }
            else if (first == "STR")
            {
                CasterInfo.SourceType = SourceType.Stream;
            } 
            CasterInfo.Name = fields[1];
            CasterInfo.Num11 = fields[11].ToLower().Equals("no") ? 0:Int32.Parse( fields[11]);


            return CasterInfo;
        }
    }
    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum SourceType
    {
        Stream,
        Caster,
        Network,
    }
    /// <summary>
    /// 帮助类
    /// </summary>
    public class SourceTypeHelper
    {
        public static List<string> SourceTypes = new List<string>(){"CAS", "NET", "STR"};// new List<string>( Enum.GetNames(typeof(SourceType)));
        /// <summary>
        /// 包含否
        /// </summary>
        /// <param name="threeChar"></param>
        /// <returns></returns>
        public static bool IsSource(string threeChar)
        {
            if (SourceTypes.Contains(threeChar)) return true;
            return false;
        }
        /// <summary>
        /// 获取枚举类型，同样可以用于判断
        /// </summary>
        /// <param name="first3Char"></param>
        /// <returns></returns>
        internal static SourceType GetSourceType(string first3Char)
        {
            switch (first3Char.ToUpper())
            {
                case "CAS": return SourceType.Caster;
                case "NET": return SourceType.Network;
                case "STR": return SourceType.Stream;
                default: throw new NotSupportedException(first3Char);
            }
        }
    }

    /// <summary>
    ///  NTRIP 数据源表
    /// </summary>
    public class SourceTable
    {

        public SourceTable()
        {
            this.items = new Dictionary<string, CasterInfo>();
        }
        /// <summary>
        /// 核心数据存储
        /// </summary>
        private Dictionary<string, CasterInfo> items { get; set; }
        /// <summary>
        /// 所有测站名称
        /// </summary>
        public List<string> Names { get { return new List<string>(items.Keys); } }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get { return items.Count; } }
        /// <summary>
        /// 获取指定元数据
        /// </summary>
        /// <param name="name">名称标识</param>
        /// <returns></returns>
        public CasterInfo Get(string name) { return items[name]; }
        /// <summary>
        /// 添加一个元数据
        /// </summary>
        /// <param name="prn"></param>
        public void Add(CasterInfo item)
        {
            this.items.Add(item.Name, item);
        }
        /// <summary>
        /// 从路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SourceTable Load(string path)
        {
            var text = System.IO.File.ReadAllLines(path); 
            return Parse(text);
        }

        /// <summary>
        /// 解析一个数据源表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static SourceTable Parse(string table)
        {
            string[] lines = table.Split('\n'); //Chr(13)
            return Parse(lines);
        }
        /// <summary>
        /// 解析一个数据源表
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static Ntrip.SourceTable Parse(string[] lines)
        {
            SourceTable SourceTable = new Ntrip.SourceTable(); 
            int i = 0;
            foreach (var line in lines)
            {
               // if (i < 5) { i++; continue; }//为什么到5呢？直接把以STR开头以前的都给跳过不行么？
                if (line.StartsWith("STR", StringComparison.CurrentCultureIgnoreCase)) //We found a STReam
                {
                    var item = CasterInfo.Parse(line);
                    SourceTable.Add(item);
                }

                i++;
            }
            return SourceTable;
        }

    }
}
