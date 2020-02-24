//2016.08.29, czs, create in 西安洪庆, 多站PPP计算器

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;

namespace Gnsser
{

    /// <summary>
    /// 多站PPP计算器
    /// </summary>
    public class MultiSiteIonoFreePppManger : BaseDictionary<string, IonoFreePpp>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSource"></param>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        public MultiSiteIonoFreePppManger(MultiSiteObsStream DataSource, DataSourceContext Context, GnssProcessOption Option)
        {
            this.Context = Context;
            this.Option = Option;
            this.DataSource = DataSource;
            this.TableTextManager = new ObjectTableManager(Option.OutputBufferCount, Option.OutputDirectory);
            this.IsOutputResult = Option.IsOutputEpochResult;
            this.GnssResultWriter = new GnssResultWriter(Option);
        }

        #region 属性
        /// <summary>
        /// 结果写入器。
        /// </summary>
        public GnssResultWriter GnssResultWriter { get; set; }
        /// <summary>
        /// 是否输出结果
        /// </summary>
        public bool IsOutputResult { get; set; }
        /// <summary>
        /// 文本输出
        /// </summary>
        public ObjectTableManager TableTextManager { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public MultiSiteObsStream DataSource { get; set; }
        /// <summary>
        /// GNSS计算选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 数据上下文
        /// </summary>
        public DataSourceContext Context { get; set; }
        /// <summary>
        /// 当前多站。
        /// </summary>
        public MultiSiteEpochInfo CurrentMaterial { get; set; }
        /// <summary>
        /// 当前计算结果，当前有效,即只保留一个历元的计算结果。
        /// </summary>
        public Dictionary<string, PppResult> CurrentResults { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override IonoFreePpp Create(string key)
        {
            return new IonoFreePpp(Context, Option);
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="mEpochInfo"></param>
        public void Solve(MultiSiteEpochInfo mEpochInfo)
        {
            this.CurrentMaterial = mEpochInfo;
            CurrentResults = new Dictionary<string, PppResult>();
            foreach (var epochInfo in mEpochInfo)
            {
                var result =  this.GetOrCreate(epochInfo.Name).Get(epochInfo) as PppResult;
                if (result == null) { continue; }

                CurrentResults[epochInfo.Name] =result;
                //输出结果
                if (IsOutputResult)
                {
                    var table = this.TableTextManager.GetOrCreate(epochInfo.Name);
                    table.NewRow();
                    table.AddItem("Epoch", mEpochInfo.ReceiverTime.ToShortTimeString());
                    table.AddItem((IVector)result.ResultMatrix.Estimated);
                    table.EndRow();
                }
            }            
        }
        #endregion

        /// <summary>
        /// 写历元结果到文件
        /// </summary>
        public void WriteRestults()
        {
            TableTextManager.WriteAllToFileAndCloseStream();
            //写历元汇总
            if (this.CurrentResults != null)
            {
                foreach (var item in this.CurrentResults)
                {
                    this.GnssResultWriter.WriteFinal(item.Value);
                }

            }

        }
    }
}