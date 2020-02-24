//2015.09.29, czs, create in xi'an hongqing, 数据处理引擎

using System;
using System.Collections.Generic;
using Geo.IO;


namespace Geo
{
    /// <summary>
    /// 顶级工作流程，由多个BPE操作文件组成。
    /// </summary>
    public class Workflow : BaseList<OperationFlow>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Workflow()
        {

        }
        /// <summary>
        /// 是否包含.同名也不行，也认为包含。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Contains(string path)
        {
            var upper = path.Trim().ToUpper();
            var name = System.IO.Path.GetFileName(path).ToUpper();
            foreach (var item in this)
            {
                var oldName =System.IO.Path.GetFileName(item.FileName).ToUpper();
                if (item.FileName.Trim().ToUpper() == upper || name == oldName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 路径，相对于工程的路径
        /// </summary>
        public List<string> GofFileNames { 
            get { 
                List<string> pathes = new List<string>();
                foreach (var item in this)
                {
                    pathes.Add(item.FileName);
                }

                return pathes;
            } }


    } 
}
