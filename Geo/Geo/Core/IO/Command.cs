 //2014.05.21,czs, add logs
//2016.11.23, czs & cuiyang,  ����ֻ�������֧�� TEQC ���ָ�֮

using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Diagnostics;//���룬ʹ�ý����࣬������������

namespace Geo.Common
{
    /// <summary>
    /// CMD�����ࡣ
    /// </summary>
    public class Command : IDisposable
    {
        delegate string CmdHandler(string cmd);
        /// <summary>
        /// �����˳��¼�
        /// </summary>
        public event EventHandler ProcessExited;
        /// <summary>
        /// ����
        /// </summary>
        protected Process Process { get { return process; } }

        private  Process process;
       
        /// <summary>
        /// ���캯����
        /// </summary>
        public Command()
        {
            process = new Process();//������������
            process.StartInfo.FileName = "cmd.exe";           //�趨������

            process.StartInfo.UseShellExecute = false;        //�ر�Shell��ʹ��
            process.StartInfo.RedirectStandardInput = true;   //�ض����׼����
            process.StartInfo.RedirectStandardOutput = true;  //�ض����׼���
            process.StartInfo.RedirectStandardError = true;   //�ض���������
            process.StartInfo.CreateNoWindow = true;          //������ʾ����

            process.Exited += process_Exited;
        } 

        /// <summary>
        /// �������˳���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void process_Exited(object sender, EventArgs e)
        {
            if (ProcessExited != null) ProcessExited(sender, e);
        }

        /// <summary>
        /// ͬ������
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string Run(string cmd)
        {
            string result;
            try
            {
                //p.StartInfo.Arguments = "/c " + cmd;    //�O����ʽ���Ѕ���
                process.Start();   //����  sw.Close(); 
                StreamWriter sw = process.StandardInput;
                sw.WriteLine(cmd);       //Ҳ�������@�N��ʽݔ��Ҫ���е�����
                sw.WriteLine("exit");        //���^Ҫӛ�ü���ExitҪ��Ȼ��һ�г�ʽ���еĕr������C
                sw.Close();

                result = process.StandardOutput.ReadToEnd();        //��ݔ����ȡ��������нY��

                process.WaitForExit();

                //throw new InvocationException() ;
            }
            catch (Exception ex)
            {
                result = "ERROR:" + ex.Message;

               // ExceptionHandlerFactory.Create(typeof(InvocationException))
                //    .Handle(new InvocationException(ex) { InvocationPath = cmd });
                //
                //�����׳�
                throw;
            }
            finally
            {
                if (process.HasExited)
                    process.Close();
            }
            return result;
        }


        /// <summary>
        /// �첽���У����õȴ���
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public void RunAsyn(string cmd)
        {
            CmdHandler handler = Run;
            handler.BeginInvoke(cmd,
                null,
                null);
        }
        /// <summary>
        /// ͬ�����������С�
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string RunCmd(string cmd)
        {
            using (Command c = new Command())
            {
                return c.Run(cmd);
            }
        }
        /// <summary>
        /// �첽���������С�
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCmdAsyn(string cmd)
        {
            new Command().RunAsyn(cmd);
        }

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose()
        {
           // Dispose(true);

            if (process != null)
                process.Dispose();
        }
    }
}
