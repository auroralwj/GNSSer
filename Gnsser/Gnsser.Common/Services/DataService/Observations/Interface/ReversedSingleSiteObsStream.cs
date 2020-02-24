//2018.07.27, czs, create in HMX, 反转的观测数据源

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
    /// 反转的单站观测数据源
    /// </summary>
    public class ReversedSingleSiteObsStream : ReversedEnumber<EpochInformation>, IObservationStream<EpochInformation>, IService
    {
        /// <summary>
        /// 反转的单站观测数据源
        /// </summary>
        /// <param name="Stream"></param>
        public ReversedSingleSiteObsStream(ISingleSiteObsStream Stream) : base(Stream)
        {
            this.OriginalSource = Stream;
        }
        /// <summary>
        /// 初始数据源
        /// </summary>
        public ISingleSiteObsStream OriginalSource { get; set; }
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
        public EpochInformation CurrentProduct { get; set; }
        /// <summary>
        /// 产品产生
        /// </summary>
        public event Action<EpochInformation> ProductProduced;

        public List<EpochInformation> GetNexts(int count)
        {
            throw new NotImplementedException();
        }

        public List<EpochInformation> Gets(int startIndex = 0, int maxCount = int.MaxValue)
        {
            throw new NotImplementedException();
        }
    }


}