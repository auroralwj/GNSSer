using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Geo.Winform.Controls
{
    public partial class Pagger
    {
        private IContainer components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStripComboBox_page = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel_pageCount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox_pagelimit = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_totalRowCount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton_first = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_prev = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_last = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.SuspendLayout();
            // 
            // toolStripComboBox_page
            // 
            this.toolStripComboBox_page.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox_page.Name = "toolStripComboBox_page";
            this.toolStripComboBox_page.Size = new System.Drawing.Size(75, 20);
            this.toolStripComboBox_page.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_page_SelectedIndexChanged);
            // 
            // toolStripLabel_pageCount
            // 
            this.toolStripLabel_pageCount.Name = "toolStripLabel_pageCount";
            this.toolStripLabel_pageCount.Size = new System.Drawing.Size(11, 12);
            this.toolStripLabel_pageCount.Text = "1";
            // 
            // toolStripComboBox_pagelimit
            // 
            this.toolStripComboBox_pagelimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox_pagelimit.Items.AddRange(new object[] {
            "5",
            "10",
            "20",
            "30",
            "50",
            "100",
            "200",
            "500",
            "1000", 
            "5000",
            "10000",
            "100000",
            "1000000",
            "10000000",
            "100000000",});
            this.toolStripComboBox_pagelimit.Name = "toolStripComboBox_pagelimit";
            this.toolStripComboBox_pagelimit.Size = new System.Drawing.Size(75, 20);
            this.toolStripComboBox_pagelimit.Text = "10";
            this.toolStripComboBox_pagelimit.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_pagelimit_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(53, 12);
            this.toolStripLabel1.Text = "每页数：";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(53, 12);
            this.toolStripLabel2.Text = "总条数：";
            // 
            // toolStripLabel_totalRowCount
            // 
            this.toolStripLabel_totalRowCount.Name = "toolStripLabel_totalRowCount";
            this.toolStripLabel_totalRowCount.Size = new System.Drawing.Size(11, 12);
            this.toolStripLabel_totalRowCount.Text = "0";
            // 
            // toolStripButton_first
            // 
            this.toolStripButton_first.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_first.Image = global::Geo.Properties.Resources.firstpage_16x40;
            this.toolStripButton_first.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_first.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_first.Name = "toolStripButton_first";
            this.toolStripButton_first.Size = new System.Drawing.Size(42, 22);
            this.toolStripButton_first.Text = "首页";
            this.toolStripButton_first.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            // 
            // toolStripButton_prev
            // 
            this.toolStripButton_prev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_prev.Image = global::Geo.Properties.Resources.prepage_16x40;
            this.toolStripButton_prev.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_prev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_prev.Name = "toolStripButton_prev";
            this.toolStripButton_prev.Size = new System.Drawing.Size(44, 20);
            this.toolStripButton_prev.Text = "上一页";
            this.toolStripButton_prev.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            // 
            // toolStripButtonNext
            // 
            this.toolStripButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNext.Image = global::Geo.Properties.Resources.nextpage_16x40;
            this.toolStripButtonNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNext.Name = "toolStripButtonNext";
            this.toolStripButtonNext.Size = new System.Drawing.Size(44, 20);
            this.toolStripButtonNext.Text = "下一页";
            this.toolStripButtonNext.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            // 
            // toolStripButton_last
            // 
            this.toolStripButton_last.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_last.Image = global::Geo.Properties.Resources.lastpage_16x40;
            this.toolStripButton_last.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_last.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_last.Name = "toolStripButton_last";
            this.toolStripButton_last.Size = new System.Drawing.Size(44, 20);
            this.toolStripButton_last.Text = "末页";
            this.toolStripButton_last.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(11, 12);
            this.toolStripLabel3.Text = "/";
            // 
            // Pagger
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_first,
            this.toolStripButton_prev,
            this.toolStripComboBox_page,
            this.toolStripLabel3,
            this.toolStripLabel_pageCount,
            this.toolStripButtonNext,
            this.toolStripButton_last,
            this.toolStripLabel1,
            this.toolStripComboBox_pagelimit,
            this.toolStripLabel2,
            this.toolStripLabel_totalRowCount});
            this.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Pagger_ItemClicked);
            this.LayoutCompleted += new System.EventHandler(this.Pagger_LayoutCompleted);
            this.ResumeLayout(false);

        }
        private ToolStripComboBox toolStripComboBox_page;
        private ToolStripLabel toolStripLabel_pageCount;

   
        private ToolStripComboBox toolStripComboBox_pagelimit;
        private ToolStripLabel toolStripLabel1;
        private ToolStripButton toolStripButton_first; 
        private ToolStripButton toolStripButton_prev;
        private ToolStripButton toolStripButtonNext;
        private ToolStripButton toolStripButton_last;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel_totalRowCount;
        private ToolStripLabel toolStripLabel3;
    }
}
