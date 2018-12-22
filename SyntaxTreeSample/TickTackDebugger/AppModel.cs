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

        public ReactiveProperty<double> ExecutionInterval { get; } = new ReactiveProperty<double>(0.5);
        public ReactiveProperty<(int start, int length)> CodeSpan { get; } = new ReactiveProperty<(int, int)>();
        public ReactiveProperty<IDictionary<string, object>> Variables { get; } = new ReactiveProperty<IDictionary<string, object>>(new Dictionary<string, object>());

        public AppModel()
        {
            TargetProgram = new TargetProgram();

            // Registers the action for breakpoints.
            DebugHelper.InfoNotified += (spanStart, spanLength, variables) =>
            {
                CodeSpan.Value = (spanStart, spanLength);
                Variables.Value = variables;
                Thread.Sleep(TimeSpan.FromSeconds(ExecutionInterval.Value));
            };
        }

        public void StartDebugging()
        {
            Task.Run(() => TargetProgram.StartDebugging());
        }
    }
}
