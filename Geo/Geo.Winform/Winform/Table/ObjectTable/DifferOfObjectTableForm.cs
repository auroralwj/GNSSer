//2018.08.13, czs, create in hmx, 两表作差


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
    public partial class DifferOfObjectTableForm : Form
    {
        /// <summary>
        /// 文本转换为表对象
        /// </summary>
        public DifferOfObjectTableForm()
        {
            InitializeComponent();
            fileOpenControlA.Filter = Setting.TextTableFileFilter;
            fileOpenControlB.Filter = Setting.TextTableFileFilter;
        }
        static int index = 0;

        private void button_convert_Click(object sender, EventArgs e)
        {
            try
            {
                var pathA = fileOpenControlA.FilePath;
                var pathB = fileOpenControlB.FilePath;

                var tableA = new ObjectTableReader(pathA, Encoding.Default).Read();
                var tableB = new ObjectTableReader(pathB, Encoding.Default).Read();

                ObjectTableStorage table = new ObjectTableStorage(tableA.Name + "-" + tableB.Name);

                var indexColNameA = tableA.GetIndexColName();
                var indexColNameB = tableB.GetIndexColName();

                var indexesInB = tableB.GetIndexValues();

                progressBarComponent1.InitProcess(tableA.RowCount);

                foreach (var rowA in tableA.BufferedValues)
                {
                    progressBarComponent1.PerformProcessStep();

                    var indexObj = rowA[indexColNameA];
                    var inB = indexesInB.IndexOf(indexObj);
                    if (inB == -1) { continue; }

                    var rowB = tableB.BufferedValues[inB];
                    table.NewRow();
                    table.AddItem(indexColNameA, indexObj);
                    foreach (var item in rowA)
                    {
                        if (!rowB.ContainsKey(item.Key) || item.Key == indexColNameA)
                        {
                            continue;
                        }

                        var valObjB = rowB[item.Key];

                        if (!Geo.Utils.ObjectUtil.IsNumerial(item.Value) || !Geo.Utils.ObjectUtil.IsNumerial(valObjB)) { continue; }

                        var valA = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                        var valB = Geo.Utils.ObjectUtil.GetNumeral(valObjB);

                        var differ = valA - valB;
                        table.AddItem(item.Key, differ);
                    }
                }
                progressBarComponent1.Full();

                this.objectTableControl1.DataBind(table);
                Geo.Utils.FormUtil.ShowOkMessageBox();

            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("转换出错：" + ex.Message);
            }
        }

        private void button_openANew_Click(object sender, EventArgs e)
        {
            try
            {
                var path = fileOpenControlA.FilePath;
                if (File.Exists(path))
                    new TableObjectViewForm(path).Show();
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("转换出错：" + ex.Message);
            }
        }

        private void button_openBNew_Click(object sender, EventArgs e)
        {
            try
            {
                var path = fileOpenControlB.FilePath;
                if (File.Exists(path))
                    new TableObjectViewForm(path).Show();
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("转换出错：" + ex.Message);
            }

        }
    }
}
