//2014.12.05, czs, create in jinxingliaomao, 并行设置

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using  System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo.Winform.Controls
{
    public partial class ParallelConfigControl : UserControl, Geo.IParallelConfig
    {
        public ParallelConfigControl()
        {
            InitializeComponent();

            this.SetMaxDegreeOfParallelism (Environment.ProcessorCount);
            this.label_processorCount.Text = this.MaxDegreeOfParallelism.ToString();
            this.textBox_parallelism.Text = this.MaxDegreeOfParallelism.ToString();
        }

        private void checkBox_parallel_CheckedChanged(object sender, EventArgs e)
        {
            this.panel_parallelism.Enabled = this.EnableParallel;
        }
        public ParallelOptions ParallelOptions
        {
            get
            {
                ParallelOptions option = new ParallelOptions();
                option.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
                return option;
            }
        }
        /// <summary>
        /// 是否启用并行计算。
        /// </summary>
        public bool EnableParallel { get { return checkBox_parallel.Checked; } set { checkBox_parallel.Checked = value; } }
        /// <summary>
        /// 并行度
        /// </summary>
        public int MaxDegreeOfParallelism { get { return int.Parse(this.textBox_parallelism.Text);  } }

        public void SetMaxDegreeOfParallelism(int maxDegree)
        {
            this.textBox_parallelism.Text = maxDegree.ToString();
        }
    }
}
