using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SampleWebApi.Controllers
{
    /// <summary>
    /// Generates random text.
    /// </summary>
    [RoutePrefix("api")]
    public class RandomTextController : ApiController
    {
        static readonly Random random = new Random();

        /// <summary>
        /// Creates new byte values with the specified count.
        /// </summary>
        /// <param name="count">The count of the result.</param>
        /// <returns>New byte values. The content type is text/plain.</returns>
        [HttpGet]
        [Route("NewBytes1/{count:int:range(0,64)}")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage NewBytes1(int count)
        {
            var content = CreateBytesString(count);

            var response = Request.CreateResponse();
            response.Content = new StringContent(content);
            return response;
        }

        /// <summary>
        /// Creates new byte values with the specified count.
        /// </summary>
        /// <param name="count">The count of the result.</param>
        /// <returns>New byte values. The content type is text/plain.</returns>
        [HttpGet]
        [Route("NewBytes2/{count:int:range(0,64)}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult NewBytes2(int count)
        {
            var content = CreateBytesString(count);
            return new TextResult(content, Request);
        }

        static string CreateBytesString(int count)
        {
            var bytes = new byte[count];
            random.NextBytes(bytes);
            return string.Join("\n", bytes);
        }
    }

    public class TextResult : IHttpActionResult
    {
        public string Content { get; }
        public HttpRequestMessage Request { get; }

        public TextResult(string content, HttpRequestMessage request)
        {
            Content = content;
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = Request.CreateResponse();
            response.Content = new StringContent(Content);
            return Task.FromResult(response);
        }
    }
}
