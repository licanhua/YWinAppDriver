// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinAppDriver.Infra.Communication;
using WinAppDriver.Infra.Request;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra.CommandHandler
{

  public class FindElementsHandler : SessionCommandHandlerBase<FindElementsReq, FindElementsResult>
  {
    protected override FindElementsResult ExecuteSessionCommand(ISessionManager sessionManager, ISession session, FindElementsReq req, string elementId)
    {
      var locator = Locator.BuildLocator(req.strategy, req.value);
      FindElementsResult result = new FindElementsResult();
      IEnumerable<IElement> elements = elementId == null ? session.FindElements(locator) : session.FindElements(elementId, locator);

      foreach (var element in elements)
      {
        result.Add(new FindElementResult()
        {
          element = element.GetId()
        });
      };

      return result;
    }
  }

  public class FindElementHandler : SessionCommandHandlerBase<FindElementReq, FindElementResult>
  {
    protected override FindElementResult ExecuteSessionCommand(ISessionManager sessionManager, ISession session, FindElementReq req, string elementId)
    {
      var locator = Locator.BuildLocator(req.strategy, req.value);
      return new FindElementResult()
      {
        element = elementId == null ? session.FindElement(locator).GetId() : session.FindElement(elementId, locator).GetId()
      };
    }
  }

  public class ElementEqualsHandler : SessionCommandHandlerBase<string, bool>
  {
    protected override bool ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string req, string elementId)
    {
      return session.IsElementEquals(req, elementId);
    }
  }

  public class SetValueHandler : SessionCommandHandlerBase<SetValueReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, SetValueReq req, string elementId)
    {
      StringBuilder sb = new StringBuilder();
      foreach (var s in req.value)
      {
        sb.Append(s.ToString());
      }

      // allow to depress all modified key
      // refer https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol#sessionsessionidelementidvalue
      sb.Append(KeyboardHelper.NULL); 
      
      session.FindElement(elementId).SendKeys(sb.ToString());

      return null;
    }
  }
}