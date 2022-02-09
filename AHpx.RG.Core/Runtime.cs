using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AHpx.RG.Core.Core;
using Manganese.Array;

namespace AHpx.RG.Core
{
    public static class Runtime
    {
        public static void Main()
        {
            var core = new ReadmeGeneratorCore
            {
                CompiledDllPath = @"E:\CSharp\Manganese\Manganese\bin\Debug\net6.0\Manganese.dll",
                XmlDocumentationPath = @"E:\CSharp\Manganese\Manganese\bin\Debug\Manganese.xml"
            };

            var list = new List<string>();
            foreach (var type in core.Types.Output())
            {
                list.Add(core.GetContent(type, new Uri("https://github.com/SinoAHpx/Manganese/tree/master/")));
            }

            File.WriteAllLines(@"C:\Users\ahpx\Desktop\test11.md", list);
        }

        public static object CW(this object o)
        {
            Console.WriteLine(o);

            return o;
        }
    }
}
