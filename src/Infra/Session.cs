using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra
{
  public class Session : ISession
  {
    private string _sessionId = Guid.NewGuid().ToString();
    public void ExitApplication()
    {
    }

    public string GetSessionId()
    {
      return _sessionId;
    }

    public void LaunchApplication()
    {
    }
  }
}
