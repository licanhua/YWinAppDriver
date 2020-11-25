using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinAppDriver.Infra.Helper
{
  public class Helpers
  {
    public const int PROCESS_PER_MONITOR_DPI_AWARE = 0x000000002;

    [DllImport("shcore.dll", CharSet = CharSet.Unicode)]
    public static extern uint SetProcessDpiAwareness(int dpiAwareness);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool BringWindowToTop(IntPtr hWnd);
  }
}
