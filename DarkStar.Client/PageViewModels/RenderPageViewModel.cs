using System.Threading.Tasks;
using DarkStar.Api.World.Types.Map;
using DarkStar.Client.Attributes;
using DarkStar.Client.Controls;
using DarkStar.Client.Models.Tiles;
using DarkStar.Client.PageViews;
using DarkStar.Client.Services;
using DarkStar.Client.ViewModels;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Map;
using DarkStar.Network.Protocol.Messages.Triggers.Npc;
using DarkStar.Network.Protocol.Types;

namespace DarkStar.Client.PageViewModels;

[PageView(typeof(RenderPageView))]
public class RenderPageViewModel : PageViewModelBase
{
    private readonly GraphicEngineRender _graphicEngineRender;
    private readonly ServiceContext _serviceContext;


    public RenderPageViewModel(GraphicEngineRender graphicEngineRender, ServiceContext serviceContext)
    {
        _graphicEngineRender = graphicEngineRender;
        _serviceContext = serviceContext;

        _serviceContext.NetworkClient.SubscribeToMessage<MapResponseMessage>(DarkStarMessageType.MapResponse, OnMapResponse);
        _serviceContext.NetworkClient.SubscribeToMessage<NpcMovedResponseMessage>(DarkStarMessageType.NpcMovedResponse, OnNpcMoved);
    }

    private Task OnNpcMoved(IDarkStarNetworkMessage arg)
    {
        var message = (NpcMovedResponseMessage)arg;

        _graphicEngineRender.MoveTile(MapLayer.Creatures, message.NpcId.ToString(), message.Position);

        return Task.CompletedTask;
    }

    private Task OnMapResponse(IDarkStarNetworkMessage arg)
    {
        var message = (MapResponseMessage)arg;

        _graphicEngineRender.ClearLayers();

        foreach (var terrain in message.TerrainsLayer)
        {
            _graphicEngineRender.AddTile(
                MapLayer.Terrain,
                new Tile(terrain.Id.ToString(), terrain.TileType, terrain.Position)
            );
        }

        foreach (var npc in message.NpcsLayer)
        {
            _graphicEngineRender.AddTile(MapLayer.Creatures, new Tile(npc.Id.ToString(), npc.TileType, npc.Position));
        }

        return Task.CompletedTask;
    }

    public void SetupRenderControl(RenderControl renderControl)
    {
        renderControl.RenderAction = _graphicEngineRender.RenderAction;
    }
}
