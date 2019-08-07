using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExceptionHandlingDemo.ExceptionHandling
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public CustomExceptionMiddleware(RequestDelegate next)
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

            CustomErrorResponse customError = CreateError(exception);

            await response.WriteAsync(JsonConvert.SerializeObject(customError));
        }

        private CustomErrorResponse CreateError(Exception exception)
        {
            CustomErrorResponse customError = new CustomErrorResponse();

            if (exception is BaseCustomException customException)
            {
                customError.Code = customException.ErrorCode;
                customError.ObjectType = customException.ObjectType;
                customError.Message = customException.Message;
                return customError;
            }

            if (exception is ArgumentNullException)
            {
                customError.Code = "ArgumentNull";
                customError.Message = exception.Message;
                return customError;
            }

            if (exception is KeyNotFoundException)
            {
                customError.Code = "KeyNotFound";
                customError.Message = exception.Message;
                return customError;
            }

            else
            {
                customError.Code = "Unexpected";
            }

            return customError;
        }
    }
}
