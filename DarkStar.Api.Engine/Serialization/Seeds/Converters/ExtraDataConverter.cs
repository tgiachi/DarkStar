using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsv.Conversions;

namespace DarkStar.Api.Engine.Serialization.Seeds.Converters
{
    public class ExtraDataConverter : IValueConverter
    {
        public string Convert(object value, object parameter, IFormatProvider provider)
        {
            if (value is Dictionary<string, string> dict)
            {
                var strBuilder = new StringBuilder();

                foreach (var (key, val) in dict)
                {
                    strBuilder.Append($"{key}={val}#");
                }
            }

            return string.Empty;
        }

        public object ConvertBack(string value, Type targetType, object parameter, IFormatProvider provider)
        {
            var dict = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(value))
            {
                if (value.Contains("#"))
                {
                    var row = value.Split('#');
                    foreach (var s in row)
                    {
                        var keyValue = s.Split("=");
                        dict.Add(keyValue[0], keyValue[1]);
                    }
                }

            }
            return dict;
        }
    }
}
