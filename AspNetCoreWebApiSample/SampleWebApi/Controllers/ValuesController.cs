using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebApi.Controllers
{
    /// <summary>
    /// Represents string data.
    /// </summary>
    [Produces("application/json")]
    //[Produces("application/json", "application/xml")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// Gets all values.
        /// </summary>
        /// <returns>All values.</returns>
        /// <remarks>GET api/values</remarks>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Gets the value for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The value.</returns>
        /// <remarks>GET api/values/5</remarks>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Posts the new value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>POST api/values</remarks>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        ///  Updates the value for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="value">The value.</param>
        /// <remarks>PUT api/values/5</remarks>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        ///  Deletes the value for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <remarks>DELETE api/values/5</remarks>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
