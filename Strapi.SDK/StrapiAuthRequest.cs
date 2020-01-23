using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Strapi_SDK
{
    internal class StrapiAuthRequest
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; }

        [JsonPropertyName("password")]
        public string Password { get; }

        public StrapiAuthRequest(string username, string password)
        {
            Identifier = username;
            Password = password;
        }
    }
}
