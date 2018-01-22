using System;
using System.Net;
using System.Runtime.Serialization;

namespace Sprotify.Web.Services.Core
{
    public class ResourceNotFoundException : HttpException
    {
        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string message) : base(HttpStatusCode.NotFound, message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}