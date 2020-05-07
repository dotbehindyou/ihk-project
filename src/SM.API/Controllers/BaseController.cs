using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SM.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public HttpResponseMessage FileResult(String fileName, String contentType, Stream fileStream)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(fileStream);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return response;
        }

        public Byte[] GetAuthToken(String auth_token_64)
        {
            return Convert.FromBase64String(auth_token_64);
        }
    }
}
