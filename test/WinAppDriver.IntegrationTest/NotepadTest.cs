// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WinAppDriver.Infra;
using WinAppDriver.Infra.Request;
using Xunit;

namespace WinAppDriver.IntegrationTest
{
  [Collection("Sequential")]
  public class NotepadTest
  {
    string TextEditor = "Text Editor";
    string Editor = "Edit";

    [Fact]
    public async void NotepadTestAll()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, AppIds.Notepad);

        sessionId.Should().NotBeNullOrEmpty();

        var element = await Helpers.FindElement(client, sessionId, new FindElementReq()
        {
          strategy = Consts.NAME,
          value = TextEditor
        });
        element.statusCode.Should().Be(HttpStatusCode.OK);
        string elementId = element.value.element;
        // set the value
        var response = await Helpers.PostElementMessage(client, sessionId, elementId, "value", new Dictionary<string, object>()
        {{"value", new List<string>() { "123", "456" }
        } });
        response.statusCode.Should().Be(HttpStatusCode.OK);

        // get the value
        response = await Helpers.GetElementMessage(client, sessionId, elementId, "text");
        response.statusCode.Should().Be(HttpStatusCode.OK);
        response.value.ToString().Should().Be("123456");

        await Helpers.DeletSession(client, sessionId);
      }
    }
  }
}
