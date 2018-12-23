using System;
using System.Collections.Generic;
using System.Linq;

namespace DebuggerLib
{
    public static class DebugHelper
    {
        public static event Action<int, int, Var[]> InfoNotified;

        public static void NotifyInfo(int spanStart, int spanLength, params Var[] variables)
        {
            InfoNotified?.Invoke(spanStart, spanLength, variables);
        }
    }

    public struct Var
    {
        public string Name;
        public object Value;

        public Var(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
