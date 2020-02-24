//2019.01.16, czs, create in hmx, 结果背景类型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Gnsser.Winform
{

    /// <summary>
    /// 结果背景类型
    /// </summary>
    public class ResultBackGroundStyle
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GnssGradeType"></param>
        public ResultBackGroundStyle(GnssGradeType GnssGradeType)
        {
            BackGroundBrush = new SolidBrush(Color.White);// Brushes.White;
            BackGroundBrushSelected = new SolidBrush(Color.DarkGray);//Brushes.DarkGray;
            var ResultState = GnssGradeTypeHelper.GradeToResultState(GnssGradeType);
            this.Init(ResultState);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ResultState"></param>
        public ResultBackGroundStyle(ResultState ResultState)
        {
            BackGroundBrush = new SolidBrush(Color.White);// Brushes.White;
            BackGroundBrushSelected = new SolidBrush(Color.DarkGray);//Brushes.DarkGray;
            this.Init(ResultState);
        }
        /// <summary>
        /// 背景刷子
        /// </summary>
        public SolidBrush BackGroundBrush { get; set; }
        /// <summary>
        /// 选择后的背景
        /// </summary>
        public SolidBrush BackGroundBrushSelected { get; set; }

        public ResultState ResultState { get; set; }

        public void Init(ResultState ResultState)
        {
            this.ResultState = ResultState;

            switch (ResultState)
            {
                case ResultState.Unknown:
                    break;
                case ResultState.Good:
                    BackGroundBrush = new SolidBrush(Color.FromArgb(255, 20, 150, 50));// Brushes.DarkGreen;// Brushes.LawnGreen;
                    BackGroundBrushSelected = new SolidBrush(Color.FromArgb(255, 50, 200, 80));
                    break;
                case ResultState.Acceptable:
                    BackGroundBrush = new SolidBrush(Color.FromArgb(255, 100, 150, 50));//  Brushes.GreenYellow;
                    BackGroundBrushSelected = new SolidBrush(Color.FromArgb(255, 150, 200, 80));
                    break;
                case ResultState.Warning:
                    BackGroundBrush = new SolidBrush(Color.FromArgb(255, 200, 180, 50));// Brushes.YellowGreen;
                    BackGroundBrushSelected = new SolidBrush(Color.FromArgb(255, 230, 200, 100));
                    break;
                case ResultState.Bad:
                    BackGroundBrush = new SolidBrush(Color.FromArgb(255, 200, 50, 25));//Brushes.OrangeRed;
                    BackGroundBrushSelected = new SolidBrush(Color.FromArgb(255, 250, 200, 150));
                    break;
                default:
                    break;
            }

        }
    }
}
