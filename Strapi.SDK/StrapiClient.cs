using Newtonsoft.Json;
using Strapi_SDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var auth = JsonConvert.DeserializeObject<StrapiAuthResponse>(stringContent);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Jwt);
        }

        public async Task<IEnumerable<T>> GetEntries<T>(string contentType)
        {
            string content = await GetContent(contentType);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }

        public async Task<IEnumerable<T>> GetEntries<T>(string contentType, Dictionary<string, string> parameters)
        {
            var url = contentType.BuildQuery(parameters);
            string content = await GetContent(url);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }

        public async Task<T> GetEntry<T>(string contentType, int id)
        {
            var url = $"{contentType}/{id}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> GetEntry<T>(string contentType, Dictionary<string, string> parameters)
        {
            var url = contentType.BuildQuery(parameters);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(content).FirstOrDefault();
        }

        public async Task<int> Count(string contentType)
        {
            var url = $"{contentType}/count";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return int.Parse(content);
        }

        public Task<byte[]> GetFile(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task<string> GetContent(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
