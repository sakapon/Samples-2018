using System;
using Newtonsoft.Json;

namespace NetStandardLib
{
    public static class RandomHelper2
    {
        public static string GenerateBase64s() =>
            JsonConvert.SerializeObject(new[] { RandomHelper.GenerateBase64(16), RandomHelper.GenerateBase64(32) });
    }
}
