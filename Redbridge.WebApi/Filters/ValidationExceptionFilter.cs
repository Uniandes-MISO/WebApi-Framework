using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Redbridge.Diagnostics;
using Redbridge.Exceptions;
using Redbridge.Validation;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace Redbridge.WebApi.Filters
{
    public class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ValidationExceptionFilterAttribute(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // Convert ValidationResultsException results or a ValidationException into ValidationResults
            _logger.WriteInfo("Checking exception for validation exception filtering....");

            ValidationResult[] results;
            var validationResultsException = actionExecutedContext.Exception as ValidationResultsException;
            var validationException = actionExecutedContext.Exception as ValidationException;
            string reasonPhrase = "";

            if (validationResultsException != null)
            {
                _logger.WriteInfo("Validation exception filtering being applied to a multi-results exception...");
                if (validationResultsException.Results?.Results != null)
                    results = validationResultsException.Results.Results.ToArray();
                else
                    results = new[] { new ValidationResult(false, validationResultsException.Message) };

                reasonPhrase = validationResultsException.Message;
            }
            else if (validationException != null)
            {
                _logger.WriteInfo("Validation exception filtering being applied to a single result validation exception...");
                results = new[] { new ValidationResult(false, validationException.Message) };
                reasonPhrase = validationException.Message;
            }
            else
            {
                _logger.WriteDebug("Validation exception filtering skipped.");
                return;
            }

            _logger.WriteInfo("Serializing results into JSON for transmission...");
            var rawJson = JsonConvert.SerializeObject(results, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            _logger.WriteDebug($"Setting JSON result on response message: {rawJson} with code 422.");
            actionExecutedContext.Response = new HttpResponseMessage((HttpStatusCode)422)
            {
                ReasonPhrase = string.Join(",", reasonPhrase.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Where(s => !string.IsNullOrWhiteSpace(s))),
                Content = new StringContent(rawJson, Encoding.UTF8, "application/json"),
                RequestMessage = actionExecutedContext.Request
            };

            actionExecutedContext.Exception = null;
        }
    }
}