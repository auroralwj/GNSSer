//2016.11.19，czs, create in hongqing, 观测文件格式化转换
//2017.05.15, czs, edit in hongqing, 一些修改
//2018.06.22, czs, edit in HMX, 增加采样率小于1s的稀疏
//2018.08.15, czs, edit in hmx, 修正并提取小观测段数据移除

using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
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
    /// 观测文件格式化转换。
    /// </summary>
    public class ObsFileFormaterOld : ObsFileEpochRunner<RinexEpochObservation>
    {
        Log log = new Log(typeof(ObsFileFormater));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ObsFileConvertOption"></param>
        /// <param name="inPath"></param>
        public ObsFileFormaterOld(ObsFileConvertOption ObsFileConvertOption, string inPath)
        {
            this.Option = ObsFileConvertOption;
            this.OutputDirectory = this.Option.OutputDirectory;
            this.FilePath = inPath;
            this.FileName = Path.GetFileName(FilePath);
            this.IsHeaderWrited = false;
            this.Inverval = Double.MaxValue;
        }
        #region 属性
        /// <summary>
        /// 缓存大小
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 只是名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 不需要的观测码，需要删除的观测码
        /// </summary>
        Dictionary<SatelliteType, List<string>> ObsCodesToBeRemove { get; set; }
        /// <summary>
        /// 操作选项
        /// </summary>
        public ObsFileConvertOption Option { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 写入器
        /// </summary>
        RinexObsFileWriter Writer { get; set; } 
        /// <summary>
        /// 是否写了头部
        /// </summary>
        bool IsHeaderWrited { get; set; }
        /// <summary>
        /// 原始头部文件。
        /// </summary>
        public RinexObsFileHeader OldHeader { get; set; }
        /// <summary>
        /// 原始头部文件。
        /// </summary>
        public RinexObsFileHeader CurrentHeader { get; set; }
        /// <summary>
        /// 当前时段起始时间。
        /// </summary>
        public Time CurrentStartTime { get; set; }
        /// <summary>
        /// 上一历元观测数据,在写入前赋值。
        /// </summary>
        public RinexEpochObservation PrevEpochObs { get; set; }
        /// <summary>
        /// 当前历元观测数据,在写入前赋值。
        /// </summary>
        public RinexEpochObservation CurrentEpochObs { get; set; }
        /// <summary>
        /// 初始秒数，用于应对采样间隔。
        /// </summary>
        public double InitSeconds { get; set; }
        /// <summary>
        /// 文件的初始时长
        /// </summary>
        public double InputTimeInMinutes { get; set; } 
        /// <summary>
        /// 管理窗口数据，判断是否连续
        /// </summary>
        WindowDataManager<SatelliteNumber, Time> WindowDataManager { get; set; }
        /// <summary>
        /// 采样间隔
        /// </summary>
        public double Inverval { get; set; }
        /// <summary>
        /// 输出的子目录
        /// </summary>
        public string SubDirectory { get; set; }
        /// <summary>
        /// 载波对齐器，只对齐一个 A。
        /// </summary>
        NumericalAlignerManager<SatelliteNumber, Time> NumericalAlignerManagerPhaseA { get; set; }
        NumericalAlignerManager<SatelliteNumber, Time> PhaseAAmenderAligner { get; set; }
        /// <summary>
        /// 载波对齐器，只对齐一个 B。
        /// </summary>
        NumericalAlignerManager<SatelliteNumber, Time> NumericalAlignerManagerPhaseB { get; set; }

        /// <summary>
        /// 载波A周跳修理器
        /// </summary>
        TimeNumeralWindowDataManager<SatelliteNumber> PhaseACycleSlipAmender { get; set; }
        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            //缓存的数据可能被过滤或删除，因此必须大于最小历元数加上断裂数量
            this.BufferSize = this.Option.MinEpochCount + this.Option.MaxBreakCount + 5;
            this.WindowDataManager = new WindowDataManager<SatelliteNumber, Time>(this.Option.MinEpochCount + this.Option.MaxBreakCount + 2);


            //构建数据流
            base.Init();
            this.BufferedStream.Completed += BufferedStream_Completed;
            this.BufferedStream.MaterialEnded += BufferedStream_MaterialEnded;
            var OldHeader = ReadOriginalHeader(FilePath);
            this.Option.Interval = Math.Max(Option.Interval, OldHeader.Interval);
            this.InputTimeInMinutes = (OldHeader.EndTime - OldHeader.StartTime) / 60.0;
            this.CurrentStartTime = OldHeader.StartTime;
            this.OldHeader = OldHeader;
            this.TableTextManager = new ObjectTableManager(10000, OutputDirectory);
            this.TableTextManager.Clear();

            this.NumericalAlignerManagerPhaseA = new NumericalAlignerManager<SatelliteNumber, Time>(this.Option.Interval * 3, m => m.SecondsOfWeek);
            this.PhaseAAmenderAligner = new NumericalAlignerManager<SatelliteNumber, Time>(this.Option.Interval * 3, m => m.SecondsOfWeek);

            PhaseACycleSlipAmender = new TimeNumeralWindowDataManager<SatelliteNumber>(this.BufferSize - 2, this.Option.Interval * 3);

            LastEpoch = this.OldHeader.EndTime;
            //医学院， 75-112,33, 900-9500，学区房，五证全
        }
        /// <summary>
        /// 数据流结束了，还要一些判断的收尾工作要做
        /// </summary>
        private void BufferedStream_Completed()
        {
        }

        /// <summary>
        /// 上一个历元
        /// </summary>
        public Time PrevOkEpoch { get; set; }
        /// <summary>
        /// 最后历元，不一定准确。
        /// </summary>
        Time LastEpoch { get; set; }

        void BufferedStream_MaterialEnded()
        {
            //        LastEpoch = BufferedStream.Last().Time;//czs, 2018.06.23, 这个不可用，否则末尾数据将离奇消失！！！
            //缓存小时前，还要一些判断的收尾工作要做
            if (Option.IsEnableMinEpochCount)
            {
                JudgeAndFiltSmallSpanInBuffer(this.BufferedStream.DataSource.Current.ReceiverTime, true);
            }
        }

        /// <summary>
        /// 计算采用间隔。单位：秒。
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static double GetInteval(IWindowData<RinexEpochObservation> buffer)
        {
            double interval = double.MaxValue;
            int i = 0;
            RinexEpochObservation prev = null;
            foreach (var item in buffer)
            {
                if (prev == null) { prev = item; continue; }
                interval = Math.Min(interval, Math.Abs( item.ReceiverTime - prev.ReceiverTime));
                if (i > 5) break;
                i++;
            }
            return interval;
        }

        /// <summary>
        /// 初探
        /// </summary>
        /// <param name="info"></param>
        public override void RawRevise(RinexEpochObservation info)
        {
            if (Option.IsAmendBigCs)
            {
                foreach (var sat in info)
                {
                    var wlen = Frequence.GetFrequence(sat.Prn, 1,info.ReceiverTime).WaveLength;
                    var window = this.PhaseACycleSlipAmender.GetOrCreate(sat.Prn);
                    var referVal = sat.RangeA.Value / wlen;
                    bool isRested = false;
                    sat.PhaseA.Value = window.FitCheckAddAndAlignWithStep(info.ReceiverTime, sat.PhaseA.Value, out isRested, referVal, 2, 50, 100); 
                    //若重置，则应该标记周跳或失锁
                    if (isRested)
                    {
                        sat.PhaseA.LossLockIndicator = LossLockIndicator.Bad;
                    }
                }
            }

            if (Option.IsEnableAlignPhase)
            {
                foreach (var sat in info)
                {
                    var NumericalAligner = NumericalAlignerManagerPhaseA.GetOrCreate(sat.Prn);
                    var phaseA = sat.PhaseA;
                    var rangeA = sat.RangeA.Value / Frequence.GetFrequence(sat.Prn,1, info.ReceiverTime).WaveLength;
                    
                    phaseA.Value = NumericalAligner.GetAlignedValue(info.ReceiverTime, phaseA.Value, rangeA);
                }
            }

            base.RawRevise(info);
            CheckAndFiltSmallSpan(info);
        }

        /// <summary>
        /// 过滤掉小历元时段，在缓存中操作，确保缓存大于断裂数量
        /// </summary>
        /// <param name="info"></param>
        private void CheckAndFiltSmallSpan(RinexEpochObservation info)
        {
            //是否启用
            if (!this.Option.IsEnableMinEpochCount) { return; }

            //添加
            foreach (var sat in info) { WindowDataManager.GetOrCreate(sat.Prn).Add(info.ReceiverTime); }
            ////保证有足够的数量
            //if (!WindowDataManager.HasFull) { return; }

            JudgeAndFiltSmallSpanInBuffer(info.ReceiverTime, info .ReceiverTime== LastEpoch);
        }
        /// <summary>
        /// 判断过滤
        /// </summary>
        /// <param name="currentTime"></param>
        /// <param name="isLastEpoch"></param>
        private void JudgeAndFiltSmallSpanInBuffer(Time currentTime, bool isLastEpoch)
        {

            //计算采样间隔，计算一次即可
            if (this.Inverval == 0 || this.Inverval == Double.MaxValue) { this.Inverval = GetInteval(this.BufferedStream.MaterialBuffers); }
            var minBreakTimeSpan = this.Inverval * Option.MaxBreakCount;

            //执行判断,并移除历元卫星
            foreach (var prn in WindowDataManager.Keys)
            {
                var window = WindowDataManager.Get(prn);
                if (window.Count == 0) { continue; }
                if (isLastEpoch)
                {
                    int ii = 0;
                }

                //当前与上一个在指定历元内，且非最后，即后续还有数据需要添加后再判断，这样可以节约大量时间
                var lastSan = Math.Abs(currentTime - window.Last);
                if (lastSan < minBreakTimeSpan && !isLastEpoch)
                {
                    continue;
                }


                //按照允许的最大间隔分离
                var gapedWindows = window.Split((prev, current) => { return Math.Abs(current - prev) > minBreakTimeSpan; });

                int subCount = 0;
                foreach (var subWindow in gapedWindows)
                {
                    subCount++;

                    if (subWindow.Count == 0) { continue; }



                    //包括当前，且非最后，即后续还有数据需要添加后再判断
                    if (subWindow.Contains(currentTime) && !isLastEpoch)
                    {
                        continue;
                    }

                    //没有包括当前，但是窗口最后一个历元与当前不超过指定的阈值，且非最后历元，则直接继续，需要添加后判断
                    lastSan = Math.Abs(currentTime - subWindow.Last);
                    if (!subWindow.Contains(currentTime) && lastSan < minBreakTimeSpan && !isLastEpoch)
                    {
                        continue;
                    }

                    //-----------以下数据进行判断长度并删除过短的数据-----------

                    //小于指定的数量
                    if (subWindow.Count < Option.MinEpochCount)
                    {
                        log.Info(prn + " 第 " + subCount + "/" + gapedWindows.Count + " 段 " + "，在缓存中移除时段 " + subWindow.ToString() + ", 当前： " + currentTime + ", 与最后差(s)：" + lastSan);
                        int i = 0;
                        foreach (var epoch in this.BufferedStream.MaterialBuffers)
                        {
                            if (epoch.Contains(prn) && window.Contains(epoch.ReceiverTime))
                            {
                                epoch.Remove(prn);
                                i++;
                            }
                        }

                        //处理最后一个历元
                        if (isLastEpoch)
                        {
                            var epo = this.BufferedStream.DataSource.Current;
                            if (epo.Contains(prn) && window.Contains(epo.ReceiverTime))
                            {
                                epo.Remove(prn);
                                i++;
                            }
                        }

                        log.Info(prn + " 移除了 " + i + " 个");
                    }

                    //除了当前历元（已跳过continue），判断一次后清空过期的数据窗口，第二段重新积累，历史车轮滚滚向前
                    window.Remove(subWindow);
                }
            }
        }




        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="current"></param>
        public override void Process(RinexEpochObservation current)
        {
            if (this.CurrentIndex == 0)
            {      
                InitSeconds = current.ReceiverTime.DateTime.TimeOfDay.TotalSeconds;
                if (Option.IsEnableInterval && Option.Interval > 1 &&  InitSeconds % 5 != 0) // 若非0, 5秒整数倍的采样间隔，则强制采用10秒整数倍间隔
                {
                    InitSeconds = 0;
                }
                //首次建立头文件，需要缓存支持
                this.CurrentHeader = BuildOutputHeader(OldHeader);
            }

            //移除其它系统
            if (Option.IsEnableSatelliteTypes)
            {
                current.RemoveOther(Option.SatelliteTypes);
            }


            //采样间隔
            if (Option.IsEnableInterval)
            { 
                //首先考虑采样率大于1s
                var diff = Math.Round(current.ReceiverTime.DateTime.TimeOfDay.TotalSeconds - InitSeconds) % Option.Interval;
                if (diff > 0.5 && Option.Interval > 1)
                {
                    return;
                }
                else
                {
                    int i = 0;
                }
                //采样率小于1s
                if(current.ReceiverTime - PrevOkEpoch < Option.Interval)
                {
                    return;
                }
            }

            if (current.ReceiverTime >= new Time(2017, 8, 11, 23))
            {
                int i = 0;
            }
            PrevOkEpoch = current.ReceiverTime;
            current.Header = this.CurrentHeader;

            //时段过滤
            if (Option.IsEnableTimePeriod)
            {
                if (!Option.TimePeriod.Contains(current.ReceiverTime)) { return; }
            }

            if (Option.IsEnableRemoveSats && Option.SatsToBeRemoved != null && Option.SatsToBeRemoved.Count > 0)
            { 
                current.Remove(Option.SatsToBeRemoved);
            }



            //删除指定数据为空的卫星
            if (Option.IsDeleteVacantSat)
            {
                List<SatelliteNumber> tobeDeletes = new List<SatelliteNumber>();
                foreach (var sat in current)
                {
                    if(sat.Prn == SatelliteNumber.Parse("G15"))
                    {
                        int ii = 0;
                    }
                    foreach (var item in Option.NotVacantCodeList)
                    {
                        if (sat.TryGetValue(item) == 0)
                        {
                            tobeDeletes.Add(sat.Prn);
                        }
                    }
                }
                current.Remove(tobeDeletes);
            }
            //移除观测值为 0 的
            if (Option.IsRemoveZeroPhaseSat)
            {
                List<SatelliteNumber> tobeDeletes = new List<SatelliteNumber>();
                foreach (var sat in current)
                {
                    foreach (var item in CurrentHeader.ObsCodes)
                    {
                        if (sat.Prn.SatelliteType == item.Key)
                        {
                            foreach (var code in item.Value)
                            {
                                if (code.StartsWith("L") && sat.TryGetValue(code) == 0) { tobeDeletes.Add(sat.Prn); }
                            }
                        }
                    }
                }
                current.Remove(tobeDeletes);
            }     
            




            //移除观测值为 0 的
            if (Option.IsRemoveZeroRangeSat)
            {
                List<SatelliteNumber> tobeDeletes = new List<SatelliteNumber>();
                foreach (var sat in current)
                {
                    foreach (var item in CurrentHeader.ObsCodes)
                    {
                        if (sat.Prn.SatelliteType == item.Key)
                        {
                            foreach (var code in item.Value)
                            {
                                if ((code.StartsWith("C") || code.StartsWith("P")) && sat.TryGetValue(code) == 0) { tobeDeletes.Add(sat.Prn); }
                            }
                        }
                    }
                }
                current.Remove(tobeDeletes);
            }

            //是否启用卫星系统过滤,此处多余，只需在头部判断即可。
            //if(Option.IsEnableSatelliteTypes){
            //    current.RemoveOthers(Option.SatelliteTypes);
            //}

            ////观测类型过滤凭借第一字母判断
            //if (Option.IsEnableObsTypes)
            //{
            //    current.RemoveOthers(Option.ObsTypes);
            //} 


            //观测码数量不足的移除
            if (ObsCodesToBeRemove != null &&  ObsCodesToBeRemove.Count > 0)
            {
                foreach (var sat in current)
                {
                    if (!ObsCodesToBeRemove.ContainsKey(sat.Prn.SatelliteType)) { continue; }

                    var codes = this.ObsCodesToBeRemove[sat.Prn.SatelliteType];

                    foreach (var item in codes)
                    {
                        sat.Remove(item);
                    }
                }
            }
            if (Option.SatCutOffAngle.Enabled && !XYZ.IsZeroOrEmpty(CurrentHeader.ApproxXyz))  
            { 
                List<RinexSatObsData> list = new List<RinexSatObsData>();
                foreach (var item in current)
                {
                    var eph = GlobalNavEphemerisService.Instance.Get(item.Prn, current.ReceiverTime);
                    var angle = CoordTransformer.XyzToGeoPolar(eph.XYZ, CurrentHeader.ApproxXyz);
                    if(angle.Elevation < Option.SatCutOffAngle.Value)
                    {
                        list.Add(item);
                    }
                }
                foreach (var item in list)
                {
                    current.Remove(item.Prn);
                }
            }

            if (Option.IsConvertPhaseToLength)
            {
                foreach (var item in current)
                {
                    if (item.Prn.SatelliteType == SatelliteType.G )
                    {
                        foreach (var val in item)
                        {
                            if (val.Key.Contains("L1"))
                            {
                                val.Value.Value = val.Value.Value * Frequence.GpsL1.WaveLength;
                            }
                            if (val.Key.Contains("L2"))
                            {
                                val.Value.Value = val.Value.Value * Frequence.GpsL2.WaveLength;
                            }
                        }
                    }
                }

            }

            //判断并写入文件
            WriteToFile(current);
        }

        /// <summary>
        /// 写到文件.判断是否分段写入
        /// </summary>
        /// <param name="current"></param>
        private void WriteToFile(RinexEpochObservation current)
        {
            this.PrevEpochObs = this.CurrentEpochObs;
            this.CurrentEpochObs = current;

            //切割
            if (Option.EnabledSection.Enabled && Option.EnabledSection.Value  < InputTimeInMinutes)
            {
                var timeDiffer = current.ReceiverTime - CurrentStartTime ;
                if (timeDiffer >= Option.EnabledSection.Value * 60)
                {
                    //结束上一段文件。
                    PostRun();//更新头部结束日期。

                   //写下一分段
                    var spanSecond = Option.EnabledSection.Value * 60;
                    //对于头部而言，只有时间不同。
                   this.CurrentHeader.ObsInfo.StartTime = CurrentStartTime + spanSecond;
                   this.CurrentHeader.ObsInfo.EndTime = current.ReceiverTime + spanSecond;

                    var newFileName = BuildSectionFileName(current, CurrentHeader);//构建新路径

                    InitWriter(newFileName);

                   this.WriteHeader(this.CurrentHeader);

                    CurrentStartTime = current.ReceiverTime;//更新当前起始时间 
                }

                //写头文件，并判断
                if (!IsHeaderWrited)
                { 
                    this.CurrentHeader.ObsInfo.StartTime = current.ReceiverTime;
                    this.CurrentHeader.ObsInfo.EndTime = current.ReceiverTime + Option.EnabledSection.Value * 60;
                    var newFileName = BuildSectionFileName(current, CurrentHeader);

                    InitWriter(newFileName);
                    this.WriteHeader(this.CurrentHeader); 
                }
            }

            //写头文件，并判断
            if (!IsHeaderWrited)
            {
                InitWriter(this.FileName);
                this.CurrentHeader.ObsInfo.StartTime = current.ReceiverTime;
                this.WriteHeader(this.CurrentHeader);
            }
      

            //最后，满足条件，写入文件
            Writer.WriteEpochObservation(current);
        }
        /// <summary>
        /// 写入
        /// </summary>
        public override void PostRun()
        {
            if (CurrentEpochObs != null && this.PrevEpochObs != null)
            {
                this.Writer.Flush();

                if (this.CurrentHeader.ObsInfo.EndTime != CurrentEpochObs.ReceiverTime 
                    &&(this.CurrentHeader.ObsInfo.EndTime != this.PrevEpochObs.ReceiverTime))
                {
                    this.Writer.ReWriteHeader(this.CurrentHeader);//方法内自动更新时间
                }
            }
        }

        /// <summary>
        /// 构建子时段名称，精确到分钟，以示区别。
        /// </summary>
        /// <param name="current"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private string BuildSectionFileName(RinexEpochObservation current, RinexObsFileHeader header)
        {
            RinexFileNameBuilder nameBuilder = new RinexFileNameBuilder();
            return nameBuilder.Build(header); 

            var fileName = Path.GetFileNameWithoutExtension(this.FilePath);
            var extension = Path.GetExtension(this.FilePath);
            return fileName + current.ReceiverTime.Hour.ToString("00") + "." + current.ReceiverTime.Minute.ToString("00") + extension; 
        }
         

        /// <summary>
        /// 格式化输出的头部。
        /// </summary>
        /// <param name="OldHeader"></param>
        /// <returns></returns>
        private RinexObsFileHeader BuildOutputHeader(RinexObsFileHeader OldHeader)
        {
            RinexObsFileHeader newHeader = (RinexObsFileHeader)OldHeader.Clone();
            //输出版本
            if (Option.IsEnableRinexVertion)
            {
                newHeader.Version = this.Option.Version;
                log.Info("输出版本：" + Option.Version);
            }

            //采用间隔
            if (Option.IsEnableInterval)
            {
                newHeader.Interval = Option.Interval;
                log.Info("指定的采样率：" + Option.Interval);
            }

            //是否启用卫星系统过滤, 为修改 header.ObsCodes，不仅如此，内容也要及时删除，以免多余劳动
            if (Option.IsEnableSatelliteTypes)
            {
                newHeader.RemoveOther(Option.SatelliteTypes);
                log.Info("将移除非此系统：" + Geo.Utils.StringUtil.ToString(Option.SatelliteTypes));
            }

            //观测类型过滤
            if (Option.IsEnableObsTypes)
            {
                newHeader.RemoveOther(Option.ObsTypes);
                log.Info("将移除非此观测值：" + Geo.Utils.StringUtil.ToString(Option.ObsTypes));
            } 


            //删除观测值为 0 的数据。
            if (Option.IsEnableMinObsCodeAppearRatio)
            {     
                //缓存
                var tables = BuildObsCodeTables(newHeader.ObsCodes); //采用新头部的

                var ratios = GetObsCodeRatios(tables);

                //出现率低于 x 的代码
                var minRatio = Option.MinObsCodeAppearRatio;
                this.ObsCodesToBeRemove = new Dictionary<SatelliteType, List<string>>();
                var ObsCodes = new Dictionary<SatelliteType, List<string>>();

                foreach (var kv in ratios)
                {
                    List<string> okCodes = new List<string>();
                    List<string> badCodes = new List<string>();
                    foreach (var item in kv.Value)
                    {
                        if (item.Value > minRatio)
                        {
                            okCodes.Add(item.Key);
                        }
                        else
                        {
                            badCodes.Add(item.Key);
                        }
                        if (item.Value != 1) { log.Info(FileName + "  " + kv.Key + " " + item.Key + " 出勤率 " + item.Value); }
                    }
                    ObsCodes.Add(kv.Key, okCodes);
                    ObsCodesToBeRemove.Add(kv.Key, badCodes);
                }

                newHeader.ObsCodes = ObsCodes; 
            }

            //删除其它观测码
            if (Option.IsReomveOtherCodes)
            {
                foreach (var collection in newHeader.ObsCodes)
                {
                    List<string> tobeRemoves = new List<string>();
                    foreach (var item in  collection.Value)
                    {
                        if (!Option.OnlyCodes.Contains(item)) { tobeRemoves.Add(item); } 
                    }
                    foreach (var item in tobeRemoves)
                    {
                        collection.Value.Remove(item);
                    }
                }
                log.Info("将移除非此观测码：" + Geo.Utils.StringUtil.ToString(Option.ObsCodes));
            }

            return newHeader;
        }

        private RinexObsFileHeader ReadOriginalHeader(string inFilePath)
        {
            var reader = new RinexObsFileReader(inFilePath);
            return  reader.GetHeader();//以原头文件为蓝本
        }
        /// <summary>
        /// 初始化读取器
        /// </summary>
        /// <param name="outFilePath"></param>
        private void InitWriter(string outFilePath)
        {
            if (Writer != null) { Writer.Dispose(); }
            //输出版本
            double version = OldHeader.Version;
            if (Option.IsEnableRinexVertion)
            {
                version = Option.Version;
            }
            //文件名称
            string fileName = BuildOutputFileName(outFilePath);
            //文件路径
            var savePath = "";
            if (!String.IsNullOrWhiteSpace(SubDirectory))
            {
                savePath = Path.Combine(Option.OutputDirectory, SubDirectory, fileName);
            }
            else
            {
                savePath = Path.Combine(Option.OutputDirectory, fileName);
            }
            Writer = new RinexObsFileWriter(savePath, version);
        }

        private string BuildOutputFileName(string outFilePath)
        {
            string fileName = Path.GetFileName(outFilePath);
            if (Option.RinexNameType != RinexNameType.保持不变)
            {
                var nameVersion = 2.0;
                if (Option.RinexNameType == RinexNameType.RINEX3长名称)
                {
                    nameVersion = 3.0;
                }
                RinexFileNameBuilder nameBuilder = new RinexFileNameBuilder(nameVersion);

                if (Option.IsEnableTimePeriod)
                {
                    if (this.CurrentHeader.StartTime < Option.TimePeriod.Start)
                    {
                        this.CurrentHeader.StartTime = Option.TimePeriod.Start;
                    }
                    if (this.CurrentHeader.EndTime > Option.TimePeriod.End)
                    {
                        this.CurrentHeader.EndTime = Option.TimePeriod.End;
                    }
                }
                if (Option.IsEnableInterval)
                {
                    if (this.CurrentHeader.Interval < Option.Interval)
                    {
                        this.CurrentHeader.Interval = Option.Interval;
                    }
                }


                fileName = nameBuilder.Build(this.CurrentHeader);
            }

            return fileName;
        }

        /// <summary>
        /// 写头部
        /// </summary>
        /// <param name="header"></param>
        private void WriteHeader(RinexObsFileHeader header)
        {
            Writer.WriteHeader(header);
            IsHeaderWrited = true;
        }

        #region 统计观测类型
        /// <summary>
        /// 统计各个系统，各个观测类型的出勤率。
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        private static Dictionary<SatelliteType, Dictionary<string, double>> GetObsCodeRatios(Dictionary<SatelliteType, ObjectTableStorage> tables)
        {
            var ratios = new Dictionary<SatelliteType, Dictionary<string, double>>();
            foreach (var kv in tables)
            {
                var table = kv.Value;
                //统计观测码出现的有效比例
                Dictionary<string, double> ratioDic = new Dictionary<string, double>();
                foreach (var item in table.ParamNames)
                {
                    var vector = table.GetVector(item, 0, int.MaxValue, false, 0);
                     
                    var q = from n in vector where n != 0 select n;
                    var ratio = q.Count() * 1.0 / vector.Count;
                    ratioDic.Add(item, ratio);
                }
                ratios.Add(kv.Key, ratioDic);
            }
            return ratios;
        }

        /// <summary>
        /// 通过缓存，按照系统类型，转换为表格，以便统计。
        /// </summary>
        /// <returns></returns>
        private Dictionary<SatelliteType, ObjectTableStorage> BuildObsCodeTables( Dictionary<SatelliteType, List<string>> ObsCodes)
        {
            var SatelliteTypes = ObsCodes.Keys;
            var buffer = this.BufferedStream.MaterialBuffers;
            var tables = new Dictionary<SatelliteType, ObjectTableStorage>();

            foreach (var epoch in buffer)
            {
                foreach (var sat in epoch)
                {
                    var satType = sat.Prn.SatelliteType;
                    //如果已经过滤则不考虑了
                    if (!SatelliteTypes.Contains( satType ))
                    {
                        continue;
                    }

                    if (!tables.ContainsKey(satType))
                    {
                        tables.Add(satType, new ObjectTableStorage());
                    }
                    ObjectTableStorage table = tables[satType];
                    table.NewRow();
                    var types = ObsCodes[satType];
                    foreach (var obsType in sat.ObsTypes)
                    {
                        if (!types.Contains(obsType))
                        {
                            continue;
                        }

                        table.AddItem(obsType.ToString(), sat.TryGetValue(obsType));
                    }
                    table.EndRow();
                }
            }
            return tables;
        }
        #endregion
        /// <summary>
        /// 数据流。
        /// </summary>
        /// <returns></returns>
        protected override BufferedStreamService<RinexEpochObservation> BuildBufferedStream()
        {
            RinexObsFileReader stream = BuildDataSource();
            return new BufferedStreamService<RinexEpochObservation>(stream, BufferSize);
        }
        /// <summary>
        /// 构建数据源
        /// </summary>
        /// <returns></returns>
        protected RinexObsFileReader BuildDataSource()
        {
            return new RinexObsFileReader(FilePath);
        }
        /// <summary>
        /// 是否完成
        /// </summary>
        protected override void OnCompleted()
        {
            if (Writer != null)
            {
                Writer.Flush();
                Writer.Dispose();
            }
            base.OnCompleted();
        }
    }

}