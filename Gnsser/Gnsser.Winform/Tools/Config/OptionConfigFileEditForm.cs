//2018.03.20, czs, create in hmx, Opt配置文件编辑器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Geo.IO;
using System.Windows.Forms;

namespace Gnsser
{
    /// <summary>
    /// Opt配置文件编辑器
    /// </summary>
    public partial class OptionConfigFileEditForm : Form
    {
        /// <summary>
        /// 配置文件编辑器
        /// </summary>
        public OptionConfigFileEditForm()
        {
            InitializeComponent();
            // this.textBox_path.Text = Setting.GnsserConfig.RtkrcvConfig;
        }
        /// <summary>
        /// 配置文件编辑器
        /// </summary>
        /// <param name="configPath"></param>
        public OptionConfigFileEditForm(string configPath)
        {
            InitializeComponent();
            this.textBox_path.Text = configPath;
            ReadConfigFile();
        }

        private void RtkrcvConfigForm_Load(object sender, EventArgs e)
        {
            enumRadioControl1.Init<GnssSolverType>(false);
            enumRadioControl1.EnumItemSelected += EnumRadioControl1_EnumItemSelected;
        }

        private void EnumRadioControl1_EnumItemSelected(string val, bool isSelected)
        {
            var selected = (GnssSolverType)Enum.Parse(typeof(GnssSolverType), val);
            Config = GnssProcessOptionManager.Instance[selected].Data;
            SetAndShow(Config);
        }

