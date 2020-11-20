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
  public class Capabilities
  {
    [JsonProperty(Required = Required.DisallowNull)]
    public string app;
    public string attachToTopLevelWindowClassName;
    public string appArguments;
    public string appWorkingDir;
    public string forceMatchAppTitle;
    public string forceMatchClassName;
  }
}
