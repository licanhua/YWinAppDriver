// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using WinAppDriver.Infra.Communication;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra
{
  public interface ISession
  {
    void SetImplicitTimeout(int msTimeout);
    string GetSessionId();
    void LaunchApplication(NewSessionReq req);
    void QuitApplication();
    Capabilities GetCapabilities();
    public IElement FindElement(Locator locator);
    public IElement FindElement(string elementId);
    public IEnumerable<IElement> FindElements(Locator locator);

    public IElement FindElement(string startElement, Locator locator);
    public IEnumerable<IElement> FindElements(string startElement, Locator locator);
    string GetSource();
  }
}
