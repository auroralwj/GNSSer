//2018.10.15, czs, create in hmx, GNSSer 窗口工具

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Geo.Winform;
using System.Windows.Forms;
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// GNSSer 窗口工具
    /// </summary>
    public class GnsserFormUtil
    {
        /// <summary>
        /// 打开卫星高度角窗口
        /// </summary>
        /// <param name="FilePath"></param>
        public static void OpenSatElevationTableForm(object FilePath)
        {
            var table = SatElevatoinTableBuilder.BuildTable((string)FilePath, 10);
            Application.Run(new TableObjectViewForm(table));//保证在线程中也可以显示

           //TableObjectViewForm form = new TableObjectViewForm(table);
           // form.Show();
        }

        /// <summary>
        /// 显示相位图
        /// </summary>
        /// <param name="path"></param>
        public static void ShowPhaseChartForm(string path)
        {
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);

                var titles = table.ParamNames.FindAll(m => m.EndsWith(ParamNames.PhaseL) || m.EndsWith(ParamNames.L1) || m.EndsWith(ParamNames.L2));
                var phaseObsTable = table.GetTable(table.Name, titles);
                new Geo.Winform.CommonChartForm(phaseObsTable).Show();
            }
        }
        /// <summary>
        /// 读取并显示绘图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="paramNames"></param>
        public static void ShowChartForm(string path, List<string> paramNames)
        {
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                var phaseObsTable = table.GetTable(table.Name, paramNames);
                new Geo.Winform.CommonChartForm(phaseObsTable).Show();
            }
        }
        /// <summary>
        /// 设置样式后绘制图
        /// </summary>
        /// <param name="path"></param>
        public static void SetThenShowChartForm(string path)
        {
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);

                DrawObjectOptionForm form = new DrawObjectOptionForm(table);
                form.Show();
            }
        }
        /// <summary>
        /// 设置样式后绘制图
        /// </summary>
        /// <param name="path"></param>
        public static void SetThenShowChartForm(string [] path)
        {
                DrawObjectOptionForm form = new DrawObjectOptionForm(path);
                form.Show(); 
        }
    }
}