// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WinAppDriver
{
  public class NewSessionReq
  {
    [JsonProperty(Required = Required.DisallowNull)]
    public Capabilities desiredCapabilities;
  }

  public class ImplicitTimeoutReq
  {
    [JsonProperty(Required = Required.DisallowNull)]
    public string type;

    [JsonProperty(Required = Required.DisallowNull)]
    public double ms;
  }
}
