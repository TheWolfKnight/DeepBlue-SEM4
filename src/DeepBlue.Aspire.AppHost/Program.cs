using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder
  .AddProject<DeepBlue_Api_MoveValidator>("move-validator-service")
  .WithDaprSidecar("move-validator");

builder
  .AddProject<DeepBlue_Api_Engine>("engine-service")
  .WithDaprSidecar("engine");

var gatewayService = builder
  .AddProject<DeepBlue_Api_Gateway>("gateway-service", null)
  .WithDaprSidecar("gateway")
  .WithHttpEndpoint(name: "http", port: 80, isProxied: false);

// builder
//   .AddProject<DeepBlue_Blazor>("frontend")
//   .WithReference(gatewayService);

var app = builder.Build();

app.Run();
