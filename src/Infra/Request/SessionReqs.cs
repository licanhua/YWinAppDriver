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

  public class ActivateWindowReq
  {
    [JsonProperty(Required = Required.DisallowNull)]
    public string name;
  }

  public class MoveToReq
  {
    public string element;

    public object xoffset;
    public object yoffset;
  }

  [Flags]
  public enum MouseButton
  {
    LEFT = 0,
    MIDDLE = 1, 
    RIGHT = 2
  }
  public class MouseActionReq
  {
    public MouseButton button;
  }

  public class ElementReq
  {
    [JsonProperty(Required = Required.DisallowNull)]
    public string element;
  }

  public class XYReq
  {
    public double x;
    public double y;
  }

  public class SizeReq
  {
    public double width;

    public double height;
  }

  public class PathFileReq
  {
    public string path { get; set; }
    public string data { get; set; }
  }
}