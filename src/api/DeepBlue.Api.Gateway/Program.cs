using DeepBlue.Api.Gateway;
using DeepBlue.Api.Gateway.Hubs;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(config => config.EnableDetailedErrors = true);

builder.Services.AddCors(opts =>
{
  opts.AddPolicy(CorsPolicys.TestHub.AllowFrontend,
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

var app = builder.Build();

app.MapHub<TestHub>("/testhub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();
app.UseResponseCompression();

app.Run();
