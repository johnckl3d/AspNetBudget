using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;


namespace MeetupAPI.Middlewares
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly IHttpContextAccessor context;

        public RequestLoggingMiddleware(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public async Task InvokeAsync(HttpContext http, RequestDelegate next)
        {
            if (context.HttpContext is not null)
            {
                HttpRequest request = context.HttpContext.Request;

                string? body = null;

                if ((request.Method == HttpMethods.Post ||
                     request.Method == HttpMethods.Put) &&
                     request.Body.CanRead)
                {
                    body = await GetRawJson(request, Encoding.UTF8);
                }

                Dictionary<string, string> pairs = new()
                {
                    { "RequestBody", body },
                    { "Method", request.Method },
                    { "Tenant", GetTenant(request) },
                    { "Route", Serialize(request.RouteValues) },
                    { "Query", Serialize(request.Query) },
                    { "Headers", Serialize(GetHeaders(request)) }
                };

                Enrich(pairs);
            }

            await next.Invoke(http);
        }

        private void Enrich(Dictionary<string, string> pairs)
        {
            foreach (KeyValuePair<string, string> item in pairs)
            {
                context.HttpContext.Features.Get<RequestTelemetry>().Properties[item.Key] = item.Value;
            }
        }

        private static string GetTenant(HttpRequest request)
        {
            bool hasTenant = request.Headers.TryGetValue("X-Request-Instance", out StringValues values);

            if (!hasTenant)
                return null;

            return values.First();
        }

        private static Dictionary<string, string> GetHeaders(HttpRequest request)
        {
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();

            foreach (KeyValuePair<string, StringValues> header in request.Headers)
            {
                requestHeaders.Add(header.Key, header.Value);
            }

            return requestHeaders;
        }

        private static async Task<string> GetRawJson(HttpRequest request, Encoding encoding = null)
        {
            request.EnableBuffering();

            string response = await new StreamReader(request.Body).ReadToEndAsync();

            request.Body.Position = 0;

            return response;
        }

        private static string Serialize(object item)
        {
            return JsonConvert.SerializeObject(item, Formatting.Indented);
        }
    }
}

