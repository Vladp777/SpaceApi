
using SpaceApi.Model;
using Newtonsoft.Json;

namespace SpaceApi.Clients
{
    public class OpenNotifyClient
    {
        private HttpClient _httpClient;
        private static string _adrres = "http://api.open-notify.org";
        

        public OpenNotifyClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_adrres);
        }
        public async Task<LocationOfISS> GetLocationAsync()
        {
            var response = await _httpClient.GetAsync("/iss-now.json");
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<LocationOfISS>(content);
            return result;
        }

        public async Task<PeopleInSpace> GetPeopleInSpaceAsync()
        {
            var response = await _httpClient.GetAsync("/astros.json");
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<PeopleInSpace>(content);

            return result;
        }

    }
}
