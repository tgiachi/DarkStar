using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsv.Conversions;

namespace DarkStar.Api.Serialization.Converters;

public class ToUpperCaseConverter : IValueConverter
{
    public string Convert(object value, object parameter, IFormatProvider provider) => value.ToString().ToUpper();

    public object ConvertBack(string value, Type targetType, object parameter, IFormatProvider provider) => value.ToUpper();
}
