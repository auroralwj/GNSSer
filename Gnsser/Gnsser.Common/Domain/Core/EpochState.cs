//2015.10.28, czs, create , 历元标记增加明显含义 EpochFlag
//2017.10.23, czs, edit in hongqing, 将 8 设为钟跳标记。
//2018.08.15, czs, edit in hmx, 扩展为2位，增加GNSSer钟跳标记
using System;
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

namespace Gnsser.Domain
{ 
    /// <summary>
    /// 历元标记，对应于 RINEX EpochFlag
    /// </summary>
    public enum EpochState
    {
        /// <summary>
        /// OK
        /// </summary>
        Ok = 0,

        /// <summary>
        ///  1: power failure between previous and current epoch
        /// </summary>
        PowerFailure = 1,
        /// <summary>
        /// 2: start moving antenna 
        /// </summary>
        StartMoving = 2,
        /// <summary>
        /// 3: new site occupation (end of kinem. satData) (at least MARKER NAME record follows)  
        /// </summary>  
        NewSiteOccupation = 3,
        /// <summary>
        /// 4: header information follows               
        /// </summary>
        HeaderInformationFollows = 4,
        /// <summary>
        /// 5: external event (epoch is significant, same time frame as observation time tags)
        /// </summary>
        ExternalEvent = 5,
        /// <summary>
        ///表明为修复周跳的记录，根据LLI判读，信号强度指示为 0 或没有。
        /// If epoch flag = 6: 
        ///6: cycle slip records follow to optionally  report detected and repaired cycle slips 
        ///(same format as OBSERVATIONS records; 
        ///slip instead of observation;
        ///LLI and  signal strength blank or zero)    
        /// </summary>
        CycleSlip = 6,
        /// <summary>
        /// GNSSer 标记，此历元具有钟跳，且载波和伪距都发生了钟跳。
        /// </summary>
        ClockJumped = 8,
        #region GNSSer 扩展
        /// <summary>
        /// 只有相位发生了钟跳
        /// </summary>
        ClockJumpedPhaseOnly = 18,
        /// <summary>
        /// 只有伪距发生了钟跳
        /// </summary>
        ClockJumpedRangeOnly = 28,

        /// <summary>
        /// Gnsser 标记，此历元是否可用，同 Unable, disabled
        /// 如，若此历元不可用。
        /// </summary>
        Disabled = 9,

        #endregion
    }


    public enum ClockJumpState
    {

        Ok,
        /// <summary>
        /// GNSSer 标记，此历元具有钟跳，且载波和伪距都发生了钟跳。
        /// </summary>
        ClockJumped = 8, 
        /// <summary>
        /// 只有相位发生了钟跳
        /// </summary>
        ClockJumpedPhaseOnly = 18,
        /// <summary>
        /// 只有伪距发生了钟跳
        /// </summary>
        ClockJumpedRangeOnly = 28,

    }
}