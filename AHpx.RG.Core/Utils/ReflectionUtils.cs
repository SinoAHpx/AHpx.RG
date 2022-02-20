using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using System.Xml.Linq;
using System.Xml.Serialization;
using AHpx.RG.TestLib;
using Manganese.Text;

namespace AHpx.RG.Core.Utils;

public static class ReflectionUtils
{
    public static AssemblyLoadContext LoadContext { get; set; } = new("default-context", true);

    /// <summary>
    /// Get public types from compiled library
    /// </summary>
    /// <returns></returns>
    public static List<Type> GetTypes()
    {
        var types = new List<Type>();
        try
        {
            var assembly = LoadContext.LoadFromAssemblyPath(Global.Config.CompiledLibraryPath);
            types.AddRange(assembly.GetTypes());
        }
        catch (ReflectionTypeLoadException e)
        {
            types.AddRange(e.Types!);
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        return types.Where(x => x != null).Where(x => x.IsPublic).ToList();
    }

    public static void LoadDependency(IEnumerable<string> dependencyPaths)
    {
        foreach (var path in dependencyPaths.Where(s => !s.IsNullOrEmpty()))
        {
            LoadContext.LoadFromAssemblyPath(path);
        }
    }

    public static void ReloadDependency(IEnumerable<string> toReload)
    {
        LoadContext = new AssemblyLoadContext("reload-context", true);

        LoadDependency(toReload);
    }

    public static T? GetMemberOf<T>(this MemberInfo memberInfo) where T : MemberInfo
    {
        try
        {
            return memberInfo as T;
        }
        catch
        {
            return null;
        }
    }

    public static string GetSignaturePrefix(this MemberInfo memberInfo)
    {
        if (memberInfo.MemberType == MemberTypes.Constructor)
            return "M:";
        if (memberInfo.MemberType == MemberTypes.NestedType)
            return "T:";

        return $"{memberInfo.MemberType.ToString()[0]}:";
    }

    public static bool HasElement(this MemberInfo memberInfo) =>
        Global.XmlMembers
            .Where(x => x!.Attribute("name")!.Value.StartsWith(memberInfo.GetSignaturePrefix()))
            .Any(x => x!.Attribute("name")!.Value
                .SubstringAfter(memberInfo.GetSignaturePrefix()).Split(".")
                .Last() == memberInfo.Name);

    public static string GetMemberSignature(this MemberInfo memberInfo)
    {
        if (memberInfo.DeclaringType == typeof(object))
            return string.Empty;
        
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Method:
                return memberInfo.GetMemberOf<MethodInfo>()!.GetSignature();
            case MemberTypes.Constructor:
                return memberInfo.GetMemberOf<ConstructorInfo>()!.GetSignature();
            case MemberTypes.TypeInfo:
                return memberInfo.GetMemberOf<Type>()!.GetSignature();
            default:
                return $"{memberInfo.GetSignaturePrefix()}{memberInfo.DeclaringType!.GetFullname()}.{memberInfo.Name}";
        }
    }
    
    public static XElement? GetElement(this MemberInfo memberInfo)
    {
        var signature = memberInfo.GetMemberSignature();
        if (string.IsNullOrEmpty(signature))
            return null;

        return Global.XmlMembers.FirstOrDefault(x => x!.Attribute("name")!.Value == signature, null);
    }
}