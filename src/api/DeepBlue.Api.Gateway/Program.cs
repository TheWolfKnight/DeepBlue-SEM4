using DeepBlue.Api.DaprClients;
using DeepBlue.Api.DaprClients.Interfaces;
using DeepBlue.Api.Gateway;
using DeepBlue.Api.Gateway.Hubs;
using DeepBlue.Api.RedisHandler;
using DeepBlue.Api.RedisHandler.Interfaces;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(config => config.EnableDetailedErrors = true);

builder.Services.AddCors(opts =>
{
  opts.AddPolicy(CorsPolicys.AllowFrontend,
    config => config.WithOrigins("http://localhost:5198",
                                 "https://localhost:7183")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("*")
  );
});

builder.Services.AddResponseCompression(opts =>
{
  opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    ["application/octet-stream"]);
});

builder.Services.AddScoped<IMakeMoveClient, MakeMoveClient>();
builder.Services.AddScoped<IMoveValidationClient, MoveValidationClient>();
builder.Services.AddScoped<IRedisHandler, RedisHandler>();

var app = builder.Build();

app.MapHub<TestHub>("/testhub");
app.MapHub<MakeMoveHub>("/makemovehub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(CorsPolicys.AllowFrontend);

app.UseHttpsRedirection();
app.UseResponseCompression();

app.Run();
