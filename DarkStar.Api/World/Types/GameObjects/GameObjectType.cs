using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.World.Types.GameObjects;

[StructLayout(LayoutKind.Auto)]
public struct GameObjectType
{
    public short Id { get; }
    public string Name { get; }

    public GameObjectType(short id, string name)
    {
        Id = id;
        Name = name;
    }

    public GameObjectType()
    {
    }
}
