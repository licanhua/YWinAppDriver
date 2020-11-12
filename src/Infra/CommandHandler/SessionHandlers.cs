// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using WinAppDriver.Infra.Result;

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
  class StatusHandler : NoSessionCommandHandlerBase<object, StatusResult>
  {
    protected override object DeserializeType(object obj)
    {
      return null;
    }
    protected override StatusResult ExecuteNoSessionCommand(ISessionManager sessionManager, object req, string elementId)
    {

      Assembly execAssembly = Assembly.GetExecutingAssembly();
      AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();

      var osName = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? "Windows" : "Not Windows";
      return new StatusResult()
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
