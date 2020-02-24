//2015.10.09, czs, create in  xi'an hongqing, 周跳探测
//2015.10.09, czs, refactor in 彭州, 重构为通用文件输入输出执行类
//2015.11.12, czs, create in hongqing, 具有可选则并行计算参数文件输入输出的顶层操作。


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using System.Threading.Tasks;

namespace Geo.IO
{ 

     /// <summary>
    /// 具有可选则并行计算参数文件输入输出的顶层操作。
    /// </summary>
    public abstract class AbstractParallelableIoOperation<TParam> : AbstractIoOperation<TParam>
        where TParam : IoParam, IParallelableParam
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AbstractParallelableIoOperation()
        { 
        }

        #region 内部参数
        /// <summary>
        /// 起始时间 计时器
        /// </summary>
        protected DateTime StartTime;
        #endregion

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {   
            var reader = GetParamFileReader(); 
            foreach (var item in reader)
            {
                 StartTime = System.DateTime.Now; 

                CurrentParam = (TParam)item;
                var inPath = item.InputPath;
                var outPath = item.OutputPath;
                //输出检查和设置
                CheckOrSetOutputDirectory(outPath);
                // InputFileManager.LocalTempDirectory = OupputDirecory;
                //输入检查和设置
                var   FilePathes = InputFileManager.GetLocalFilePathes(inPath, WorkFileExtension, InputFileExtension);

                //执行计算
                if (CurrentParam.IsParallel)
                { 
                    ParallelProcess(item, FilePathes, outPath);
                }
                else { SerialProcess(item, FilePathes, outPath); }

                var span = DateTime.Now - StartTime;
                var avrPerFile = span.TotalSeconds / FilePathes.Count;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("操作 "+this.GetType()+"的参数 " + item + " 执行完成！耗时: " + span.ToString());
                sb.Append("并行度:\t" + item.ParallelProcessCount + "\t文件数:\t" + FilePathes.Count);
                sb.AppendLine("\t总时耗(m):\t" + span.TotalMinutes.ToString("0.0000") + "\t单文件时耗(s):\t " + avrPerFile.ToString("0.000"));
                sb.AppendLine("\t占用内存:\t" + Geo.Utils.ProcessUtil.GetProcessUsedMemoryString());
                //  log.Info(msg);
                log.Fatal(sb.ToString());
            }

            //if (Geo.Utils.FormUtil.ShowYesNoMessageBox(msg + "\r\n是否打开输出目录？") == System.Windows.Forms.DialogResult.Yes)
            //{
            //    Geo.Utils.FileUtil.OpenFileOrDirectory(this.OutputDirectory);
            //}

            return true;
        }

        /// <summary>
        /// 并行计算 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filePathes"></param>
        /// <param name="outPath"></param>
        protected virtual void ParallelProcess(TParam item, List<string> filePathes, string outPath)
        {
            Parallel.ForEach(filePathes, this.GetParallelOptions(item.ParallelProcessCount), (file, state) =>
            {
                var outFile = BuildOutputFilePath(outPath, file);
                CheckOrExecute(file, outFile, item.IsOverwrite);

                //是否终止计算//|| (PointPositioner !=null &&  PointPositioner.IsCancel)
                if (IsCancel) state.Break();
            });
        }

        /// <summary>
        ///  并行配置
        /// </summary>
        public virtual ParallelOptions GetParallelOptions(int ProcessCount)
        {
            ParallelOptions option = new ParallelOptions();
            option.MaxDegreeOfParallelism = ProcessCount;
            return option;
        }
        /// <summary>
        /// 串行计算
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filePathes"></param>
        /// <param name="outPath"></param>
        protected virtual void SerialProcess(TParam item, List<string> filePathes, string outPath)
        {
            foreach (var file in filePathes)
            {
                var outFile = BuildOutputFilePath(outPath, file);
                CheckOrExecute(file, outFile, item.IsOverwrite);
            }
            var Message = "已完成 " + item.InputPath + " 到 " + item.OutputPath;
            this.OnStatedMessageProduced(StatedMessage.GetProcessed(Message));
        }
    }

     
}
