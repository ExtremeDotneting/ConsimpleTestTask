using System;
using Newtonsoft.Json;

namespace ConsimpleTestTask.WebApp.Utils
{
    public static class EnumExtensions
    {
        public static string AsString<T>(this T enumVal)
            where T : Enum

        {
            return JsonConvert.SerializeObject(enumVal).Replace("\"", "");
        }

        public static T ToEnum<T>(this string str)
            where T : Enum
        {
            if (string.IsNullOrWhiteSpace(str))
                return default(T);
            return JsonConvert.DeserializeObject<T>("\""+str+ "\"");
        }
    }
}
