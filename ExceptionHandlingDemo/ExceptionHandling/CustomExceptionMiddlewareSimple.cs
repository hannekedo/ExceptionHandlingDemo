using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExceptionHandlingDemo.ExceptionHandling
{
    public class CustomExceptionMiddlewareSimple
    {
        private readonly RequestDelegate next;

        public CustomExceptionMiddlewareSimple(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;//For now, decide on different statuscodes for different errors later

            await response.WriteAsync(JsonConvert.SerializeObject(exception.Message));
        }
    }
}
