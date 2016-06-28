using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Cortex.InterOp
{
    public static class User32

    {
        private const uint WmGeticon = 0x007f;
        private static readonly IntPtr IconSmall2 = new IntPtr(2);
        private static IntPtr _idiApplication = new IntPtr(0x7F00);
        private const int GclHicon = -14;

        public static IntPtr IdiApplication
        {
            get { return _idiApplication; }
            set { _idiApplication = value; }
        }

        /// <summary>
        /// 64 bit version maybe loses significant 64-bit specific information
        /// </summary>
        private static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return new IntPtr(NativeMethods.GetClassLong32(hWnd, nIndex));
            }
            else
            {
                return NativeMethods.GetClassLong64(hWnd, nIndex);
            }
                
        }


        

        public static Image GetSmallWindowIcon(IntPtr hWnd)
        {
            try
            {
                var hIcon = NativeMethods.SendMessage(hWnd, WmGeticon, IconSmall2, IntPtr.Zero);

                if (hIcon == IntPtr.Zero)
                    hIcon = GetClassLongPtr(hWnd, GclHicon);

                if (hIcon == IntPtr.Zero)
                    hIcon = NativeMethods.LoadIcon(IntPtr.Zero, (IntPtr)0x7F00/*IDI_APPLICATION*/);

                return hIcon != IntPtr.Zero ? new Bitmap(Icon.FromHandle(hIcon).ToBitmap(), 16, 16) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static class NativeMethods
        {
            // See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx
            public const int WmClipboardupdate = 0x031D;
            public static IntPtr HwndMessage = new IntPtr(-3);

            // See http://msdn.microsoft.com/en-us/library/ms632599%28VS.85%29.aspx#message_only
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AddClipboardFormatListener(IntPtr hwnd);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int SetForegroundWindow(int hwnd);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int GetForegroundWindow();

            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

            [DllImport("user32.dll", EntryPoint = "GetClassLong")]
            public static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
            public static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

        }
    }
}
