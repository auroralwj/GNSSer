//2019.02.23, czs, create in hongqing, 修改为  MatrixEquationManager


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.IO;
using Geo.Times;
using Geo.Algorithm.Adjust;

namespace Geo.Algorithm
{
    /// <summary>
    /// 法方程 NX=U
    /// </summary>
    public class MatrixEquationManager : BaseDictionary<string, MatrixEquation>
    {
        Log log = new Log(typeof(MatrixEquationManager));
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatrixEquationManager()
        {

        }

        /// <summary>
        /// 添加编号到不同方程的观测值
        /// </summary>
        /// <param name="startNum">起始编号</param>
        /// <returns>返回结果编号+1</returns>
        public int  AddNumbersToObs(int startNum = 0)
        { 
            foreach (var item in this)
            {
                item.SetObsNameNumber(startNum++);
            }
            return startNum;
        } 


        /// <summary>
        /// 获取法方程
        /// </summary>
        /// <returns></returns>
        public MatrixEquationManager GetNormalEquations()
        {
            MatrixEquationManager result = new MatrixEquationManager();
            foreach (var item in this)
            {
                result.Add(item.Name, item.NormalEquation);
            }
            return result;
        }

        /// <summary>
        /// 读取文本
        /// </summary>
        /// <returns></returns>
        public string ToReadableText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var eq in this)
            {
                sb.AppendLine(eq.ToReadableText());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取对象表
        /// </summary>
        /// <returns></returns>
        public ObjectTableManager GetObjectTables()
        {
            ObjectTableManager ressult = new ObjectTableManager();
            ressult.Name = this.Name;
            foreach (var eq in this)
            {
                ressult.AddTable( eq.GetObjectTables());  
            }
            return ressult;
        }
        /// <summary>
        /// 获取结果表格，当此是法方程才有效
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetResultTable()
        {
            ObjectTableStorage result = new ObjectTableStorage("计算结果");
            foreach (var eq in this)
            {
                if (!eq.IsSolvable) { continue; }

                var q = eq.EstVector;
                var name = eq.Name;
                result.NewRow();

                AddNameValueToTable(result, name);

                result.AddItem(q);
            }
            return result;  
        }


        /// <summary>
        /// 残差表格，当此是法方程才有效
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetResidualTable()
        {
            ObjectTableStorage result = new ObjectTableStorage("计算结果观测残差");
            foreach (var eq in this)
            {
                if (!eq.IsSolvable) { continue; }

                var q = eq.ResidualVector;
                var name = eq.Name;
                result.NewRow();

                AddNameValueToTable(result, name);

                result.AddItem(q);
            }
            return result;
        }
        /// <summary>
        /// 观测值表格
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetObsTable()
        {
            ObjectTableStorage result = new ObjectTableStorage("原始观测值表格");
            foreach (var eq in this)
            {
                var q = eq.RightVector;
                var name = eq.Name;
                result.NewRow();

                AddNameValueToTable(result, name);

                result.AddItem((IVector)q);
            }
            return result;
        }
        private static void AddNameValueToTable(ObjectTableStorage result, string name)
        {
            DateTime epoch = DateTime.Now;

            if (DateTime.TryParse(name, out epoch))
            {
                result.AddItem("Epoch", new Time(epoch));
            }
            else
            {
                result.AddItem("Name", name);
            }
        }
    }
}
