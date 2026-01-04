using Gw2ItemTracker.App.Adapters;
using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra;
using Gw2ItemTracker.Infra.Repositories;

namespace Gw2ItemTracker.App.Application;

public class AccountApplication : IAccountApplication
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IGw2HttpClient _gw2HttpClient;
    private readonly IAccountMaterialAdapter _adapter;
    private readonly ILogger<AccountApplication> _logger;

    public AccountApplication(IMaterialRepository materialRepository,
        IItemRepository itemRepository,
        ILogger<AccountApplication> logger, 
        IAccountMaterialAdapter adapter)
    {
        _materialRepository = materialRepository;
        _itemRepository = itemRepository;
        _logger = logger;
        _adapter = adapter;

        _gw2HttpClient = new Gw2HttpClient();
    }

    public async Task<IEnumerable<AccountMaterialStorageDto>> GetAccountMaterialsAsync(string authHeader)
    {
        var accountMaterialList =
            await _gw2HttpClient.GetAuthAsync<List<AccountMaterialDto>>(
                "v2/account/materials",
                authHeader);

        if (accountMaterialList is null)
        {
            throw new Exception("No AccountMaterials found");
        }

        var matStorageList = (await _materialRepository.GetAllStorageAsync()).ToList();
        if (!matStorageList.Any())
        {
            await InsertMaterialStorageAsync(accountMaterialList, matStorageList);
        }

        var accMaterials = BuildAccountStorageDtoList(accountMaterialList, matStorageList);
        return accMaterials;
    }

    public async Task<IEnumerable<string>> GetAllCharactersAsync(string authHeader)
    {
        var characters = await _gw2HttpClient.GetAuthAsync<List<string>>(
            "v2/characters",
            authHeader);

        if (characters is null)
        {
            throw new Exception("No Characters found");
        }
        
        return characters;
    }

    public async Task<CharacterDto> GetCharacterByIdAsync(string id, string authHeader)
    {
        var character = await _gw2HttpClient.GetAuthAsync<CharacterDto>(
            $"v2/characters/{id}",
            authHeader);

        if (character is null)
        {
            throw new Exception($"Character {id} not found");
        }
        
        return character;
    }

    private List<AccountMaterialStorageDto> BuildAccountStorageDtoList(List<AccountMaterialDto> accountMaterialList, List<MaterialStorage> matStorageList)
    {
        var accMaterials = new List<AccountMaterialStorageDto>();
        foreach (var dto in accountMaterialList)
        {
            var matStorage = matStorageList.FirstOrDefault(x => x.ItemId.Equals(dto.ItemId));

            if (matStorage is null)
            {
                _logger.LogWarning("Material storage for item {item} not found", dto.ItemId);
                continue;
            }
            
            var accMatStorageDto = _adapter.ConvertToDto(dto, matStorage);
            accMaterials.Add(accMatStorageDto);
        }

        return accMaterials;
    }

    private async Task InsertMaterialStorageAsync(List<AccountMaterialDto> accountMaterialList, List<MaterialStorage> matStorageList)
    {
        foreach (var dto in accountMaterialList)
        {
            var category = await _materialRepository.FindCategoryByIdAsync(dto.CategoryId);
            if (category is null)
            {
                _logger.LogWarning("Category {id} not found", dto.CategoryId);
                continue;
            }
                
            var item = await _itemRepository.FindByIdAsync(dto.ItemId);
            if (item is null)
            {
                _logger.LogWarning("Item {id} not found", dto.ItemId);
                continue;
            }
                
            var matStorage = _adapter.ConvertToStorageDomain(dto, item, category);
            matStorageList.Add(matStorage);
        }

        await _materialRepository.AddManyAsync(matStorageList);
    }
}