//2015.04.14, czs, edit in namu �� ����ע��

using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// ���湤��
    /// </summary>
    public class UiUtil
    {
        #region ��ӭ����
        /// <summary>
        /// ϵͳ����
        /// </summary>
        public static string SystemName = "��Ϣϵͳ"; 
        /// <summary>
        /// ��ӭ����
        /// </summary>
        public static void ShowSplash(string systemName = "��Ϣϵͳ")
        {
            UiUtil.SystemName = systemName;
            //splash
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(MyStartingMethod));
            myThread.Start();
        }
        /// <summary>
        /// �ڵ�ǰ�߳�������������
        /// </summary>
        public static void MyStartingMethod()
        {
            SplashForm splashForm = new SplashForm();
            splashForm.Title = SystemName;
            splashForm.ShowDialog();
        }
        /// <summary>
        /// ��ʾ�ȴ����ݿ�����
        /// </summary>
        public static void ShowWaittingForConnectionDbForm()
        {
            //splash
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowWaittingForConnectionDb));
            myThread.Start();
        }
        /// <summary>
        /// ��ʾ�����������ݿ⣬��ȴ�
        /// </summary>
        public static void ShowWaittingForConnectionDb()
        {
           Geo.Utils. WaitingForm splashForm = new WaitingForm("���Ժ������������ݿ�.....");
            splashForm.ShowDialog();
        } 

        #endregion 
    }
}
