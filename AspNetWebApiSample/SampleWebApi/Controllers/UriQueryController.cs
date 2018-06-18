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
    public class UriQueryController : ApiController
    {
        // GET api/uriquery/hello
        // GET api/uriquery?id=hello
        public string Get(string id)
        {
            Debug.WriteLine(id);
            return id;
        }

        // GET api/uriquery/wildcard/hello
        [HttpGet]
        [Route("api/UriQuery/Wildcard/{*id}")]
        public string Wildcard(string id) => Get(id);

        // POST api/uriquery
        public string Post(Person person)
        {
            Debug.WriteLine(person.Name);
            return person.Name;
        }
    }
}
