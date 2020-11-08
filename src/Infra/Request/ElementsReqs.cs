// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra.Request
{
  public class ElementsReqs
  {
    [JsonProperty("using", Required = Required.DisallowNull)]
    public string strategy;

    [JsonProperty(Required = Required.DisallowNull)]
    public string value;
  }

  public class FindElementReq
  {
    [JsonProperty("using", Required = Required.DisallowNull)]
    public string strategy;

    [JsonProperty(Required = Required.DisallowNull)]
    public string value;
  }
}
