using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Serialization.Converters.Base;
using DarkStar.Api.World.Types.Tiles;
using TinyCsv.Conversions;

namespace DarkStar.Api.Serialization.Converters;

public class TileTypeConverter : IValueConverter
{
    public string Convert(object value, object parameter, IFormatProvider provider)
    {
        return "";
    }

    public object ConvertBack(string value, Type targetType, object parameter, IFormatProvider provider)
    {
        return null!;
    }
}
