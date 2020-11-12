// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinAppDriver;
using WinAppDriver.Infra;
using WinAppDriver.Infra.Request;
using Xunit;

namespace WinAppDriver.IntegrationTest
{
  public class SessionTest
  {
    [Fact]
    public async Task Test_Launch_Then_KillAlarmApp()
    {
      using (var client = new TestClientProvider().Client)
      {
        string sessionId = await Helpers.CreateNewSession(client, "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
        await Helpers.DeletSession(client, sessionId);
      }
    }

    [Fact]
    public async Task Test_DeleteSessionAlwaysSuccess()
    {
      using (var client = new TestClientProvider().Client)
      {
        var response = await client.DeleteAsync("session/123");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var sessionOkResponse = await Helpers.FromBody<SessionOkResponse>(response);
        sessionOkResponse.sessionId.Should().NotBeNullOrEmpty();
        sessionOkResponse.value.Should().BeNull();
      }
    }


    [Fact]
    public async Task Test_SessionCreate_With_MissingAppId()
    {
      using (var client = new TestClientProvider().Client)
      {
        var response = await Helpers.PostMessage< SessionFailDetail, Dictionary< string, object> >(client, "session", new Dictionary<string, object>() {
          { "desiredCapabilities", new Dictionary<string, object>() {
                                      { "app2", "more"}
                                    }
          }
        });

        response.statusCode.Should().NotBe(HttpStatusCode.OK);
        response.status.Should().Be((int)ResponseStatusCode.SessionNotCreateException);
      }
    }

    [Fact]
    public async Task Test_FindNotExistElement()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");

        var response = await Helpers.FindElement(client, sessionId, new FindElementReq()
           {
             strategy = "name",
             value = "doesn't exist"
           });
        response.statusCode.Should().NotBe(HttpStatusCode.OK);
      }
    }

    [Fact]
    public async Task Test_FindDesktopElement()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");

        var response =  await Helpers.FindElement(client, sessionId, new FindElementReq()
           {
             strategy = "class name",
             value = "#32769"
           });

        response.value.element.Should().NotBeNullOrEmpty();
      }
    }
    [Fact]
    public async Task Test_FindNotExistElements()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");

        var response = await Helpers.FindElements(client, sessionId, new FindElementsReqs()
           {
             strategy = "name",
             value = "doesn't exist"
           });
        response.value.Count.Should().Be(0);
      }
    }

    [Theory]
    [InlineData("timeouts")]
    [InlineData("timeouts/implicit_wait")]
    public async Task Test_SetTimeout(string endpoint)
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");
        var response = await Helpers.PostSessionMessage<object, SetImplicitTimeoutReq>(client, sessionId, endpoint, new SetImplicitTimeoutReq() { type = "implicit", ms= 1});
        response.statusCode.Should().Be(HttpStatusCode.OK);
      }
    }

    [Theory]
    [InlineData("timeouts")]
    [InlineData("timeouts/async_script")]
    public async Task Test_SetTimeout_WithUnsupportedType(string endpoint)
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");
        var response = await Helpers.PostSessionMessage<object, SetImplicitTimeoutReq>(client, sessionId, endpoint, new SetImplicitTimeoutReq() { type = "async_script", ms = 1 });
        response.statusCode.Should().NotBe(HttpStatusCode.OK);
      }
    }

    [Fact]
    public async Task Test_GetWindowHandle()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");

        var response = await Helpers.GetSessionMessage<string>(client, sessionId, "window_handle");
        response.statusCode.Should().Be(HttpStatusCode.OK);
        response.value.Should().NotBeNullOrEmpty();
      }
    }

    [Fact]
    public async Task Test_GetWindowHandles()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, AppIds.Calculator);

        var response = await Helpers.GetSessionMessage<List<string>>(client, sessionId, "window_handles");

        await Helpers.DeletSession(client, sessionId);
        response.statusCode.Should().Be(HttpStatusCode.OK);
        response.value.Count.Should().BeGreaterThan(0);
      }
    }

    [Fact]
    public async Task Test_GetWindowHandles_ForRoot()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, AppIds.Root);

        var response = await Helpers.GetSessionMessage<List<string>>(client, sessionId, "window_handles");

        await Helpers.DeletSession(client, sessionId);
        response.statusCode.Should().Be(HttpStatusCode.OK);
        response.value.Count.Should().BeGreaterThan(1);
      }
    }

    [Fact]
    public async Task Test_ActivateWindow_WithWrongWindowId()
    {
      using (var client = new TestClientProvider().Client)
      {
        var session = await Helpers.CreateNewSession(client, AppIds.Root);
        
        var result = await Helpers.ActivateWindow(client, session, "Anything");
        result.statusCode.Should().NotBe(HttpStatusCode.OK);
        result.status.Should().Be((int)ResponseStatusCode.NoSucnWindow);
      }
    }

    [Fact]
    public async Task Test_DeleteWindow()
    {
      using (var client = new TestClientProvider().Client)
      {
        var session = await Helpers.CreateNewSession(client, AppIds.AlarmClock);

        var result = await Helpers.DeleteWindow(client, session);
        result.statusCode.Should().Be(HttpStatusCode.OK);
      }
    }

    [Fact]
    public async Task Test_DeleteDesktopWindow_WillFail()
    {
      using (var client = new TestClientProvider().Client)
      {
        var session = await Helpers.CreateNewSession(client, AppIds.Root);

        var result = await Helpers.DeleteWindow(client, session);
        result.statusCode.Should().NotBe(HttpStatusCode.OK);
      }
    }
    [Fact]
    public async Task Test_ActivateWindow_Interactive()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionAlarm = await Helpers.CreateNewSession(client, AppIds.AlarmClock);
        var windowAlarm = (await Helpers.GetSessionMessage<string>(client, sessionAlarm, "window_handle")).value;

        var sessionCalculator = await Helpers.CreateNewSession(client, AppIds.Calculator);
        var windowCalcluator = (await Helpers.GetSessionMessage<string>(client, sessionCalculator, "window_handle")).value;

        var result = await Helpers.ActivateWindow(client, sessionAlarm, windowAlarm);
        result.statusCode.Should().Be(HttpStatusCode.OK);

        Task.Delay(50).Wait();
        result = await Helpers.ActivateWindow(client, sessionCalculator, windowCalcluator);
        result.statusCode.Should().Be(HttpStatusCode.OK);

        Task.Delay(50).Wait();
        await Helpers.DeletSession(client, sessionAlarm);
        await Helpers.DeletSession(client, sessionCalculator);
        
      }
    }
  }
}
