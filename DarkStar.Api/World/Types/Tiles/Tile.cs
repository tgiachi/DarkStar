using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


namespace DarkStar.Api.World.Types.Tiles;

[StructLayout(LayoutKind.Auto)]
public struct Tile
{
    public string FullName => $"{Category}_{SubCategory}_{Name}";

    public uint Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public bool IsTransparent { get; set; }
    public string? Tag { get; set; }

    public Tile(string name, int id, string category, string subCategory, bool isTransparent, string? tag)
    {
        Name = name;
        Id = (uint)id;
        Category = category;
        SubCategory = subCategory;
        IsTransparent = isTransparent;
        Tag = tag;
    }

    public Tile()
    {

    }

    public override string ToString() => $"Name: {Name}, Id: {Id}, Category: {Category}, IsTransparent: {IsTransparent}";

    
}
