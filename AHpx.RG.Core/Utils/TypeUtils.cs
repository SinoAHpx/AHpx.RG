using System.Reflection;
using System.Xml.Linq;
using Manganese.Text;

namespace AHpx.RG.Core.Utils;

public static class TypeUtils
{
    public static string GetSignature(this Type type)
    {
        return $"{type.GetSignaturePrefix()}{type.GetFullname()}";
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="withBacktick">with `, default is true</param>
    /// <returns></returns>
    public static string GetFullname(this Type type, bool withBacktick = true)
    {
        var name = type.Name;
        if (!withBacktick && name.Contains('`'))
        {
            var toEmpty = $"`{name.SubstringAfter("`")}";
            name = name.Empty(toEmpty);
        }
        
        return $"{type.Namespace}.{name}";
    }
}