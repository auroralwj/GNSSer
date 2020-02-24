//2014.12.27, czs, create in namu, 多（连续）文件数据流数据源.默认为采用同一个测站的数据，请输入时注意判断。

using System;
using System.Collections;
using System.Collections.Generic;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 多（连续）文件数据流数据源.默认为采用同一个测站的数据，请输入时注意判断。
    /// </summary>
    public class MultiRinexFileObsDataSource : AbstractObservationStream
    {
        /// <summary>
        /// 以多（连续）文件路径构建。
        /// </summary>
        /// <param name="filePathes">观测文件路径，默认为采用同一个测站的数据，请输入时注意判断。</param>
        public MultiRinexFileObsDataSource(List<string> filePathes) :this(filePathes.ToArray()){ }
        /// <summary>
        /// 以多（连续）文件路径构建。
        /// </summary>
        /// <param name="filePathes">观测文件路径，默认为采用同一个测站的数据，请输入时注意判断。</param>
        public MultiRinexFileObsDataSource(string[] filePathes)
        {
            this.ObsFileReaders = new List<RinexObsFileReader>();
            foreach (var filePath in filePathes)
            {
                var obsFileReader = new RinexObsFileReader(filePath);
                ObsFileReaders.Add(obsFileReader);
            }
            this.CurrentIndex = 0;
            var header = this.CurrentReader.GetHeader();
            this.SiteInfo = header.SiteInfo;
            this.ObsInfo = header.ObsInfo; 
            
        } 
        /// <summary>
        /// 结束时间，最后一个文件的日期。
        /// </summary>
        public  Time EndTime { get { return ObsFileReaders[FileCount-1].GetHeader().EndTime; } }

        /// <summary>
        /// 每个文件对应一个读取器
        /// </summary>
        private List<RinexObsFileReader> ObsFileReaders { get; set; }

        /// <summary>
        /// 适配器设计模式.当前文件的读取器。
        /// </summary>
        public  RinexObsFileReader CurrentReader { get { return ObsFileReaders[CurrentFileIndex]; } }

        /// <summary>
        /// 当前文件编号，从 0 开始。
        /// </summary>
        public int CurrentFileIndex { get; set; }
        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get { return ObsFileReaders.Count; } }
  
        /// <summary>
        /// 当前之后，是否还有文件
        /// </summary>
        public bool HasNexFile
        {
            get { return CurrentFileIndex + 1 < FileCount; }
        }
        /// <summary>
        /// 移动到下一个。
        /// </summary>
        /// <returns>如果没有了，就返回 false </returns>
        public override bool MoveNext()
        {
            bool result = CurrentReader.MoveNext();
            if (result)
            {
                CurrentIndex++;
            }
            else//当前的已经读完,
            {   //还有下一个文件
                if ( this.HasNexFile)
                {
                    CurrentFileIndex++;
                    CurrentReader.Reset();//重置
                    return MoveNext();
                }
            }
            return result;
        }


        /// <summary>
        /// 重置，标记退回到 0.
        /// </summary>
        public override void Reset()
        {
            CurrentIndex = 0;
            CurrentFileIndex = 0;
            //只需重置第一个，其它的在MoveNext中重置了。
            CurrentReader.Reset();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            foreach (var item in ObsFileReaders)
            {
                item.Dispose();
            }
        }
        /// <summary>
        /// 除非时间相等或靠后，否则此方法非常耗费时间。
        /// 若没有找到，返回 null。
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <param name="toleranceSeccond"></param>
        /// <returns></returns>
        public override Domain.EpochInformation Get(Time gpsTime, double toleranceSeccond = 1e-15)
        {
            if (gpsTime < this.ObsInfo.StartTime) return null;//不在有效时段内。 
            if (this.ObsInfo.IsEndTimeAvailable)
            {
                if (gpsTime > this.EndTime) return null;
            }
            var differ = gpsTime - this.Current.ReceiverTime;

            if (Math.Abs(differ) < toleranceSeccond)
            {
               this.CurrentReader.Get(gpsTime, toleranceSeccond); 
            }

            if (differ > 0) //往后，此处可以优化，先定位每个文件，再查找，？？//2015.10.15，留给以后做吧！！！！
            {
                while (this.MoveNext())
                {
                    return Get(gpsTime, toleranceSeccond);
                }
                return null;

            }else{
                this.Reset();
                return Get( gpsTime, toleranceSeccond);
            }
        }
    }
}
