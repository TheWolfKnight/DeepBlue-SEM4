
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Blazor.Features.SaveGamesFeature.Services.Interfaces;

public interface ISaveGameService
{
    Task<GameDto> GetGamebyIdAsync(int gameId);
    Task<IEnumerable<GameDto>> GetGamesByUser(string userName);

    Task CreateGameAsync(CreateGameDto request);
    Task DeleteGameAsync(DeleteGameDto request);
    Task UpdateGameAsync(UpdateGameDto request);
}
