using System.Runtime.InteropServices;

namespace DarkStar.Api.World.Types.Npc;

/*
public enum NpcSubType : short
{
    // Animals
    Cat,
    Dog,
    Cow,
    Horse,
    // Humans
    Human,

    MushroomFinder,
    // Monsters
    Rat,
    Goblin,
    Orc,
    Skeleton,
    Zombie,
    Ghoul,

}
*/

[StructLayout(LayoutKind.Auto)]
public struct NpcSubType
{
    public short NpcTypeId { get; set; }

    public short Id { get; set; }
    public string Name { get; set; }

    public NpcSubType(short npcTypeId, short id, string name)
    {
        NpcTypeId = npcTypeId;
        Id = id;
        Name = name;
    }

    public NpcSubType()
    {

    }
}
