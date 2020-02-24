//2016.09.24, czs, create in 西安洪庆, 观测文件预处理数据流
//2018.09.06, czs, edit in hmx, 名字修改为 ObsObjFormatStreamer ，区别于RINEX格式
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
    /// 观测文件预处理数据流，采用GNSSer对象，非RINEX对象，主要用于数据预处理文件生成。
    /// </summary>
    public class ObsObjFormatStreamer : SingleSiteObsAdjustStreamer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ObsObjFormatStreamer(bool IsUseRangeCorrections)
            : base(Gnsser.Setting.GnsserConfig.TempDirectory)
        {
            this.IsUseRangeCorrections = IsUseRangeCorrections;
        }

        /// <summary>
        /// 历元写
        /// </summary>
        public EpochRinexObsFileWriter EpochRinexObsFileWriter { get; set; }
        /// <summary>
        /// 是否使用伪距改正
        /// </summary>
        public bool IsUseRangeCorrections  { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            
            var destPath = System.IO.Path.Combine(this.Option.OutputDirectory, System.IO.Path.GetFileName(Path));
            EpochRinexObsFileWriter = new Data.Rinex.EpochRinexObsFileWriter(destPath, this.Option.OutputRinexVersion, IsUseRangeCorrections);
        }
        /// <summary>
        /// 矫正器，此处只需进行模型改正了。
        /// </summary>
        /// <returns></returns>
        protected override IReviser<EpochInformation> BuildProducingReviser()
        {
            SatTimeInfoManager = new SatTimeInfoManager(DataSource.ObsInfo.Interval);

            return EpochInfoReviseManager.GetProducingReviser(Context, Option, SatTimeInfoManager);
        }

        /// <summary>
        /// 处理一个历元
        /// </summary>
        /// <param name="epoch"></param>
        public override void Process(EpochInformation epoch)
        { 
            EpochRinexObsFileWriter.Write(epoch);
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            EpochRinexObsFileWriter.Dispose();
            base.Dispose();
        }

        public override SimpleGnssResult Produce(EpochInformation material)
        {
            throw new NotImplementedException();
        }
    }

}