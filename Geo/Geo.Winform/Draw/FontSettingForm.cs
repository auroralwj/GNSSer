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

namespace Geo
{
    /// <summary>
    /// 字体样式设置
    /// </summary>
    public partial class FontSettingForm : Form, IEntityEditor<FontSettingOption>
    {
        /// <summary>
        /// 字体样式设置
        /// </summary>
        /// <param name="option"></param>
        public FontSettingForm(FontSettingOption option = null)
        {
            InitializeComponent();
            this.enumRadioControl_FontStyle.Init<FontStyle>(true);
            if ( option != null)
            {
                Entity = option;
                EntityToUi();
            }
        }
        /// <summary>
        /// 字体样式设置
        /// </summary>
        public FontSettingOption Entity { get; set; }

        private void button_ok_Click(object sender, EventArgs e)
        {
            CheckOrCreateOption();

            UiToEntity();

            this.DialogResult = DialogResult.OK;
        }

        private void CheckOrCreateOption()
        {
            if (Entity == null)
            {
                Entity = new FontSettingOption();
            }
        }

        public void EntityToUi()
        { 
            colorSelectControl1.SetValue(Entity.Color);
            textBox_fontSize.Text = Entity.Font.Size + ""; ;
            enumRadioControl_FontStyle.SetCurrent(Entity.Font.Style);
            comboBox_fontType.Text = Entity.Font.FontFamily.Name + ""; ; 
        }

        public void UiToEntity()
        { 
            float fontSize = float.Parse(textBox_fontSize.Text);
            string FontFamily = comboBox_fontType.Text.Trim();
            FontStyle fontStyle = enumRadioControl_FontStyle.GetCurrent<FontStyle>();

            Entity.Font = new Font(new FontFamily(FontFamily), fontSize, fontStyle);
            Entity.Color = colorSelectControl1.GetValue();
        }
         

        private void FontSettingForm_Load(object sender, EventArgs e)
        {
        }
    }

    /// <summary>
    /// 字体设置
    /// </summary>
    public class FontSettingOption
    {
        /// <summary>
        /// 默认
        /// </summary>
        public static FontFamily DefaultFontFamily = new FontFamily("微软雅黑");
      //  public static FontFamily DefaultFontFamily = new FontFamily("Times New Roman");

        /// <summary>
        /// 字体设置
        /// </summary>
        public FontSettingOption()
            :this(new Font(DefaultFontFamily, 12, FontStyle.Regular, GraphicsUnit.Pixel),    Color.Black)
        { 
        }
        /// <summary>
        /// 字体设置
        /// </summary>
        /// <param name="Font"></param>
        /// <param name="Color"></param>
        public FontSettingOption(Font Font, Color Color)
        {
            this.Font = Font;
            this.Color = Color;
        }

        /// <summary>
        /// 字体
        /// </summary>
        public Font Font { get; set; } 
        /// <summary>
        /// X轴标题
        /// </summary>
        public Color Color { get; set; } 
    }
}
