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
        static readonly List<string> values = new List<string> { "value0", "value1" };

        /// <summary>
        /// Gets all values.
        /// </summary>
        /// <returns>All values.</returns>
        /// <remarks>GET api/values</remarks>
        public IEnumerable<string> Get()
        {
            return values;
        }

        /// <summary>
        /// Gets the value for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The value.</returns>
        /// <remarks>GET api/values/5</remarks>
        public string Get(int id)
        {
            if (id < 0 || values.Count <= id)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No value with ID = {id}");
                throw new HttpResponseException(response);

                // No content:
                //throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return values[id];
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
