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
      { Command.SetImplicitTimeout, new SetImplicitTimeoutHandler() },
      { Command.GetStatus, new GetStatusHandler() },
      { Command.GetWindowHandle, new GetWindowHandleHandler() },
      { Command.GetWindowHandles, new GetWindowHandlesHandler() },
      { Command.UnknownCommand, new UknownCommandHandler() },
      { Command.ActivateWindow, new ActivateWindowHandler() },
      { Command.DeleteWindow, new CloseActiveWindowHandler() },
      { Command.GetTitle, new GetTitleHandler() },
      { Command.ElementEquals, new ElementEqualsHandler() },
      { Command.GetFocusedElement, new GetFocusedElementHandler() },
      { Command.ElementSendKeys, new SetValueHandler() },
      { Command.AppiumCloseApp, new AppiumCloseAppHandler() },
      { Command.AppiumLaunchApp, new AppiumLaunchAppHandler() },
      { Command.SessionSendKeys, new SessionSendKeysHandler() },
      { Command.GetCapabilities, new GetCapabilitesHandler() },
      { Command.SessionMoveTo, new SessionMoveToHandler() },
      { Command.SessionMouseAction, new SessionMouseActionHandler() },
      { Command.SessionTouchActionOnElement, new SessionTouchActionOnElementHandler() },
      { Command.SessionTouchUpDownMove, new SessionTouchUpDownMoveHandler() },
      { Command.MaximizeWindow, new MaximizeWindowHandler() },
      { Command.GetWindowPosition, new GetWindowPositionHandler() },
      { Command.SetWindowPosition, new SetWindowPositionHandler() },
      { Command.GetWindowSize, new GetWindowSizeHandler() },
      { Command.SetWindowSize, new SetWindowSizeHandler() },
      { Command.TakeScreenshot, new TakeScreenshotHandler() },
      { Command.ClickOnElement, new ClickHandler() },
      { Command.DevicePushFile, new DevicePushFileHandler() },
      { Command.DevicePullFile, new DevicePullFileHandler() }
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
