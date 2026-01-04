using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Application;

public interface IAccountApplication
{
    Task<IEnumerable<AccountMaterialStorageDto>> GetAccountMaterialsAsync(string authHeader);
    Task<IEnumerable<string>> GetAllCharactersAsync(string authHeader);
    Task<CharacterDto> GetCharacterByIdAsync(string id, string authHeader);
}