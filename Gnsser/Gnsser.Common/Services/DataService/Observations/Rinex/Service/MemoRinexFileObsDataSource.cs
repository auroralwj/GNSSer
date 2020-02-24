//2014.08.19, czs, create, 以数据流方式传递的观测数据源
//2014.12.27, czs, refactor in namu, 提取抽象方法
//2015.01.02, czs, edit in namu, 修正 ReSet 方法，ReSet 后 Header 对象不改变。

using System;
using System.Collections;
using System.Collections.Generic;
using Gnsser.Times;
using Gnsser.Domain;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 以数据流方式传递的观测数据源。
    /// 这种数据源比较节约内存，但是只能从起始位置一步一步的往下读取数据，不能获知整个数据流大小。
    /// </summary>
    public class MemoRinexFileObsDataSource : AbstractObservationStream
    {
        /// <summary>
        /// 以单观测文件路径构建。
        /// </summary>
        /// <param name="ObsFile">文件</param> 
        public MemoRinexFileObsDataSource(RinexObsFile ObsFile)
        {
            this.ObsFile = ObsFile;
            EnumCount = int.MaxValue / 2;
            if (ObsFile.Header != null)
            {
                this.Name = ObsFile.Header.MarkerName;
                this.Header = ObsFile.Header;
                this.SiteInfo = Header.SiteInfo;
                this.ObsInfo = Header.ObsInfo;

                this.NavPath = Header.NavFilePath;
            }
            this.EpochInfoBuilder = new RinexEpochInfoBuilder(this.ObsInfo.SatelliteTypes);
        }

        public RinexObsFile ObsFile { get; set; }
        /// <summary>
        /// 头部信息
        /// </summary>
        public RinexObsFileHeader Header { get; private set; }
        /// <summary>
        /// 标准RINEX对象创建EpochInformation
        /// </summary>
        public RinexEpochInfoBuilder EpochInfoBuilder { get; set; }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose() { ObsFile.Dispose(); }
        /// <summary>
        /// 重置，标记退回到 0.
        /// </summary>
        public override void Reset()
        {
            CurrentIndex = 0;
        }

        /// <summary>
        /// 当前历元编号从0 开始。
        /// </summary>
        //  public override int CurrentIndex { get { return _CurrentReader.CurrentIndex; }  }
        /// <summary>
        /// 起始编号，从0开始。
        /// </summary>
        //   public override int StartIndex { get { return _CurrentReader.StartIndex; } set { _CurrentReader.StartIndex = value; } }
        /// <summary>
        /// 遍历数量，默认为最大值的一半。
        /// </summary>
        //  public override int EnumCount { get { return _CurrentReader.EnumCount; } set { if(_CurrentReader!=null) _CurrentReader.EnumCount = value; } }

        /// <summary>
        /// 获取指定时刻的观测数据
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <param name="toleranceSeccond"></param>
        /// <returns></returns>
        public override Domain.EpochInformation Get(Geo.Times.Time gpsTime, double toleranceSeccond = 1e-15)
        {
            var epochObs = this.ObsFile.GetEpochObservation(gpsTime, toleranceSeccond);
            return EpochInfoBuilder.Build(epochObs);
        }

        /// <summary>
        /// 获取下面的观测数据，当前除外
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public override List<Domain.EpochInformation> GetNexts(int count)
        {
            List<EpochInformation> list = new List<EpochInformation>();
            for (int i = 0; i < count && MoveNext(); i++)
            {
                list.Add(Current);
            }
            return list;
        }

        /// <summary>
        /// 向下移动
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            #region 流程控制
            CurrentIndex++;
            if (CurrentIndex == StartIndex) { log.Debug("数据流 " + this.Name + " 开始读取数据。"); }
            if (this.IsCancel) { log.Info("数据流 " + this.Name + " 已被手动取消。"); return false; }
            if (CurrentIndex > this.MaxEnumIndex) { log.Info("数据流 " + this.Name + " 已达指定的最大编号 " + this.MaxEnumIndex); return false; }
            while (CurrentIndex < this.StartIndex) { this.MoveNext(); }
            #endregion
            var epoch = this.ObsFile.Get(CurrentIndex);

            if (epoch != null)
            {
                this.Current = EpochInfoBuilder.Build(epoch);
                return true;
            }
            return false;
        }
    }
}
