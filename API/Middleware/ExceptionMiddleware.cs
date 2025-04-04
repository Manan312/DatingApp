using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try{
                await next(context);
            }
            catch(Exception ex)
            {
                logger.LogError(ex,ex.Message);
                context.Response.ContentType="application/json";
                context.Response.StatusCode=(int) HttpStatusCode.InternalServerError;
                
                var response=env.IsDevelopment()
                ?new APIException(context.Response.StatusCode,ex.Message,ex.StackTrace)
                :new APIException(context.Response.StatusCode,ex.Message,"Internal Server Error");

                var options=new JsonSerializerOptions{
                    PropertyNamingPolicy=JsonNamingPolicy.CamelCase
                };

                var json=JsonSerializer.Serialize(response,options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}