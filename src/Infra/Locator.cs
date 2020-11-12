// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
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
    XPath,
  }

  public class Consts
  {
    public const string ID = "id";
    public const string ACCESSIBILITY_ID = "accessibility id";
    public const string CLASS_NAME = "class name";
    public const string NAME = "name";
    public const string XPATH = "xpath";
    public const string TAG_NAME = "tag name";
  }
  public class Locator
  {
    public LocatorStrategy Strategy { get; set; }
    public string Value { get; set; }

    public static Locator BuildLocator(string strategy, string value)
    {
      Locator locator = new Locator() { Value = value, Strategy = LocatorStrategy.NotSupported };

      if (strategy == Consts.ACCESSIBILITY_ID)
      {
        locator.Strategy = LocatorStrategy.AccessibilityId;
      }
      else if (strategy == Consts.CLASS_NAME)
      {
        locator.Strategy = LocatorStrategy.ClassName;
      }
      else if (strategy == Consts.NAME)
      {
        locator.Strategy = LocatorStrategy.Name;
      }
      else if (strategy == Consts.ID)
      {
        locator.Strategy = LocatorStrategy.Id;
      }
      else if (strategy == Consts.TAG_NAME)
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
