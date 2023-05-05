using DarkStar.Network.Client.Interfaces;

namespace DarkStar.Client.Services;

public static class ServiceContext
{
    public static IDarkStarNetworkClient NetworkClient { get; set; }

}
