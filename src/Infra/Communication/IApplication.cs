// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;

namespace WinAppDriver.Infra.Communication
{
  public interface IApplication
  {
    public int GetProcessId();
    public IElement GetApplicationRoot();
    public void QuitApplication();
  }

  public interface IApplicationManager
  {
    public IApplication LaunchApplication(Capabilities capabilities);
  }
}
