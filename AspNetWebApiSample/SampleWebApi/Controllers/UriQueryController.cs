using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SampleWebApi.Models;

namespace SampleWebApi.Controllers
{
    /// <summary>
    /// For the test of URL-encoded query and form.
    /// </summary>
    public class UriQueryController : ApiController
    {
        /// <summary>
        /// Echoes the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The ID.</returns>
        /// <remarks>
        /// GET api/uriquery/hello
        /// GET api/uriquery?id=hello
        /// </remarks>
        public string Get(string id)
        {
            Debug.WriteLine(id);
            return id;
        }

        /// <summary>
        /// Echoes the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The ID.</returns>
        /// <remarks>
        /// GET api/uriquery/wildcard/hello
        /// GET api/uriquery/wildcard/?id=hello
        /// </remarks>
        [HttpGet]
        [Route("api/UriQuery/Wildcard/{*id}")]
        public string Wildcard(string id) => Get(id);

        /// <summary>
        /// Echoes the specified name.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns>The name.</returns>
        /// <remarks>
        /// POST api/uriquery
        /// </remarks>
        public string Post(Person person)
        {
            Debug.WriteLine(person.Name);
            return person.Name;
        }
    }
}
