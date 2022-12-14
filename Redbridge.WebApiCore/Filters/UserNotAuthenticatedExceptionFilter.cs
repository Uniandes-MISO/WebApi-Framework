using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Redbridge.Exceptions;

namespace Redbridge.WebApiCore.Filters
{
    public class UserNotAuthenticatedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UserNotAuthenticatedExceptionFilterAttribute(ILogger<UserNotAuthenticatedExceptionFilterAttribute> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UserNotAuthenticatedException exception)
            {
                _logger.LogDebug("Processing user not authenticated exception filtering.");
                context.Result = new UnauthorizedResult();
                context.ExceptionHandled = true;
            }
        }
    }
}