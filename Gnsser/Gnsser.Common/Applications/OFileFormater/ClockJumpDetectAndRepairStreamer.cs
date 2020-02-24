//2018.08.15, czs, create in HMX, 观测文件预处理数据流

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
    public class ClockJumpDetectAndRepairStreamer : SingleObservationStreamer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        public ClockJumpDetectAndRepairStreamer(GnssProcessOption Option) : base(Option)
        {
            ClockJumpDetector = new ClockJumpDetector(Option);
        }

        /// <summary>
        /// 历元写
        /// </summary>
        public EpochRinexObsFileWriter EpochRinexObsFileWriter { get; set; }
        ClockJumpDetector ClockJumpDetector { get; set; }
        DataSourceContext DataSourceContext;
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            var destPath = System.IO.Path.Combine(this.Option.OutputDirectory, System.IO.Path.GetFileName(Path));
            EpochRinexObsFileWriter = new Data.Rinex.EpochRinexObsFileWriter(destPath, this.Option.OutputRinexVersion);

            ClockJumpDetector.Buffers = this.BufferedStream.MaterialBuffers;

            DataSourceContext = DataSourceContext.LoadDefault(Option);

            if(DataSourceContext.ClockJumpFile != null)
            {
                var file = DataSourceContext.ClockJumpFile;
                var index = file.GetIndexColName();//time 
                var valColName = file. GetColNameNotIndex();
                ClockJumpDic = new Dictionary<Time, double>();
                foreach (var item in file.BufferedValues)
                {
                    var time = (Time)item[index];
                    var val = (Double)item[valColName];
                    ClockJumpDic[time] = val;
                }

            }
         
        }
         
        Dictionary<Time, double> ClockJumpDic { get; set; }

        /// <summary>
        /// 处理一个历元
        /// </summary>
        /// <param name="epoch"></param>
        public override void Process(EpochInformation epoch)
        {
            if (ClockJumpDic != null)
            {
                if(ClockJumpDic.ContainsKey(epoch.ReceiverTime))
                {
                    var val = ClockJumpDic[epoch.ReceiverTime];

                    epoch.CorrectClockJump(-val, ClockJumpState.ClockJumped);
                    log.Info("尝试修复钟跳 " + epoch.ReceiverTime + ", " + val);
                }
            }
            else
            {
                ClockJumpDetector.Revise(ref epoch);
            }

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
    }

}