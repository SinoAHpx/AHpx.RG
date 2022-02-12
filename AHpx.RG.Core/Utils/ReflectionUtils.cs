using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Manganese.Text;

namespace AHpx.RG.Core.Utils;

public static class ReflectionUtils
{
    /// <summary>
    /// Get public types from compiled library
    /// </summary>
    /// <returns></returns>
    public static List<Type>? GetTypes()
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
}