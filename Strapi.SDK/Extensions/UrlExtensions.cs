using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Strapi_SDK.Extensions
{
    public static class UrlExtensions
    {
        public static string BuildQuery(this string uri, Dictionary<string, string> parameters)
        {
            var stringBuilder = new StringBuilder();
            string str = "?";

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                stringBuilder.Append(str + parameter.Key + "=" + parameter.Value);
                str = "&";
            }
            return uri + stringBuilder;
        }
    }
}
