using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Cortex.Hotkey
{

    public class HotkeyBinding : IDisposable
    {
        private HwndSource _source;
        private const int HotkeyId = 9000;

        private Action _hotkeyHandlerDelegate;
        private readonly Window _window;

        public Action HotkeyAction
        {
            get
            {
                if (_hotkeyHandlerDelegate != null)
                {
                    return _hotkeyHandlerDelegate;
                }
                return delegate
                {
                    Debug.WriteLine("Default Action");
                };
            }

            set { _hotkeyHandlerDelegate = value; }
        }

        public HotkeyBinding(Window window, uint key, uint modifier)
        {
            _window = window;
            var helper = new WindowInteropHelper(window);
            _source = HwndSource.FromHwnd(helper.Handle);
            if (_source != null)
            {
                _source.AddHook(HwndHook);
                _RegisterHotKey(key, modifier);
            }


        }

        private void _RegisterHotKey(uint key, uint modifier)
        {
            var helper = new WindowInteropHelper(_window);

            if (!NativeMethods.RegisterHotKey(helper.Handle, HotkeyId, modifier, key))
            {
                // handle error
            }
        }

        private void _UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(_window);
            NativeMethods.UnregisterHotKey(helper.Handle, HotkeyId);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Hotkey Constant: https://msdn.microsoft.com/en-us/library/windows/desktop/ms646279(v=vs.85).aspx
            const int wmHotkey = 0x0312;
            switch (msg)
            {
                case wmHotkey:
                    switch (wParam.ToInt32())
                    {
                        case HotkeyId:
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnHotKeyPressed()
        {
            if (HotkeyAction != null)
            {
                HotkeyAction.DynamicInvoke();
            }
            else
            {
                throw new NullReferenceException("Hotkey Handler Delegate is not set");
            }

        }

        public void Dispose()
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            _UnregisterHotKey();
        }

        internal static class NativeMethods
        {
            [DllImport("User32.dll")]
            public static extern bool RegisterHotKey(
                [In] IntPtr hWnd,
                [In] int id,
                [In] uint fsModifiers,
                [In] uint vk);

            [DllImport("User32.dll")]
            public static extern bool UnregisterHotKey(
                [In] IntPtr hWnd,
                [In] int id);
        }
    }
}
