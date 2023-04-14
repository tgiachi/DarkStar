using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Map.Entities;

namespace DarkSun.Api.Engine.Interfaces.Objects
{
    public interface IGameObjectAction
    {
        Task OnInitializedAsync(string mapId, WorldGameObject gameObject);

        Task OnActivatedAsync(string mapId, WorldGameObject gameObject, Guid senderId, bool isNpc);
    }
}
