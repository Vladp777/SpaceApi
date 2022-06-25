using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceApi.Clients;
using SpaceApi.Extensions;
using SpaceApi.Model;

namespace SpaceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DynamoDBController : ControllerBase
    {
        private readonly DynamoDBClient _dynamoDataBaseClient;

        public DynamoDBController(DynamoDBClient dynamoDataBaseClient)
        {
            _dynamoDataBaseClient = dynamoDataBaseClient;
        }


        [HttpGet("getInfoAboutItemFromDB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get_Data_from_DB(int userID, int messageID)
        {
            var result = await _dynamoDataBaseClient.GetInfoAboutUserFavourites(userID, messageID);

            if (result == null)
            {
                return NotFound("This item doesn't exist in your 'Favourites list'");
            }

            var DB_response = new DB_object
            {
                userID = result.userID,
                messageID = result.messageID,
                title = result.title,
                explanation = result.explanation,
                data = result.data,
                url = result.url
            };

            return Ok(DB_response);
        }
    
        [HttpGet("getAllUserFavouritesFromDB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get_All_user_favourites_from_DB(int userID)
        {
            var response = await _dynamoDataBaseClient.GetAllUserDataFromDynamoDB(userID);

            if (response == null)
            {
                return NotFound("There are no records in your 'Favourites list'");
            }

            var result = response
                .Select(i => new APOD()
                {
                    title = i.title,
                    date = i.data,
                    explanation = i.explanation,
                    url = i.url,
                    hdurl = "",
                    media_type = ""
                })
                .ToList();

            return Ok(result);
        }


        [HttpPost("addFavouriteToDB")]
        public async Task<IActionResult> Add_item_to_favourites_database(DB_object db_object)
        {
            var data = new DB_object
            {
                userID = db_object.userID,
                messageID = db_object.messageID,
                title = db_object.title,
                explanation = db_object.explanation,
                data = db_object.data,
                url = db_object.url
            };

            var result = await _dynamoDataBaseClient.PostDataToDynamoDB(data);

            if (result == false)
            {
                return BadRequest("Unablle to add this item to your 'Favourites list'");
            }

            return Ok("The item was successfully added to your 'Favourites list'");
        }



        
        [HttpDelete("deleteFromDB")]
        public async Task<IActionResult> Delete_item_fron_favourites_database(int userID, int messageID)
        {
            var result = await _dynamoDataBaseClient.DeleteDataFromDynamoDB(userID, messageID);

            if (result == false)
            {
                return BadRequest("You don't have this item in your 'Favourites list'");
            }

            return Ok("The item was successfully deleted from your 'Favourites list'");
        }



        // видаляє певний елемент з бази даних
        // вхідні дані:
        // -- айді телеграм-аккаунта юзера
        // спочатку перевіряє наявність елементів у даного юзера
        // якщо елементи є, то видаляє їх
        [HttpDelete("deleteAllUserDataFromDB")]
        public async Task<IActionResult> Delete_ALL_user_items_from_favourites_database(int userID)
        {
            var result = await _dynamoDataBaseClient.DeleteAllUserDataFromDynamoDB(userID);

            if (result == false)
            {
                return BadRequest("You don't have any items in your 'Favourites list'");
            }

            return Ok("All items were successfully deleted from your 'Favourites list'");
        }
    }
}
