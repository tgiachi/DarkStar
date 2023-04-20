using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Events.Map;
using DarkStar.Api.Engine.Events.Players;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Engine.MessageListeners.Helpers;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Live;

using DarkStar.Network.Protocol.Messages.Triggers.Npc;
using Microsoft.Extensions.Logging;
using Redbus;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService(nameof(NetworkEventDispatcherService), 1000)]
    public class NetworkEventDispatcherService : BaseService<NetworkEventDispatcherService>, INetworkEventDispatcherService
    {
        private readonly HashSet<SubscriptionToken> _subscriptionTokens = new();

        public NetworkEventDispatcherService(ILogger<NetworkEventDispatcherService> logger) : base(logger)
        {

        }

        protected override ValueTask<bool> StartAsync()
        {
            Logger.LogDebug("Subscribing events");
            _subscriptionTokens.Add(Engine.EventBus.Subscribe<GameObjectAddedEvent>(OnGameObjectAdded));
            _subscriptionTokens.Add(Engine.EventBus.Subscribe<GameObjectMovedEvent>(OnGameObjectMoved));
            _subscriptionTokens.Add(Engine.EventBus.Subscribe<GameObjectRemovedEvent>(OnGameObjectRemoved));
            _subscriptionTokens.Add(Engine.EventBus.Subscribe<PingRequestEvent>(OnPingRequestMessage));
            _subscriptionTokens.Add(Engine.EventBus.Subscribe<PlayerLoggedEvent>(OnPlayerLoggedMessage));
            return ValueTask.FromResult(true);
        }

        private void OnPlayerLoggedMessage(PlayerLoggedEvent obj)
        {
            _ = Task.Run(async () =>
            {
                Logger.LogDebug("Sending information of map to player {PlayerId}", obj.PlayerId);
                Engine.NetworkServer.SendMessageAsync(obj.SessionId, await MapDataHelper.BuildMapResponseDataAsync(Engine, obj.MapId, obj.PlayerId));

            });
        }

        public override ValueTask<bool> StopAsync()
        {
            Logger.LogDebug("Unsubscribing events");

            foreach (var subscriptionToken in _subscriptionTokens)
            {
                Engine.EventBus.Unsubscribe(subscriptionToken);
            }

            return base.StopAsync();
        }

        private async void OnGameObjectAdded(GameObjectAddedEvent obj)
        {
            var playerToNotify = Engine.WorldService.GetPlayers(obj.MapId);
            IDarkStarNetworkMessage message = null!;

            switch (obj.Layer)
            {
                case MapLayer.Creatures:
                    message = new NpcAddedResponseMessage(obj.MapId, "", "", obj.Position, TileType.Null);
                    break;
            }

            if (message != null)
            {
                foreach (var player in playerToNotify)
                {
                    await Engine.NetworkServer.SendMessageAsync(player.NetworkSessionId, message);
                }
            }
        }

        private void OnGameObjectRemoved(GameObjectRemovedEvent obj)
        {
            var playerToNotify = Engine.WorldService.GetPlayers(obj.MapId);
            foreach (var player in playerToNotify)
            {
                // Engine.NetworkServer.SendMessageAsync(player.NetworkSessionId, )
            }
        }

        private void OnGameObjectMoved(GameObjectMovedEvent obj)
        {
            var playerToNotify = Engine.WorldService.GetPlayers(obj.MapId);
            foreach (var player in playerToNotify)
            {
                // Engine.NetworkServer.SendMessageAsync(player.NetworkSessionId, )
            }
        }

        private void OnPingRequestMessage(PingRequestEvent obj)
        {
            _ = Task.Run(async () => await Engine.NetworkServer.BroadcastMessageAsync(new PingMessageResponse()));
        }
    }
}
