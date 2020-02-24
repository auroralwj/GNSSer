//2018.03.20, czs, create in hmx, 选项与文件
//2018.03.20, czs, edit in hmx, 存储核心改为Config，利于文件存储和交互


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo.Times;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Core;
using Geo;
using Geo.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;

namespace Gnsser
{   
    /// <summary>
    /// 选项类型
    /// </summary>
    public enum OptionParamType
    {
        /// <summary>
        /// String
        /// </summary>
        String,
        /// <summary>
        /// Double
        /// </summary>
        Double,
        /// <summary>
        /// Int32
        /// </summary>
        Int,
        /// <summary>
        /// Boolean
        /// </summary>
        Bool,
        /// <summary>
        /// DateTime
        /// </summary>
        DateTime,
        /// <summary>
        /// 定位类型
        /// </summary>
        PositionType,
        ProcessType,
        RinexObsFileFormatType,
        AdjustmentType,
        GnssSolverType,
        XYZ,
        RangeType,
        SatObsDataType,
        SatApproxDataType,
        List_SatelliteType,
        CaculateType,
        Dictionary_CycleSlipDetectorType_Bool,
        WeightedVector,
        List_String,
        SatelliteNumber
    }

   
     
}
