//2017.08.11, czs, edit in hongqing, 单独提出
//2018.08.03, czs, edit in hmx, 修改为平滑伪距单独

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Service;
using Gnsser.Times;
using Geo.Times;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Winform.Wizards; 

namespace Gnsser.Winform
{
    public partial class PsuedoRangeSmoothOptionPage : BaseGnssProcessOptionPage
    {
        public PsuedoRangeSmoothOptionPage()
        {
            InitializeComponent();

            this.Name = "伪距平滑";

            enumRadioControl_smoothSuposType.Init<SmoothRangeSuperpositionType>();
            enumRadioControl_ionDifferType.Init<IonoDifferCorrectionType>();
            enumRadioControl_SmoothRangeType.Init<SmoothRangeType>();
            fileOpenControl_ionoDelta.Filter = Setting.TextTableFileFilter;
        }

        public override void UiToEntity()
        {
            base.UiToEntity();



            #region  伪距平滑
            Option.IsUseGNSSerSmoothRangeMethod = this.checkBox_IsUseGNSSerSmoothRangeMethod.Checked;
            Option.IsSmoothRange = this.checkBox_IsSmoothingRange.Checked;
            //Option.IonoDifferCorrectionType = this.checkBox_IsFitDeltaIonoInSmoothRange.Checked;
            Option.WindowSizeOfPhaseSmoothRange = this.namedIntControl_WindowSizeOfPhaseSmoothRange.GetValue(); 
            Option.IsWeightedPhaseSmoothRange = this.checkBox_IsWeightedPhaseSmoothRange.Checked;
            Option.BufferSize = namedIntControl_bufferSize.GetValue();
            Option.IonoFitEpochCount = this.namedIntControl_IonoFitEpochCount.GetValue();
            Option.OrderOfDeltaIonoPolyFit = this.namedIntControl_OrderOfDeltaIonoPolyFit.GetValue();
            Option.IonoDifferCorrectionType = enumRadioControl_ionDifferType.GetCurrent<IonoDifferCorrectionType>();
            Option.SmoothRangeSuperPosType = enumRadioControl_smoothSuposType.GetCurrent<SmoothRangeSuperpositionType>();
            Option.SmoothRangeType = enumRadioControl_SmoothRangeType.GetCurrent<SmoothRangeType>();
            Option.IonoDeltaFilePath = fileOpenControl_ionoDelta.FilePath;
            #endregion
        }
        public override void EntityToUi()
        {
            base.UiToEntity();

            #region  伪距平滑

            this.checkBox_IsUseGNSSerSmoothRangeMethod.Checked = Option.IsUseGNSSerSmoothRangeMethod;

            this.namedIntControl_IonoFitEpochCount.SetValue(Option.IonoFitEpochCount);
            this.checkBox_IsWeightedPhaseSmoothRange.Checked = Option.IsWeightedPhaseSmoothRange;
            this.checkBox_IsSmoothingRange.Checked = Option.IsSmoothRange;
            this.namedIntControl_WindowSizeOfPhaseSmoothRange.SetValue(Option.WindowSizeOfPhaseSmoothRange);
            //this.checkBox_IsFitDeltaIonoInSmoothRange.Checked = Option.IonoDifferCorrectionType; 
            this.namedIntControl_bufferSize.SetValue(Option.BufferSize);
            this.namedIntControl_OrderOfDeltaIonoPolyFit.SetValue(Option.OrderOfDeltaIonoPolyFit);

            enumRadioControl_ionDifferType.SetCurrent(Option.IonoDifferCorrectionType); 
            enumRadioControl_smoothSuposType.SetCurrent(Option.SmoothRangeSuperPosType);
            enumRadioControl_SmoothRangeType.SetCurrent(Option.SmoothRangeType);
            fileOpenControl_ionoDelta.FilePath = Option.IonoDeltaFilePath;
            #endregion
        }


    }
}