//2014.05.31, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{

    /// <summary>
    /// 图像基准。
    /// </summary>
    public class ImageDatum : Datum
    {
        /// <summary>
        /// 
        /// </summary>
        PiexInCell PiexInCell { get; set; }
    }

    /// <summary>
    /// 图像坐标和图像数据的关系。
    /// </summary>
    public class PiexInCell : IdentifiedObject
    {
        /// <summary>
        /// 图像坐标系统原点在中心。
        /// </summary>
        public PiexInCell CellCenter {
            get
            {
                return new PiexInCell();
            }
        }
    }
}
