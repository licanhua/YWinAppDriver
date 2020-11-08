using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

namespace WinAppDriver.Infra.CommandHandler
{
  public class GetSessionsCommandHandler : ICommandHandler
  {
    private class Item {
      public string sessionId;
      public Capabilities capabilities;
    }
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      var result = new List<Item>();
      foreach (var session in sessionManager.GetSessions())
      {
        result.Add(new Item() { sessionId = session.GetSessionId(), capabilities = session.GetCapabilities() });
      }
      return result;
    }
  }
}
