using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Map.Entities;

namespace DarkStar.Api.Engine.Interfaces.Objects
{
    public interface IGameObjectAction
    {
        ValueTask OnInitializedAsync(string mapId, WorldGameObject gameObject);

        ValueTask OnActivatedAsync(string mapId, WorldGameObject gameObject, Guid senderId, bool isNpc);
    }
}
