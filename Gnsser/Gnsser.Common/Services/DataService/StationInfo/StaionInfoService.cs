//2016.12.27, czs, add in hongqing, 测站信息服务

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates; 
using Gnsser.Service; 
using Gnsser.Data;
using Gnsser.Times;
using Gnsser.Data.Rinex;

using Geo.IO;
using Geo; 
using Geo.Times;
using Gnsser.Data.Sinex;

namespace Gnsser
{ 

    /// <summary>
    /// 测站信息服务
    /// </summary>
    public class StaionInfoService : Named, IService
    {
        /// <summary>
        /// 测站信息服务 
        /// </summary>
        /// <param name="option"></param>
        public StaionInfoService(string option)
            : this(new FileOption(option))
        {
        }

        public StationInfoReader reader { get; set; }

        /// <summary>
        /// 测站信息服务
        /// </summary>
        public StaionInfoService(FileOption option)
        {
            log.Info("启用测站信息文件服务。");
            this.FileOption = option;
            this.Name = Path.GetFileName(option.FilePath);

            this.Data = new BaseDictionary<string, Dictionary<TimePeriod, StationInfo>>();

            reader = new StationInfoReader(option.FilePath);
            var all = reader.ReadAll();
            foreach (var item in all)
            {
                if (!Data.Contains(item.SiteName)) { Data.Add(item.SiteName, new Dictionary<TimePeriod, StationInfo>()); }

                Data[item.SiteName].Add(item.TimePeriod, item); 
            }
            log.Info("加载了 " + Data.Count + " 个测站信息。"); 

        }

        /// <summary>
        /// 文件选型
        /// </summary>
        FileOption FileOption { get; set; }

        BaseDictionary<string, Dictionary<TimePeriod, StationInfo>> Data { get; set; }


        public List<StationInfo> GetAll()
        {
            List<StationInfo> all = new List<StationInfo>();

            foreach (var item in Data)
            {
                all.AddRange(item.Values);
            }
            return all;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public StationInfo Get(string name, Time date)
        { 
            if (Data == null) { return null; }

            var Name = name.Trim().ToUpper();
            if (Data.Contains(Name))
            {
                var dic = Data[Name];
                foreach (var item in dic)
                {
                    if (item.Key.Contains(date))
                    {
                        return dic[item.Key];
                    }
                }  
            }
            return null;
        } 
 
    }

}