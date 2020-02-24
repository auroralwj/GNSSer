//2018.12.14, czs, create in hmx, ChartSettingOption为工程而生

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser;

namespace Geo
{
    /// <summary>
    /// 绘图样式设置
    /// </summary>
    public partial class SimpleChartSettingForm : Form, IEntityEditor<ChartSettingOption>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public SimpleChartSettingForm(ChartSettingOption option = null)
        {
            InitializeComponent();

            this.Entity = option;
            if (this.Entity != null)
            {
                EntityToUi();
            }
        }

        public event Action<ChartSettingOption> ApplyAction;

        /// <summary>
        /// 设置选项对象
        /// </summary>
        public ChartSettingOption Entity { get; set; }
        private void button_ok_Click(object sender, EventArgs e)
        {
            CheckOrInitOption();

            UiToEntity();
            ApplyAction?.Invoke(this.Entity);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void EntityToUi()
        {
            //style
            //main axies
            this.SetLableFontStyle(this.label_fontOfTitleXY, Entity.FontOfXYTitle);
            this.checkBox_showGridOfX.Checked = Entity.IsShowGridX;
            this.checkBox_isShowYGrid.Checked = Entity.IsShowGridY;
            this.enabledStringControl_xTitle.SetEnabledValue(Entity.TitleOfX);
            this.enabledStringControl_yTitle.SetEnabledValue(Entity.TitleOfY);
            this.enabledFloatSpanControl_xValueSpan.SetEnabledValue(Entity.ValueSpanOfX);
            this.enabledFloatControl_xInterval.SetEnabledValue(Entity.IntervalOfX);
            this.enabledFloatSpanControl_YValuespan.SetEnabledValue(Entity.ValueSpanOfY);
            this.enabledFloatControl_yInterval.SetEnabledValue(Entity.IntervalOfY);
            this.checkBox_XIsStartedFromZero.Checked = Entity.IsStartedFromZeroOfX;
            this.checkBox_YIsStartedFromZero.Checked = Entity.IsStartedFromZeroOfY;
            
            //sub   axies
            enabledStringControl_xAxieFormat.SetEnabledValue(Entity.AxisXLableFormat);
            enabledStringControl_aixesYFormat.SetEnabledValue(Entity.AxisYLableFormat);
        }

        public void UiToEntity()
        {
            //main axis
            Entity.IsShowGridX = checkBox_showGridOfX.Checked;
            Entity.IsShowGridY = checkBox_isShowYGrid.Checked;
            Entity.TitleOfX = this.enabledStringControl_xTitle.GetEnabledValue();
            Entity.TitleOfY = this.enabledStringControl_yTitle.GetEnabledValue();
            Entity.ValueSpanOfX = enabledFloatSpanControl_xValueSpan.GetEnabledValue();
            Entity.IntervalOfX = this.enabledFloatControl_xInterval.GetEnabledValue();
            Entity.ValueSpanOfY = enabledFloatSpanControl_YValuespan.GetEnabledValue();
            Entity.IntervalOfY = enabledFloatControl_yInterval.GetEnabledValue();
            Entity.FontOfXYTitle = GetLableFontStyle(this.label_fontOfTitleXY);
            Entity.IsStartedFromZeroOfX = this.checkBox_XIsStartedFromZero.Checked;
            Entity.IsStartedFromZeroOfY = this.checkBox_YIsStartedFromZero.Checked;
            //sub main axis

            Entity.AxisXLableFormat = enabledStringControl_xAxieFormat.GetEnabledValue();
            Entity.AxisYLableFormat = enabledStringControl_aixesYFormat.GetEnabledValue();
        }

        private void CheckOrInitOption()
        {
            if (Entity == null)
            {
                Entity = new ChartSettingOption();
            }
        }

        private void label_fontOfTitle_Click(object sender, EventArgs e)
        {
            CheckOrInitOption();

            FontSettingForm form = new FontSettingForm(Entity.FontOfXYTitle);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.Entity.FontOfXYTitle = form.Entity;
                SetLableFontStyle(this.label_fontOfTitleXY, form.Entity);
            }
        }

        private void SetLableFontStyle(Label Label, FontSettingOption Option)
        {
            if (Option == null)
            {
                Option = new FontSettingOption();
            }
            Label.ForeColor = Option.Color;
            Label.Font = Option.Font;
        }
        private FontSettingOption GetLableFontStyle(Label Label)
        {
            FontSettingOption Option = new FontSettingOption(Label.Font, Label.ForeColor);
            return Option;
        }

        private void ChartSettingForm_Load(object sender, EventArgs e)
        {
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            UiToEntity();
            ApplyAction?.Invoke(this.Entity);
        }
        
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

    }

}
