using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes;
using DarkSun.Api.Engine.Attributes.Network;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.MessageListeners;
using DarkSun.Database.Entities.Base;
using DarkSun.Engine.MessageListeners.Helpers;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Messages.Players;
using DarkSun.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.MessageListeners
{
    [NetworkMessageListener(DarkSunMessageType.PlayerCreateRequest)]
    public class PlayerCreationMessageListener : BaseNetworkMessageListener<PlayerCreateRequestMessage>
    {
        public PlayerCreationMessageListener(ILogger<BaseNetworkMessageListener<PlayerCreateRequestMessage>> logger,
            IDarkSunEngine engine) : base(logger, engine)
        {
        }

        public override async Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId,
            DarkSunMessageType messageType, PlayerCreateRequestMessage message)
        {
            if (Engine.PlayerService.GetSession(sessionId).IsLogged == false)
            {
                Logger.LogWarning("Player creation request from unlogged session {Id}", sessionId);
                return SingleMessage(new PlayerCreateResponseMessage(false, Guid.Empty));
            }


            var player = await Engine.PlayerService.CreatePlayerAsync(
                Engine.PlayerService.GetSession(sessionId).AccountId, message.TileId,
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
}
