using Microsoft.AspNetCore.Mvc;
using NasaApi.Model;
using NasaApi.Clients;

namespace NasaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarsRoverPhotosController : ControllerBase
    {
        [HttpGet(Name = "MarsRoverPhotos")]

        public MarsRoverPhotos MarsPhotos( string date, string camera = "all", int page = 1)
        {
            NasaClient client = new NasaClient();
            return client.GetRoverPhotosAsync(date, camera, page).Result;
        }
    }
}
