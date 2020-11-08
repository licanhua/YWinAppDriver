// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using WinAppDriver.Infra;
using WinAppDriver.Infra.Communication;
using Xunit;

namespace Infra.UnitTest
{
  public class SessionManagerTest
  {
    private ISessionManager CreateSessionManager()
    {
      return new SessionManager(() => { 
        var mock = new Mock<ISession>();
        var sessionId = Guid.NewGuid().ToString();
        mock.Setup(session => session.GetSessionId()).Returns(sessionId);
        return mock.Object;
      });
    }
    
    [Fact]
    public void SessionManager_AddSession_WithReachMaxLmimit_ThrowsSessionNotCreatedException()
    {
      var sessionManager = CreateSessionManager();
      for (int i = 0; i < SessionManager.MaxSessionSupported; i++)
      {
        sessionManager.AddSession(sessionManager.NewSession());
      }

      Assert.Throws<SessionNotCreatedException>(() => sessionManager.AddSession(sessionManager.NewSession()));
    }

    [Fact]
    public void SessionManager_GetSession_WithUnknownSessionId_ThrowsInvalidSessionIdException()
    {
      var sessionManager = CreateSessionManager();
      Assert.Throws<InvalidSessionIdException>( () => sessionManager.GetSession("abc"));
    }

    [Fact]
    public void SessionManager_DeleteSession_WithUnknownSessionId_NoThrows()
    {
      var sessionManager = CreateSessionManager();
      sessionManager.DeleteSession("abc");
    }

    [Fact]
    public void SessionManager_GetAddDeleteSession()
    {
      var sessionManager = CreateSessionManager();
      var session1 = sessionManager.NewSession();
      sessionManager.AddSession(session1);
      sessionManager.ContainsSession(session1.GetSessionId()).Should().BeTrue();

      var session2 = sessionManager.NewSession();
      sessionManager.AddSession(session2);
      sessionManager.ContainsSession(session2.GetSessionId()).Should().BeTrue();

      sessionManager.GetSessions().Count.Should().Be(2);

      sessionManager.DeleteSession(session1.GetSessionId());
      sessionManager.DeleteSession(session2.GetSessionId());

      sessionManager.GetSessions().Count.Should().Be(0);
    }

  }
}
