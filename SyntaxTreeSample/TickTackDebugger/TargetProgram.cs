using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TickTackDebugger
{
    public class TargetProgram
    {
        const string SourcePath = @"..\..\..\NumericConsole\Program.cs";
        const string GeneratedPath = @"Program.g.cs";

        MethodInfo entryPoint;

        public void Initialize()
        {
            // Generates the code for debugging.
            var sourceCode = File.ReadAllText(SourcePath);
            var generatedCode = SyntaxHelper.InsertBreakpoints(sourceCode);
            File.WriteAllText(GeneratedPath, generatedCode, Encoding.UTF8);

            // Compiles and loads the assembly.
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var compilerOption = new CompilerParameters(new[] { "DebuggerLib.dll" }) { GenerateExecutable = true };
            var compilerResult = provider.CompileAssemblyFromFile(compilerOption, GeneratedPath);
            if (compilerResult.Errors.HasErrors) return;

            entryPoint = compilerResult.CompiledAssembly.EntryPoint;
        }

        public void StartDebugging()
        {
            entryPoint.Invoke(null, new object[] { new string[0] });
        }
    }
}
