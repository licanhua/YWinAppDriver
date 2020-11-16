// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.Windows.Apps.Test.Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using WinAppDriver.Infra.Communication;

namespace WinAppDriver.Infra
{
  internal class ElementCache
  {

    private Dictionary<LocatorStrategy, Dictionary<string, IElement>> _cache = new Dictionary<LocatorStrategy, Dictionary<string, IElement>>();
    private Dictionary<string, IElement> _windowCache = new Dictionary<string, IElement>();
    public ElementCache()
    {
      _cache[LocatorStrategy.AccessibilityId] = new Dictionary<string, IElement>();
      _cache[LocatorStrategy.ClassName] = new Dictionary<string, IElement>();
      _cache[LocatorStrategy.Id] = new Dictionary<string, IElement>();
      _cache[LocatorStrategy.Name] = new Dictionary<string, IElement>();
      _cache[LocatorStrategy.TagName] = new Dictionary<string, IElement>();
      _cache[LocatorStrategy.XPath] = new Dictionary<string, IElement>();
    }

    public void AddWindow(string key, IElement element)
    {
      _windowCache[key] = element;
    }

    public IElement GetWindowOrDefault(string windowId)
    {
      var window = _windowCache.GetValueOrDefault(windowId);
      if (window != null && window.IsStaleElement())
      {
        _windowCache.Remove(windowId);
        window = null;
      }
      return window;
    }
    public void AddElement(IElement element) 
    {
      _cache[LocatorStrategy.Id][element.GetId()] = element;
      var value = element.GetAttribute(LocatorStrategy.AccessibilityId);
      if (!string.IsNullOrEmpty(value))
      { _cache[LocatorStrategy.AccessibilityId][value] = element; 
      }

      value = element.GetAttribute(LocatorStrategy.ClassName);
      if (!string.IsNullOrEmpty(value))
      {
        _cache[LocatorStrategy.ClassName][value] = element;
      }

      value = element.GetAttribute(LocatorStrategy.Name);
      if (!string.IsNullOrEmpty(value))
      {
        _cache[LocatorStrategy.Name][value] = element;
      }

      value = element.GetAttribute(LocatorStrategy.TagName);
      if (!string.IsNullOrEmpty(value))
      {
        _cache[LocatorStrategy.TagName][value] = element;
      }
    }

    public IElement GetOrDefault(LocatorStrategy locator, string key)
    {
      var value = _cache[locator].GetValueOrDefault(key);

      if (value != null && locator != LocatorStrategy.Id)
      {
        if (value.IsStaleElement() || value.GetAttribute(locator) != key)
        {
          // obselete cache
          RemoveElement(locator, key);
          return null;
        }
      }
      return value;
    }

    public void RemoveElement(LocatorStrategy locator, string key)
    {
      _cache[locator].Remove(key);
    }

    public void Clear()
    {
      _cache[LocatorStrategy.AccessibilityId].Clear();
      _cache[LocatorStrategy.ClassName].Clear();
      _cache[LocatorStrategy.Id].Clear();
      _cache[LocatorStrategy.Name].Clear();
      _cache[LocatorStrategy.TagName].Clear();
      //_windowCache.Clear();
    }
  }

  public class Session : ISession
  {
    private readonly string _sessionId = Guid.NewGuid().ToString();
    private Capabilities _capabilities;
    private IApplicationManager _applicationManager;
    private IApplication _application;
    private int _msTimeout = 0;
    private readonly ILogger _logger;
    readonly ElementCache _cache = new ElementCache();

    public Session(IApplicationManager applicationManager, ILogger logger)
    {
      _applicationManager = applicationManager;
      _logger = logger;
    }

    public void QuitApplication()
    {
      if (_application != null)
      {
        _application.QuitApplication();
      }
    }

    public Capabilities GetCapabilities()
    {
      return _capabilities;
    }

    public string GetSessionId()
    {
      return _sessionId;
    }

