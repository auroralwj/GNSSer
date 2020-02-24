//2018.09.21, czs, create in hmx, 观测文件编辑器


using Gnsser.Times;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Geo.Utils;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Times;
using Geo;
using Geo.IO;
using Gnsser.Domain;
using Geo.Draw;
using Gnsser.Data;
using Geo.Winform;


namespace Gnsser.Winform
{
    public partial class ObsFileEditorForm : Form, IShowLayer
    {
        Log log = new Log(typeof(ObsFileEditorForm));

        public ObsFileEditorForm()
        {
            InitializeComponent();

            fileOpenControl_ofilePath.Filter = Setting.RinexOFileFilter;
            fileOpenControl_ofilePath.FilePath = Setting.GnsserConfig.SampleOFile;
            isInitialized = true;
        }

        bool isInitialized = false;
        #region 属性
        public event ShowLayerHandler ShowLayer;
        /// <summary>
        /// 观测文件
        /// </summary>
        public Data.Rinex.RinexObsFile ObsFile { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string ObsPath { get { return fileOpenControl_ofilePath.FilePath; } set { fileOpenControl_ofilePath.FilePath = value; } }
        /// <summary>
        /// 当前卫星
        /// </summary>
        SatelliteNumber CurrrentPrn { get; set; }
        /// <summary>
        /// 当前表
        /// </summary>
        ObjectTableStorage TableObjectStorage { get; set; }
        #endregion

        private void button_read_Click(object sender, EventArgs e)
        {
            if (!File.Exists(ObsPath))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("文件不存在！");
                return;
            }

            this.ObsFile = ObsFileUtil.ReadFile(ObsPath);

            EntityToUi();
        }
        public bool IsShowL1Only { get => checkBox_show1Only.Checked; }
        public void EntityToUi()
        {
            InitTreeView();

            CheckOrReadObsFile(); 
            SatelliteNumber prn = GetCurrentSelectedPrn();// (SatelliteNumber)   this.bindingSource_sat.Current;
            ShowData(prn);
        }

        public void ShowData(SatelliteNumber prn)
        {
            ObjectTableStorage table = this.ObsFile.BuildObjectTable(prn, IsShowL1Only);
            DataBind(table);
        }


        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="table"></param>
        public void DataBind(ObjectTableStorage table)
        {
            var title = table.Name + "， 共 " + table.ColCount + "列 × " + table.RowCount + "行";
            log.Debug("绑定显示 " + title); 
            this.TableObjectStorage = table;
            var dataTable = table.GetDataTable();
           
            //bindingSource1.DataSource = dataTable;
            this.dataGridView1.DataSource = dataTable;
            tabPage1.Text = CurrrentPrn + "-数据内容";
            ShowInfo( "数量："+ dataTable.Rows.Count + ",  " + table.FirstIndex + "->" + table.LastIndex);
            //toolStripLabel_info.Text = title;
            //bindingSource1.DataSource = this.dataGridView1.DataSource;
            //bindingNavigator1.BindingSource = bindingSource1;
        }

        public void UiToEntity()
        {

        }

        public void ShowInfo(string info)
        {
            this.Invoke(new Action(delegate ()
            {
                toolStripLabel1.Text = info;
            })); 
        }

        void InitTreeView()
        {
            CheckOrReadObsFile();
            this.treeView1.Nodes.Clear();

            var satTypes = ObsFile.Header.SatelliteTypes; 
            var dic = ObsFile.GetSatTypedPrns(); 

            foreach (var kv in dic)
            {
                var satTypeNode = treeView1.Nodes.Add(kv.Key.ToString());
                satTypeNode.Tag = kv.Key;

                foreach (var prn in kv.Value)
                {
                   var satNode = satTypeNode.Nodes.Add(prn.ToString());
                    satNode.ContextMenuStrip = contextMenuStrip_prn;
                    satNode.Tag = prn;
                }
            }
            this.treeView1.ExpandAll();
        }
        
        private void ObsFileEditorForm_Load(object sender, EventArgs e)
        {
        }

        private void button_saveTo_Click(object sender, EventArgs e)
        {
            var toPath = Path.Combine(Setting.TempDirectory, Path.GetFileName(ObsPath));
            using (RinexObsFileWriter writer = new RinexObsFileWriter(toPath))
            {
                writer.Write(ObsFile);
            }
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(toPath));
        }

