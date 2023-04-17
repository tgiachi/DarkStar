
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Engine.Services.Base;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services
{

    [DarkStarEngineService(nameof(ItemService), 6)]
    public class ItemService : BaseService<ItemService>, IItemService
    {
        public ItemService(ILogger<ItemService> logger) : base(logger)
        {

        }
    }
}
