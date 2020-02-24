//2018.08.21, czs, create in hmx, ChartSettingOption为论文而生

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
    public partial class ChartSettingForm : Form, IEntityEditor<ChartSettingOption>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public ChartSettingForm(ChartSettingOption option = null)
        {
            InitializeComponent();

            this.comboBox_chartType.DataSource = Geo.Utils.EnumUtil.GetList<SeriesChartType>();
            enumRadioControl_LegendTableStyle.Init<LegendTableStyle>();
            enumRadioControl_legendDoc.Init<Docking>();
            this.enumRadioControl_legendStyle.Init<LegendStyle>();
            enumRadioControl_titleDoc.Init<Docking>();

            this.comboBox_Series.DataSource = option.SeriesSettingOptions.Keys.ToList();


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
            enabledStringControl_title.SetEnabledValue(Entity.Title);
            this.SetLableFontStyle(this.label_titleFont, Entity.FontOfTitle);

            this.textBox_legentX.Text = Entity.LegnedX.ToString();
            this.textBox_legnedY.Text = Entity.LegnedY.ToString();
            this.checkBox_isEnableLengedPos.Checked = Entity.EnableLegendPostion;
            this.textBox_legWidth.Text = Entity.LegendSize.Width.ToString();
            this.textBox_legHeight.Text = Entity.LegendSize.Height.ToString();
            this.checkBox_enableSize.Checked = Entity.EnableLegnedSize;
            this.enumRadioControl_LegendTableStyle.SetCurrent<LegendTableStyle>(Entity.LegendTableStyle);
            this.enumRadioControl_legendDoc.SetCurrent<Docking>(Entity.LegendDocking);
            this.enumRadioControl_titleDoc.SetCurrent<Docking>(Entity.TitleDocking);

            this.enumRadioControl_legendStyle.SetCurrent<LegendStyle>(Entity.LegendStyle);
            SetLableFontStyle(this.label_LengendFont, Entity.LengendFont);

            //style
            this.textBoxMarkerSize.Text = Entity.MarkerSize.ToString();
            this.textBox2BorderWidth.Text = Entity.BorderWidth.ToString();
            this.checkBox_enableSeriesChartType.Checked = Entity.IsEnableSeriesChartType;
            this.comboBox_chartType.SelectedItem = Entity.SeriesChartType.ToString();

            //main axies
            this.SetLableFontStyle(this.label_fontOfTitleXY, Entity.FontOfXYTitle);
            this.SetLableFontStyle(this.label_lableOfXY, Entity.FontOfAxiesLabel);
            this.checkBox_showGridOfX.Checked = Entity.IsShowGridX;
            this.checkBox_isShowYGrid.Checked = Entity.IsShowGridY;
            this.enabledStringControl_xTitle.SetEnabledValue(Entity.TitleOfX);
            this.enabledStringControl_yTitle.SetEnabledValue(Entity.TitleOfY);
            this.enabledFloatSpanControl_xValueSpan.SetEnabledValue(Entity.ValueSpanOfX);
            this.enabledFloatControl_xInterval.SetEnabledValue(Entity.IntervalOfX);
            this.enabledFloatSpanControl_YValuespan.SetEnabledValue(Entity.ValueSpanOfY);
            this.enabledFloatControl_yInterval.SetEnabledValue(Entity.IntervalOfY);
            this.enabledFloatControl_axisWidth.SetEnabledValue(Entity.AxisWidth);
            this.colorSelectControl_Axis.SetValue(Entity.AxisColor);
            this.checkBox_XIsStartedFromZero.Checked = Entity.IsStartedFromZeroOfX;
            this.checkBox_YIsStartedFromZero.Checked = Entity.IsStartedFromZeroOfY;
            this.checkBox_enbaleSubAxis.Checked = Entity.EnableSubAxis;

            this.enabledFloatControl_LogarithmBaseX.SetEnabledValue(Entity.LogarithmBaseX);
            this.enabledFloatControl_LogarithmBaseY.SetEnabledValue(Entity.LogarithmBaseY);
            enabledIntControl1XLabelAngle.SetEnabledValue(Entity.XLabelAngle);

            //sub   axies
            enabledStringControl_xAxieFormat.SetEnabledValue(Entity.AxisXLableFormat);
            enabledStringControl_aixesYFormat.SetEnabledValue(Entity.AxisYLableFormat);
            enabledStringControl3AxisX2LableFormat.SetEnabledValue(Entity.AxisX2LableFormat);
            enabledStringControl2AxisY2LableFormat.SetEnabledValue(Entity.AxisY2LableFormat);

            this.SetLableFontStyle(this.label_fontOfSubTitle, Entity.FontOfXY2Title);
            this.SetLableFontStyle(this.label_lableOfXY2, Entity.FontOfAxies2Label);
            this.enabledStringControl_subXTitle.SetEnabledValue(Entity.TitleOfX2);
            this.enabledStringControl_subYTitle.SetEnabledValue(Entity.TitleOfY2);
            this.checkBox_showSubGridOfX.Checked = Entity.IsShowGridX2;
            this.checkBox_showSubGridOfY.Checked = Entity.IsShowGridY2;
            this.enabledFloatSpanControl_subXValueSpan.SetEnabledValue(Entity.ValueSpanOfX2);
            this.enabledFloatControl_subXInterval.SetEnabledValue(Entity.IntervalOfX2);
            this.enabledFloatSpanControl_subYValueSpan.SetEnabledValue(Entity.ValueSpanOfY2);
            this.enabledFloatControl_subYInterval.SetEnabledValue(Entity.IntervalOfY2);
            this.enabledFloatControl_subaxisWidth.SetEnabledValue(Entity.Axis2Width);
            this.colorSelectControl_subAxis.SetValue(Entity.Axis2Color);
            this.checkBox_subXIsStartedFromZero.Checked = Entity.IsStartedFromZeroOfX2;
            this.checkBox_subYIsStartedFromZero.Checked = Entity.IsStartedFromZeroOfY2;
            this.enabledFloatControl_LogarithmBaseX2.SetEnabledValue(Entity.LogarithmBaseX2);
            this.enabledFloatControl_LogarithmBaseY2.SetEnabledValue(Entity.LogarithmBaseY2);

        }

        public void UiToEntity()
        {
            Entity.Title = enabledStringControl_title.GetEnabledValue();
            Entity.FontOfTitle = GetLableFontStyle(this.label_titleFont);

            //legend
            Entity.LegnedX = float.Parse(this.textBox_legentX.Text);
            Entity.LegnedY = float.Parse(this.textBox_legnedY.Text);
            Entity.EnableLegendPostion = this.checkBox_isEnableLengedPos.Checked;
            Entity.EnableLegnedSize = checkBox_enableSize.Checked;
            Entity.LengendFont = GetLableFontStyle(this.label_LengendFont);
            var width = float.Parse(this.textBox_legWidth.Text);
            var height = float.Parse(this.textBox_legHeight.Text);
            Entity.LegendSize = new SizeF(width, height);
            Entity.LegendTableStyle = enumRadioControl_LegendTableStyle.GetCurrent<LegendTableStyle>();
            Entity.LegendDocking = enumRadioControl_legendDoc.GetCurrent<Docking>();
            Entity.TitleDocking = enumRadioControl_titleDoc.GetCurrent<Docking>();
            Entity.LegendStyle = this.enumRadioControl_legendStyle.GetCurrent<LegendStyle>();

            //style
            Entity.MarkerSize = int.Parse(this.textBoxMarkerSize.Text);
            Entity.BorderWidth = int.Parse(this.textBox2BorderWidth.Text);
            Entity.IsEnableSeriesChartType = this.checkBox_enableSeriesChartType.Checked;
            Entity.SeriesChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), this.comboBox_chartType.SelectedItem.ToString());

            //main axis
            Entity.IsShowGridX = checkBox_showGridOfX.Checked;
            Entity.IsShowGridY = checkBox_isShowYGrid.Checked;
            Entity.TitleOfX = this.enabledStringControl_xTitle.GetEnabledValue();
            Entity.TitleOfY = this.enabledStringControl_yTitle.GetEnabledValue();
            Entity.ValueSpanOfX = enabledFloatSpanControl_xValueSpan.GetEnabledValue();
            Entity.IntervalOfX = this.enabledFloatControl_xInterval.GetEnabledValue();
            Entity.ValueSpanOfY = enabledFloatSpanControl_YValuespan.GetEnabledValue();
            Entity.IntervalOfY = enabledFloatControl_yInterval.GetEnabledValue();
            Entity.AxisWidth = this.enabledFloatControl_axisWidth.GetEnabledValue();
            Entity.AxisColor = colorSelectControl_Axis.GetValue();
            Entity.FontOfXYTitle = GetLableFontStyle(this.label_fontOfTitleXY);
            Entity.FontOfAxiesLabel = GetLableFontStyle(this.label_lableOfXY);
            Entity.IsStartedFromZeroOfX = this.checkBox_XIsStartedFromZero.Checked;
            Entity.IsStartedFromZeroOfY = this.checkBox_YIsStartedFromZero.Checked;
            Entity.EnableSubAxis = this.checkBox_enbaleSubAxis.Checked;
            Entity.LogarithmBaseX = this.enabledFloatControl_LogarithmBaseX.GetEnabledValue();
            Entity.LogarithmBaseY = this.enabledFloatControl_LogarithmBaseY.GetEnabledValue();

            Entity.XLabelAngle = enabledIntControl1XLabelAngle.GetEnabledValue();
            //sub main axis

            Entity.AxisXLableFormat = enabledStringControl_xAxieFormat.GetEnabledValue();
            Entity.AxisYLableFormat = enabledStringControl_aixesYFormat.GetEnabledValue();
            Entity.AxisX2LableFormat = enabledStringControl3AxisX2LableFormat.GetEnabledValue();
            Entity.AxisY2LableFormat = enabledStringControl2AxisY2LableFormat.GetEnabledValue();

            Entity.IsShowGridX2 = checkBox_showSubGridOfX.Checked;
            Entity.IsShowGridY2 = checkBox_showSubGridOfY.Checked;
            Entity.TitleOfX2 = this.enabledStringControl_subXTitle.GetEnabledValue();
            Entity.TitleOfY2 = this.enabledStringControl_subYTitle.GetEnabledValue();
            Entity.ValueSpanOfX2 = enabledFloatSpanControl_subXValueSpan.GetEnabledValue();
            Entity.IntervalOfX2 = this.enabledFloatControl_subXInterval.GetEnabledValue();
            Entity.ValueSpanOfY2 = enabledFloatSpanControl_subYValueSpan.GetEnabledValue();
            Entity.IntervalOfY2 = enabledFloatControl_subYInterval.GetEnabledValue();
            Entity.Axis2Width = this.enabledFloatControl_subaxisWidth.GetEnabledValue();
            Entity.Axis2Color = colorSelectControl_subAxis.GetValue();
            Entity.FontOfAxies2Label = GetLableFontStyle(label_lableOfXY2);
            Entity.FontOfXY2Title = GetLableFontStyle(this.label_fontOfSubTitle);
            Entity.IsStartedFromZeroOfX2 = this.checkBox_subXIsStartedFromZero.Checked;
            Entity.IsStartedFromZeroOfY2 = this.checkBox_subYIsStartedFromZero.Checked;
            Entity.LogarithmBaseX2 = this.enabledFloatControl_LogarithmBaseX2.GetEnabledValue();
            Entity.LogarithmBaseY2 = this.enabledFloatControl_LogarithmBaseY2.GetEnabledValue();
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

        private void label_LengendFont_Click(object sender, EventArgs e)
        {
            FontSettingForm form = new FontSettingForm(Entity.LengendFont);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.Entity.LengendFont = form.Entity;
                SetLableFontStyle(this.label_LengendFont, form.Entity);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_Series_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button_Series_Click(object sender, EventArgs e)
        {
            var opt = Entity.SeriesSettingOptions[this.comboBox_Series.SelectedItem.ToString()];
            SeriesSettingForm form = new SeriesSettingForm(opt);

            form.Show();

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void label_lableOfXY_Click(object sender, EventArgs e)
        {
            CheckOrInitOption();

            FontSettingForm form = new FontSettingForm(Entity.FontOfAxiesLabel);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.Entity.FontOfAxiesLabel = form.Entity;
                SetLableFontStyle(this.label_lableOfXY, form.Entity);
            }
        }

        private void label_lableOfXY2_Click(object sender, EventArgs e)
        {
            CheckOrInitOption();

            FontSettingForm form = new FontSettingForm(Entity.FontOfAxies2Label);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.Entity.FontOfAxies2Label = form.Entity;
                SetLableFontStyle(this.label_lableOfXY2, form.Entity);
            }
        }

        private void label_fontOfSubTitle_Click(object sender, EventArgs e)
        {
            CheckOrInitOption();

            FontSettingForm form = new FontSettingForm(Entity.FontOfXY2Title);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.Entity.FontOfXY2Title = form.Entity;
                SetLableFontStyle(this.label_fontOfSubTitle, form.Entity);
            }
        }

        private void label_titleFont_Click(object sender, EventArgs e)
        {
            CheckOrInitOption();

            FontSettingForm form = new FontSettingForm(Entity.FontOfTitle);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.Entity.FontOfTitle = form.Entity;
                SetLableFontStyle(this.label_titleFont, form.Entity);
            }
        }
    }

}
