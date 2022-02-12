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

    public static List<XElement> GetCandidates(this MethodInfo method)
    {
        var candidates = Global.XmlMembers.WhereNameContains(method.Name);

        return candidates;
    }

    public static string GetSignature(this MethodInfo method)
    {
        var type = method.DeclaringType!;
        var signature = $"{type.GetFullname()}.{method.Name}";
        var genericArgs = method.GetGenericArguments().ToList();
        
        if (method.IsGenericMethod)
            signature += $"``{genericArgs.Count}";

        signature += method.GetParametersSignature();
        
        return signature;
    }

    private static string GetParametersSignature(this MethodInfo method)
    {
        var parameters = method.GetParameters();
        if (parameters.Length == 0)
            return string.Empty;

        
        var signature = "";
        var type = method.DeclaringType!;
        
        var typeGenericArgs = type.GetGenericArguments().ToList();
        var genericArgs = method.GetGenericArguments().ToList();
        
        
        signature += "(";

        if (!parameters.Any(p => genericArgs.Contains(p.ParameterType)) &&
            !parameters.Any(p => typeGenericArgs.Contains(p.ParameterType)) &&
            !parameters.Any(p => p.ParameterType.IsGenericType))
        {
           signature += parameters
                .Select(x => $"{x.ParameterType.Namespace}.{x.ParameterType.Name}")
                .JoinToString(",");
        }
        else
        {
            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterType;
                if (genericArgs.Contains(parameterType))
                    signature += $"``{genericArgs.IndexOf(parameterType)}";
                else if (typeGenericArgs.Contains(parameterType))
                    signature += $"`{typeGenericArgs.IndexOf(parameterType)}";
                else
                    signature += parameter.ParameterType.GetGenericParameterSignature();

                signature += ",";
            }

            signature = signature.Trim(',');
        }

        

        signature += ")";

        return signature;
    }

    public static string GetGenericParameterSignature(this Type parameterType)
    {
        var signature = $"{parameterType.GetFullname(false)}";

        if (parameterType.IsGenericType)
        {
            signature += "{";
            var genericArgs = parameterType.GetGenericArguments();

            foreach (var genericArg in genericArgs)
            {
                signature += genericArg.GetGenericParameterSignature();
                signature += ",";
            }

            signature = signature.Trim(',');

            signature += "}";
        }
        
        return signature;
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="withBacktick">with `, default is true</param>
    /// <returns></returns>
    private static string GetFullname(this Type type, bool withBacktick = true)
    {
        var name = type.Name;
        if (!withBacktick && name.Contains('`'))
        {
            var toEmtpy = $"`{name.SubstringAfter("`")}";
            name = name.Empty(toEmtpy);
        }

        return $"{type.Namespace}.{name}";
    }
}