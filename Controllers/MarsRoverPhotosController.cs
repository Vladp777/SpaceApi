using Microsoft.AspNetCore.Mvc;
using SpaceApi.Model;
using SpaceApi.Clients;

namespace SpaceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarsRoverPhotosController: ControllerBase
    {
        private readonly MarsPhotoDynamoDBClient _marsPhotoDynamoDBClient;


        public MarsRoverPhotosController(MarsPhotoDynamoDBClient DataBaseClient)
        {
            _marsPhotoDynamoDBClient = DataBaseClient;

        }

        [HttpGet("getMarsPhotos")]

        public MarsRoverPhotos MarsPhotos( string date, string camera = "all", int page = 1)
        {
            NasaClient client = new NasaClient();
            return client.GetRoverPhotosAsync(date, camera, page).Result;
        }

        [HttpGet("getInfoAboutMarsPhotoFromDB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get_marsphoto_from_DB(int userID, string url)
        {
            var result = await _marsPhotoDynamoDBClient.GetInfoAboutUserFavourites(userID, url);

            if (result == null)
            {
                return NotFound("This item doesn't exist in your 'Favourites list'");
            }

            var DB_response = new MarsPhotoDB
            {
                userID = result.userID,
                url = result.url,
                earth_date = result.earth_date,
                cameraName = result.cameraName,
                roverName = result.roverName
            };

            return Ok(DB_response);
        }

        [HttpGet("getAllUserFavouriteMarsPhotosFromDB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get_All_user_favourite_marsphoto_from_DB(int userID)
        {
            var response = await _marsPhotoDynamoDBClient.GetAllUserDataFromDynamoDB(userID);

            if (response == null)
            {
                return NotFound("There are no records in your 'Favourites list'");
            }

            var result = response
                .Select(i => new MarsPhotoDB()
                {
                    userID = i.userID,
                    url = i.url,
                    earth_date = i.earth_date,
                    cameraName = i.cameraName,
                    roverName = i.roverName
                })
                .ToList();

            return Ok(result);
        }


        [HttpPost("addFavouriteMarsPhotosToDB")]
        public async Task<IActionResult> Add_item_to_favouritemarsphoto_database(MarsPhotoDB db_object)
        {
            //var data = new DB_object
            //{
            //    userID = db_object.userID,
            //    //messageID = db_object.messageID,
            //    title = db_object.title,
            //    explanation = db_object.explanation,
            //    date = db_object.date,
            //    url = db_object.url
            //};

            var result = await _marsPhotoDynamoDBClient.PostDataToDynamoDB(db_object);

            if (result == false)
            {
                return BadRequest("Unablle to add this item to your 'Favourites list'");
            }

            return Ok("The item was successfully added to your 'Favourites list'");
        }


        [HttpDelete("deleteMarsPhotoFromDB")]
        public async Task<IActionResult> Delete_item_fron_favouritemarsphoto_database(int userID, string url)
        {
            var result = await _marsPhotoDynamoDBClient.DeleteDataFromDynamoDB(userID, url);

            if (result == false)
            {
                return BadRequest("You don't have this item in your 'Favourites list'");
            }

            return Ok("The item was successfully deleted from your 'Favourites list'");
        }


        [HttpDelete("deleteAllUserMarsPhotosFromDB")]
        public async Task<IActionResult> Delete_ALL_user_items_from_favouritemarsphotodatabase(int userID)
        {
            var result = await _marsPhotoDynamoDBClient.DeleteAllUserDataFromDynamoDB(userID);

            if (result == false)
            {
                return BadRequest("You don't have any items in your 'Favourites list'");
            }

            return Ok("All items were successfully deleted from your 'Favourites list'");
        }
    }
}
