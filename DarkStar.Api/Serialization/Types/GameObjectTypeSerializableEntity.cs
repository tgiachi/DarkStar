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
[SeedObject("GameObjectTypes")]
public class GameObjectTypeSerializableEntity
{
    [Column] public short Id { get; set; }

    [Column(converter: typeof(ToUpperCaseConverter))]
    public string Name { get; set; }
}
