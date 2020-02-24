//2014.09.19, czs, create, in hailutu, 数据源统一设计
//2014.09.24, czs, edit in hailutu, 增加缓存技术，加快访问速度
//2014.10.05, czs, eidt in hailutu, 增加卫星天线读取，修正只读接收机天线的错误。
//2014.10.24, czs, eidt in namu shuangliao, 名称改为 AntennaFileService ，一切皆服务
//2014.12.27, czs, eidt in namu shuangliao, 考虑多日的每颗卫星情况
//2018.08.01, czs, edit in hmx, 做了简单的修改

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates; 
using Gnsser.Service; 
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Geo.Times;

using Gnsser.Times;
using Geo;

namespace Gnsser.Data
{
    /// <summary>
    /// 天线文件数据服务
    /// </summary>
    public class AntennaFileService : GnssFileService<Antenna>, IAntennaFileService
    {
        /// <summary>
        /// 天线数据源。
        /// </summary>
        /// <param name="antennaPath">天线文件路径</param>
        public AntennaFileService(FileOption antennaPath)
            : base(antennaPath)
        {
            this.Option = antennaPath;
            this.antexReader = new AntexReader(antennaPath.FilePath);
            this.typedBuffer = new Dictionary<string, Antenna>();
            this.serialEpochBuffer = new Dictionary<string, Dictionary<BufferedTimePeriod, Antenna>>();
        }
        /// <summary>
        /// 天线数据源。
        /// </summary>
        /// <param name="antennaPath">天线文件路径</param>
        public AntennaFileService(string antennaPath)
            : base(antennaPath)
        {
           // this.Option = antennaPath;
            this.antexReader = new AntexReader(antennaPath);
            this.typedBuffer = new Dictionary<string, Antenna>();
            this.serialEpochBuffer = new Dictionary<string, Dictionary<BufferedTimePeriod, Antenna>>();
        }

        AntexReader antexReader { get; set; }
        /// <summary>
        /// 存储一般的测站天线
        /// </summary>
        Dictionary<String, Antenna> typedBuffer { get; set; }
        Dictionary<String, Dictionary<BufferedTimePeriod, Antenna>> serialEpochBuffer { get; set; }
        private object locker = new object();

        /// <summary>
        /// 根据天线类型获取天线对象。测站一般采用此法。
        /// </summary>
        /// <param name="typeAndRadome">天线类型, 加上天线罩</param>
        /// <returns></returns>
        public Antenna Get(string typeAndRadome)
        {
            if (typeAndRadome == null) return null;

            if (typedBuffer.ContainsKey(typeAndRadome))
                return typedBuffer[typeAndRadome];

            Antenna antenna = null;
            lock (locker)
            {
                antenna = antexReader.GetAntenna(typeAndRadome);
                //无论是否为空null，都加进去, 下次直接提取，以保证速度。这是和 Reader 的区别。
                typedBuffer[typeAndRadome] = antenna;
            }

            return antenna;
        }
        static object obj = new object();
        /// <summary>
        /// 获取天线，通常是卫星天线
        /// </summary>
        /// <param name="serial">序号，通常是卫星的PRN</param>
        /// <param name="epoch">历元时间</param>
        /// <returns></returns>
        public Antenna Get(string serial, Time epoch)
        {
            lock (obj)
            {
                if (serialEpochBuffer.ContainsKey(serial))
                {
                    Dictionary<BufferedTimePeriod, Antenna> dic = serialEpochBuffer[serial];
                    foreach (var item in dic)
                    {
                        if (item.Key.Contains(epoch) || (item.Key.BufferedStart <= epoch && item.Key.BufferedEnd >= epoch))
                        {
                            return item.Value;
                        }
                    }
                }

                Antenna antenna = null;
                //lock (locker)
                {
                    antenna = antexReader.GetAntenna(serial, epoch);
                }

                //无论是否为空null，都加进去, 下次直接提取，以保证速度。这是和 Reader 的区别。
                if (serialEpochBuffer.ContainsKey(serial))//包含，但不包含这个时段
                {
                    BufferedTimePeriod span = new BufferedTimePeriod(antenna.ValidDateFrom, antenna.ValidDateUntil);
                    serialEpochBuffer[serial].Add(span, antenna);
                }
                else if (antenna != null)//不包含，且不为空，则
                {
                    Dictionary<BufferedTimePeriod, Antenna> dic = new Dictionary<BufferedTimePeriod, Antenna>();
                    BufferedTimePeriod span = new BufferedTimePeriod(antenna.ValidDateFrom, antenna.ValidDateUntil);
                    dic.Add(span, antenna);
                    serialEpochBuffer[serial] = dic;
                }
                else//不包含，文件中也没有，则认为本次计算时段内（1年）都没有,其中10秒为钟差预留
                {
                    Dictionary<BufferedTimePeriod, Antenna> dic = new Dictionary<BufferedTimePeriod, Antenna>();
                    BufferedTimePeriod span = new BufferedTimePeriod(epoch.Date - 10, epoch.Date + TimeSpan.FromDays(365));
                    dic.Add(span, antenna);
                    serialEpochBuffer[serial] = dic;
                }

                return antenna;
            }
        }


    }
}