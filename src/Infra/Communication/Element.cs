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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra.Communication
{

  public class Element : IElement
  {
    static Regex TagNameRegExpr = new Regex("^[a-zA-Z0-9 ]*$");
    const string TAGUNKNOWN = "Unknown";
    const string UNIQID = "UniqId";

    private readonly UIObject _uiObject;
    private string _id;
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

    public void GenerateGuidAsId()
    {
      _id = Guid.NewGuid().ToString();
    }
    public Element(UIObject uIObject)
    {
      _uiObject = uIObject;
 
      _id = _uiObject.RuntimeId;
      if (_id == null)
      {
        GenerateGuidAsId();
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

    private UICondition GetUIObjectSearchCondition(Locator locator)
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
      if (locator.Strategy == LocatorStrategy.XPath)
      {
        return FindElementByXPath(locator.Value);
      }

      var condition = GetUIObjectSearchCondition(locator);
      if (_uiObject.Matches(condition))
      {
        return this;
      }

      EnsureTimeoutSettting(msTimeout);

      try
      {
        var descendants = new UIBreadthFirstDescendants<UIObject>(_uiObject, UIObject.Factory);
        var element = descendants.Find(condition);
        if (element == null) throw new ElementNotFound("By " + locator.Strategy.ToString() + ":" + locator.Value);

        return new Element(element);
      }
      catch (Exception e)
      {
        throw new ElementNotFound(e.Message);
      }
    }

    public IEnumerable<IElement> FindElements(Locator locator, int msTimeout)
    {
      if (locator.Strategy == LocatorStrategy.XPath)
      {
        return FindElementsByXPath(locator.Value);
      }

      List<IElement> result = new List<IElement>();
      var condition = GetUIObjectSearchCondition(locator);

      if (_uiObject.Matches(condition))
      {
        result.Add(this);
      }

      EnsureTimeoutSettting(msTimeout);

      try
      { 
        var descendants = new UIBreadthFirstDescendants<UIObject>(_uiObject, UIObject.Factory);
        result.AddRange(descendants.FindMultiple(condition).Select(element => new Element(element)));
        return result;
      }
      catch (Exception e)
      {
        throw new ElementNotFound(e.Message);
      }
    }

    public string GetId()
    {
      return _id;
    }

    public bool IsSelected()
    {
      foreach (var pattern in _uiObject.GetSupportedPatterns())
      {
        if (pattern.Id == SelectionItemPattern.Pattern.Id)
        {
          return new ListViewItem(_uiObject).IsSelected; // ListView supports the ISelectItem.IsSelected
        }
        else if (pattern.Id == TogglePattern.Pattern.Id)
        {
          return new ToggleButton(_uiObject).ToggleState.ToString() == "On";
        }
      }
      throw new NotImplementedException("Not implement yet, what control are you using>");
    }

    public void Click()
    {
      EnsureNotOffScreen();
      foreach (var pattern in _uiObject.GetSupportedPatterns())
      {
        if (pattern.Id == InvokePattern.Pattern.Id)
        {
          new Button(_uiObject).Invoke(); // Invoke has better performance than click. I guess click is synchronized but invoke is not
          return;
        }
      }
      _uiObject.Click();

    }

    public void DoubleClick()
    {
      EnsureNotOffScreen();
      _uiObject.DoubleClick();
    }

    public bool IsStaleElement()
    {
      try
      {
        if (_uiObject == null || !(_uiObject.ProcessId > 0 && _uiObject.LocalizedControlType != null))
        {
          return true;
        }
      }
      catch
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

    // Add an indirect node to make appiumdesktop v1.18.3 happy
    private void BuildXmlDoc(XmlDocument doc, XmlNode node)
    {
      var body = doc.CreateElement("Body");
      body.AppendChild(node);
      doc.AppendChild(body);
    }
    public XmlDocument GetXmlDocument()
    {
      var doc = new XmlDocument();
      var node = BuildXmlNode(doc, _uiObject, null);
      BuildXmlDoc(doc, node);
      return doc;
    }

    public IElement FindElementByXPath(string xpath)
    {
      Dictionary<string, IElement> uniqIdCache = new Dictionary<string, IElement>();
      
      var doc = new XmlDocument();
      var xmlNode = BuildXmlNode(doc, _uiObject, uniqIdCache);
      BuildXmlDoc(doc, xmlNode);

      var node = doc.SelectSingleNode(xpath);
      if (node == null)
      {
        throw new ElementNotFound();
      }
      else
      {
        var key = node.Attributes[UNIQID].Value;
        return uniqIdCache[key];
      }
    }

    public IEnumerable<IElement> FindElementsByXPath(string xpath)
    {
      List<IElement> elements = new List<IElement>();
      Dictionary<string, IElement> uniqIdCache = new Dictionary<string, IElement>();

      var doc = new XmlDocument();
      var xmlNode = BuildXmlNode(doc, _uiObject, uniqIdCache);
      BuildXmlDoc(doc, xmlNode);

      var nodes = doc.SelectNodes(xpath);
      foreach (XmlNode node in nodes)
      {
        if (node == null) continue;

        var key = node.Attributes[UNIQID].Value;
        elements.Add(uniqIdCache[key]);
      }
      return elements;
    }

    private string GetXamlNodeTag(UIObject obj)
    {
      var name = obj.LocalizedControlType.ToProperCase();

      if (!string.IsNullOrWhiteSpace(name) && TagNameRegExpr.IsMatch(name))
      {
        return name;
      }
      else 
      {
        return TAGUNKNOWN;
      }
    }

    private XmlNode BuildXmlNode(XmlDocument doc, UIObject obj, Dictionary<string, IElement> uniqIdCache)
    {
      var element = doc.CreateElement(GetXamlNodeTag(obj));
      element.SetAttribute("AutomationId", System.Net.WebUtility.HtmlEncode(obj.AutomationId));
      element.SetAttribute("x", System.Net.WebUtility.HtmlEncode(obj.BoundingRectangle.X.ToString()));
      element.SetAttribute("y", System.Net.WebUtility.HtmlEncode(obj.BoundingRectangle.Y.ToString()));
      element.SetAttribute("width", System.Net.WebUtility.HtmlEncode(obj.BoundingRectangle.Width.ToString()));
      element.SetAttribute("height", System.Net.WebUtility.HtmlEncode(obj.BoundingRectangle.Height.ToString()));
      element.SetAttribute("IsOffScreen", System.Net.WebUtility.HtmlEncode(obj.IsOffscreen.ToString()));
      element.SetAttribute("IsEnabled", System.Net.WebUtility.HtmlEncode(obj.IsEnabled.ToString()));
      element.SetAttribute("ClassName", System.Net.WebUtility.HtmlEncode(obj.ClassName));
      element.SetAttribute("LocalizedControlType", System.Net.WebUtility.HtmlEncode(obj.LocalizedControlType));
      element.SetAttribute("Name", System.Net.WebUtility.HtmlEncode(obj.Name));
      element.SetAttribute("RuntimeId", System.Net.WebUtility.HtmlEncode(obj.RuntimeId));

      if (uniqIdCache != null)
      {
        var e = new Element(obj);
        if (uniqIdCache.ContainsKey(e.GetId()))
        {
          e.GenerateGuidAsId();
        }
        element.SetAttribute(UNIQID, e.GetId());
        uniqIdCache[e.GetId()] = e;
      }

      foreach (var e in obj.Children)
      {
        element.AppendChild(BuildXmlNode(doc, e, uniqIdCache));
      }
      return element;
    }

    public string GetAttribute(LocatorStrategy locator)
    {
      if (locator == LocatorStrategy.Id) return _id;
      else if (locator == LocatorStrategy.AccessibilityId) return _uiObject.AutomationId;
      else if (locator == LocatorStrategy.ClassName) return _uiObject.ClassName;
      else if (locator == LocatorStrategy.TagName) return _uiObject.LocalizedControlType;
      else if (locator == LocatorStrategy.Name) return _uiObject.Name;
      return null;
    }

    public string GetTagName()
    {
      return _uiObject.LocalizedControlType;
    }

    public void Clear()
    {
      EnsureNotOffScreen();
       new Edit(_uiObject).SetValue(String.Empty);
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

    public SizeResult GetSize()
    {
      return new SizeResult() { height = _uiObject.BoundingRectangle.Height, width = _uiObject.BoundingRectangle.Width };
    }

    public XYResult GetLocation()
    {
      return new XYResult() { x = _uiObject.BoundingRectangle.X, y = _uiObject.BoundingRectangle.Y };
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
    public IElement GetWindowHandle()
    {
      return new Element(GetActiveWindow());
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


    public IEnumerable<IElement> GetWindowHandles()
    {
      return GetWindows().Select(window => new Element(window));
    }

    public void SetFocus()
    {
      _uiObject.SetFocus();
    }
    public IElement GetWindow(string windowId)
    {
      if (String.IsNullOrEmpty(windowId))
      {
        throw new InvalidArgumentException("name can't be empty");
      }
      var found = GetWindows().Where(w => { return w.NativeWindowHandle.ToString() == windowId; }).FirstOrDefault();
      if (found == null)
      {
        throw new NoSuchWindow();
      }
      return new Element(found);
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

    public bool ElementEquals(IElement element)
    {
      return _uiObject == (UIObject)element.GetUIObject();
    }

    public object GetUIObject()
    {
      return _uiObject;
    }

    public IElement GetFocusedElement()
    {
      if (UIObject.Focused != null && UIObject.Focused.ProcessId != _uiObject.ProcessId) // Focus on other app
      {
        return null;
      }
      else
      {
        return new Element(UIObject.Focused);
      }
    }

    public string GetAttribute(string attributeName)
    {
      // support https://github.com/microsoft/WinAppDriver/blob/master/Tests/WebDriverAPI/ElementAttribute.cs
      if (attributeName == ActionStrings.RuntimeId)
      {
        return _id;
      }
      else if (attributeName == "name")
      {
        return _uiObject.Name;
      }
      else if (UIProperty.Exists(attributeName))
      {
        return _uiObject.GetProperty(UIProperty.Get(attributeName)).ToString();
      }
      throw new NotSupportedException("attribute name: " + attributeName);
    }

    public void SendKeys(string keys)
    {
      var text = KeyboardHelper.TranslateKey(keys);
      if (!String.IsNullOrEmpty(text))
      {
        _uiObject.SendKeys(text);
      }
    }

    public IEnumerable<IElement> GetChildren()
    {
      return _uiObject.Children.Select(item => new Element(item));
    }

    public Rectangle GetBoundingRectangle()
    {
      return _uiObject.BoundingRectangle;
    }

    public Rectangle GetDesktopRectangle()
    {
      return UIObject.Root.BoundingRectangle;
    }
  }
}
