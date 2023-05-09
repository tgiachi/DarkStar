using System;
using System.IO;
using DarkStar.Network.Client.Interfaces;

namespace DarkStar.Client.Services;

public class ServiceContext
{
    public string RootDirectory { get; set; }
    public string AssetDirectory { get; set; }
    public string ServerUrl { get; set; }

    public ServiceContext(IDarkStarNetworkClient networkClient)
    {
        NetworkClient = networkClient;
        RootDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DarkStarClient");
        AssetDirectory = Path.Join(RootDirectory, "Assets");

        Directory.CreateDirectory(RootDirectory);
        Directory.CreateDirectory(AssetDirectory);
    }
    public IDarkStarNetworkClient NetworkClient { get; set; }


}
