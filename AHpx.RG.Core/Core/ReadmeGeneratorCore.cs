using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Linq;
using AHpx.RG.Core.Utils;
using Manganese.Array;
using Manganese.Text;

namespace AHpx.RG.Core.Core;

public class ReadmeGeneratorCore
{
    public string CompileLibraryPath
    {
        get => Global.Config.CompiledLibraryPath;
        set => Global.Config.CompiledLibraryPath = value.ThrowIfNullOrEmpty("Library path cannot be null or empty.");
    }

    public string XmlDocumentationPath
    {
        get => Global.Config.XmlDocumentationPath;
        set => Global.Config.XmlDocumentationPath =
            value.ThrowIfNullOrEmpty("Xml documentation path cannot be null or empty.");
    }
    
    

    // private List<string> MarkdownLines { get; set; } = new();
    //
    // public string GetContent(IEnumerable<Type> types, string? url = null)
    // {
    //     foreach (var type in types)
    //     {
    //         MarkdownLines.Add(GenerateTypeContent(type, url));
    //     }
    //
    //     return MarkdownLines.JoinToString(Environment.NewLine);
    // }
    //
    // #region Type
    //
    // private string GenerateTypeContent(Type type, string? appendix)
    // {
    //     var readme = new StringBuilder();
    //
    //     if (appendix!.IsNullOrEmpty())
    //         readme.AppendLine($"### {type.Name}");
    //     else
    //         readme.AppendLine($"[{type.Name}]({appendix})");
    //
    //     var element = type.GetElement();
    //     if (element != null)
    //         readme.AppendLine(ParseTypeSummary(element));
    //
    //     type.GetMembers().ToList().ForEach(m => readme.AppendLine(GenerateMemberContent(m)));
    //     
    //     return readme.ToString();
    // }
    //
    // private string GenerateMemberContent(MemberInfo memberInfo)
    // {
    //     var readme = new StringBuilder();
    //     var element = memberInfo.GetElement();
    //     
    //     if (element != null)
    //     {
    //         switch (memberInfo.MemberType)
    //         {
    //             case MemberTypes.Method when memberInfo is MethodInfo methodInfo:
    //                 readme.AppendLine(ParseMethodSummary(element, methodInfo));
    //                 break;
    //         }
    //     }
    //     else
    //     {
    //         readme.AppendLine($"- ```{memberInfo.GetSignaturePrefix()}{memberInfo.Name}```");
    //     }
    //
    //     return readme.ToString();
    // }
    //
    // #endregion
    //
    // #region Inline parsers
    //
    // private string ParseTypeSummary(XElement element)
    // {
    //     var re = new List<string>();
    //     re.Add(GetSummaryContent(element, true));
    //
    //     element.Elements()
    //         .RemoveIf(x => x.Name.ToString() == "summary")
    //         .ToList()
    //         .ForEach(x => re.Add(GetElementContent(x)));
    //
    //     return re
    //         .Where(x => !x.IsNullOrEmpty())
    //         .JoinToString(Environment.NewLine);
    // }
    //
    // private string ParseMethodSummary(XElement element, MethodInfo methodInfo)
    // {
    //     var readme = new StringBuilder($"- ```{methodInfo.Name}({methodInfo.ReturnType.Name})```");
    //     var summaryContent = GetSummaryContent(element);
    //     if (!summaryContent.IsNullOrEmpty())
    //         readme.Append($": {summaryContent}");
    //     
    //     var parameters = methodInfo.GetParameters();
    //     var childElements = element.Elements();
    //
    //     if (parameters.Length != 0)
    //     {
    //         var mapped = parameters.Select(x =>
    //         {
    //             (ParameterInfo, XElement?) re;
    //             var xElements = childElements.ToList();
    //             if (x.Name.IsIn(xElements.Select(z => z.Attribute("name")?.Value)))
    //                 re = (x, xElements.First(z => z.Attribute("name")?.Value == x.Name));
    //             else
    //                 re = (x, null);
    //
    //             return re;
    //         });
    //
    //         foreach (var (parameterInfo, paramElement) in mapped)
    //         {
    //             readme.Append($"\t- ```{parameterInfo.Name}({parameterInfo.ParameterType.Name})```");
    //
    //             if (paramElement != null)
    //                 readme.Append($": {paramElement.Value}");
    //
    //             readme.AppendLine();
    //         }
    //     }
    //
    //     return readme.ToString();
    // }
    //
    // private string GetElementContent(XElement element, string? prefix = null)
    // {
    //     var re = $"```{prefix}{element.Name.ToString().CapitalizeInitial()}```";
    //     if (!element.Value.IsNullOrEmpty())
    //         re += $": {element.Value}";
    //
    //
    //     return re;
    // }
    //
    // private string GetSummaryContent(XElement element, bool withChildren = false)
    // {
    //     var summary = element.Element("summary");
    //     var re = new StringBuilder();
    //     
    //     if (summary != null)
    //     {
    //         var toAdd = summary.Value.Trim();
    //         if (summary.HasElements)
    //         {
    //             var toEmpty = summary.Elements().Select(x => x.Value.Trim()).JoinToString(string.Empty);
    //             toAdd = toAdd.Empty(toEmpty);
    //         }
    //
    //         re.AppendLine(toAdd);
    //
    //         if (withChildren)
    //             summary.Elements().ToList().ForEach(x => re.AppendLine(GetElementContent(x)));
    //
    //     }
    //
    //     return re.ToString();
    // }
    //
    // #endregion
}