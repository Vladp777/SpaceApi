using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using SpaceApi.Constant;
using SpaceApi.Clients;
using SpaceApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var credentials = new BasicAWSCredentials(Constants.accesskey, Constants.secretkey);    // ???'??????? ??????????? ??? ?????? ? ??
var config = new AmazonDynamoDBConfig()
{
    RegionEndpoint = RegionEndpoint.EUWest3
};
var client = new AmazonDynamoDBClient(credentials, config);
builder.Services.AddSingleton<IAmazonDynamoDB>(client);
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();


builder.Services.AddSingleton<APODDynamoDBClient>(); // ???'??????? ?볺??? ??? ?????? ? ??
builder.Services.AddSingleton<MarsPhotoDynamoDBClient>(); // ???'??????? ?볺??? ??? ?????? ? ??



var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
