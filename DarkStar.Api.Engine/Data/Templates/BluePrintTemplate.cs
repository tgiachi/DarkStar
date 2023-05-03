using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Map;
using FastEnumUtility;

namespace DarkStar.Api.Engine.Data.Templates;

public class BluePrintTemplate
{
    public string Name { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public Dictionary<MapLayer, List<BluePrintTemplatePoint>> Layers { get; set; } = new();

    public BluePrintTemplate()
    {
        InitLayers();
    }

    public BluePrintTemplate(string name, string className)
    {
        Name = name;
        ClassName = className;
        InitLayers();
    }

    private void InitLayers()
    {
        foreach (var layer in FastEnum.GetValues<MapLayer>())
        {
            Layers.Add(layer, new List<BluePrintTemplatePoint>());
        }
    }

    public override string ToString() => $"{Name} - {ClassName}";
}
