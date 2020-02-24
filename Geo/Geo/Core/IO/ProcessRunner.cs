//2014.05.21,czs, add logs
//2014.11.24, lv, add StandardError, by cy
//2015.07.12, czs, �޸ģ����ٻ��������г��� , ���Ի����κο�ִ�г���


using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Diagnostics;//���룬ʹ�ý����࣬������������
using Microsoft.CSharp;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Geo.Common
{
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="cmd"></param> 
    /// <returns></returns>
    delegate void CmdHandler(string cmd);

    /// <summary>
    ///  ��װ��һ��Process�ࡣ��ָ�������ṩ�����Ĳ�����
    ///  ͨ��һ���߳̽����ˣ��������ٽ���һ����
    /// </summary>
    public class ProcessRunner : IDisposable
    {
        /// <summary>
        /// �Ƿ����õ���ģʽ�����Ƿ���ʾ��ɫ�����
        /// </summary>
        public bool IsDebug = false;
        /// <summary>
        /// �����˳��¼�
        /// </summary>
        public event EventHandler ExitedOrDisposed;
        /// <summary>
        /// �����ó�����������¼�
        /// </summary>          
        public event DataReceivedEventHandler ErrorDataReceived; 
        /// <summary>
        /// �������
        /// </summary>
        public event DataReceivedEventHandler OutputDataReceived;

        /// <summary>
        /// �첽ִ���г���ִ�����ʱ�����¼���
        /// </summary>
        public event AsyncCallback AsyncProcessExited;

        /// <summary>
        /// ���캯����������ùܵ���IsRedirectPipe��������shell�������̣�IsUseShellExecute����
        /// ��������첽����IsSyncInput�����룬����Ҫ�ض���IsRedirectPipe����
        /// </summary>
        /// <param name="exePath">Ĭ��Ϊcmd.exe</param>
        /// <param name="IsUseShellExecute">�Ƿ����shell��������</param>
        /// <param name="IsRedirectPipe">�Ƿ����ùܵ��ض��������������������/����ʹ�����ض���</param>
        /// <param name="IsSyncInput">�Ƿ������첽����</param>
        public ProcessRunner(string exePath = "cmd.exe", bool IsUseShellExecute = false, bool IsRedirectPipe = true, bool IsSyncInput = true)
        {
            //��ʼ������
            this.Process = new Process();              //ʵ������������
            this.Process.EnableRaisingEvents = true;   //�¼������˳� 
            this.StartInfo =  this.Process.StartInfo;        //---�󶨳�ʼ����Ϣ        

            //�¼�
            this.Process.Exited += process_Exited;
            this.Process.Disposed += process_Exited;
            this.Process.OutputDataReceived += process_OutputDataReceived;
            this.Process.ErrorDataReceived += process_ErrorDataReceived;

            SetExePath(exePath);
            SetIsUseShellExecute(IsUseShellExecute);
            SetIsRedirectPipe(IsRedirectPipe);
            SetIsSyncInput(IsSyncInput);

            Init();
        }

        /// <summary>
        ///  ��ʼ�������������ú�������á�
        /// </summary>
        public ProcessRunner Init()
        { 
            //��ʼ��������������Ϣ
            StartInfo.FileName  = ExePath;
            this.StartInfo.UseShellExecute = IsUseShellExecute;     //�Ƿ�ر�Shell��ʹ��

            //��������CMD����������ǲ��ó����������
            this.StartInfo.RedirectStandardInput = IsRedirectPipe;   //�ض����׼����
            this.StartInfo.RedirectStandardOutput = IsRedirectPipe;  //�ض����׼���
            this.StartInfo.RedirectStandardError = IsRedirectPipe;   // ���ⲿ����������д��StandardError���� LV ADDS

            //��ʾ�����Է������
            this.StartInfo.CreateNoWindow = !IsDebug;          //������ʾ����   
    
            return this;
        }

        #region �������Բ�����Smart����

        public string ExePath ;
        public bool IsUseShellExecute = false;
        public bool IsRedirectPipe = true;
        public bool IsSyncInput = true;

        public ProcessRunner SetExePath(string exePath = "cmd.exe") { this.ExePath = exePath; return this; }
        public ProcessRunner SetIsUseShellExecute(bool IsUseShellExecute = false) { this.IsUseShellExecute = IsUseShellExecute; return this; }
        public ProcessRunner SetIsRedirectPipe(bool IsRedirectPipe = false) { this.IsRedirectPipe = IsRedirectPipe; return this; }
        public ProcessRunner SetIsSyncInput(bool IsSyncInput = false) { this.IsSyncInput = IsSyncInput; return this; }       

        #endregion


        #region ���Ľ���
        public ProcessStartInfo StartInfo { get; set; }
        /// <summary>
        /// ��ǰ����
        /// </summary>
        public Process Process { get; set; }
        /// <summary>
        /// ָʾ�����Ƿ��Ѿ����������˶��ѡ�
        /// </summary>
        public bool Started { get; protected set; }
        /// <summary>
        /// �Ƿ��Ѿ��˳�
        /// </summary>
        public bool HasExited { get { return this.Started && this.Process.HasExited; } }
        #endregion

        #region ���������������
        /// <summary>
        /// ������д�������൱��д������̨��
        /// </summary>
        public StreamWriter StreamWriter { get { return Process.StandardInput; } }
        /// <summary>
        /// ��������ȡ�����ӳ���Ŀ���̨��ȡ���ݡ�
        /// </summary>
        public StreamReader StreamReader { get { return Process.StandardOutput; } }
        /// <summary>
        /// �쳣�������
        /// </summary>
        public StreamReader StreamErrorReader { get { return Process.StandardError; } }
        #endregion

        #region �¼�������Ӧ����
        /// <summary>
        /// �������˳���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void process_Exited(object sender, EventArgs e) { if (ExitedOrDisposed != null) ExitedOrDisposed(sender, e); }
        /// <summary>
        /// �����ڲ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void process_OutputDataReceived(object sender, DataReceivedEventArgs e) { if (OutputDataReceived != null) OutputDataReceived(sender, e); }
        protected void process_ErrorDataReceived(object sender, DataReceivedEventArgs e) { if (ErrorDataReceived != null) ErrorDataReceived(sender, e); }

        #endregion

        /// <summary>
        /// ��������,����ʼ�첽��ȡ���
        /// </summary>
        /// <param name="Arguments">��������ʱͬһ������Ĳ���</param>
        public void Start(string Arguments = "")
        {
            this.StartInfo.Arguments = Arguments;    //��������ʱͬһ������Ĳ���

            bool stated = this.Process.Start();     //���� 
            this.Started = true;

            //��ʼ�첽��ȡ���
            if (!this.IsSyncInput && this.StartInfo.RedirectStandardInput) { this.Process.BeginOutputReadLine(); }
            if (!this.IsSyncInput && this.StartInfo.RedirectStandardError) { this.Process.BeginErrorReadLine(); }//����ض���󲻶�ȡ������ܷ�������            
        }

        /// <summary>
        /// ͬ�����С����к���˳���
        /// </summary>
        /// <param name="cmd">��ִ������</param>
        /// <returns></returns>
        public List<string> Run(string cmd)
        {
           List<string> results = new List<string>();
            string result = "�첽�޷���";
            this.Start(cmd);//�����߳� 

            if (this.IsSyncInput)
            {
                this.WriteExist();
                //ƽ�вŲ���
                Task<string> taskresult = Task<string>.Factory.StartNew(GetOutputText);
                Task<string> taskGetError = Task<string>.Factory.StartNew(GetErrorsText);

                results.Add(taskresult.Result);
                results.Add(taskGetError.Result);

                //�ȴ��߳��˳�
                this.Process.WaitForExit();
            }
            else
            {
                results.Add(result);
            }

            return results;
        }
        
        public void RunNoReturn(string cmd)  {  Run(cmd);    }

        /// <summary>
        /// ��ȡ��׼������ı�����Ҫͬ�����ض�����ܶ�ȡ��
        /// </summary>
        /// <returns></returns>
        public string GetOutputText()   {    return  StreamReader.ReadToEnd(); }
        /// <summary>
        /// ��ȡ�����ı�.��Ҫͬ�����ض�����ܶ�ȡ��
        /// </summary>
        /// <returns></returns>
        public string GetErrorsText()   { return StreamErrorReader.ReadToEnd();  }

        #region д������
        /// <summary>
        /// д��һ�����
        /// </summary>
        /// <param name="cmd"></param>
        public void WriteCommand(string cmd)
        {
            if (this.StartInfo.RedirectStandardInput)  { this.StreamWriter.WriteLine(cmd); this.StreamWriter.Flush(); }
            else { throw new Exception("��ȷ���ض��� RedirectStandardInput �������ԣ�����ֻ��ͨ��Shell�������롣"); }
        }
        
        #region ��������
        /// <summary>
        /// ��ܵ�д�� exit ���
        /// </summary>
        public void WriteExist() { WriteCommand("exit"); }
        /// <summary>
        /// ��ܵ�д�� y ���
        /// </summary>
        public void WriteY() { WriteCommand("y"); }
        /// <summary>
        /// ��ܵ�д�� n ���
        /// </summary>
        public void WriteN() { WriteCommand("n"); }
        /// <summary>
        /// ��ܵ�д�� shutdown ���
        /// </summary>
        public void WriteShutdown() { WriteCommand("shutdown"); }
        /// <summary>
        /// ��ܵ�д�� start ���
        /// </summary>
        public void WriteStart() { WriteCommand("start"); }
        #endregion
        #endregion

        #region �첽����

        /// <summary>
        /// �첽���У����õȴ�����Ҫ���ǣ����������̡�
        /// </summary>
        /// <param name="cmd">����</param>
        /// <returns></returns>
        public IAsyncResult RunAsyn(string cmd)
        {
            CmdHandler handler = RunNoReturn;
            IAsyncResult result = handler.BeginInvoke(cmd, AsyncProcessExited, null );
            return result;
        }

        #endregion

        /// <summary>
        /// ֱ��ɱ�����̡�
        /// </summary>
        public void KillProcess() { 
            if (!this.Process.HasExited) 
            { this.Process.Kill(); }  
        }

        /// <summary>
        /// ֱ���ͷŽ�����Դ
        /// </summary>
        public void Dispose() { if (Process != null) { KillProcess(); Process.Dispose(); } }

        #region ������
        /// <summary>
        /// ͬ�����������С�
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string RunCmd(string cmd)
        {
            using (ProcessRunner c = new ProcessRunner())
            {
                return c.Run(cmd)[0];
            }
        }
        /// <summary>
        /// �첽���������С�
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCmdAsyn(string cmd)
        {
            new ProcessRunner().RunAsyn(cmd);
        }
        #endregion
    }
}
