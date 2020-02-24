using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
     

namespace Geo.Winform
{

        /// <summary> 
        /// Mover 的摘要说明。 
        /// </summary> 
        public class SizableControl : System.Windows.Forms.Control
        {
            private const int WM_NCHITTEST = 0x84;
            private const int WM_NCLBUTTONDOWN = 0xa1;

            private const int HTCLIENT = 1;
            private const int HTBOTTOMRIGHT = 17;

            private System.Drawing.Drawing2D.GraphicsPath m_Path;

            [System.Runtime.InteropServices.DllImport("user32.dll ")]
            public static extern int SendMessage(IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam
            );

            public SizableControl()
            {
               // InitializeComponent();

                this.m_Path = new System.Drawing.Drawing2D.GraphicsPath();

                this.m_Path.StartFigure();
                this.m_Path.AddLines(new Point[] { new Point(this.Width, 0), new Point(this.Width - this.Height, this.Height), new Point(this.Width, this.Height) });
                this.m_Path.CloseFigure();

            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                //ControlPaint.DrawSizeGrip(this.CreateGraphics(), System.Drawing.Color.White, this.Width - this.Height, 0, this.Height, this.Height); 

            }

            protected override void OnSizeChanged(EventArgs e)
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.StartFigure();
                path.AddLines(new Point[] { new Point(this.Width, 0), new Point(this.Width - this.Height, this.Height), new Point(this.Width, this.Height) });
                path.CloseFigure();

                Region reReg = new Region(path);
                reReg.Complement(m_Path);

                this.Invalidate(reReg);

                base.OnSizeChanged(e);
                this.m_Path = path;

                //ControlPaint.DrawSizeGrip(this.CreateGraphics(), System.Drawing.Color.FromArgb(0, 30, 88), this.Width - this.Height, 0, this.Height, this.Height); 
            }

            protected override void WndProc(ref Message m)
            {
                if (this.DesignMode)
                {
                    base.WndProc(ref m);
                    return;
                }

                switch (m.Msg)
                {
                    case WM_NCHITTEST:
                        SendMessage(this.Parent.Handle, m.Msg, m.WParam, m.LParam);
                        base.WndProc(ref m);
                        if ((int)m.Result == HTCLIENT)
                        {
                            m.Result = (IntPtr)HTBOTTOMRIGHT;//右下 
                        }
                        break;
                    case WM_NCLBUTTONDOWN:
                        SendMessage(this.Parent.Handle, m.Msg, m.WParam, m.LParam);
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
        } 


}
