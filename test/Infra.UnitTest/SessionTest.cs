// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinAppDriver.Infra;
using WinAppDriver.Infra.CommandHandler;
using WinAppDriver.Infra.Communication;
using WinAppDriver.Infra.Request;
using Xunit;

namespace Infra.UnitTest
{
  public class SessionTest
  {
    [Fact]
    public void FindElement_FindNoElement()
    {
      ISession _session = Helpers.CreateSession();
      Assert.ThrowsAny<ElementNotFound>(() => _session.FindElement(Helpers.EmptyLocator));
    }

    [Fact]
    public void FindElement_FoundOneElement()
    {
      ISession _session = Helpers.CreateSession();
      var element = _session.FindElement(Helpers.RootLocator);
      element.Should().NotBeNull();
    }

    [Fact]
    public void FindElements_FoundEmptyElement()
    {
      ISession _session = Helpers.CreateSession();
      var elements = _session.FindElements(Helpers.EmptyLocator);
      elements.ToList().Count.Should().Be(0);
    }

    [Fact]
    public void FindElements_FoundTwoElements()
    {
      ISession _session = Helpers.CreateSession();
      var elements = _session.FindElements(Helpers.TwoElementLocator);
      elements.Count().Should().Be(2);
    }

    [Fact]
    public void FindElementFromElement_WithValidId()
    {
      ISession _session = Helpers.CreateSession();
      var element = _session.FindElement(Helpers.RootLocator);
      var id = element.GetId();

      var sub = _session.FindElement(id, Helpers.TwoElementLocator);
      sub.GetId().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void FindElementFromElement_WithInvalidId()
    {
      ISession _session = Helpers.CreateSession();
      var element = _session.FindElement(Helpers.RootLocator);
      Assert.Throws<ElementNotFound>(() => _session.FindElement(element.GetId() + "Invalid", Helpers.TwoElementLocator));
    }

    [Fact]
    public void FindElementsFromElement_WithInvalidId()
    {
      ISession _session = Helpers.CreateSession();
      var element = _session.FindElement(Helpers.RootLocator);

      Assert.Throws<ElementNotFound>(() => _session.FindElements(element.GetId() + "Invalid", Helpers.TwoElementLocator));

    }

    [Fact]
    public void FindElementsFromElement_WithValidId()
    {
      ISession _session = Helpers.CreateSession();
      var element = _session.FindElement(Helpers.RootLocator);

      var elements = _session.FindElements(element.GetId(), Helpers.TwoElementLocator);
      elements.Count().Should().Be(2);
    }

  }
}
