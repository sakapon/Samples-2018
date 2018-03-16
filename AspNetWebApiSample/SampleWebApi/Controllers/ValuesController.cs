using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SampleWebApi.Controllers
{
    public class ValuesController : ApiController
    {
        /// <summary>
        /// Gets all values.
        /// </summary>
        /// <returns>All values.</returns>
        /// <remarks>GET api/values</remarks>
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
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Posts the new value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>POST api/values</remarks>
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        ///  Updates the value for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="value">The value.</param>
        /// <remarks>PUT api/values/5</remarks>
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        ///  Deletes the value for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <remarks>DELETE api/values/5</remarks>
        public void Delete(int id)
        {
        }
    }
}
