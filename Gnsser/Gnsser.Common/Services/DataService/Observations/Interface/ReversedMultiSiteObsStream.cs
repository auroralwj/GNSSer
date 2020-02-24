//2018.07.27, czs, create in HMX, 反转的多观测数据源

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
using Gnsser.Times;
using Gnsser.Data.Rinex;
using Geo.Times;
using Geo.IO;
using Geo;
using Gnsser.Domain;

namespace Gnsser
{

    /// <summary>
    /// 反转的多观测数据源
    /// </summary>
    public class ReversedMultiSiteObsStream : ReversedEnumber<MultiSiteEpochInfo>, IObservationStream<MultiSiteEpochInfo>, IService
    {
        /// <summary>
        /// 反转的多观测数据源
        /// </summary>
        /// <param name="Stream"></param>
        public ReversedMultiSiteObsStream(MultiSiteObsStream Stream) : base(Stream)
        {
            this.OriginalSource = Stream;
        }
        /// <summary>
        /// 初始数据源
        /// </summary>
        public MultiSiteObsStream OriginalSource { get; set; }
        /// <summary>
        /// 测站信息
        /// </summary>
        public ISiteInfo SiteInfo { get => OriginalSource.SiteInfo; }
        /// <summary>
        /// 观测信息
        /// </summary>
        public IObsInfo ObsInfo { get => OriginalSource.ObsInfo; }
        /// <summary>
        /// 当前产品
        /// </summary>
        public MultiSiteEpochInfo CurrentProduct { get; set; }
        /// <summary>
        /// 产品产生
        /// </summary>
        public event Action<MultiSiteEpochInfo> ProductProduced;

        public List<MultiSiteEpochInfo> GetNexts(int count)
        {
            throw new NotImplementedException();
        }

        public List<MultiSiteEpochInfo> Gets(int startIndex = 0, int maxCount = int.MaxValue)
        {
            throw new NotImplementedException();
        }
    }


}