using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DebuggerLib;
using Reactive.Bindings;
using TickTackDebugger.Properties;

namespace TickTackDebugger
{
    public class AppModel
    {
        const string DefaultSourcePath = @"..\..\..\NumericConsole\Program.cs";

        public string SourceCode { get; set; }
        public ReactiveProperty<(int start, int length)> CodeSpan { get; } = new ReactiveProperty<(int, int)>();
        public ReactiveCollection<Variable> Variables { get; } = new ReactiveCollection<Variable>();
        public ReactiveProperty<string> ErrorMessage { get; } = new ReactiveProperty<string>("");

        public ReactiveProperty<double> ExecutionInterval { get; } = new ReactiveProperty<double>(0.5);
        public ReactiveProperty<bool> IsReady { get; } = new ReactiveProperty<bool>(true);

        public AppModel()
        {
            SourceCode = File.Exists(DefaultSourcePath) ? File.ReadAllText(DefaultSourcePath) : Resources.Program;

            // Registers the action for breakpoints.
            DebugHelper.InfoNotified += (spanStart, spanLength, variables) =>
            {
                CodeSpan.Value = (spanStart, spanLength);
                UpdateVariables(variables);
                Thread.Sleep(TimeSpan.FromSeconds(ExecutionInterval.Value));
            };
        }

        public void StartDebugging()
        {
            Variables.Clear();
            ErrorMessage.Value = "";
            IsReady.Value = false;

            Task.Run(() =>
            {
                try
                {
                    TargetProgram.StartDebugging(SourceCode);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Value = ex.Message;
                }
            })
            .ContinueWith(_ => IsReady.Value = true);
        }

        void UpdateVariables(Var[] variables)
        {
            var commonLength = Math.Min(Variables.Count, variables.Length);
            for (var i = 0; i < commonLength; i++)
                Variables[i].SetValues(variables[i]);

            if (Variables.Count < variables.Length)
                Variables.AddRangeOnScheduler(variables.Skip(Variables.Count).Select(v => new Variable(v)));

            for (var i = Variables.Count - 1; i >= variables.Length; i--)
                Variables.RemoveAtOnScheduler(i);
        }
    }

    public class Variable
    {
        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<object> Value { get; }
        public ReactiveProperty<Type> Type { get; }

        public Variable(string name, object value, Type type)
        {
            Name = new ReactiveProperty<string>(name);
            Value = new ReactiveProperty<object>(value);
            Type = new ReactiveProperty<Type>(type);
        }

        public Variable(Var v) : this(v.Name, v.Value, v.Value?.GetType()) { }

        public void SetValues(Var v)
        {
            Name.Value = v.Name;
            Value.Value = v.Value;
            Type.Value = v.Value?.GetType();
        }
    }
}
