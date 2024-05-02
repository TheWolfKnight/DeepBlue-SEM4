using DeepBlue.Api.Engine;
using DeepBlue.Api.Engine.Services;
using DeepBlue.Api.Engine.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opts =>
  opts.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod())
);

builder.Services
  .AddControllers()
  .AddDapr();

string httpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "20003";
string grpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "10003";

builder.Services.AddDaprClient(builder =>
  builder.UseHttpEndpoint($"http://localhost:{httpPort}")
         .UseGrpcEndpoint($"http://localhost:{grpcPort}")
);

builder.Services.AddScoped<IMoveGeneratorService, MoveGeneratorService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();

app.UseCors();

app.UseCloudEvents();
app.MapSubscribeHandler();

// app.UseHttpsRedirection();

app.Run();
