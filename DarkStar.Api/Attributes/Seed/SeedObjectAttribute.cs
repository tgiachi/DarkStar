namespace DarkStar.Api.Attributes.Seed;

[AttributeUsage(AttributeTargets.Class)]
public class SeedObjectAttribute : Attribute
{
    public string TemplateDirectory { get; set; }

    public SeedObjectAttribute(string templateDirectory)
    {
        TemplateDirectory = templateDirectory;
    }
}
