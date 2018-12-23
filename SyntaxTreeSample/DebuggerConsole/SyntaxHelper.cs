using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using DebugStatement = System.ValueTuple<Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax, string[]>;

namespace DebuggerConsole
{
    public static class SyntaxHelper
    {
        public static string InsertBreakpoints(string sourceCode)
        {
            var root = ParseText(sourceCode);

            var method = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .First(m => m.Identifier.ValueText == "Main");
            var statements = DetectStatements(method);

            var result = sourceCode;
            foreach (var (statement, variables) in statements.Reverse())
            {
                var (span, debugIndex) = GetSpan(statement);
                result = result.Insert(debugIndex, $"DebugHelper.NotifyInfo({span.Start}, {span.Length}{ToParamsArrayText(variables)});\r\n");
            }

            return result.Insert(root.Usings.FullSpan.End, "using DebuggerLib;\r\n");
        }

        public static CompilationUnitSyntax ParseText(string text)
        {
            var tree = CSharpSyntaxTree.ParseText(text);
            var diagnostics = tree.GetDiagnostics().ToArray();
            if (diagnostics.Length > 0) return null;

            return tree.GetCompilationUnitRoot();
        }

        public static DebugStatement[] DetectStatements(MethodDeclarationSyntax method)
        {
            var statements = new List<DebugStatement>();
            DetectStatements(method.Body, statements, new List<(string, SyntaxNode)>());
            return statements.ToArray();
        }

        static void DetectStatements(SyntaxNode node, List<DebugStatement> statements, List<(string name, SyntaxNode scope)> variables)
        {
            // Adds variables.
            if (node is VariableDeclarationSyntax varSyntax)
            {
                var varNames = varSyntax.Variables.Select(v => v.Identifier.ValueText).ToArray();
                var scope = ((node.Parent is LocalDeclarationStatementSyntax) ? node.Parent : node)
                    .Ancestors()
                    .First(n => n is StatementSyntax);

                variables.AddRange(varNames.Select(v => (v, scope)));
            }

            // Maps variables to the statement.
            if ((node is StatementSyntax statement) &&
                !(node is BlockSyntax) &&
                !(node is BreakStatementSyntax))
                statements.Add((statement, variables.Select(v => v.name).ToArray()));

            // Recursively.
            foreach (var child in node.ChildNodes())
                DetectStatements(child, statements, variables);

            // Maps variables to the last line of the block.
            if (node is BlockSyntax block)
                statements.Add((block, variables.Select(v => v.name).ToArray()));

            // Clears variables out of the scope.
            if (node is StatementSyntax)
                for (var i = variables.Count - 1; i >= 0; i--)
                    if (variables[i].scope == node)
                        variables.RemoveAt(i);
                    else
                        break;
        }

        static (TextSpan, int) GetSpan(StatementSyntax statement)
        {
            switch (statement)
            {
                case ForStatementSyntax f:
                    var span = new TextSpan(f.ForKeyword.Span.Start, f.CloseParenToken.Span.End - f.ForKeyword.Span.Start);
                    return (span, statement.FullSpan.Start);
                case BlockSyntax b:
                    return (b.CloseBraceToken.Span, b.CloseBraceToken.FullSpan.Start);
                default:
                    return (statement.Span, statement.FullSpan.Start);
            }
        }

        static string ToParamsArrayText(string[] variables) =>
            string.Concat(variables.Select(v => $", new Var(\"{v}\", {v})"));

        static string ToDictionaryText(string[] variables) =>
            $"new Dictionary<string, object> {{ {string.Join(", ", variables.Select(v => $"{{ \"{v}\", {v} }}"))} }}";
    }
}
