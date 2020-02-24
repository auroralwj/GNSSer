//2015.09.26, czs, create in xi'an hongqing, API开始了! 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;

namespace Geo.IO
{
    /// <summary>
    /// 元数据
    /// </summary>
    public class Gmetadata : Config
    {
        protected const string IO = "IO";
        protected const string Sample = "Sample";
        protected const string Default = "Default"; 

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Gmetadata() {
            Init();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        public Gmetadata(Config config)
            : this(config.Data, config.Comments)
        {
            Init();
        }

        /// <summary>
        /// 采用已有列表初始化
        /// </summary>
        /// <param name="ConfigItems"></param>
        /// <param name="Comments"></param>
        public Gmetadata(IDictionary<String, ConfigItem> ConfigItems, List<String> Comments)
        {
            this.SetData( ConfigItems);
            this.Comments = Comments;
            Init();
        }


        private void Init()
        {
        }

        /// <summary>
        /// 起始行，默认为 0 。
        /// </summary>
        public int StartRowIndex { get { return GetInt(VariableNames.StartRowIndex); } set { SetObj(VariableNames.StartRowIndex, value, Default); } }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get { return GetString(VariableNames.FileType); } set { Set(VariableNames.FileType, value); } }

        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get { return GetString(VariableNames.OutputDirectory); } set { Set(VariableNames.OutputDirectory, value , IO); } }
        /// <summary>
        /// 输出文件路径
        /// </summary>
        public string OutputFilePath { get { return GetString(VariableNames.OutputFilePath); } set { Set(VariableNames.OutputFilePath, value, IO); } }
        /// <summary>
        /// 注释标记
        /// </summary>
        public string[] CommentMarkers
        {
            get
            {
                var str = GetString(VariableNames.CommentMarker,"#");
                return str.Split(new char[]{'|'},  StringSplitOptions.RemoveEmptyEntries);
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("|");
                foreach (var item in value)
                {
                    sb.Append(item);
                    sb.Append("|");
                }
                var val = sb.ToString();

                Set(VariableNames.CommentMarker, val);
            }
        }
        /// <summary>
        /// 项目分隔符号.以 | 分割，含空格：| |
        /// </summary>
        public string[] ItemSplliter
        {
            get
            {
                var str = GetString(VariableNames.ItemSplliter);
                return str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("|");
                foreach (var item in value)
                {
                    sb.Append(item);
                    sb.Append("|");
                }
                var val = sb.ToString();

                Set(VariableNames.ItemSplliter, val);
            }
        }
        
        /// <summary>
        /// 属性名称,只返回名称。以逗号,或分号;分割。
        /// 设置的时候要注意，可以带上的单位。
        /// </summary>
        public string[] PropertyNames
        {
            get
            {
                var splitter = new char[] { ',', ';' };
                var str = GetString(VariableNames.PropertyNames);
                var strs = str.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                List<string> propertyNames = new List<string>();
                foreach (var item in strs)
                {
                    string name = GetName(item); 
                    propertyNames.Add(name);
                }
                return propertyNames.ToArray();
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                //sb.Append("|");
                int i = 0;
                foreach (var item in value)
                {
                    if (i != 0) { sb.Append(","); }

                    //if(key.Contains("(")){
                    //    var s = key.Substring(0, key.IndexOf("("));
                    //    sb.Append(key);
                    //}else{
                    sb.Append(item);
                    //}
                    //  sb.Append("|");
                    i++;
                }
                var val = sb.ToString();

                Set(VariableNames.PropertyNames, val);
            }
        }
        /// <summary>
        /// 比较单位是否相等
        /// </summary>
        public bool IsPropertyUnitChanged
        {
            get
            {
                if (DestPropertyUnits.Count == 0) return false;
                return !Geo.Utils.DictionaryUtil.IsEqual<string, Unit>(PropertyUnits, DestPropertyUnits);
            }
        }

