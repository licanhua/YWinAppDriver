// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Windows.Apps.Test.Automation;
using Microsoft.Windows.Apps.Test.Foundation;
using Microsoft.Windows.Apps.Test.Foundation.Collections;
using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Microsoft.Windows.Apps.Test.Foundation.Patterns;
using Microsoft.Windows.Apps.Test.Foundation.Waiters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra.Communication
{
  public class Element : IElement
  {
    private readonly UIObject _uiObject;
    private readonly string id;
    private void EnsureTimeoutSettting(int msTimeout)
    {
      if (msTimeout == 0)
      {
        UICollection.Timeout = TimeSpan.FromMilliseconds(0) ;
        UICollection.RetryCount = 1;
      }
      else 
      {
        UICollection.Timeout = TimeSpan.FromMilliseconds(msTimeout);
        UICollection.RetryCount = Math.Max(1, msTimeout / 100);
      }
    }

    public Element(UIObject uIObject)
    {
      _uiObject = uIObject;
      if (_uiObject != null)
      {
        id = _uiObject.RuntimeId;
      }
      if (id == null)
      {
        id = Guid.NewGuid().ToString();
      }
    }

    public void CloseWindow()
    {
      try
      {
        if (_uiObject != null)
        {
          Window applicationWindow = new Window(_uiObject);
          using (UIEventWaiter windowClosedWaiter = applicationWindow.GetWindowClosedWaiter())
          {
            applicationWindow.Close();
            windowClosedWaiter.TryWait(TimeSpan.FromSeconds(5));
          }
        }
      }
      catch { }
    }

    private UICondition GetSearchCondition(Locator locator)
    {
      if (locator.Strategy == LocatorStrategy.AccessibilityId)
      {
        return UICondition.CreateFromId(locator.Value);
      }
      else if (locator.Strategy == LocatorStrategy.ClassName)
      {
        return UICondition.CreateFromClassName(locator.Value);
      }
      else if (locator.Strategy == LocatorStrategy.Name)
      {
        return UICondition.CreateFromName(locator.Value);
      }
      else if (locator.Strategy == LocatorStrategy.Id)
      {
        return UICondition.Create(UIProperty.Get(ActionStrings.RuntimeId), locator.Value);
      }
      else if (locator.Strategy == LocatorStrategy.TagName)
      {
        return UICondition.Create(UIProperty.Get(ActionStrings.LocalizedControlType), locator.Value);
      }
      throw new LocatorNotSupported();
    }


    public IElement FindElement(Locator locator,int msTimeout)
    {
      var condition = GetSearchCondition(locator);
      if (_uiObject.Matches(condition))
      {
        return this;
      }

      EnsureTimeoutSettting(msTimeout);
      var descendants = new UIBreadthFirstDescendants<UIObject>(_uiObject, UIObject.Factory);
      var element = descendants.Find(condition);
      if (element == null) throw new ElementNotFound("By " + locator.Strategy.ToString() + ":" + locator.Value);

      return new Element(element);
    }

    public IEnumerable<IElement> FindElements(Locator locator, int msTimeout)
    {
      List<IElement> result = new List<IElement>();
      var condition = GetSearchCondition(locator);

      if (_uiObject.Matches(condition))
      {
        result.Add(this);
      }

      EnsureTimeoutSettting(msTimeout);
      var descendants = new UIBreadthFirstDescendants<UIObject>(_uiObject, UIObject.Factory);
      result.AddRange(descendants.FindMultiple(condition).Select(element => new Element(element)));
      return result;
    }

    public string GetId()
    {
      return id;
    }

    public bool IsSelected()
    {
      return Convert.ToBoolean(_uiObject.GetProperty(UIProperty.Get(ActionStrings.IsSelected)).ToString());
    }

    public void Click()
    {
      EnsureNotOffScreen();
      _uiObject.Click();
    }

    public void DoubleClick()
    {
      EnsureNotOffScreen();
      _uiObject.DoubleClick();
    }

    public bool IsStaleElement()
    {
      if (_uiObject == null || !(_uiObject.ProcessId > 0 && _uiObject.LocalizedControlType != null))
      {
        return true;
      }
      return false;
    }

    public string GetText()
    {
      // TODO:
      // This is only part of implementation. Complete list can refer to 
      // https://docs.microsoft.com/en-us/windows/uwp/design/accessibility/control-patterns-and-interfaces
      List<AutomationPattern> supportedPattern = _uiObject.GetSupportedPatterns().ToList();

      if (supportedPattern.Contains(ValuePatternIdentifiers.Pattern))
      {
        return new ValueImplementation(_uiObject).Value;
      }
      else if (supportedPattern.Contains(TextPatternIdentifiers.Pattern))
      {
        return new TextBlock(_uiObject).DocumentText;
      }
      else if (supportedPattern.Contains(RangeValuePatternIdentifiers.Pattern))
      { 
        return new RangeValueImplementation(_uiObject).Value.ToString();
      }
      return _uiObject.Name;
    }

    public XmlDocument GetXmlDocument()
    {
      var doc = new XmlDocument();
      var node = BuildXmlNode(doc, _uiObject);
      doc.AppendChild(node);
      return doc;
    }

    private XmlNode BuildXmlNode(XmlDocument doc, UIObject obj)
    {
      var element = doc.CreateElement(GetName());
      element.SetAttribute("AccessibilityId", obj.AutomationId);
      element.SetAttribute("Location", obj.BoundingRectangle.ToString());
      element.SetAttribute("ClassName", obj.ClassName);
      element.SetAttribute("TagName", obj.LocalizedControlType);
      element.SetAttribute("Name", obj.Name);
      element.SetAttribute("id", obj.RuntimeId);

      foreach (var e in obj.Children)
      {
        element.AppendChild(BuildXmlNode(doc, e));
      }
      return element;
    }

    public string GetName()
    {
      return _uiObject.LocalizedControlType;
    }

    public void Clear()
    {
      EnsureNotOffScreen();
       new Edit(_uiObject).SetValue("");
    }

    public void Value(string value)
    {
      // TODO: Need to implement https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol#sessionsessionidelementidvalue
      EnsureNotOffScreen();
      _uiObject.SendKeys(value);
    }

    private void EnsureNotOffScreen()
    {
      if (_uiObject.IsOffscreen) {
        throw new ElementOffScreen();
      }
    }

    public GetSizeResult GetSize()
    {
      return new GetSizeResult() { height = _uiObject.BoundingRectangle.Height, width = _uiObject.BoundingRectangle.Width };
    }

    public GetLocationResult GetLocation()
    {
      return new GetLocationResult() { x = _uiObject.BoundingRectangle.X, y = _uiObject.BoundingRectangle.Y };
    }

    public bool IsDisplayed()
    {
      return !_uiObject.IsOffscreen;
    }

    public bool IsEnabled()
    {
      return _uiObject.IsEnabled;
    }

    private UIObject GetActiveWindow()
    {
      if (UIObject.Focused.ProcessId != _uiObject.ProcessId)
      {
        return _uiObject;
      }
      else 
      {
        return UIObject.Focused;
      }
    }
    public string GetWindowHandle()
    {
      return GetActiveWindow().NativeWindowHandle.ToString();
    }

    private IEnumerable<UIObject> GetWindows()
    {
      List<UIObject> result = new List<UIObject>();

      var condition = UICondition.CreateFromClassName("Window").OrWith(UICondition.CreateFromClassName("ApplicationFrameWindow"));
      foreach (var window in UIObject.Root.Descendants.FindMultiple(condition))
      {
        if (window.ProcessId == _uiObject.ProcessId || _uiObject == UIObject.Root || _uiObject.Parent == UIObject.Root) // root returns itself and all its child process window
        {
          result.Add(window);
        }
      }

      return result;
    }


    public IEnumerable<string> GetWindowHandles()
    {
      return GetWindows().Select(window => window.NativeWindowHandle.ToString());
    }

    public void ActivateWindow(string window)
    {
      if (String.IsNullOrEmpty(window))
      {
        throw new InvalidArgumentException("name can't be empty");
      }
      var found = GetWindows().Where(w => { return w.NativeWindowHandle.ToString() == window; }).FirstOrDefault();
      if (found == null)
      {
        throw new NoSuchWindow();
      }
      found.SetFocus();
    }

    public void CloseActiveWindow()

    {
      var activeWindow = GetActiveWindow();
      if (activeWindow == UIObject.Root)
      {
        throw new InvalidArgumentException("Not allow to close the desktop root window");
      }
      new Window(GetActiveWindow()).Close();
    }

    public string GetTitle()
    {
      return _uiObject.Name;
    }
  }
}
