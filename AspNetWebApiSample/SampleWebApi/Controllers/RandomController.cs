using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SampleWebApi.Controllers
{
    /// <summary>
    /// Generates random data.
    /// </summary>
    [RoutePrefix("api")]
    [Route("{action}")]
    public class RandomController : ApiController
    {
        static readonly Random random = new Random();

        /// <summary>
        /// Creates a new integer.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>A new integer.</returns>
        [HttpGet]
        [ActionName("NewInteger")]
        public int NewInteger1(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Creates a new integer.
        /// </summary>
        /// <param name="range">The range info.</param>
        /// <returns>A new integer.</returns>
        [HttpPost]
        [ActionName("NewInteger")]
        public int NewInteger2([FromBody]RangeInfo range)
        {
            return random.Next(range.MinValue, range.MaxValue);
        }

        /// <summary>
        /// Creates a new double values with the specified count.
        /// </summary>
        /// <param name="count">The count of the result.</param>
        /// <returns>A new double values.</returns>
        [HttpGet]
        [Route("NewDoubles/{count:int:range(0,64)}")]
        public double[] NewDoubles(int count)
        {
            return Enumerable.Range(0, count)
                .Select(i => random.NextDouble())
                .ToArray();
        }

        /// <summary>
        /// Creates a new UUID (GUID).
        /// </summary>
        /// <returns>A new UUID (GUID).</returns>
        [HttpGet]
        public Guid NewUuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new UUID info.
        /// </summary>
        /// <returns>A new UUID info.</returns>
        [HttpGet]
        public UuidInfo NewUuidInfo()
        {
            return new UuidInfo { Id = Guid.NewGuid(), Date = DateTime.Now };
        }
    }

    /// <summary>The range info.</summary>
    public struct RangeInfo
    {
        /// <summary>The minimum value.</summary>
        public int MinValue { get; set; }

        /// <summary>The maximum value.</summary>
        public int MaxValue { get; set; }
    }

    /// <summary>The UUID info.</summary>
    public struct UuidInfo
    {
        /// <summary>The ID.</summary>
        public Guid Id { get; set; }

        /// <summary>The date.</summary>
        public DateTime Date { get; set; }
    }
}
