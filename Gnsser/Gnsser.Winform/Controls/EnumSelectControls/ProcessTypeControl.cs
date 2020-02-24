//2016.09.25, czs, create in hongqing, 数据处理流程类型
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
    public class ProcessTypeControl : EnumSelectControl<ProcessType>
    {
        public ProcessTypeControl() { this.Text = "数据处理流程类型"; }

    }
}
