using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Utils;
using Geo.Algorithm.Adjust;

namespace Geo.WinTools
{
    /// <summary>
    /// 残差分析。
    /// </summary>
    public class ResidualAnalysisForm : DataProcessingForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResidualAnalysisForm():base()
        { 
        }
        /// <summary>
        /// 运行
        /// </summary>
        protected override void Run()
        {
            int count = this.InputLines.Length; 

            double[] doubles = Utils.DoubleUtil.ParseLines(this.InputLines);
            ResidualAnalysiser analysiser = new ResidualAnalysiser(doubles);
            this.OutputText = analysiser.ToString();
        }

    }

}
