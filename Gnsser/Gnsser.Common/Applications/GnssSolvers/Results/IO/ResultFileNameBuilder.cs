//2014.12.03, czs, create in jinxingliaomao shuangliao, 定位信息写入器
//2014.12.06, czs, edit in jinxingliaomao shuangliao, 历元目录加入日期，利于符合多历元计算
//2015.12.25, czs, edit in hongqing, 改进，简化，面向用户
//2016.10.01, czs, edit in hongqing, 优化代码，修复错误
//2018.11.28, czs, edit in hmx, 修改代码
//2019.01.12, czs, edit in hmx, 结果存储在对应的算法目录

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Gnsser;
using Gnsser.Domain;
using Gnsser.Service; 
using Gnsser.Data.Rinex;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Common;
using Geo;
using Geo.Algorithm;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 定位信息写入器
    /// </summary>
    public class ResultFileNameBuilder
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog log = Log.GetLog(typeof(ResultFileNameBuilder));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ProjectOutputDirectory"></param>
        /// <param name="GnssSolverType"></param>
        public ResultFileNameBuilder(string ProjectOutputDirectory)
        {
            this.ProjectOutputDirectory = ProjectOutputDirectory;
        }
        /// <summary>
        /// 工程输出目录
        /// </summary>
        public string ProjectOutputDirectory { get; set; }
        /// <summary>
        /// 计算器目录
        /// </summary>
        public string SolverDirectory { get { Geo.Utils.FileUtil.CheckOrCreateDirectory(ProjectOutputDirectory); return ProjectOutputDirectory; } }// Path.Combine(ProjectOutputDirectory, GnssSolverType.ToString()); }

        /// <summary>
        /// 获取基线结果文件
        /// </summary>
        /// <returns></returns>
        public string GetBaseLineResultFile()
        {
            var files = Directory.GetFiles(SolverDirectory, "*" + Setting.BaseLineFileExtension);
            if (files.Length > 0) { return files[0]; }
            return null;
        }
        /// <summary>
        /// 最终输出结果
        /// </summary>
        /// <param name="gnssResultType"></param>
        /// <returns></returns>
        public string BuildFinalResultFilePath(Type gnssResultType)
        {
            string allPath = Path.Combine(SolverDirectory, gnssResultType.Name + Setting.EpochCoordFileExtension);
            if (gnssResultType.IsSubclassOf( typeof( DualSiteEpochDoubleDifferResult))
                || gnssResultType.IsSubclassOf(typeof(TwoSitePeriodDifferPositionResult))
                || gnssResultType == (typeof(NetDoubleDifferPositionResult))
                )//基线
            {
                var name = gnssResultType.Name;
                allPath = BuildBaseLineResulPath( name);
            }
            else if (gnssResultType.IsSubclassOf(typeof(SingleSiteGnssResult)))
            {
                allPath = Path.Combine(SolverDirectory, gnssResultType.Name + Setting.SiteCoordFileExtension);
            }
             
            return allPath;
        }
        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        public List<string> GetAllTwoSiteSolverEpochParamFileName(string materialName)
        {
            var names = Enum.GetNames(typeof(TwoSiteSolverType));
            return GetAllEpochParamFileName(materialName, names);
        }
        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        public List<string> GetAllSolverEpochParamFileName(string materialName)
        {
            var names = Enum.GetNames(typeof(GnssSolverType));
            return GetAllEpochParamFileName(materialName, names);
        }
        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        public List<string> GetAllSingleSiteGnssSolverEpochParamFileName(string materialName)
        {
            var names = Enum.GetNames(typeof(SingleSiteGnssSolverType));
            return GetAllEpochParamFileName(materialName, names);
        }

        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        public List<string> GetAllEpochResidualFileName(string materialName)
        {
            var names = Enum.GetNames(typeof(GnssSolverType));
            return GetAllEpochResidualFileName(materialName, names);
        }

        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        public List<string> GetAllTwoSiteSolverResidualFileName(string materialName)
        {
            var names = Enum.GetNames(typeof(TwoSiteSolverType));
            return GetAllEpochResidualFileName(materialName, names);
        }
        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        public List<string> GetAllSingleSiteGnssSolverResidualFileName(string materialName)
        {
            var names = Enum.GetNames(typeof(SingleSiteGnssSolverType));
            return GetAllEpochResidualFileName(materialName, names);
        }
        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <returns></returns>
        public List<string> GetAllMultiSiteNetSolverResidualFileName(string materialName)
        {
            var names = Enum.GetNames(typeof(MultiSiteNetSolverType));
            return GetAllEpochResidualFileName(materialName, names);
        }
        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public List<string> GetAllEpochResidualFileName(string materialName, string[] names)
        {
            List<string> result = new List<string>();

            var mainProjDirectory = Path.GetDirectoryName(ProjectOutputDirectory);
            foreach (var item in names)
            {
                var path = Path.Combine(mainProjDirectory, item, BuildEpochResidualFileName(materialName));
                result.Add(path);
            }
            return result;
        }

        /// <summary>
        /// 获取所有可能的残差路径
        /// </summary>
        /// <param name="materialName"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public List<string> GetAllEpochParamFileName(string materialName, string[] names)
        {
            List<string> result = new List<string>();
            var mainProjDirectory = Path.GetDirectoryName(ProjectOutputDirectory);
            foreach (var item in names)
            {
                var path = Path.Combine(mainProjDirectory, item, BuildEpochParamFileName(materialName));
                result.Add(path);
            }
            return result;
        }

        /// <summary>
        /// 获取基线结果文件
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllBaseLineResultFile()
        { 
            List<string> result = new List<string>();
            var names = Enum.GetNames(typeof(TwoSiteSolverType));
            var mainProjDirectory = Path.GetDirectoryName(ProjectOutputDirectory);
            foreach (var item in names)
            {
                var solvDir = Path.Combine(mainProjDirectory, item);
                if (!Directory.Exists(solvDir)) { continue; }
                var files = Directory.GetFiles(solvDir, "*" + Setting.BaseLineFileExtension);
                if (files.Length > 0)
                {
                    var path = files[0];
                    result.Add(path);
                }               
            }
            return result; 
        }

        /// <summary>
        /// 获取基线结果文件
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllNetDoubleDifferBaseLineEpochResidualFile()
        {
            List<string> result = new List<string>();
            var mainProjDirectory = Path.GetDirectoryName(ProjectOutputDirectory);
            var solvDir = Path.Combine(mainProjDirectory, GnssSolverType.网解双差定位.ToString());
            if (!Directory.Exists(solvDir)) { return new List<string>(); }
            var files = Directory.GetFiles(solvDir, "*" + Setting.EpochResidualFileExtension);

            return new List<string>(files);
        }

        /// <summary>
        /// 获取基线结果文件
        /// </summary>
        /// <returns></returns>
        public string GetNetDoubleDifferBaseLineEpochResidualFile(string baseSiteName)
        {
            List<string> result = new List<string>();
            var mainProjDirectory = Path.GetDirectoryName(ProjectOutputDirectory);
            var solvDir = Path.Combine(mainProjDirectory, GnssSolverType.网解双差定位.ToString());
            if (!Directory.Exists(solvDir)) { return null; }
            var files = Directory.GetFiles(solvDir, "*-" + baseSiteName + Setting.EpochResidualFileExtension);

            var results = new List<string>(files);
            if (results.Count > 0) { return results[0]; }
            return null;
        }
        /// <summary>
        /// 获取基线结果文件
        /// </summary>
        /// <returns></returns>
        public string GetNetDoubleDifferBaseLineEpochParamFile(string baseSiteName)
        {
            List<string> result = new List<string>();
            var mainProjDirectory = Path.GetDirectoryName(ProjectOutputDirectory);
            var solvDir = Path.Combine(mainProjDirectory, GnssSolverType.网解双差定位.ToString());
            if (!Directory.Exists(solvDir)) { return null; } 
            var files = Directory.GetFiles(solvDir, "*-" + baseSiteName + Setting.EpochParamFileExtension);

            var results = new List<string>(files);
            if (results.Count > 0) { return results[0]; }
            return null;
        }


        /// <summary>
        /// 获取基线结果文件
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllNetDoubleDifferBaseLineEpochParamFile()
        {
            List<string> result = new List<string>();
            var mainProjDirectory = Path.GetDirectoryName(ProjectOutputDirectory);
            var solvDir = Path.Combine(mainProjDirectory, GnssSolverType.网解双差定位.ToString());
            if (!Directory.Exists(solvDir)) { return new List<string>(); }
            var files = Directory.GetFiles(solvDir, "*" + Setting.EpochParamFileExtension);
       
            return new List<string>( files);
        }
        /// <summary>
        /// 生成基线结果路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string BuildBaseLineResulPath(string name)
        {
            return Path.Combine(SolverDirectory, name + Setting.BaseLineFileExtension);
        }
        /// <summary>
        /// 生成测站结果路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string BuildSiteCoordResulPath(string name)
        {
            return Path.Combine(SolverDirectory, name + Setting.SiteCoordFileExtension);
        }

        /// <summary>
        /// 构造观测残差 小 l 文件名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string BuildEpochObsFileName(string name)
        {
            return name + Setting.EpochObsFileExtension;
        }
        /// <summary>
        /// 构造算后残差 V 文件名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string BuildEpochResidualFileName(string name)
        {
            return name + Setting.EpochResidualFileExtension;
        }
        /// <summary>
        /// 历元参数RMS文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string BuildEpochParamRmsFileName(string name)
        {
            return name + Setting.EpochParamRmsFileExtension; 
        }
        /// <summary>
        /// 历元参数文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string BuildEpochParamFileName(string name)
        {
            return name + Setting.EpochParamFileExtension; 
        }
        /// <summary>
        /// 构建多站历元观测对象名称
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="rovNames"></param>
        /// <returns></returns>
        static public string BuildMultiSiteEpochInfoName(string baseName, List<string> rovNames)
        {
            StringBuilder sb = new StringBuilder();
            int i = -1;
            foreach (var item in rovNames)
            {
                i++;
                if (i > 0) { sb.Append(","); }
                sb.Append(item);
                if (i > 2) { sb.Append("_Of" + rovNames.Count + "Sites"); break; }
            }
            sb.Append("-");
            sb.Append(baseName);
            var name = sb.ToString();
            return name;
        }

    }
}
