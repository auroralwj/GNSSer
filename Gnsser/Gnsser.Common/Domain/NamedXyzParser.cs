//2016.11.29, czs, created, 具有名称的坐标读取器
//2017.03.22, double, add in zhengzhou, GetCoordsAndTime
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
using Geo.Utils;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Geo.Data;

namespace Gnsser
{



    /// <summary>
    /// 命名XYZ坐标解析器
    /// </summary>
    public class NamedXyzParser
    {
        /// <summary>
        /// 坐标解析
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<NamedXyz> GetCoords(string path)
        {
            try
            {
                string extension = Path.GetExtension(path).ToLower();
                switch (extension)
                {
                    case ".snx":
                        SinexFile fileA = SinexReader.Read(path, true);
                        return fileA.GetSiteEstimatedCoords();
                    case ".xls":
                        return NamedXyz.ReadNamedXyz(path);
                    case ".txt":
                        return NamedXyz.ReadNamedXyztxt(path);
                    case ".org":
                        GamitOrgFileService service = new GamitOrgFileService(path);
                        return service.GetNamedXyzs();
                    case ".rep":
                        var p = new PinnacleRepFileService(path);
                        return p.GetNamedXyzs();
                    case ".pos"://"Rtklib 结果|*.pos
                        {
                            var pp = Gnsser.Interoperation.RtkpostResult.Load(path);
                            return pp.GetNamedXyzs();
                        }
                    case ".clk":
                    case ".clk_30s":
                    case ".clk_05s":
                        ClockFileHeader a = ClockFileReader.ReadHeader(path);
                        List<NamedXyz> results = new List<NamedXyz>();
                        foreach (var item in a.ClockSolnStations)
                        {
                            NamedXyz bb = new NamedXyz();
                            bb.Name = item.Name;
                            bb.Value = item.XYZ;
                            results.Add(bb);
                        }
                        return results;
                    default:
                        break;
                }
                return new List<NamedXyz>();
            }catch(Exception ex)
            {
                new Log(ex.Message).Error(ex.Message + "， 解析出错！");
                return new List<NamedXyz>();
            }
        }



        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<NamedXyzAndTime> GetCoordsAndTime(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            switch (extension)
            {
                case ".xls":
                    return NamedXyzAndTime.ReadNamedXyz(path);
                default:
                    break;
            }
            return new List<NamedXyzAndTime>();
        }
    }
}