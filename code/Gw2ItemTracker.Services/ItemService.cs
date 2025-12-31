using System.Threading.Channels;
using Gw2ItemTracker.Domain.Adapters;
using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra.Context;
using Microsoft.Extensions.Hosting;

namespace Gw2ItemTracker.Services;

public class ItemService : BackgroundService
{
    private readonly Channel<ProcessingResource<ItemDto>> _itemDtoChannel;
    private readonly DbContext _dbContext;
    private readonly IItemAdapter _itemAdapter;

    public ItemService(
        Channel<ProcessingResource<ItemDto>> itemDtoChannel, 
        DbContext dbContext, 
        IItemAdapter itemAdapter
        )
    {
        _itemDtoChannel = itemDtoChannel;
        _dbContext = dbContext;
        _itemAdapter = itemAdapter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (await _itemDtoChannel.Reader.WaitToReadAsync(stoppingToken))
            {
                if(!_itemDtoChannel.Reader.TryRead(out var itemDto))
                    continue;

                try
                {
                    itemDto.StartProcessing();
                    Console.WriteLine($"Processing item {itemDto.Id}");
                    var idFilter = MongoDB.Driver.Builders<Item>.Filter.Eq("_id", itemDto.Id);
                    var entity = _itemAdapter.ConvertToDomain(itemDto.Resource, itemDto.CurrentPage);
                    
                    var replaced = await _dbContext.Items.FindOneAndReplaceAsync<Item>(idFilter,
                        entity,
                        cancellationToken: stoppingToken);
                    
                    if (replaced is null)
                        await _dbContext.Items.InsertOneAsync(entity, cancellationToken: stoppingToken);
                    
                    itemDto.CompleteProcessing();
                }
                catch (OperationCanceledException e) when (stoppingToken.IsCancellationRequested)
                {
                    itemDto.FailProcessing();
                    Console.WriteLine(e);
                    continue;
                }
                catch (Exception e)
                {
                    itemDto.FailProcessing();
                    Console.WriteLine(e);
                    continue;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}