using System.Reflection;
using System.Text;
using System.Xml.Linq;
using AHpx.RG.Core.Data;
using AHpx.RG.Core.Utils;
using Manganese.Text;
using Markdown;

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

    public ReadmeGeneratorConfig GeneratorConfig { get; set; } = new();
    
    #region General agent

    public string GetDocument(IEnumerable<Type> types, string repositoryUrl)
    {
        var document = new StringBuilder();

        foreach (var type in types)
            document.AppendLine(GetTypeDocument(type, repositoryUrl));
        
        var re = document.ToString();
        re = re.Split(Environment.NewLine)
            .Where(s => !s.IsNullOrEmpty())
            .JoinToString(Environment.NewLine);
        
        return re;
    }

    #endregion

    #region Chain

    #region Type readme generator

    private string GetTypeDocument(Type type, string url)
    {
        var document = new MarkdownDocument();
        if (url.IsNullOrEmpty())
            document.AppendHeader(type.Name, GeneratorConfig.TypeHeaderSize);
        else
            document.AppendHeader(new MarkdownLink(type.Name, url), GeneratorConfig.TypeHeaderSize);

        document.AppendParagraph(GetTypeContent(type));

        var members = type.GetMembers()
            .Where(x => x.MemberType != MemberTypes.NestedType)
            .ToList();
        document.AppendParagraph(ShuntMembersDocument(members));
            

        return document.ToString();
    }

    private string GetTypeContent(Type type)
    {
        var element = type.GetElement();
        if (element == null)
            return string.Empty;
        
        var builder = new StringBuilder();
        
        var summaryElement = element.Element("summary");
        var summaryContent = GetTypeSummaryContent(summaryElement);
        if (!summaryContent.IsNullOrEmpty())
            builder.AppendLine(summaryContent);

        var typeParamElements = element.Elements("typeparam").ToList();
        if (typeParamElements.Count != 0)
        {
            builder.AppendLine(new MarkdownList(typeParamElements.Select(x =>
            {
                var re = $"```{x.Attribute("name")!.Value}```";
                if (!x.Value.IsNullOrEmpty())
                    re += $": {x.Value}";

                return re;
            }).ToArray()).ToString());
        }

        return builder.ToString();
    }

    private string GetTypeSummaryContent(XElement? summaryElement)
    {
        if (summaryElement == null)
            return string.Empty;

        var builder = new StringBuilder();
        builder.AppendLine(GetPureSummaryValue(summaryElement));

        if (summaryElement.HasElements)
        {
            var subElements = summaryElement.Elements();
            var toAppend = subElements.Select(GetTypeSubSummaryValue);

            builder.AppendLine(new MarkdownList(toAppend.ToArray()).ToString());
        }

        return builder.ToString();
    }

    private string GetTypeSubSummaryValue(XElement subSummaryElement)
    {
        var listContent = $"{subSummaryElement.Name.ToString().CapitalizeInitial()}";

        if (!subSummaryElement.Value.IsNullOrEmpty())
            listContent += $": {subSummaryElement.Value.Trim()}";

        return listContent;
    }

    #endregion

    #region Shunter

    private string ShuntMembersDocument(List<MemberInfo> memberInfos)
    {
        var builder = new StringBuilder();
        
        //constructors
        var constructors = memberInfos
            .OfType<ConstructorInfo>()
            .Where(c => c.IsPublic)
            .Where(m => m.DeclaringType != typeof(object))
            .Where(m => m.DeclaringType != typeof(Enum))
            .ToList();

        if (constructors.Any())
            builder.AppendLine(GetMemberDocument(constructors, GetConstructorDocument));

        //properties

        var properties = memberInfos
            .OfType<PropertyInfo>()
            .Where(p => p.GetMethod?.IsPublic is true || p.SetMethod?.IsPublic is true)
            .ToList();

        if (properties.Any())
            builder.AppendLine(GetMemberDocument(properties, GetPropertyDocument));
        
        //fields
        var fields = memberInfos
            .OfType<FieldInfo>()
            .Where(f => f.IsPublic)
            .ToList();
        
        if (fields.Any())
            builder.AppendLine(GetMemberDocument(fields, GetFieldDocument));
        
        //events
        
        var events = memberInfos
            .OfType<EventInfo>()
            .Where(e => e.AddMethod?.IsPublic is true || e.RemoveMethod?.IsPublic is true)
            .ToList();

        if (events.Any())
            builder.AppendLine(GetMemberDocument(events, GetEventDocument));

            //methods
        var methods = memberInfos
            .OfType<MethodInfo>()
            .Where(m => m.IsPublic)
            .Where(m => m.DeclaringType != typeof(object))
            .Where(m => m.DeclaringType != typeof(Enum))
            .Where(IsNotGetterOrSetter)
            .Where(IsNotAdderOrRemover)
            .ToList();
        
        if (methods.Any())
            builder.AppendLine(GetMemberDocument(methods, GetMethodDocument));

        return builder.ToString();
    }

    private string GetMemberDocument<T>(IEnumerable<T> memberInfos, Func<T, string> handler) where T : MemberInfo
    {
        var builder = new StringBuilder();

        builder.AppendLine(new MarkdownHeader(typeof(T).Name.Empty("Info") + "s", GeneratorConfig.TypeSubtitleSize)
            .ToString());

        foreach (var methodInfo in memberInfos)
            builder.AppendLine(handler(methodInfo));

        return builder.ToString();
    }
    
    #region Methods readme generator

    private string GetMethodDocument(MethodInfo methodInfo)
    {
        var builder = new StringBuilder();
        var methodElement = methodInfo.GetElement();

        builder.AppendLine($"- {GetMethodContent(methodInfo, methodElement)}");

        var parameters = methodInfo.GetParameters().ToList();
        if (parameters.Count != 0)
            foreach (var parameterInfo in parameters)
                builder.AppendLine($"\t- {GetParameterDocument(parameterInfo, methodElement)}");
        
        return builder.ToString();
    }

    private string GetMethodContent(MethodInfo methodInfo, XElement? methodElement)
    {
        var builder = new StringBuilder();

        builder.Append($"```{methodInfo.Name}");
        if (methodInfo.ReturnType == typeof(void))
            builder.Append("```");
        else
            builder.Append($"({methodInfo.ReturnType.Name})```");

        var summaryElement = methodElement?.Element("summary");
        if (summaryElement != null)
        {
            var pureSummaryValue = GetPureSummaryValue(summaryElement);
            if (!pureSummaryValue.IsNullOrEmpty())
                builder.Append($": {GetPureSummaryValue(summaryElement)}");
        }

        return builder.ToString();
    }

    private string GetParameterDocument(ParameterInfo parameterInfo, XElement? methodElement)
    {
        var builder = new StringBuilder();
        builder.Append($"```{parameterInfo.Name}({parameterInfo.ParameterType.Name})```");

        if (methodElement != null)
        {
            var paramElements = methodElement.Elements("param").ToList();

            if (paramElements.Any())
            {
                var paraElement =
                    paramElements.FirstOrDefault(x => x!.Attribute("name")!.Value == parameterInfo.Name, null);

                if (paraElement != null && !paraElement.Value.IsNullOrEmpty())
                    builder.Append($": {paraElement.Value}");
            }
        }

        return builder.ToString();
    }

    #endregion

    #region Constructors readme generator

    private string GetConstructorDocument(ConstructorInfo constructorInfo)
    {
        var builder = new StringBuilder();

        var constructorElement = constructorInfo.GetElement();
        builder.AppendLine($"- {GetConstructorContent(constructorElement)}");
        
        var parameters = constructorInfo.GetParameters().ToList();
        if (parameters.Count != 0)
            foreach (var parameterInfo in parameters)
                builder.AppendLine($"\t- {GetParameterDocument(parameterInfo, constructorElement)}");

        return builder.ToString();
    }

    private string GetConstructorContent(XElement? constructorElement)
    {
        var builder = new StringBuilder();
        builder.Append($"Constructor");
        
        var summaryElement = constructorElement?.Element("summary");
        if (summaryElement != null)
        {
            var pureSummaryValue = GetPureSummaryValue(summaryElement);
            if (!pureSummaryValue.IsNullOrEmpty())
                builder.Append($": {GetPureSummaryValue(summaryElement)}");
        }

        return builder.ToString();
    }

    #endregion

    #region General member readme generator

    private string GetPropertyDocument(PropertyInfo propertyInfo)
    {
        return GetRegularMemberDocument($"{propertyInfo.Name}({propertyInfo.PropertyType.Name})", propertyInfo);
    }

    private string GetFieldDocument(FieldInfo fieldInfo)
    {
        return GetRegularMemberDocument($"{fieldInfo.Name}({fieldInfo.FieldType.Name})", fieldInfo);
    }
    
    private string GetEventDocument(EventInfo eventInfo)
    {
        return GetRegularMemberDocument($"{eventInfo.Name}({eventInfo.EventHandlerType?.Name ?? "object"})", eventInfo);
    }

    private string GetRegularMemberDocument(string header, MemberInfo memberInfo)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"- ```{header}```");
        
        var memberElement = memberInfo.GetElement();
        if (memberElement != null)
        {
            var summaryElement = memberElement.Element("summary");
            if (summaryElement != null)
            {
                var pureSummaryValue = GetPureSummaryValue(summaryElement);
                if (!pureSummaryValue.IsNullOrEmpty())
                    builder.AppendLine($"\t- {GetPureSummaryValue(summaryElement)}");
            }
        }

        return builder.ToString();
    }

    #endregion

    #endregion
    
    

    #endregion

    #region Helpers

    /// <summary>
    /// Get summary value without value from children element
    /// </summary>
    /// <param name="summaryElement"></param>
    /// <returns></returns>
    private string GetPureSummaryValue(XElement summaryElement)
    {
        var summaryValue = summaryElement.Value;

        if (summaryValue.IsNullOrEmpty())
            goto re;

        if (summaryElement.HasElements)
        {
            var subSummaryElementsValue = summaryElement.Elements()
                .Select(x => x.Value).JoinToString("");
            summaryValue = summaryValue.Empty(subSummaryElementsValue);
        }

        re:
        return summaryValue.Trim();
    }

    private bool IsNotGetterOrSetter(MethodInfo methodInfo)
    {
        var type = methodInfo.DeclaringType!;
        //1: get 2: set
        var properties = type.GetProperties()
            .Select(p => (p.GetGetMethod(), p.GetSetMethod()));

        return !properties.Any(x => x.Item1 == methodInfo || x.Item2 == methodInfo);
    }

    private bool IsNotAdderOrRemover(MethodInfo methodInfo)
    {
        var type = methodInfo.DeclaringType!;
        //1: add 2: remove
        var events = type.GetEvents()
            .Select(p => (p.GetAddMethod(), p.GetRemoveMethod()));
        
        return !events.Any(x => x.Item1 == methodInfo || x.Item2 == methodInfo);
    }

    #endregion
    

    // private List<string> MarkdownLines { get; set; } = new();
    //
    // public string GetTypeContent(IEnumerable<Type> types, string? url = null)
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
    //     re.Add(GetTypeSummaryContent(element, true));
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
    //     var summaryContent = GetTypeSummaryContent(element);
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
    // private string GetTypeSummaryContent(XElement element, bool withChildren = false)
    // {
    //     var summary = element.ElementBase("summary");
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