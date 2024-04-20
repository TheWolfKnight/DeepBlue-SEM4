using DeepBlue.Blazor;
using DeepBlue.Blazor.Features.HubConnectionFeature;
using DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;
using DeepBlue.Blazor.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<GameState>();
builder.Services.AddScoped<ITestHubConnection, TestHubConnection>();

var app = builder.Build();

await app.RunAsync();