using System;
using System.Collections.Generic;
using System.Linq;

namespace DebuggerLib
{
    public static class DebugHelper
    {
        public static event Action<int, int, IDictionary<string, object>> InfoNotified;

        public static void NotifyInfo(int spanStart, int spanLength, IDictionary<string, object> variables)
        {
            InfoNotified?.Invoke(spanStart, spanLength, variables);
        }
    }
}
