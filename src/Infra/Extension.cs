using System;
using System.Collections.Generic;
using System.Text;

namespace WinAppDriver.Infra
{
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
  }
}
