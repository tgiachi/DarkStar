using DarkStar.Network.Client.Interfaces;

namespace DarkStar.Client.Services;

public class ServiceContext
{
    public ServiceContext(IDarkStarNetworkClient networkClient) => NetworkClient = networkClient;
    public IDarkStarNetworkClient NetworkClient { get; set; }
}
