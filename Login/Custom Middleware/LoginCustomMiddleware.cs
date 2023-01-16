using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

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
            if (httpContext.Request.Path == "/" && httpContext.Request.Method == "POST")
            {
                StreamReader reader = new StreamReader(httpContext.Request.Body);
                string body = await reader.ReadToEndAsync();
                Dictionary<string, StringValues> queryDict = QueryHelpers.ParseQuery(body);

                string? email = null, password = null;

                if (queryDict.ContainsKey("email"))
                {
                    email = Convert.ToString(queryDict["email"][0]);
                }
                else
                {
                    httpContext.Response.StatusCode = 400;
                    httpContext.Response.WriteAsync("Invalid input for 'email'\n");
                }

                if (queryDict.ContainsKey("password"))
                {
                    password = Convert.ToString(queryDict["password"][0]);
                }
                else
                {
                    if (httpContext.Response.StatusCode == 200)
                    {
                        httpContext.Response.StatusCode = 400;
                    }
                    httpContext.Response.WriteAsync("Invalid input for 'password'\n");
                }

                if (string.IsNullOrEmpty(email) == false && string.IsNullOrEmpty(password) == false)
                {
                    string validEmail = "admin@test.com", validPw = "123";
                    bool isValidLogin;

                    if (email == validEmail && password == validPw)
                    {
                        await httpContext.Response.WriteAsync("Successful login!");
                    }
                    else
                    {
                        if (httpContext.Response.StatusCode == 200)
                        {
                            httpContext.Response.StatusCode = 400;
                        }
                        httpContext.Response.WriteAsync("Invalid login");
                    }
                }
                else
                {
                    if (httpContext.Response.StatusCode == 200)
                    {
                        httpContext.Response.StatusCode = 400;
                    }
                    httpContext.Response.WriteAsync("Invalid login");
                }
            }
            else
            {
                await _next(httpContext);
            }
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

