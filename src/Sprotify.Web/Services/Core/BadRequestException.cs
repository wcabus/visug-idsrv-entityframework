using System;
using System.Net;
using System.Runtime.Serialization;

namespace Sprotify.Web.Services.Core
{
    public class BadRequestException : HttpException
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}