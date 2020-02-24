
//2017.07.25, czs, create in honqging, 通用对象编辑器


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.IO;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates; 
using Geo;
using Geo.Times; 
using Geo.Winform.Controls;

namespace Geo.Winform
{
    /// <summary>
    /// 通用对象编辑器
    /// </summary>
    public partial class ObjectEditForm<T> : Form where T:IOrderedProperty
    {
        /// <summary>
        /// 通用对象编辑器
        /// </summary>
        public ObjectEditForm()
        {
            InitializeComponent();
            Init();
        }
        /// <summary>
        /// 对象类型
        /// </summary>
        public Type ObjType { get { return typeof(T); } }
         
        /// <summary>
        /// 初始化，采用
        /// </summary> 
        public void Init()
        {
            int width = this.flowLayoutPanel1.Width;
            int halfWidth = width / 2-10; 
            var obj = ObjType.Assembly.CreateInstance(ObjType.FullName,true) ;
            T o = (T)obj;

            this.Height = o.OrderedProperties.Count * 15 + 150;

            foreach (var item in o.OrderedProperties)
            {
                var p = ObjType.GetProperty(item);
                var PropertyType = p.PropertyType;

                if (PropertyType == typeof(Int32))
                {
                    NamedIntControl control = new NamedIntControl();
                    control.Width = halfWidth;
                    control.Title = p.Name+":";
                    control.Name = p.Name;

                    AddControl(control);
                }
                if (PropertyType == typeof(Boolean))
                {
                    CheckBox control = new CheckBox();
                    control.Width = halfWidth;
                    control.Text = p.Name;
                    control.Name = p.Name;
                    AddControl(control);
                }
                if (PropertyType == typeof(Double))
                {
                    NamedFloatControl control = new NamedFloatControl();
                    control.Width = halfWidth;
                    control.Title = p.Name + ":";
                    control.Name = p.Name;
                    AddControl(control);
                }
                if (PropertyType == typeof(String))
                {
                    NamedStringControl control = new NamedStringControl();
                    control.Width = halfWidth;
                    control.Title = p.Name + ":";
                    control.Name = p.Name;
                    AddControl(control);
                }
                if (PropertyType == typeof(Time) || p.PropertyType == typeof(DateTime))
                {
                    NamedTimeControl control = new NamedTimeControl();
                    control.Width = halfWidth;
                    control.Title = p.Name + ":";
                    control.Name = p.Name;
                    AddControl(control);
                }
                if (PropertyType == typeof(TimePeriod) )
                {
                    TimePeriodControl control = new TimePeriodControl();
                    control.Width = width;
                    control.Title = p.Name + ":";
                    control.Name = p.Name;
                    AddControl(control);
                }
            }
        }

        private void AddControl(Control Control)    {  this.flowLayoutPanel1.Controls.Add(Control);   }
        /// <summary>
        /// 通过名称获取控件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Control GetControl(string name) {  return this.flowLayoutPanel1.Controls.Find(name, false)[0]; }
        /// <summary>
        /// 从界面输入更新到对象
        /// </summary>
        /// <returns></returns>
        public T UiToObj() 
        { 
            var obj = ObjType.Assembly.CreateInstance(ObjType.FullName, true);
            T o = (T)obj;
            UiToObj(o);
            return o;
        }
        /// <summary>
        /// 从界面输入更新到对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public T UiToObj(T obj)
        { 
            foreach (var item in obj.OrderedProperties)
            {
                var control = GetControl(item);
                var p = ObjType.GetProperty(item);
                var PropertyType = p.PropertyType;

                if (PropertyType == typeof(Int32))
                {
                    var valControl = control as  NamedIntControl;
                    if (valControl == null) continue;

                    var val = valControl.GetValue();
                    p.SetValue(obj, val, null); 
                }
                if (PropertyType == typeof(Boolean))
                { 
                    var valControl = control as CheckBox;
                    if (valControl == null) continue;

                    var val = valControl.Checked;
                    p.SetValue(obj, val, null); 
                }
                if (PropertyType == typeof(Double))
                { 
                    var valControl = control as NamedFloatControl;
                    if (valControl == null) continue;

                    var val = valControl.GetValue();
                    p.SetValue(obj, val, null); 
                }
                if (PropertyType == typeof(String))
                { 
                    var valControl = control as NamedStringControl;
                    if (valControl == null) continue;

                    var val = valControl.GetValue();
                    p.SetValue(obj, val, null); 
                }
                if (PropertyType == typeof(Time) || p.PropertyType == typeof(DateTime))
                { 
                    var valControl = control as NamedTimeControl;
                    if (valControl == null) continue;

                    var val = valControl.GetValue();
                    if (PropertyType == typeof(Time))
                    {
                         var val2 =  new Time( (DateTime)val);
                         p.SetValue(obj, val2, null); 
                    }else
                    p.SetValue(obj, val, null); 
                }
                if (PropertyType == typeof(TimePeriod))
                { 
                    var valControl = control as TimePeriodControl;
                    if (valControl == null) continue;

                    var val = valControl.TimePeriod;
                    p.SetValue(obj, val, null); 
                }

            }
            return obj;
        }
        /// <summary>
         /// 更新界面
        /// </summary>
        /// <param name="obj"></param>
         public void ObjToUi(T obj)
        { 
            if (obj == null)
            {
                obj = (T)ObjType.Assembly.CreateInstance(ObjType.Name, true); 
            }

            foreach (var item in obj.OrderedProperties)
            { 
                var control = GetControl(item);
                var p = ObjType.GetProperty(item);
                var PropertyType = p.PropertyType;

                if (PropertyType == typeof(Int32))
                {
                    var valControl = control as NamedIntControl;
                    if (valControl == null) continue;
                    var val = (Int32)p.GetValue(obj, null);
                    valControl.SetValue(val); 
                }
                if (PropertyType == typeof(Boolean))
                {
                    var valControl = control as CheckBox;
                    if (valControl == null) continue;

                    var val = (Boolean)p.GetValue(obj, null);
                    valControl.Checked = (val); 
                }
                if (PropertyType == typeof(Double))
                {
                    var valControl = control as NamedFloatControl;
                    if (valControl == null) continue;

                    var val = (Double)p.GetValue(obj, null);
                    valControl.SetValue(val); 
                }
                if (PropertyType == typeof(String))
                {
                    var valControl = control as NamedStringControl;
                    if (valControl == null) continue;

                    var val = (String)p.GetValue(obj, null);
                    valControl.SetValue(val); 
                }
                if (PropertyType == typeof(Time) || p.PropertyType == typeof(DateTime))
                {
                    var valControl = control as NamedTimeControl;
                    if (valControl == null) continue;

                    if (PropertyType == typeof(Time))
                    {
                        var val = (Time)p.GetValue(obj, null);
                        valControl.SetValue(val.DateTime);
                    }
                    else
                    { 
                        var val = (DateTime)p.GetValue(obj, null);
                        valControl.SetValue(val); 
                    }
                }
                if (PropertyType == typeof(TimePeriod))
                {
                    var valControl = control as TimePeriodControl;
                    if (valControl == null) continue;


                    var val = (TimePeriod)p.GetValue(obj, null);
                    valControl.TimeFrom = (val.Start.DateTime);
                    valControl.TimeFrom = (val.EndDateTime); 
                }
            }
        }

         private void button_ok_Click(object sender, EventArgs e)
         {
             this.DialogResult = System.Windows.Forms.DialogResult.OK; 
         }

         private void ObjectEditForm_Load(object sender, EventArgs e)
         {

         }
    }
}
