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
            // Generates the code for debugging.
            var sourceCode = File.ReadAllText(SourcePath);
            var generatedCode = SyntaxHelper.InsertBreakpoints(sourceCode);
            File.WriteAllText(GeneratedPath, generatedCode, Encoding.UTF8);

            // Compiles and loads the assembly.
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var compilerOption = new CompilerParameters(new[] { "System.Core.dll", "DebuggerLib.dll" }) { GenerateExecutable = true };
            var compilerResult = provider.CompileAssemblyFromFile(compilerOption, GeneratedPath);
            if (compilerResult.Errors.HasErrors) return;

            // Registers the action for breakpoints.
            DebugHelper.InfoNotified += (spanStart, spanLength, variables) =>
            {
                Console.WriteLine(string.Join(", ", variables.Select(v => $"{v.Name}: {v.Value}")));
                Console.WriteLine(sourceCode.Substring(spanStart, spanLength));
                Thread.Sleep(1000);
            };

            // Calls the Main method.
            var entryPoint = compilerResult.CompiledAssembly.EntryPoint;
            entryPoint.Invoke(null, new object[] { new string[0] });
        }
    }
}
