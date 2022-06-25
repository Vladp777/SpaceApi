using SpaceApi.Constant;
using SpaceApi.Model;
using Newtonsoft.Json;

namespace SpaceApi.Clients
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

        public async Task<APOD> GetAPODAsync()
        {
            var response = await _httpClient.GetAsync($"/planetary/apod?api_key={_apikey}");
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<APOD>(content);
            return result;
        }
        public async  Task<APOD> GetAPODAsync(string date)
        {
            var response = await _httpClient.GetAsync($"/planetary/apod?api_key={_apikey}&date={date}");
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<APOD>(content);
            return result;
        }
        public async Task<MarsRoverPhotos> GetRoverPhotosAsync(string date, string camera = "all", int page = 1)
        {
            var response = await _httpClient.GetAsync($"/mars-photos/api/v1/rovers/curiosity/photos?earth_date={date}&camera={camera}&page={page}&api_key={_apikey}");
            if (camera == "all")
                response = await _httpClient.GetAsync($"/mars-photos/api/v1/rovers/curiosity/photos?earth_date={date}&page={page}&api_key={_apikey}");

            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<MarsRoverPhotos>(content);
            return result;
        }
        
    }
}
