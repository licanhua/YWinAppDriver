// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

namespace WinAppDriver.Controllers
{
  using Microsoft.AspNetCore.Mvc;
  using System;
  using System.Collections.Generic;
  using WinAppDriver.Infra;

  public abstract class WinAppDriverControllerBase : ControllerBase
  {
    protected IActionResult ReplyOk(string sessionId, object value)
    {
      return Ok(
        new SessionOkResponse() { sessionId = sessionId, value = value }
        );
    }

    protected IActionResult ReplyFail(int httpCode, ResponseStatusCode statusCode, string message = null)
    {
      return StatusCode(httpCode,
       new SessionFailResponse() 
       { 
         status = (int)statusCode , 
         value = new SessionFailDetail() { error = statusCode.ToString(), message = message } 
      });
    }
  }
}
