using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{ 
    /// <summary>
    /// SINEX矩阵行。
    /// 一行最多可写三个数字。列的数据不能大于行。
    /// </summary>
    public class MatrixLine : IBlockItem
    {
        public MatrixLine()
        {
            this.MatrixUnits = new List<MatrixUnit>();
        }
        public int Row { get; set; }
        public int Col { get; set; }
        public List<MatrixUnit> MatrixUnits { get; set; }

        public override string ToString()
        {
            string line =  
                " " + StringUtil.FillSpaceLeft(Row, 5)
           +    " " + StringUtil.FillSpaceLeft(Col, 5) ;//写入本行起始行、列号

            foreach (var item in MatrixUnits)//写入值。
            {
                line += " " + item.ToString();
            }
            return line;
        } 

        /// <summary>
        /// 以文本行初始化。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public  IBlockItem Init(string line)
        {
            //throw new NotImplementedException();

          //  MatrixLine this1 = new MatrixLine();
            this.Row = int.Parse(line.Substring(1, 5));
            this.Col = int.Parse(line.Substring(7, 5));

            int i = 0, cur =0;
            while (line.Length > (cur = 13 + i * 22))
            {
                MatrixUnit u = new MatrixUnit();
                string s = line.Substring(cur, cur + 21 > line.Length ? line.Length - cur : 21);
                if (s == "                     " || s == " " || s == "                    "||s.Trim()=="")
                {
                    i++;

                }
                else
                {
                    u.Row = this.Row;
                    u.Col = this.Col + (i++);

                    u.Val = double.Parse(line.Substring(cur, cur + 21 > line.Length ? line.Length - cur : 21));

                    this.MatrixUnits.Add(u);
                }
            }
            return this;

        }
        /// <summary>
        /// 解析矩阵行。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static MatrixLine ParseLine(string line)
        {
            MatrixLine b = new MatrixLine();
            b.Row = int.Parse(line.Substring(1, 5));
            b.Col = int.Parse(line.Substring(7, 5));

            int i = 0, cur;
            while (line.Length > (cur = 13 + (i++) * 22))
            {
                MatrixUnit u = new MatrixUnit();
                u.Row = b.Row;
                u.Col = b.Col + i;
                u.Val = double.Parse(line.Substring(cur, 21));

                b.MatrixUnits.Add(u);
            }
            return b;
        }
    }
}
