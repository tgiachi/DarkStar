using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.World.Types.Items;

[StructLayout(LayoutKind.Auto)]
public struct ItemType
{
    public ushort Id { get; set; }
    public string Name { get; set; }

    public ItemType(ushort id, string name)
    {
        Id = id;
        Name = name;
    }

    public ItemType()
    {
    }

    public override string ToString() => $"{Id} - {Name}";
}
