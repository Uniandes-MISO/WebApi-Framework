using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Redbridge.Exceptions;

namespace Redbridge.WebApiCore.Filters
{
    public class UserNotAuthorizedExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UserNotAuthorizedExceptionFilter(ILogger<UserNotAuthorizedExceptionFilter> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogInformation("Checking exception for user not authorized exception filtering....");

            if (context.Exception is UserNotAuthorizedException exception)
            {
                context.ExceptionHandled = true;
                context.Result = new JsonResult(null) { StatusCode = 403 };
            }
        }
    }
}