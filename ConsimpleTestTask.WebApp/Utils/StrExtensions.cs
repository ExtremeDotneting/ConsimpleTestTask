using System.Globalization;

namespace ConsimpleTestTask.WebApp.Utils
{
    public static class StrExtensions
    {
        public static string ToPascalCase(string str)
        {
            var pascalString = str.ToLower().Replace("_", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            pascalString = info.ToTitleCase(pascalString).Replace(" ", string.Empty);
            return pascalString;
        }
    }
}