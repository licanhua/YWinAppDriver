// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text;
using System.Xml;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra.Communication
{
  public interface IElement
  {
    public bool IsStaleElement();
    public bool IsSelected();
    public string GetId();
    public void CloseWindow();
    public string GetName();
    public string GetText();
    public XmlDocument GetXmlDocument();
    public IElement FindElement(Locator locator, int msTimeout);
    public IEnumerable<IElement> FindElements(Locator locator, int msTimeout);

    public void Click();
    public void DoubleClick();
    public void Clear();
    SizeResult GetSize();
    LocationResult GetLocation();
    bool IsDisplayed();
    bool IsEnabled();
  }
}
