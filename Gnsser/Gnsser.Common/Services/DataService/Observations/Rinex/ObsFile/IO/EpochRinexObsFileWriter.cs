//2016.09.26, czs, create in hongqing, 将 EpochInformation 写为 RINEX

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
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

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 多文件写。
    /// </summary>
    public class EpochRinexObsFileWriterManager : BaseDictionary<string, EpochRinexObsFileWriter> 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="Version"></param>
        public EpochRinexObsFileWriterManager(string directory, double Version = 3.02)
        {
            this.Directory = directory;
            this.Version = Version;
        }

        /// <summary>
        /// 目录
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public double Version { get; set; }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override EpochRinexObsFileWriter Create(string key)
        {
            var path = Path.Combine(Directory, key);
            return new EpochRinexObsFileWriter(path, Version);
        } 
    }
   
    /// <summary>
    /// 将 EpochInformation 写为 RINEX
    /// </summary>
    public class EpochRinexObsFileWriter : IDisposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Version"></param>
        /// <param name="IsUseRangeCorrections">是否使用伪距改正</param>
        public EpochRinexObsFileWriter(string path, double Version, bool IsUseRangeCorrections = false)
        {
            EpochInfoToRinex = new EpochInfoToRinex(Version, IsUseRangeCorrections);
            this.RinexWriter = new RinexObsFileWriter(path, Version);
            RinexObsFileHeader = null;
            this.IsUseRangeCorrections = IsUseRangeCorrections;
        }
        EpochInfoToRinex EpochInfoToRinex { get; set; }
        RinexObsFileWriter RinexWriter { get; set; }
        RinexObsFileHeader RinexObsFileHeader { get; set; }
        bool IsUseRangeCorrections { get; set; }
        /// <summary>
        /// 写一个。
        /// </summary>
        /// <param name="epoch"></param>
        public void Write(EpochInformation epoch)
        {
            if (epoch == null || epoch.Count ==0) { return; }
            if (RinexObsFileHeader == null)
            {
                List<string> comments = new List<string>();
                if (IsUseRangeCorrections)
                {
                    // comments info
                    var names = epoch.First.ObsCorrectionNames; 

                    if (names.Contains(CorrectionNames.DcbP1C1))
                    {
                        comments.Add("code C1 corrected to P1 by  DcbP1C1");
                    } 
                    if (names.Contains(CorrectionNames.DcbP2C2))
                    {
                        comments.Add("code C2 corrected to P2 by  DcbP2C2");
                    }
                    //smoooth range info how to judge???                     
                    if (names.Contains(CorrectionNames.PhaseSmoothRangeA))
                    {
                        comments.Add("Psuedorange A corrected by carrier smooth range A");
                    }
                    if (names.Contains(CorrectionNames.PhaseSmoothRangeB))
                    {
                        comments.Add("Psuedorange B corrected by carrier smooth range B");
                    }
                    if (names.Contains(CorrectionNames.PhaseSmoothRangeC))
                    {
                        comments.Add("Psuedorange C corrected by carrier smooth range C");
                    }

                    //update
                    epoch.TryUpdateObsWithCorrections();
                }

                RinexObsFileHeader = EpochInfoToRinex.BuildHeader(epoch);
                RinexObsFileHeader.Comments.AddRange(comments);
                RinexWriter.WriteHeader(RinexObsFileHeader);
            }

            //是否更新
            if (IsUseRangeCorrections && RinexObsFileHeader!=null)
            { 
                epoch.TryUpdateObsWithCorrections();
            }

            var obs = EpochInfoToRinex.Build(epoch);
          
            RinexWriter.WriteEpochObservation(obs);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            RinexWriter.Dispose();
        }
    }
}