    void DFS(IElement element)
    {
      if (element != null)
      {
        _cache.AddElement(element);
      }
      foreach (var child in element.GetChildren())
      {
        DFS(child);
      }
    }
    public void RebuildCache()
    {
      DFS(GetApplicationRoot());
    }
    public void LaunchApplication(NewSessionReq req)
    {
      _capabilities = req.desiredCapabilities;
      _application = _applicationManager.LaunchApplication(_capabilities);
      RebuildCache();
    }

    public IElement FindElement(Locator locator)
    {
      // check cache first
      var cached = _cache.GetOrDefault(locator.Strategy, locator.Value);
      if (cached != null)
      {
        return cached;
      }

      _logger.LogError("Miss cache");

      var element = GetApplicationRoot().FindElement(locator, _msTimeout);
      _cache.AddElement(element);
      RebuildCache();
      return element;
    }

    public IEnumerable<IElement> FindElements(Locator locator)
    {
      var elements = GetApplicationRoot().FindElements(locator, _msTimeout);
      foreach (var element in elements)
      {
        _cache.AddElement(element);
      }
      return elements;
    }

    public IElement FindElement(string elementId)
    {
      if (String.IsNullOrEmpty(elementId))
      {
        throw new ElementNotFound("element id is empty");
      }
      var element = _cache.GetOrDefault(LocatorStrategy.Id, elementId);
      if (element == null)
      {
        throw new ElementNotFound("Didn't found the element in cache");
      }
      if (element.IsStaleElement())
      {
        _cache.RemoveElement(LocatorStrategy.Id, element.GetId());
        throw new StaleElementException("stale element " + element.GetId());
      }
      return element;
    }

    public IElement FindElement(string startElement, Locator locator)
    {
      return FindElement(startElement).FindElement(locator, _msTimeout);
    }

    public IEnumerable<IElement> FindElements(string startElement, Locator locator)
    {
      return FindElement(startElement).FindElements(locator, _msTimeout);
    }

    public string GetSource()
    {
      var xmlDoc = GetApplicationRoot().GetXmlDocument();
      StringWriter sw = new StringWriter();
      XmlTextWriter xw = new XmlTextWriter(sw);
      xmlDoc.WriteTo(xw);
      return  sw.ToString();
    }

    public void SetImplicitTimeout(int msTimeout)
    {
      _msTimeout = msTimeout;
    }

    public IElement GetApplicationRoot()
    {
      return _application.GetApplicationRoot();
    }

    public bool IsElementEquals(string id, string other)
    {
      return id == other || FindElement(id).ElementEquals(FindElement(other));
    }

    public IElement GetFocusedElement()
    {
      var element = GetApplicationRoot().GetFocusedElement();
      _cache.AddElement(element);
      return element;
    }

    public void RelaunchApplication()
    {
      LaunchApplication(new NewSessionReq() { desiredCapabilities = _capabilities });
      RebuildCache();
    }

    private IElement GetWindowFromCache(string windowId)
    {
      return _cache.GetWindowOrDefault(windowId);
    }

    private void SaveWindowToCache(string key, IElement element)
    {
      _cache.AddWindow(key, element);
    }

    public string GetWindowHandle()
    {
      IElement e = GetApplicationRoot().GetWindowHandle();
      var uiObject = (UIObject)(e.GetUIObject());
      var key = uiObject.NativeWindowHandle.ToString();
      SaveWindowToCache(key, e);
      return key;
    }

    public IEnumerable<string> GetWindowHandles()
    {
      IEnumerable<IElement> handles = GetApplicationRoot().GetWindowHandles();
      return handles.Select(e => {
        var uiObject = (UIObject)(e.GetUIObject());
        var key = uiObject.NativeWindowHandle.ToString();
        SaveWindowToCache(key, e);
        return key;
      });
    }

    public IElement GetWindow(string windowId)
    {
      if (windowId == "current")
      {
        return GetApplicationRoot();
      }

      var cached = GetWindowFromCache(windowId);
      if (cached != null)
      {
        return cached;
      }
      else
      {
        var window = GetApplicationRoot().GetWindow(windowId);
        SaveWindowToCache(windowId, window);
        return window;
      }
    }
  }
}
