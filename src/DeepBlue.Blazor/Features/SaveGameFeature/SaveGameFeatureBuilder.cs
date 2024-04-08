
using DeepBlue.Blazor.Features.SaveGamesFeature.Proxy;
using DeepBlue.Blazor.Features.SaveGamesFeature.Proxy.Interfaces;
using DeepBlue.Blazor.Features.SaveGamesFeature.Services;
using DeepBlue.Blazor.Features.SaveGamesFeature.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace DeepBlue.Blazor.Features.SaveGamesFeature;

public static class SaveGameFeatureBuilder
{

    public static void Load(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<ISaveGameProxy, SaveGameProxy>();
        builder.Services.AddScoped<ISaveGameService, SaveGameService>();
    }

}
