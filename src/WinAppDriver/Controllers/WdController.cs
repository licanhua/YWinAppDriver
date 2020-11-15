using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinAppDriver.Controllers;

namespace WinAppDriver
{
  [Route("wd/hub")]
  [ApiController]
  public class WdController : WinAppDriverControllerBase
  {
    [HttpPost]
    [Route("{**path}")]
    public IActionResult NotSupported(string path)
    {
      return ReplyFail(400, Infra.ResponseStatusCode.UnsupportedOperation, "You are trying to use wd/hub, but you didn't enable this endpoint, make sure you enabled app.UsePathBase(\"/ wd/hub\") in code");
    }

    [HttpGet]
    [Route("{**path}")]
    public IActionResult GetNotSupported(string path)
    {
      return NotSupported(path);
    }

    [HttpDelete]
    [Route("{**path}")]
    public IActionResult DeleteNotSupported(string path)
    {
      return NotSupported(path);
    }
  }
}
