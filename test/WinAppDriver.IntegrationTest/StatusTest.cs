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
  public class StatusTest
  {
    [Fact]
    public async Task Test_Status()
    {
      using (var client = new TestClientProvider().Client)
      {
        var response = await client.GetAsync("status");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await Helpers.FromBody<SessionOkResponse>(response);
        var status = JsonConvert.DeserializeObject<StatusResult>(body.value.ToString());
        status.osName.Should().NotBeNullOrEmpty();
        status.buildTime.Should().NotBeNullOrEmpty();

      }
    }
  }
}
