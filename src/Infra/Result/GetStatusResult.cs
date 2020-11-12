using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra.Result
{
  public class GetStatusResult
  {
    [JsonProperty("build.version")]
    public string buildVersion;

    [JsonProperty("build.time")]
    public string buildTime;

    [JsonProperty("build.revision")]
    public string buildRevision;

    [JsonProperty("os.arch")]
    public string osArch;

    [JsonProperty("os.name")]
    public string osName;

    [JsonProperty("version")]
    public string osVersion;
  }
}
