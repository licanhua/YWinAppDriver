// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

namespace WinAppDriver.Infra
{

  public enum LocatorStrategy
  {
    NotSupported,
    AccessibilityId,
    ClassName,
    Name,
    Id,
    TagName,
    //Id,
    //TagName,
    //XPath,
  }

  public class Locator
  {
    public LocatorStrategy Strategy { get; set; }
    public string Value { get; set; }

    public static Locator BuildLocator(string strategy, string value)
    {
      Locator locator = new Locator() { Value = value, Strategy = LocatorStrategy.NotSupported };

      if (strategy == "accessibility id")
      {
        locator.Strategy = LocatorStrategy.AccessibilityId;
      }
      else if (strategy == "class name")
      {
        locator.Strategy = LocatorStrategy.ClassName;
      }
      else if (strategy == "name")
      {
        locator.Strategy = LocatorStrategy.Name;
      }
      else if (strategy == "id")
      {
        locator.Strategy = LocatorStrategy.Id;
      }
      else if (strategy == "tag name")
      {
        locator.Strategy = LocatorStrategy.TagName;
      }
      else
      {
        throw new LocatorNotSupported();
      }
      return locator;
    }
  }
}
