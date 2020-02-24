//2015.05.19,cy, 连续观测时段大于一定时段的才保留

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
    /// 观测文件中卫星连续观测时段控制管理器
    /// </summary>
    public class ObsFileCountManager : Manager<RinexObsFile>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ObsFile"></param>
        public ObsFileCountManager(RinexObsFile ObsFile)
        {
            this.Name = "观测类型管理器";
            this.ObsFile = ObsFile;

          //  this.SatObsTypeMarkers = GetSatObsTypeMarkers(this.ObsFile);
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
        //public Dictionary<SatelliteType, List<string>> GetObsTypesWithAppearanceLesserThan(double maxPercentage)
        //{
        //   // return GetObsTypesWithAppearanceLesserThan(ref ObsFile, maxPercentage);
        //}


        /// <summary>
        ///  若某一观测量值出勤率低， 则直接删除该观测量。
        /// </summary>
        public Dictionary<SatelliteType, List<string>> RemoveObservers(double maxPercentage)
        {
            RinexObsFile obsFile = ObsFile;
            Dictionary<SatelliteType, List<string>> obsTypesTobeRemove = GetObsTypesWithAppearanceLesserThan(ref obsFile, maxPercentage);

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
            Dictionary<SatelliteType, List<string>> obsTypesTobeRemove = GetObsTypesWithAppearanceLesserThan(ref obsFile, maxPercentage);

            obsFile.RemoveObsType(obsTypesTobeRemove);

            return obsTypesTobeRemove;
        }


        /// <summary>
        /// 获取出现百分比小于指定值的观测类型。
        /// </summary>
        /// <param name="maxPercentage"></param>
        /// <returns></returns>
        static public Dictionary<SatelliteType, List<string>> GetObsTypesWithAppearanceLesserThan(ref RinexObsFile ObsFile, double maxPercentage)
        {
            Dictionary<SatelliteType, SatObsTypeMarker> markers = GetSatObsTypeMarkers(ref ObsFile);
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
        public static Dictionary<SatelliteType, SatObsTypeMarker> GetSatObsTypeMarkers(ref RinexObsFile ObsFile)
        {
            Dictionary<SatelliteType, SatObsTypeMarker> markers = new Dictionary<SatelliteType, SatObsTypeMarker>();

         
            
            List<Geo.Times.Time> ss = new List<Geo.Times.Time>();

            //存储待删除的时段的起始与结束历元标记，多个时段
            Dictionary<SatelliteNumber, List<Geo.Times.Time>> data = new Dictionary<SatelliteNumber, List<Geo.Times.Time>>();

            //记录一个时段的观测历元信息
            Dictionary<SatelliteNumber, List<dataItem>> data0 = new Dictionary<SatelliteNumber, List<dataItem>>(); 




            foreach (var obsSection in ObsFile)
            {
                //只记录观测值，不管卫星编号
                foreach (var obsData in obsSection)
                {
                    //第一次出现
                    if (!data0.ContainsKey(obsData.Prn))
                    {
                        if (!data.ContainsKey(obsData.Prn)) data.Add(obsData.Prn, new List<Geo.Times.Time>());
                       // satData[obsData.Prn].Add(obsSection.Time);


                        data0.Add(obsData.Prn, new List<dataItem>());
                        dataItem pair=new dataItem(obsSection.ReceiverTime,0,obsData.Prn);

                        data0[obsData.Prn].Add(pair);
                        data0[obsData.Prn][0].lastGpsTime = obsSection.ReceiverTime;
                        data0[obsData.Prn][0].Count += 1;
                        data0[obsData.Prn][0].TimeList.Add(obsSection.ReceiverTime);
                        continue;
                    }


                    if (Math.Abs(obsSection.ReceiverTime - data0[obsData.Prn][0].lastGpsTime) <= 31)
                    {

                        data0[obsData.Prn][0].Count += 1;
                        data0[obsData.Prn][0].TimeList.Add(obsSection.ReceiverTime);

                        data0[obsData.Prn][0].lastGpsTime = obsSection.ReceiverTime;

                    }
                    else
                    {
                        //判断这个时段内观测的数据是否超过40分钟，否则要删除
                        if (data0[obsData.Prn][0].Count < 80)
                        {
                            data[obsData.Prn].Add(data0[obsData.Prn][0].beginGpsTime); //删除该时段的开始标记

                            data[obsData.Prn].Add(data0[obsData.Prn][0].lastGpsTime); //删除该时段的结束标记

                        }

                        data0.Remove(obsData.Prn); //移除，重新开始

                        //再次添加新的时段
                        if (!data0.ContainsKey(obsData.Prn))
                        {
                            data0.Add(obsData.Prn, new List<dataItem>());
                            dataItem pair = new dataItem(obsSection.ReceiverTime, 0, obsData.Prn);

                            data0[obsData.Prn].Add(pair);
                            data0[obsData.Prn][0].lastGpsTime = obsSection.ReceiverTime;
                            data0[obsData.Prn][0].Count += 1;
                            data0[obsData.Prn][0].TimeList.Add(obsSection.ReceiverTime);
                        }
                    }

                   


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

            //最后一个时段是否也满足弧长段
            foreach (var item in data0)
            {
                if (item.Value[0].Count < 80)
                {
                    data[item.Value[0].Prn].Add(item.Value[0].beginGpsTime); //删除该时段的开始标记

                    data[item.Value[0].Prn].Add(item.Value[0].lastGpsTime); //删除该时段的结束标记
                }
            }


           
          List<string> list = new List<string>();
            
            foreach (var marker in markers)
            {

              foreach (var type in marker.Value)
                {                 
                    list.Add(type);
                }
            }

            foreach (var obsSection in ObsFile)
            {
                //只记录观测值，不管卫星编号
                foreach (var obsData in obsSection)
                {
                    if (data.ContainsKey(obsData.Prn))
                    {
                        if (data[obsData.Prn].Count > 0)
                        {
                            //
                            for (int i = 0; i < data[obsData.Prn].Count / 2; i++)
                            {
                                //
                                if (data[obsData.Prn][i * 2] <= obsSection.ReceiverTime && obsSection.ReceiverTime <= data[obsData.Prn][i * 2 + 1])
                                {
                                    //
                                    obsSection[obsData.Prn].Remove(list);
                                }
                            }
                        }
                    }
                }
            }





            return markers;
        }


        /// <summary>
        /// 一颗卫星一个时段的卫星记录
        /// </summary>
        private class dataItem
        {
            public dataItem(Geo.Times.Time gpsTime, int li,SatelliteNumber prn)
            {
                this.lastGpsTime = gpsTime;
                this.beginGpsTime = gpsTime;
                this.Count = li;
                this.Prn = prn;
                this.TimeList = new List<Geo.Times.Time>();
            }

            public List<Geo.Times.Time> TimeList { get; set; }


            public SatelliteNumber Prn { get; set; }

            public Geo.Times.Time lastGpsTime { get; set; }


            public Geo.Times.Time beginGpsTime { get; set; }


            public int Count { get; set; }
        }

        #endregion
    } 
}
