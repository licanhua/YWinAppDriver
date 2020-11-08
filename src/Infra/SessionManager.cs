// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

namespace WinAppDriver.Infra
{
  using System;
  using System.Collections.Generic;
  using WinAppDriver.Infra.Communication;
  using WinAppDriver.Infra.Result;

  public class SessionManager : ISessionManager
  {
    public const int MaxSessionSupported = 100;
    private Dictionary<string, ISession> _sessions = new Dictionary<string, ISession>();
    private Func<ISession> _sessionCreator;
    public SessionManager(Func<ISession> sessionCreator) 
    {
      _sessionCreator = sessionCreator;
    }
    public ISession NewSession()
    {
      return _sessionCreator();
    }
    public void AddSession(ISession session)
    {
      if (_sessions.Count >= MaxSessionSupported)
      {
        throw new SessionNotCreatedException("Max session reached");
      }

      var sessionId = session.GetSessionId();
      _sessions[sessionId] = session;
    }

    public void DeleteSession(string sessionId)
    {
      if (_sessions.ContainsKey(sessionId))
      {
        var session = _sessions[sessionId];
        _sessions.Remove(sessionId);
      }
    }

    public ISession GetSession(string sessionId)
    {
      if (_sessions.ContainsKey(sessionId))
      {
        return _sessions[sessionId];
      }
      else
      {
        throw new InvalidSessionIdException("session id: " + sessionId);
      }
    }

    public List<ISession> GetSessions()
    {
      List<ISession> sessions = new List<ISession>(_sessions.Values);
      return sessions;
    }

    public bool ContainsSession(string sessionId)
    {
      if (String.IsNullOrEmpty(sessionId)) return false;
      return _sessions.ContainsKey(sessionId);
    }
  }
}
