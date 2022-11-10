using Microsoft.AspNetCore.Builder;
using Redbridge.Web;
using Redbridge.Web.Messaging;

namespace Redbridge.WebApiCore
{
    public static class QueryStringAuthenticationExtension
    {
        public static void UseQueryStringAuthentication(this IApplicationBuilder app)
        {
            _ = app.Use(async (httpContext, next) =>
            {
                if (httpContext.Request.QueryString.HasValue && httpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeader) && string.IsNullOrWhiteSpace(authorizationHeader))
                {
                    var queryString = HttpUtility.ParseQueryString(httpContext.Request.QueryString.Value);
                    if (queryString.ContainsKey(QueryStringParts.Authentication))
                    {
                        var token = queryString[QueryStringParts.Authentication];

                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            httpContext.Request.Headers.Add(HeaderNames.Authorization, new[] { BearerTokenFormatter.CreateToken(token) });
                        }
                    }
                }

                try
                {
                    await next.Invoke();
                }
                catch (OperationCanceledException)
                {
                    // Do not propagate this exception.
                }
            });
        }
    }
}