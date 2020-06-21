using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Questore.Middleware
{
    public class AllowAuthenticated
    {
        private readonly RequestDelegate _next;

        public AllowAuthenticated(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var user = context.Session.GetString("user");

            if (user != null || context.Request.Path.Value == "/" || context.Request.Path.Value == "/Error" || context.Request.Path.Value == "/Login")
            {
                await _next(context);
            }
            else
            {
                context.Response.Redirect("/");

            }
        }
    }
}
