using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using EnergyTrading.WebApi.Common.Client;

namespace EnergyTrading.WebApi.Common.Testing.Client
{
    public static class Extensions
    {
        public static T UseMockMessageHandler<T>(this T gateway, HttpMessageHandler httpMessageHandler) where T : RestGatewayBase
        {
            if (gateway != null && httpMessageHandler != null)
            {
                gateway.UseMockHttpMessageHandler(httpMessageHandler);
            }
            return gateway;
        }

        public static HttpRequestMessage WithDefaultConfiguration(this HttpRequestMessage request)
        {
            if (request == null)
            {
                return request;
            }
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();
            return request;
        }
    }
}