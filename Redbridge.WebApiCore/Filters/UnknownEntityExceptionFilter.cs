using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Redbridge.Data;

namespace Redbridge.WebApiCore.Filters
{
    public class UnknownEntityExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UnknownEntityExceptionFilterAttribute(ILogger<UnknownEntityExceptionFilterAttribute> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UnknownEntityException unknownEntityException)
            {
                _logger.LogInformation($"Unknown entity exception processing with message {unknownEntityException.Message}");
                var errorMessageError = new { error = unknownEntityException.Message };
                context.Result = new JsonResult(errorMessageError) { StatusCode = 422 };
            }
        }
    }
}