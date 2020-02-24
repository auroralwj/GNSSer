using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace Geo.WinTools.Images
{
    /// <summary>
    /// 封装一些API
    /// </summary>
    public class API
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetDesktopWindow();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hookid"></param>
        /// <param name="pfnhook"></param>
        /// <param name="hinst"></param>
        /// <param name="threadid"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int hookid, HookProc pfnhook, IntPtr hinst, int threadid);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hhook"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhook);

        /// <summary>
        /// 变量
        /// </summary>
        public enum WindowsHookCodes
        {
            /// <summary>
            /// 1
            /// </summary>
            WH_MSGFILTER = (-1),
            /// <summary>
            /// 
            /// </summary>
            WH_JOURNALRECORD = 0,
            /// <summary>
            /// 
            /// </summary>
            WH_JOURNALPLAYBACK = 1,
            /// <summary>
            /// 2
            /// </summary>
            WH_KEYBOARD = 2,
            /// <summary>
            /// 3
            /// </summary>
            WH_GETMESSAGE = 3,
            /// <summary>
            /// 4
            /// </summary>
            WH_CALLWNDPROC = 4,
            /// <summary>
            /// 5
            /// </summary>
            WH_CBT = 5,
            /// <summary>
            /// 6
            /// </summary>
            WH_SYSMSGFILTER = 6,
            /// <summary>
            /// 7
            /// </summary>
            WH_MOUSE = 7,
            /// <summary>
            /// 8
            /// </summary>
            WH_HARDWARE = 8,
            /// <summary>
            /// 9
            /// </summary>
            WH_DEBUG = 9,
            /// <summary>
            /// 10
            /// </summary>
            WH_SHELL = 10,
            /// <summary>
            /// 11
            /// </summary>
            WH_FOREGROUNDIDLE = 11,
            /// <summary>
            /// 12
            /// </summary>
            WH_CALLWNDPROCRET = 12,
            /// <summary>
            /// 13
            /// </summary>
            WH_KEYBOARD_LL = 13,
            /// <summary>
            /// 14
            /// </summary>
            WH_MOUSE_LL = 14
        }

    }

    /// <summary>
    /// 一个根据矩形截图类
    /// zgke@Sina.com
    /// qq:116149
    /// </summary>
    public class CopyScreen
    {
        /// <summary>
        /// 屏幕大小
        /// </summary>
        private Size ScreenSize { get { return Screen.PrimaryScreen.Bounds.Size; } }
        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Point MousePoint { get { return Cursor.Position; } }
        /// <summary>
        /// 私有方法获取屏幕图形(全部图形)
        /// </summary>
        public Bitmap ScreenImage
        {
            get
            {
                Bitmap m_BackBitmap = new Bitmap(ScreenSize.Width, ScreenSize.Height);
                Graphics _Graphics = Graphics.FromImage(m_BackBitmap);
                _Graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), ScreenSize);
                _Graphics.Dispose();
                return m_BackBitmap;
            }
        }
        /// <summary>
        /// 钩子
        /// </summary>
        private HookMessage m_HookMessage;
        /// <summary>
        /// 屏幕句柄
        /// </summary>
        private IntPtr m_ScreenForm;
        /// <summary>
        /// 图形
        /// </summary>
        private Bitmap m_Image;

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="p_Image"></param>
        public delegate void GetImage(Image p_Image);
        /// <summary>
        /// 获取屏幕截图
        /// </summary>
        public event GetImage GetScreenImage;


        /// <summary>
        /// 构造
        /// </summary>
        public CopyScreen()
        {
            m_ScreenForm = API.GetDesktopWindow();
            m_HookMessage = new HookMessage(API.WindowsHookCodes.WH_MOUSE_LL, true);
            m_HookMessage.GetHook += new HookMessage.GetHookMessage(m_HookMessage_GetHook);
        }

        /// <summary>
        /// 钩子事件
        /// </summary>
        /// <param name="p_Code"></param>
        /// <param name="p_wParam"></param>
        /// <param name="p_lParam"></param>
        /// <param name="p_Send"></param>
        void m_HookMessage_GetHook(int p_Code, IntPtr p_wParam, IntPtr p_lParam, ref bool p_Send)
        {
            if (m_StarMouse)
            {
                switch (p_wParam.ToInt32())
                {
                    case 512: //Move
                        MouseMove();
                        break;
                    case 513:  //Down
                        MouseDown();
                        p_Send = false;
                        break;
                    case 514:  //Up
                        MouseUp();
                        p_Send = false;
                        break;
                    default:
                        m_StarMouse = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 根据矩形 如果Width是正直接返回 如果使-会转换成正的矩形 保证大小位置不变
        /// </summary>
        /// <param name="p_Rectangle">矩形</param>
        /// <returns>正矩形</returns>
        public static Rectangle GetUprightRectangle(Rectangle p_Rectangle)
        {
            Rectangle _Rect = p_Rectangle;
            if (_Rect.Width < 0)
            {
                int _X = _Rect.X;
                _Rect.X = _Rect.Width + _Rect.X;
                _Rect.Width = _X - _Rect.X;
            }
            if (_Rect.Height < 0)
            {
                int _Y = _Rect.Y;
                _Rect.Y = _Rect.Height + _Rect.Y;
                _Rect.Height = _Y - _Rect.Y;
            }
            return _Rect;
        }

        private Rectangle m_MouseRectangle = new Rectangle(0, 0, 0, 0);
        private bool m_DrawStar = false;
        private void MouseDown()
        {
            m_MouseRectangle.X = MousePoint.X;
            m_MouseRectangle.Y = MousePoint.Y;
            m_DrawStar = true;
        }
        private void MouseMove()
        {
            if (m_DrawStar)
            {
                ControlPaint.DrawReversibleFrame(m_MouseRectangle, Color.Transparent, FrameStyle.Dashed);
                m_MouseRectangle.Width = MousePoint.X - m_MouseRectangle.X;
                m_MouseRectangle.Height = MousePoint.Y - m_MouseRectangle.Y;
                ControlPaint.DrawReversibleFrame(m_MouseRectangle, Color.White, FrameStyle.Dashed);

            }
        }
        private void MouseUp()
        {
            ControlPaint.DrawReversibleFrame(m_MouseRectangle, Color.Transparent, FrameStyle.Dashed);
            m_DrawStar = false;
            m_StarMouse = false;
            Rectangle _ScreenRectangle = GetUprightRectangle(m_MouseRectangle);
            m_MouseRectangle.X = 0;
            m_MouseRectangle.Y = 0;
            m_MouseRectangle.Width = 0;
            m_MouseRectangle.Height = 0;
            if (GetScreenImage != null)
            {
                if (_ScreenRectangle.Width != 0 && _ScreenRectangle.Height != 0) GetScreenImage(m_Image.Clone(_ScreenRectangle, m_Image.PixelFormat));
            }
        }
        private bool m_StarMouse = false;

        /// <summary>
        /// 获取图形
        /// </summary>
        public void GerScreenFormRectangle()
        {
            m_Image = ScreenImage;
            m_StarMouse = true;
        }

        /// <summary>
        /// 获取图形
        /// </summary>
        public void GetScreen()
        {
            if (GetScreenImage != null) GetScreenImage(ScreenImage);
        }

    }


    /// <summary>
    /// 用钩子获取消息
    /// zgke@Sina.com
    /// </summary>
    public class HookMessage
    {
        private IntPtr m_HookEx;
        /// <summary>
        /// 设置自己进程的钩子
        /// </summary>
        /// <param name="p_HookCodes">钩子类型</param>
        public HookMessage(API.WindowsHookCodes p_HookCodes)
        {
            m_HookEx = API.SetWindowsHookEx((int)p_HookCodes, new API.HookProc(SetHookProc), IntPtr.Zero, API.GetCurrentThreadId());
        }
        /// <summary>
        /// 设置进程的钩子
        /// </summary>
        /// <param name="p_HookCodes">钩子类型</param>
        /// <param name="p_Zero">全局钩子</param>
        public HookMessage(API.WindowsHookCodes p_HookCodes, bool p_Zero)
        {
            IntPtr _Value = System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]);
            m_HookEx = API.SetWindowsHookEx((int)p_HookCodes, new API.HookProc(SetHookProc), _Value, 0);
        }
        /// <summary>
        /// 关闭钩子
        /// </summary>
        public void UnHookMessage()
        {
            if (API.UnhookWindowsHookEx(m_HookEx))
            {
                m_HookEx = IntPtr.Zero;
            }
        }
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="p_Code"></param>
        /// <param name="p_wParam"></param>
        /// <param name="p_lParam"></param>
        /// <param name="p_Send"></param>
        public delegate void GetHookMessage(int p_Code, IntPtr p_wParam, IntPtr p_lParam, ref bool p_Send);
        /// <summary>
        /// 钩子
        /// </summary>
        public event GetHookMessage GetHook;

        private IntPtr SetHookProc(int p_Code, IntPtr p_wParam, IntPtr p_lParam)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            bool _SendMessage = true;
            if (GetHook != null) GetHook(p_Code, p_wParam, p_lParam, ref _SendMessage);
            if (!_SendMessage) return new IntPtr(1);
            return IntPtr.Zero;
        }
    }
}