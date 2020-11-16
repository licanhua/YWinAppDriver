using Microsoft.Windows.Apps.Test.Automation;
using Microsoft.Windows.Apps.Test.Foundation;
using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Microsoft.Windows.Apps.Test.Foundation.Patterns;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using WinAppDriver.Infra.Communication;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Infra
{
  public enum TouchAction
  { 
    Click,
    DoubleClick,
    PressAndHold,
    Down,
    Up,
    Move,
  }

  public enum MouseAction
  {
    Up,
    Down,
    Click,
    DoubleClick,
  }

  static class Extension
  {
    // Convert the string to camel case.
    public static string ToCamelCase(this string the_string)
    {
      // If there are 0 or 1 characters, just return the string.
      if (the_string == null || the_string.Length < 2)
        return the_string;

      // Split the string into words.
      string[] words = the_string.Split(
          new char[] {' '},
          StringSplitOptions.RemoveEmptyEntries);

      // Combine the words.
      string result = words[0].ToLower();
      for (int i = 1; i < words.Length; i++)
      {
        result +=
            words[i].Substring(0, 1).ToUpper() +
            words[i].Substring(1);
      }

      return result;
    }

    // Capitalize the first character + ToCamelCase
    public static string ToProperCase(this string the_string)
    {
      var camelCase = ToCamelCase(the_string);
      if (string.IsNullOrEmpty(camelCase)) return camelCase;
      return char.ToUpper(camelCase[0]) + camelCase.Substring(1);
    }

    static Point ToPoint(object x, object y)
    {
      if (x == null || y == null)
      {
        throw new InvalidArgumentException("x or y is null");
      }
      return new Point() { X = (int)Double.Parse(x.ToString()), Y = (int)Double.Parse(y.ToString()) };
    }

    static Point ToPoint(double x, double y)
    {
      return new Point() { X = (int)x, Y = (int)y };
    }

    public static void MouseMoveTo(this IElement element, object x, object y)
    {
      if (element == null)
      {
        PointerInput.Move(ToPoint(x, y));
      }
      else
      {
        UIObject uiObject = (UIObject)element.GetUIObject();
        if (x == null || y == null)
        {
          PointerInput.Move(uiObject);
        }
        else
        {
          PointerInput.Move(uiObject, Double.Parse(x.ToString()), Double.Parse(y.ToString()));
        }

      }
    }

    private static PointerButtons ToPointerButton(this MouseButton button)
    {
      if (button == MouseButton.LEFT)
      {
        return PointerButtons.Primary;
      }
      else if (button == MouseButton.RIGHT)
      {
        return PointerButtons.Secondary;
      }
      else return PointerButtons.Middle;
    }

    public static void MouseAction(this MouseButton button, string action)
    {
      MouseAction mouseAction = (MouseAction)Enum.Parse(typeof(MouseAction), action);
      if (mouseAction == Infra.MouseAction.Down)
      {
        PointerInput.Press(ToPointerButton(button));
      }
      else if (mouseAction == Infra.MouseAction.Up)
      {
        PointerInput.Release(ToPointerButton(button));
      }
      else if (mouseAction == Infra.MouseAction.Click)
      {
        PointerInput.Click(button.ToPointerButton(), 1);
      }
      else if (mouseAction == Infra.MouseAction.DoubleClick)
      {
        PointerInput.Click(button.ToPointerButton(), 2);
      }
      else
      {
        throw new InvalidArgumentException();
      }
    }

  
    public static void TouchActionOnElement(this IElement element, string action)
    {
      UIObject uiObject = (UIObject)element.GetUIObject();

      TouchAction touchAction = (TouchAction)Enum.Parse(typeof(TouchAction), action);
      if (touchAction == TouchAction.Click)
      {
        uiObject.Tap();
      }
      else if (touchAction == TouchAction.DoubleClick)
      {
        uiObject.DoubleTap();
      }
      else if (touchAction == TouchAction.PressAndHold)
      {
        uiObject.TapAndHold();
      }
      else
      {
        throw new InvalidArgumentException();
      }
    }

    public static void TouchUpDownMove(this XYReq req, string action)
    {
      TouchAction touchAction = (TouchAction)Enum.Parse(typeof(TouchAction), action);
      if (touchAction == TouchAction.Up)
      {
        using (InputController.Activate(PointerInputType.MultiTouch))
        {
          PointerInput.Move(ToPoint(req.x, req.y));
          PointerInput.Release(PointerButtons.Primary);
        }

      }
      else if (touchAction == TouchAction.Move)
      {
        using (InputController.Activate(PointerInputType.MultiTouch))
        {
          PointerInput.Move(ToPoint(req.x, req.y));
        }
      }
      else if (touchAction == TouchAction.Down)
      {
        using (InputController.Activate(PointerInputType.MultiTouch))
        {
          PointerInput.Move(ToPoint(req.x, req.y));
          PointerInput.Press(PointerButtons.Primary);
        }

      }
      else throw new InvalidArgumentException();
    }

    public static UIObject UI(this IElement element)
    {
      return (UIObject)element.GetUIObject();
    }
    public static XYResult GetWindowPosition(this IElement window)
    {
      var rect = window.UI().BoundingRectangle;
      return new XYResult() { x = rect.X, y = rect.Y };
    }

    public static SizeResult GetWindowSize(this IElement window)
    {
      var rect = window.UI().BoundingRectangle;
      return new SizeResult() { width = rect.Width, height = rect.Height };
    }

    public static int ToInt(this double value)
    {
      return (int)value;
    }

    public static WindowImplementation Window(this IElement window)
    {
      return new WindowImplementation(window.UI());
    }

    public static void SetWindowPosition(this IElement window, XYReq req)
    {
      window.Window().SetWindowVisualState(WindowVisualState.Normal); // can't move maximize and minimize window
      TransformImplementation transform = new TransformImplementation(window.UI());
      transform.Move(req.x, req.x);
    }

    public static void SetWindowSize(this IElement window, SizeReq req)
    {
      window.Window().SetWindowVisualState(WindowVisualState.Normal); // can't move maximize and minimize window
      TransformImplementation transform = new TransformImplementation(window.UI());
      transform.Resize(req.width, req.height);
    }

    public static void MaximizeWindow(this IElement window)
    {
      window.Window().SetWindowVisualState(WindowVisualState.Maximized);
    }
  }
}
