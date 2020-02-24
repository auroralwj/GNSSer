//2016.08.03, czs, create in fujian yongan, 模糊度存储器
//2018.09.19, czs, edit in hmx, 模糊度管理器
//2018.10.19, czs, edit in hmx， 模糊度增加RMS信息

using System;
using Geo;
using Geo.Times;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using System.IO;
using System.Collections.Generic;

namespace Gnsser
{

    /// <summary>
    /// 模糊度存储器
    /// </summary>
    public class AmbiguityManager 
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public AmbiguityManager(GnssProcessOption Option)
        {
            this.AmbiguityFilePath = Option.AmbiguityFilePath;
            this.IsUsingAmbiguityFile = Option.IsUsingAmbiguityFile;
            this.SatObsDataType = Option.ObsDataType;
            Init();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="AmbiguityFilePath"></param>
        /// <param name="IsUsingAmbiguityFile"></param>
        /// <param name="ObsDataType"></param>
        public AmbiguityManager(string AmbiguityFilePath, bool IsUsingAmbiguityFile, SatObsDataType ObsDataType)
        {
            this.AmbiguityFilePath = AmbiguityFilePath;
            this.IsUsingAmbiguityFile = IsUsingAmbiguityFile;
            this.SatObsDataType = ObsDataType;
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (this.IsUsingAmbiguityFile && File.Exists(AmbiguityFilePath)) {
                AmbiguityProduct = PeriodRmsedNumeralStoarge.Read(AmbiguityFilePath);
            }

            this.AmbiguityStorage = new TimedRmsedNumeralStorage();
            //在这里设置模糊度固定器
            this.AmbiguityFixer = new MathRoundAmbiguityFixer();            
        }

        #region 主要属性 
        /// <summary>
        /// 卫星观测数据类型
        /// </summary>
        public SatObsDataType SatObsDataType { get; set; }
        /// <summary>
        /// 模糊度文件路径
        /// </summary>
        public string AmbiguityFilePath { get; set; }
        /// <summary>
        /// 是否使用模糊度文件
        /// </summary>
        public bool IsUsingAmbiguityFile { get; set; }

        /// <summary>
        /// 模糊度固定器
        /// </summary>
        public IAmbiguityFixer AmbiguityFixer { get; set; }

        /// <summary>
        /// 模糊度管理器
        /// </summary>
        public TimedRmsedNumeralStorage AmbiguityStorage { get; set; }

        /// <summary>
        /// 模糊度存储
        /// </summary>
        public PeriodRmsedNumeralStoarge AmbiguityProduct { get; set; }
        #endregion

        

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="result"></param> 
        public void Regist(TwoSitePeriodDifferPositionResult result)
        {
            //单位化为周,若是GLONASS 将重新考虑！！2018.09.26， czs， hmx
            Vector vector = result.FloatAmbiguities / Frequence.GetFrequence(result.Material.EnabledPrns[0], SatObsDataType, result.ReceiverTime).WaveLength;

            var vector2 = AmbiguityFixer.GetFixedAmbiguities(new WeightedVector(vector));
            //var vector3 = vector2 * Frequence.WaveLength;
            var time = result.MaterialObj.Last.ReceiverTime;

            Regist(time, vector2);
        }
        /// <summary>
        /// 注册 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="ambiguities"></param>
        public void Regist(Time time, WeightedVector ambiguities)
        {
            AmbiguityStorage.Regist(time, ambiguities);
        }

        /// <summary>
        /// 保存产品
        /// </summary>
        /// <param name="outputPath"></param>
        public void SaveProduct(string outputPath)
        {
            if(AmbiguityStorage!= null)
            {
                var product = AmbiguityStorage.GetProduct();
                var table = product.ToTable();
                if(table.RowCount == 0 || table.ColCount == 0) { return; }
                var writer = new ObjectTableWriter(outputPath);
                writer.Write(table);
                writer.Close();
            }
        }
    }     
}
