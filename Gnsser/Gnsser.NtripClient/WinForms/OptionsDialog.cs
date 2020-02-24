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
    public partial class OptionsDialog
    {
        public OptionsDialog()
        {
            InitializeComponent();

            //Added to support default instance behavour in C#
            if (defaultInstance == null)
                defaultInstance = this;
        }

        public NtripOption NtripParam { get; set; }
        public LocalGps GPS { get; set; }

        #region Default Instance

        private static OptionsDialog defaultInstance;

        /// <summary>
        /// Added by the VB.Net to C# Converter to support default instance behavour in C#
        /// </summary>
        public static OptionsDialog Default
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new OptionsDialog();
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


        public void btnAudioHelp_Click(System.Object sender, System.EventArgs e)
        {
            MessageBox.Show("To select an audio alert, you will need to put a .wav file in the same folder as this application.");
        }


        public void OK_Button_Click(System.Object sender, System.EventArgs e)
        {

            switch (this.boxText2.SelectedIndex)
            {
                case 1:
                    GPS.Disp2 = "age";
                    break;
                case 2:
                    GPS.Disp2 = "hdop";
                    break;
                case 3:
                    GPS.Disp2 = "vdop";
                    break;
                case 4:
                    GPS.Disp2 = "pdop";
                    break;
                case 5:
                    GPS.Disp2 = "elevation-feet";
                    break;
                case 6:
                    GPS.Disp2 = "elevation-meters";
                    break;
                case 7:
                    GPS.Disp2 = "speed-mph";
                    break;
                case 8:
                    GPS.Disp2 = "speed-mph-smoothed";
                    break;
                case 9:
                    GPS.Disp2 = "speed-kmh";
                    break;
                case 10:
                    GPS.Disp2 = "speed-kmh-smoothed";
                    break;
                case 11:
                    GPS.Disp2 = "heading";
                    break;
                default:
                    GPS.Disp2 = "";
                    break;
            }
            this.NtripParam.SaveSetting("Display Center", GPS.Disp2);

            switch (this.boxText3.SelectedIndex)
            {
                case 1:
                    GPS.Disp3 = "age";
                    break;
                case 2:
                    GPS.Disp3 = "hdop";
                    break;
                case 3:
                    GPS.Disp3 = "vdop";
                    break;
                case 4:
                    GPS.Disp3 = "pdop";
                    break;
                case 5:
                    GPS.Disp3 = "elevation-feet";
                    break;
                case 6:
                    GPS.Disp3 = "elevation-meters";
                    break;
                case 7:
                    GPS.Disp3 = "speed-mph";
                    break;
                case 8:
                    GPS.Disp3 = "speed-mph-smoothed";
                    break;
                case 9:
                    GPS.Disp3 = "speed-kmh";
                    break;
                case 10:
                    GPS.Disp3 = "speed-kmh-smoothed";
                    break;
                case 11:
                    GPS.Disp3 = "heading";
                    break;
                default:
                    GPS.Disp3 = "";
                    break;
            }
            this.NtripParam.SaveSetting("Display Right", GPS.Disp3);

            if (this.boxAudioFile.SelectedItem != null && !(this.boxAudioFile.SelectedItem.ToString() == this.NtripParam.AudioFile))
            {
                this.NtripParam.AudioFile = System.Convert.ToString(this.boxAudioFile.SelectedItem);
                this.NtripParam.SaveSetting("Audio Alert File", this.NtripParam.AudioFile);
            }

            if (this.boxDoLogging.SelectedIndex == 0)
            {
                if (NtripParam.WriteEventsToFile)
                {
                    this.NtripParam.SaveSetting("Write Events to File", "False");
                    NtripParam.WriteEventsToFile = false;
                }
            }
            else
            {
                if (!NtripParam.WriteEventsToFile)
                {
                    this.NtripParam.SaveSetting("Write Events to File", "True");
                    NtripParam.WriteEventsToFile = true;
                }
            }

            if (this.boxDoSaveNMEA.SelectedIndex == 0)
            {
                if (NtripParam.WriteNMEAToFile)
                {
                    this.NtripParam.SaveSetting("Write NMEA to File", "False");
                    NtripParam.WriteNMEAToFile = false;
                }
            }
            else
            {
                if (!NtripParam.WriteNMEAToFile)
                {
                    this.NtripParam.SaveSetting("Write NMEA to File", "True");
                    NtripParam.WriteNMEAToFile = true;
                }
            }


            //this.NtripParam.RedisplayTexts2and3();
            MessageBox.Show("Options Saved");

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        




        public void Cancel_Button_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void OptionsDialog_Load(object sender, EventArgs e)
        {

            (new Microsoft.VisualBasic.Devices.Audio()).Stop(); //In case some long .wav file is still playing

            switch (GPS.Disp2)
            {
                case "age":
                    this.boxText2.SelectedIndex = 1;
                    break;
                case "hdop":
                    this.boxText2.SelectedIndex = 2;
                    break;
                case "vdop":
                    this.boxText2.SelectedIndex = 3;
                    break;
                case "pdop":
                    this.boxText2.SelectedIndex = 4;
                    break;
                case "elevation-feet":
                    this.boxText2.SelectedIndex = 5;
                    break;
                case "elevation-meters":
                    this.boxText2.SelectedIndex = 6;
                    break;
                case "speed-mph":
                    this.boxText2.SelectedIndex = 7;
                    break;
                case "speed-mph-smoothed":
                    this.boxText2.SelectedIndex = 8;
                    break;
                case "speed-kmh":
                    this.boxText2.SelectedIndex = 9;
                    break;
                case "speed-kmh-smoothed":
                    this.boxText2.SelectedIndex = 10;
                    break;
                case "heading":
                    this.boxText2.SelectedIndex = 11;
                    break;
                default:
                    this.boxText2.SelectedIndex = 0;
                    break;
            }

            switch (GPS.Disp3)
            {
                case "age":
                    this.boxText3.SelectedIndex = 1;
                    break;
                case "hdop":
                    this.boxText3.SelectedIndex = 2;
                    break;
                case "vdop":
                    this.boxText3.SelectedIndex = 3;
                    break;
                case "pdop":
                    this.boxText3.SelectedIndex = 4;
                    break;
                case "elevation-feet":
                    this.boxText3.SelectedIndex = 5;
                    break;
                case "elevation-meters":
                    this.boxText3.SelectedIndex = 6;
                    break;
                case "speed-mph":
                    this.boxText3.SelectedIndex = 7;
                    break;
                case "speed-mph-smoothed":
                    this.boxText3.SelectedIndex = 8;
                    break;
                case "speed-kmh":
                    this.boxText3.SelectedIndex = 9;
                    break;
                case "speed-kmh-smoothed":
                    this.boxText3.SelectedIndex = 10;
                    break;
                case "heading":
                    this.boxText3.SelectedIndex = 11;
                    break;
                default:
                    this.boxText3.SelectedIndex = 0;
                    break;
            }

            this.boxAudioFile.Items.Clear();
            this.boxAudioFile.Items.Add("None");
            this.boxAudioFile.SelectedIndex = 0;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath);
            foreach (System.IO.FileInfo fi in di.GetFiles())
            {
                if (fi.Extension == ".wav")
                {
                    //MsgBox(fi.Name)
                    this.boxAudioFile.Items.Add(fi.Name);
                    if (fi.Name == this.NtripParam.AudioFile)
                    {
                        this.boxAudioFile.SelectedIndex = this.boxAudioFile.Items.Count - 1;
                    }
                }
            }

            if (NtripParam.WriteEventsToFile)
            {
                this.boxDoLogging.SelectedIndex = 1;
            }
            else
            {
                this.boxDoLogging.SelectedIndex = 0;
            }

            if (NtripParam.WriteNMEAToFile)
            {
                this.boxDoSaveNMEA.SelectedIndex = 1;
            }
            else
            {
                this.boxDoSaveNMEA.SelectedIndex = 0;
            }

        }


    }

}
