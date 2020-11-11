// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WinAppDriver.Infra
{
  public class SessionOkResponse
  {
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string sessionId;
    public int status;
    public object value;
  }

  public class SessionFailDetail
  {
    public string error;
    public string message;
  }

  public class SessionFailResponse
  {
    public int status;
    public SessionFailDetail value;
  }
}
