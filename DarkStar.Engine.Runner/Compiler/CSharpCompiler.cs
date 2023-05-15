using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Utils;
using DarkStar.Database.Entities.Player;
using GoRogue.GameFramework;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Utilities;
using ILogger = Serilog.ILogger;

namespace DarkStar.Engine.Runner.Compiler;

public class CSharpCompiler
{
    private readonly List<PortableExecutableReference> _references = new();
    private readonly ILogger _logger;
    private readonly DirectoriesConfig _directoriesConfig;

    public CSharpCompiler(ILogger logger, DirectoriesConfig directoriesConfig)
    {
        _logger = logger;
        _directoriesConfig = directoriesConfig;
    }

    public Task CompileSources()
    {
        string errorMessage = null;
        Assembly assembly = null;

        Stream codeStream = null;
        AddNetCoreDefaultReferences();
        _logger.Information("Search for compilation sources...");
        var files = Directory.GetFiles(_directoriesConfig[DirectoryNameType.Sources], "*.cs", SearchOption.AllDirectories);
        _logger.Information("Found {Count} compilation sources", files.Length);

        var trees = new List<SyntaxTree>();
        foreach (var file in files)
        {
            try
            {
                trees.Add(SyntaxFactory.ParseSyntaxTree(File.ReadAllText(file)));
            }
            catch (Exception ex)
            {
                _logger.Error("Error during syntax check for  file: {File}: {Err}", file, ex);
                throw;
            }
        }

        var compilation = CSharpCompilation.Create(
            "DarkStar.External.Sources",
            trees,
            _references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );
        var sw = new Stopwatch();
        sw.Start();
        _logger.Information("Start compilation...");

        using (codeStream = new MemoryStream())
        {
            // Actually compile the code
            EmitResult compilationResult = null;
            compilationResult = compilation.Emit(codeStream);

            // Compilation Error handling
            if (!compilationResult.Success)
            {
                var sb = new StringBuilder();
                foreach (var diag in compilationResult.Diagnostics)
                {
                    sb.AppendLine(diag.ToString());
                }

                errorMessage = sb.ToString();

                _logger.Error("Error during compilation: {Err}", errorMessage);

                Assert.IsTrue(false, errorMessage);

            }

            sw.Stop();
            _logger.Information("Compilation finished in {Elapsed}", sw.ElapsedMilliseconds.Milliseconds());

            // Load
            assembly = Assembly.Load(((MemoryStream)codeStream).ToArray());
            AssemblyUtils.AddAssembly(assembly);
        }

        return Task.CompletedTask;
    }



    private void AddNetCoreDefaultReferences()
    {
        var rtPath = Path.GetDirectoryName(typeof(object).Assembly.Location) +
                     Path.DirectorySeparatorChar;

        AddAssemblies(
            rtPath + "System.Private.CoreLib.dll",
            rtPath + "System.Runtime.dll",
            rtPath + "System.Console.dll",
            rtPath + "netstandard.dll",
            rtPath + "System.Text.RegularExpressions.dll", // IMPORTANT!
            rtPath + "System.Linq.dll",
            rtPath + "System.Linq.Expressions.dll", // IMPORTANT!
            rtPath + "System.IO.dll",
            rtPath + "System.Net.Primitives.dll",
            rtPath + "System.Net.Http.dll",
            rtPath + "System.Private.Uri.dll",
            rtPath + "System.Reflection.dll",
            rtPath + "System.ComponentModel.Primitives.dll",
            rtPath + "System.Globalization.dll",
            rtPath + "System.Collections.Concurrent.dll",
            rtPath + "System.Collections.NonGeneric.dll",
            rtPath + "Microsoft.CSharp.dll"
        );

        // this library and CodeAnalysis libs
        AddAssembly(typeof(ReferenceList)); // Scripting Library
        AssemblyUtils.GetAppAssemblies()
            .ToList()
            .ForEach(a => AddAssembly(a.Location));

        AddAssembly(typeof(GameObject).Assembly);
        AddAssembly(typeof(ILogger<>).Assembly);
        AddAssembly(typeof(PlayerEntity).Assembly);

    }

    private bool AddAssembly(Assembly assembly) => AddAssembly(assembly.Location);

    private bool AddAssemblies(params string[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
        {
            return false;
        }

        foreach (var assembly in assemblies)
        {
            if (!AddAssembly(assembly))
            {
                return false;
            }
        }

        return true;
    }

    private bool AddAssembly(string assemblyDll)
    {
        if (string.IsNullOrEmpty(assemblyDll))
        {
            return false;
        }

        var file = Path.GetFullPath(assemblyDll);

        if (!File.Exists(file))
        {
            // check framework or dedicated runtime app folder
            var path = Path.GetDirectoryName(typeof(object).Assembly.Location);
            file = Path.Combine(path, assemblyDll);
            if (!File.Exists(file))
            {
                return false;
            }
        }

        if (_references.Any(r => r.FilePath == file))
        {
            return true;
        }

        try
        {
            var reference = MetadataReference.CreateFromFile(file);
            _references.Add(reference);
        }
        catch
        {
            return false;
        }

        return true;
    }

    private bool AddAssembly(Type type)
    {
        try
        {
            if (_references.Any(r => r.FilePath == type.Assembly.Location))
                return true;

            var systemReference = MetadataReference.CreateFromFile(type.Assembly.Location);
            _references.Add(systemReference);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
