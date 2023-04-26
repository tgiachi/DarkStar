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
    public ushort Id { get; set; }
    public string Name { get; set; }

    public NpcSubType(ushort id, string name)
    {
        Id = id;
        Name = name;
    }

    public NpcSubType()
    {

    }
}
