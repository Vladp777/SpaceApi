using Microsoft.AspNetCore.Mvc;
using SpaceApi.Model;
using SpaceApi.Clients;

namespace SpaceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarsRoverPhotosController : ControllerBase
    {
        [HttpGet("getMarsPhotos")]

        public MarsRoverPhotos MarsPhotos( string date, string camera = "all", int page = 1)
        {
            NasaClient client = new NasaClient();
            return client.GetRoverPhotosAsync(date, camera, page).Result;
        }
    }
}
