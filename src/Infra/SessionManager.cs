namespace WinAppDriver.Infra
{
  using System;
  using System.Collections.Generic;

  public class SessionManager : ISessionManager
  {
    private Dictionary<string, ISession> _sessions;

    private Func<ISession> _createSessionFunc;

    public SessionManager(Func<ISession> creatSessionFunc)
    {
      _createSessionFunc = creatSessionFunc;
      _sessions = new Dictionary<string, ISession>();
    }

    public string CreateSession(object req)
    {
      var session = _createSessionFunc();
      if (session == null)
      {
        throw new SessionNotCreated("Fail to create session with parameter: " + req.ToString());
      }

      session.LaunchApplication();

      var sessionId = session.GetSessionId();
      _sessions[sessionId] = session;
      return sessionId;
    }

    public void DeleteSession(string sessionId)
    {
      if (_sessions.ContainsKey(sessionId))
      {
        var session = _sessions[sessionId];
        _sessions.Remove(sessionId);
        session.ExitApplication();
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
        throw new SessionNotFound("session id: " + sessionId);
      }
    }
  }
}
