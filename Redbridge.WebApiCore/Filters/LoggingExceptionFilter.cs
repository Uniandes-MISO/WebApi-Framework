using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Redbridge.WebApiCore.Filters
{
    public class LoggingExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public LoggingExceptionFilterAttribute(ILogger<LoggingExceptionFilterAttribute> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public override void OnException(ExceptionContext actionExecutedContext)
        {
            _logger.LogError(actionExecutedContext.Exception, actionExecutedContext.Exception.Message);
        }
    }
}
