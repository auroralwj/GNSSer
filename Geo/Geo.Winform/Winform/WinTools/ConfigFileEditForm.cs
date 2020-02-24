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

namespace Geo.WinTools
{
    /// <summary>
    /// 配置文件编辑器
    /// </summary>
    public partial class ConfigFileEditForm : Form
    {
        /// <summary>
        /// 配置文件编辑器
        /// </summary>
        public ConfigFileEditForm()
        {
            InitializeComponent();
           // this.textBox_path.Text = Setting.GnsserConfig.RtkrcvConfig;
        }
        /// <summary>
        /// 配置文件编辑器
        /// </summary>
        /// <param name="configPath"></param>
        public ConfigFileEditForm(string configPath)
        {
            InitializeComponent();
            this.textBox_path.Text = configPath;
            ReadConfigFile();
        }

        private void RtkrcvConfigForm_Load(object sender, EventArgs e)
        {
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

            //首先清除
            this.dataGridView1.Rows.Clear();

            string path = this.textBox_path.Text;
            ConfigReader reader = new ConfigReader(path);
            var config = reader.Read();

            foreach (var item in config)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Tag = item;

                DataGridViewTextBoxCell textCell = new DataGridViewTextBoxCell();
                textCell.Value = item.Name;
                DataGridViewTextBoxCell valueCell = new DataGridViewTextBoxCell();
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
                if (item.HasSources)
                {
                    var sourceItems = new List<string>( item.GetKeyValueSources().Values);

                    ComboBox comboBox = new ComboBox();
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.Name = item.Name;
                    comboBox.DataSource = sourceItems;
                    comboBox.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
                    comboBox.DrawItem += comboBox1_DrawItem;

                    //将下拉列表加入到DataGridView的控件集合内，否则下拉列表不会显示在你点击的单元格上 
                    comboBox.Visible = false;
                    this.dataGridView1.Controls.Add(comboBox);
                }
            }
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
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ConfigWriter writer = new ConfigWriter(saveFileDialog1.FileName);
                Config config = BuildConfigFromDataGridView();
                writer.Write(config);

                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(saveFileDialog1.FileName));
            }
        }

        private Config BuildConfigFromDataGridView()
        {
            Config config = new Config();
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var name = row.Cells[0].Value + "";
                var val = row.Cells[1].Value + "";
                var comment = row.Cells[2].Value + "";

                if( String.IsNullOrEmpty( name.Trim() )) continue;

                ConfigItem ConfigItem = new ConfigItem(name, val, comment);
                config.Add(ConfigItem.Name, ConfigItem);
            }
            return config;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null) return;

            DataGridViewColumn column = dataGridView1.CurrentCell.OwningColumn;
            DataGridViewRow row = dataGridView1.CurrentRow;
            var item = row.Tag as ConfigItem;
            if (item == null) return;

            ComboBox comboBox1 = null;
            if (this.dataGridView1.Controls.ContainsKey(item.Name))
            {
                comboBox1 = this.dataGridView1.Controls[item.Name] as ComboBox;
            }
            //是否为空，或已经出现
            if (comboBox1 == null) return;


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

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (var item in this.dataGridView1.Controls)
            {
                if (item is ComboBox)
                {
                    ComboBox combo = item as ComboBox;
                    combo.Visible = false;
                }
            }
        }
    }
}