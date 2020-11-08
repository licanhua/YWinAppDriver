// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Windows.Apps.Test.Foundation;
using Microsoft.Windows.Apps.Test.Foundation.Waiters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppDriver.Infra.Communication
{
  public class ApplicationMananger : IApplicationManager
  {
    private UIObject GetTopLevelWindow(UIObject uiObject)
    {
      if (uiObject == null) return null;

      while (uiObject.Parent != UIObject.Root) {
        uiObject = uiObject.Parent;
      }
      return uiObject;
    }

    // refer https://github.com/microsoft/microsoft-ui-xaml/blob/40531c714f8003bf0d341a0729fa04dd2ed87710/test/testinfra/MUXTestInfra/Infra/Application.cs#L269
    public IApplication LaunchModernApp(string appName)
    {
      UICondition condition = UICondition.CreateFromClassName("ApplicationFrameWindow").OrWith(UICondition.CreateFromClassName("Windows.UI.Core.CoreWindow"));

      var coreWindow = UAPApp.Launch(appName, condition);
      var rootWindow = GetTopLevelWindow(coreWindow);
      return new Application(new Element(rootWindow), coreWindow.ProcessId);
    }

    public IApplication LaunchLegacyApp(string filename, string arguments, string workingDirectory)
    {
      // refer to https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.-ctor?view=netcore-3.1#System_Diagnostics_ProcessStartInfo__ctor_System_String_System_String_
      ProcessStartInfo startInfo;

      if (String.IsNullOrEmpty(arguments))
      {
        startInfo = new ProcessStartInfo(filename);
      }
      else
      {
        startInfo = new ProcessStartInfo(filename, arguments);
      }

      if (!String.IsNullOrEmpty(workingDirectory))
      {
        startInfo.WorkingDirectory = workingDirectory;
      }

      startInfo.UseShellExecute = false;

      Process process;
      using (var windowOpenedWaiter = new WindowOpenedWaiter())
      {
        process = Process.Start(startInfo);
        if (process == null)
        {
          throw new AppLaunchException("Fail to start legacy process");
        }
        windowOpenedWaiter.TryWait(TimeSpan.FromSeconds(10));

        int retry = 5;
        TimeSpan sleepTimer = TimeSpan.FromSeconds(5);

        while (retry-- > 0)
        {
          // check if the new opened window
          if (windowOpenedWaiter.Source != null && windowOpenedWaiter.Source.ProcessId == process.Id)
          {
            // found
            var root = GetTopLevelWindow(windowOpenedWaiter.Source);
            return new Application(new Element(root), process.Id);
          }
          Task.Delay(sleepTimer).Wait(sleepTimer);
        }

      }

      var condition = UICondition.Create(UIProperty.Get(ActionStrings.ProcessId), process.Id);
      var matched = UIObject.Root.Children.FindMultiple(condition).FirstOrDefault();
      if (matched != null)
      {
        var root = GetTopLevelWindow(matched);
        return new Application(new Element(root), process.Id);
      }
      else
      {
        process.Kill();
        throw new AppLaunchException("Process was created, but fail to match a window");
      }
    }

      
    public IApplication LaunchApplication(Capabilities capabilities)
    {
      var app = capabilities.app;

      // Root means the the Windows desktop. It allows user to manipute all applications in Windows
      // refer https://github.com/microsoft/WinAppDriver/blob/master/Samples/C%23/CortanaTest/BingSearch.cs
      if (app == "Root")
      {
        var attachToTopLevelWindowClassName = capabilities.attachToTopLevelWindowClassName;

        Debug.WriteLine("LaunchApplication attach to " + attachToTopLevelWindowClassName == null ? "Root" : "window with ClassName" + attachToTopLevelWindowClassName);

        if (String.IsNullOrEmpty(attachToTopLevelWindowClassName))
        {
          return new Application(new Element(UIObject.Root), UIObject.Root.ProcessId, false);
        }
        else
        {
          var condition = UICondition.CreateFromClassName(attachToTopLevelWindowClassName);
          var matched = UIObject.Root.Children.FindMultiple(condition).FirstOrDefault();
          if (matched == null)
          {
            throw new AppLaunchException("There is no window match with class name " + attachToTopLevelWindowClassName);
          }
          else
          {
            return new Application(new Element(matched), matched.ProcessId, false);
          }
        }
      }
      else if (app.Contains("!"))
      {
        Debug.WriteLine("Start UWPApp " + app);
        return LaunchModernApp(capabilities.app);
      }
      else
      {
        Debug.WriteLine("Start Legacy app " + capabilities.ToString());
        return LaunchLegacyApp(app, capabilities.appArguments, capabilities.appWorkingDir);
      }

    }
  }
}
