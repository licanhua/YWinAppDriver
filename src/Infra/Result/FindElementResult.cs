// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra.Result
{
  public class FindElementResult
  {
    [JsonProperty("ELEMENT", Required = Required.DisallowNull)]
    public string element;
  }

  public class FindElementsResult: List<FindElementResult>
  { 
  }
}
