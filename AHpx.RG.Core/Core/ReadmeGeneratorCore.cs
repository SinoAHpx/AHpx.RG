using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using AHpx.RG.Core.Utils;
using Manganese.Text;

namespace AHpx.RG.Core.Core;

public class ReadmeGeneratorCore
{
    #region Fields and properties

    public string? CompiledDllPath { get; set; }

    public string? XmlDocumentationPath { get; set; }

    public List<Type>? Types => AssemblyUtils.GetTypes(CompiledDllPath);

    public XDocument XDocument => XDocument.Parse(File.ReadAllText(XmlDocumentationPath!));

    private IEnumerable<XElement?> MemberElements => XDocument.Descendants("member");

    #endregion

    #region Mapper

    private XElement? Map(Type type)
    {
        var re = MemberElements
            .Where(e => e?.Attribute("name")!.Value.StartsWith("T") is true)
            .First(e => e?
                .Attribute("name")!.Value.SubstringAfter(":")
                .Split(".").Last() == type.Name);

        return re;
    }

    private XElement? Map(MethodInfo method)
    {
        var candidates = MemberElements
            .Where(e => e?.Attribute("name")?.Value.StartsWith("M") is true)
            .Where(e => e?.Attribute("name")?.Value.Contains(method.Name) is true);

        var paramInfos = method.GetParameters();

        foreach (var candidate in candidates)
        {
            var paramNames = candidate?.Elements("param")
                .Select(z => z.Attribute("name")?.Value);

            if (paramInfos.Select(x => x.Name)
                .All(x => paramNames?.Contains(x) is true))
            {
                return candidate;
            }
        }

        throw new ArgumentException($"Method {method.Name} has no summary in {method.GetParameters().JoinToString(",")}");

        // return null;
    }

    private XElement? Map(ParameterInfo parameter, MethodInfo methodInfo)
    {
        var methodElement = Map(methodInfo);

        return methodElement?
            .Elements("param")
            .First(e => e.Attribute("name")?.Value == parameter.Name);
    }

    #endregion

    #region Generating markdown

    private string GetContent(Type type, string header)
    {
        var typeElement = Map(type);
        var typeContent = new StringBuilder($"### {header}{Environment.NewLine}");

        var typeSummaryElement = typeElement?.Element("summary");
        var typeSummary = typeSummaryElement!.Value.Trim()
            .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
            .Where(x => !x.IsNullOrEmpty())
            .Select(x => x.Trim())
            .JoinToString(Environment.NewLine);

        typeContent.Append(typeSummary);
        typeContent.Append(Environment.NewLine);

        var methods = type.GetMethods().Where(x => x.IsPublic);
        foreach (var methodInfo in methods)
        {
            typeContent.Append(GetContent(methodInfo, type));
        }

        typeContent.Append(Environment.NewLine);

        return typeContent.ToString().Split(Environment.NewLine).Where(x => !x.IsNullOrEmpty())
            .JoinToString(Environment.NewLine);
    }

    public string GetContent(Type type)
    {
        return GetContent(type, type.Name);
    }

    public string GetContent(Type type, Uri uri)
    {
        var link = $"{uri}";

        if (!link.EndsWith("/"))
            link += "/";

        return GetContent(type, $"[{type.Name}]({link}{type.Namespace?.Split(".").JoinToString("/")}/{type.Name}.cs)");
    }

    public string GetContent(MethodInfo methodInfo, Type type)
    {
        try
        {
            var methodElement = Map(methodInfo);
            var methodContent = new StringBuilder($"+ ```{methodInfo.Name}({methodInfo.ReturnType.Name})```: ");
            methodContent.Append(methodElement?.Element("summary")?.Value.Trim());
            methodContent.Append(Environment.NewLine);

            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                methodContent.Append(GetContent(parameterInfo, methodInfo));
            }

            methodContent.Append(Environment.NewLine);

            return methodContent.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return string.Empty;
        }
        
    }

    public string GetContent(ParameterInfo parameterInfo, MethodInfo methodInfo)
    {
        var parameterElement = Map(parameterInfo, methodInfo);
        var parameterContent =
            new StringBuilder($"\t+ ```{parameterInfo.Name}({parameterInfo.ParameterType.Name})```");

        if (!parameterElement?.Value.IsNullOrEmpty() is true)
            parameterContent.Append($": {parameterElement?.Value.Trim()}");

        parameterContent.Append(Environment.NewLine);

        return parameterContent.ToString();
    }

    #endregion
}