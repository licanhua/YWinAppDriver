// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WinAppDriver;
using WinAppDriver.Infra;
using WinAppDriver.Infra.Communication;

namespace Infra.UnitTest
{
  class Helpers
  {
    public static Locator EmptyLocator = new Locator() { Strategy = LocatorStrategy.AccessibilityId, Value = "Empty"};
    public static Locator TwoElementLocator = new Locator() { Strategy = LocatorStrategy.ClassName, Value = "Two" };
    public static Locator RootLocator = new Locator() { Strategy = LocatorStrategy.ClassName, Value = "Root" };

    private static IElement _foundElement = CreateElement();
    private static IElement _rootElement = CreateElement();

    private static List<IElement> _emptyList = new List<IElement>();
    private static List<IElement> _twoList = new List<IElement>() { CreateElement(), CreateElement() };

    public static IApplication CreateApplication()
    {
      var app = new Mock<IApplication>();
      app.Setup(x => x.GetApplicationRoot()).Returns(_rootElement);
      app.Setup(x => x.GetProcessId()).Returns(1);
      return app.Object;
    }
    public static IElement CreateElement()
    {
      var element = new Mock<IElement>();
      element.Setup(e => e.FindElement(EmptyLocator, 0)).Throws(new ElementNotFound());
      element.Setup(e => e.FindElement(TwoElementLocator, 0)).Returns(() => _foundElement);
      element.Setup(e => e.FindElement(RootLocator, 0)).Returns(() => _rootElement);
      element.Setup(e => e.FindElements(EmptyLocator, 0)).Returns(() => _emptyList);
      element.Setup(e => e.FindElements(TwoElementLocator, 0)).Returns(() => _twoList);
      var id = Guid.NewGuid().ToString();
      element.Setup(e => e.GetId()).Returns(id);
      return element.Object;
    }
    public static ISession CreateSession()
    {   
      var application = CreateApplication();
      var applicationManager = new Mock<IApplicationManager>();
      applicationManager.Setup(app => app.LaunchApplication(It.Ref<Capabilities>.IsAny)).Returns(application);
      var session =new Session(applicationManager.Object);
      session.LaunchApplication(new NewSessionReq());
      return session;
    }
  }
}
