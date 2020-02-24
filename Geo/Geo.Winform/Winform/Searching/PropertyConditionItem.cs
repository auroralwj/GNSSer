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
    /// 代表对象的一个属性。如果是对象查询，则列出对象列表。
    /// </summary>
    public class PropertyConditionItem
    {
        public Type PropertyType { get; set; }
        public List<Restriction> Restrictions { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        public Restriction Restriction { get; set; }
        public Connection Connection { get; set; }
        /// <summary>
        /// 是否是第一个条件。第一个条件不用连接词。
        /// </summary>
        public bool IsFirst { get; set; }
        public object MatchingValue1 { get; set; }
        public object MatchingValue2 { get; set; }
        public bool HasTwoValue { get { return MatchingValue2 != null; } }

        private System.Windows.Forms.BindingSource bindingSource = new BindingSource();

        /// <summary>
        /// 对象列表专用。
        /// </summary>
        public System.Windows.Forms.BindingSource BindingSource
        {
            get
            {
                //if (bindingSource == null)
                //    bindingSource = new BindingSource();
                return bindingSource;
            }
            set { bindingSource = value; }
        }

        #region 重写
        public override string ToString()
        {
            return DisplayName;
        }
        public override int GetHashCode()
        {
            return Name.Length * 13;
        }

        public override bool Equals(object obj)
        {
            PropertyConditionItem o = obj as PropertyConditionItem;
            if (o == null) return false;
            if (o.Name != Name
                || o.PropertyType != PropertyType)
                return false;

            return true;
        }
        #endregion

        private static List<Restriction> GetRestrictions(System.Type Type)
        {
            if (Type.Equals(typeof(string)))
                return stringRestrictions;

            if (Type.Equals(typeof(DateTime)) || Type.Equals(typeof(DateTime?)))
                return dateTimeRestrictions;

            if (Type.Equals(typeof(int)) || Type.Equals(typeof(int?))
                || Type.Equals(typeof(Int16)) || Type.Equals(typeof(Int16?))
                || Type.Equals(typeof(Int64)) || Type.Equals(typeof(Int64?))
                || Type.Equals(typeof(Double)) || Type.Equals(typeof(Double?))
                )
                return numberRestrictions;


            return objectRestrictions;
        }

        public static List<Restriction> objectRestrictions = new List<Restriction>(){
              Restriction.Is //,
              //Restriction.NotEq 
            };
        public static List<Restriction> dateTimeRestrictions = new List<Restriction>(){
              
              // Restriction.Like,
               //Restriction.Eq,   
               Restriction.Gt,   
               Restriction.Between,       
               Restriction.Lt,
             //  Restriction.NotEq
            };
        public static List<Restriction> numberRestrictions = new List<Restriction>(){
              
              // Restriction.Like,
               Restriction.Eq,   
               Restriction.Between,          
               Restriction.Gt,
               Restriction.Lt,
             //  Restriction.NotEq
            };
        public static List<Restriction> stringRestrictions = new List<Restriction>(){ 
               Restriction.Like,
               Restriction.Eq, 
              // Restriction.NotEq
            };
         

        /// <summary>
        /// useDisplayName = true 时，只有设置了DisplayName的才生成。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static PropertyConditionItem Create(System.Reflection.PropertyInfo info, bool useDisplayName)
        {
            string displayName = Geo.Utils.ObjectUtil.GetDisplayName(info);
            if (useDisplayName)
            {
                if (displayName == null ) return null;//|| displayName == info.Name
            }

           

            List<Restriction> restrictions = GetRestrictions(info.PropertyType);
            return new PropertyConditionItem()
            {
                Name = info.Name,
                DisplayName = displayName,
                PropertyType = info.PropertyType,
                Restrictions = restrictions,
                Restriction = restrictions[0]
            };
        }
    }
    public enum Connection
    {
        OR = 0, 
        AND = 1,
        NOT = 2
    }

    /// <summary>
    /// 对属性的限制
    /// </summary>
    public enum Restriction
    {
        Eq = 0, Lt = 1, Gt=2, Like = 3, Between = 4, NotEq = 5,
        Is = 6//列表
    } 
}
