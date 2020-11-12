// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using WinAppDriver.Infra.Communication;

namespace WinAppDriver.Infra
{
  internal class ElementCache
  {
    private Dictionary<string, IElement> _cache = new Dictionary<string, IElement>();
    public void AddElement(IElement element) 
    {
      _cache[element.GetId()] = element;
    }

    public IElement GetOrDefault(string key)
    {
      return _cache.GetValueOrDefault(key);
    }

    public void RemoveElement(string key)
    {
      _cache.Remove(key);
    }
  }

  public class Session : ISession
  {
    private readonly string _sessionId = Guid.NewGuid().ToString();
    private Capabilities _capabilities;
    private IApplicationManager _applicationManager;
    private IApplication _application;
    private int _msTimeout = 0;
    readonly ElementCache _cache = new ElementCache();

    public Session(IApplicationManager applicationManager)
    {
      _applicationManager = applicationManager;
    }

    public void QuitApplication()
    {
      if (_application != null) 
      {
        _application.QuitApplication();
        _application = null;
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

    public void LaunchApplication(NewSessionReq req)
    {
      _capabilities = req.desiredCapabilities;
      _application = _applicationManager.LaunchApplication(_capabilities);
    }

    public IElement FindElement(Locator locator)
    {
      var element = GetApplicationRoot().FindElement(locator, _msTimeout);
      _cache.AddElement(element);
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
      var element = _cache.GetOrDefault(elementId);
      if (element == null)
      {
        throw new ElementNotFound("Didn't found the element in cache");
      }
      if (element.IsStaleElement())
      {
        _cache.RemoveElement(element.GetId());
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
  }
}
