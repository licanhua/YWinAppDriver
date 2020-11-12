// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra.CommandHandler
{
  public class GetSessionsCommandHandler: ICommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      var result = new List<GetSessionsResultItem>();
      foreach (var session in sessionManager.GetSessions())
      {
        result.Add(new GetSessionsResultItem() { sessionId = session.GetSessionId(), capabilities = session.GetCapabilities() });
      }
      return result;
    }
  }
  class NewSessionHandler : NoSessionCommandHandlerBase<NewSessionReq, NewSessionIntermediateResult>
  {
    protected override NewSessionIntermediateResult ExecuteNoSessionCommand(ISessionManager sessionManager, NewSessionReq req, string elementId)
    {
      try
      {
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
  class GetStatusHandler : ICommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {

      Assembly execAssembly = Assembly.GetExecutingAssembly();
      AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();

      var osName = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? "Windows" : "Not Windows";
      return new GetStatusResult()
      {
        buildVersion = assemblyName.Version.ToString(),
        buildTime = (new FileInfo(execAssembly.Location).CreationTime).ToString(),
        buildRevision = assemblyName.Version.Revision.ToString(),
        osArch = RuntimeInformation.OSArchitecture.ToString(),
        osName = osName,
        osVersion = RuntimeInformation.OSDescription
      };
    }
  }

}
