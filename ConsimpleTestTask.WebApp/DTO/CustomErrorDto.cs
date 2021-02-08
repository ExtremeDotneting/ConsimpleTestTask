using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsimpleTestTask.WebApp.DTO
{
    public class CustomErrorDto
    {
        [JsonProperty("error")]
        public ErrorObj Error { get; set; } = new ErrorObj();

        public class ErrorObj
        {
            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("errorKey")]
            public string ErrorKey { get; set; }

            [JsonProperty("debugUrl")]
            public string DebugUrl { get; set; }

        }
    }
}
