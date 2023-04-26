using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.World.Types.Npc;

/*public enum NpcType : short
{
    Human,
    Monster,
    Animal
}*/

public struct NpcType
{
    public ushort Id { get; set; }

    public string Name { get; set; }

    public NpcType(ushort id, string name)
    {
        Id = id;
        Name = name;
    }

    public NpcType()
    {

    }
}
