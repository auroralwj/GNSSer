//2016.11.20, czs, edit in hongqing, 增加 获取数据表 方法

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Data;


namespace Geo.Utils
{
    /// <summary>
    /// 窗口工具类
    /// </summary>
    public static class DataGridViewUtil
    {
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        static public DataTable GetDataTable(DataGridView dataGridView)
        {
            if (dataGridView.DataSource is DataTable)
            {
                return  dataGridView.DataSource as DataTable;
            }

            DataTable tb = new DataTable();
            foreach (DataGridViewColumn col in dataGridView.Columns)
	        {
                if (col.ValueType != null)
                {
                    tb.Columns.Add(col.HeaderText, col.ValueType);
                }
                else { tb.Columns.Add(col.HeaderText); }
	        }   
            int columnCount = dataGridView.Columns.Count;
            foreach (DataGridViewRow row in dataGridView.Rows)
	        {
		         DataRow dataRow = tb.NewRow();
                 tb.Rows.Add(dataRow); 
                for (int i = 0; i < columnCount; i++)
			    {
                    dataRow[i] = row.Cells[i].Value;
			    }               
	        }
             
            return tb;
        }

        /// <summary>
        /// 属性按此排序,是否应该有一个反序？？
        /// </summary>
        static List<string> NAME_ASC_SORT_LIST = new List<string>()
        {
            "Id",
            "编号",
            "名称",
            "工程",
            "档案柜",
            "档案",
            "测站",
            "测量网",
            "坐标系统",
            "标识",
            "分类",
            "类型",
            "材料",
            "原料",
            "数量",
            "单位",
            "价",
           // "时间"
        };
        /// <summary>
        /// 移除选中的行
        /// </summary>
        /// <param name="dataGridView1">数据表</param>
        public static bool RemoveSelectedRows(DataGridView dataGridView1)
        {
            List<DataGridViewRow> list = GetSelectedRows(dataGridView1);
            foreach (DataGridViewRow row in list)
            {
                dataGridView1.Rows.Remove(row);
            }
            return list.Count > 0;
        }
        /// <summary>
        /// 返回行绑定的对象集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dataGridView1">数据表</param>
        /// <returns></returns>
        public static List<T> GetObjects<T>(DataGridView dataGridView1)
        {
            List<T> list = new List<T>();
            if (dataGridView1.Rows == null || dataGridView1.Rows.Count == 0) return list;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.DataBoundItem is T)
                {
                    T ar = (T)row.DataBoundItem;
                    list.Add(ar);
                }
            } 
            return list;
        }
        /// <summary>
        /// 返回选择后的行。若没有选择，则返回当前单元格所在行。
        /// </summary>
        /// <param name="dataGridView1">表对象</param>
        /// <returns></returns>
        public static List<DataGridViewRow> GetSelectedRows(DataGridView dataGridView1)
        {
            List<DataGridViewRow> list = new List<DataGridViewRow>();
            if (dataGridView1.Rows == null || dataGridView1.Rows.Count == 0) return list;

            if (dataGridView1.SelectedRows.Count == 0)//借阅单个
            {
               list.Add(dataGridView1.CurrentRow);
            }
            else//批量借阅。
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    list.Add(row);
                }
            }
            return list;
        }
        /// <summary>
        /// 选中的第一个,若无返回默认对象，如 null。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataGridView1"></param>
        /// <returns></returns>
        public static T GetSelectedObject<T>(DataGridView dataGridView1)
        {
            List<T> list = GetSelectedObjects<T>(dataGridView1);
            if (list == null || list.Count == 0) return default(T);
            return list[0];
        }
        /// <summary>
        /// 选中的第一个
        /// </summary>
        /// <param name="dataGridView1"></param>
        /// <returns></returns>
        public static Object GetSelectedObject(DataGridView dataGridView1)
        {
           var list = GetSelectedObjects(dataGridView1);
            if (list == null || list.Count == 0) return null;
            return list[0];
        }
         
        /// <summary>
        /// 获取与选定行绑定的对象。需要选中行。
        /// </summary>
        /// <typeparam name="T">数据类型,或其子类</typeparam>
        /// <param name="dataGridView1">表对象</param>
        /// <returns></returns>
        public static List<T> GetSelectedObjects<T>(DataGridView dataGridView1)
        {            
            List<T> list = new List<T>();

            if (dataGridView1.Rows == null || dataGridView1.Rows.Count == 0) return list;

            try
            {
                if (dataGridView1.SelectedRows.Count == 0)//借阅单个
                {
                    if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.DataBoundItem is T)
                        list.Add((T)dataGridView1.CurrentRow.DataBoundItem);
                }
                else//批量借阅。
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        if (row.DataBoundItem is T)
                        {
                            T ar = (T)row.DataBoundItem;
                            list.Add(ar);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Geo.Utils.DataGridViewUtil 绑定 DataGridView 出错：" + ex.Message);
            }
            return list;
        }
        /// <summary>
        /// 已选择的范围
        /// </summary>
        /// <param name="DataGridView"></param>
        /// <param name="formRowIndex"></param>
        /// <param name="formColIndex"></param>
        /// <param name="toRowIndex"></param>
        /// <param name="toColIndex"></param>
        public static void GetSelectedIndexRange(DataGridView DataGridView, out int formRowIndex, out int formColIndex, out int toRowIndex, out int toColIndex)
        {
            formRowIndex = int.MaxValue;
            formColIndex = int.MaxValue;
            toRowIndex = int.MinValue;
            toColIndex = int.MinValue;
            foreach (DataGridViewCell cell in DataGridView.SelectedCells)
            {
                if (formRowIndex > cell.RowIndex)
                {
                    formRowIndex = cell.RowIndex;
                }
                if (toRowIndex < cell.RowIndex)
                {
                    toRowIndex = cell.RowIndex;
                }

                if (formColIndex > cell.ColumnIndex)
                {
                    formColIndex = cell.ColumnIndex;
                }

                if (toColIndex < cell.ColumnIndex)
                {
                    toColIndex = cell.ColumnIndex;
                }
            }
        }


        /// <summary>
        /// 表中选中行的对象。一个或多个。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="dataGridView1">表对象</param>
        /// <returns></returns>
        public static List<T> GetSelectedObjs<T>(DataGridView dataGridView1) where T: class
        {
            List<T> list = new List<T>();
            if (dataGridView1.SelectedRows.Count == 0)//借阅单个
            {
                list.Add(dataGridView1.CurrentRow.DataBoundItem as T);
            }
            else//批量
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    list.Add(row.DataBoundItem as T);
                }
            }
            return list;
        }

        /// <summary>
        /// 返回选择的行绑定的对象
        /// </summary>
        /// <param name="dataGridView1">数据表</param>
        /// <returns></returns>
        public static List<Object> GetSelectedObjects(DataGridView dataGridView1)
        {
            List<Object> list = new List<Object>();
            if (dataGridView1.SelectedRows.Count == 0)//借阅单个
            {
                if (dataGridView1.CurrentRow!= null)
                   list.Add(dataGridView1.CurrentRow.DataBoundItem);
            }
            else//批量借阅。
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    list.Add(row.DataBoundItem);
                }
            }
            return list;
        }
        
        /// <summary>
        /// 根据对象的属性建立DataGridView表列。
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        /// <param name="useDisplayName"></param>
        /// <returns></returns>
        public static bool SetDataGridViewCloumnsWithProperties(DataGridView view, Type type, bool useDisplayName = true)
        {
            view.Columns.AddRange(BuildDataGridViewCols(type, useDisplayName));
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            return true;
        }
        public static DataGridViewColumn[] BuildDataGridViewCols(Type classType, bool useDisplayName, List<string> hiddenAttributes , string [] orderedNamedCols)
        {
           var rawCols =  BuildDataGridViewCols(classType, useDisplayName, hiddenAttributes);
            if(orderedNamedCols == null){return rawCols;}
           List<DataGridViewColumn> list = new List<DataGridViewColumn>();
           foreach (var name in orderedNamedCols)
           {
               foreach (var item in rawCols)
               {
                   if (item.HeaderText == name) { list.Add(item); break; }
               } 
           }
           return list.ToArray();
        }

        /// <summary>
        /// 根据对象的属性建立DataGridView表。
        /// </summary>
        /// <param name="hiddenAttributes">需要隐藏的属性</param>
        /// <param name="useDisplayName">是否使用属性的Display属性</param>
        /// <param name="classType">待建表的对象类型</param>
        /// <returns></returns>
        public static DataGridViewColumn[] BuildDataGridViewCols(Type classType,  bool useDisplayName =false, List<string> hiddenAttributes = null)
        {
            if (hiddenAttributes == null) hiddenAttributes = new List<string>();


            PropertyInfo[] infos = classType.GetProperties();
            List<PropertyInfo> oldInfoList = new List<PropertyInfo>(infos);
            //排序
            List<PropertyInfo> infoList = new List<PropertyInfo>();

            foreach (string name in NAME_ASC_SORT_LIST)
            {
                foreach (PropertyInfo p in infos)
                {
                    string disName = ObjectUtil.GetDisplayName(p);
                    if (p.Name == name
                        || (p.Name != null && p.Name.Contains(name))
                        || (disName != null && (disName == name || disName.Contains(name))))
                        if (!infoList.Contains(p)) { infoList.Add(p); }
                }
            }
            //没有排上的添加过来
            foreach (PropertyInfo pro in infos)  {  if (!infoList.Contains(pro)) infoList.Add(pro);  }

            List<DataGridViewColumn> cols = new List<DataGridViewColumn>();
            foreach (PropertyInfo info in infoList)
            {
                if (hiddenAttributes.Contains(info.Name)) continue;

                string name = info.Name;
                string propertyName = info.Name;

                string headerText = info.Name;
                if (useDisplayName)
                {
                    headerText = ObjectUtil.GetDisplayName(info);
                    if (headerText == null) continue;
                }

                DataGridViewColumn col = null;
                if (info.PropertyType == typeof(bool))
                {
                    col = DataGridViewUtil.CreateBoolCol(name, propertyName, headerText);
                }
                else if (info.PropertyType == typeof(byte[]))
                {
                    col = DataGridViewUtil.CreateImageCol(name, propertyName, headerText);
                }
                else
                {
                    col = DataGridViewUtil.CreateTxtCol(name, propertyName, headerText);
                }
                cols.Add(col);
            }
            return cols.ToArray();
        }
        /// <summary>
        /// 由数据类型的属性产生列
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static DataGridViewColumn[] GenDataGridViewCols(Type classType)
        {

            List<DataGridViewColumn> cols = new List<DataGridViewColumn>();

            PropertyInfo[] infos = classType.GetProperties();
            foreach (PropertyInfo info in infos)
            {
                string name = info.Name;
                string propertyName = info.Name;
                string headerText = info.Name;

                DataGridViewColumn col = null;
                if (info.PropertyType == typeof(bool))
                {
                    col = DataGridViewUtil.CreateBoolCol(name, propertyName, headerText);
                }
                else if (info.PropertyType == typeof(byte[]))
                {
                    col = DataGridViewUtil.CreateImageCol(name, propertyName, headerText);
                }
                else
                {
                    col = DataGridViewUtil.CreateTxtCol(name, propertyName, headerText);
                }
                cols.Add(col);
            }
            return cols.ToArray();
        }
        /// <summary>
        /// 创建布尔型列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyName"></param>
        /// <param name="headerText"></param>
        /// <returns></returns>
        public static DataGridViewCheckBoxColumn CreateBoolCol(string name, string propertyName, string headerText)
        {
            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            col.DataPropertyName = propertyName;
            col.HeaderText = headerText;
            col.Name = name;
            col.ReadOnly = true;
            return col;
        }

        /// <summary>
        /// 创建一个文本列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyName"></param>
        /// <param name="headerText"></param>
        /// <returns></returns>
        public static DataGridViewTextBoxColumn CreateTxtCol
            (string name, string propertyName, string headerText)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = propertyName;
            col.HeaderText = headerText;
            col.Name = name;
            col.ReadOnly = true;
            return col;
        }

        /// <summary>
        /// 创建图像列
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="headerText">列文本</param>
        /// <returns></returns>
        public static DataGridViewColumn CreateImageCol(string name, string propertyName, string headerText)
        {
            DataGridViewImageColumn col = new DataGridViewImageColumn();
            col.DataPropertyName = propertyName;
            col.HeaderText = headerText;
            col.Name = name;
            col.ReadOnly = true;
            return col;
        }
        /// <summary>
        /// 将DataGridView中选定的Cells的值按照表格格式保存到string中。
        /// </summary>
        /// <param name="selectedCellSelection">所选集合</param>
        /// <returns></returns>
        public static string GetSelectedCellsValue(DataGridViewSelectedCellCollection selectedCellSelection)
        {
            if (selectedCellSelection.Count == 0) return null;

            //find tht index of the cells.
            int minRowIndex = int.MaxValue;
            int minColumnIndex = int.MaxValue;
            int maxRowIndex = int.MinValue;
            int maxColumnIndex = int.MinValue;

            foreach (DataGridViewCell cell in selectedCellSelection)
            {
                minRowIndex = Math.Min(cell.RowIndex, minRowIndex);
                minColumnIndex = Math.Min(cell.ColumnIndex, minColumnIndex);
                maxRowIndex = Math.Max(cell.RowIndex, maxRowIndex);
                maxColumnIndex = Math.Max(cell.ColumnIndex, maxColumnIndex);
            }

            StringBuilder stringBuilder = new StringBuilder();
            string appendStr;
            for (int rowIndex = minRowIndex; rowIndex <= maxRowIndex; rowIndex++)
            {
                appendStr = "\t";
                for (int colIndex = minColumnIndex; colIndex <= maxColumnIndex; colIndex++)
                {
                    if (colIndex == maxColumnIndex) appendStr = "\r\n";
                    foreach (DataGridViewCell cell in selectedCellSelection)
                    {
                        if (cell.ColumnIndex == colIndex && cell.RowIndex == rowIndex)
                            stringBuilder.Append(cell.Value.ToString() + appendStr);
                    }
                }
            }
            //移除最后的回车换行符
            return stringBuilder.Remove(stringBuilder.Length - 2, 2).ToString();
        }

        /// <summary>
        /// 绘制数据表。
        /// </summary>
        /// <param name="DataGridView"></param>
        /// <param name="indexName"></param>
        /// <param name="chartName"></param>
        public static void SelectColsAndDraw(DataGridView DataGridView, string indexName, string chartName)
        {
            //检查是否选中列
            if (DataGridView.SelectedRows == null || DataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要绘制的行！");
                return;
            }
            //选择需要打印的列。
            List<string> titleList = new List<string>();
            foreach (DataGridViewColumn col in DataGridView.Columns) { if (col.HeaderText == indexName) continue; titleList.Add(col.HeaderText); }
            //弹窗选择
            var list = Geo.Utils.FormUtil.OpenFormSelectTitles(titleList);
            if (list.Count < 1) return;

            list.Insert(0, indexName);
            //着手打印了 
            ObjectTableStorage tb = new ObjectTableStorage();
            int i = 0;
            foreach (DataGridViewRow row in DataGridView.SelectedRows)
            {
                tb.NewRow();
                foreach (var title in list)
                {
                    var val = row.Cells[title].Value;
                    if (title.Equals(indexName)) {
                        DateTime time;
                        double dIndex = 0;
                        if (DateTime.TryParse(val.ToString(), out time))
                        {
                              tb.AddItem(title, time); 
                        }else if(Double.TryParse(val.ToString(), out dIndex)){
                            tb.AddItem(title, dIndex);
                        } else{
                              tb.AddItem(title, i);
                        }
                    }
                    else if (Geo.Utils.ObjectUtil.IsNumerial(val)) {
                        tb.AddItem(title, double.Parse(val.ToString()));
                    }
                    else
                    {
                        double dval = 0;
                        if (double.TryParse(val.ToString(), out dval))
                        {
                            tb.AddItem(title, dval);
                        }
                    }
                }
               i++;
            }
            var chartForm = new Geo.Winform.CommonChartForm(tb);
            chartForm.Text = "" + chartName;
            chartForm.Show();
        }
    }
}
