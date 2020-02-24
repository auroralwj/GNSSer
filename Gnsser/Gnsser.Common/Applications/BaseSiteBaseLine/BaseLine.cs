//2016.11.28, czs, edit in hongqing, 增加注释

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;

namespace Gnsser
{
    /// <summary>
    /// 基线向量。
    /// </summary>
     public class BaseLine
     {
         /// <summary>
         /// 构造函数
         /// </summary>
         public BaseLine() { }
         /// <summary>
         /// 构造函数 
         /// </summary>
         /// <param name="name"></param>
         /// <param name="vector"></param>
         public BaseLine(string name, XYZ vector)
         {
             this.Name = name;
             this.Vector = vector;
         }
         /// <summary>
         /// 构造函数
         /// </summary>
         /// <param name="name"></param>
         /// <param name="from"></param>
         /// <param name="to"></param>
         public BaseLine(string name, XYZ from, XYZ to)
         {
             this.Name = name;
             this.Vector = to - from;
         }
         /// <summary>
         ///  名称
         /// </summary>
         public string Name { get; set; }
         /// <summary>
         /// 向量。
         /// </summary>
         public XYZ Vector { get; set; }
         /// <summary>
         /// 方差
         /// </summary>
         public XYZ Cova { get; set; }

         public override string ToString()
         {
             return Name;
         }
    }
}
