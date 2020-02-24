//2015.06.17, czs, create in namu, 表显示器
//2017.02.06, czs, edit in hongqing, 增加一些显示设置
//2017.02.25, czs, edit in hongqing, 增加绘图
//2018.07.12, czs, edit in HMX, 数据绑定control的表格

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using Geo.Utils;

namespace Geo.Winform
{
    /// <summary>
    /// 通用表格数据显示窗口。TableObjectStorage
    /// </summary>
    public partial class TableObjectViewForm : Form
    {        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isShowToolStrip"></param>
        /// <param name="isShowNavigator"></param>
        public TableObjectViewForm(string path, bool isShowToolStrip = true, bool isShowNavigator = true)
        {
            InitializeComponent();

            ObjectTableReader reader = new ObjectTableReader(path, Encoding.Default);
            var table = reader.Read();  
            this.Text = Path.GetFileName(path); 
            this.toolStrip1.Visible = isShowToolStrip;
            Init(table);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="isShowToolStrip"></param>
        /// <param name="isShowNavigator"></param>
        public TableObjectViewForm(ObjectTableStorage dataSource = null, bool isShowToolStrip = true, bool isShowNavigator = true)
        {
            InitializeComponent();
            this.Text = dataSource.Name; 
            this.toolStrip1.Visible = isShowToolStrip;
            Init(dataSource);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="table"></param>
        public void Init(ObjectTableStorage table)
        {
            BindDataSource(table);
            this.paramVectorRenderControl1.SetParamIndexRange(1, table.ColCount - 1);
            this.paramVectorRenderControl1.SetParamNames(table.ParamNames);
        }
        /// <summary>
        /// 当前数据表
        /// </summary>
        ObjectTableStorage TableObjectStorage { get => objectTableControl1.TableObjectStorage; }
        /// <summary>
        /// 当前文件路径
        /// </summary>
        public string FilePath { get; set; }
            /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="table"></param>
        public void BindDataSource(ObjectTableStorage table)
        {
            this.objectTableControl1.DataBind(table);
        }
    
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="info"></param>
        public void ShowInfo(string info)
        {
            this.Invoke(new Action(() =>
            {
                objectTableControl1.ShowInfo(info);
             //   toolStripLabel_info.Text = info;
            }));
        }

        private void toolStripButton_toExcel_Click(object sender, EventArgs e) { ReportUtil.SaveToExcel(this.objectTableControl1.DataGridView); }

        private void toolStripButton_toWord_Click(object sender, EventArgs e) { ReportUtil.SaveToWord(this.objectTableControl1.DataGridView); }

        private void button_draw_Click(object sender, EventArgs e)
        {
            paramVectorRenderControl1.SetTableTextStorage(TableObjectStorage);
            paramVectorRenderControl1.DrawParamLines();
        }

        private void DataTableViewForm_Load(object sender, EventArgs e)
        {
            if (TableObjectStorage != null)
            {
                this.ShowInfo("列数量:" + TableObjectStorage.ColCount);
            }
        }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Geo表格文件(*.txt;*.xls)|*.txt;*.xls|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.FilePath = openFileDialog.FileName;
                LoadTable();
            }
        }

        private void LoadTable()
        {
            ObjectTableReader reader = new ObjectTableReader(FilePath, Encoding.Default);

            var table = reader.Read();//.GetDataTable(); 
            var fileName = Path.GetFileName(FilePath);
            // new DataTableViewForm(table) { Text = fileName }.Show();
            this.Init(table);
        }

        private void toolStripLabel1新窗口打开_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Geo表格文件(*.txt;*.xls)|*.txt;*.xls|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;

                ObjectTableReader reader = new ObjectTableReader(path, Encoding.Default);

                var table = reader.Read();//.GetDataTable(); 
                var fileName = Path.GetFileName(path);
                new TableObjectViewForm(table) { Text = fileName }.Show();
                // this.Init(table);
            }

        }

        private void toolStripLabel_multiDraw_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Geo表格文件(*.txt;*.xls)|*.txt;*.xls|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Parallel.ForEach(openFileDialog.FileNames, path =>
                //     foreach (var path in openFileDialog.FileNames)
                {
                    ObjectTableReader reader = new ObjectTableReader(path, Encoding.Default);
                    var table = reader.Read();//.GetDataTable(); 
                    //   var fileName = Path.GetFileName(path);
                    //   new DataTableViewForm(table) { Text = fileName }.Show();
                    // this.Init(table);
                    paramVectorRenderControl1.DrawTable(table);

                });
            }
        }

        private void toolStripLabel_reload_Click(object sender, EventArgs e)
        {
            if (Geo.Utils.FileUtil.IsValid(FilePath))
            {
                LoadTable();
                MessageBox.Show("重新加载完毕！" + FilePath);
            }
        }
        private void button_selectDraw_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { return; }
            var indexName = TableObjectStorage.GetIndexColName();
            Geo.Utils.DataGridViewUtil.SelectColsAndDraw(objectTableControl1.DataGridView, indexName, TableObjectStorage.Name + "");
        }
        private void toolStripLabel1另存为_Click(object sender, EventArgs e)
        {
            objectTableControl1.SaveAs();
        }
    }
}
