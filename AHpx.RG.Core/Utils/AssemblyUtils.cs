using System.Reflection;
using Manganese.Text;

namespace AHpx.RG.Core.Utils;

public static class AssemblyUtils
{
    public static List<Type>? GetTypes(string? dllPath)
    {
        if (!dllPath!.IsNullOrEmpty())
        {
            var types = new List<Type>();
            try
            {
                var assembly = Assembly.LoadFile(dllPath!);
                types.AddRange(assembly.GetTypes());
            }
            catch (ReflectionTypeLoadException e)
            {
                types.AddRange(e.Types!);
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return types.Where(x => x != null).Where(x => x.IsPublic).ToList();
        }

        return null;
    }
}