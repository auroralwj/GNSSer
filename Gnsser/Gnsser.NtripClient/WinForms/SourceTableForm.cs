//2016.10.16, czs, create in hongqing, 数据源查看器
//2017.05.02, czs, edit in hongqing,  增加排序功能

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Ntrip.WinForms
{
    public partial class SourceTableForm : Form
    { 
        public SourceTableForm()
        {
            InitializeComponent();
            simpleTableControl1.Operation += simpleTableControl1_Operation;
            simpleTableControl1.ColumnSortEventHandler += simpleTableControl1_ColumnSortEventHandler;
            this.simpleTableControl1.EnableExport = true;

            var tables = Setting.NtripSourceManager.NtripSourceTables;
            this.comboBox1.Items.AddRange( new List<string>( tables.Keys).ToArray());

            enumRadioControl1.Init<SourceType>(); 
        }


        NtripSourceTable NtripSourceTable { get; set; }

        public  List<NtripStream> SelectedNtripStreams { get; set; }

        void simpleTableControl1_Operation(Geo.Winform.OperationType type, object sender)
        {
            var ntripType = enumRadioControl1.GetCurrent<SourceType>();
            if (ntripType != SourceType.Stream) {
                Geo.Utils.FormUtil.ShowWarningMessageBox("只可以选择测站数据流 Stream"); return;
            }
            switch (type)
            {
                case Geo.Winform.OperationType.Select:
                    SelectedNtripStreams = this.simpleTableControl1.GetSelectedObjects<NtripStream>();
                    break;
            }
        }
        private void button_openAndView_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl1.FilePath;


            this.NtripSourceTable = NtripSourceTable.Load(path);

            ViewContent(NtripSourceTable); 
        }

        void simpleTableControl1_ColumnSortEventHandler(string arg1, SortOrder arg2)
        {  
            ViewContent(this.NtripSourceTable, arg1, arg2); 
        }

        private void ViewContent(NtripSourceTable table, string propertyName = "", SortOrder SortOrder = SortOrder.Ascending)
        {
            var type = enumRadioControl1.GetCurrent<SourceType>();

            simpleTableControl1.Title = type.ToString();
            IEnumerable<INtripSourceItem> dataSource = null;
            switch (type)
            {
                case SourceType.Caster:
                    simpleTableControl1.Init<NtripCasterItem>(false);
                    var list = table.GetItems<NtripCasterItem>(type);
                    if (!String.IsNullOrWhiteSpace(propertyName))
                    {
                        list.Sort(new Comparison<NtripCasterItem>(delegate(NtripCasterItem a, NtripCasterItem b)
                        {
                            return Geo.Utils.ObjectUtil.Compare(a, b, propertyName, SortOrder == SortOrder.Ascending);
                        }));
                    }
                    dataSource = list;
                    break;
                case SourceType.Network:
                    simpleTableControl1.Init<NtripNetwork>(false);
                    var list2 = table.GetItems<NtripNetwork>(type);
                    if (!String.IsNullOrWhiteSpace(propertyName))
                    {
                        list2.Sort(new Comparison<NtripNetwork>(delegate(NtripNetwork a, NtripNetwork b)
                        {
                            return Geo.Utils.ObjectUtil.Compare(a, b, propertyName, SortOrder == SortOrder.Ascending);
                        }));
                    }
                    dataSource = list2;
                    break;
                case SourceType.Stream:
                    simpleTableControl1.Init<NtripStream>(false);
                    var list3 = table.GetItems<NtripStream>(type);
                    if (!String.IsNullOrWhiteSpace(propertyName))
                    {
                        list3.Sort(new Comparison<NtripStream>(delegate(NtripStream a, NtripStream b)
                        {
                            return Geo.Utils.ObjectUtil.Compare(a, b, propertyName, SortOrder == SortOrder.Ascending);
                        }));
                    }
                    dataSource = list3;
                    break;
                default: break;
            }

            simpleTableControl1.DataBind(dataSource);
            simpleTableControl1.SetColumnSortModel(DataGridViewColumnSortMode.Programmatic);
        }
       

        private void SourceTableForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void enumRadioControl1_EnumItemSelected(string val, bool isSeleted)
        {
            if (NtripSourceTable != null)
            {
                ViewContent(NtripSourceTable);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var key = comboBox1.SelectedItem.ToString();
            this.NtripSourceTable = Setting.NtripSourceManager.NtripSourceTables[key];
            ViewContent(NtripSourceTable);
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            SelectedNtripStreams = this.simpleTableControl1.GetSelectedObjects<NtripStream>();                 
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
