// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace WinAppDriver.Infra.Utils
{
  class JsonHelper
  {
    public static T Deserialize<T>(object obj)
    {
      //JsonSerializerSettings settings = new JsonSerializerSettings();
      //settings.MissingMemberHandling = MissingMemberHandling.Error;
      //return JsonConvert.DeserializeObject<T>(obj.ToString(), settings);
      return JsonConvert.DeserializeObject<T>(obj.ToString());
    }


    public static string Serialize<T>(T obj)
    {
      return JsonConvert.SerializeObject(obj);
    }
  }
}