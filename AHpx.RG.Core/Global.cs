using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using Manganese.Text;

namespace AHpx.RG.Core;

public static class Global
{
    public static GlobalConfig Config { get; set; } = null!;

    public static List<XElement> XmlMembers => XDocument
        .Parse(File.ReadAllText(Config.XmlDocumentationPath))
        .Descendants("member")
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        .Where(x => x != null)
        .ToList();

    /// <summary>
    /// Match attribute name
    /// </summary>
    /// <param name="source"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<XElement> WhereNameContains(this List<XElement> source, string name)
    {
        return source
            .Where(x => x.Attribute("name")!.Value.Contains(name))
            .ToList();
    }
}

public class GlobalConfig
{
    private string _compiledLibraryPath;

    public string CompiledLibraryPath
    {
        get => _compiledLibraryPath;
        set => _compiledLibraryPath = value.ThrowIfNullOrEmpty("Invalid compiled library path");
    }

    public string XmlDocumentationPath { get; set; }
}