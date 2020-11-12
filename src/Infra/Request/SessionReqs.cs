// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
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

  public class SetImplicitTimeoutReq
  {
    public string type;

    [JsonProperty(Required = Required.DisallowNull)]
    public double ms;
  }
}
