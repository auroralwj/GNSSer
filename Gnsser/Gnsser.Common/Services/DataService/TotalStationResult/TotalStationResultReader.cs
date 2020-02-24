//2019.01.09, czs, create in hmx, 全站仪结果

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Algorithm;
using System.Threading.Tasks;
using Geo.Times;
using Geo.Coordinates;

namespace Gnsser.Data
{
    //https://surveyequipment.com/assets/index/download/id/221/

    /// <summary>
    /// 文件
    /// </summary>
    public class TotalStationResultReader
    {
        public TotalStationResultReader(string path)
        {
            this.FilePath = path;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns></returns>
        public TotalStationResultFile Read()
        {
            TotalStationResultFile file = new TotalStationResultFile();
            file.Name = Path.GetFileName(FilePath);

            string line = null;
            using (StreamReader reader = new StreamReader(FilePath, Encoding.Default))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("----------------") ||String.IsNullOrWhiteSpace(line)) { continue; }

                    if(line == null) { break; }
                    if (line.Contains("Name          X(m)            Y(m)"))//近似坐标
                    {
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        bool isStarted = false;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var items = Geo.Utils.StringUtil.Split(line, new char[] { ' ' }, true);
                            if (items.Length < 3)
                            {
                                if (isStarted) { break; }
                                continue;
                            }
                            isStarted = ParseApproxXY(file, isStarted, items);
                            if (line.Contains("----------------") && isStarted) { break; }
                        }
                    }

                    if (line == null) { break; }
                    if (line.Contains("FROM      TO  TYPE      VALUE(dms)  M(sec)  V(sec)    RESULT(dms)     Ri"))//方向平差结果
                    {
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        var isStarted = false;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var items = Geo.Utils.StringUtil.Split(line, new char[] { ' ' }, true);
                            if (items.Length < 8)
                            {
                                if (isStarted) { break; }
                                continue;
                            }
                            isStarted = ParseDirectionLine(file, isStarted, items);
                            if (line.Contains("----------------") && isStarted) { break; }
                        }
                    }


