using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AHpx.RG.Core.Core;
using AHpx.RG.Core.Utils;
using AHpx.RG.TestLib;
using Manganese.Array;
using Manganese.Text;
using Console = System.Console;

namespace AHpx.RG.Core
{
    public static class Runtime
    {
        public static void Main()
        {

            foreach (var memberInfo in typeof(TestLib1).GetMembers())
            {
                memberInfo.GetMemberSignature().CW();
            }

            return;
            
            Global.Config = new GlobalConfig
            {
                CompiledLibraryPath =
                    @"C:\Users\ahpx\source\repos\AHpx.RG\AHpx.RG.TestLib\bin\Debug\netstandard2.0\AHpx.RG.TestLib.dll",
                XmlDocumentationPath =
                    @"C:\Users\ahpx\source\repos\AHpx.RG\AHpx.RG.TestLib\bin\Debug\AHpx.RG.TestLib.xml"
            };
            
            var types = typeof(TestLib2).Assembly.GetTypes().Where(x => x.Name.StartsWith("Test"));
            
            foreach (var type in types)
            {
                foreach (var method in type.GetMethods().Where(x => x.IsPublic))
                {
                    var signature = method.GetSignature();
                    var element = Global.XmlMembers
                        .Select(x => x.Attribute("name")!.Value)
                        .FirstOrDefault(x => x == signature, null);

                    if (!element!.IsNullOrEmpty())
                    {
                        Console.WriteLine(element);
                    }
                }
            }
        }

        public static T CW<T>(this T o)
        {
            Console.WriteLine(o);

            return o;
        }
    }
}
