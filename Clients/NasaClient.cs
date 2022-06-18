using NasaApi.Constant;
using NasaApi.Model;

using Newtonsoft.Json;

namespace NasaApi.Clients
{
    public class NasaClient
    {
        private HttpClient _httpClient;
        private static string _adrres;
        private static string _apikey;

        public NasaClient()
        {
            _adrres = Constants.address;
            _apikey = Constants.apikey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_adrres);
        }

        public async Task<APODmodel> GetAPODAsync()
        {
            var response = await _httpClient.GetAsync($"/planetary/apod?api_key={_apikey}");
            var image = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<APODmodel>(image);
            return result;
        }
        public async  Task<APODmodel> GetAPODAsync(string date)
        {
            var response = await _httpClient.GetAsync($"/planetary/apod?api_key={_apikey}&date={date}");
            var image = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<APODmodel>(image);
            return result;
        }

    }
}
