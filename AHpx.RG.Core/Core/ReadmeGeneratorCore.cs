using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.PortableExecutable;
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

    public List<Type>? Types => ReflectionUtils.GetTypes();

    public XDocument XDocument => XDocument.Parse(File.ReadAllText(XmlDocumentationPath!));

    private IEnumerable<XElement?> MemberElements => XDocument.Descendants("member");

    #endregion

    #region Checker

    public bool HasSummary(Type type) =>
        MemberElements
            .Where(x => x!.Attribute("name")!.Value.StartsWith("T:"))
            .Any(x => x!.Attribute("name")!.Value
                .SubstringAfter("T:").Split(".")
                .Last() == type.Name);

    public bool HasSummary(MethodInfo methodInfo)
    {
        var parameterTypes = methodInfo
            .GetParameters()
            .Select(x => x.ParameterType.FullName)
            .ToList();

        var signaure = $"{methodInfo.DeclaringType?.FullName}.{methodInfo?.Name}({parameterTypes.JoinToString(",")})";

        
        var candidates = MemberElements
            .Where(x => x!.Attribute("name")!.Value.StartsWith("M:"))
            .Where(x => x!.Attribute("name")!.Value.Contains(methodInfo.Name))
            .ToList();

        if(!candidates.Any())
            return false;

        if (candidates.Count == 1)
            return true;

        foreach (var candidate in candidates)
        {
            // var candidateName = candidate!.Attribute("name")!.Value;
            // Console.WriteLine($"Signature: {signaure}");
            // Console.WriteLine($"Candidate: {candidateName}");
        }

        return false;
    }

    private string GetMethodSignature(MethodInfo methodInfo)
    {
        var signature = $"{methodInfo.DeclaringType?.FullName}.{methodInfo.Name}";
        if (methodInfo.IsGenericMethod)
            signature += $"``{methodInfo.GetGenericArguments().Length}";

        // methodInfo.GetParameters().Where(x => )
        // methodInfo.GetParameters()
        return null;
    }

    #endregion

    #region Mapper

    private XElement? Map(Type type)
    {
        try
        {
            var re = MemberElements
                .Where(e => e?.Attribute("name")!.Value.StartsWith("T") is true)
                .First(e => e?
                    .Attribute("name")!.Value.SubstringAfter(":")
                    .Split(".").Last() == type.Name);

            return re;
        }
        catch
        {
            return null;
        }
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

        throw new ArgumentException(
            $"Method {method.Name} has no summary in {method.GetParameters().JoinToString(",")}");

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
        if (typeElement == null)
            return string.Empty;

        var typeContent = new StringBuilder($"### {header}{Environment.NewLine}");

        var typeSummaryElement = typeElement?.Element("summary");
        var typeSummary = typeSummaryElement!.Value.Trim()
            .Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None)
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