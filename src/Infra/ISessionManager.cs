using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra
{
  public interface ISessionManager
  {
    string CreateSession(object req);
    void DeleteSession(string sessionId);
    ISession GetSession(string sessionId);
  }
}
