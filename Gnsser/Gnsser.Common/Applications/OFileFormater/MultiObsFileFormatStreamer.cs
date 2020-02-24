//2016.09.26, czs, create in 西安洪庆, 观测文件预处理数据流

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;

namespace Gnsser
{



    /// <summary>
    /// 观测文件预处理数据流
    /// </summary>
    public class MultiObsFileFormatStreamer : MultiSiteObsStreamer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiObsFileFormatStreamer()
            : base(Gnsser.Setting.GnsserConfig.TempDirectory)
        {

        }
        EpochRinexObsFileWriterManager EpochRinexObsFileWriterManager { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            EpochRinexObsFileWriterManager = new Data.Rinex.EpochRinexObsFileWriterManager(this.Option.OutputDirectory, this.Option.OutputRinexVersion);

        } 

        /// <summary>
        /// 处理一个历元
        /// </summary>
        /// <param name="epoch"></param>
        public override void Process(MultiSiteEpochInfo mEpoch)
        {
            if (mEpoch == null || mEpoch.Count == 0) { return; }
            foreach (var item in mEpoch)
            {
                EpochRinexObsFileWriterManager.GetOrCreate(item.Name).Write(item);
            } 
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            EpochRinexObsFileWriterManager.Dispose();
            base.Dispose();
        }

        public override SimpleGnssResult Produce(MultiSiteEpochInfo material)
        {
            throw new NotImplementedException();
        }
    }

}