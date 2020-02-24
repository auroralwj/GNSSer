using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{  
    /// <summary>
    /// 矩阵记录单元。格式： E21.14
    /// </summary>
    public class MatrixUnit
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public double Val { get; set; }

        public override string ToString()
        {
            return DoubleUtil.ScientificFomate(Val, "E21.14");
        }
    }
}
