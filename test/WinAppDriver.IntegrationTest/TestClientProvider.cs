// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WinAppDriver.IntegrationTest
{
  public class TestClientProvider
  {
    public HttpClient Client { get; private set; }

    public TestClientProvider()
    {
      var server = new TestServer(new WebHostBuilder().UseStartup<WinAppDriver.Startup>());

      Client = server.CreateClient();
    }
  }
}
