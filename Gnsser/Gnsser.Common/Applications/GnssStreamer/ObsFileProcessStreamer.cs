//2016.08.20, czs, create in 福建永安, 宽项计算器，多站观测数据遍历器
//2016.08.29, czs, edit in 西安洪庆, 重构多站观测数据遍历器
//2016.11.19，czs, refact in hongqing, 提取更通用的观测文件数据流
//2018.07.29, czs, edit in HMX, 修改通用GNSS数据流执行器


using System;
using System.IO;
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
    /// 通用GNSS数据流执行器。
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class ObsFileProcessStreamer<TMaterial, TResult> :
        ObsFileEpochRunner<TMaterial>, IObsFileProcessStreamer<TMaterial>, Namable where TMaterial : ISiteSatObsInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ObsFileProcessStreamer()
        {
            this.RunnerFileExtension = Setting.RinexOFileFilter;
            this.IsInReversing = false;
            this.IsRawReviseBreaked = false;
            this.RawReviseBreakTime = Time.MinValue;
        }
        /// <summary>
        /// 是否输出结果
        /// </summary>
        public bool IsOutputResult { get; set; }

        #region  事件 
        /// <summary>
        /// 产生一个产品
        /// </summary>
        public event Action<TResult, ObsFileProcessStreamer<TMaterial, TResult>> ResultProduced;
        /// <summary>
        /// 完成
        /// </summary> 
        protected override void OnCompleted()
        {
            base.OnCompleted();

            this.RawReviser.Complete();
            this.ProducingReviser.Complete();

            if (Option.IsOutputEpochResult) { TableTextManager.WriteAllToFileAndCloseStream(); }
        }
        #endregion

        #region 属性

        /// <summary>
        /// 计算器
        /// </summary>
        public IGnssSolver Solver { get; set; }
        /// <summary>
        /// GNSS结果生成器。
        /// </summary>
        public GnssResultBuilder GnssResultBuilder { get; set; }
        /// <summary>
        /// 平差详细表格
        /// </summary>
        public AioAdjustFileBuilder AioAdjustFileBuilder { get; set; }
        /// <summary>
        /// 观测方程存储
        /// </summary>
        public AdjustEquationFileBuilder AdjustEquationFileBuilder { get; set; }
        /// <summary>
        /// 星历结束时间
        /// </summary>
        public Time EphemerisEndTime { get; set; }

        /// <summary>
        /// GNSS计算选项，需要在 Init 函数之前设置
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 数据上下文
        /// </summary>
        public DataSourceContext Context { get; set; }
        /// <summary>
        /// 数据检查
        /// </summary>
        public IChecker<TMaterial> EpochChecker { get; set; }

        /// <summary>
        /// 数据第一次加载（到缓存）时执行。
        /// </summary>
        public IReviser<TMaterial> RawReviser { get; set; }
        /// <summary>
        /// 矫正赋值器,在计算前一刻执行。
        /// </summary>
        public IReviser<TMaterial> ProducingReviser { get; set; }
        /// <summary>
        /// 当前计算结果
        /// </summary>
        public TResult CurrentGnssResult { get; private set; }
        /// <summary>
        /// 星历数据源
        /// </summary>
        public IEphemerisService EphemerisDataSource { get; set; }
        /// <summary>
        /// 钟差
        /// </summary>
        public Data.ISimpleClockService ClockFile { get; set; }
        /// <summary>
        /// 指示，当前是否正在正反算。
        /// </summary>
        public bool IsInReversing { get; set; }
        /// <summary>
        /// 观测数据
        /// </summary>
        public IObservationStream<TMaterial> DataStream { get { return this.BufferedStream.DataSource as IObservationStream<TMaterial>; } }
        #region  正反算与先验值赋予
        /// <summary>
        /// 当前是否数据源逆序
        /// </summary>
        public bool IsReversedDataSource { get; private set; }
        /// <summary>
        /// 正反算剩余计算次数
        /// </summary>
        public int OrdinalAndReverseCount { get; private set; }

        #endregion
        #endregion

        #region 方法
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            this.TableTextManager = new ObjectTableManager(Option.OutputBufferCount, Option.OutputDirectory);
            this.TableTextManager.Clear();

            if (this.Option == null) { this.Option = BuildGnssOption(); }


            this.IsOutputResult = this.Option.IsOutputResult;
            //正反算与先验值赋予
            this.IsReversedDataSource = Option.IsReversedDataSource;
            this.OrdinalAndReverseCount = Option.OrdinalAndReverseCount;

            SetDataStreamAndInit();
        }

        /// <summary>
        /// 生成解算选项
        /// </summary>
        /// <returns></returns>
        protected virtual GnssProcessOption BuildGnssOption()
        {
            if (Option == null) { Option = new GnssProcessOption(); }
            Option.OutputDirectory = this.OutputDirectory;
            return Option;
        }

        /// <summary>
        /// 设置数据流并且初始化
        /// </summary>
        public void SetDataStreamAndInit()
        {
            // if (this.BufferedStream == null || this.BufferedStream.DataSource == null)
            {
                BuildAndSetDataStream();
            }
            this.AioAdjustFileBuilder = new AioAdjustFileBuilder(Option.OutputDirectory, BufferedStream.Name + ".adjust");

            this.AdjustEquationFileBuilder = new AdjustEquationFileBuilder(Option.OutputDirectory, BufferedStream.Name + ".equation");

            //if (this.Context == null)
            {
                this.Context = BuildContext(DataStream);
                this.EphemerisDataSource = this.Context.EphemerisService;
                if (this.EphemerisDataSource != null)
                {
                    this.EphemerisEndTime = this.EphemerisDataSource.TimePeriod.BufferedEnd;
                }
            }

            //if (Option.ProcessType != ProcessType.仅计算)
            {
                this.EpochChecker = BuildChecker();
                this.RawReviser = BuildRawReviser();
                this.RawReviser.Buffers = BufferedStream.MaterialBuffers;
            }

            this.ProducingReviser = BuildProducingReviser();
            this.ProducingReviser.Buffers = BufferedStream.MaterialBuffers;

            //坐标和信息设置器
            ApproxCoordSetter<TMaterial> coordSetter = new ApproxCoordSetter<TMaterial>(BufferedStream, Option, Context);
            coordSetter.CheckOrSetApproxXyz();//检查或设置初始坐标          
            coordSetter.CheckOrSetDatasourceApproxXyz();  //检查或检查近似坐标           
            coordSetter.CheckOrUpdateStationInfo(); //检查或更新测站信息
        }

        /// <summary>
        /// 构建并初始化观测数据流
        /// </summary>
        protected void BuildAndSetDataStream()
        {
            this.BufferedStream = BuildBufferedStream();
            this.BufferedStream.SetEnumIndex(this.Option.StartIndex, this.Option.CaculateCount);
            this.BufferedStream.MaterialBuffersFullOrEnd += OnMaterialBuffersFullOrEnd;
            this.BufferedStream.MaterialEnded += BufferedStream_MaterialEnded;
            this.BufferedStream.MaterialCheck += CheckMaterial;
            this.BufferedStream.AfterMaterialCheckPassed += OnAfterMaterialCheckPassed;
            log.Info(BufferedStream.Name + " 起始处理编号: " + BufferedStream.StartIndex + ", 最大处理数量: " + BufferedStream.EnumCount);
        }

        private void BufferedStream_MaterialEnded()
        {
        }

        /// <summary>
        /// 构建数据上下文
        /// </summary>
        /// <returns></returns>
        protected virtual DataSourceContext BuildContext(IObservationStream<TMaterial> DataStream)
        {
            return DataSourceContext.LoadDefault(Option, DataStream, EphemerisDataSource, ClockFile);
        }

        /// <summary>
        /// 计算器
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        /// <returns></returns>
        protected IGnssSolver BuildRolver(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        {
            if (IsInReversing && Solver != null)
            {
                return Solver;
            }

            return GnssSolverFactory.Create(DataSourceContext, GnssOption);
        }


        /// <summary>
        /// 构建检核器
        /// </summary>
        /// <returns></returns>
        protected abstract IChecker<TMaterial> BuildChecker();
        /// <summary>
        /// 构建初始矫正器，通常对数据进行简单的判断，注册等。
        /// </summary>
        /// <returns></returns>
        protected abstract IReviser<TMaterial> BuildRawReviser();
        /// <summary>
        /// 构建精细矫正器，在即将生产之前。
        /// </summary>
        /// <returns></returns>
        protected abstract IReviser<TMaterial> BuildProducingReviser();

        #endregion

        #region 计算过程

        /// <summary>
        /// 正式运行
        /// </summary>
        public override void Run()
        {
#if !DEBUG
            try
            {
#endif
            if (!InitCheck())
            {
                log.Error(this.Name + ", 数据初始检核未通过。计算失败！");
                return;
            }
            //运行主体程序
            Running();
            //检查并运行正反算
            CheckAndRunReverseSove();
#if !DEBUG
            }
            catch (Exception ex)
            {
                var msg = Current.Name + ", 计算发生错误：" + ex.Message;
                log.Error(msg);
                if (Setting.GnsserConfig.IsDebug)
                {
                    ex.Data["GNSSerMsg"] = msg;
                    throw ex;
                }
            }
            finally
            {
            }
#endif
            PostRun();
            Complete();
        }

        /// <summary>
        /// 运行中
        /// </summary>
        public virtual void Running()
        {
            CurrentIndex = -1;
            foreach (var mEpochInfo in BufferedStream)
            {
                CurrentIndex++;
                OnEpochEntityProduced(mEpochInfo);

                var controlType = PreProcess(mEpochInfo);

                if (controlType == LoopControlType.Continue) { continue; }
                if (controlType == LoopControlType.Break) { break; }
                if (controlType == LoopControlType.Return) { return; }

                //3.计算 
                Process(mEpochInfo);


                //4.显示输出 

                //通知显示
                OnInfoProduced("当前进度：" + CurrentIndex);
            }
        }

        /// <summary>
        /// 处理历元
        /// </summary>
        /// <param name="material"></param>
        public override void Process(TMaterial material)
        {
            var newResult = Produce(material);
            if (newResult == null) { return; }

            PostProcess(newResult);

            //检核是否合格
            if (CheckResult(newResult))
            {
                OnResultProduced(newResult, this);

                //历元输出
                if (!Option.TopSpeedModel && Option.IsOutputResult && this.Option.IsOutputEpochResult)
                {
                    OutputEpochResult(material, newResult);
                }

                this.SetResult(newResult);
            }
        }


        /// <summary>
        /// 设置结果。
        /// </summary>
        /// <param name="newResult"></param>
        private void SetResult(TResult newResult)
        {
            this.CurrentGnssResult = newResult;
        }

        /// <summary>
        /// 计算，返回计算结果，不要直接赋予给当前结果。
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public abstract TResult Produce(TMaterial material);

        /// <summary>
        /// 处理后执行
        /// </summary>
        protected virtual void PostProcess(TResult newResult)
        {

        }

        /// <summary>
        /// 检核结果，如果成功则添加到当前否则不添加。
        /// </summary>
        protected abstract bool CheckResult(TResult newResult);

        /// <summary>
        /// 处理后执行
        /// </summary>
        public override void PostRun()
        {
            base.PostRun();
            //是否正反算
            if (this.OrdinalAndReverseCount <= 0)
            {
                base.PostRun();
                if (Option.IsOutputAdjustMatrix) { GnssResultBuilder.WriteAdjustMatrixText(); }
                //WriteResultsToFileAndClearBuffer();
            }
            else
            {
                CheckAndRunReverseSove();
            }
            if (Solver != null)
            {
                Solver.Complete();
            }

            CheckOrOutputCycleSlipFile();
        }

        /// <summary>
        /// 检查并进行正反算
        /// </summary>
        protected void CheckAndRunReverseSove()
        {
            while (this.OrdinalAndReverseCount > 0)
            {
                // GnssResultBuilder.Clear();

                this.IsInReversing = true;

                this.OrdinalAndReverseCount--;
                this.IsReversedDataSource = !this.IsReversedDataSource;

                var info = "数据流 即将进行 ";
                info += (this.IsReversedDataSource ? "反" : "正") + "算, ";
                info += "剩余单独计算次数 " + this.OrdinalAndReverseCount;

                log.Info(info);

                //Init();
                // BuildAndSetDataStream();//按照指定顺序，重新初始化数据流
                SetDataStreamAndInit();

                //是否清空输出缓存
                if (Option.IsClearOutBufferWhenReversing)
                {
                    ClearResultBuffer();

                    this.TableTextManager.Clear();
                }

                Running();

                var last1 = this.CurrentGnssResult;
                if (last1 != null && last1 is SimpleGnssResult)
                {
                    SimpleGnssResult last = last1 as SimpleGnssResult;
                    log.Info(GnssResultBuilder.BuildFinalInfo(last));

                    log.Fatal(last.Name + "\t" + last.ReceiverTime + "\t" + "ParamNames\t" + String.Format(new EnumerableFormatProvider(), "{0:\t}", last.ParamNames));
                    log.Fatal(last.Name + "\t" + last.ReceiverTime + "\t" + "RmsOfEstimated\t" + FormatVector(last.ResultMatrix.Estimated.GetRmsVector()));
                    log.Fatal(last.Name + "\t" + last.ReceiverTime + "\t" + "Estimated\t" + FormatVector(last.ResultMatrix.Estimated));
                }
            }

            this.IsInReversing = false;
        }

        /// <summary>
        /// 检核新读入数据是否合格，合格才加入缓存。
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        protected override bool CheckMaterial(TMaterial material)
        {
            if (Option.ProcessType == ProcessType.仅计算) { return true; }

            //是否超过有效的星历时间
            // if (material.ReceiverTime > EphemerisEndTime){ return false;}

            //检核数据
            if (!EpochChecker.Check(material))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 数据刚刚读入
        /// </summary>
        /// <param name="material"></param>
        protected override void OnAfterMaterialCheckPassed(TMaterial material)
        {
            //if (Option.ProcessType == ProcessType.仅计算) { return; }
            if (this.IsCancel) { return; }

            RawRevise(material);
        }
        /// <summary>
        /// 初始矫正是否中断了。
        /// </summary>
        public bool IsRawReviseBreaked { get; set; }
        /// <summary>
        /// 初始矫正中断时间。
        /// </summary>
        public Time RawReviseBreakTime { get; set; }


        /// <summary>
        /// 数据刚刚进入，尚未加入缓存时执行。
        /// </summary>
        /// <param name="material"></param>
        public override void RawRevise(TMaterial material)
        {
            if (IsRawReviseBreaked)
            {
                return;
            }

            var m = material;
            if (m.EnabledSatCount == 0)
            {
                log.Error(m + ", 可用卫星为 0 ，本历元无法计算 " + m.ReceiverTime); return;
            }

            RawReviser.Buffers = this.BufferedStream.MaterialBuffers;//初始矫正，缓存为0，不需要 2016.09.26. 
            //czs,2018.05.20, 这是需要的，如缓存多项式拟合伪距
            if (!RawReviser.Revise(ref m))
            {
                IsRawReviseBreaked = true;
                RawReviseBreakTime = m.ReceiverTime;
                //初始矫正错误是出现在缓存中，不中断，2019.05.15，czs， 洪庆
                // this.IsCancel = true;
                log.Warn("初始校正发生错误，计算将在此取消！");
            }

            //可以用于测试所有改正数加完。
            //this.ProducingReviser.Revise(ref m);
        }
        #endregion

        /// <summary>
        /// 历元数据预处理
        /// </summary>
        /// <param name="mEpochInfo"></param>
        /// <returns></returns>
        public virtual LoopControlType PreProcess(TMaterial mEpochInfo)
        {
            var loop = CheckLoop(mEpochInfo);
            if (loop != LoopControlType.GoOn) { return loop; }

            return ProducingRevise(mEpochInfo);
        }

        /// <summary>
        /// 算前数据矫正
        /// </summary>
        /// <param name="mEpochInfo"></param>
        /// <returns></returns>
        public virtual LoopControlType ProducingRevise(TMaterial mEpochInfo)
        {
            //2.赋值与矫正数据
            var val = mEpochInfo;
            ProducingReviser.Buffers = BufferedStream.MaterialBuffers;
            if (!ProducingReviser.Revise(ref val)) { return LoopControlType.Continue; }

            return LoopControlType.GoOn;
        }
        /// <summary>
        /// 循环条件检查，是否继续
        /// </summary>
        /// <param name="mEpochInfo"></param>
        /// <returns></returns>
        protected virtual LoopControlType CheckLoop(TMaterial mEpochInfo)
        {
            if (IsCancel)
            {
                log.Info("计算被手动取消。");
                return LoopControlType.Break;
            }

            if (IsRawReviseBreaked)
            {
                if (this.RawReviseBreakTime <= mEpochInfo.ReceiverTime)
                {
                    log.Warn(mEpochInfo + ", 已经超出初始矫正出错时间 "+ RawReviseBreakTime + "， 计算即将结束！");
                    this.IsCancel = true;
                    return LoopControlType.Break;
                }

                if (this.Option.IsEphemerisRequired && mEpochInfo.GetEpochSatWithEphemeris().Count == 0)
                {
                    log.Warn("指定需要星历，但此历元没有星历，计算取消。" + mEpochInfo.ReceiverTime);
                    return LoopControlType.Break;
                }
            }


            if (this.EphemerisEndTime < mEpochInfo.ReceiverTime)
            {
                log.Error(mEpochInfo + ",已经超出星历服务范围:" + this.Context.EphemerisService.TimePeriod + "，无法计算 " + mEpochInfo.ReceiverTime + ",计算终止！");
                this.IsCancel = true;
                return LoopControlType.Break;
            }

            return LoopControlType.GoOn;
        }
        #endregion

        #region 输出

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="result"></param>
        private void OutputEpochResult(ISiteSatObsInfo epoch, TResult result)
        {
            GnssResultBuilder.AddEpochResult(epoch, result as SimpleGnssResult);
        }
        public override void Complete()
        {
            this.RawReviser.Complete();
            ProducingReviser.Complete();

            if (GnssResultBuilder != null) { GnssResultBuilder.Complete(); }

            base.Complete();
        }
        /// <summary>
        /// 产生了结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="streamer"></param>
        private void OnResultProduced(TResult result, ObsFileProcessStreamer<TMaterial, TResult> streamer)
        {
            if (ResultProduced != null) { ResultProduced(result, streamer); }
        }
        /// <summary>
        /// 检查和输出周跳文件
        /// </summary>
        private void CheckOrOutputCycleSlipFile()
        {

            //周跳输出
            if (Option.IsOutputCycleSlipFile)
            {
                var revisers = this.RawReviser as EpochInfoReviseManager;
                if (revisers == null) { return; }
                foreach (var reviser in revisers.Precessors)
                {
                    if (reviser is EpochInfoReviseManager)
                    {
                        foreach (var csReviserOr in ((EpochInfoReviseManager)reviser).Precessors)
                        {
                            var csReviwer = csReviserOr as CycleSlipDetectReviser;
                            if (csReviwer == null) { continue; }
                            csReviwer.WriteStorageToFile(OutputDirectory, this.BufferedStream.Name, Option.ObsDataType);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 写入文件并清空内存
        /// </summary>
        public void WriteResultsToFileAndClearBuffer()
        {
            WriteResultsToFile();
            ClearResultBuffer();
        }
        /// <summary>
        /// 写入文件并清空内存
        /// </summary>
        public void WriteResultsToFile()
        {
            TableTextManager.WriteAllToFileAndCloseStream();
            AioAdjustFileBuilder.WriteToFile();
            AdjustEquationFileBuilder.WriteToFile();
        }
        /// <summary>
        /// 清空内存
        /// </summary>
        public void ClearResultBuffer()
        {
            AdjustEquationFileBuilder.Clear();
            AioAdjustFileBuilder.Clear();
            if (this.IsClearTableWhenOutputted) { TableTextManager.Clear(); }
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public string FormatVector(Geo.Algorithm.IVector vector)
        {
            return String.Format(new EnumerableFormatProvider(), "{0:\t}", vector);
        }
        #endregion
    }

}