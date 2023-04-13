using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Utils;
using TinyCsv.Conversions;

namespace DarkSun.Api.Engine.Serialization.Seeds.Converters.Base
{
    public class BaseEnumConverter<TEnum> : IValueConverter where TEnum : Enum
    {
        public virtual string Convert(object value, object parameter, IFormatProvider provider)
        {
            return value.ToString()!;
        }


        public virtual object ConvertBack(string value, Type targetType, object parameter, IFormatProvider provider)
        {
            if (value == null)
            {
                throw new Exception("Null value in enum converter");
            }

            if (value.ToLower() == "random")
            {

                return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList().RandomItem().ToString();
            }
            else
            {
                var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
                if (value.Contains("*"))
                {
                    // Replace * and search value in enum 

                    var enumValue = enumValues.FirstOrDefault(x =>
                        x.ToString().ToLower().Contains(value[value.IndexOf("*", StringComparison.Ordinal)]));

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
}
