
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
  opts.AddDefaultPolicy(policy => policy.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod());
});

builder.Services.AddResponseCompression(opts =>
{
  opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    ["application/octet-stream"]);
});

builder.Services
  .AddControllers()
  .AddDapr();

string httpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "20001";
string grpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "10001";

builder.Services.AddDaprClient(builder =>
  builder.UseHttpEndpoint($"http://localhost:{httpPort}")
         .UseGrpcEndpoint($"http://localhost:{grpcPort}")
);

var app = builder.Build();

app.MapHub<TestHub>("/testhub");
app.MapHub<MakeMoveHub>("/makemovehub");

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

await app.RunAsync();
