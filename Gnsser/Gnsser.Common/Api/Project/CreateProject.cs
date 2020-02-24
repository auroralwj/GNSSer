//2016.11.24, czs, create in hongqing, 创建工程

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;

namespace Gnsser.Api
{
    /// <summary>
    /// 创建工程,注意，此处参数借用IoParam，输入为工作流序列，输出为工程目录地址。
    /// </summary>
    public class CreateProject : ParamBasedOperation<IoParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateProject()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new IoParamReader(this.OperationInfo.ParamFilePath);
            reader.ItemSpliters = new string[] { "\t" };
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                if (File.Exists(item.OutputPath))
                {
                    if (item.IsOverwrite)
                    {
                        this.StatedMessage = StatedMessage.Processing;
                        this.StatedMessage.Message = "文件(夹)已存在，即将覆盖 " + item.OutputPath;
                        this.OnStatedMessageProduced();
                    }
                    else
                    {
                        this.StatedMessage = StatedMessage.Processing;
                        this.StatedMessage.Message = "文件(夹)已存在，操作取消 " + item.OutputPath;
                        this.OnStatedMessageProduced();
                    }
                }

               string directory =  Geo.Utils.FileUtil.GetDirectory(item.OutputPath);

                GnsserProject Project = new GnsserProject();
                Project.ProjectDirectory = directory;
                Project.CheckOrCreateProjectDirectories();
                //添加工作流
                Project.SetWorkflowFiles(item.InputPath);

                Project.SaveToFile();

                this.StatedMessage = StatedMessage.Processing;
                this.StatedMessage.Message = "已在 " + directory + " 创建工程目录 ";
                this.OnStatedMessageProduced(); 
                    
                //if (Geo.Utils.FileUtil.MoveFileOrDirectory(key.InputPath, key.OutputPath, key.IsOverwrite))
                //{
                //    this.StatedMessage = StatedMessage.Processing;
                //    this.StatedMessage.Message = "已移动 " + key.InputPath + " 到 " + key.OutputPath;
                //    this.OnStatedMessageProduced(); 
                //}

            }
            return true;
        }
    }
}
