using System.Text.Json;
using Dapr;
using Dapr.Client;
using DeepBlue.Api.MoveValidator;
using DeepBlue.Api.MoveValidator.Services;
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Cors;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opts =>
{
  opts.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());
});

builder.Services
  .AddControllers()
  .AddDapr();

string httpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "20002";
string grpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "10002";

builder.Services.AddDaprClient(builder =>
  builder.UseHttpEndpoint($"http://localhost:{httpPort}")
         .UseGrpcEndpoint($"http://localhost:{grpcPort}")
);

builder.Services.AddScoped<IFENService, FENService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCloudEvents();
app.MapSubscribeHandler();
app.MapControllers();

app.UseCors();
// app.UseHttpsRedirection();

await app.RunAsync();