        private void button_setPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_path.Text = this.openFileDialog1.FileName;
            }
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            ReadConfigFile();
        }

        private void ReadConfigFile()
        {
            string path = this.textBox_path.Text;
            OptionConfigReader reader = new OptionConfigReader(path);
            Config = reader.Read();
            Config.Name = Path.GetFileName(path);
            SetAndShow(Config);
        }
        /// <summary>
        /// 核心数据对象
        /// </summary>
        OptionConfig Config { get; set; }

        private void SetAndShow(OptionConfig config)
        {
            //首先清除
            this.dataGridView1.Rows.Clear();

            foreach (var item in config)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Tag = item;

                DataGridViewTextBoxCell textCell = new DataGridViewTextBoxCell();
                textCell.Value = item.Name;

                DataGridViewCell valueCell = null;
                if (OptionManager.GetTypeByKey(item.Name) == OptionParamType.Bool)
                {
                    valueCell = new DataGridViewCheckBoxCell();
                }
                else
                {
                    valueCell = new DataGridViewTextBoxCell();

                }
                valueCell.Value = item.Value;

                DataGridViewTextBoxCell commentCell = new DataGridViewTextBoxCell();
                commentCell.Value = item.Comment;
                DataGridViewTextBoxCell groupCell = new DataGridViewTextBoxCell();
                groupCell.Value = item.Group;

                row.Cells.Add(textCell);

                row.Cells.Add(valueCell);

                row.Cells.Add(commentCell);
                row.Cells.Add(groupCell);

                this.dataGridView1.Rows.Add(row);

                //下拉菜单否
                //(0:llh,1:xyz,2:single,3:posfile,4:rinexhead,5:rtcm)  

                if (item.IsEnumType())
                {
                    var names = Enum.GetNames(OptionManager.GetSysType(item.Name));

                    var sourceItems = new List<string>(names);

                    ComboBox comboBox = new ComboBox();
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.Name = item.Name.ToString();
                    comboBox.DataSource = sourceItems;
                    comboBox.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
                    comboBox.DrawItem += comboBox1_DrawItem;

                    //将下拉列表加入到DataGridView的控件集合内，否则下拉列表不会显示在你点击的单元格上 
                    comboBox.Visible = false;
                    this.dataGridView1.Controls.Add(comboBox);
                }
                //if(OptionManager.GetTypeByKey(item.Name) == OptionParamType.Bool)
                //{
                //    CheckBox checkBox = new CheckBox();
                //    checkBox.Name = item.Name.ToString();
                //    checkBox.Tag = item;
                //    checkBox.Visible = false;
                //    checkBox.CheckedChanged += CheckBox_CheckedChanged;

                //    this.dataGridView1.Controls.Add(checkBox); 
                //}
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;
            var item = box.Tag as OptionConfigItem;
            item.Value = box.Checked;
            if (dataGridView1.CurrentCell != null)
                dataGridView1.CurrentCell.Value = item.Value;
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox1 = sender as ComboBox;
            e.DrawBackground();
            e.Graphics.DrawString(comboBox1.Items[e.Index].ToString(), e.Font, Brushes.Black,
               e.Bounds, StringFormat.GenericDefault);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox1 = sender as ComboBox;
            if (dataGridView1.CurrentCell != null && comboBox1.SelectedIndex != -1)
                dataGridView1.CurrentCell.Value = comboBox1.Items[comboBox1.SelectedIndex];
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            OptionConfig config = BuildConfigFromDataGridView();
            saveFileDialog1.FileName = config.Name;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OptionConfigWriter writer = new OptionConfigWriter(saveFileDialog1.FileName);
                writer.Write(config);

                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(saveFileDialog1.FileName));
            }
        }

        private OptionConfig BuildConfigFromDataGridView()
        {
            OptionConfig config = new OptionConfig();
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var name = row.Cells[0].Value + "";
                var val = OptionManager.ObjToString(row.Cells[1].Value);
                var comment = row.Cells[2].Value + "";
                var group = row.Cells[3].Value + "";

                if (String.IsNullOrEmpty(name.Trim())) continue;

                var key = OptionManager.ParseKey(name);
                var valObj = OptionManager.ParseValue(val, key);

                OptionConfigItem ConfigItem = new OptionConfigItem(key, valObj, group, comment);
                config[ConfigItem.Name] = ConfigItem;
            }
            return config;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null) return;

            DataGridViewColumn column = dataGridView1.CurrentCell.OwningColumn;
            DataGridViewRow row = dataGridView1.CurrentRow;
            var item = row.Tag as OptionConfigItem;
            if (item == null) return;

            ComboBox comboBox1 = null;
            CheckBox checkBox = null;
            if (this.dataGridView1.Controls.ContainsKey(item.Name.ToString()))
            {
                comboBox1 = this.dataGridView1.Controls[item.Name.ToString()] as ComboBox;
                checkBox = this.dataGridView1.Controls[item.Name.ToString()] as CheckBox;
            }

            if (checkBox != null)
            {
                //如果是要显示下拉列表的列的话
                if (column.Name.Equals("Column2"))
                {
                    int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    Rectangle rect = dataGridView1.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    checkBox.Left = rect.Left;
                    checkBox.Top = rect.Top;
                    checkBox.Width = rect.Width;
                    checkBox.Height = rect.Height;

                    //将单元格的内容显示为下拉列表的当前项 
                    checkBox.Checked = (bool)dataGridView1.Rows[rowIndex].Cells[columnIndex].Value;
                    checkBox.Visible = true;
                }
                else
                {
                    checkBox.Visible = false;
                }
            }

            //是否为空，或已经出现
            if (comboBox1 != null)
            {
                //如果是要显示下拉列表的列的话
                if (column.Name.Equals("Column2"))
                {
                    int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    Rectangle rect = dataGridView1.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    comboBox1.Left = rect.Left;
                    comboBox1.Top = rect.Top;
                    comboBox1.Width = rect.Width;
                    comboBox1.Height = rect.Height;

                    //将单元格的内容显示为下拉列表的当前项
                    string consultingRoom = dataGridView1.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                    int index = comboBox1.Items.IndexOf(consultingRoom);

                    comboBox1.SelectedIndex = index;
                    comboBox1.Visible = true;
                }
                else
                {
                    comboBox1.Visible = false;
                }
            }
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (var item in this.dataGridView1.Controls)
            {
                if (item is ComboBox)
                {
                    ComboBox combo = item as ComboBox;
                    combo.Visible = false;
                }
                if (item is CheckBox)
                {
                    CheckBox combo = item as CheckBox;
                    combo.Visible = false;
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (1 == e.ColumnIndex)
            {
                if (e.Value == null)
                {
                    return;
                }

                e.Value = OptionManager.ObjToString(e.Value);
            }

        }
    }
     
}