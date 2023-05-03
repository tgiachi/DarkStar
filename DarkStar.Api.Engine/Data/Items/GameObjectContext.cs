using DarkStar.Api.Engine.Map.Entities;

namespace DarkStar.Api.Engine.Data.Items;

public class GameObjectContext
{
    public string MapId { get; set; }
    public WorldGameObject GameObject { get; set; }
    public Guid SenderId { get; set; }
    public bool IsNpc { get; set; }
}
