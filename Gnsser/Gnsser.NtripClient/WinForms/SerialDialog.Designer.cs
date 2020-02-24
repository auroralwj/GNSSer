// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports


namespace Gnsser.Ntrip
{
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public 
	partial class SerialDialog : System.Windows.Forms.Form
	{
		
		//Form overrides dispose to clean up the component colName.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the obsCode editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.boxSerialPort = new System.Windows.Forms.ComboBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.boxSpeed = new System.Windows.Forms.ComboBox();
            this.boxDataBits = new System.Windows.Forms.ComboBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.boxMsgRate = new System.Windows.Forms.ComboBox();
            this.lblMsgRate = new System.Windows.Forms.Label();
            this.boxCorrDataType = new System.Windows.Forms.ComboBox();
            this.lblCorrDataType = new System.Windows.Forms.Label();
            this.boxReceiverType = new System.Windows.Forms.ComboBox();
            this.Label15 = new System.Windows.Forms.Label();
            this.TableLayoutPanel1.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel1.ColumnCount = 2;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Controls.Add(this.OK_Button, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.Cancel_Button, 1, 0);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(369, 395);
            this.TableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 1;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(195, 40);
            this.TableLayoutPanel1.TabIndex = 0;
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OK_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK_Button.Location = new System.Drawing.Point(4, 3);
            this.OK_Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(89, 33);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "ȷ��";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel_Button.Location = new System.Drawing.Point(101, 3);
            this.Cancel_Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(89, 33);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "ȡ��";
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(48, 22);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(58, 25);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "����:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(28, 62);
            this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(78, 25);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "������:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(28, 101);
            this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(78, 25);
            this.Label3.TabIndex = 4;
            this.Label3.Text = "����λ:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(28, 140);
            this.Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(78, 25);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "У��λ:";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(28, 179);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(78, 25);
            this.Label5.TabIndex = 6;
            this.Label5.Text = "ֹͣλ:";
            // 
            // boxSerialPort
            // 
            this.boxSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxSerialPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxSerialPort.FormattingEnabled = true;
            this.boxSerialPort.Location = new System.Drawing.Point(131, 22);
            this.boxSerialPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxSerialPort.Name = "boxSerialPort";
            this.boxSerialPort.Size = new System.Drawing.Size(403, 33);
            this.boxSerialPort.TabIndex = 7;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(339, 65);
            this.Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(112, 25);
            this.Label6.TabIndex = 8;
            this.Label6.Text = "bits/second";
            // 
            // boxSpeed
            // 
            this.boxSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxSpeed.FormattingEnabled = true;
            this.boxSpeed.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.boxSpeed.Location = new System.Drawing.Point(131, 61);
            this.boxSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxSpeed.Name = "boxSpeed";
            this.boxSpeed.Size = new System.Drawing.Size(199, 33);
            this.boxSpeed.TabIndex = 9;
            // 
            // boxDataBits
            // 
            this.boxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxDataBits.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxDataBits.FormattingEnabled = true;
            this.boxDataBits.Items.AddRange(new object[] {
            "7",
            "8"});
            this.boxDataBits.Location = new System.Drawing.Point(131, 100);
            this.boxDataBits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxDataBits.Name = "boxDataBits";
            this.boxDataBits.Size = new System.Drawing.Size(101, 33);
            this.boxDataBits.TabIndex = 10;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(131, 143);
            this.Label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(59, 25);
            this.Label7.TabIndex = 13;
            this.Label7.Text = "None";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(131, 182);
            this.Label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(23, 25);
            this.Label8.TabIndex = 14;
            this.Label8.Text = "1";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.Label8);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.Label7);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.boxDataBits);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.boxSpeed);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.boxSerialPort);
            this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(21, 14);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GroupBox1.Size = new System.Drawing.Size(543, 217);
            this.GroupBox1.TabIndex = 15;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "��������";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.boxMsgRate);
            this.GroupBox2.Controls.Add(this.lblMsgRate);
            this.GroupBox2.Controls.Add(this.boxCorrDataType);
            this.GroupBox2.Controls.Add(this.lblCorrDataType);
            this.GroupBox2.Controls.Add(this.boxReceiverType);
            this.GroupBox2.Controls.Add(this.Label15);
            this.GroupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(21, 238);
            this.GroupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GroupBox2.Size = new System.Drawing.Size(543, 147);
            this.GroupBox2.TabIndex = 16;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "���ջ��Զ�����";
            // 
            // boxMsgRate
            // 
            this.boxMsgRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxMsgRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxMsgRate.FormattingEnabled = true;
            this.boxMsgRate.Items.AddRange(new object[] {
            "1 Hz",
            "5 Hz",
            "10 Hz"});
            this.boxMsgRate.Location = new System.Drawing.Point(316, 100);
            this.boxMsgRate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxMsgRate.Name = "boxMsgRate";
            this.boxMsgRate.Size = new System.Drawing.Size(217, 33);
            this.boxMsgRate.TabIndex = 17;
            // 
            // lblMsgRate
            // 
            this.lblMsgRate.AutoSize = true;
            this.lblMsgRate.Font = new System.Drawing.Font("΢���ź�", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsgRate.Location = new System.Drawing.Point(3, 104);
            this.lblMsgRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMsgRate.Name = "lblMsgRate";
            this.lblMsgRate.Size = new System.Drawing.Size(152, 20);
            this.lblMsgRate.TabIndex = 16;
            this.lblMsgRate.Text = "GGA �� RMC��Ϣ����:";
            // 
            // boxCorrDataType
            // 
            this.boxCorrDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxCorrDataType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxCorrDataType.FormattingEnabled = true;
            this.boxCorrDataType.Location = new System.Drawing.Point(251, 61);
            this.boxCorrDataType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxCorrDataType.Name = "boxCorrDataType";
            this.boxCorrDataType.Size = new System.Drawing.Size(283, 33);
            this.boxCorrDataType.TabIndex = 15;
            // 
            // lblCorrDataType
            // 
            this.lblCorrDataType.AutoSize = true;
            this.lblCorrDataType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorrDataType.Location = new System.Drawing.Point(3, 65);
            this.lblCorrDataType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCorrDataType.Name = "lblCorrDataType";
            this.lblCorrDataType.Size = new System.Drawing.Size(118, 25);
            this.lblCorrDataType.TabIndex = 10;
            this.lblCorrDataType.Text = "��������ʽ:";
            // 
            // boxReceiverType
            // 
            this.boxReceiverType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxReceiverType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxReceiverType.FormattingEnabled = true;
            this.boxReceiverType.Items.AddRange(new object[] {
            "���Զ�����",
            "NovAtel OEMV ����ϵ��"});
            this.boxReceiverType.Location = new System.Drawing.Point(161, 22);
            this.boxReceiverType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxReceiverType.Name = "boxReceiverType";
            this.boxReceiverType.Size = new System.Drawing.Size(372, 33);
            this.boxReceiverType.TabIndex = 9;
            this.boxReceiverType.SelectedIndexChanged += new System.EventHandler(this.boxReceiverType_SelectedIndexChanged);
            this.boxReceiverType.SelectionChangeCommitted += new System.EventHandler(this.boxReceiverType_SelectionChangeCommitted);
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label15.Location = new System.Drawing.Point(3, 25);
            this.Label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(118, 25);
            this.Label15.TabIndex = 6;
            this.Label15.Text = "���ջ�����:";
            // 
            // SerialDialog
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(580, 442);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.TableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SerialDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "��������";
            this.Load += new System.EventHandler(this.SerialDialog_Load);
            this.TableLayoutPanel1.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
		internal System.Windows.Forms.Button OK_Button;
		internal System.Windows.Forms.Button Cancel_Button;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.ComboBox boxSerialPort;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.ComboBox boxSpeed;
		internal System.Windows.Forms.ComboBox boxDataBits;
		internal System.Windows.Forms.Label Label7;
		internal System.Windows.Forms.Label Label8;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.GroupBox GroupBox2;
		internal System.Windows.Forms.ComboBox boxReceiverType;
		internal System.Windows.Forms.Label Label15;
		internal System.Windows.Forms.Label lblCorrDataType;
		internal System.Windows.Forms.ComboBox boxCorrDataType;
		internal System.Windows.Forms.ComboBox boxMsgRate;
		internal System.Windows.Forms.Label lblMsgRate;
		
	}
	
}
