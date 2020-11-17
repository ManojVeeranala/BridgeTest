using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TeacherTablet.API
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ILogger<ExceptionHandler> logger)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured");

                var result = JsonConvert.SerializeObject(new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "Internal server error"
                });

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(result);
            }
        }
    }
}
