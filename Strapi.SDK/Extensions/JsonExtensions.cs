using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Strapi_SDK.Extensions
{
    public static class JsonExtensions
    {
        public async static Task<T> DeserializeJsonFromStream<T>(this Stream stream)
        {
            if (stream == null || !stream.CanRead)
                return default(T);

            try
            {
                return await JsonSerializer.DeserializeAsync<T>(stream);
            }
            catch (Exception e)
            {
                throw new ContractViolationException(e.Message);
            }
        }
    }
}