        private void buttonViewOnChart_Click(object sender, EventArgs e)
        {
            if(ObsFile == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取！");  return; }
            bool isDrawAllPhase = checkBox1ViewAllPhase.Checked; 
            var isSort = checkBox_sortPrn.Checked;
            var form = new ObsFileChartEditForm(this.ObsFile, ObsPath, true, isSort, isDrawAllPhase, true );
            form.Show();
        }
        #region 数据读取
      



        /// <summary>
        /// 检查，如果为null，则读取数据
        /// </summary>
        private bool CheckOrReadObsFile()
        {
            if (ObsFile == null) { this.ObsFile = ObsFileUtil.ReadFile(ObsPath); }
            if (ObsFile == null) return false;
                return true;
        }
        #endregion

         

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            this.CurrrentPrn = GetSelectedPrn(node);
            ShowData(CurrrentPrn);
        }
        private  SatelliteNumber GetCurrentSelectedPrn()
        {
             var node = this.treeView1.SelectedNode;
            if(node == null)   { node = this.treeView1.TopNode; }
            if (node == null) { return SatelliteNumber.Default; }
            this.CurrrentPrn = GetSelectedPrn(node);
            return CurrrentPrn;
        }

        
        private static SatelliteNumber GetSelectedPrn(TreeNode node)
        { 
            if (node.Tag is SatelliteNumber) { return (SatelliteNumber)node.Tag; }
            foreach (TreeNode n in node.Nodes)
            {
                var prn = GetSelectedPrn(n);
                if(prn != SatelliteNumber.Default)
                {
                    return prn;
                }
            }
            return SatelliteNumber.Default;
        }

        private void 对象表中打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            TableObjectViewForm form = new TableObjectViewForm(this.TableObjectStorage);
            form.Show();
        }

        private void button_showMap_Click(object sender, EventArgs e)
        {
            if (!CheckOrReadObsFile()) { Geo.Utils.FormUtil.ShowWarningMessageBox("请确保路径的数据可用！"); }
            var xyz = ObsFile.Header.ApproxXyz;
            
           var layer = AnyInfo.LayerFactory.CreatePointLayer(new AnyInfo.Geometries.Point(ObsFile.Header.SiteInfo.ApproxGeoCoord,"0", ObsFile.Header.MarkerName));
            ShowLayer(layer);
        }

        private void button_satPolar_Click(object sender, EventArgs e)
        {
            if (!CheckOrReadObsFile()) { Geo.Utils.FormUtil.ShowWarningMessageBox("请确保路径的数据可用！"); }
            var xyz = ObsFile.Header.ApproxXyz;
            SatPolarCalculator cal = new SatPolarCalculator(xyz, ObsFile.GetPrns(), ObsFile.Header.TimePeriod, 30);
            var table = cal.BuildTable();
            table.Name = Path.GetFileName(ObsPath) + "_站星位置";
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();

            SatPolarCalculator.ShowChartForm(table);
        }

        private void button_satEle_Click(object sender, EventArgs e)
        {
            if (ObsFile == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取数据！"); return; }
            var table = SatElevatoinTableBuilder.BuildTable(ObsFile);
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show(); 
            //log.Info("采用了星历服务 " + ephService); 
        }

        private void 删除行RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Time> epochs = new List<Time>();
            var rows = dataGridView1.SelectedRows;
            foreach (DataGridViewRow row in rows)
            {
                var epoch = Time.Parse( row.Cells["Epoch"].Value.ToString());
                epochs.Add(epoch);
                dataGridView1.Rows.Remove(row);
            }
            ObsFile.Remove(this.CurrrentPrn, epochs);
        }

        private void 删除此星DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.treeView1.SelectedNode;// (TreeNode)sender;
            if(node == null) { return; }
            var prn = GetSelectedPrn(node);
            ObsFile.Remove(prn);

            this.EntityToUi();
        }

        private void 删除历元不全的卫星AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obsCount = ObsFile.Count;
            var prns = ObsFile.GetPrns();
            foreach (var prn in prns)
            {
                if(ObsFile.GetEpochObservations(prn) .Count != obsCount)
                {
                    ObsFile.Remove(prn);
                    log.Warn("移除 " + prn);
                }
            }
            this.EntityToUi();

        }

        private void fileOpenControl_ofilePath_FilePathSetted(object sender, EventArgs e)
        {
            if(isInitialized)
            if(Geo.Utils.FormUtil.ShowYesNoMessageBox("已选择，是否立即读取？" + fileOpenControl_ofilePath.FilePath) == DialogResult.Yes)
            {
                button_read_Click(null, null);
            }
        }
    }
}
