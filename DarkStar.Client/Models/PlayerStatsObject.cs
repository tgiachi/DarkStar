using ReactiveUI;

namespace DarkStar.Client.Models;


public class PlayerStatsObject : ReactiveObject
{
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Luck { get; set; }
    public int MaxHealth { get; set; }
    public int MaxMana { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
}
