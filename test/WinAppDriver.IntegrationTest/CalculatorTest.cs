// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
using Xunit;

namespace WinAppDriver.IntegrationTest
{
  public class CalculateTest
  {
    [Fact]
    public async void TestCalculatorAll()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, AppIds.Calculator);

        sessionId.Should().NotBeNullOrEmpty();


        await Helpers.DeletSession(client, sessionId);
      }
    }
  }
}
