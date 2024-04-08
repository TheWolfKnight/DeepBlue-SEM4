
using DeepBlue.Blazor.Features.PieceFeature.Services;
using DeepBlue.Blazor.Features.PieceFeature.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace DeepBlue.Blazor.Features.PieceFeature;

public static class PieceFeatureBuilder
{
    public static void Load(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IPieceService, PieceService>();
    }
}
