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
    public class RinexFileObsDataSource : AbstractObservationStream
    {
        /// <summary>
        /// 以单观测文件路径构建。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="IsReadContent">是否读取内容</param>
        public RinexFileObsDataSource(string filePath, bool IsReadContent = true)
        {
            this.Path = filePath;
            if (!System.IO.File.Exists(Path)) { throw new ArgumentException("文件不存在，" + filePath); }
            EnumCount = int.MaxValue / 2;
            this.Name = System.IO.Path.GetFileName(filePath);
            this._CurrentReader = new RinexObsFileReader(Path, IsReadContent);
             this.Header = this.CurrentReader.GetHeader();
             this.SiteInfo = Header.SiteInfo;
             this.ObsInfo = Header.ObsInfo;

             this.NavPath = Header.NavFilePath;

            this.EpochInfoBuilder = new RinexEpochInfoBuilder(this.ObsInfo.SatelliteTypes);
        }
        /// <summary>
        /// 头部信息
        /// </summary>
        public RinexObsFileHeader Header { get; private set; }
        /// <summary>
        /// 标准RINEX对象创建EpochInformation
        /// </summary>
        public RinexEpochInfoBuilder EpochInfoBuilder { get; set; }

        ///// <summary>
        ///// 单文件路径
        ///// </summary>
        //public string Path { get; set; }

        private RinexObsFileReader _CurrentReader;
        /// <summary>
        /// 适配器设计模式
        /// </summary>
        public RinexObsFileReader CurrentReader { get { return _CurrentReader; } } 

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose() { CurrentReader.Dispose(); }
        /// <summary>
        /// 重置，标记退回到 0.
        /// </summary>
        public override void Reset()
        {
            CurrentIndex = 0; 
            _CurrentReader.Reset(); 
        }   
        
        /// <summary>
        /// 当前历元编号从0 开始。
        /// </summary>
        public override int CurrentIndex { get { return _CurrentReader.CurrentIndex; }  }
        /// <summary>
        /// 起始编号，从0开始。
        /// </summary>
        public override int StartIndex { get { return _CurrentReader.StartIndex; } set { _CurrentReader.StartIndex = value; } }
        /// <summary>
        /// 遍历数量，默认为最大值的一半。
        /// </summary>
        public override int EnumCount { get { return _CurrentReader.EnumCount; } set { if(_CurrentReader!=null) _CurrentReader.EnumCount = value; } }

        #region 静态实用方法

        /// <summary>
        /// 以数据流形式导入观测文件数据
        /// </summary>
        /// <returns></returns>
        public static List<ISingleSiteObsStream> LoadObsData(string[] pathes, Geo.Utils.ExceptionInfo exceptionInfo = null)
        {
            List<ISingleSiteObsStream> obsDataSources = new List<ISingleSiteObsStream>();
            foreach (var item in pathes)
                Geo.Utils.ExceptionUtil.TryInvoke(new Action(delegate() { obsDataSources.Add(new RinexFileObsDataSource(item)); }), exceptionInfo);
            return obsDataSources;
        }
        #endregion
        /// <summary>
        /// 获取指定时刻的观测数据
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <param name="toleranceSeccond"></param>
        /// <returns></returns>
        public override Domain.EpochInformation Get(Geo.Times.Time gpsTime, double toleranceSeccond = 1e-15)
        {
            var epochObs = _CurrentReader.Get(gpsTime, toleranceSeccond);
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
            //#region 流程控制
            //CurrentIndex++;
            //if (CurrentIndex == StartIndex) { log.Debug("数据流 " + this.Name + " 开始读取数据。"); }
            //if (this.IsCancel) { log.Info("数据流 " + this.Name + " 已被手动取消。"); return false; }
            //if (CurrentIndex > this.MaxEnumIndex) { log.Info("数据流 " + this.Name + " 已达指定的最大编号 " + this.MaxEnumIndex); return false; }
            //while (CurrentIndex < this.StartIndex) { this.MoveNext(); }
            //#endregion
              
            if (_CurrentReader.MoveNext())
            {
               var result = (EpochInfoBuilder.Build(_CurrentReader.Current));
                if(result == null) { return false; }
                this.Current = result;
                return true;
            }
            return false;
        }
    }
}
