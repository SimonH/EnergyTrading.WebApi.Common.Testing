using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EnergyTrading.WebApi.Common.Testing.Client
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public static JsonMediaTypeFormatter JsonFormatter = new JsonMediaTypeFormatter { SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All } };
        private Dictionary<string, Func<HttpRequestMessage, HttpResponseMessage>> _setupResponses = new Dictionary<string, Func<HttpRequestMessage, HttpResponseMessage>>(); 
        private string CreateKey(Uri uri, HttpMethod method)
        {
            return $"{method}||{uri}";
        } 

        public void Setup(Uri uri, HttpMethod method, HttpResponseMessage responseMessage)
        {
            Func<HttpRequestMessage, HttpResponseMessage> creator = hrm => responseMessage;
            Setup(uri, method, creator);
        }

        public bool StrictBehaviour { get; set; }
        
        public MockHttpMessageHandler Setup(Uri uri, HttpMethod method, Func<HttpRequestMessage, HttpResponseMessage> createResponse)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (createResponse == null)
            {
                throw new ArgumentNullException(nameof(createResponse));
            }
            _setupResponses.Add(CreateKey(uri, method), createResponse);
            return this;
        }

        public MockHttpMessageHandler WithStrictBehaviour()
        {
            StrictBehaviour = true;
            return this;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var key = CreateKey(request.RequestUri, request.Method);
            if (_setupResponses.ContainsKey(key))
            {
                return Task.FromResult(_setupResponses[key](request));
            }
            if (StrictBehaviour)
            {
                throw new ClientTestingException($"Unexpected request {request.Method} made to Uri {request.RequestUri}");
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
        }
    }
}