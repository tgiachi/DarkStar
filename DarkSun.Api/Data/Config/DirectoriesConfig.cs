using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Api.Data.Config
{
  
    public class DirectoriesConfig
    {
        public int Length => Directories.Count;
        public Dictionary<DirectoryNameType, string> Directories { get; }

        public DirectoriesConfig()
        {
            Directories = new Dictionary<DirectoryNameType, string>();
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

        public string GetDirectory(DirectoryNameType type)
        {
            return Directories[type];
        }

    }
}
