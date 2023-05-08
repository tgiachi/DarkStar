using System;

namespace DarkStar.Client.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PageViewAttribute : Attribute
{
    public Type View { get; set; }

    public PageViewAttribute(Type view) => View = view;
}
