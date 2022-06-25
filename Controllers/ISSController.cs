using Microsoft.AspNetCore.Mvc;
using SpaceApi.Model;
using SpaceApi.Clients;

namespace SpaceApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ISSController : ControllerBase
    {
        [HttpGet("locationISS")]

        public LocationOfISS location()
        {
            var onclient = new OpenNotifyClient();
            return onclient.GetLocationAsync().Result;
        }

        [HttpGet("numberOfPeopleInSpace")]

        public PeopleInSpace number()
        {
            OpenNotifyClient client = new OpenNotifyClient();

            return client.GetPeopleInSpaceAsync().Result;
        }
    }
}

