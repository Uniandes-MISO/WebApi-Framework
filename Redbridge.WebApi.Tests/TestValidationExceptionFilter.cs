using Moq;
using NUnit.Framework;
using Redbridge.Diagnostics;
using Redbridge.Exceptions;
using Redbridge.Threading;
using Redbridge.Validation;
using Redbridge.Web.Messaging;
using Redbridge.WebApi.Filters;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Redbridge.WebApi.Tests
{
    [TestFixture()]
    public class TestValidationExceptionFilter
    {
        [Test()]
        public void FilterValidationExceptionSingleType()
        {
            var logger = new Mock<ILogger>();
            var filter = new ValidationExceptionFilterAttribute(logger.Object);
            var context = new HttpActionExecutedContext() { };
            filter.OnException(context);
            context.Exception = new ValidationResultsException("Test", new ValidationResultCollection());
            context.Exception = new ValidationException("Test");
            Assert.IsNotNull(context);
        }

        [Test()]
        public void FilterValidationExceptionSingleTypeViewValidateBodyJson()
        {
            var logger = new Mock<ILogger>();
            var filter = new ValidationExceptionFilterAttribute(logger.Object);

            var httpActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage(HttpMethod.Get, "orders/api")
                }
            };

            filter.OnException(new HttpActionExecutedContext(httpActionContext, new ValidationException("Something went wrong.")));

            try
            {
                httpActionContext.Response.ThrowResponseException().WaitAndUnwrapException();
            }
            catch (ValidationException ve)
            {
                Assert.AreEqual("Something went wrong.", ve.Message);
            }
        }

        [Test()]
        public void FilterValidationExceptionMultiTypeViewValidateBodyJson()
        {
            var logger = new Mock<ILogger>();
            var filter = new ValidationExceptionFilterAttribute(logger.Object);

            var httpActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage(HttpMethod.Get, "orders/api")
                }
            };

            filter.OnException(new HttpActionExecutedContext(httpActionContext, new ValidationResultsException("Many things went wrong", new ValidationResultCollection(new[]
            {
                new ValidationResult(false, "Badness 1"),
                new ValidationResult(false, "Badness 2"),
                new ValidationResult(false, "Badness 3"),
                new ValidationResult(true, "Goodness 1"),
            }))));

            try
            {
                httpActionContext.Response.ThrowResponseException().WaitAndUnwrapException();
            }
            catch (ValidationResultsException ve)
            {
                Assert.AreEqual("Many things went wrong", ve.Message);
                Assert.IsFalse(ve.Results.Success);
                Assert.AreEqual(4, ve.Results.Results.Count());
            }
        }

        [Test()]
        public void FilterValidationExceptionMultiTypeViewValidateBodyJsonNoDefinedMessage()
        {
            var logger = new Mock<ILogger>();
            var filter = new ValidationExceptionFilterAttribute(logger.Object);

            var httpActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage(HttpMethod.Get, "orders/api")
                }
            };

            filter.OnException(new HttpActionExecutedContext(httpActionContext, new ValidationResultsException(new ValidationResultCollection(new[]
            {
                new ValidationResult(false, "Badness 1"),
                new ValidationResult(false, "Badness 2"),
                new ValidationResult(false, "Badness 3"),
                new ValidationResult(true, "Goodness 1"),
            }))));

            try
            {
                httpActionContext.Response.ThrowResponseException().WaitAndUnwrapException();
            }
            catch (ValidationResultsException ve)
            {
                Assert.AreEqual("Badness 1", ve.Message);
                Assert.IsFalse(ve.Results.Success);
                Assert.AreEqual(4, ve.Results.Results.Count());
            }
        }
    }
}