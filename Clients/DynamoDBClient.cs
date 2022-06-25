using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceApi.Extensions;
using SpaceApi.Model;
using SpaceApi.Constant;

namespace SpaceApi.Clients
{
    public class DynamoDBClient: IDisposable
    {
        private readonly string _tableName;
        private readonly IAmazonDynamoDB _dynamoDB;

        public DynamoDBClient(IAmazonDynamoDB dynamoDB) 
        {
            _dynamoDB = dynamoDB;
            _tableName = Constants.TableName;            
        }


        // отримання інформації про наявність або відсутність певного елементу у БД
        public async Task<DB_object?> GetInfoAboutUserFavourites(int userID, int messageID)  
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"userID", new AttributeValue{N = $"{userID}" } },
                    {"messageID", new AttributeValue{N = $"{messageID}" } }                   
                }
            };
             
            var response = await _dynamoDB.GetItemAsync(item);

            if (response.Item == null || !response.IsItemSet)
            {
                return null;
            }
            var result = response.Item.ToClass<DB_object>();

            return result;
        }



        // додавання об'єкту до БД
        public async Task<bool> PostDataToDynamoDB(DB_object db) 
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"userID", new AttributeValue {N = $"{db.userID}"}},
                    {"messageID", new AttributeValue {N = $"{db.messageID}"}},
                    {"data", new AttributeValue {S = $"{db.data}"}},
                    {"explanation", new AttributeValue {S = $"{db.explanation}"}},
                    {"title", new AttributeValue {S = $"{db.title}"}},
                    {"url", new AttributeValue {S = $"{db.url}"}}
                }
            };

            try
            {
                var response = await _dynamoDB.PutItemAsync(request);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to add this item to database\n" + ex);

                return false;
            }              
        }



        // видалення об'єкту з БД
        public async Task<bool> DeleteDataFromDynamoDB(int userID, int messageID) 
        {
            var check_item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"userID", new AttributeValue{N = $"{userID}" } },
                    {"messageID", new AttributeValue{N = $"{messageID}" } }
                }
            };

            var check_item_response = await _dynamoDB.GetItemAsync(check_item);

            if (check_item_response.Item == null || !check_item_response.IsItemSet)
            {
                return false;
            }           

            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"userID", new AttributeValue{N = $"{userID}" } },
                    {"messageID", new AttributeValue{N = $"{messageID}" } }
                }
            };

            try
            {
                var response = await _dynamoDB.DeleteItemAsync(request);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unablle to delete this item from data base\n" + ex);

                return false;
            }            
        }



        // отримання усіх об'єктів конкретного юзера
        public async Task<List<DB_object>> GetAllUserDataFromDynamoDB(int userID)
        {
            var result = new List<DB_object>();

            var request = new QueryRequest
            {
                TableName = _tableName,
                ReturnConsumedCapacity = "TOTAL",
                KeyConditionExpression = "userID = :v_replyuserID",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> 
                {
                    {":v_replyuserID", new AttributeValue{N = $"{userID}"} }
                }
            };

            var response = await _dynamoDB.QueryAsync(request);

            if (response.Items == null || response.Items.Count == 0)
            {
                return null;
            }

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                result.Add(item.ToClass<DB_object>());
            }

            return result;
        }



        // видалення усіх елементів юзера       
        public async Task<bool> DeleteAllUserDataFromDynamoDB(int userID)
        {
            var check_item = new List<DB_object>();

            var check_item_request = new QueryRequest
            {
                TableName = _tableName,
                ReturnConsumedCapacity = "TOTAL",
                KeyConditionExpression = "userID = :v_replyuserID",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_replyuserID", new AttributeValue{N = $"{userID}"} }
                }
            };

            var check_item_response = await _dynamoDB.QueryAsync(check_item_request);

            if (check_item_response.Items == null || check_item_response.Items.Count == 0)
            {
                return false;
            }

            foreach (Dictionary<string, AttributeValue> item in check_item_response.Items)
            {
                check_item.Add(item.ToClass<DB_object>());
            }

            foreach (DB_object db_object in check_item)
            {
                var request = new DeleteItemRequest
                {
                    TableName = _tableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        {"userID", new AttributeValue{N = $"{db_object.userID}" } },
                        {"messageID", new AttributeValue{N = $"{db_object.messageID}" } }
                    }
                };

                await _dynamoDB.DeleteItemAsync(request);                
            }

            return true;
        }












        public void Dispose()
        {
            _dynamoDB.Dispose();
        }







        //// отримання усіх об'єктів конкретного юзера
        //public async Task<List<DB_object>> Get_ALL_user_data_from_DynamoDB(int telegram_user_id)
        //{
        //    var result = new List<DB_object>();

        //    //Dictionary<string, AttributeValue> lastKeyEvaluated = null;     

        //    var request = new ScanRequest
        //    {
        //        TableName = _tableName,
        //        //ExclusiveStartKey = lastKeyEvaluated,
        //        //ExpressionAttributeValues = new Dictionary<string, AttributeValue>
        //        //{
        //        //    {":val", new AttributeValue{N = $"{telegram_user_id}"}}
        //        //},
        //        //FilterExpression = "Telegran_ID = :val",
        //        //ProjectionExpression = "Telegran_ID, Title_ID, Title_name, Title_type"
        //    };

        //    var response = await _dynamoDB.ScanAsync(request);

        //    if (response.Items == null || response.Items.Count == 0)
        //    {
        //        return null;
        //    }

        //    foreach (Dictionary<string, AttributeValue> item in response.Items)
        //    {
        //        result.Add(item.ToClass<DB_object>());
        //    }

        //    return result;
        //}


     

    }
}
