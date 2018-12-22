using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DebuggerLib;
using Reactive.Bindings;

namespace TickTackDebugger
{
    public class AppModel
    {
        public TargetProgram TargetProgram { get; }

        public ReactiveProperty<(int start, int length)> CodeSpan { get; } = new ReactiveProperty<(int, int)>();
        public ReactiveCollection<Variable> Variables { get; } = new ReactiveCollection<Variable>();

        public ReactiveProperty<double> ExecutionInterval { get; } = new ReactiveProperty<double>(0.5);
        public ReactiveProperty<bool> IsReady { get; } = new ReactiveProperty<bool>(true);

        public AppModel()
        {
            TargetProgram = new TargetProgram();

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
            IsReady.Value = false;
            Task.Run(() => TargetProgram.StartDebugging())
                .ContinueWith(_ => IsReady.Value = true);
        }

        void UpdateVariables(IDictionary<string, object> variables)
        {
            foreach (var (p, i) in variables.Select((p, i) => (p, i)))
            {
                if (i < Variables.Count)
                {
                    var v = Variables[i];
                    v.Name.Value = p.Key;
                    v.Value.Value = p.Value;
                }
                else
                {
                    Variables.InsertOnScheduler(i, new Variable(p.Key, p.Value));
                }
            }

            for (var i = Variables.Count - 1; i >= variables.Count; i--)
                Variables.RemoveAtOnScheduler(i);
        }
    }

    public class Variable
    {
        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<object> Value { get; }

        public Variable(string name, object value)
        {
            Name = new ReactiveProperty<string>(name);
            Value = new ReactiveProperty<object>(value);
        }
    }
}
