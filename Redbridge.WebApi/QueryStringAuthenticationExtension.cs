using Owin;
using Redbridge.Web;
using Redbridge.Web.Messaging;
using System;

namespace Redbridge.WebApi
{
    public static class QueryStringAuthenticationExtension
    {
        public static void UseQueryStringAuthentication(this IAppBuilder app)
        {
            app.Use(async (owinContext, next) =>
            {
                if (owinContext.Request.QueryString.HasValue && string.IsNullOrWhiteSpace(owinContext.Request.Headers.Get(HeaderNames.Authorization)))
                {
                    var queryString = HttpUtility.ParseQueryString(owinContext.Request.QueryString.Value);
                    if (queryString.ContainsKey(QueryStringParts.Authentication))
                    {
                        var token = queryString[QueryStringParts.Authentication];
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            owinContext.Request.Headers.Add(HeaderNames.Authorization, new[] { BearerTokenFormatter.CreateToken(token) });
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