namespace Gnsser.Winform
{
    partial class PPPAmbiguityVizardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_netSolveWm = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button_coordCompare = new System.Windows.Forms.Button();
            this.button_viewWcb = new System.Windows.Forms.Button();
            this.button_multiSite = new System.Windows.Forms.Button();
            this.button_elevate = new System.Windows.Forms.Button();
            this.buttonWmNew = new System.Windows.Forms.Button();
            this.button_ppp = new System.Windows.Forms.Button();
            this.buttonNewMethod = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSatelliteSelectorForm = new System.Windows.Forms.Button();
            this.button_viewAndDrawTable = new System.Windows.Forms.Button();
            this.button_bsdSolver = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_mwExtructor = new System.Windows.Forms.Button();
            this.button_totalWLOfBSD = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_netSolveWm);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button_coordCompare);
            this.groupBox1.Controls.Add(this.button_viewWcb);
            this.groupBox1.Controls.Add(this.button_multiSite);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(666, 87);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "老算法服务端";
            // 
            // button_netSolveWm
            // 
            this.button_netSolveWm.Location = new System.Drawing.Point(19, 20);
            this.button_netSolveWm.Name = "button_netSolveWm";
            this.button_netSolveWm.Size = new System.Drawing.Size(98, 47);
            this.button_netSolveWm.TabIndex = 0;
            this.button_netSolveWm.Text = "网解宽项";
            this.button_netSolveWm.UseVisualStyleBackColor = true;
            this.button_netSolveWm.Click += new System.EventHandler(this.button_netSolveWm_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(331, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 47);
            this.button1.TabIndex = 2;
            this.button1.Text = "格式化观测文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_coordCompare
            // 
            this.button_coordCompare.Location = new System.Drawing.Point(435, 20);
            this.button_coordCompare.Name = "button_coordCompare";
            this.button_coordCompare.Size = new System.Drawing.Size(98, 47);
            this.button_coordCompare.TabIndex = 5;
            this.button_coordCompare.Text = "坐标比较";
            this.button_coordCompare.UseVisualStyleBackColor = true;
            this.button_coordCompare.Click += new System.EventHandler(this.button_coordCompare_Click);
            // 
            // button_viewWcb
            // 
            this.button_viewWcb.Location = new System.Drawing.Point(227, 20);
            this.button_viewWcb.Name = "button_viewWcb";
            this.button_viewWcb.Size = new System.Drawing.Size(84, 47);
            this.button_viewWcb.TabIndex = 3;
            this.button_viewWcb.Text = "查看IGS宽项";
            this.button_viewWcb.UseVisualStyleBackColor = true;
            this.button_viewWcb.Click += new System.EventHandler(this.button_viewWcb_Click);
            // 
            // button_multiSite
            // 
            this.button_multiSite.Location = new System.Drawing.Point(123, 20);
            this.button_multiSite.Name = "button_multiSite";
            this.button_multiSite.Size = new System.Drawing.Size(98, 47);
            this.button_multiSite.TabIndex = 1;
            this.button_multiSite.Text = "网解窄项";
            this.button_multiSite.UseVisualStyleBackColor = true;
            this.button_multiSite.Click += new System.EventHandler(this.button_multiSite_Click);
            // 
            // button_elevate
            // 
            this.button_elevate.Location = new System.Drawing.Point(19, 143);
            this.button_elevate.Name = "button_elevate";
            this.button_elevate.Size = new System.Drawing.Size(78, 47);
            this.button_elevate.TabIndex = 4;
            this.button_elevate.Text = "卫星高度角";
            this.button_elevate.UseVisualStyleBackColor = true;
            this.button_elevate.Click += new System.EventHandler(this.button_elevate_Click);
            // 
            // buttonWmNew
            // 
            this.buttonWmNew.Location = new System.Drawing.Point(216, 81);
            this.buttonWmNew.Name = "buttonWmNew";
            this.buttonWmNew.Size = new System.Drawing.Size(71, 47);
            this.buttonWmNew.TabIndex = 4;
            this.buttonWmNew.Text = "单星BSD宽巷";
            this.buttonWmNew.UseVisualStyleBackColor = true;
            this.buttonWmNew.Click += new System.EventHandler(this.buttonWmNew_Click);
            // 
            // button_ppp
            // 
            this.button_ppp.Location = new System.Drawing.Point(19, 20);
            this.button_ppp.Name = "button_ppp";
            this.button_ppp.Size = new System.Drawing.Size(279, 47);
            this.button_ppp.TabIndex = 4;
            this.button_ppp.Text = "PPP";
            this.button_ppp.UseVisualStyleBackColor = true;
            this.button_ppp.Click += new System.EventHandler(this.button_ppp_Click);
            // 
            // buttonNewMethod
            // 
            this.buttonNewMethod.Location = new System.Drawing.Point(337, 112);
            this.buttonNewMethod.Name = "buttonNewMethod";
            this.buttonNewMethod.Size = new System.Drawing.Size(118, 47);
            this.buttonNewMethod.TabIndex = 4;
            this.buttonNewMethod.Text = "BSD窄巷";
            this.buttonNewMethod.UseVisualStyleBackColor = true;
            this.buttonNewMethod.Click += new System.EventHandler(this.buttonNewMethod_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(13, 331);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(665, 156);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "客户端";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.buttonSatelliteSelectorForm);
            this.groupBox3.Controls.Add(this.button_viewAndDrawTable);
            this.groupBox3.Controls.Add(this.button_bsdSolver);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.button_elevate);
            this.groupBox3.Controls.Add(this.button_mwExtructor);
            this.groupBox3.Controls.Add(this.button_totalWLOfBSD);
            this.groupBox3.Controls.Add(this.buttonWmNew);
            this.groupBox3.Controls.Add(this.buttonNewMethod);
            this.groupBox3.Controls.Add(this.button_ppp);
            this.groupBox3.Location = new System.Drawing.Point(12, 105);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(665, 209);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "新算法服务端";
            // 
            // buttonSatelliteSelectorForm
            // 
            this.buttonSatelliteSelectorForm.Location = new System.Drawing.Point(125, 143);
            this.buttonSatelliteSelectorForm.Name = "buttonSatelliteSelectorForm";
            this.buttonSatelliteSelectorForm.Size = new System.Drawing.Size(56, 46);
            this.buttonSatelliteSelectorForm.TabIndex = 7;
            this.buttonSatelliteSelectorForm.Text = "选星";
            this.buttonSatelliteSelectorForm.UseVisualStyleBackColor = true;
            this.buttonSatelliteSelectorForm.Click += new System.EventHandler(this.buttonSatelliteSelectorForm_Click);
            // 
            // button_viewAndDrawTable
            // 
            this.button_viewAndDrawTable.Location = new System.Drawing.Point(482, 20);
            this.button_viewAndDrawTable.Name = "button_viewAndDrawTable";
            this.button_viewAndDrawTable.Size = new System.Drawing.Size(72, 139);
            this.button_viewAndDrawTable.TabIndex = 3;
            this.button_viewAndDrawTable.Text = "表格数据查看与绘图";
            this.button_viewAndDrawTable.UseVisualStyleBackColor = true;
            this.button_viewAndDrawTable.Click += new System.EventHandler(this.button_viewAndDrawTable_Click);
            // 
            // button_bsdSolver
            // 
            this.button_bsdSolver.Location = new System.Drawing.Point(331, 20);
            this.button_bsdSolver.Name = "button_bsdSolver";
            this.button_bsdSolver.Size = new System.Drawing.Size(118, 44);
            this.button_bsdSolver.TabIndex = 6;
            this.button_bsdSolver.Text = "单星BSD集成计算";
            this.button_bsdSolver.UseVisualStyleBackColor = true;
            this.button_bsdSolver.Click += new System.EventHandler(this.button_bsdSolver_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(297, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "-->";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(298, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "\\";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(295, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "--->";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(189, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "->";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "->";
            // 
            // button_mwExtructor
            // 
            this.button_mwExtructor.Location = new System.Drawing.Point(19, 81);
            this.button_mwExtructor.Name = "button_mwExtructor";
            this.button_mwExtructor.Size = new System.Drawing.Size(163, 45);
            this.button_mwExtructor.TabIndex = 4;
            this.button_mwExtructor.Text = "MW提取平滑器";
            this.button_mwExtructor.UseVisualStyleBackColor = true;
            this.button_mwExtructor.Click += new System.EventHandler(this.button_mwExtructor_Click);
            // 
            // button_totalWLOfBSD
            // 
            this.button_totalWLOfBSD.Location = new System.Drawing.Point(216, 143);
            this.button_totalWLOfBSD.Name = "button_totalWLOfBSD";
            this.button_totalWLOfBSD.Size = new System.Drawing.Size(71, 47);
            this.button_totalWLOfBSD.TabIndex = 4;
            this.button_totalWLOfBSD.Text = "全历元分段BSD宽巷";
            this.button_totalWLOfBSD.UseVisualStyleBackColor = true;
            this.button_totalWLOfBSD.Click += new System.EventHandler(this.button_totalWLOfBSD_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 499);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "PPP模糊度解算";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_multiSite;
        private System.Windows.Forms.Button button_viewWcb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_coordCompare;
        private System.Windows.Forms.Button button_netSolveWm;
        private System.Windows.Forms.Button buttonNewMethod;
        private System.Windows.Forms.Button buttonWmNew;
        private System.Windows.Forms.Button button_elevate;
        private System.Windows.Forms.Button button_ppp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_viewAndDrawTable;
        private System.Windows.Forms.Button button_bsdSolver;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_mwExtructor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_totalWLOfBSD;
        private System.Windows.Forms.Button buttonSatelliteSelectorForm;
    }
}