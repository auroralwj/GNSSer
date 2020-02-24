//2016.12.26 czs & cuiyang, created, 坐标文件写入器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Common;
using System.IO;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;


namespace Geo.Coordinates
{ 
    /// <summary>
    /// 坐标文件写入器
    /// </summary>
    public class GnsserXyzCoordWriter : LineFileWriter<GnsserXyzCoord>
    { /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public GnsserXyzCoordWriter(string gofFilePath, string metaFilePath = null)
            : base(gofFilePath, metaFilePath)
        {
            //默认路径是 ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public GnsserXyzCoordWriter(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {
            //默认路径是 ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }
        /// <summary>
        /// 构造函数，以数据流初始化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="Gmetadata"></param>
        public GnsserXyzCoordWriter(Stream stream, Gmetadata Gmetadata)
            : base(stream, Gmetadata)
        {
        }

        protected override void InitStreamWriter(Stream Stream, Encoding Encoding)
        {
            base.InitStreamWriter(Stream, Encoding); 
        }
    }
    /// <summary>
    /// 坐标文件写入器
    /// </summary>
    public class GnsserXyzCoordReader : LineFileReader<GnsserXyzCoord>
    { /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public GnsserXyzCoordReader(string gofFilePath, string metaFilePath = null)
            : base(gofFilePath, metaFilePath)
        {
           
            //默认路径是 ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public GnsserXyzCoordReader(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {
            //默认路径是 ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        } 
        protected override void Init(Gmetadata Gmetadata)
        {
            base.Init(Gmetadata);
            this.ItemSpliters = new string[] { "\t" };
            StartIndex = 1;
        }
    }
    /// <summary>
    /// GNSSer坐标文件
    /// </summary>
    public class GnsserXyzCoord : IOrderedProperty
    {
        public GnsserXyzCoord()
        {
            OrderedProperties = new List<string>() { "CaculateTime", "SiteName", "ReceivingTime", "EstX", "EstY", "EstZ", "RmsX", "RmsY", "RmsZ", "FileName", "Info" };
        } 
        /// <summary>
        /// 标识符,可以自定义，作为全局唯一标识。
        /// </summary>
        public virtual string Id { get { return SiteName.ToUpper() + Geo.Utils.StringUtil.SubString(FileName.ToUpper(), 0, 4); } }

        public DateTime CaculateTime { get; set; }
        public DateTime ReceivingTime { get; set; }
        public string SiteName { get; set; }
        public string FileName { get; set; }
        public string Info { get; set; }

        public double EstX { get; set; }
        public double EstY { get; set; }
        public double EstZ { get; set; }
        public double RmsX { get; set; }
        public double RmsY { get; set; }
        public double RmsZ { get; set; }

        public XYZ EstXyz { get { return new XYZ(EstX, EstY, EstZ); } set { this.EstX = value.X; this.EstY = value.Y; this.EstZ = value.Z; } }

        public XYZ RmsXyz { get { return new XYZ(RmsX, RmsY, RmsZ); } set { this.RmsX = value.X; this.RmsY = value.Y; this.RmsZ = value.Z; } }
         
        public List<string> OrderedProperties { get; set; }

        public List<ValueProperty> Properties { get; set; }

        public override bool Equals(object obj)
        {
            return Id.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id;
        }

        public static GnsserXyzCoord Average(List<GnsserXyzCoord> coords)
        {
            var last = coords[coords.Count - 1];
            GnsserXyzCoord coord = new GnsserXyzCoord()
            {
                FileName = last.FileName,
                CaculateTime = DateTime.Now,
                ReceivingTime = last.ReceivingTime,
                SiteName = last.SiteName,
                Info = "Count=" + coords.Count
            };
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var item in coords)
            {
                table.NewRow();
                table.AddItem("X", item.EstX);
                table.AddItem("Y", item.EstY);
                table.AddItem("Z", item.EstZ);
                table.EndRow();
            }
            var resultDic = table.GetAveragesWithStdDev();
            XYZ xyz = new XYZ(resultDic["X"][0], resultDic["Y"][0], resultDic["Z"][0]);
            XYZ rms = new XYZ(resultDic["X"][1], resultDic["Y"][1], resultDic["Z"][1]);
            coord.EstXyz = xyz;
            coord.RmsXyz = rms;

            return coord;
        }
    }
}