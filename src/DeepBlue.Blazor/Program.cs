using DeepBlue.Blazor;
using DeepBlue.Blazor.Features.PieceFeature;
using DeepBlue.Blazor.Features.SaveGamesFeature;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

SaveGameFeatureBuilder.Load(builder);
PieceFeatureBuilder.Load(builder);

await builder.Build().RunAsync();
