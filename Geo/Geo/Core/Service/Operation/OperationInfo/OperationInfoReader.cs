//2015.09.27, czs, create in xi'an hongqing, 操作信息数据文件的读取

using System;
using System.IO;
using Geo.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;

namespace Geo
{
    /// <summary>
    ///操作信息数据文件的读取
    /// </summary>
    public class OperationInfoReader : LineFileReader<OperationInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public OperationInfoReader(string gofFilePath, string metaFilePath = null) : base(gofFilePath, metaFilePath)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public OperationInfoReader(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {

        } 

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override OperationInfo Parse(string[] items)
        {
            var initRow = base.Parse(items);
            //只更新路径的绝对位置
            initRow.ParamFilePath = Geo.Utils.PathUtil.GetAbsPath(initRow.ParamFilePath, BaseDirectory); 

            return initRow;
        }


        ///// <summary>
        ///// 字符串列表解析为属性
        ///// </summary>
        ///// <param name="items"></param>
        ///// <returns></returns>
        //public override OperationInfo Parse(string[] items)
        //{
        //    var name =  items[ PropertyIndexes[ GnsserVariableNames.Name ]];
        //    var path = items[ PropertyIndexes[ GnsserVariableNames.ParamFilePath ]];
        //    var dependIndex = PropertyIndexes[GnsserVariableNames.Depends];
        //    //获取和存储相对路径，并不检查文件的存在性
        //    path = Geo.Utils.PathUtil.GetAbsPath(path, BaseDirectory);

        //    var obj = new OperationInfo(name, path);

        //    if (items.Length > dependIndex) {
        //        var prmsString = items[dependIndex]; 
        //        obj.DependsString = prmsString; 
        //    }

        //    return obj;
        //} 
    }
}
