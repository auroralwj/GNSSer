//2015.10.13, czs, create in hongqing, 工程配置文件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using System.IO;
using Geo.IO;
using Gnsser.Times;
using System.Configuration;
using Gnsser;
using Geo;

namespace Gnsser
{
    /// <summary>
    /// 工程配置文件
    /// </summary>
    public class GnsserProject : Config
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GnsserProject() :base()
        {
            this.Name = "Name";
            this.ProjectName = "ProjectName";
            this.Session = new BufferedTimePeriod(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now);
            this.SatelliteTypes = new List<SatelliteType> { SatelliteType.C, SatelliteType.G };
            //初始目录
            this.InitialDirectories();
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public GnsserProject(Config config)
            : base(config.Data, config.Comments)
        { 
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public GnsserProject(string configPath)
            : this(new ConfigReader(configPath).Read())
        {

        }

        #region 常用属性
        /// <summary>
        /// 工程文件路径.由工程名和工程目录自动生成。
        /// </summary>
        public string ProjectFilePath { get { return Path.Combine( ProjectDirectory, ProjectName + ".Gproj" ); } }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get { return GetString("ProjectName"); } set { Set( "ProjectName", value); } }

        /// <summary>
        /// 一个项目对应一个工作流，一个工作流由多个操作流文件组成。当前项目的工作流，在同一行存储。
        /// 不可以直接改变它！！
        /// </summary>
        public Workflow Workflow
        {
            get
            {
                Workflow workflow = new Workflow();
                var str = GetString("Workflow");
                var strs = str.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in strs)
                {
                    var workflowItem = new OperationFlow { FileName = item }; 
                    workflow.Add(workflowItem);
                }
                return workflow;
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in value)
                {
                    if (i != 0) sb.Append(";");
                    sb.Append(item);
                    i++;
                }
                var str = sb.ToString();
                SetWorkflowFiles(str);
            }
        }
        /// <summary>
        /// 设置工作流文件。文件以分号分隔。如 PppParallelTest.gof;CreateProjectTest.gof
        /// </summary>
        /// <param name="str"></param>
        public void SetWorkflowFiles(string str)
        {
            this.Set("Workflow", str);
        } 

        /// <summary>
        /// 卫星系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes
        {
            get
            {
                List<SatelliteType> satellites = new List<SatelliteType>();
                var str = GetString("SatelliteTypes");
                var strs = str.Split(new char[] { ',', ';', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in strs)
                {
                    satellites.Add((SatelliteType)Enum.Parse(typeof(SatelliteType), item));
                }
                return satellites;
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in value)
                {
                    if (i != 0) sb.Append(";");
                    sb.Append(item);
                    i++;
                }
                this.Set("SatelliteTypes", sb.ToString()); 
            }
        } 
        
        /// <summary>
        /// 有效时间，会话
        /// </summary>
        public BufferedTimePeriod Session
        {
            get
            {
                 var str = GetString("Session");
                 if (String.IsNullOrWhiteSpace(str))
                 {
                     return BufferedTimePeriod.Zero;
                 }
                 return  BufferedTimePeriod.Parse(str);
            }
            set
            {
                SetObj("Session", value, Common);
            }
        }
        #endregion

        #region 目录
        /// <summary>
        /// 项目目录.这是项目的根目录。一个工程中，只有该目录才是绝对路径！！！
        /// 并且该目录一旦切换后，自动采用新路径。
        /// </summary>
        public string ProjectDirectory
        {
            get { var path = GetString("ProjectDirectory"); if (String.IsNullOrWhiteSpace(path)) { path = "D:\\GnsserTemp"; ProjectDirectory = path; } return path; }
            set {
              //  var oldProjectPath = ProjectDirectory;

                Set("ProjectDirectory", value); 
                //更新其它目录的根目录
                //不用更新？？？ 
            }
        }
        /// <summary>
        /// 参数文件目录
        /// </summary>
        public string ParamDirectory { get { return GetProjectPath("ParamDirectory"); } set { SetProjectPath(value, "ParamDirectory"); } }

        /// <summary>
        /// 脚本文件，控制文件目录
        /// </summary>
        public string ScriptDirectory { get { return GetProjectPath("ScriptDirectory"); } set { SetProjectPath(value, "ScriptDirectory"); } }


        /// <summary>
        /// 原始观测文件目录
        /// </summary>
        public string ObservationDirectory { get { return GetProjectPath("ObservationDirectory"); } set { SetProjectPath(value, "ObservationDirectory"); } }

        /// <summary>
        /// 公共文件目录
        /// </summary>
        public string CommonDirectory { get { return GetProjectPath("CommonDirectory"); } set { SetProjectPath(value, "CommonDirectory"); } }

        /// <summary>
        /// 中间文件目录
        /// </summary>
        public string MiddleDirectory { get { return GetProjectPath("MiddleDirectory"); } set { SetProjectPath(value, "MiddleDirectory"); } }

        /// <summary>
        /// 输出文件目录
        /// </summary>
        public string OutputDirectory { get { return GetProjectPath("OutputDirectory"); } set { SetProjectPath(value, "OutputDirectory"); } }

        /// <summary>
        /// 矫正后（预处理后）文件目录
        /// </summary>
        public string RevisedObsDirectory { get { return GetProjectPath("RevisedObsDirectory"); } set { SetProjectPath(value, "RevisedObsDirectory"); } }
        #endregion
        /// <summary>
        /// 初始目录
        /// </summary>
        public void InitialDirectories()
        {
            this.RevisedObsDirectory = this.MiddleDirectory+@"\RevisedObservation\";
            this.OutputDirectory = @"Output\";
            this.MiddleDirectory = @"Middle\";
            this.CommonDirectory = @"Common\";
            this.ObservationDirectory = @"Observation\";
            this.ScriptDirectory = @"Script\";
            this.ParamDirectory =  this.ScriptDirectory + @"\Param\"; 
        }

        ///// <summary>
        ///// 由工程文件更新其余目录。
        ///// </summary>
        //public void UpdateWithProjectDirectory()
        //{
        //    this.RevisedObsDirectory = this.RevisedObsDirectory;
        //    this.OutputDirectory = this.OutputDirectory;
        //    this.MiddleDirectory = this.MiddleDirectory;
        //    this.CommonDirectory = this.CommonDirectory;
        //    this.ObservationDirectory = this.ObservationDirectory;
        //}

        /// <summary>
        /// 检查并创建工程目录。
        /// </summary>
        public void CheckOrCreateProjectDirectories()
        {
            Geo.Utils.FileUtil.CheckOrCreateDirectory(ProjectDirectory);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(this.ScriptDirectory);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(this.ParamDirectory);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(ObservationDirectory);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(CommonDirectory);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(MiddleDirectory);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(OutputDirectory);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(RevisedObsDirectory); 
        }

        /// <summary>
        /// 返回路径，完全路径。
        /// </summary>
        /// <param name="settingName">名称</param>
        /// <returns></returns>
        protected string GetProjectPath(string settingName)
        {
            String path = GetConfigValue(settingName);
            //是否包含了硬盘符号
            if (path.Contains(@":")) return path;
            //否则为相对路径。
            return Path.Combine(ProjectDirectory, path);
        }

        /// <summary>
        /// 设置相对于项目文档的路径,相对路径。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="settingName"></param>
        protected void SetProjectPath(string value, string settingName)
        {
            string path = value.Replace(ProjectDirectory, "");

            path = path.TrimStart('\\');
            path = path.TrimStart('/');

            SetConfigVlue(settingName, path, DataSource);
        }

        /// <summary>
        /// 加载文件。保存到 GnsserConfig 属性中。
        /// </summary>
        /// <returns></returns>
        public static GnsserProject LoadConfig(string configPath)
        { 
            var project = new GnsserProject(new ConfigReader(configPath).Read()); 
            return project;
        }
        
        /// <summary>
        /// 保存到文件中去
        /// </summary>
        public void SaveToFile()
        {
            SaveToFile(this.ProjectFilePath);
        }
        /// <summary>
        /// 保存到指定文件中去
        /// </summary>
        public void SaveToFile(string configPath)
        {
            Geo.IO.ConfigWriter writer = new Geo.IO.ConfigWriter(configPath);
            writer.Write(this);
            this.IsChangSaved = true;          
        }
        /// <summary>
        /// 去除操作流
        /// </summary>
        /// <param name="operFlow"></param>
        /// <param name="removeFile"></param>
        /// <returns></returns>
        public StatedMessage DeleteOperatonFlow(OperationFlow operFlow, bool removeFile = false)
        {
            var workflow = this.Workflow; 
            if (!workflow.Contains(operFlow))
            {
                var msg = StatedMessage.Faild;
                msg.Message = "本工程不包含该操作流啊！" + operFlow.FileName;
                return msg;
            }

            if (workflow.Remove(operFlow))
            {
                this.Workflow = workflow;
                this.SaveToFile();
            }

            if (removeFile)
            {
                var path = GetAbsScriptPath(operFlow.FileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else
                {
                    int a = 0;
                }
            }


            return StatedMessage.Ok;
        }
        /// <summary>
        /// 从外部导入，需要复制参数文件。直接保存到工程文件和操作流文件。
        /// </summary>
        /// <param name="inportGofPath"></param>
        /// <returns></returns>
        public StatedMessage ImportGofFile(string inportGofPath)
        {
            var workflow = this.Workflow;
            OperationFlow operFlow = OperationFlow.ReadFromFile(inportGofPath);
            if (workflow.Contains(operFlow))
            {
                var msg = StatedMessage.Faild;
                msg.Message = "已经具有该工作流，不用导入";
                return msg;
            }

            //不在工程目录内，
            if (!IsInProjectDirectory(inportGofPath))
            {
                //保存操作流到当前工程工作流
                operFlow.SaveToDirectory(this.ScriptDirectory);


                //检查参数文件，全部复制到 ParamDirectory 中
                CopyParamFileToCurrentProject(inportGofPath, operFlow);

            }
            //在目录内，直接添加
            //添加到当前工作流
            if (AddOperationFlow(operFlow))
            {
                return StatedMessage.Ok;
            }

            return StatedMessage.Ok;
        }

        /// <summary>
        /// 检查参数文件，全部复制到 ParamDirectory 中
        /// </summary>
        /// <param name="oldOperFlowPath"></param>
        /// <param name="operFlow"></param>
        private void CopyParamFileToCurrentProject(string oldOperFlowPath, OperationFlow operFlow)
        { 
            var newGofFolder = Path.GetDirectoryName(Geo.Utils.PathUtil.GetAbsPath(operFlow.FileName, this.ProjectDirectory));

            var oldGofFolder = Path.GetDirectoryName(oldOperFlowPath);
            foreach (var item in operFlow)
            {
                var oldParamAbsPath = Geo.Utils.PathUtil.GetAbsPath(item.ParamFilePath, oldGofFolder);
                if (File.Exists(oldParamAbsPath))
                {
                    var newParamPath = Geo.Utils.PathUtil.GetAbsPath(item.ParamFilePath, newGofFolder);

                    File.Copy(oldParamAbsPath, newParamPath, true);
                }
            }
        }
        /// <summary>
        /// 修改操作流的名称，并重命名操作流文件。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="overwrite"></param>
        public void RenameOperationFlow(OperationFlow obj, string name, bool overwrite = true)
        {
            var oldPath = this.GetAbsPath(obj.FileName);
            var newPath = this.GetAbsPath(name);

            obj.FileName = name;
            if (File.Exists(oldPath))
            {
                if (overwrite && File.Exists(newPath)) { File.Delete(newPath); }
                File.Move(oldPath, newPath);
            }
        }
        /// <summary>
        /// 添加到当前工作流,同时保存到文件
        /// </summary>
        /// <param name="operFlow"></param>
        /// <returns></returns>
        public bool AddOperationFlow(OperationFlow operFlow)
        {
            var workflow = this.Workflow;
            if (!workflow.Contains(operFlow))
            {
                workflow.Add(operFlow);
                this.Workflow = workflow;

                this.SaveToFile(this.ProjectFilePath); 
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取操作文件。
        /// </summary>
        /// <returns></returns>
        public  string [] GetAbsGofFilePathes()
        {
           return GetAbsScriptPath( Workflow.GofFileNames.ToArray());//.GofFileNames.ToArray()); 
        }

        /// <summary>
        /// 是否在脚本目录中
        /// </summary>
        /// <param name="absFlowPath"></param>
        /// <returns></returns>
        public bool IsInProjectDirectory(string absFlowPath)
        {
            return absFlowPath.ToUpper().Contains(this.ProjectDirectory.Trim().ToUpper());
        }

        /// <summary>
        /// 获取工作流的绝对路径
        /// </summary>
        /// <param name="pathInProject"></param>
        /// <returns></returns>
        public string GetAbsScriptPath(string pathInProject)
        {
            return Geo.Utils.PathUtil.GetAbsPath(pathInProject, this.ScriptDirectory);
        }

        /// <summary>
        /// 获取工作流的绝对路径
        /// </summary>
        /// <param name="pathInProject"></param>
        /// <returns></returns>
        public string [] GetAbsScriptPath(string [] pathInProject)
        {
            var pathes = new string [pathInProject.Length];
            for (int i = 0; i < pathInProject.Length; i++)
            {
                pathes[i] = Geo.Utils.PathUtil.GetAbsPath( pathInProject[i], this.ScriptDirectory);
            }
            return pathes;
        } 

        /// <summary>
        /// 获取绝对路径，如果已经是绝对路径则不做更改。
        /// </summary>
        /// <param name="scriptPathes"></param>
        /// <returns></returns>
        public string [] GetAbsPath(string[] scriptPathes)
        {
            string[] pathes = new string[scriptPathes.Length];
            for (int i = 0; i < pathes.Length; i++)
            {
                pathes[i] = GetAbsPath(scriptPathes[i]);
            }
            return pathes;
        } 

        /// <summary>
        /// 获取工作流的绝对路径
        /// </summary>
        /// <param name="pathInProject"></param>
        /// <returns></returns>
        public string GetAbsPath(string pathInProject)
        {
            return Geo.Utils.PathUtil.GetAbsPath(pathInProject, this.ProjectDirectory);
        } 
    }
}
