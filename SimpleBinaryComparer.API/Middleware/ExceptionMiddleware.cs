using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SimpleBinaryComparer.Domain.Service.Model;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            string message = string.Empty;

            message = exception.Message;

            var result = JsonConvert.SerializeObject(ResponseBase.CreateError(message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
