using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.World.Types.Npc;

public struct NpcType
{
    public short Id { get; set; }

    public string Name { get; set; }

    public NpcType(short id, string name)
    {
        Id = id;
        Name = name;
    }

    public NpcType()
    {
    }

    public override string ToString() => Name;
}
