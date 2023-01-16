using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Login.CustomMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var query = httpContext.Request.Query;

            if (query.ContainsKey("email")
                && query.ContainsKey("password"))
            {
                string email = query["email"];
                string password = query["password"];

                if (email == "admin@test.com" && password == "123")
                {
                    await httpContext.Response.WriteAsync("Successful login!"); 
                }
                else
                {
                    httpContext.Response.StatusCode = 400; 
                    await httpContext.Response.WriteAsync("Invalid email or password"); 
                }
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("No credentials entered!");
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoginCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginCustomMiddleware>();
        }
    }
}

