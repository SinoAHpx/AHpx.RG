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

    public static string GetSignature(this Type type)
    {
        if (!type.IsGenericType)
            return type.GetFullname();

        return type.GetGenericTypeSignature();
    }

    private static string GetGenericTypeSignature(this Type type)
    {
        var signature = type
            .GetGenericArguments()
            .Select(t => t.GetGenericTypeSignature())
            .JoinToString(",");;

        return $"{type.GetFullname(false)}{{{signature}}}";
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