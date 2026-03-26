namespace mini_project_aspnet.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Allow access to Account/Login and static files without authentication
            if (path.Contains("/account/login") || 
                path.Contains("/lib/") || 
                path.Contains("/css/") || 
                path.Contains("/js/") || 
                path == "/")
            {
                await _next(context);
                return;
            }

            // Check if user is authenticated
            var userId = context.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                // Redirect to login if not authenticated
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }
    }
}
