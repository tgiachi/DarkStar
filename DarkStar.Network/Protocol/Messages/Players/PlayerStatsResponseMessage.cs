using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[NetworkMessage(DarkStarMessageType.PlayerStatsResponse)]
[ProtoContract]
public struct PlayerStatsResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(0)]
    public int Level { get; set; }
    [ProtoMember(1)]
    public int Experience { get; set; }
    [ProtoMember(2)]
    public int Strength { get; set; }
    [ProtoMember(3)]
    public int Dexterity { get; set; }
    [ProtoMember(4)]
    public int Intelligence { get; set; }
    [ProtoMember(5)]
    public int Luck { get; set; }
    [ProtoMember(6)]
    public int MaxHealth { get; set; }
    [ProtoMember(7)]
    public int MaxMana { get; set; }
    [ProtoMember(8)]
    public int Health { get; set; }
    [ProtoMember(9)]
    public int Mana { get; set; }

    public PlayerStatsResponseMessage(int level, int experience, int strength, int dexterity, int intelligence, int luck, int maxHealth, int maxMana, int health, int mana)
    {
        Level = level;
        Experience = experience;
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Luck = luck;
        MaxHealth = maxHealth;
        MaxMana = maxMana;
        Health = health;
        Mana = mana;
    }

    public PlayerStatsResponseMessage()
    {

    }
}
