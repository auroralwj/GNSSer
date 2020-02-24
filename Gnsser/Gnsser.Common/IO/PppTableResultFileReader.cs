//2017.02.06, czs, create in hongqing, FCB 计算器
//2017.03.19,czs, edit in hongqing, 提取为单独的类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Gnsser.Checkers;

namespace Gnsser
{

    /// <summary>
    /// 基于数据表的PPP结果读取器
    /// </summary>
    public class PppTableResultFileReader
    {
        /// <summary>
        /// 读取到历元存储对象
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <param name="isRemoveCsPrn"></param>
        /// <returns></returns>
        static public MultiSiteEpochValueStorage ReadToEpochStorage(string[] inputPathes, bool isRemoveCsPrn = true)
        {
            var tables = ReadPppAmbiResultInCycle(inputPathes, isRemoveCsPrn);

            return ReadToEpochStorage(tables);
        }
        /// <summary>
        /// 读取到历元存储对象
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static MultiSiteEpochValueStorage ReadToEpochStorage(ObjectTableManager tables)
        {
            MultiSiteEpochValueStorage multiSiteStorage = new MultiSiteEpochValueStorage("多站PPP结果");
            foreach (var table in tables)
            {
                MultiSatEpochRmsNumeralStorage multiSatStorage = multiSiteStorage.GetOrCreate(table.Name);
                var indexName = table.GetIndexColName();
                foreach (var row in table.BufferedValues)
                {
                    var epoch = (Time)row[indexName];
                    foreach (var item in row)
                    {
                        if (item.Key == indexName) { continue; }
                        var prn = SatelliteNumber.Parse(item.Key);
                        var val = (double)item.Value;
                        multiSatStorage.GetOrCreate(epoch).Add(prn, new RmsedNumeral(val, 0));
                    }
                }
            }
            return multiSiteStorage;
        }

        /// <summary>
        /// 读取模糊度
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <param name="isRemoveCsPrn"></param>
        /// <param name="isInLenthOrCycle"></param>
        /// <param name="waveLen">如果设为周，则必须设定波长</param>
        /// <returns></returns>
        public static ObjectTableManager ReadPppAmbiResult(string[] inputPathes, bool isRemoveCsPrn, bool isInLenthOrCycle = false, double waveLen = 1)
        {
            ObjectTableManager smartTalbes = ReadPppAmbiResultInLength(inputPathes, isRemoveCsPrn);
            if (!isInLenthOrCycle)
            {
                var abmiResultInCycle = smartTalbes.GetNewTableByDivision(waveLen, false,"");
                return abmiResultInCycle;
            }
            return smartTalbes;
        }


        /// <summary>
        /// 读取PPP模糊度结果，单位周，默认以GPS L1载波长度量。
        /// </summary>
        /// <param name="inputPathes">PPP文件路径</param>
        /// <param name="isRemoveCsPrn">是否移除周跳卫星</param>
        /// <returns></returns>
        public static ObjectTableManager ReadPppAmbiResultInCycle(string[] inputPathes, bool isRemoveCsPrn = true)
        {
            return ReadPppAmbiResult(inputPathes,  isRemoveCsPrn, false, Frequence.GpsL1.WaveLength);
        }
        /// <summary>
        /// 读取PPP模糊度结果,单位：米
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <param name="isRemoveCsPrn"></param>
        /// <returns></returns>
        public static ObjectTableManager ReadPppAmbiResultInLength(string[] inputPathes, bool isRemoveCsPrn)
        {
            //读取PPP定位结果，主要为提取模糊度参数，这里注意原始为以米为单位，
            var rawPppResultTables = ObjectTableManager.Read(inputPathes);

            //对读入的PPP结果文件做一些必要的处理 
            //去掉表名称的“_Params”，读入时，已经去掉了。
            //rawPppResultTables.ReplaceTableName("_Params", "");

            //首先删除无关历元和列
            rawPppResultTables.SetIndexColName("Epoch");
            var smartTalbes = rawPppResultTables.GetRepalcedColNameTable(new string[] { ParamNames.PhaseLengthSuffix, "_Nλ", "_N位", "_位N" }); //后者为字符编码错误
            //移除周跳的卫星
            if (isRemoveCsPrn)
            {
                RemoveCycleSlipedSat(smartTalbes);
            }

            foreach (var item in smartTalbes)
            {
                item.RemoveCols(new string[] { "CycleSlipe", Gnsser.ParamNames.Du, Gnsser.ParamNames.De, Gnsser.ParamNames.Dn, Gnsser.ParamNames.Dx, Gnsser.ParamNames.Dy, Gnsser.ParamNames.Dz, "cDt_r", "C*dtr", "StdDev", "WetTrop" }); // 删除不参与计算的无关列
                //key.RemoveEmptyRowsOf(BasePrn + "_Nλ"); // 删除没有基准数据的行
                //key.RemoveEmptyCols(); //删除空行
            }

            return smartTalbes;
        }

        /// <summary>
        /// 移除周跳卫星结果
        /// </summary>
        /// <param name="smartTalbes"></param>
        private static void RemoveCycleSlipedSat(ObjectTableManager smartTalbes)
        {
            var csKey = "CsOrRemoved";
            foreach (var table in smartTalbes)
            {
                foreach (var row in table.BufferedValues)
                {
                    if (row.ContainsKey(csKey))
                    {

                        var csStr = row[csKey].ToString();
                        var prns = csStr.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var prn in prns)
                        {
                            row.Remove(prn);
                        }
                    }
                }
            }
        }

    }
}
