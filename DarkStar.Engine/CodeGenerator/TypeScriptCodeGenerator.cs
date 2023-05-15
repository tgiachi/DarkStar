using System.Reflection;
using System.Text;
using DarkStar.Api.Engine.Data.Ai;
using DarkStar.Api.Engine.Data.Blueprint;
using DarkStar.Api.Engine.Data.ScriptEngine;
using FastEnumUtility;


namespace DarkStar.Engine.CodeGenerator;

public static class TypeScriptCodeGenerator
{
    public static string GetTypeScriptType(Type type)
    {
        if (type == typeof(int))
        {
            return "number";
        }
        else if (type == typeof(uint))
        {
            return "number";
        }
        else if (type == typeof(short))
        {
            return "number";
        }
        else if (type == typeof(string))
        {
            return "string";
        }
        else if (type == typeof(bool))
        {
            return "boolean";
        }
        else if (type == typeof(void))
        {
            return "void";
        }
        else if (type.IsEnum)
        {
            return type.Name;
        }
        else if (type.IsArray)
        {
            if (type == typeof(object[]))
            {
                return "any[]";
            }

            if (type == typeof(string[]))
            {
                return "string[]";
            }

            return $"{GetTypeScriptType(type.GenericTypeArguments[0])}[]";
        }

        else if (type == typeof(List<>))
        {
            return "[]";
        }
        else if (type == typeof(object))
        {
            return "any";
        }
        else if (IsAction(type))
        {
            if (type.GenericTypeArguments.Length == 0)
            {
                return $"() => void";
            }

            return $"( c: {type.GenericTypeArguments[0].Name}  ) => void";
        }
        else if (IsFunc(type))
        {
            return $"( c: {type.GenericTypeArguments[0].Name}  ) => {type.GenericTypeArguments[1].Name}";
        }
        else if (type.ToString().StartsWith("System.Collections.Generic."))
        {
            return "any[]";
        }

        return type.Name;
    }

    static bool IsAction(Type type)
    {
        if (type == typeof(System.Action))
        {
            return true;
        }

        Type generic = null;
        if (type.IsGenericTypeDefinition)
        {
            generic = type;
        }
        else if (type.IsGenericType)
        {
            generic = type.GetGenericTypeDefinition();
        }

        if (generic == null)
        {
            return false;
        }

        if (generic == typeof(Action<>))
        {
            return true;
        }

        if (generic == typeof(Action<,>))
        {
            return true;
        }

        return false;
    }

    static bool IsFunc(Type type)
    {
        if (type == typeof(System.Action))
        {
            return true;
        }

        Type generic = null;
        if (type.IsGenericTypeDefinition)
        {
            generic = type;
        }
        else if (type.IsGenericType)
        {
            generic = type.GetGenericTypeDefinition();
        }

        if (generic == null)
        {
            return false;
        }

        if (generic == typeof(Func<>))
        {
            return true;
        }

        if (generic == typeof(Func<,>))
        {
            return true;
        }

        return false;
    }

    public static void GenerateTypeScriptDefinition(Type type, StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine($"interface {type.Name} {{");


        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var propertyName = property.Name;
            var propertyType = GetTypeScriptType(property.PropertyType);

            stringBuilder.AppendLine($"    {propertyName}: {propertyType};");
        }

