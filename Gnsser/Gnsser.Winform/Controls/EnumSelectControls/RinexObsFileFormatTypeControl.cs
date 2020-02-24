//2016.09.25, czs, create in hongqing, 观测文件格式化类型
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
    public class RinexObsFileFormatTypeControl : EnumSelectControl<RinexObsFileFormatType>
    {
        public RinexObsFileFormatTypeControl() { this.Text = "观测文件格式化类型"; }

    }
}
