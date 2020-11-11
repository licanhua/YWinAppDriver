// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System;
using WinAppDriver.Infra.Communication;

namespace WinAppDriver.Infra.CommandHandler
{
  public interface ICommandHandler
  {
    object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId);
  }

  // Handler doesn't need to process the body
  public interface IElementCommandHandler
  {
    object ExecuteCommand(ISessionManager sessionManager, string sessionId, string elementId, Func<IElement, object> func);
  }
}
