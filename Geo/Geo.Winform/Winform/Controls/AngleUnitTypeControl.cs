using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform;
using Geo.Winform.Controls;
using Geo.Coordinates;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 角度类型选择器
    /// </summary>
    public class AngleUnitTypeControl : EnumSelectControl<AngleUnit>
    {
        public AngleUnitTypeControl() { this.Text = "角度单位"; }

    }
}
