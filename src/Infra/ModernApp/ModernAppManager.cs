using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Windows.Apps.Test.Foundation;
using Microsoft.Windows.Apps.Test.Foundation.Waiters;

/**
 * Adapted from https://github.com/BrianLima/UWPHook/blob/master/UWPHook/AppManager.cs
 */
namespace WinAppDriver.Infra.ModernApp
{
  static class ModernAppManager
  {
    static int Activate(string appName, string arguments)
    {
      var mgr = new ApplicationActivationManager();

      try
      {
        mgr.ActivateApplication(appName, arguments, ActivateOptions.None, out var processId);

        BringProcess(processId);

        return processId;
      }
      catch (Exception e)
      {
        throw new Exception("Error while trying to launch your app." + Environment.NewLine + e.Message, e);
      }
    }

    public static UIObject Launch(
      string appName,
      string arguments,
      UICondition topLevelWindowCondition)
    {
      using AppLaunchWaiter appLaunchWaiter = new AppLaunchWaiter(topLevelWindowCondition);
      Activate(appName, arguments);
      appLaunchWaiter.Wait();

      return appLaunchWaiter.Source;
    }

    [DllImport("user32.dll")]
    private static extern
      bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern
      bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern
      bool IsIconic(IntPtr hWnd);

    public static void BringProcess(int processId)
    {
      /*
      const int SW_HIDE = 0;
      const int SW_SHOWNORMAL = 1;
      const int SW_SHOWMINIMIZED = 2;
      const int SW_SHOWMAXIMIZED = 3;
      const int SW_SHOWNOACTIVATE = 4;
      const int SW_RESTORE = 9;
      const int SW_SHOWDEFAULT = 10;
      */

      var me = Process.GetCurrentProcess();
      using var arrProcesses = Process.GetProcessById(processId);

      // get the window handle
      IntPtr hWnd = arrProcesses.MainWindowHandle;

      // if iconic, we need to restore the window
      if (IsIconic(hWnd))
      {
        ShowWindowAsync(hWnd, 3);
      }

      // bring it to the foreground
      SetForegroundWindow(hWnd);
    }
  }
}