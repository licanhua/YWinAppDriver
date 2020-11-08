// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra
{
  public interface ISessionManager
  {
    ISession NewSession();
    void AddSession(ISession session);
    void DeleteSession(string sessionId);
    ISession GetSession(string sessionId);
    bool ContainsSession(string sessionId);
    List<ISession> GetSessions();
  }
}
