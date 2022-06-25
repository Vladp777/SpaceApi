using Microsoft.AspNetCore.Mvc;
using SpaceApi.Model;
using SpaceApi.Clients;
namespace SpaceApi.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class APODController : ControllerBase
    {
        [HttpGet("apod")]

        public APOD info()
        {
            NasaClient nasaClient = new NasaClient();
            return nasaClient.GetAPODAsync().Result;
        }

        [HttpGet("apodbydate")]

        public APOD info1(string date)
        {
            NasaClient nasaClient = new NasaClient();
            return nasaClient.GetAPODAsync(date).Result;
        }
    }
}
