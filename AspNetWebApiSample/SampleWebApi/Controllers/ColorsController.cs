﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;

namespace SampleWebApi.Controllers
{
    /// <summary>
    /// Generates color images.
    /// </summary>
    public class ColorsController : ApiController
    {
        /// <summary>
        /// Creates a PNG data of the specified color.
        /// </summary>
        /// <param name="name">The color name.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <returns>A PNG data. The content type is image/png.</returns>
        [HttpGet]
        [Route("api/Colors/{name}.png")]
        [ResponseType(typeof(byte[]))]
        public HttpResponseMessage GetImage(string name, int w = 300, int h = 200)
        {
            var color = Regex.IsMatch(name, "^[0-9A-Fa-f]{6}$") ? ToColor(name) : Color.FromName(name);
            if (color.A == 0) return Request.CreateResponse(HttpStatusCode.NotFound);

            var bitmap = CreateBitmap(w, h, color);

            var response = Request.CreateResponse();
            response.Content = new ByteArrayContent(ToBytes(bitmap));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return response;
        }

        static Color ToColor(string rgb)
        {
            var opaque = 0xFF000000;
            var argb = (int)opaque | Convert.ToInt32(rgb, 16);
            return Color.FromArgb(argb);
        }

        static Bitmap CreateBitmap(int width, int height, Color color)
        {
            var bitmap = new Bitmap(width, height);

            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                    bitmap.SetPixel(i, j, color);

            return bitmap;
        }

        static byte[] ToBytes(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                return memory.ToArray();
            }
        }
    }
}
