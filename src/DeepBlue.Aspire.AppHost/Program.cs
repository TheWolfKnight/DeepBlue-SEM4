using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder
  .AddProject<DeepBlue_Api_MoveValidator>("move-validator-service")
  .WithDaprSidecar("move-validator");

builder
  .AddProject<DeepBlue_Api_Engine>("engine-service")
  .WithDaprSidecar("engine");

var gatewayService = builder
  .AddProject<DeepBlue_Api_Gateway>("gatewayservice")
  .WithDaprSidecar("gateway")
  .WithHttpEndpoint(name: "gatewayaccess", port: 80, isProxied: false);

builder
  .AddProject<DeepBlue_Blazor>("frontend")
  .WithReference(gatewayService);

string? daprPath = Environment.GetEnvironmentVariable("DAPR_PATH", EnvironmentVariableTarget.User);

if (daprPath is not null)
  builder.AddDapr(opts => opts.DaprPath = daprPath);
else
  builder.AddDapr();

var app = builder.Build();

app.Run();
