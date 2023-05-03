using DarkStar.Api.Engine.Map.Entities;

namespace DarkStar.Api.Engine.Interfaces.Objects;

public interface IItemAction
{
    Task OnInitializedAsync(string mapId, ItemGameObject itemObject);
    Task OnActionAsync(string mapId, ItemGameObject itemObject, Guid senderId, bool isNpc);
    Task OnEquippedAsync(string mapId, ItemGameObject itemObject, Guid senderId, bool isNpc);
    Task OnRemovedAsync(string mapId, ItemGameObject itemObject, Guid senderId, bool isNpc);
    Task<int> GetAttackAsync(string mapId, ItemGameObject itemObject, Guid senderId, bool isNpc);
    Task<int> GetDefenceAsync(string mapId, ItemGameObject itemObject, Guid senderId, bool isNpc);
}
