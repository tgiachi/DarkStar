using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services
{

    [DarkSunEngineService(nameof(ItemService), 6)]
    public class ItemService : BaseService<ItemService>, IItemService
    {
        public ItemService(ILogger<ItemService> logger) : base(logger)
        {

        }
    }
}
