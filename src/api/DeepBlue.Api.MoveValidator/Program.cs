using System.Text.Json;
using DeepBlue.Api.MoveValidator.Services;
using DeepBlue.Api.MoveValidator.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

JsonSerializerOptions jsonOptions = new JsonSerializerOptions
{
  AllowTrailingCommas = true,
  PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

builder.Services
  .AddControllers()
  .AddDapr(config => config.UseJsonSerializationOptions(jsonOptions));

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

app.UseHttpsRedirection();

app.Run();