        /// <summary>
        /// 属性的单位，通常为数值单位，如度，米等。
        /// </summary>
        public Dictionary<string, Unit> PropertyUnits
        {
            get
            {
                var strin = GetString(VariableNames.PropertyUnits);
                Dictionary<string, Unit> propertyUnits = ParsePropertyUnis(strin);
                return propertyUnits;
            }
            set
            {

                var val = ToString(value);

                Set(VariableNames.PropertyUnits, val);
            }
        }


        /// <summary>
        /// 属性的单位，通常为数值单位，如度，米等。
        /// </summary>
        public Dictionary<string, Unit> DestPropertyUnits
        {
            get
            {
                var strin = GetString(VariableNames.DestPropertyUnits);

                Dictionary<string, Unit> propertyUnits = ParsePropertyUnis(strin);
                return propertyUnits;
            }
            set
            {
                var val = ToString(value);
                Set(VariableNames.DestPropertyUnits, val);
            }
        }

        private static Dictionary<string, Unit> ParsePropertyUnis(string strin)
        {
            if (String.IsNullOrWhiteSpace(strin))
            {
                return new Dictionary<string, Unit>();
            }

            Dictionary<string, Unit> propertyUnits = new Dictionary<string, Unit>();
            var splitter = new char[] { ',', ';' };
            var strs = strin.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var str in strs)
            {
                string name;
                Unit unit = null;
                ParsePropertyUnit(str, out name, out unit);
                if (unit != null)
                {
                    propertyUnits.Add(name, unit);
                }
            }
            return propertyUnits;
        }

        private static string ToString(Dictionary<string, Unit> value)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("|");
            int i = 0;
            foreach (var item in value)
            {
                if (i != 0) { sb.Append(","); }

                sb.Append(item.Key + ":" + item.Value.Name);
                i++;
            }
            var val = sb.ToString();
            return val;
        }


        /// <summary>
        /// 只提取名称
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string GetName(string str)
        {
            string name = null;
            Unit unit = null;
            ParsePropertyUnit(str, out   name, out   unit);
            return name;
        }

        /// <summary>
        /// 可以有两种方式：
        /// X:M or X(M)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        private static void ParsePropertyUnit(string str, out string name, out Unit unit)
        {
            name = str;
            unit = null;
            if (str.Contains("("))
            {
                int i = str.IndexOf("(");
                name = str.Substring(0, i).Trim();
                var u = str.Substring(i + 1).Replace(")", "").Trim();
                unit = UnitHelper.GetUnitByName(u);
            }
            if (str.Contains(":"))
            {
                var strs = str.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                name = strs[0].Trim();
                unit = UnitHelper.GetUnitByName(strs[1].Trim());
            }
        }


        #region 默认元数据
        /// <summary>
        /// 新的实例.默认版本为1.0，分隔符为{ ",", "\t", ";", " " }
        /// </summary>
        public static Gmetadata NewInstance
        {
            get
            {
                return new Gmetadata
                {
                    Version = 1.0,
                    ItemSplliter = new string[] { "\t" }//",", "\t", ";"}
                };
            }
        }
        /// <summary>
        /// 新的实例.默认版本为1.0，分隔符为{ ",", "\t", ";", " " }
        /// </summary>
        public static Gmetadata NewInstanceWithOtherComment
        {
            get
            {
                return new Gmetadata
                {
                    Version = 1.0,
                    ItemSplliter = new string[] { "\t" },
                    CommentMarkers = new string[] { "©", }//",", "\t", ";"}
                };
            }
        }
        /// <summary>
        /// 默认的坐标元数据
        /// </summary>
        public static Gmetadata DefaultXyzMetadata
        {
            get
            {
                Gmetadata data = NewInstance;
                data.PropertyNames = new string[] { VariableNames.Id, VariableNames.X, VariableNames.Y, VariableNames.Z };

                data.PropertyUnits = new Dictionary<string, Unit>
                {
                    {VariableNames.X, Unit.Meter},
                    {VariableNames.Y, Unit.Meter},
                    {VariableNames.Z, Unit.Meter},

                }; 

                return data;
            }
        }
        /// <summary>
        /// 默认的坐标元数据
        /// </summary>
        public static Gmetadata DefaultXyMetadata
        {
            get
            {
                Gmetadata data = NewInstance;
                data.PropertyNames = new string[] { VariableNames.Id, VariableNames.X, VariableNames.Y, };

                data.PropertyUnits = new Dictionary<string, Unit>
                {
                    {VariableNames.X, Unit.Meter},
                    {VariableNames.Y, Unit.Meter}, 

                }; 

                return data;
            }
        }
        /// <summary>
        /// 默认的单值元数据
        /// </summary>
        public static Gmetadata DefaultSingleValueMetadata
        {
            get
            {
                Gmetadata data = NewInstance;
                data.PropertyNames = new string[] { VariableNames.Id, VariableNames.Value }; 
                return data;
            }
        }
        /// <summary>
        /// 默认的双值元数据
        /// </summary>
        public static Gmetadata DefaultTwoValueMetadata
        {
            get
            {
                Gmetadata data = NewInstance;
                data.PropertyNames = new string[] { VariableNames.Id, VariableNames.Value , VariableNames.Value2 }; 
                return data;
            }
        }
        /// <summary>
        /// 默认的3值元数据
        /// </summary>
        public static Gmetadata DefaultThreeValueMetadata
        {
            get
            {
                Gmetadata data = NewInstance;
                data.PropertyNames = new string[] { VariableNames.Id, VariableNames.Value , VariableNames.Value2  , VariableNames.Value3}; 
                return data;
            }
        }

        /// <summary>
        /// 默认大地坐标元数据
        /// </summary>
        public static Gmetadata DefaultLbhMetadata
        {
            get
            {
                Gmetadata data = new Gmetadata
                {
                    Version = 1.0,
                    PropertyNames = new string[] { VariableNames.Id, VariableNames.Lon, VariableNames.Lat, VariableNames.Height },

                    ItemSplliter = new string[] { ",", "\t", ";", " " },

                    PropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lon, Unit.DMS_S},
                        {VariableNames.Lat, Unit.DMS_S}, 
                        {VariableNames.Height, Unit.Meter}, 
                    },
                    DestPropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lon, Unit.DMS_S},
                        {VariableNames.Lat, Unit.DMS_S}, 
                        {VariableNames.Height, Unit.Meter}, 
                    }
                };
                return data;
            }
        }
        /// <summary>
        /// 默认大地坐标元数据
        /// </summary>
        public static Gmetadata DefaultBlhMetadata
        {
            get
            {
                Gmetadata data = new Gmetadata
                {
                    Version = 1.0,
                    PropertyNames = new string[] { VariableNames.Id, VariableNames.Lat,VariableNames.Lon,  VariableNames.Height },

                    ItemSplliter = new string[] { ",", "\t", ";", " " },

                    PropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lat, Unit.DMS_S}, 
                        {VariableNames.Lon, Unit.DMS_S},
                        {VariableNames.Height, Unit.Meter}, 
                    },
                    DestPropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lat, Unit.DMS_S}, 
                        {VariableNames.Lon, Unit.DMS_S},
                        {VariableNames.Height, Unit.Meter}, 
                    }
                };
                return data;
            }
        }

        /// <summary>
        /// 默认大地坐标元数据
        /// </summary>
        public static Gmetadata DefaultGeoCoordWithCoordSysMetadata
        {
            get
            {
                Gmetadata data = new Gmetadata
                {
                    Version = 1.0,
                    PropertyNames = new string[] { VariableNames.CoordSys, VariableNames.Id, VariableNames.Lon, VariableNames.Lat, VariableNames.Height },

                    ItemSplliter = new string[] { ",", "\t", ";", " " },

                    PropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lon, Unit.DMS_S},
                        {VariableNames.Lat, Unit.DMS_S}, 
                        {VariableNames.Height, Unit.Meter}, 
                    },      
                    DestPropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lon, Unit.DMS_S},
                        {VariableNames.Lat, Unit.DMS_S}, 
                        {VariableNames.Height, Unit.Meter}, 
                    }
                };
                return data;
            }
        }
        /// <summary>
        /// 默认基线名称文件元数据
        /// </summary>
        public static Gmetadata DefaultVectorNameMetadata
        {
            get
            {
                Gmetadata data = new Gmetadata
                {
                    Version = 1.0,
                    PropertyNames = new string[] { VariableNames.StartName, VariableNames.EndName},
                    ItemSplliter = new string[] { ",", "\t", ";", " ", "-" }
                };
                return data;
            }
        }
        #endregion

        public static Gmetadata DefaultAstroCoordMetadata
        {
            get
            {
                Gmetadata data = new Gmetadata
                       {
                           Version = 1.0,//VariableNames.ToId,
                           PropertyNames = new string[] { VariableNames.Id,  VariableNames.Lon, VariableNames.Lat},// VariableNames.Azimuth },
                           CommentMarkers = new string[] { "©" },

                           ItemSplliter = new string[] { ",", "\t", ";", " " },

                           PropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lon, Unit.DMS_S},
                        {VariableNames.Lat, Unit.DMS_S}, 
                        //{VariableNames.Azimuth, Unit.DMS_S}, 
                    },
                           DestPropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lon, Unit.HMS_S},
                        {VariableNames.Lat, Unit.DMS_S}, 
                       // {VariableNames.Azimuth, Unit.DMS_S}, 
                    }
                       };
                return data;
            }
        }
        /// <summary>
        /// 默认大地方位角元数据
        /// </summary>
        public static Gmetadata DefaultAzimuthMetadata
        {
            get
            {
                Gmetadata data = new Gmetadata
                       {
                           Version = 1.0,
                           PropertyNames = new string[] { VariableNames.Id, VariableNames.ToId, VariableNames.Azimuth },

                           ItemSplliter = new string[] { ",", "\t", ";", " " },

                           PropertyUnits = new Dictionary<string, Unit>
                    { 
                        {VariableNames.Azimuth, Unit.DMS_S}, 
                    },
                           DestPropertyUnits = new Dictionary<string, Unit>
                    { 
                        {VariableNames.Azimuth, Unit.DMS_S}, 
                    }
                       };
                return data;
            }
        }
        public static Gmetadata DefaultAstrVectorMetadata
        {
            get
            {
                Gmetadata data = new Gmetadata
                       {
                           Version = 1.0,
                           PropertyNames = new string[] { VariableNames.Id, VariableNames.ToId, VariableNames.Lon, VariableNames.Lat, VariableNames.ToLon, VariableNames.ToLat, VariableNames.Azimuth },
                           CommentMarkers = new string[] { "©" },
                           ItemSplliter = new string[] { ",", "\t", ";", " " },

                           PropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lon, Unit.DMS_S},
                        {VariableNames.Lat, Unit.DMS_S}, 
                        {VariableNames.ToLon, Unit.DMS_S},
                        {VariableNames.ToLat, Unit.DMS_S}, 
                        {VariableNames.Azimuth, Unit.DMS_S}, 
                    },
                           DestPropertyUnits = new Dictionary<string, Unit>
                    {
                        {VariableNames.Lon, Unit.HMS_S},
                        {VariableNames.Lat, Unit.DMS_S}, 
                        {VariableNames.ToLon, Unit.DMS_S},
                        {VariableNames.ToLat, Unit.DMS_S}, 
                        {VariableNames.Azimuth, Unit.DMS_S}, 
                    }
                       };
                return data;
            }
        }
    }
}
