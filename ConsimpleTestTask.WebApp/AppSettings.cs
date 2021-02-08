using System;
using Microsoft.Extensions.Configuration;

namespace ConsimpleTestTask.WebApp
{
    public static class AppSettings
    {
        static IConfiguration Configuration { get; set; }

        public const string ApiPath = "api/v1";

        public static string DB_CONNECTION => Configuration["DB_CONNECTION"];
        public static string EXTERNAL_URL => Configuration["EXTERNAL_URL"];

        public static bool IS_DEBUG => Convert.ToBoolean(Configuration["IS_DEBUG"]);

        public static void Init(IConfiguration configuration)
        {
            Configuration = configuration;
        }

    }
}
