using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Database.Entities.Npc;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.Data.Ai;

public class AiContext
{
    public NpcGameObject NpcGameObject { get; set; } = null!;
    public NpcEntity NpcEntity { get; set; } = null!;
    public string MapId { get; set; } = null!;

    public ILogger<AiContext> Logger { get; set; }
}
