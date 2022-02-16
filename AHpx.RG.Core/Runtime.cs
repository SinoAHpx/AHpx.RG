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
            Global.Config = new GlobalConfig
            {
                CompiledLibraryPath =
                    @"C:\Users\ahpx\source\repos\AHpx.RG\AHpx.RG.TestLib\bin\Debug\netstandard2.0\AHpx.RG.TestLib.dll",
                XmlDocumentationPath =
                    @"C:\Users\ahpx\source\repos\AHpx.RG\AHpx.RG.TestLib\bin\Debug\AHpx.RG.TestLib.xml"
            };
            
            var core = new ReadmeGeneratorCore();
            
            var types = ReflectionUtils.GetTypes();
            
            Console.WriteLine(core.GetContent(types));
        }

        public static T CW<T>(this T o)
        {
            Console.WriteLine(o);

            return o;
        }
    }
}
