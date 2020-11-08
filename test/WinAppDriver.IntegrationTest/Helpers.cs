// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
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
  class Helpers
  {
    public static Task<HttpResponseMessage> PostMessage<T>(HttpClient client, string url, T obj)
    {
      return client.PostAsync(url, ToStringContent(obj));
    }

    public static StringContent ToStringContent<T>(T obj)
    {
      return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
    }

    public static async Task<T> FromBody<T>(HttpResponseMessage response)
    {
      return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
    }

    public static async Task<string> CreateNewSession(HttpClient client, string app)
    {
      var response = await Helpers.PostMessage(client, "session", new NewSessionReq()
      {
        desiredCapabilities = new Capabilities()
        {
          app = app
        }
      });

      response.StatusCode.Should().Be(HttpStatusCode.OK);

      var sessionOkResponse = await Helpers.FromBody<SessionOkResponse>(response);
      sessionOkResponse.value.Should().NotBeNull();
      
      sessionOkResponse.sessionId.Should().NotBeNullOrEmpty();
      return sessionOkResponse.sessionId;
    }

    public static async Task<HttpResponseMessage> DeletSession(HttpClient client, string sessionId)
    {
      var response = await client.DeleteAsync("session/" + sessionId);
      return response;
    }

    public static async Task<FindElementResult> FindElement(HttpClient client, string sessionId, FindElementReq req)
    {
      var response = await PostMessage(client, "session/" + sessionId + "/element", req);

      response.StatusCode.Should().Be(HttpStatusCode.OK);

      var sessionOkResponse = await Helpers.FromBody<SessionOkResponse>(response);
      sessionOkResponse.value.Should().NotBeNull();

      var value = sessionOkResponse.value.ToString();
      return JsonConvert.DeserializeObject<FindElementResult>(value);
    }

    public static async Task<FindElementsResult> FindElements(HttpClient client, string sessionId, ElementsReqs req)
    {
      var response = await PostMessage(client, "session/" + sessionId + "/elements", req);

      response.StatusCode.Should().Be(HttpStatusCode.OK);

      var sessionOkResponse = await Helpers.FromBody<SessionOkResponse>(response);
      sessionOkResponse.value.Should().NotBeNull();

      var value = sessionOkResponse.value.ToString();
      return JsonConvert.DeserializeObject<FindElementsResult>(value);
    }
  }
}
