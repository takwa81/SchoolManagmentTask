using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolManagement.WebAPI.Middlewares
{
    public class CustomAuthResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = false,
                    message = "Unauthorized: Please login first",
                    code = 401,
                    data = (string?)null
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = false,
                    message = "Forbidden: You do not have permission",
                    code = 403,
                    data = (string?)null
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
