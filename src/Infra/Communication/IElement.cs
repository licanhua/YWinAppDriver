// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra.Communication
{
  public interface IElement
  {
    public IEnumerable<IElement> GetChildren();
    public string GetAttribute(string attributeName);
    public string GetAttribute(LocatorStrategy locator);
    public bool IsStaleElement();
    public bool IsSelected();
    public string GetId();
    public void CloseWindow();
    public string GetTagName();
    public void SendKeys(string keys);
    public string GetText();
    public XmlDocument GetXmlDocument();
    public IElement FindElement(Locator locator, int msTimeout);
    public IEnumerable<IElement> FindElements(Locator locator, int msTimeout);

    public IElement GetWindowHandle();
    public IEnumerable<IElement> GetWindowHandles();
    public IElement GetWindow(string windowId);
    public void SetFocus();
    public void CloseActiveWindow();
    public string GetTitle();
    public void Click();
    public void DoubleClick();
    public void Clear();
    SizeResult GetSize();
    XYResult GetLocation();
    bool IsDisplayed();
    bool IsEnabled();
    bool ElementEquals(IElement element);
    object GetUIObject();
    public IElement GetFocusedElement();
    // UI Element screen size
    Rectangle GetBoundingRectangle();
    // Windows desktop screen size
    Rectangle GetDesktopRectangle();
  }
}
