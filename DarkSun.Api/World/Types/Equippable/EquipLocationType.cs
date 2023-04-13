namespace DarkSun.Api.World.Types.Equippable;

[Flags]
public enum EquipLocationType : int
{
    None = 0,
    Head = 1,
    Neck = 2,
    Shoulders = 4,
    Chest = 8,
    Back = 16,
    Wrists = 32,
    Hands = 64,
    Waist = 128,
    Legs = 256,
    Feet = 512,
    Ring = 1024,
    Trinket = 2048,
    MainHand = 4096,
    OffHand = 8192,
    Ranged = 16384,
    Ammo = 32768
}
