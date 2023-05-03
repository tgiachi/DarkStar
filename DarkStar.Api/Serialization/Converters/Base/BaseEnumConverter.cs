using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Utils;
using FastEnumUtility;
using TinyCsv.Conversions;

namespace DarkStar.Api.Serialization.Converters.Base;

public class BaseEnumConverter<TEnum> : IValueConverter where TEnum : struct, Enum
{
    public virtual string Convert(object value, object parameter, IFormatProvider provider) => value.ToString()!;


    public virtual object ConvertBack(string value, Type targetType, object parameter, IFormatProvider provider)
    {
        if (value == null)
        {
            throw new Exception("Null value in enum converter");
        }

        if (value.ToLower() == "random")
        {
            return FastEnum.GetValues<TEnum>().ToList().RandomItem().ToString();
        }
        else
        {
            var enumValues = FastEnum.GetValues<TEnum>().ToList();
            if (value.Contains("*"))
            {
                // Replace * and search value in enum 

                var enumValue = enumValues.FirstOrDefault(
                    x =>
                        x.ToString().ToLower().StartsWith(value.ToLower().Replace("*", ""))
                );

                return enumValue!;
            }
            else
            {
                var enumValue = enumValues.FirstOrDefault(x => x.ToString().ToLower() == value.ToLower());
                return enumValue!;
            }
        }
    }
}
