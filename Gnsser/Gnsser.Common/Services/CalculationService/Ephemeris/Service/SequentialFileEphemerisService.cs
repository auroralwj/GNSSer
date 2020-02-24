//2013.03.19.21.15，czs, edit, 应该用集合表示，但是累了，以后再弄吧。
//2014.12.25, czs, edit, 将 FileCollectionEphemerisService 重构为 MultiFileEphemerisService，意为多文件数据源星历服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser.Service;
using Gnsser.Times;
using Geo.Times; 
namespace Gnsser
{
    /// <summary>
    /// 依据输入的路径，提供一段连续的星历数据文件服务。
    /// 文件集合数据源。支持单系统类型取决于数据内容。
    /// 假想的使用条件为多个连续的SP3文件。
    /// 根据传入的路径，按照时间和系统类型进行排序。如果是相邻数据源，则直接拼接，以获得连续的服务。
    /// 如果有重复的传输，则取第一个（实际上，还需要判断有效时间等）。
    /// 传入的文件按照系统进行分类，如果传入的数据混乱，则可能造成不可用。
    /// </summary>
    public class SequentialFileEphemerisService : FileEphemerisService,  IMultiFileEphemerisService
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="factory">工厂</param>
        /// <param name="pathes">文件路径</param>
        public SequentialFileEphemerisService(IEphemerisServiceFactory factory,string[] pathes)
        {
            var sources = new List<IEphemerisService>();
            foreach (var item in pathes)
            { 
                var service = factory.CreateFromFile(item);
                sources.Add(service);
            }

            Init(sources);
        } 
        /// <summary>
        /// 单个服务集合注册成多文件服务
        /// </summary>
        /// <param name="sources"></param>
        public SequentialFileEphemerisService( IEnumerable<IEphemerisService> sources)
        {
            Init(sources);
        }

        private void Init(IEnumerable<IEphemerisService> sources)
        {
            this._sources = new List<IEphemerisService>(sources);

            this.dic = new Dictionary<SatelliteType, List<IEphemerisService>>();
            foreach (var service in sources)
            {
                foreach (var type in service.SatelliteTypes)
                {
                    if (!dic.ContainsKey(type)) dic[type] = new List<IEphemerisService>();
                    dic[type].Add(service);
                }
            }

            //排序
            foreach (var item in this.dic.Values)
            {
                item.Sort();
            }
            this._sources.Sort();
            this.MaxGap = TimeSpan.FromHours(2);
        }

        #endregion

        #region 核心变量
        BufferedTimePeriod period;
        /// <summary>
        /// 星历数据源字典。
        /// </summary>
        Dictionary<SatelliteType, List<IEphemerisService>> dic { get; set; }
        /// <summary>
        /// 暂时保留，以列表形式存储。
        /// </summary>
        List<IEphemerisService> _sources { get; set; }
        #endregion

        /// <summary>
        /// 该星历采用的坐标系统,如 IGS08， ITR97
        /// </summary>
        public override string CoordinateSystem { get { if (_sources == null || _sources.Count ==0) return Data.Rinex.Sp3File.UNDEF; return _sources[0].CoordinateSystem; } }

        /// <summary>
        /// 数据源最大的间隔，超过这个间隔则不可以提供服务。
        /// </summary>
        public TimeSpan MaxGap { get; set; }
        /// <summary>
        /// 所有的卫星列表
        /// </summary>
        public override List<SatelliteNumber> Prns
        {
            get
            {
                List<SatelliteNumber> list = new List<SatelliteNumber>();
                foreach (var item in _sources)
                {
                    item.Prns.ForEach(new Action<SatelliteNumber>(  delegate(SatelliteNumber m)
                        {
                            if (!list.Contains(m))  list.Add(m);
                        }));
                }
                return list;
            }
        }
        /// <summary>
        /// 有效时间段。用于多文件。性能已优化。
        /// </summary>
        public List<BufferedTimePeriod> GetTimePeriods(SatelliteType satelliteType)
        {
            if (periodsDic.ContainsKey(satelliteType)) return periodsDic[satelliteType];

            List<BufferedTimePeriod> spans = GetTimePeriodsDirectly(satelliteType);
            //合并时段
            periodsDic[satelliteType] = BufferedTimePeriod.Merge(spans);

            return periodsDic[satelliteType];
        }

