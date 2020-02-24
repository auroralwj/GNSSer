//2016.10.04, czs, create in hongqing, 平差向量类型

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using Geo.Winform.Controls;
using Geo.Algorithm.Adjust;


namespace Geo.Winform.Controls
{
    /// <summary>
    /// 平差向量类型选择器
    /// </summary>
    public class AdjustVectorTypeControl : EnumSelectControl<AdjustParamVectorType>
    {
        /// <summary>
        /// 平差向量类型选择器
        /// </summary>
        public AdjustVectorTypeControl() { this.Text = "平差向量类型"; }

    }
}
