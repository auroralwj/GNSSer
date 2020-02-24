using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Interoperation.Bernese;
using Geo.Times;
using Gnsser.Api;
using System.IO;
using Geo;

namespace Gnsser.Winform
{
    public partial class TaskEditForm : Form
    {
        public TaskEditForm()
        {
            InitializeComponent();
            Init();
        }
        OperationManager OperationManager = Setting.CurrentOperationManager;
        public static int BasicId = 0;

        private void Init()
        {
            this.textBox_id.Text = ++BasicId + "";

            this.comboBox_solveType.DataSource = OperationManager.Keys;// Enum.GetNames(typeof(Gnsser.Interoperation.Bernese.PcfName));
        }

        public TaskEditForm(Task Task)
        {
            InitializeComponent();
            Init();
            this.Task = Task;
        }
        private void TaskEditForm_Load(object sender, EventArgs e)
        {

        }
        Task task;
        public Task Task
        {
            get { return task; }
            set
            {
                task = value;
                this.textBox_id.Text = value.Id.ToString();
                this.textBox_name.Text = value.Name;
                this.textBox_campaign.Text = value.Campaign.ToString();
                this.textBox_resultFtp.Text = value.ResultFtp;
                this.dateTimePicker1.Value = value.Time.DateTime;
                this.comboBox_solveType.SelectedItem = task.OperationName.ToString(); 
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (Task == null)
            {
                task = new Task();
                task.Sites = new List<Site>();
            }

            Task.Id = int.Parse(this.textBox_id.Text.Trim()); 

            Task.Name = this.textBox_name.Text.Trim();//cui 此处有BUG，暂时根据名称确定测站名
            char[] spliter = new char[] { ' ', ',', '-', ':' };
            string[] sites = Task.Name.Split(spliter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var site in sites)
            {
                Task.Sites.Add(new Site() { Name = site });
            }
            Task.ResultFtp = this.textBox_resultFtp.Text.Trim();
            Task.Time = new Time(this.dateTimePicker1.Value);
            Task.OperationName =  this.comboBox_solveType.SelectedItem.ToString();
            Task.Campaign = this.textBox_campaign.Text.Trim();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

    }
}