        #region GetTimePeriods 的缓存
        //缓存
        Dictionary<SatelliteType, List<BufferedTimePeriod>> periodsDic = new Dictionary<SatelliteType, List<BufferedTimePeriod>>();
        private List<BufferedTimePeriod> GetTimePeriodsDirectly(SatelliteType satelliteType)
        {
            List<BufferedTimePeriod> spans = new List<BufferedTimePeriod>();
            if (this.dic.ContainsKey(satelliteType))
            {
                foreach (var item in dic[satelliteType])
                {
                    spans.Add(item.TimePeriod);
                }
            }
            return spans;
        }
        #endregion

        /// <summary>
        /// 总的有效时间段，多个时段可能不是连续的，详细的可以使用 GetTimePeriods(SatelliteType satelliteType) 方法。
        /// 该属性将被多次访问，所以预先赋值。性能已优化。
        /// </summary>
        public override BufferedTimePeriod TimePeriod { get { if (period == null) period = GetGpsTimePeriod(); return period; } }
        #region TimePeriod 缓存
        /// <summary>
        /// 懒加载
        /// </summary>
        /// <returns></returns>
        private BufferedTimePeriod GetGpsTimePeriod()
        {
            double startBuffer = 0;
            double endBuffer = 0;
            Time start = Time.MaxValue;
            Time end = Time.MinValue;
            foreach (var item in this._sources)
            {
                if (item.TimePeriod.Start < start) { start = item.TimePeriod.Start; startBuffer = item.TimePeriod.StartBuffer; }
                if (item.TimePeriod.End > end) { end = item.TimePeriod.End; endBuffer = item.TimePeriod.EndBuffer; }
            }
            return new BufferedTimePeriod(start, end, startBuffer, endBuffer);
        }
        #endregion

        /// <summary>
        /// 原始输出所有的数据。
        /// </summary>
        /// <param name="prn">卫星编号</param>
        /// <param name="from">起始时间</param>
        /// <param name="to">截止时间</param>
        /// <returns></returns>
        public override List<Ephemeris> Gets(SatelliteNumber prn, Time from, Time to)
        {
            List<Ephemeris> list = new List<Ephemeris>();
            foreach (var item in _sources)  list.AddRange(item.Gets(prn, from, to)); 
            list.Sort();//排序
            return list;
        }
        //public List<Ephemeris> GetPrn(Time from, Time to)
        //{
        //    List<Ephemeris> list = new List<Ephemeris>();
        //    list.AddRange(key.Gets( from, to)); 
        //    list.Sort();//排序
        //    return list;
 
