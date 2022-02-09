using System.Reflection;

namespace AHpx.RG.Core.Utils;

public static class AssemblyUtils
{
    public static List<Type>? GetTypes(string? dllPath)
    {
        if (dllPath != null)
        {
            var assembly = Assembly.LoadFile(dllPath);

            return assembly.GetTypes().Where(x => x.IsPublic).ToList();
        }

        return null;
    }
}