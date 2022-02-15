﻿using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using AHpx.RG.TestLib;
using Manganese.Text;

namespace AHpx.RG.Core.Utils;

public static class ReflectionUtils
{
    /// <summary>
    /// Get public types from compiled library
    /// </summary>
    /// <returns></returns>
    public static List<Type> GetTypes()
    {
        var types = new List<Type>();
        try
        {
            var assembly = Assembly.LoadFile(Global.Config.CompiledLibraryPath);
            types.AddRange(assembly.GetTypes());
        }
        catch (ReflectionTypeLoadException e)
        {
            types.AddRange(e.Types!);
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        return types.Where(x => x != null).Where(x => x.IsPublic).ToList();
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
        
        if (memberInfo.MemberType == MemberTypes.Method)
        {
            var method = memberInfo.GetMemberOf<MethodInfo>();
        }

        return null;
    }
}