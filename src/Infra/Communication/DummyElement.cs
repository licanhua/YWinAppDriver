using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra.Communication
{
  public class DummyElement : IElement
  {
    public void SetFocus()
    {
      throw new NoSuchWindow();
    }

    public void Clear()
    {
      throw new NoSuchWindow();
    }

    public void Click()
    {
      throw new NoSuchWindow();
    }

    public void CloseActiveWindow()
    {
      throw new NoSuchWindow();
    }

    public void CloseWindow()
    {
      throw new NoSuchWindow();
    }

    public void DoubleClick()
    {
      throw new NoSuchWindow();
    }

    public bool ElementEquals(IElement element)
    {
      return false;
    }

    public IElement FindElement(Locator locator, int msTimeout)
    {
      throw new NoSuchWindow();
    }

    public IEnumerable<IElement> FindElements(Locator locator, int msTimeout)
    {
      return new List<IElement>();
    }

    public string GetAttribute(string attributeName)
    {
      throw new NoSuchWindow();
    }

    public string GetAttribute(LocatorStrategy locator)
    {
      throw new NoSuchWindow();
    }

    public IEnumerable<IElement> GetChildren()
    {
      return new List<IElement>();
    }

    public IElement GetFocusedElement()
    {
      throw new NoSuchWindow();
    }

    public string GetId()
    {
      return "NoThisElement";
    }

    public XYResult GetLocation()
    {
      throw new NoSuchWindow();
    }

    public SizeResult GetSize()
    {
      throw new NoSuchWindow();
    }

    public string GetTagName()
    {
      throw new NoSuchWindow();
    }

    public string GetText()
    {
      throw new NoSuchWindow();
    }

    public string GetTitle()
    {
      throw new NoSuchWindow();
    }

    public object GetUIObject()
    {
      throw new NoSuchWindow();
    }

    public IElement GetWindow(string windowId)
    {
      throw new NoSuchWindow();
    }

    public IElement GetWindowHandle()
    {
      throw new NoSuchWindow();
    }

    public IEnumerable<IElement> GetWindowHandles()
    {
      return new List<IElement>();
    }

    public XmlDocument GetXmlDocument()
    {
      throw new NoSuchWindow();
    }

    public bool IsDisplayed()
    {
      throw new NoSuchWindow();
    }

    public bool IsEnabled()
    {
      throw new NoSuchWindow();
    }

    public bool IsSelected()
    {
      throw new NoSuchWindow();
    }

    public bool IsStaleElement()
    {
      throw new NoSuchWindow();
    }

    public void SendKeys(string keys)
    {
      throw new NoSuchWindow();
    }

    public Rectangle GetBoundingRectangle()
    {
      throw new NoSuchWindow();
    }

    public Rectangle GetDesktopRectangle()
    {
      throw new NoSuchWindow();
    }
  }
}
