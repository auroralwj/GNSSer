//2015.05.10, czs, create in namu, 路径服务
//2015.12.09, czs, create in 达州到成都火车上, 名称修改为 IgsDailyProductNameBuilder
//2015.12.22, czs , edit in hongqing, 名称改为 IgsDailyProductNameBuilder，标注为日文件
//2017.08.18, czs, edit in hongqing, 增加电离层产品生成

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo;

namespace Gnsser.Data
{
    /// <summary>
    /// 计算产品名称，文件路径服务。通过一些条件提供路径服务。
    /// </summary>
    public class IgsProductNameBuilder : AbstractService<FileOption, Time>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extension"></param>
        public IgsProductNameBuilder(string name, string extension, bool IsWeekly = false) : this(BuildFileNameModel(name, GetProductType(extension.TrimStart('*', '.')), IsWeekly))
        { 
        }

        private static IgsProductType GetProductType(string extension)
        {
            IgsProductType type = IgsProductType.Sp3;
            switch (extension.ToLower())
            {
                case "sp3":
                    type = IgsProductType.Sp3;
                    break;
                case "clk":
                    type = IgsProductType.Clk;
                    break;
                case "clk_30s":
                    type = IgsProductType.Clk_30s;
                    break;
                case "clk_05s":
                    type = IgsProductType.Clk_05s;
                    break;
                case "erp":
                    type = IgsProductType.Erp;
                    break;
                case "i":
                case "??i":
                case "**i":
                    type = IgsProductType.I;
                    break;

            }

            return type;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public IgsProductNameBuilder(string name, IgsProductType type, bool IsWeekly = false) : this(BuildFileNameModel(name, type, IsWeekly))
        {

        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathModels">路径模板，安装默认顺序</param>
        public IgsProductNameBuilder(List<string> pathModels) { this.FilePathBuilder = new TimeBasedFilePathBuilder(pathModels); }
         

        /// <summary>
        /// 文件路径生成
        /// </summary>
        TimeBasedFilePathBuilder FilePathBuilder { get; set; }

        /// <summary>
        /// 根据时间获取路径
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public override FileOption Get(Time time)
        {
            var fileOption = FilePathBuilder.Get(time);


            return fileOption;  
        }
        /// <summary>
        /// 获取后缀名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetFileExtension( IgsProductType type, Time time ){
            var  fileNameZipModel= Gnsser.Data.IgsProductFileNameModel.Instance.GetExtensionModel(type);
            TimeBasedFilePathBuilder FilePathBuilder  =new  TimeBasedFilePathBuilder( fileNameZipModel);
            var models = FilePathBuilder.Get(time);

            Dictionary<string, string> dic = new Dictionary<string, string>(); 
            dic.Add(ELMarker.ProductType, type.ToString());
            ELMarkerReplaceService service = new ELMarkerReplaceService(dic);
            var extensoin = service.Get(models.FilePath);

            return extensoin;
        }


        private static List<string> BuildFileNameModel(string name, IgsProductType type, bool IsWeekly)
        {
            var  fileNameZipModel= Gnsser.Data.IgsProductFileNameModel.Instance.Get(type);
            var fileNameModel = fileNameZipModel.TrimEnd('Z','.'); 
            var pathModels = new List<string> { fileNameModel, fileNameZipModel };

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(ELMarker.SourceName, name);
            dic.Add(ELMarker.ProductType, type.ToString());
            ELMarkerReplaceService service = new ELMarkerReplaceService(dic);
            var models = service.Get(pathModels);

            return models;
        }         

         
    }
}