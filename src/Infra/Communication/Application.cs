// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;

namespace WinAppDriver.Infra.Communication
{
  public class Application : IApplication
  {
    private int _processId;
    private IElement _appRoot;
    private bool _allowQuit;
    public Application(IElement appRoot, int processId, bool allowQuit = true)
    {
      _processId = processId;
      _appRoot = appRoot;
      _allowQuit = allowQuit;
    }
    public IElement GetApplicationRoot()
    {
      return _appRoot;
    }

    public int GetProcessId()
    {
      return _processId;
    }

    public void QuitApplication()
    {
      if (_appRoot != null && _processId > 0 && _allowQuit)
      {
        try
        {
          _appRoot.CloseWindow();

          var process = Process.GetProcessById(_processId);
          if (process != null)
          {
            process.Kill();
          }
        }
        catch { }

        _appRoot = null;
        _processId = 0;
      }
    }
  }
}
