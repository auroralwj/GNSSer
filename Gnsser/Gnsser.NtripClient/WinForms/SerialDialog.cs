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
    public partial class SerialDialog
    {
        public SerialDialog()
        {
            InitializeComponent();

            //Added to support default instance behavour in C#
            if (defaultInstance == null)
                defaultInstance = this;
        }

        #region Default Instance

        private static SerialDialog defaultInstance;

        /// <summary>
        /// Added by the VB.Net to C# Converter to support default instance behavour in C#
        /// </summary>
        public static SerialDialog Default
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new SerialDialog();
                    defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
                }

                return defaultInstance;
            }
            set
            {
                defaultInstance = value;
            }
        }

        static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
        {
            defaultInstance = null;
        }

        #endregion
        public void boxReceiverType_SelectionChangeCommitted(System.Object sender, System.EventArgs e)
        {
            RedisplayAutoConfigOptions(boxReceiverType.SelectedIndex);
        }

        public void RedisplayAutoConfigOptions(int ReceiverType)
        {
            switch (ReceiverType)
            {
                case 1:
                    boxCorrDataType.Items.Clear();
                    boxCorrDataType.Items.Add("RTCM v2");
                    boxCorrDataType.Items.Add("RTCM v3");
                    boxCorrDataType.Items.Add("CMR or CMR+");
                    boxCorrDataType.Items.Add("RTCA");
                    boxCorrDataType.Items.Add("OmniSTAR");
                    boxCorrDataType.Items.Add("NovAtel");
                    switch (".ReceiverCorrDataType.ToLower()")
                    {
                        case "rtcm":
                            boxCorrDataType.SelectedIndex = 0;
                            break;
                        case "rtcmv3":
                            boxCorrDataType.SelectedIndex = 1;
                            break;
                        case "cmr":
                            boxCorrDataType.SelectedIndex = 2;
                            break;
                        case "rtca":
                            boxCorrDataType.SelectedIndex = 3;
                            break;
                        case "omnistar":
                            boxCorrDataType.SelectedIndex = 4;
                            break;
                        default:
                            boxCorrDataType.SelectedIndex = 5;
                            break;
                    }
                    lblCorrDataType.Visible = true;
                    boxCorrDataType.Visible = true;

                    switch (5)//"NtripParam.ReceiverMessageRate")
                    {
                        case 5:
                            boxMsgRate.SelectedIndex = 1;
                            break;
                        case 10:
                            boxMsgRate.SelectedIndex = 2;
                            break;
                        default:
                            boxMsgRate.SelectedIndex = 0;
                            break;
                    }

                    lblMsgRate.Visible = true;
                    boxMsgRate.Visible = true;
                    break;


                default:
                    lblCorrDataType.Visible = false;
                    boxCorrDataType.Visible = false;

                    lblMsgRate.Visible = false;
                    boxMsgRate.Visible = false;
                    break;
            }
        }

        public NtripOption NtripParam { get; set; }

        public void OK_Button_Click(System.Object sender, System.EventArgs e)
        {  
             
                if ((string)this.boxSerialPort.SelectedItem == "未发现串口")
                {
                    //Do nothing here
                }
                else //Some serial port was selected
                {
                    NtripParam.SerialPort = int.Parse(Strings.Replace(System.Convert.ToString(this.boxSerialPort.SelectedItem), "COM", "", 1, -1, (Microsoft.VisualBasic.CompareMethod)0));
                    NtripParam.SaveSetting("Serial Port Number", NtripParam.SerialPort + "");
                }

                switch (this.boxSpeed.SelectedIndex)
                {
                    case 0:
                        NtripParam.SerialSpeed = 2400;
                        break;
                    case 1:
                        NtripParam.SerialSpeed = 4800;
                        break;
                    case 2:
                        NtripParam.SerialSpeed = 9600;
                        break;
                    case 3:
                        NtripParam.SerialSpeed = 14400;
                        break;
                    case 4:
                        NtripParam.SerialSpeed = 19200;
                        break;
                    case 5:
                        NtripParam.SerialSpeed = 38400;
                        break;
                    case 6:
                        NtripParam.SerialSpeed = 57600;
                        break;
                    case 7:
                        NtripParam.SerialSpeed = 115200;
                        break;
                    case 8:
                        break;
                    //custom speed selected. Don't change it
                }

                if (this.boxDataBits.SelectedIndex == 0)
                {
                    NtripParam.SerialDataBits = 7;
                }
                else
                {
                    NtripParam.SerialDataBits = 8;
                }


                switch (this.boxReceiverType.SelectedIndex)
                {
                    case 1:
                        NtripParam.ReceiverType = (ReceiverType)1;
                        switch (this.boxCorrDataType.SelectedIndex)
                        {
                            case 1:
                                NtripParam.ReceiverCorrDataType = "RTCMV3";
                                break;
                            case 2:
                                NtripParam.ReceiverCorrDataType = "CMR";
                                break;
                            case 3:
                                NtripParam.ReceiverCorrDataType = "RTCA";
                                break;
                            case 4:
                                NtripParam.ReceiverCorrDataType = "OMNISTAR";
                                break;
                            case 5:
                                NtripParam.ReceiverCorrDataType = "NOVATEL";
                                break;
                            default:
                                NtripParam.ReceiverCorrDataType = "RTCM";
                                break;
                        }

                        switch (this.boxMsgRate.SelectedIndex)
                        {
                            case 1:
                                NtripParam.ReceiverMessageRate = 5;
                                break;
                            case 2:
                                NtripParam.ReceiverMessageRate = 10;
                                break;
                            case 3:
                                NtripParam.ReceiverMessageRate = 1;
                                break;
                        }

                        NtripParam.SaveSetting("Receiver Type", "NovAtel", "Receiver Correction Format", NtripParam.ReceiverCorrDataType, "Receiver Message Rate", System.Convert.ToString(NtripParam.ReceiverMessageRate));
                        break;
                    default:
                        NtripParam.ReceiverType = ReceiverType.Unkown;
                        NtripParam.SaveSetting("Receiver Type", "NoAutoConfig");
                        break;
                }



                NtripParam.SaveSetting("Serial Port Speed", System.Convert.ToString(NtripParam.SerialSpeed), "Serial Port Data Bits", System.Convert.ToString(NtripParam.SerialDataBits), "Serial Port Stop Bits", System.Convert.ToString(NtripParam.SerialStopBits));

                MessageBox.Show("Serial Port Settings Saved");


                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close(); 
        }


        public void Cancel_Button_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void boxReceiverType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SerialDialog_Load(object sender, EventArgs e)
        {
            this.boxSerialPort.Items.Clear();
            string targetport = "COM" + NtripParam.SerialPort.ToString();

            int i = 0;
            int portindex = 0;
            foreach (string portName in (new Microsoft.VisualBasic.Devices.Computer()).Ports.SerialPortNames)
            {
                char[] portNumberChars = portName.Substring(3).ToCharArray(); //Remove "COM", put the rest in a character array
                //???
                string newportName = "COM"; //Start over with "COM" VBConversions Warning: A foreach variable can't be assigned to in C#.
                foreach (char portNumberChar in portNumberChars)
                {
                    if (char.IsDigit(portNumberChar)) //Good character, append to portName
                    {
                        //???
                        newportName += portNumberChar.ToString();// VBConversions Warning: A foreach variable can't be assigned to in C#.
                    }
                }
                this.boxSerialPort.Items.Add(newportName);
                if (newportName == targetport)
                {
                    portindex = i;
                }
                i++;
            }
            if (i == 0)
            {
                this.boxSerialPort.Items.Add("未发现串口");
            }
            this.boxSerialPort.SelectedIndex = portindex;

            if (this.boxSpeed.Items.Count == 9)
            {
                this.boxSpeed.Items.RemoveAt(8);
            }

            switch (NtripParam.SerialSpeed)
            {
                case 2400:
                    this.boxSpeed.SelectedIndex = 0;
                    break;
                case 4800:
                    this.boxSpeed.SelectedIndex = 1;
                    break;
                case 9600:
                    this.boxSpeed.SelectedIndex = 2;
                    break;
                case 14400:
                    this.boxSpeed.SelectedIndex = 3;
                    break;
                case 19200:
                    this.boxSpeed.SelectedIndex = 4;
                    break;
                case 38400:
                    this.boxSpeed.SelectedIndex = 5;
                    break;
                case 57600:
                    this.boxSpeed.SelectedIndex = 6;
                    break;
                case 115200:
                    this.boxSpeed.SelectedIndex = 7;
                    break;
                default: //How did this happen
                    if (this.boxSpeed.Items.Count == 8)
                    {
                        this.boxSpeed.Items.Add(NtripParam.SerialSpeed.ToString());
                    }
                    this.boxSpeed.SelectedIndex = 8;
                    break;
            }

            if (NtripParam.SerialDataBits == 7)
            {
                this.boxDataBits.SelectedIndex = 0;
            }
            else
            {
                this.boxDataBits.SelectedIndex = 1;
            }



            //Receiver auto-config stuff
            switch (NtripParam.ReceiverType)
            {
                case ReceiverType.Novatel:
                    this.boxReceiverType.SelectedIndex = 1;
                    break;
                default:
                    this.boxReceiverType.SelectedIndex = 0;
                    break;
            }
            this.RedisplayAutoConfigOptions((int)NtripParam.ReceiverType);


        }
    }
}
