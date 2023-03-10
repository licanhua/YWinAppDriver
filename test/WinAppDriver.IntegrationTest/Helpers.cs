// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WinAppDriver.Infra;
using WinAppDriver.Infra.Request;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.IntegrationTest
{
  public class SessionResponse<T>
  {
    public HttpStatusCode statusCode;
    public string sessionId;
    public int status;
    public T value;  
  }

  class AppIds
  {
    public const string Root = "Root";
    public const string Calculator = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";
    public const string Edge = "Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge";
    public const string AlarmClock = "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App";
    public const string Notepad = "c:\\windows\\system32\\notepad.exe";
    public const string WinVer = "C:\\Windows\\System32\\winver.exe";
  }
  class Helpers
  {
    public static async Task<SessionResponse<V>> TranslateResult<V>(HttpResponseMessage response)
    {
      var body = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<SessionResponse<V>>(body);
      result.statusCode = response.StatusCode;
      return result;
    }
    public static async Task<SessionResponse<V>> PostMessage<V, U>(HttpClient client, string url, U obj)
    {
      var response = await client.PostAsync(url, ToStringContent(obj));
      return await TranslateResult<V>(response);
    }

    public static Task<SessionResponse<V>> PostSessionMessage<V, U>(HttpClient client, string sessionId, string url, U obj)
    {
      return PostMessage<V, U>(client, "session/" + sessionId + "/" + url, obj);
    }

    public static async Task<SessionResponse<V>> GetMessage<V>(HttpClient client, string url)
    {
      var response = await client.GetAsync(url);
      return await TranslateResult<V>(response);
    }

    public static Task<SessionResponse<V>> GetSessionMessage<V>(HttpClient client, string sessionId, string url)
    {
      return GetMessage<V>(client, "session/" + sessionId + "/" + url);
    }
    public static StringContent ToStringContent<T>(T obj)
    {
      return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
    }

    public static async Task<T> FromBody<T>(HttpResponseMessage response)
    {
      var body = await response.Content.ReadAsStringAsync();
      Console.WriteLine(body);
      return JsonConvert.DeserializeObject<T>(body);
    }

    public static Task<string> CreateNewSession(HttpClient client, string app)
    {
      return CreateNewSession(client, new NewSessionReq()
      {
        desiredCapabilities = new Capabilities()
        {
          app = app
        }
      });

    }

    public static async Task<string> CreateNewSession(HttpClient client, NewSessionReq req)
    {
      var response = await Helpers.PostMessage<object, NewSessionReq>(client, "session", req);

      return response.sessionId;
    }

    public static async Task<HttpResponseMessage> DeletSession(HttpClient client, string sessionId)
    {
      var response = await client.DeleteAsync("session/" + sessionId);
      return response;
    }

    public static Task<SessionResponse<FindElementResult>> FindElement(HttpClient client, string sessionId, FindElementReq req)
    {
      return PostSessionMessage<FindElementResult, FindElementReq>(client, sessionId, "element", req);
    }

    public static Task<SessionResponse<FindElementsResult>> FindElements(HttpClient client, string sessionId, FindElementsReq req)
    {
      return PostSessionMessage<FindElementsResult, FindElementsReq>(client, sessionId, "elements", req);
    }

    public static Task<SessionResponse<object>> ActivateWindow(HttpClient client, string sessionId, string windowId)
    {
      return PostSessionMessage<object, ActivateWindowReq>(client, sessionId, "window", new ActivateWindowReq() { name = windowId }) ;
    }

    public static Task<SessionResponse<string>> GetTitle(HttpClient client, string sessionId)
    {
      return GetSessionMessage<string>(client, sessionId, "title");
    }

    public static async Task<SessionResponse<object>> DeleteWindow(HttpClient client, string sessionId)
    {
      var response = await client.DeleteAsync("session/" + sessionId + "/window");
      return await Helpers.TranslateResult<object>(response);
    }

    public static Task<SessionResponse<object>> PostElementMessage<U>(HttpClient client, string sessionId, string elementId, string action, U body)
    {
      return PostSessionMessage<object, U>(client, sessionId, "element/" + elementId + "/" + action, body);
    }

    public static Task<SessionResponse<object>> GetElementMessage(HttpClient client, string sessionId, string elementId, string action)
    {
      return GetSessionMessage<object>(client, sessionId, "element/" + elementId + "/" + action);
    }
  }
}
