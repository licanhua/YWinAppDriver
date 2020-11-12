// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WinAppDriver.Infra;
using Xunit;

namespace WinAppDriver.IntegrationTest
{
  public class NotepadTest
  {
    [Fact]
    public async void NotepadTestAll()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, new NewSessionReq()
        {
          desiredCapabilities = new Capabilities()
          {
            app = "C:\\Windows\\notepad.exe"
          }
        });

        sessionId.Should().NotBeNullOrEmpty();
        await Helpers.DeletSession(client, sessionId);
      }
    }
  }
}
