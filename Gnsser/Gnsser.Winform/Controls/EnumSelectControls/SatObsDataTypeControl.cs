//2014.12.13, czs, edit in numu, 卫星观测数据类型控件

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Domain;
using Geo.Winform.Controls;

namespace Gnsser.Winform.Controls
{
    /// <summary>
    /// 卫星观测数据类型控件
    /// </summary>
    public partial class SatObsDataTypeControl : EnumSelectControl<SatObsDataType>
    {
        /// <summary>
        /// 卫星观测数据类型控件
        /// </summary>
        public SatObsDataTypeControl()
        {
            this.CurrentdType = SatObsDataType.AlignedIonoFreePhaseRange;
        }
    }
}
