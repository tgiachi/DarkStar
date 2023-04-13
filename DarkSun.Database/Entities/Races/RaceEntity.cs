using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Races;

[Table(Name = "races")]
public class RaceEntity : BaseStatEntity
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public TileType TileId { get; set; }

    public bool IsVisible { get; set; }
}
