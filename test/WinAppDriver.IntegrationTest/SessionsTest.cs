using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WinAppDriver.Infra;
using WinAppDriver.Infra.Result;
using Xunit;

namespace WinAppDriver.IntegrationTest
{
  public class SessionsTest
  {
    [Fact]
    public async Task Test_GetSessions()
    {
      using (var client = new TestClientProvider().Client)
      {
        var session = await Helpers.CreateNewSession(client, "Root");
        session.Should().NotBeNullOrEmpty();

        var response = await client.GetAsync("sessions");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await Helpers.FromBody<SessionOkResponse>(response);
        body.Should().NotBeNull();

        var sessions = JsonConvert.DeserializeObject<List<GetSessionsResultItem>>(body.value.ToString());
        sessions.Count.Should().BeGreaterThan(0);
        sessions[0].sessionId.Should().NotBeNullOrEmpty();
      }
    }
  }
}
