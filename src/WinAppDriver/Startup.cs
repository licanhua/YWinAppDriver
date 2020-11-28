// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WinAppDriver.Infra;
using WinAppDriver.Infra.CommandHandler;
using WinAppDriver.Infra.Communication;

namespace WinAppDriver
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers().AddNewtonsoftJson();
      services.AddSingleton<ISessionManager>((container) => new SessionManager(() => {    var logger = container.GetRequiredService<ILogger<Session>>();
                                                                                          return new Session(
                                                                                              new ApplicationMananger(), logger
                                                                                    ); }));
      services.AddSingleton<ICommandHandlers, CommandHandlers>();
      services.AddSingleton<IElementCommandHandler, ElementCommandHandler>();

      services.AddSwaggerDocument();

      services.AddLogging(opt =>
       {
         opt.AddConsole(c =>
         {
           c.TimestampFormat = "[HH:mm:ss:fffffff] ";
         });

       });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      var logPath= Configuration["LogPath"];
      if (!string.IsNullOrEmpty(logPath))
      {
        var file = logPath + "\\WinAppDriver-{Date}.log";
        Console.WriteLine("LogPath: " + logPath);
        loggerFactory.AddFile(file);
      }

      var basepath = Configuration["BasePath"];
      var urls = Configuration["Urls"];
      if (!string.IsNullOrEmpty(basepath))
      {
        Console.WriteLine("Launch " + urls + basepath);
        app.UsePathBase(basepath);
      }
      else 
      {
        Console.WriteLine("Launch " + urls);
      }
      app.UseRouting();

      app.UseAuthorization();

      // Register the Swagger generator and the Swagger UI middlewares
      app.UseOpenApi();
      app.UseSwaggerUi3();

      app.UseMiddleware<RequestResponseLoggingMiddleware>();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
