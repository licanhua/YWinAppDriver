// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra.CommandHandler
{
  public class CommandHandlers: ICommandHandlers
  {
    private Dictionary<Command, ICommandHandler> _handlers = new Dictionary<Command, ICommandHandler>() {
      { Command.NewSession, new NewSessionHandler() },
      { Command.DeleteSession, new DeleteSessionHandler() },
      { Command.GetSessions, new GetSessionsCommandHandler() },
      { Command.FindElement, new FindElementHandler() },
      { Command.FindElements, new FindElementsHandler() },
      { Command.GetSource, new GetSourceHandler() },
      { Command.ImplicitTimeout, new ImplicitTimeoutHandler() },
    };

    public object ExecuteCommand(Command command, ISessionManager sessionManager, string sessionId, object req, string elementId)
    {
      if (command == Command.UnknownCommand || !_handlers.ContainsKey(command)) {
        throw new SessionNotCreatedException();
      }
      return _handlers[command].ExecuteCommand(sessionManager, sessionId, req, elementId);
    }
  }
}