                    if (line == null) { break; }
                    if (line.Contains("FROM      TO  TYPE     VALUE(m)   M(cm)   V(cm)    RESULT(m)     Ri"))//距离平差结果
                    {
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        var isStarted = false;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var items = Geo.Utils.StringUtil.Split(line, new char[] { ' ' }, true);
                            if (items.Length < 8)
                            {
                                if (isStarted) { break; }
                                continue;
                            }
                            isStarted = ParseDistanceLine(file, isStarted, items);
                            if (line.Contains("----------------") && isStarted) { break; }
                        }  
                    }


                    if (line == null) { break; }
                    if (line.Contains("Name            X(m)             Y(m)   MX(cm)  MY(cm) MP(cm) E(cm)  F(cm)   T(dms)"))//平差坐标及其精度
                    {
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        var isStarted = false;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var items = Geo.Utils.StringUtil.Split(line, new char[] { ' ' }, true);
                            if (items.Length < 3)
                            {
                                if (isStarted) { break; }
                                continue;
                            }
                            isStarted = ParseAdjustCoordLine(file, isStarted, items);
                            if (line.Contains("----------------") && isStarted) { break; }
                        }  
                    }


                    if (line == null) { break; }
                    if (line.Contains("FROM       TO       A(dms)     MA(sec)     S(m)      MS(cm)  S/MS     E(cm)   F(cm)     T(dms)"))//网点间边长、方位角及其相对精度
                    {
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        //line = reader.ReadLine();
                        var isStarted = false;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var items = Geo.Utils.StringUtil.Split(line, new char[] { ' ' }, true);
                            if (items.Length < 10)
                            {
                                if (isStarted) { break; }
                                continue;
                            }
                            isStarted = ParseCombinedLine(file, isStarted, items);
                            if (line.Contains("----------------") && isStarted) { break; }
                        }  
                    } 
                }
            }

            return file;
        }

        private static bool ParseApproxXY(TotalStationResultFile file, bool isStarted, string[] items)
        {
            var name = items[0];
            var x = Geo.Utils.StringUtil.ParseDouble(items[1]);
            var y = Geo.Utils.StringUtil.ParseDouble(items[2]);
            if (x != 0 && y != 0)
            {
                isStarted = true;
                file.ApproxCoords[name] = new XY(x, y);
            }

            return isStarted;
        }
        private static bool ParseCombinedLine(TotalStationResultFile file, bool isStarted, string[] items)
        {
            var startName = items[0];
            var endName = items[1];
            var lineName = new GnssBaseLineName(endName, startName);
            var Azimuth = Geo.Utils.StringUtil.ParseDouble(items[2]);
            var MA = Geo.Utils.StringUtil.ParseDouble(items[3]);
            var S = Geo.Utils.StringUtil.ParseDouble(items[4]);
            var MS = Geo.Utils.StringUtil.ParseDouble(items[5]);
            var SPerMS = Geo.Utils.StringUtil.ParseDouble(items[6]);
            var E = Geo.Utils.StringUtil.ParseDouble(items[7]);
            var F = Geo.Utils.StringUtil.ParseDouble(items[8]);
            var T = Geo.Utils.StringUtil.ParseDouble(items[9]);
             

            var azi = new DMS(Azimuth, AngleUnit.D_MS).Degrees;
            var t = new DMS(T, AngleUnit.D_MS).Degrees;


            if ( S != 0 && SPerMS != 0 && E != 0)
            {
                isStarted = true;
                file.CombinedResults[lineName] = new TotalStationCombinedResult(lineName)
                {
                    Azimuth = azi,
                    MA = MA,
                    Distance = S,
                    MS = MS,
                    DisPerMs = SPerMS,
                    E = E,
                    F = F,
                    T = t
                };
            }
            return isStarted;
        }

        private static bool ParseAdjustCoordLine(TotalStationResultFile file, bool isStarted, string[] items)
        {
            var name = items[0];
            var x = Geo.Utils.StringUtil.ParseDouble(items[1]);
            var y = Geo.Utils.StringUtil.ParseDouble(items[2]);
            if (x != 0 && y != 0 )
            {
                isStarted = true;
                TotalStationAdjustCoordResult obj =  new TotalStationAdjustCoordResult()
                {
                    Name = name,
                    XY = new XY(x, y)
                };
                file.AdjustCoordResults[name] = obj;
                if (items.Length > 3)
                {
                    var mx = Geo.Utils.StringUtil.ParseDouble(items[3]);
                    var my = Geo.Utils.StringUtil.ParseDouble(items[4]);
                    var mp = Geo.Utils.StringUtil.ParseDouble(items[5]);
                    var e = Geo.Utils.StringUtil.ParseDouble(items[6]);
                    var f = Geo.Utils.StringUtil.ParseDouble(items[7]);
                    var t = Geo.Utils.StringUtil.ParseDouble(items[8]);
                    obj.MXY = new XY(mx, my);
                    obj.MP = mp;
                    obj.E = e;
                    obj.F = f;
                    obj.T = t;
                }
            } 
            return isStarted;
        }

        private static bool ParseDistanceLine(TotalStationResultFile file, bool isStarted, string[] items)
        {
            var startName = items[0];
            var endName = items[1];
            var lineName = new GnssBaseLineName(endName, startName);
            var type = items[2];

            var VALUE = Geo.Utils.StringUtil.ParseDouble(items[3]);
            var M = Geo.Utils.StringUtil.ParseDouble(items[4]);
            var V = Geo.Utils.StringUtil.ParseDouble(items[5]);
            var RESULT = Geo.Utils.StringUtil.ParseDouble(items[6]);
            var Ri = Geo.Utils.StringUtil.ParseDouble(items[7]);


            var measure = new DMS(VALUE, AngleUnit.D_MS).Degrees;
            var result = new DMS(RESULT, AngleUnit.D_MS).Degrees;


            if (VALUE != 0 && RESULT != 0 && V != 0 && Ri != 0 && M != 0)
            {
                isStarted = true;
                file.DistanceResults[lineName] = new TotalStationDistanceResult(lineName)
                {
                    AdjustValue = result,
                    MeasureValue = measure,
                    MSec = M,
                    Ri = Ri,
                    Type = type,
                    VSec = V
                };
            } 
            return isStarted;
        }

        private static bool ParseDirectionLine(TotalStationResultFile file, bool isStarted, string[] items)
        {
            var startName = items[0];
            var endName = items[1];
            var lineName = new GnssBaseLineName(endName, startName);
            var type = items[2];

            var VALUE = Geo.Utils.StringUtil.ParseDouble(items[3]);
            var M = Geo.Utils.StringUtil.ParseDouble(items[4]);
            var V = Geo.Utils.StringUtil.ParseDouble(items[5]);
            var RESULT = Geo.Utils.StringUtil.ParseDouble(items[6]);
            var Ri = Geo.Utils.StringUtil.ParseDouble(items[7]);

            var measure = new DMS(VALUE, AngleUnit.D_MS).Degrees;
            var result = new DMS(RESULT, AngleUnit.D_MS).Degrees;

            if ( V != 0 && Ri != 0 && M != 0)
            {
                isStarted = true;
                file.DirectionResults[lineName] = new TotalStationDirectionResult(lineName)
                {
                    AdjustValue = result,
                    MeasureValue = measure,
                    MSec = M,
                    Ri = Ri,
                    Type = type,
                    VSec = V
                };
            }

            return isStarted;
        }

    }
}
