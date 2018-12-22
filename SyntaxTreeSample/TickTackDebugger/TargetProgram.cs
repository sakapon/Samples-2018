using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TickTackDebugger
{
    public static class TargetProgram
    {
        const string GeneratedPath = @"Program.g.cs";

        public static void StartDebugging(string sourceCode)
        {
            // Generates the code for debugging.
            var generatedCode = SyntaxHelper.InsertBreakpoints(sourceCode);
            File.WriteAllText(GeneratedPath, generatedCode, Encoding.UTF8);

            // Compiles and loads the assembly.
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var compilerOption = new CompilerParameters(new[] { "DebuggerLib.dll" }) { GenerateExecutable = true };
            var compilerResult = provider.CompileAssemblyFromFile(compilerOption, GeneratedPath);
            if (compilerResult.Errors.HasErrors) return;

            // Calls the Main method.
            var entryPoint = compilerResult.CompiledAssembly.EntryPoint;
            entryPoint.Invoke(null, new object[] { new string[0] });
        }
    }
}
