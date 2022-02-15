using System.Reflection;
using System.Xml.Linq;
using Manganese.Text;

namespace AHpx.RG.Core.Utils;

public static class MethodUtils
{
    /// <summary>
    /// Just check if it has a xml element in documentation
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static bool HasElement(this MethodInfo method)
    {
        var candidates = method.GetCandidates();

        return candidates.Any();
    }

    private static List<XElement> GetCandidates(this MethodInfo method)
    {
        var candidates = Global.XmlMembers.WhereNameContains(method.Name);

        return candidates;
    }

    public static string GetSignature(this MethodInfo method)
    {
        var signature = $"{method.GetFullname()}";

        var parameters = method.GetParameters();
        if (parameters.Length != 0)
            signature += $"({method.GetParameters().Select(p => p.GetSignature<MethodInfo>()).JoinToString(",")})";

        return $"{method.GetSignaturePrefix()}{signature}";
    }

    public static string GetSignature<T>(this ParameterInfo parameterInfo) where T : MethodBase
    {
        var parameterType = parameterInfo.ParameterType;
        var methodInfo = (parameterInfo.Member as T)!;
        var declaringType = methodInfo.DeclaringType!;

        var methodGenericArgs = typeof(T) == typeof(ConstructorInfo)
            ? new List<Type>()
            : methodInfo.GetGenericArguments().ToList();
        var typeGenericArgs = declaringType.GetGenericArguments().ToList();
        
        if (!parameterType.IsGenericType)
            return $"{parameterType.GetGenericParameterSignature(methodGenericArgs, typeGenericArgs)}";

        return $"{parameterType.GetFullname(false)}" +
               $"{{{parameterInfo.GetGenericParameterSignature(methodGenericArgs, typeGenericArgs)}}}";
    }
    
    
    public static string GetSignature(this ConstructorInfo constructorInfo)
    {
        var signature = constructorInfo.GetFullname();
        
        var parameters = constructorInfo.GetParameters();
        if (parameters.Length != 0)
            signature +=
                $"({constructorInfo.GetParameters().Select(p => p.GetSignature<ConstructorInfo>()).JoinToString(",")})";

        return $"{constructorInfo.GetSignaturePrefix()}{signature}";
    }
    
    private static string GetGenericParameterSignature(this ParameterInfo parameterInfo, List<Type> methodGenericArgs, List<Type> typeGenericArgs)
    {
        var parameterType = parameterInfo.ParameterType;

        var signatures = new List<string>();
        var parameterGenericArgs = parameterType.GetGenericArguments().ToList();

        parameterGenericArgs.ForEach(t =>
            signatures.Add(t.GetGenericParameterSignature(methodGenericArgs, typeGenericArgs)));

        return signatures.JoinToString(",");
    }

    public static string GetGenericParameterSignature(this Type type, List<Type> methodGenericArgs, List<Type> typeGenericArgs)
    {
        string signature;

        if (!type.IsGenericType)
        {
            if (methodGenericArgs.Contains(type))
                signature = $"``{methodGenericArgs.IndexOf(type)}";
            else if (typeGenericArgs.Contains(type))
                signature = $"`{typeGenericArgs.IndexOf(type)}";
            else
                signature = $"{type.GetFullname(false)}";

            return signature;
        }

        signature = type.GetGenericArguments()
            .Select(t => t.GetGenericParameterSignature(methodGenericArgs, typeGenericArgs))
            .JoinToString(",");

        return $"{type.GetFullname(false)}{{{signature}}}";
    }

    private static string GetFullname(this MethodInfo method)
    {
        var type = method.DeclaringType!;
        var name = $"{type.GetFullname()}.{method.Name}";
        var genericArgs = method.GetGenericArguments().ToList();
        
        if (method.IsGenericMethod)
            name += $"``{genericArgs.Count}";

        return name;
    }
    
    
    public static string GetFullname(this ConstructorInfo constructorInfo)
    {
        var type = constructorInfo.DeclaringType!;

        return $"{type.GetFullname()}.{constructorInfo.Name.Replace(".", "#")}";
    }
}