

namespace DarkStar.Api.Data.Config;

public class DirectoriesConfig
{
    public int Length => Directories.Count;
    public Dictionary<DirectoryNameType, string> Directories { get; }

    public DirectoriesConfig()
    {
        Directories = new Dictionary<DirectoryNameType, string>();
        foreach (var type in Enum.GetValues(typeof(DirectoryNameType)).Cast<DirectoryNameType>())
        {
            Directories.Add(type, type.ToString());
        }
    }

    public string this[DirectoryNameType index]
    {
        get => Directories[index];
        set => Directories[index] = value;
    }

    public void AddDirectory(DirectoryNameType type, string directory)
    {
        Directories.Add(type, directory);
    }

    public string GetDirectory(DirectoryNameType type) => Directories[type];
}
