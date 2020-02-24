//2015.02.09, czs, create in 麦克斯西餐厅 双辽， 观测文件观测类型管理器

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Geo.Utils;
using System.Text;
using Geo.Coordinates; 
using Gnsser.Times;
using Geo;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 观测文件观测类型管理器
    /// </summary>
    public class ObsFileTypeManager : Manager<RinexObsFile>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ObsFile"></param>
        public ObsFileTypeManager(RinexObsFile ObsFile)
        {
            this.Name = "观测类型管理器";
            this.ObsFile = ObsFile;

            this.SatObsTypeMarkers = GetSatObsTypeMarkers(this.ObsFile);
        }

        /// <summary>
        /// 观测文件。
        /// </summary>
        public RinexObsFile ObsFile { get; protected set; }

        /// <summary>
        /// 统计各类型的卫星观测类型出勤率。
        /// </summary>
        public  Dictionary<SatelliteType, SatObsTypeMarker> SatObsTypeMarkers { get; protected set; }

        /// <summary>
        /// 获取出现百分比小于指定值的观测类型
        /// </summary>
        /// <param name="maxPercentage"></param>
        /// <returns></returns>
        public Dictionary<SatelliteType, List<string>> GetObsTypesWithAppearanceLesserThan(double maxPercentage)
        {
            return GetObsTypesWithAppearanceLesserThan(ObsFile, maxPercentage);
        }


        /// <summary>
        ///  若某一观测量值出勤率低， 则直接删除该观测量。
        /// </summary>
        public Dictionary<SatelliteType, List<string>> RemoveObservers(double maxPercentage)
        {
            RinexObsFile obsFile = ObsFile;
            Dictionary<SatelliteType, List<string>> obsTypesTobeRemove = GetObsTypesWithAppearanceLesserThan(obsFile, maxPercentage);

            obsFile.RemoveObsType(obsTypesTobeRemove);

            return obsTypesTobeRemove;
        } 


        #region 工具方法

        /// <summary>
        /// 返回一个便于显示的字符串。
        /// </summary>
        /// <param name="obsFile"></param>
        /// <param name="maxPercentage"></param>
        /// <returns></returns>
       static  public string RemoveObserversInfo(ref RinexObsFile obsFile, double maxPercentage)
        {
            var list = RemoveObservers( ref obsFile,  maxPercentage);

            return ToString(list);
        }

       private static string ToString(Dictionary<SatelliteType, List<string>> list)
       {
           StringBuilder sb = new StringBuilder();
           foreach (var item in list)
           {
               sb.Append(item.Key + "[");
               int i = 0;
               foreach (var val in item.Value)
               {
                   if (i != 0) sb.Append(",");
                   sb.Append(val);
                   i++;
               }
               sb.Append("], ");
           }

           return sb.ToString();
       }

        /// <summary>
        ///  若某一观测量值出勤率低， 则直接删除该观测量。
        /// </summary>
        static public Dictionary<SatelliteType, List<string>> RemoveObservers(ref RinexObsFile obsFile, double maxPercentage)
        {
            Dictionary<SatelliteType, List<string>> obsTypesTobeRemove = GetObsTypesWithAppearanceLesserThan(obsFile, maxPercentage);

            obsFile.RemoveObsType(obsTypesTobeRemove);

            return obsTypesTobeRemove;
        }


        /// <summary>
        /// 获取出现百分比小于指定值的观测类型。
        /// </summary>
        /// <param name="maxPercentage"></param>
        /// <returns></returns>
        static public Dictionary<SatelliteType, List<string>> GetObsTypesWithAppearanceLesserThan(RinexObsFile ObsFile, double maxPercentage)
        {
            Dictionary<SatelliteType, SatObsTypeMarker> markers = GetSatObsTypeMarkers(ObsFile);
            Dictionary<SatelliteType, List<string>> obsTypesTobeRemove = new Dictionary<SatelliteType, List<string>>();
            foreach (var marker in markers)
            {
                List<string> list = new List<string>();
                obsTypesTobeRemove[marker.Key] = list;
                int maxCount = marker.Value.GetMaxCount();
                foreach (var type in marker.Value)
                {
                    var percentage = marker.Value.GetPercentage(type, maxCount);
                    if (percentage <= maxPercentage)
                    {
                        list.Add(type);
                    }
                }
            }
            return obsTypesTobeRemove;
        }


        /// <summary>
        /// 获取文件的类型出入次数
        /// </summary>
        /// <param name="ObsFile"></param>
        /// <returns></returns>
        public static Dictionary<SatelliteType, SatObsTypeMarker> GetSatObsTypeMarkers(RinexObsFile ObsFile)
        {
            Dictionary<SatelliteType, SatObsTypeMarker> markers = new Dictionary<SatelliteType, SatObsTypeMarker>();
            foreach (var obsSection in ObsFile)
            {
                //只记录观测值，不管卫星编号

                foreach (var obsData in obsSection)
                {
                    //某一特定类型卫星
                    var satType = obsData.Prn.SatelliteType;
                    if (!markers.ContainsKey(satType)) { markers[satType] = new SatObsTypeMarker(); }

                    var sateObsMarker = markers[satType];
                    //该卫星的所有观测量
                    foreach (var type in obsData)
                    {
                        if (obsData.TryGetValue(type.Key) != 0)
                        {
                            sateObsMarker.Markes(type.Key);
                        }
                        else if (!sateObsMarker.Contains(type.Key)) //查看是否具有记录，如果没有，就标记为 0 。
                        {
                            sateObsMarker.SetObsTypeCount(type.Key, 0);
                        }
                    }
                }
            }
            return markers;
        }
        #endregion
    } 
}
