// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using WinAppDriver.Infra.Communication;
using WinAppDriver.Infra.Request;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra.CommandHandler
{

  public class FindElementsHandler : SessionCommandHandlerBase<ElementsReqs, FindElementsResult>
  {
    protected override FindElementsResult ExecuteSessionCommand(ISessionManager sessionManager, ISession session, ElementsReqs req, string elementId)
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
}