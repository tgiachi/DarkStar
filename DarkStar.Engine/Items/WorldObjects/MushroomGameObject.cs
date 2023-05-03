using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Objects;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Items.WorldObjects.Base;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.World.Types.GameObjects;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Items.WorldObjects;

[GameObjectAction("PROP_MUSHROOM")]
public class MushroomGameObject : BaseWorldObjectAction
{
    public MushroomGameObject(ILogger<BaseWorldObjectAction> logger, IDarkSunEngine engine) : base(logger, engine)
    {
    }

    public override ValueTask OnActivatedAsync(string mapId, WorldGameObject gameObject, Guid senderId, bool isNpc)
    {
        Logger.LogInformation("Mushroom activated");
        return RemoveMySelfAsync();
    }
}
