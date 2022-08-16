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
    public class ResponseLoggingMiddleware : IMiddleware
    {
        private readonly IHttpContextAccessor context;

        public ResponseLoggingMiddleware(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public async Task InvokeAsync(HttpContext http, RequestDelegate next)
        {
            if (context.HttpContext is not null)
            {
                HttpResponse response = context.HttpContext.Response;

                string? body = null;

                if (response.Body.CanRead)
                {
                    body = await GetRawJson(response, Encoding.UTF8);
                }

                Dictionary<string, string> pairs = new()
                {
                    { "ResponseBody", body },
                    { "Tenant", GetTenant(response) },
                    { "Headers", Serialize(GetHeaders(response)) }
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

        private static string GetTenant(HttpResponse response)
        {
            bool hasTenant = response.Headers.TryGetValue("X-Request-Instance", out StringValues values);

            if (!hasTenant)
                return null;

            return values.First();
        }

        private static Dictionary<string, string> GetHeaders(HttpResponse response)
        {
            Dictionary<string, string> responseHeaders = new Dictionary<string, string>();

            foreach (KeyValuePair<string, StringValues> header in response.Headers)
            {
                responseHeaders.Add(header.Key, header.Value);
            }

            return responseHeaders;
        }

        private static async Task<string> GetRawJson(HttpResponse request, Encoding encoding = null)
        {
            //request.EnableBuffering();

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

