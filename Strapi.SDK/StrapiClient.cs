using Strapi_SDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Strapi_SDK
{
    public class StrapiClient : IDisposable, IStrapiClient
    {
        private readonly HttpClient _httpClient;

        public StrapiClient(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentException(nameof(endpoint));
            }

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(endpoint)
            };
        }

        public async Task Login(string username, string password)
        {
            var formValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("identifier", username),
                new KeyValuePair<string, string>("password", password)
            };
            var content = new FormUrlEncodedContent(formValues);
            var response = await _httpClient.PostAsync("auth/local", content);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var auth = JsonSerializer.Deserialize<StrapiAuthResponse>(stringContent);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Jwt);
        }

        public async Task<IEnumerable<T>> GetEntries<T>(string contentType)
        {
            return await GetContent<IEnumerable<T>>(contentType);
        }

        public async Task<IEnumerable<T>> GetEntries<T>(string contentType, Dictionary<string, string> parameters)
        {
            var url = contentType.BuildQuery(parameters);
            return await GetContent<IEnumerable<T>>(url);
        }

        public async Task<T> GetEntry<T>(string contentType, int id)
        {
            var url = contentType.BuildQuery(id);
            var result = await GetContent<T>(url);

            return result;
        }

        public async Task<T> GetEntry<T>(string contentType, Dictionary<string, string> parameters)
        {
            var url = contentType.BuildQuery(parameters);
            var result = await GetContent<IEnumerable<T>>(url);
            return result.SingleOrDefault();
        }

        public async Task<int> Count(string contentType)
        {
            var url = $"{contentType}/count";
            var response = await _httpClient.GetStringAsync(url);

            return int.Parse(response);
        }

        private async Task<T> GetContent<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return await stream.DeserializeJsonFromStream<T>();
        }

        public Task<byte[]> GetFile(string id)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
