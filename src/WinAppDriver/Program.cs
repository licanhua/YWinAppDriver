// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WinAppDriver.Infra.Helper;

namespace WinAppDriver
{
  public class Program
  {
    public static void Main(string[] args)
    {
      // Without this setting, it used logical coordinates, and touch/move/screenshot would be in the wrong location.
      // Make it to use physic coordinates. https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-screenscaling
      Helpers.SetProcessDpiAwareness(Helpers.PROCESS_PER_MONITOR_DPI_AWARE);

      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
              config.AddCommandLine(args, new Dictionary<string, string>() {
                                            { "--basepath", "BasePath"}
                                          });
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
