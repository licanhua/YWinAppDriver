// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WinAppDriver.Infra.Communication;
using WinAppDriver.Infra.Utils;

namespace WinAppDriver.Infra.CommandHandler
{
  public abstract class CommandHandlerBase<ReqType, ResultType>
  {
    protected virtual ReqType DeserializeType(object obj)
    {
      try
      {
        var req = JsonHelper.Deserialize<ReqType>(obj);

        // Serialize to valid the fields
        JsonHelper.Serialize(req);
        return req;
      }
      catch (JsonSerializationException ex)
      {
        throw new InvalidArgumentException(ex.GetType().Name + ": " + ex.Message);
      }
    }

  }

  public abstract class SessionCommandHandlerBase<ReqType, ResultType> : CommandHandlerBase<ReqType, ResultType>, ICommandHandler
  {
    protected abstract ResultType ExecuteSessionCommand(ISessionManager sessionManager, ISession session, ReqType req, string elementId);

    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      var session = sessionManager.GetSession(sessionId);
      ReqType req = DeserializeType(body);
      return ExecuteSessionCommand(sessionManager, session, req, elementId);
    }
  }

  public abstract class SessionCommandHandlerBase<ResultType> : CommandHandlerBase<object, ResultType>, ICommandHandler
  {
    protected abstract ResultType ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId);

    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      var session = sessionManager.GetSession(sessionId);
      return ExecuteSessionCommand(sessionManager, session, elementId);
    }
  }

  public abstract class NoSessionCommandHandlerBase<ReqType, ResultType> : CommandHandlerBase<ReqType, ResultType>, ICommandHandler
  {
    protected abstract ResultType ExecuteNoSessionCommand(ISessionManager sessionManager, ReqType req, string elementId);

    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      ReqType req = DeserializeType(body);
      return ExecuteNoSessionCommand(sessionManager, req, elementId);
    }
  }

  public class ElementCommandHandler: IElementCommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, string elementId, Func<IElement, object> func)
    {
      var element = sessionManager.GetSession(sessionId).FindElement(elementId);
      return func(element);
    }
  }

}