        stringBuilder.AppendLine("}");
    }

    public static string GenerateFunctionComment(ScriptFunctionDescriptor descriptor)
    {
        var sb = new StringBuilder();
        sb.AppendLine("/**");
        sb.AppendLine($" * {descriptor.Help}");
        sb.AppendLine(" *");
        foreach (var parameter in descriptor.Parameters)
        {
            sb.AppendLine($" * @param {parameter.ParameterName} {GetTypeScriptType(parameter.RawParameterType)}");
        }

        sb.AppendLine($" * @returns {GetTypeScriptType(descriptor.RawReturnType)}");
        sb.AppendLine($" */");

        return sb.ToString();
    }

    public static string GenerateTypeDefinitionOfEnum<TEnum>(List<TEnum> enums) where TEnum : struct, Enum, IConvertible
    {
        var sb = new StringBuilder();
        foreach (var enumValue in enums)
        {
            sb.AppendLine(GenerateTypeDefinitionOfEnum(enumValue));
        }

        return sb.ToString();
    }

    public static string GenerateTypeDefinitionOfEnum<TEnum>(TEnum enumValue) where TEnum : struct, Enum, IConvertible
    {
        var sb = new StringBuilder();
        sb.AppendLine($"declare enum {typeof(TEnum).Name} {{");
        foreach (var enumVal in FastEnum.GetMembers<TEnum>())
        {
            sb.AppendLine($"  {enumVal.Name} = {enumVal.Value.ToString("D")},");
        }

        sb.AppendLine("}");
        sb.AppendLine("");

        return sb.ToString();
    }

    public static string GenerateTypeDefinitionOfEnum<TEnum>() where TEnum : struct, Enum, IConvertible
    {
        var sb = new StringBuilder();
        sb.AppendLine($"declare enum {typeof(TEnum).Name} {{");
        foreach (var enumValue in FastEnum.GetMembers<TEnum>())
        {
            sb.AppendLine($"  {enumValue.Name} = {enumValue.Value.ToString("D")},");
        }

        sb.AppendLine("}");
        sb.AppendLine("");

        return sb.ToString();
    }


    public static string GenerateTypeDefinitionOfConstants(Dictionary<string, object> constants)
    {
        var sb = new StringBuilder();
        sb.AppendLine("");
        sb.AppendLine("// Declare constants");
        foreach (var constant in constants)
        {
            if (constant.Value is int)
            {
                sb.AppendLine($"declare const {constant.Key}: number;");
            }

            if (constant.Value is uint)
            {
                sb.AppendLine($"declare const {constant.Key}: number;");
            }

            if (constant.Value is short)
            {
                sb.AppendLine($"declare const {constant.Key}: number;");
            }
            else if (constant.Value is string)
            {
                sb.AppendLine($"declare const {constant.Key}: string;");
            }
            else
            {
                // Logger.LogWarning("Unknown type of constant {ConstantName}", constant.Key);
            }
        }

        return sb.ToString();
    }

    public static string GenerateTypeDefinitionOfFunction(ScriptFunctionDescriptor descriptor)
    {
        var sb = new StringBuilder();
        sb.AppendLine(GenerateFunctionComment(descriptor));
        sb.AppendLine($"declare function {descriptor.FunctionName}(");
        foreach (var parameter in descriptor.Parameters)
        {
            sb.AppendLine($"  {parameter.ParameterName}: {GetTypeScriptType(parameter.RawParameterType)},");
        }

        sb.AppendLine($"): {GetTypeScriptType(descriptor.RawReturnType)};");
        sb.AppendLine("");

        return sb.ToString();
    }

    public static string GenerateInterface(Type type)
    {
        var sb = new StringBuilder();

        if (type.GenericTypeArguments != null && type.GenericTypeArguments.Length > 0)
        {
            foreach (var generic in type.GenericTypeArguments)
            {
                sb.AppendLine(GenerateInterface(generic));
            }

            return sb.ToString();
        }

        if (type == typeof(Func<>))
        {
            return GenerateInterface(type.GetGenericTypeDefinition());
        }

        sb.AppendLine($"declare interface {type.Name} {{");
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            sb.AppendLine($"  {property.Name}: {GetTypeScriptType(property.PropertyType)};");
        }

        foreach (var method in type.GetMethods())
        {
            sb.Append($"  {method.Name}(");
            foreach (var parameter in method.GetParameters())
            {
                sb.Append($"{parameter.Name}: {GetTypeScriptType(parameter.ParameterType)}, ");
            }

            sb.AppendLine($"): {GetTypeScriptType(method.ReturnType)};");
        }

        sb.AppendLine("}");
        sb.AppendLine("");

        return sb.ToString();
    }

    public static string GenerateInterfaces(List<Type> types)
    {
        var strOutput = new StringBuilder();
        var generationQueue = new List<Type>(types);
        foreach (var type in types)
        {
            foreach (var method in type.GetMethods())
            {
                generationQueue.Add(method.ReturnType);
                foreach (var parameter in method.GetParameters())
                {
                    generationQueue.Add(parameter.ParameterType);
                }
            }
        }

        generationQueue = generationQueue.Where(
                s => s != typeof(object)
                     && !s.ToString().Contains("Void")
                     && s != typeof(string)
                     && s != typeof(bool)
                     && s != typeof(Int16)
                     && s != typeof(Int32)
                     && s != typeof(Int64)
                     && s != typeof(int)
                     && s != typeof(Type)
                     && s != typeof(object[])
            )
            .Distinct()
            .ToList();

        foreach (var type in generationQueue)
        {
            strOutput.AppendLine(GenerateInterface(type));
        }


        return strOutput.ToString();
    }
}
