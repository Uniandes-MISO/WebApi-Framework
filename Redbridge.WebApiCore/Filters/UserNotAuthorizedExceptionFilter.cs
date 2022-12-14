using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Redbridge.Exceptions;

namespace Redbridge.WebApiCore.Filters
{
    public class UserNotAuthorizedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UserNotAuthorizedExceptionFilterAttribute(ILogger<UserNotAuthorizedExceptionFilterAttribute> logger)
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