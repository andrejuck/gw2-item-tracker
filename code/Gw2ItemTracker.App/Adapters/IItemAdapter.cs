using Gw2ItemTracker.App.Views;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Adapters;

public interface IItemAdapter
{
    ItemView ConvertToView(Item domain);
}