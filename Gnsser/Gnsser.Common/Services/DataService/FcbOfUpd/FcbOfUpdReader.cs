//2018.08.31, czs, create in HMX, GPS 窄巷服务器
//2018.09.12, czs, edit in hmx, 宽巷窄巷格式定义

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Times;
using Geo;
using Geo.IO;


namespace Gnsser.Service
{
    /// <summary>
    /// 宽巷窄巷格式读取，相位未校准延迟的小数部分(Fraction Code Bias of Uncalibrated phase delay)
    /// </summary>
    public class FcbOfUpdReader : LineFileReader<FcbOfUpd>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public FcbOfUpdReader(string gofFilePath, string metaFilePath = null) : base(gofFilePath, metaFilePath)
        {
            ItemSpliters = new string[] { "\t" };
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public FcbOfUpdReader(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {
            ItemSpliters = new string[] { "\t" };
        }
         
        /// <summary>
        /// 读取为文件
        /// </summary>
        /// <returns></returns>
        public FcbOfUpdFile ReadToFile()
        {
            FcbOfUpdFile file = new FcbOfUpdFile();
            file.Name = System.IO.Path.GetFileName(InputPath);
            foreach (var item in this)
            {
                if(item == null) { continue; }

                file.Add(item);
            }

            return file;
        }

        public override FcbOfUpd Parse(string[] items)
        {
            if(CurrentIndex <= 0)
            {
                Time time;
                if (!Time.TryParse(items[0], out time))
                {
                    return null;
                }
            }
            var item = new FcbOfUpd();
            int i = 0;
            item.Epoch = Time.Parse(items[i++]);
            item.WnMarker = (items[i++]);
            item.BasePrn = SatelliteNumber.Parse(items[i++]);
            item.Count = int.Parse(items[i++]);
            //item.PrnsString = (items[i++]);
            int length = items.Length;
            for (int j = i; j < length; j++)
            {
                var prn = new SatelliteNumber(j - i + 1, SatelliteType.G);
                var val = RmsedNumeral.Parse(items[j]);
                //double rms = 0.0001;
                //int rmsIndex = j + FcbOfUpdFile.TotalGpsSatCount;
                //if(items.Length > rmsIndex)
                //{
                //    rms = Double.Parse(items[rmsIndex]);
                //}

                item.Data.Add(prn, val);// new RmsedNumeral(val,rms)); 
            }
            //foreach (var prn in item.Prns)
            //{
            //    item.Data.Add(prn, Double.Parse(items[i++]));
            //}
             
            return item;
        }
    }
}
