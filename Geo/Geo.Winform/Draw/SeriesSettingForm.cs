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
using System.Windows.Forms.DataVisualization.Charting;

namespace Geo
{
    /// <summary>
    /// 设置序列格式
    /// </summary>
    public partial class SeriesSettingForm : Form, IEntityEditor<SeriesSettingOption>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public SeriesSettingForm(SeriesSettingOption option = null)
        {
            InitializeComponent();
            this.enumRadioControl_XValueType.Init<ChartValueType>(true);
            this.enumRadioControl_YValueType.Init<ChartValueType>(true);
            this.enumRadioControl2SeriesChartType.Init<SeriesChartType>(true);
            this.enumRadioControl_AxisTypeOfY.Init<AxisType>(true);
            this.enumRadioControl_AxisTypeOfX.Init<AxisType>(true);

            if ( option != null)
            {
                Entity = option;
                EntityToUi();
            }
        }

        public SeriesSettingOption Entity { get; set; }

        private void button_ok_Click(object sender, EventArgs e)
        {
            CheckOrCreateOption();

            UiToEntity();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CheckOrCreateOption()
        {
            if (Entity == null)
            {
                Entity = new SeriesSettingOption();
            }
        }

        public void EntityToUi()
        {
            this.enumRadioControl_AxisTypeOfY.SetCurrent<AxisType>(Entity.AxisTypeOfY);
            this.enumRadioControl_AxisTypeOfX.SetCurrent<AxisType>(Entity.AxisTypeOfX);
            this.enumRadioControl_XValueType.SetCurrent(Entity.XValueType); 
            this.enumRadioControl_YValueType.SetCurrent(Entity.YValueType);
            this.enumRadioControl2SeriesChartType.SetCurrent(Entity.SeriesChartType);

             
            colorSelectControl1.SetValue(Entity.Color);
            this.textBoxMarkerSize.Text = Entity.MarkerSize.ToString();
            this.textBox2BorderWidth.Text = Entity.BorderWidth.ToString(); 
        }

        public void UiToEntity()
        {

            Entity.AxisTypeOfY =  this.enumRadioControl_AxisTypeOfY.GetCurrent<AxisType>();
            Entity.AxisTypeOfX = this.enumRadioControl_AxisTypeOfX.GetCurrent<AxisType>();
            Entity.XValueType =  this.enumRadioControl_XValueType.GetCurrent<ChartValueType>();
            Entity.YValueType = this.enumRadioControl_YValueType.GetCurrent<ChartValueType>();
            Entity.SeriesChartType=  this.enumRadioControl2SeriesChartType.GetCurrent<SeriesChartType>();
             
            Entity.Color = colorSelectControl1.GetValue(); ;
            Entity.MarkerSize = int.Parse(this.textBoxMarkerSize.Text);
            Entity.BorderWidth = int.Parse(this.textBox2BorderWidth.Text);

        }
         

        private void button_apply_Click(object sender, EventArgs e)
        {
            UiToEntity();
    //        EntityToUi();
        }
    }

    /// <summary>
    /// 字体设置
    /// </summary>
    public class SeriesSettingOption
    {  
        public SeriesSettingOption(ChartValueType XValueType= ChartValueType.Auto, ChartValueType YValueType = ChartValueType.Auto)
        {
            this.XValueType = XValueType;
            this.YValueType = YValueType;
            this.AxisTypeOfX = AxisType.Primary;
            this.AxisTypeOfY= AxisType.Primary;
            Color = Color.Blue;
        }
         
        /// <summary>
        /// 字体
        /// </summary>
        public ChartValueType XValueType { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public ChartValueType YValueType { get; set; }
        /// <summary>
        /// X轴标题
        /// </summary>
        public SeriesChartType SeriesChartType { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public AxisType AxisTypeOfX { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public AxisType AxisTypeOfY { get; set; }

        public Color Color { get; set; }

        public int MarkerSize { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public int BorderWidth { get; set; }

    }
}
