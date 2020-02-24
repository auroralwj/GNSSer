//2015.09.29, czs, create in xi'an hongqing, 数据处理引擎
//2015.10.21, czs, edit in xi'an hongqing, 操作流程文件

using System;
using Geo.IO;
using System.ComponentModel.DataAnnotations;


namespace Geo
{ 
    /// <summary>
    /// 操作流程文件，对应1个.Gof文件
    /// 是一个相对信息，不可脱离工程而活。
    /// </summary>
    public class OperationFlow : BaseList<OperationInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public OperationFlow()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public OperationFlow(String name)
        {
            this.FileName = name;
        }
        /// <summary>
        /// 流程路径
        /// </summary>
        [Display(Name="操作流文件")]
        public String FileName { get; set; }

        /// <summary>
        /// 保存到路径
        /// </summary>
        public void SaveToDirectory(String directory)
        {
            //确保具有 gof 后缀
            if (System.IO.Path.GetExtension(FileName).ToLower() != ".gof") FileName = FileName + ".gof";

            var path = Geo.Utils.PathUtil.GetAbsPath(FileName, directory);

            using (var writer = new OperationInfoWriter(path))
            {
                writer.WriteCommentLine("writed in " + Geo.Utils.DateTimeUtil.GetFormatedDateTimeNow() + " by Gnsser");
                     
                foreach (var item in this)
                {
                    writer.Write(item);
                }
            }
        }
        /// <summary>
        /// 从文件中解析。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static OperationFlow ReadFromFile(string path)
        {
            OperationFlow flow = new OperationFlow();
            flow.FileName = System.IO.Path.GetFileName(path);
            OperationInfoReader reader = new OperationInfoReader(path);
            flow.Add(reader.ReadAll());
            return flow;
        }
       

        #region override

        public override string ToString()
        {
            return FileName;
        }
        public override bool Equals(object obj)
        {
            var o = obj as OperationFlow;
            if (o == null)
            {
                return false;
            }

            return String.Compare(o.FileName, this.FileName, true) == 0;// o.Path.Equals(this.Path);
        }

        public override int GetHashCode()
        {
            if (FileName == null) return 1;

            return FileName.GetHashCode();
        }
        #endregion
    }
}
