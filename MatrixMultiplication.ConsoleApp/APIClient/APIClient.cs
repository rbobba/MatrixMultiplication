using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MatrixMultiplication.ConsoleApp
{
    public class APIClient : IAPIClient
    {
        private readonly HttpClient _httpClient;

        public APIClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://recruitment-test.investcloud.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var result = await _httpClient.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<T>();
            }

            return default(T);
        }

        public async Task<T> PostAsync<T>(string uri, T data)
        {

            var result = await _httpClient.PostAsJsonAsync(uri, data);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<T>();
            }

            return default(T);

        }

    }
}
