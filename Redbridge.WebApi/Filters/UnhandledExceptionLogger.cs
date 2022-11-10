﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Redbridge.Diagnostics;

namespace Redbridge.WebApi.Filters
{
    public class UnhandledExceptionLogger : IExceptionLogger
    {
        private readonly ILogger _logger;

        public UnhandledExceptionLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            _logger.WriteInfo("Logging (only) unhandled exceptions...");

            if (context.Exception != null)
                _logger.WriteException(context.Exception);

            return Task.CompletedTask;
        }

        public void OnException(HttpActionExecutedContext context)
        {
            throw new NotImplementedException();
        }
    }
}
