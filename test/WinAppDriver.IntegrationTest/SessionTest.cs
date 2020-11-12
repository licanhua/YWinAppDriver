// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
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
        var response = await Helpers.PostMessage(client, "session", new Dictionary<string, object>() {
          { "desiredCapabilities", new Dictionary<string, object>() {
                                      { "app2", "more"}
                                    }
          }
        });

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var sessionResponse = await Helpers.FromBody<SessionFailResponse>(response);
        sessionResponse.value.Should().NotBeNull();

        var value = sessionResponse.value.ToString();
        sessionResponse.status.Should().Be((int)ResponseStatusCode.SessionNotCreateException);
      }
    }

    [Fact]
    public async Task Test_FindNotExistElement()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");

        await Assert.ThrowsAnyAsync<Exception>(async () =>
           await Helpers.FindElement(client, sessionId, new FindElementReq()
           {
             strategy = "name",
             value = "doesn't exist"
           }));
        
      }
    }

    [Fact]
    public async Task Test_FindDesktopElement()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");

        var element =  await Helpers.FindElement(client, sessionId, new FindElementReq()
           {
             strategy = "class name",
             value = "#32769"
           });

        element.element.Should().NotBeNullOrEmpty();
      }
    }
    [Fact]
    public async Task Test_FindNotExistElements()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, "Root");

        var elements = await Helpers.FindElements(client, sessionId, new GetElementsReqs()
           {
             strategy = "name",
             value = "doesn't exist"
           });
        elements.Count.Should().Be(0);
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
        var response = await Helpers.PostMessage<SetImplicitTimeoutReq>(client, sessionId, endpoint, new SetImplicitTimeoutReq() { type = "implicit", ms= 1});
        response.StatusCode.Should().Be(HttpStatusCode.OK);
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
        var response = await Helpers.PostMessage<SetImplicitTimeoutReq>(client, sessionId, endpoint, new SetImplicitTimeoutReq() { type = "async_script", ms = 1 });
        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
      }
    }
  }
}
