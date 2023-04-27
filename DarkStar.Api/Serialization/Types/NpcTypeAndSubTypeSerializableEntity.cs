using DarkStar.Api.Attributes.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Serialization.Converters;
using TinyCsv.Attributes;

namespace DarkStar.Api.Serialization.Types;

[HasHeaderRecord(true)]
[Delimiter(";")]
[SeedObject("NpcTypeSubTypes")]
public class NpcTypeAndSubTypeSerializableEntity 
{
    [Column]
    public int IdNpcType { get; set; }

    [Column]
    public int IdNpcSubType { get; set; }

    [Column(converter: typeof(ToUpperCaseConverter))]
    public string NpcName { get; set; }

    [Column(converter: typeof(ToUpperCaseConverter))]
    public string NpcSubTypeName { get; set; }

    [Column]
    public string TileName { get; set; }

}
