using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.ExceptionDomain;

namespace WTR.Infrastructure
{
    public static class ExceptionMiddlewareExtention
    {
        public static IApplicationBuilder WeatherBusinessExceptionHandler(this IApplicationBuilder app)
        {

            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    context.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Plain;

                    var exceptionHandlerPathFeature =
                        context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error is WeatherCustomException)
                    {
                        var exception = exceptionHandlerPathFeature?.Error as WeatherCustomException;
                        await context.Response.WriteAsJsonAsync(new { Status = "Error", ErrorMessage = exception.Message });
                    }
                    else
                    {
                        await context.Response.WriteAsJsonAsync(new { Status = "Error", ErrorMessage = "An exception was thrown." });
                    }

                });
            });

            return app;
        }
    }
}
