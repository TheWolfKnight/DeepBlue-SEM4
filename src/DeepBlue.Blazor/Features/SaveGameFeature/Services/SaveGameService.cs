

using DeepBlue.Blazor.Features.SaveGamesFeature.Services.Interfaces;
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Blazor.Features.SaveGamesFeature.Services;

public class SaveGameService : ISaveGameService
{
    public Task CreateGameAsync(CreateGameDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGameAsync(DeleteGameDto request)
    {
        throw new NotImplementedException();
    }

    public Task<GameDto> GetGamebyIdAsync(int gameId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GameDto>> GetGamesByUser(string userName)
    {
        throw new NotImplementedException();
    }

    public Task UpdateGameAsync(UpdateGameDto request)
    {
        throw new NotImplementedException();
    }
}
