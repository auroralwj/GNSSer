using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform
{
    /// <summary>
    /// 具有条件的搜索界面。
    /// </summary>
    public partial class SearchItem : UserControl
    {
        public SearchItem()
        {
            InitializeComponent();
        }

        public SearchItem( DbSearchItemOption  SearchOption) :
            this(
            SearchOption.ClassType,
            SearchOption.PropertiesDic,
            SearchOption.UseDisplayName,
            SearchOption.InvisibleAttributes 
            ) { }
         
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="classType">待搜索类的类型</param>
        /// <param name="dataSources">数据源</param>
        /// <param name="useDisplayName">是否使用属性显示的名称</param>
        /// <param name="invisibleAttributes">需要隐藏的属性列表</param>
        public SearchItem(
            Type classType,
            Dictionary<String, Object> dataSources=null,
            bool useDisplayName=true,
            List<string> invisibleAttributes = null
            )
        {
            InitializeComponent();

            this.UseDisplayName = useDisplayName;
            this.InvisibleAttributes = invisibleAttributes;

            this.DataSources = dataSources;

            this.ClassType = classType;  
        }  
        /// <summary>
        /// 需要隐藏（不显示）的属性名称.
        /// 务必先初始化本属性，再为ClassType赋值。
        /// </summary>
        public List<string> InvisibleAttributes { get; set; }
        /// <summary>
        /// 数据源绑定。
        /// </summary>
        public Dictionary<String, Object> DataSources { get; set; }
        /// <summary>
        /// 如果是第一个条件，则不必显示。
        /// </summary>
        public bool IsFirst
        {
            get { return !this.comboBox_connect.Visible; }
            set { this.comboBox_connect.Visible = !value; }
        }
        /// <summary>
        /// 是否使用属性的显示名称。
        /// </summary>
        public bool UseDisplayName { get; set; }
        /// <summary>
        /// 类 类型
        /// </summary>
        private Type classType;
        /// <summary>
        /// 主类类型。由此提取属性，需要在最后初始化
        /// </summary>
        public Type ClassType
        {
            get { return classType; }
            set
            {
                classType = value;
                if (value != null)
                {
                    List<Geo.Winform.PropertyConditionItem> list = new List<Winform.PropertyConditionItem>();
                    System.Reflection.PropertyInfo[] ps = value.GetProperties();
            
                    foreach (System.Reflection.PropertyInfo info in ps)
                    {
                        PropertyConditionItem item = Geo.Winform.PropertyConditionItem.Create(info, UseDisplayName);
                       

                        //确定是否显示
                        if (item==null 
                            || InvisibleAttributes != null && !UseDisplayName && InvisibleAttributes.Contains(item.Name)
                            || InvisibleAttributes != null && UseDisplayName && InvisibleAttributes.Contains(item.DisplayName)) 
                            continue;

                        if (item != null)
                        {
                            //设置可选数据源
                            if (info.PropertyType.Equals(typeof(Boolean)))
                            {
                                item.BindingSource.DataSource = new List<bool>() { true, false  };
                            }
                            else if (DataSources != null && DataSources.ContainsKey(item.Name))
                            {
                                item.BindingSource.DataSource = DataSources[item.Name];
                            }

                            list.Add(item);
                        }
                    }
                    this.bindingSource_property.DataSource = list;
                }
            }
        }
        /// <summary>
        /// 属性限制。
        /// </summary>
        public Restriction Restriction
        {
            get
            {
                if (this.bindingSource_condition.Current == null) return Winform.Restriction.Eq;
                return (this.bindingSource_condition.Current as ComboxOperation).Restriction;
            }
            set
            {
                this.comboBox_List.Visible = false;
                this.dateTimePicker1.Visible = false;
                this.dateTimePicker2.Visible = false;
                this.textBox_value1.Visible = false;
                this.textBox_value2.Visible = false;
                if (conditionItem != null)
                {
                    //按需显示
                    switch (value)
                    {
                        case Winform.Restriction.Is:
                            this.comboBox_List.Visible = true;
                            this.comboBox_List.DataSource = this.conditionItem.BindingSource;
                            break;
                        case Winform.Restriction.Between:
                            if (conditionItem.PropertyType == typeof(DateTime)
                                || conditionItem.PropertyType == typeof(DateTime?))
                            {
                                this.dateTimePicker1.Visible = true;
                                this.dateTimePicker2.Visible = true;
                            }
                            else
                            {
                                this.textBox_value1.Visible = true;
                                this.textBox_value2.Visible = true;
                            }
                            break;
                        case Winform.Restriction.Eq:
                        case Winform.Restriction.Gt:
                        case Winform.Restriction.Like:
                        case Winform.Restriction.Lt:
                        case Winform.Restriction.NotEq:
                            if (conditionItem.PropertyType == typeof(DateTime)
                                || conditionItem.PropertyType == typeof(DateTime?))
                            {
                                this.dateTimePicker1.Visible = true;
                            }
                            else
                            {
                                this.textBox_value1.Visible = true;
                            }
                            break;
                        default://超出处理范围了！
                            break;
                    }
                }
            }
        }
        private PropertyConditionItem conditionItem;

        public PropertyConditionItem ConditionItem
        {
            get
            {
                if (conditionItem == null)
                    conditionItem = new PropertyConditionItem();

                //设置连接词
                if (this.comboBox_connect.Text == "或")
                    conditionItem.Connection = Connection.OR;
                else if (this.comboBox_connect.Text == "且")
                    conditionItem.Connection = Connection.AND;
                else if (this.comboBox_connect.Text == "非")
                    conditionItem.Connection = Connection.NOT;

                //获取查询条件
                this.conditionItem.Restriction = this.Restriction;
                //获取输入值
                if (this.comboBox_List.Visible)//列表对象
                {
                    conditionItem.MatchingValue1 = this.comboBox_List.SelectedItem;
                }
                if (this.dateTimePicker1.Visible)
                {
                    conditionItem.MatchingValue1 = this.dateTimePicker1.Value;
                }
                if (this.dateTimePicker2.Visible)
                {
                    conditionItem.MatchingValue2 = this.dateTimePicker2.Value;
                }
                if (this.textBox_value1.Visible)
                {
                    string val = this.textBox_value1.Text.Trim();
                    Type objType = this.conditionItem.PropertyType;

                    object obj = GetObjectValue(val, objType);
                    conditionItem.MatchingValue1 = obj;
                }
                if (this.textBox_value2.Visible)
                {
                    string val = this.textBox_value2.Text.Trim();
                    Type objType = this.conditionItem.PropertyType;

                    object obj = GetObjectValue(val, objType);
                    conditionItem.MatchingValue2 = obj;
                }
                return conditionItem;
            }
            set
            {
                conditionItem = value;
                if (value != null)
                {
                    this.bindingSource_condition.DataSource = ComboxOperation.GetOperateList(value.Restrictions);
                    this.Restriction = value.Restriction;
                }
            }
        }
        /// <summary>
        /// 获取对象值。
        /// </summary>
        /// <param name="val"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static object GetObjectValue(object val, Type objType)
        {
            object obj = val;
            if (objType == typeof(Int32)
                || objType == typeof(long)
                )
            {
                obj = int.Parse(val.ToString());
            }
            else if (objType == typeof(Double)
                )
            {
                obj = Double.Parse(val.ToString());
            }
            return obj;
        }

        private void bindingSource_property_CurrentChanged(object sender, EventArgs e)
        {
            this.ConditionItem = this.bindingSource_property.Current as PropertyConditionItem;
        }

        private void bindingSource_condition_CurrentChanged(object sender, EventArgs e)
        {
            ComboxOperation item = this.bindingSource_condition.Current as ComboxOperation;
            this.Restriction = item.Restriction;
        }
    }
}
