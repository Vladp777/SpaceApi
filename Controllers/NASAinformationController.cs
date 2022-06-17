using Microsoft.AspNetCore.Mvc;
using NasaApi.Model;
using NasaApi.Clients;
namespace NasaApi.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class NASAinformationController : ControllerBase
    {
        [HttpGet(Name = "NASAinformation")]

        public ImageNasa info()
        {
            NasaClient nasaClient = new NasaClient();
            return nasaClient.GetImageAsync().Result;
        }

    }
}
