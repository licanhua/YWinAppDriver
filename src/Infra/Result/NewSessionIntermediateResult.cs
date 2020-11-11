// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WinAppDriver.Infra.Result
{
  public class NewSessionIntermediateResult
  {
    [Required]
    public string sessionId;

    [Required]
    public Capabilities capabilities;
  }
}