        //}
        /// <summary>
        /// 获取所有星历信息。
        /// </summary>
        /// <returns></returns>
        public override List<Ephemeris> Gets()
        {
            List<Ephemeris> list = new List<Ephemeris>();
            foreach (var item in _sources) list.AddRange(item.Gets());
            list.Sort();//排序
            return list;
        }
        static object locker = new object();
        /// <summary>
        /// 拟合输出。
        /// 当前策略，在众多数据源中，选择包含有效时间段内的进行计算并返回。
        /// 待改进：如果有精密星历。。。。。
        /// </summary>
        /// <param name="prn">卫星编号</param>
        /// <param name="gpsTime">时刻</param>
        /// <returns></returns>
        public override Ephemeris Get(SatelliteNumber prn, Time gpsTime)
        {

            if (!IsAvailable(gpsTime, prn.SatelliteType)) return null;

            List<IEphemerisService> sources = this.dic[prn.SatelliteType];

            //直接获取之
            if (sources.Count == 0) return null;
            bool caculated = false;//标记是否计算过
            //获取星历并赋权
            List<Ephemeris> results = new List<Ephemeris>();
            foreach (var source in sources)
            {
                if (source.TimePeriod.Contains(gpsTime) || (source.TimePeriod.BufferedEnd >= gpsTime && source.TimePeriod.BufferedStart <= gpsTime))
                {
                    caculated = true;

                    //是否包含这颗卫星
                    if (!source.Prns.Contains(prn)) continue;

                    var result = source.Get(prn, gpsTime);
                    if (result == null) continue;
                    //赋权,只有一个就不用比较了。
                    if (sources.Count != 1 && result.Rms == null)
                    {
                        result.Rms = EphemerisUtil.GetRms(source.ServiceType);
                    }
                    results.Add(result);
                }
            }
            //别无二家，只能用它
            if (results.Count == 1) return results[0];
            //根据权重进行选取
            if (results.Count != 0)
            {
                var result = results[0];
                foreach (var item in results)
                {
                    if (result.Rms.Length > item.Rms.Length) result = item;
                }
                return result;
            }

            //计算失败了，可能是根本就没有这颗卫星，继续计算仍然将失败。
            if (caculated) return null;


            //以上已经排除在单独文件有效范围内了。
            //下面看是否在这些文件有效时间范围间隙内。
            //如果在整体时间段内，但不在单独文件范围内，则前后联合起来加密,

            //如果间隔太远（如超过半天）则效果不好，因此在数据录入时应该判断。

            //选择前后最接近的两个文件,这个时间段的起点部分在一个文件时间末尾，结束部分在一个文件的开始时间处。

            //检查是否在可用的间隙内
            var two = GetNearstService(gpsTime, prn.SatelliteType);
            IEphemerisService first = two[0];
            IEphemerisService second = two[1];
            double gap = (double)(two[1].TimePeriod.Start - two[0].TimePeriod.End);

            if (this.MaxGap.TotalSeconds < gap)
                throw new Exception("星历间隙太大：" + TimeSpan.FromSeconds(gap).ToString() + ", 最大允许：" + MaxGap.ToString());

            //合并两者
            //获取一个小时的数据，时间间隔为5分钟
            List<Ephemeris> infosA = first.Gets(prn, first.TimePeriod.BufferedEnd - 3600, first.TimePeriod.BufferedEnd, 5 * 60);
            List<Ephemeris> infosB = second.Gets(prn, second.TimePeriod.BufferedStart, second.TimePeriod.BufferedStart + 3600, 5 * 60);

            List<Ephemeris> infosAList = new List<Ephemeris>();
            foreach (var item in infosA)
            {
                if (item != null)
                    infosAList.Add(item);
            }
            foreach (var item in infosB)
            {
                if (item != null)
                    infosAList.Add(item);
            }
            //是否没有该星的服务
            if (infosAList.Count == 0) return null;

            EphemerisInterpolator interp = new EphemerisInterpolator(infosAList);
            var finalResult = interp.GetEphemerisInfo(gpsTime);
            return finalResult;//怎样考虑精度信息？？？？2014.12.26，czs

            //如果还不行
            throw new NotImplementedException("数据源提供的信息暂不支持 " + prn + " 在 " + gpsTime + " 的解算。");

        }    

        /// <summary>
        /// 是否健康可用
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="satTime"></param>
        /// <returns></returns>
        public override bool IsAvailable(SatelliteNumber prn, Time satTime)
        {
            return true;

            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 是否可用.只是一个初步的判断，并没有去获取星历。
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="satelliteType">卫星类型</param>
        /// <returns></returns>
        public bool IsAvailable(Time time, SatelliteType satelliteType)
        {
            if (this.SatelliteTypes.Contains(satelliteType))
            {
                //检查是否在自服务时段内
                var times =  this.GetTimePeriods(satelliteType);
                foreach (var item in times)
                {
                    if (item.Contains(time) || (item.BufferedStart <= time && item.BufferedEnd >= time)) return true;
                }
                
                //检查是否在可用的间隙内
                var two = GetNearstService(time, satelliteType);

                double gap = (double)(two[1].TimePeriod.Start - two[0].TimePeriod.End);
                if (this.MaxGap.TotalSeconds >= gap)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 指定时间前后最接近的服务。
        /// </summary>
        /// <param name="time">指定时间</param>
        /// <param name="satelliteType">指定的系统</param>
        /// <returns></returns>
        private IEphemerisService[] GetNearstService(Time time, SatelliteType satelliteType)
        {
            var sources = this.dic[satelliteType];
            IEphemerisService first = sources[0];
            IEphemerisService second = sources[sources.Count - 1];
            foreach (var source in sources)
            {
                if (source.TimePeriod.End < time
                    && source.TimePeriod.End > first.TimePeriod.End)
                    first = source;

                if (source.TimePeriod.Start > time
                    && source.TimePeriod.Start < second.TimePeriod.Start)
                    second = source;
            }
            var two = new IEphemerisService[2];
            two[0] = first;
            two[1] = second;
            
            return two;
        }
    }
}
