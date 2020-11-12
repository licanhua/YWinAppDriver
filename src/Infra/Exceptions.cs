// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra
{
  public enum ResponseStatusCode
  {
    // https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol#response-status-codes
    Success = 0,
    NoSuchDriver = 6,
    NoSuchElement = 7,
    NoSuchFrame = 8,
    UnknownCommand = 9,
    StaleElementReference = 10,
    ElementNotVisible = 11,
    InvalidElementState = 12,
    UnknownError = 13,
    ElementIsNotSelectable = 15,
    JavaScriptError = 17,
    XPathLookError = 19,
    Timeout = 21,
    NoSucnWindow = 23,
    InvalidCookieDomain = 24,
    UnableToSetCookie = 25,
    UnexpectedAlertOpen = 26,
    NoAlertOpenError = 27,
    ScriptTimeout = 28,
    InvalidElementCoordinates = 29,
    IMENotAvailable = 30,
    IMEEngineActivationFailed = 31,
    InvalidSelector = 32,
    SessionNotCreateException = 33,
    MoveTargetOutOfBounds = 34,

    // no in selenium json wire
    ElementClickIntercepted = 100,
    ElementNotInteractable = 101,
    InsecureCertificate = 102,
    InvalidArgument = 103,
    InvalidSessionId = 104,
    NoSuchAlert = 105,
    NoSuchCookie = 106,
    UnableToCaptureScreen = 107,
    UnknownMethod = 108,
    UnsupportedOperation = 109,
  }

  public class SessionException : Exception {

    // https://www.w3.org/TR/webdriver/#dfn-error-code
    private static Dictionary<ResponseStatusCode, int> _map = new Dictionary<ResponseStatusCode, int>() {
      { ResponseStatusCode.ElementNotInteractable, 400},
      { ResponseStatusCode.ElementClickIntercepted, 400},
      { ResponseStatusCode.InsecureCertificate, 400},
      { ResponseStatusCode.InvalidArgument, 400},
      { ResponseStatusCode.InvalidCookieDomain, 400},
      { ResponseStatusCode.InvalidElementState, 400},
      { ResponseStatusCode.InvalidSelector, 400},
      { ResponseStatusCode.InvalidSessionId, 404},
      { ResponseStatusCode.JavaScriptError, 500},
      { ResponseStatusCode.MoveTargetOutOfBounds, 500},
      { ResponseStatusCode.NoSuchAlert, 404},
      { ResponseStatusCode.NoSuchCookie, 404},
      { ResponseStatusCode.NoSuchElement, 404},
      { ResponseStatusCode.NoSuchFrame, 404},
      { ResponseStatusCode.NoSucnWindow, 404},
      { ResponseStatusCode.ScriptTimeout, 500},
      { ResponseStatusCode.SessionNotCreateException, 500},
      { ResponseStatusCode.StaleElementReference, 404},
      { ResponseStatusCode.Timeout, 500},
      { ResponseStatusCode.UnableToSetCookie, 500},
      { ResponseStatusCode.UnableToCaptureScreen, 500},
      { ResponseStatusCode.UnexpectedAlertOpen, 500},
      { ResponseStatusCode.UnknownCommand, 404},
      { ResponseStatusCode.UnknownError, 500},
      { ResponseStatusCode.UnknownMethod, 400},
      { ResponseStatusCode.UnsupportedOperation, 500},
    };
    
    private ResponseStatusCode _statusCode;
    private string _message;
    public SessionException(ResponseStatusCode statusCode, string message = null) {
      _statusCode = statusCode;
      _message = message;
    }

    public int HttpCode => _map.GetValueOrDefault(_statusCode, 500);
    public ResponseStatusCode Status => _statusCode;
    public string Error => _statusCode.ToString();
    public string ErrorMessage => _message;
  }

  public class SessionNotCreatedException : SessionException
  {
    public SessionNotCreatedException(string message = null): base(ResponseStatusCode.SessionNotCreateException, message){}
  }

  public class InvalidSessionIdException : SessionException
  {
    public InvalidSessionIdException(string message = null) : base(ResponseStatusCode.InvalidSessionId, message) { }
  }

  public class InvalidArgumentException : SessionException
  {
    public InvalidArgumentException(string message = null) : base(ResponseStatusCode.InvalidArgument, message) { }
  }

  public class UnknownCommandException : SessionException
  {
    public UnknownCommandException(string message = null) : base(ResponseStatusCode.UnknownCommand, message) { }
  }
  public class AppLaunchException : SessionException
  {
    public AppLaunchException(string message = null) : base(ResponseStatusCode.SessionNotCreateException, message) { }
  }

  public class LocatorNotSupported : SessionException
  {   
    public LocatorNotSupported() : base(ResponseStatusCode.UnsupportedOperation, "Locator is not supported") { }
  }

  public class ElementNotFound : SessionException
  {
    public ElementNotFound(string message = null) : base(ResponseStatusCode.NoSuchElement, message) { }

  }

  public class StaleElementException : SessionException
  {
    public StaleElementException(string message = null) : base(ResponseStatusCode.StaleElementReference, message) { }
  }

  public class NotImplementedException : SessionException
  {
    public NotImplementedException(string message = "Interface is not implemented") : base(ResponseStatusCode.UnsupportedOperation, message) { }

  }

  public class ElementOffScreen : SessionException
  {
    public ElementOffScreen() : base(ResponseStatusCode.ElementNotVisible, "Element is offscreen") { }

  }

  public class NoSuchWindow : SessionException
  {
    public NoSuchWindow() : base(ResponseStatusCode.NoSucnWindow) { }
  }
}
