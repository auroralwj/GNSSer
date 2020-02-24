//2016.09.05, czs, create in hongqing, GNSS文件流计算器。
//2017.09.01, czs, edit in hongqing, 添加平差详细表格

using System;
using System.Collections.Generic;
using Gnsser.Service;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Geo;

namespace Gnsser
{
    /// <summary>
    /// GNSS文件流计算器。
    /// </summary>
    public interface IFileGnssSolver
    { 
        /// <summary>
        /// 是否输出结果
        /// </summary>
        bool IsOutputResult { get; }
        /// <summary>
        ///  计算
        /// </summary>
        global::Gnsser.IGnssSolver Solver { get; }
        /// <summary>
        /// 名称
        /// </summary>
        List<string> ParamNames { get; }
        /// <summary>
        /// GNSS结果生成器。
        /// </summary>
        GnssResultBuilder GnssResultBuilder { get; set; }

        /// <summary>
        /// 当前计算结果。
        /// </summary>
        SimpleGnssResult CurrentGnssResult { get;  }

         /// <summary>
         /// 表格输出管理器
         /// </summary>
        ObjectTableManager TableTextManager { get; }

        /// <summary>
        /// 平差详细表格
        /// </summary>
        AioAdjustFileBuilder AioAdjustFileBuilder { get; set; }
        AdjustEquationFileBuilder AdjustEquationFileBuilder { get; set; }
        /// <summary>
        /// 名称同epochinfo的name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 判断是否具有表格数据
        /// </summary>
        bool HasTableData { get; }

        /// <summary>
        /// 写入文件并清空内存
        /// </summary>
        void WriteResultsToFileAndClearBuffer();
        /// <summary>
        /// 写入文件并清空内存
        /// </summary>
        void WriteResultsToFile();
        /// <summary>
        /// 清空内存
        /// </summary>
        void ClearResultBuffer();
        //   /// <summary>
        ///// 输出结果
        ///// </summary>
        ///// <param name="epoch"></param>
        ///// <param name="result"></param>
        //void OutputEpochResult(ISiteSatObsInfo epoch, SimpleGnssResult result);
    }
}
