using System;
using System.Globalization;
using Avalonia.Data.Converters;
using DarkStar.Network.Protocol.Messages.Common;
using FastEnumUtility;

namespace DarkStar.Client.Converters;

public class MovementTypeValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value == null ? MoveDirectionType.North : FastEnum.Parse<MoveDirectionType>((string)value);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
}
