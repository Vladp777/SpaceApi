using Microsoft.AspNetCore.Mvc;
using NasaApi.Model;
using NasaApi.Clients;
namespace NasaApi.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class APODController : ControllerBase
    {
        [HttpGet(Name = "APOD")]

        public APODmodel info()
        {
            NasaClient nasaClient = new NasaClient();
            return nasaClient.GetAPODAsync().Result;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class APODbyDateController : ControllerBase
    {
        [HttpGet(Name = "APODbyDate")]

        public APODmodel info(string date)
        {
            NasaClient nasaClient = new NasaClient();
            return nasaClient.GetAPODAsync(date).Result;
        }
    }
}
