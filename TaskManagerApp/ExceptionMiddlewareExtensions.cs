using TaskManagerApp.Exceptions;

namespace TaskManagerApp
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddeware>();
        }
    }
}
