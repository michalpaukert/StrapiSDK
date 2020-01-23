using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Strapi_SDK
{
    internal class StrapiAuthResponse
    {
        [JsonPropertyName("jwt")]
        public string Jwt { get; set; }
    }
}
