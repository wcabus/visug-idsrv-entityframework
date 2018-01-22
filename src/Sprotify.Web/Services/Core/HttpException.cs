using System;
using System.Net;
using System.Runtime.Serialization;

namespace Sprotify.Web.Services.Core
{
    public class HttpException : Exception
    {
        public HttpException()
        {
        }

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;
    }
}