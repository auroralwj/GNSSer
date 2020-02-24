//2015.09.30, czs, create in xi'an hongqing, 数据处理引擎
//2015.10.21, czs, edit in xi'an hongqing, 名称修改为 OperationProcessEngine，增加批量gof文件处理

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;
using Gnsser;
using System.Threading.Tasks;

namespace Geo
{ 
    /// <summary>
    /// 操作处理委托
    /// </summary>
    /// <param name="oper"></param>
    public delegate void OperationEventHandler(IOperation oper);

    /// <summary>
    /// 数据遍历处理器
    /// </summary>  
    public class OperationProcessEngine
    {
        #region 构造函数与初始化
         
        
        /// <summary>
        /// 数据处理链条
        /// </summary>
        /// <param name="OperationManager"></param>
        /// <param name="gofPath"></param>
        public OperationProcessEngine(OperationManager OperationManager, string gofPath)
            : this(OperationManager, new OperationInfoReader(gofPath).ReadAll())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OperationManager"></param>
        /// <param name="OperationInfos"></param>
        public OperationProcessEngine(OperationManager OperationManager, List<OperationInfo> OperationInfos)
            : this(OperationManager)
        {
            this.OperationInfos = OperationInfos;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OperationManager"></param>
        public OperationProcessEngine(OperationManager OperationManager, bool isParallel = false, int processCount = 4)
        {
            this.IsParallel = isParallel;
            this.ProcessCount = processCount;
            this.SetAndInitOperatinManager(OperationManager);
            this.OperationInfos = new List<OperationInfo>();
        }
        private void SetAndInitOperatinManager(OperationManager OperationManager)
        {
            this.OperationManager = OperationManager;
            foreach (var item in OperationManager)
            {
                item.StatedMessageProduced += item_StatedMessageProduced;
            }
        }

        void item_StatedMessageProduced(StatedMessage StatedMessage)
        {
            if (OperationStatedMessageProduced != null) OperationStatedMessageProduced(StatedMessage);
        }
        #endregion

        #region 事件、属性、变量
        /// <summary>
        /// 处理器数量
        /// </summary>
        public int ProcessCount { get; set; }
        /// <summary>
        /// 是否并行计算
        /// </summary>
        public bool IsParallel { get; set; }
        /// <summary>
        /// 操作即将执行
        /// </summary>
        public event OperationEventHandler OperationProcessing;
        /// <summary>
        /// 操作已经执行完毕
        /// </summary>
        public event OperationEventHandler OperationCompleted;
        /// <summary>
        /// 处理细节信息
        /// </summary>
        public event StatedMessageProducedEventHandler OperationStatedMessageProduced;
        /// <summary>
        /// 一些全局设置
        /// </summary>
        public GnsserConfig GnsserConfig { get; set; }
         
        /// <summary>
        /// 根目录。
        /// </summary>
        public string BaseDirecory { get; set; }
        /// <summary>
        /// 当前操作
        /// </summary>
        public IOperation CurrentOperation{ get; set; }
        /// <summary>
        /// 取消执行！
        /// </summary>
        public bool CancelProcessing { get {
            if (CurrentOperation == null) return true;
            return CurrentOperation.IsCancel;
        }
            set {
                if (CurrentOperation!= null)
                CurrentOperation.IsCancel = value;
            }
        }
        /// <summary>
        /// 当前是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// 操作信息读取器
        /// </summary>
        public OperationManager OperationManager { get; set; }

        /// <summary>
        /// 操作信息读取器
        /// </summary>
        public List<OperationInfo> OperationInfos { get; set; }
        #endregion

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <returns></returns>
        public void Process()
        {
            IsRunning = true;

            InitAndAccept();

            if (this.IsParallel)
            {
                ParallelProcess(); 
            }
            else
            {
               SerialProcess(); 
            }
        }
        /// <summary>
        /// 串行计算
        /// </summary>
        /// <returns></returns>
        private void SerialProcess()
        {
            foreach (var item in OperationInfos)
            {
                DoOperation(item);

                //是否取消执行
                if (CancelProcessing)
                {
                    CancelProcessing = false;
                    break;
                }
            }
            this.IsRunning = false; 
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="key"></param>
        private void DoOperation(OperationInfo item)
        {
            var oper = OperationManager.Get(item.OperationName);
            DoOperation(oper);
        }

        /// <summary>
        /// 执行一个操作。
        /// </summary>
        /// <param name="oper"></param>
        private void DoOperation(IOperation oper)
        {
            this.CurrentOperation = oper;

            //执行前通知
            if (OperationProcessing != null) { this.OperationProcessing(oper); }

            //执行
            if (!oper.Do())
            {
                throw new GeoException(oper.StatedMessage.Message);
            }
            //执行后通知
            OnOperationFinished(oper);
        }

        /// <summary>
        /// 初始化并进行参数检核。
        /// </summary>
        private void InitAndAccept()
        {
            //init
            foreach (var item in OperationInfos)
            {
                var oper = OperationManager.Get(item.OperationName);
                this.CurrentOperation = oper;

                if (oper is IWithGnsserConfig)
                {
                    (oper as IWithGnsserConfig).GnsserConfig = GnsserConfig;
                }

                if (oper == null) { throw new GeoException("请先注册操作再试！" + item.OperationName); }

                //检核参数文件，如文件的存在性，将相对路径修改为绝对路径
                oper.Accept(item);
            }
        }

        /// <summary>
        /// 并行计算，按照各个操作的依赖关系执行。
        /// </summary>
        public void ParallelProcess()
        {
            //根据依赖关系，建立并行树
            List<List<OperationInfo>> operTree = new DependentTreeBuilder(this.OperationInfos).Build();

            foreach (var list in operTree)
            {
                Parallel.ForEach(list, this.ParallelOptions, (oper, state) =>
                {
                    DoOperation(oper);
                    //是否终止计算//|| (PointPositioner !=null &&  PointPositioner.IsCancel)
                    //是否取消执行
                    if (CancelProcessing)
                    {
                        CancelProcessing = false;
                        state.Stop();
                    }
                });
            }
        }
        /// <summary>
        ///  并行配置
        /// </summary>
        public ParallelOptions ParallelOptions
        {
            get
            {
                ParallelOptions option = new ParallelOptions();
                option.MaxDegreeOfParallelism = ProcessCount;
                return option;
            }
        }

        private void OnOperationFinished(IOperation oper)
        {
            if (this.OperationCompleted != null) { this.OperationCompleted(oper); }
        }
        /// <summary>
        /// 设置数据流，清空以往。
        /// </summary>
        /// <param name="gofPathes"></param>
        public void SetGofes(string[] gofPathes)
        {
            this.Clear();
            this.AddGofes(gofPathes);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this.OperationInfos.Clear();
        }
        /// <summary>
        /// 添加操作文件到当前任务.如果当前有任务正在执行，则添加到其后。
        /// </summary>
        /// <param name="gofPathes"></param>
        public void AddGofes(string[] gofPathes)
        {
            foreach (var gofPath in gofPathes)
            {
                var infos = new OperationInfoReader(gofPath).ReadAll();
                this.OperationInfos.AddRange(infos);
            }
        }

    }


    //2015.11.06, czs, create in 西安五路口袁记肉夹馍, 依赖树生成器

    /// <summary>
    /// 依赖树生成器
    /// </summary>
    public class DependentTreeBuilder
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="OperationInfos"></param>
        public DependentTreeBuilder(List<OperationInfo> OperationInfos)
        {
            this.OperationInfos = OperationInfos;

            //检核是否具有同名任务
            foreach (var item in OperationInfos)
            {
               if( OperationInfos.FindAll(m => m.Name == item.Name).Count > 1)
               {
                   throw new ArgumentException("任务名称不可重复！" + item.Name);
               }
            }

        }

        /// <summary>
        /// 原始操作列表。默认名称不能一样！！！
        /// </summary>
        public List<OperationInfo> OperationInfos {get;set;}


        public List<List<OperationInfo>> Build()
        {
            //根据依赖关系，建立并行树
            List<List<OperationInfo>> operTree = new List<List<OperationInfo>>();
            //首先查找没有依赖的列为第一梯队
            List<OperationInfo> firsts = OperationInfos.FindAll(m => !m.HasDepends);
            operTree.Add(firsts);

            List<OperationInfo> lowers = OperationInfos.FindAll(m => m.HasDepends);
            List<OperationInfo> uppers = new List<OperationInfo>(firsts);

            while (lowers.Count > 0)
            {
                //解析具有依赖的
                List<OperationInfo> nexts = GetNextOperationInfos(uppers, lowers);
                if (nexts.Count == 0) { break; } //如果没有下一个了，无论解析完全没有都退出。
                operTree.Add(nexts);
                uppers.AddRange(nexts);
                foreach (var item in nexts)
                {
                    lowers.Remove(item);
                }
            }

            return operTree;
        }

        /// <summary>
        /// 提取下层中依赖已经全部在上层的操作信息。
        /// </summary>
        /// <param name="upppers"></param>
        /// <param name="lowers"></param>
        /// <returns></returns>
        public static List<OperationInfo> GetNextOperationInfos(List<OperationInfo> upppers, List<OperationInfo> lowers )
        {
            List<OperationInfo> nexts = new List<OperationInfo>();
            foreach (var low in lowers)
            {
                foreach (var depend in low.Depends)
                {
                    //必须全部包含，才可以计算哦
                    var find = upppers.Find(m => String.Equals(depend, m.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (find == null)
                    {
                        break;
                    }
                }
                nexts.Add(low);
            }
            return nexts;
        }
    }
}
