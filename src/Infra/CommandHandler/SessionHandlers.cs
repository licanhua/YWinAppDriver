using System;
using System.Collections.Generic;
using System.Text;
using WinAppDriver.Infra.Result;
// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

namespace WinAppDriver.Infra.CommandHandler
{
  class ImplicitTimeoutHandler : SessionCommandHandlerBase<ImplicitTimeoutReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, ImplicitTimeoutReq req, string elementId)
    {
      if (req.ms < 0)
      {
        throw new InvalidArgumentException("ms shoudl not be negative");
      }
      session.SetImplicitTimeout((int)req.ms);
      return null;
    }
  }
  class NewSessionHandler : CommandHandlerBase<NewSessionReq, NewSessionIntermediateResult>, ICommandHandler
  {
#pragma warning disable IDE0060 // Remove unused parameter
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
#pragma warning restore IDE0060 // Remove unused parameter
    {
      try
      {
        NewSessionReq req = DeserializeType(body);

        var session = sessionManager.NewSession();
        session.LaunchApplication(req);
        sessionManager.AddSession(session);

        return new NewSessionIntermediateResult()
        {
          sessionId = session.GetSessionId(),
          capabilities = req.desiredCapabilities
        };
      }
      catch (SessionException ex) 
      {
        throw new SessionNotCreatedException(ex.ErrorMessage);
      }
    }
  }
}
