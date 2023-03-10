using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json.Linq;
using WinAppDriver.Infra;
using WinAppDriver.Infra.Request;
using WinAppDriver.Infra.Result;
using Xunit;

namespace WinAppDriver.IntegrationTest
{
  [Collection("Sequential")]
  public class DeviceTest: IDisposable
  {
    public readonly string _tempDirectory;
    public readonly string _uniqueTempDirectory;

    public DeviceTest()
    {
      _tempDirectory = Path.GetTempPath();
      _uniqueTempDirectory = Path.Combine(_tempDirectory, Guid.NewGuid().ToString());

      Directory.CreateDirectory(_uniqueTempDirectory);
    }

    [Theory]
    [InlineData("file.bin")]
    [InlineData("foo\\bar\\file.bin")]
    public async Task Test_PushFile_UploadFileToRemote(string partialFilePath)
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, AppIds.WinVer);
        sessionId.Should().NotBeNullOrEmpty();

        try
        {
          var fileContent = Guid.NewGuid().ToByteArray();
          var filePath = Path.Combine(_uniqueTempDirectory, partialFilePath);

          var res = await Helpers.PostSessionMessage<object, PathFileReq>(client, sessionId, "appium/device/push_file",
            new PathFileReq() { data = Convert.ToBase64String(fileContent), path = filePath });

          res.statusCode.Should().Be(HttpStatusCode.OK);

          File.Exists(filePath).Should().BeTrue();
          (await File.ReadAllBytesAsync(filePath)).Should().BeEquivalentTo(fileContent);
        }
        finally
        {

          await Helpers.DeletSession(client, sessionId);
        }
      }
    }

    [Theory]
    [InlineData("", "content", "Invalid path: Null or whitespace")] // path empty 
    [InlineData("C:\\file.bin", "", "Invalid file content: Null or whitespace")] // empty content
    [InlineData("C:\\file.bin", "???????", "The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.")] // invalid content
    public async Task Test_PushFile_InvalidPayload(string filePath, string fileContent, string errorMessage)
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, AppIds.WinVer);
        sessionId.Should().NotBeNullOrEmpty();

        try
        {
          var res = await Helpers.PostSessionMessage<object, PathFileReq>(client, sessionId, "appium/device/push_file",
            new PathFileReq() { data = fileContent, path = filePath });

          res.statusCode.Should().Be(HttpStatusCode.InternalServerError);
          res.value.Should().BeOfType<JObject>();

          var resValue = (JObject)res.value;
          
          resValue.Value<string>("error").Should().BeEquivalentTo("UnknownError");
          resValue.Value<string>("message").Should().BeEquivalentTo(errorMessage);
        }
        finally
        {
          await Helpers.DeletSession(client, sessionId);
        }
      }
    }

    [Fact]
    public async Task Test_PullFile_DownloadFileFromRemote()
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, AppIds.WinVer);
        sessionId.Should().NotBeNullOrEmpty();

        try
        {
          var fileContent = Guid.NewGuid().ToByteArray();
          var filePath = Path.Combine(_uniqueTempDirectory, "file.bin");
          File.WriteAllBytes(filePath, fileContent);

          var res = await Helpers.PostSessionMessage<object, PathFileReq>(client, sessionId, "appium/device/pull_file",
            new PathFileReq() { path = filePath });

          res.statusCode.Should().Be(HttpStatusCode.OK);
          res.value.Should().BeEquivalentTo(Convert.ToBase64String(fileContent));
        }
        finally
        {

          await Helpers.DeletSession(client, sessionId);
        }
      }
    }

    [Theory]
    [InlineData("", "Invalid path: Null or whitespace")] // path empty 
    [InlineData("C:\\file.bin", "The requested file doesn't exist.")] // file doesn't exist
    public async Task Test_PullFile_InvalidPayload(string filePath, string errorMessage)
    {
      using (var client = new TestClientProvider().Client)
      {
        var sessionId = await Helpers.CreateNewSession(client, AppIds.WinVer);
        sessionId.Should().NotBeNullOrEmpty();

        try
        {
          var res = await Helpers.PostSessionMessage<object, PathFileReq>(client, sessionId, "appium/device/pull_file",
            new PathFileReq() { path = filePath });

          res.statusCode.Should().Be(HttpStatusCode.InternalServerError);
          res.value.Should().BeOfType<JObject>();

          var resValue = (JObject)res.value;

          resValue.Value<string>("error").Should().BeEquivalentTo("UnknownError");
          resValue.Value<string>("message").Should().BeEquivalentTo(errorMessage);
        }
        finally
        {
          await Helpers.DeletSession(client, sessionId);
        }
      }
    }

    public void Dispose()
    {
      if (Directory.Exists(_uniqueTempDirectory))
      {
        Directory.Delete(_uniqueTempDirectory, true);
      }
    }
  }
}
