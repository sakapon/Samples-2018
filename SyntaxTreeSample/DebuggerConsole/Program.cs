using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DebuggerLib;

namespace DebuggerConsole
{
    class Program
    {
        const string SourcePath = @"..\..\..\NumericConsole\Program.cs";
        const string GeneratedPath = @"Program.g.cs";

        static void Main(string[] args)
        {
            var sourceCode = File.ReadAllText(SourcePath);
            var generatedCode = SyntaxHelper.InsertBreakpoints(sourceCode);
            File.WriteAllText(GeneratedPath, generatedCode, Encoding.UTF8);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var compilerOption = new CompilerParameters(new[] { "DebuggerLib.dll" }) { GenerateExecutable = true };
            var compilerResult = provider.CompileAssemblyFromFile(compilerOption, GeneratedPath);
            if (compilerResult.Errors.HasErrors) return;

            DebugHelper.InfoNotified += (spanStart, spanLength, variables) =>
            {
                Console.WriteLine(string.Join(", ", variables.Select(p => $"{p.Key}: {p.Value}")));
                Console.WriteLine(sourceCode.Substring(spanStart, spanLength));
                Thread.Sleep(1000);
            };

            var entryPoint = compilerResult.CompiledAssembly.EntryPoint;
            entryPoint.Invoke(null, new object[] { new string[0] });
        }
    }
}
