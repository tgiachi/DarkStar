using DarkStar.Api.Attributes.Seed;
using DarkStar.Api.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsv.Attributes;

namespace DarkStar.Api.Serialization.Types;

[HasHeaderRecord(true)]
[Delimiter(";")]
[SeedObject("gameObjectTypes")]
public class GameObjectTypeSerializableEntity
{

    [Column]
    public ushort Id { get; set; }

    [Column(converter: typeof(ToUpperCaseConverter))]
    public string Name { get; set; }
}
