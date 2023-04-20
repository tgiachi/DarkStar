using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Database.Entities.Base;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Types;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners;

[NetworkMessageListener(DarkStarMessageType.PlayerCreateRequest)]
public class PlayerCreationMessageListener : BaseNetworkMessageListener<PlayerCreateRequestMessage>
{
    public PlayerCreationMessageListener(ILogger<BaseNetworkMessageListener<PlayerCreateRequestMessage>> logger,
        IDarkSunEngine engine) : base(logger, engine)
    {
    }

    public override async Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(Guid sessionId,
        DarkStarMessageType messageType, PlayerCreateRequestMessage message)
    {
        if (Engine.PlayerService.GetSession(sessionId).IsLogged == false)
        {
            Logger.LogWarning("Player creation request from unlogged session {Id}", sessionId);
            return SingleMessage(new PlayerCreateResponseMessage(false, Guid.Empty));
        }


        var player = await Engine.PlayerService.CreatePlayerAsync(
            Engine.PlayerService.GetSession(sessionId).AccountId, message.Name, message.TileId,
            message.RaceId,
            new BaseStatEntity()
            {
                Dexterity = message.Dexterity,
                Intelligence = message.Intelligence,
                Luck = message.Luck,
                Strength = message.Strength
            });


        return SingleMessage(new PlayerCreateResponseMessage(true, player.Id));
    }
}
