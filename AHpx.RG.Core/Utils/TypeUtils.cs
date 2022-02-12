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
    
    
}