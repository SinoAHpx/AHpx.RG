﻿using System.Reflection;

namespace AHpx.RG.Core.Utils;

public static class AssemblyUtils
{
    public static List<Type> GetTypes(string dllPath)
    {
        var assembly = Assembly.LoadFile(dllPath);

        return assembly.GetTypes().Where(x => x.IsPublic).ToList();
    }
}