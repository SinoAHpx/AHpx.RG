using System.Reflection;

namespace AHpx.RG.Core.Utils;

public static class AssemblyUtils
{
    public static List<Type>? GetTypes(string? dllPath)
    {
        if (dllPath != null)
        {
            var assembly = Assembly.LoadFile(dllPath);

            try
            {
                return assembly.GetTypes().Where(x => x.IsPublic).ToList();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(x => x != null).Where(x => x.IsPublic).ToList()!;
            }
            ;
        }

        return null;
    }
}