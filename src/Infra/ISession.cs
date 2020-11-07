using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra
{
  public interface ISession
  {
    string GetSessionId();
    void LaunchApplication();
    void ExitApplication();
  }
}
