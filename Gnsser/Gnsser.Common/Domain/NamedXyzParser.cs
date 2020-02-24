//2016.11.29, czs, created, �������Ƶ������ȡ��
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
    /// ����XYZ���������
    /// </summary>
    public class NamedXyzParser
    {
        /// <summary>
        /// �������
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
                    case ".pos"://"Rtklib ���|*.pos
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
                new Log(ex.Message).Error(ex.Message + "�� ��������");
                return new List<NamedXyz>();
            }
        }



        /// <summary>
        /// ��ȡ
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