using DarkSun.Api.Engine.Interfaces.Services.Base;
using DarkSun.Network.Protocol.Messages.Common;

namespace DarkSun.Api.Engine.Interfaces.Services;

public interface IWorldService : IDarkSunEngineService
{
    PointPosition GetRandomWalkablePosition(string mapId);
}
