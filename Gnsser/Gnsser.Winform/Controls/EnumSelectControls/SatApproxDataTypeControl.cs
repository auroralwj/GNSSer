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
    public class SatApproxDataTypeControl : EnumSelectControl<SatApproxDataType>
    {
        public SatApproxDataTypeControl() { this.Text = "站星距离近似值"; }

    }
}
