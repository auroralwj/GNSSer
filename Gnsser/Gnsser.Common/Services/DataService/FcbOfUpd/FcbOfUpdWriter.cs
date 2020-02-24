//2018.08.31, czs, create in HMX, GPS 窄巷服务器
//2018.09.12, czs, edit in hmx, 宽巷窄巷格式定义

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Geo.Times;
using Geo;
using Geo.IO;


namespace Gnsser.Service
{
    /// <summary>
    /// 宽巷窄巷格式写入，相位未校准延迟的小数部分(Fraction Code Bias of Uncalibrated phase delay)
    /// </summary>
    public class FcbOfUpdWriter : LineFileWriter<FcbOfUpd>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public FcbOfUpdWriter(string gofFilePath, string metaFilePath = null) : base(gofFilePath, metaFilePath)
        {
            ItemSpliter = '\t'; 
            this.PropertieNames = FcbOfUpdFile.BuildTitles();

            this.WriteHeaderLine();
            //this.WriteCommentLine("Epoch"
            //    + ItemSpliter + "WnMarker"
            //    + ItemSpliter + "BasePrn"
            //    + ItemSpliter + "Count"
            //    + ItemSpliter + "G01" + ItemSpliter + "G02" + ItemSpliter + "G03" + ItemSpliter + "G04" + ItemSpliter + "G05"
            //    + ItemSpliter + "G06" + ItemSpliter + "G07" + ItemSpliter + "G08" + ItemSpliter + "G09" + ItemSpliter + "G10"
            //    + ItemSpliter + "G11" + ItemSpliter + "G12" + ItemSpliter + "G13" + ItemSpliter + "G14" + ItemSpliter + "G15"
            //    + ItemSpliter + "G16" + ItemSpliter + "G17" + ItemSpliter + "G18" + ItemSpliter + "G19" + ItemSpliter + "G20"
            //    + ItemSpliter + "G21" + ItemSpliter + "G22" + ItemSpliter + "G23" + ItemSpliter + "G24" + ItemSpliter + "G25"
            //    + ItemSpliter + "G26" + ItemSpliter + "G27" + ItemSpliter + "G28" + ItemSpliter + "G29" + ItemSpliter + "G30" 
            //    + ItemSpliter + "G31" + ItemSpliter + "G32"
            //    );
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public FcbOfUpdWriter(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {
            ItemSpliter = '\t';
        }

        //protected override void SetProperties()
        //{
        //    this.EntityType = typeof(FcbOfUpd);
        //    Properties = new List<System.Reflection.PropertyInfo>();

        //    Properties.Add(typeof(Time).);

        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in Properties.)
        //    {
        //        Properties.Add(EntityType.GetProperty(item.Trim()));
        //    }
        //}

        /// <summary>
        ///   "Epoch", "BasePrn", "Count", "WnMarker",
        ///        "G01", "G02",  "G03", "G04",  "G05", "G06",  "G07", "G08",  "G09",
        ///"G10",  "G11", "G12",  "G13", "G14",  "G15", "G16",  "G17", "G18",  "G19",
        ///"G20",  "G21", "G22",  "G23", "G24",  "G25", "G26",  "G27", "G28",  "G29",
        ///"G30",  "G31", "G32",
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override string EntityToLine(FcbOfUpd obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(obj.Epoch.ToString());
            sb.Append(ItemSpliter);
            sb.Append(obj.WnMarker);
            sb.Append(ItemSpliter);
            sb.Append(obj.BasePrn.ToString());
            sb.Append(ItemSpliter);
            sb.Append(obj.Count.ToString());
            //sb.Append(ItemSpliter);
            //sb.Append(obj.PrnsString.ToString()); 
            var prns = SatelliteNumber.GpsPrns;
            foreach (var item  in prns)
            { 
                sb.Append(ItemSpliter);
                var val = obj.Get(item);
                if(RmsedNumeral.IsValid( val))
                {
                    sb.Append(val.ToString("0.0000"));
                }
                else
                {
                    sb.Append(val);
                }
            }
            return sb.ToString();
        }
         /// <summary>
         /// 写入临时目录
         /// </summary>
         /// <param name="list"></param>
         /// <param name="name"></param>
        public static String WriteEpochProducts(List<FcbOfUpd> list, string name = "EpochNLFcbOfDcb")
        {
            var toPath = Path.Combine(Setting.TempDirectory, name + Gnsser.Setting.FcbExtension);
            FcbOfUpdWriter writer = new FcbOfUpdWriter(toPath);

            foreach (var fcb in list)
            {
                if (fcb == null) { continue; }
                writer.Write(fcb);
            }
            writer.Dispose();
            return toPath;
        }
    }
}
