//2018.08.06, czs, create in hmx, 文本转换为表对象
//2019.02.16, czs, edit in hongqing, 增加分隔符选项

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Geo.Winform
{
    /// <summary>
    /// 文本转换为表对象
    /// </summary>
    public partial class TextToObjectTableForm : Form
    {
        /// <summary>
        /// 文本转换为表对象
        /// </summary>
        public TextToObjectTableForm()
        {
            InitializeComponent();
        }
        static int index = 0;

        private void button_convert_Click(object sender, EventArgs e)
        {
            try
            {
                var text = richTextBoxControl_text.Text;


                MemoryStream memory = new MemoryStream();
                StreamWriter writer = new StreamWriter(memory, Encoding.Default);
                writer.Write(text);
                writer.Flush();
                memory.Position = 0;

                var splitter = this.richTextBox_splitter.Lines;
                var isRemoveVacant = this.checkBox_removeVacant.Checked;


                var reader = new ObjectTableReader(memory, splitter);
                if (isRemoveVacant) { reader.StringSplitOptions = StringSplitOptions.RemoveEmptyEntries; }

                var table = reader.Read();//.GetDataTable();  
                var name = "文本表格_" + (index++);

                var form = new Geo.Winform.TableObjectViewForm(table) { Text = name };
                form.FilePath = Path.Combine(Setting.TempDirectory, name + Setting.TextTableFileExtension);
                form.Show();
            }catch(Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("转换出错：" + ex.Message);
            }
        }

        private void button_insertIndex_Click(object sender, EventArgs e)
        {
            var lines = richTextBoxControl_text.Lines;
            StringBuilder sb = new StringBuilder();
            int i = 0;//第一行为title
            foreach (var line in lines)
            {
                sb.AppendLine(i + "\t" + line);
                i++;
            }
            richTextBoxControl_text.Text = sb.ToString();
        }

        private void TextToObjectTableForm_Load(object sender, EventArgs e)
        { 
        }
    }
}
