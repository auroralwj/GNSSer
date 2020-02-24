//2014.12.12, czs, create, 定位结果绘图，查看残差，均方根等,提取为控件
//2016.10.04, czs, create in hongqing, 取消与GnssResult的绑定，增加平差向量选项

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting; 
using Gnsser; 
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo;
using Geo.Algorithm;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 定位结果绘图，查看残差，均方根等。
    /// </summary>
    public partial class AdjustVectorRenderControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdjustVectorRenderControl()
        {
            InitializeComponent();
        }

        #region 属性
        /// <summary>
        /// 待绘参数名称
        /// </summary>
        protected List<string> ParamNames { get; set; }
        /// <summary>
        /// 首列是否是编号，默认为false，如果true，则忽略编号列。
        /// </summary>
        public bool IsHasIndexParamName { get; set; }
        /// <summary>
        /// 计算结果
        /// </summary>
        protected List<AdjustResultMatrix> Adjustments { get; set; } 

        /// <summary>
        /// 绘图起始历元编号
        /// </summary>
        public int StartIndex { get { return Int32.Parse(textBox_startEpoch.Text); } }
        /// <summary>
        /// 绘图截止历元编号
        /// </summary>
        public int EndIndex { get { return Int32.Parse(textBox_endEpoch.Text); } }
        /// <summary>
        /// 绘图参数个数，默认为3个。
        /// </summary>
        public int ParamCount { get { return int.Parse(textBox_paramCount.Text); } }
        /// <summary>
        /// 起始参数编号，从0开始
        /// </summary>
        public int StartParamIndex { get { return int.Parse(textBox_startParamIndex.Text); } }
        /// <summary>
        /// 结束参数编号
        /// </summary>
        public int EndParamIndex { get { return StartParamIndex + ParamCount; } }
        #endregion

        #region 绘制
        /// <summary>
        /// 设置平差列表
        /// </summary>
        /// <param name="Adjustments"></param>
        public void SetResult(List<AdjustResultMatrix> Adjustments)
        {
            this.Adjustments = Adjustments;
        }
        /// <summary>
        /// 设置参数名称列表
        /// </summary>
        /// <param name="names"></param>
        public void SetParamNames(List<string> names)
        {
            var startParamIndex = StartParamIndex;
            if (IsHasIndexParamName)
            {
                startParamIndex = StartParamIndex + 1;
            }
            names = names.GetRange(startParamIndex, names.Count - startParamIndex);
            this.ParamNames = names;
        }
         
         
        /// <summary>
        /// 绘图中心
        /// </summary>
        public Vector CenterVector { 
            get { 
                Vector vector = new Vector(ParamCount + StartParamIndex);
                Vector inputVector = Vector.Parse(this.textBox_differ.Text);
                vector.SetSubVector(inputVector);
                return vector;
            }
        }
        /// <summary>
        /// 平差类型
        /// </summary>
        public AdjustParamVectorType AdjustVectorType { get { return this.adjustVectorTypeControl1.CurrentdType; } }
        #endregion
         
        /// <summary>
        /// 绘制估计值
        /// </summary>
        public void DrawParamLines()
        {
            if (Adjustments == null || Adjustments.Count == 0) return; 

            Vector vector = CenterVector;

            int start = StartIndex;
            int end = EndIndex;

            int resultIndex = 0;


            //X
            var names = Adjustments[0].ParamNames;
            if (ParamNames != null)
            {
                names = ParamNames;
            } 

            Dictionary<string, Series> dic = new Dictionary<string, Series>();
            int i = 0;
            foreach (var name in names)
            {
                i++;
                if (i > ParamCount) break;

                Series seriesX = new Series(name);
                seriesX.ChartType = SeriesChartType.Point;
                dic[name] = seriesX;
            }
             
            foreach (var result in Adjustments.ToArray())//避免集合修改无法遍历
            {
                resultIndex++;

                //起始
                if (resultIndex > start)
                {
                    var est = result.Get(AdjustVectorType );//.Estimated;
                    int paramIndex = StartParamIndex;
                    foreach (var kv in dic)
                    {
                        if (paramIndex >= EndParamIndex) break;

                        var name = kv.Key;
                        if (result.ParamNames.Contains(name))
                        { 
                            kv.Value.Points.Add(new DataPoint(resultIndex, est[name] - vector[paramIndex]));
                        }
                        paramIndex++;
                    } 
                }

                //截止
                if (resultIndex > end) break;
            }
            List<Series> list = new List<Series>(dic.Values);
            Geo.Winform.CommonChartForm form = new Geo.Winform.CommonChartForm(list.ToArray());
            form.Text = "Estimated Values Of First " + list.Count + " Params";
            form.Show();
        }

        /// <summary>
        /// 如果非
        /// </summary>
        /// <param name="_results"></param>
        public void DrawParamRmsLine()
        {

            if (Adjustments == null || Adjustments.Count == 0) return;

            Vector vector = CenterVector;

            int start = StartIndex;
            int end = EndIndex;

            int resultIndex = 0;
            //X
            var names = Adjustments[0].ParamNames;

            if (ParamNames != null)
            {
                names = ParamNames;
            } 

            Dictionary<string, Series> dic = new Dictionary<string, Series>();
            int i = 0;
            foreach (var name in names)
            {
                i++;
                if (i > ParamCount) break;

                Series seriesX = new Series(name);
                seriesX.ChartType = SeriesChartType.Point;
                
                dic[name] = seriesX;
            }


            foreach (var result in Adjustments.ToArray())//避免集合修改无法遍历
            {
                resultIndex++;

                //起始
                if (resultIndex > start)
                {
                    var est = result.Get(AdjustVectorType);//.Estimated;
                    int paramIndex = StartParamIndex;
                    foreach (var kv in dic)
                    {
                        if (paramIndex >= EndParamIndex) break;

                        var name = kv.Key;
                        var indexOfParam = result.ParamNames.IndexOf(name);
                        if (indexOfParam != -1)
                        {
                            var qrt = est.InverseWeight[indexOfParam, indexOfParam];
                            kv.Value.Points.Add(new DataPoint(resultIndex, Math.Sqrt(qrt)));
                        }
                        paramIndex++;
                    }
                }

                //截止
                if (resultIndex > end) break;
            }
            List<Series> list = new List<Series>(dic.Values);
            Geo.Winform.CommonChartForm form = new Geo.Winform.CommonChartForm(list.ToArray());
            form.Text = "Estimated Rms Of First " + list.Count + " Params";
            form.Show();
        }
    }
}
