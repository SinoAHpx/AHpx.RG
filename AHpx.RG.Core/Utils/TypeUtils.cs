using System.Xml.Linq;
using Manganese.Text;

namespace AHpx.RG.Core.Utils;

public static class TypeUtils
{
    public static bool HasElement(this Type type) =>
        Global.XmlMembers
            .Where(x => x!.Attribute("name")!.Value.StartsWith("T:"))
            .Any(x => x!.Attribute("name")!.Value
                .SubstringAfter("T:").Split(".")
                .Last() == type.Name);

    public static XElement? GetElement(this Type type)
    {
        if (!type.HasElement())
            return null;

        return Global.XmlMembers
            .FirstOrDefault(x => x!.Attribute("name")!.Value == type.GetSignature(), null);
    }
    
    public static string GetSignature(this Type type)
    {
        return $"T:{type.GetFullname()}";
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