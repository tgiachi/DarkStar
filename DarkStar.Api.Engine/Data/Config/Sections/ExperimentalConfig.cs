namespace DarkStar.Api.Engine.Data.Config.Sections;

public class ExperimentalConfig
{
    public CompilerConfig Compiler { get; set; }
}


public class CompilerConfig
{
    public bool EnableCSharpCompiler { get; set; } = false;
}
