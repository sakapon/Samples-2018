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
            var compilerOption = new CompilerParameters(new[] { "System.Core.dll", "DebuggerLib.dll" }) { GenerateExecutable = true };
            var compilerResult = provider.CompileAssemblyFromFile(compilerOption, GeneratedPath);
            if (compilerResult.Errors.HasErrors) throw new FormatException(ToMessage(compilerResult.Errors[0]));

            // Calls the Main method.
            var entryPoint = compilerResult.CompiledAssembly.EntryPoint;
            var parameters = entryPoint.GetParameters();
            if (parameters.Length == 0)
                entryPoint.Invoke(null, null);
            else
                entryPoint.Invoke(null, new object[] { new string[0] });
        }

        static string ToMessage(CompilerError error) => $"{error.ErrorNumber}: {error.ErrorText}";
    }
}
