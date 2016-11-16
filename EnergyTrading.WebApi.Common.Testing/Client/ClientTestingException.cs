using System;
using System.Runtime.Serialization;

namespace EnergyTrading.WebApi.Common.Testing.Client
{
    public class ClientTestingException : Exception
    {
        public ClientTestingException()
        {
        }

        public ClientTestingException(string message) : base(message)
        {
        }

        public ClientTestingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientTestingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}