//2015.10.09, czs, create in  xi'an hongqing, 周跳探测
//2015.10.09, czs, refactor in 彭州, 重构为通用文件输入输出执行类
//2015.10.28, czs, create in hongqing, 增加具有版本参数的操作

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
    /// 直接采用 IoParam 作为参数
   /// </summary>
    public abstract class AbstractIoOperation : AbstractIoOperation<IoParam> 
    {

    }

    /// <summary>
    /// 具有参数文件输入输出的顶层操作。
    /// </summary>
    public abstract class AbstractIoOperation<TParam> : ParamBasedOperation<TParam>
        where TParam : IoParam, IIoParam
    {
        public AbstractIoOperation(string InputFileExtension = "*.*O")
        {
            this.InputFileExtension = InputFileExtension;
            this.WorkFileExtension = InputFileExtension;
            this.InputFileManager = new InputFileManager(Setting.TempDirectory);
        }
        /// <summary>
        /// 需要指定输入输出类型的
        /// </summary>
        /// <param name="InputFileExtension"></param>
        /// <param name="WorkFileExtension"></param>
        public AbstractIoOperation(string InputFileExtension, string WorkFileExtension = "*.*O")
        {
            this.InputFileExtension = InputFileExtension;
            this.WorkFileExtension = WorkFileExtension;
            this.InputFileManager = new InputFileManager(Setting.TempDirectory);
        }

        #region 属性

        /// <summary>
        /// 输入文件管理器
        /// </summary>
        public InputFileManager InputFileManager { get; set; }


        /// <summary>
        /// 输入文件匹配类型，可以以分号分隔多个匹配类型
        /// </summary>
        public string InputFileExtension { get; set; }
        /// <summary>
        /// 执行时文件匹配类型，区别于输入文件匹配类型，如果不同则需要文件类型转换。，可以以分号分隔多个匹配类型
        /// </summary>
        public string WorkFileExtension { get; set; }
        /// <summary>
        /// 工程输出目录
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 临时目录,如网络下载的数据。
        /// </summary>
        public string TempDirecory { get { return InputFileManager.LocalTempDirectory; } set { InputFileManager.LocalTempDirectory = value; } } 

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
                this.CurrentParam = (TParam)item;

                var inPath = item.InputPath;
                var outPath = item.OutputPath; 

                //输出检查和设置
                CheckOrSetOutputDirectory(outPath);

                var inputFiles = InputFileManager.GetLocalFilePathes(inPath, WorkFileExtension, InputFileExtension);

                var files = Geo.Utils.FileUtil.GetFiles(inPath, InputFileExtension);
                foreach (var file in inputFiles)
                {
                    var outFile = BuildOutputFilePath(outPath, file);
                    CheckOrExecute(file, outFile, item.IsOverwrite);
                }

                var Message = "已执行 " + item.InputPath + " 到 " + item.OutputPath;
                this.OnStatedMessageProduced(StatedMessage.GetProcessed(Message));
            }
            return true;
        }

        /// <summary>
        /// 获取参数文件读取器
        /// </summary>
        /// <returns></returns> 
        protected virtual  LineFileReader<TParam> GetParamFileReader()
        {          
            return new LineFileReader<TParam>(this.OperationInfo.ParamFilePath);
        }



        /// <summary>
        /// 检查输出目录是否为空，如果是则设置。
        /// </summary>
        /// <param name="outPath"></param>
        protected void CheckOrSetOutputDirectory(string outPath)
        {
            if (OutputDirectory == null)
            {
                if (Geo.Utils.FileUtil.IsDirectory(outPath))
                {
                    this.OutputDirectory = outPath;
                }
                else
                {
                    OutputDirectory = Path.GetDirectoryName(outPath);
                }
            }

            Geo.Utils.FileUtil.CheckOrCreateDirectory(this.OutputDirectory);
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="fileInPath">输入文件路径</param>
        /// <param name="fileOutPath">输出文件路径</param>
        /// <param name="isOverwrite"></param>
        protected virtual void CheckOrExecute(string fileInPath, string fileOutPath, bool isOverwrite)
        {
            if (File.Exists(fileOutPath))
            {
                if (isOverwrite)
                {
                    var Message = "文件" + fileOutPath + "已存在，即将覆盖 ";
                    this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));
                    Execute(fileInPath, fileOutPath);
                }
                else
                {
                    var Message = "文件" + fileOutPath + "已存在，操作取消 ";
                    this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));
                }
            }
            else
            {
                var Message = "正在处理 " + fileInPath + " 到 " + fileOutPath;
                this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));

                Execute(fileInPath, fileOutPath);
            }
        }

        /// <summary>
        /// 建立输出文件路径，以此指定或标记程序输出类型。
        /// </summary>
        /// <param name="outPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual string BuildOutputFilePath(string outPath, string file)
        {
            var outFile = Geo.Utils.FileUtil.GetOutputFilePath(outPath, file);
            return outFile;
        }

       /// <summary>
       /// 具体的执行
       /// </summary>
       /// <param name="fileInPath"></param>
       /// <param name="fileOutPath"></param>
        protected abstract void Execute(string fileInPath, string fileOutPath);
    } 
}
