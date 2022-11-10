using Redbridge.Diagnostics;
using Redbridge.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Redbridge.WebApi.Filters
{
    public class UserNotAuthenticatedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UserNotAuthenticatedExceptionFilterAttribute(ILogger logger)
        {
            if (logger == null) throw new System.ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            _logger.WriteInfo("Checking exception for user not authenticated exception filtering....");
            if (actionExecutedContext.Exception is UserNotAuthenticatedException)
            {
                _logger.WriteDebug("Processing user not authenticated exception filtering....");
                var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionExecutedContext.Response = responseMessage;
                actionExecutedContext.Exception = null;
            }
        }
    }
}