using Microsoft.Windows.Apps.Test.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra.Communication
{
  

  class KeyboardHelper
  {
    // https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol#sessionsessionidelementidvalue
    public const char NULL = '\uE000';
    public const char CANCEL = '\uE001';
    public const char HELP = '\uE002';
    public const char BACKSPACE = '\uE003';
    public const char TAB = '\uE004';
    public const char CLEAR = '\uE005';
    public const char RETURN = '\uE006';
    public const char ENTER = '\uE007'; 
    public const char SHIFT = '\uE008';
    public const char CONTROL = '\uE009'; 
    public const char ALT = '\uE00A';
    public const char PAUSE = '\uE00B';
    public const char ESCAPE = '\uE00C'; 
    public const char SPACE = '\uE00D';
    public const char PAGEUP = '\uE00E'; 
    public const char PAGEDOWN = '\uE00F';
    public const char END = '\uE010'; 
    public const char HOME = '\uE011';
    public const char LEFTARROW = '\uE012'; 
    public const char UPARROW = '\uE013';
    public const char RIGHTARROW = '\uE014';
    public const char DOWNARROW = '\uE015';
    public const char INSERT = '\uE016';
    public const char DELETE = '\uE017';
    public const char SEMICOLON = '\uE018';
    public const char EQUALS = '\uE019';
    public const char NUMPAD0 = '\uE01a';
    public const char NUMPAD1 = '\uE01b';
    public const char NUMPAD2 = '\uE01c';
    public const char NUMPAD3 = '\uE01d';
    public const char NUMPAD4 = '\uE01e';
    public const char NUMPAD5 = '\uE01f';
    public const char NUMPAD6 = '\uE020';
    public const char NUMPAD7 = '\uE021';
    public const char NUMPAD8 = '\uE022';
    public const char NUMPAD9 = '\uE023';
    public const char MULTIPLY = '\uE024';
    public const char ADD = '\uE025';
    public const char SEPARATOR = '\uE026';
    public const char SUBTRACT = '\uE027';
    public const char DECIMAL = '\uE028';
    public const char DIVIDE = '\uE029';
    public const char F1 = '\uE031';
    public const char F2 = '\uE032';
    public const char F3 = '\uE033';
    public const char F4 = '\uE034';
    public const char F5 = '\uE035';
    public const char F6 = '\uE036';
    public const char F7 = '\uE037';
    public const char F8 = '\uE038';
    public const char F9 = '\uE039';
    public const char F10 = '\uE03a';
    public const char F11 = '\uE03b';
    public const char F12 = '\uE03c';
    public static char WIN = '\uE03D';

    //https://github.com/microsoft/microsoft-ui-xaml/blob/c46d28b3e706a24a5b89ec110ac2e93a9a86aa58/test/testinfra/MUXTestInfra/Common/KeyboardHelper.cs#L58
    private static Dictionary<char, string> keyToKeyStringDictionary = new Dictionary<char, string>()
        {
            { ENTER, "{ENTER}" },
            { ESCAPE, "{ESC}" },
            { TAB, "{TAB}" },
            { UPARROW, "{UP}" },
            { DOWNARROW, "{DOWN}" },
            { LEFTARROW, "{LEFT}" },
            { RIGHTARROW, "{RIGHT}" },
            { PAGEUP, "{PGUP}" },
            { PAGEDOWN, "{PGDN}" },
            { HOME, "{HOME}" },
            { END, "{END}" },
            { SPACE, "{SPACE}" },
            { BACKSPACE, "{BACKSPACE}" },
            { F10, "{F10}" },
            { F9, "{F9}" },
            { F8, "{F8}" },
            { F7, "{F7}" },
            { F6, "{F6}" },
            { F5, "{F5}" },
            { F4, "{F4}" },
            { F3, "{F3}" },
            { F2, "{F2}" },
            { F1, "{F1}" },
            { RETURN, "{RETURN}" },
            { DELETE, "{DELETE}" },
        };

    
    public static string  TranslateKey(string keys)
    {
      keys = Keyboard.EscapeSpecialCharacters(keys);
      StringBuilder sb = new StringBuilder();

      bool altDown = false;
      bool shiftDown = false;
      bool ctrDown = false;
      bool winDown = false;

      Action<char> modifyKey = (key) => {
        if (key == ALT)
        {
          altDown = !altDown;
          sb.Append(altDown ? "{ALT DOWN}" : "{ALT UP}");
        }
        else if (key == SHIFT)
        {
          shiftDown = !shiftDown;
          sb.Append(shiftDown ? "{SHIFT DOWN}" : "{SHIFT UP}");
        }
        else if (key == CONTROL)
        {
          ctrDown = !ctrDown;
          sb.Append(ctrDown ? "{CONTROL DOWN}" : "{CONTROL UP}");
        }
        else if (key == WIN)
        {
          winDown = !winDown;
          sb.Append(winDown? "{WIN DOWN}" : "WIN UP");
        }
      }; 
      foreach (var key in keys)
      {
        if (key == NULL)
        {
          // Modifier keys(Ctrl, Shift, Alt, and Command/ Meta) are assumed to be "sticky"; each modifier should be held down(e.g.only a keydown event) until either the modifier is encountered again in the sequence, or the NULL (U+E000) key is encountered.
          if (altDown) modifyKey(ALT);
          if (ctrDown) modifyKey(CONTROL);
          if (shiftDown) modifyKey(SHIFT);
          if (winDown) modifyKey(WIN);
        }
        else if (key == ALT || key == SHIFT || key == WIN || key == CONTROL)
        {
          modifyKey(key);
        }
        else if (keyToKeyStringDictionary.ContainsKey(key))
        {
          sb.Append(keyToKeyStringDictionary[key]);
        }
        else
        {
          sb.Append(key);
        }
      }
      return sb.ToString();
    }
  }

  
}
