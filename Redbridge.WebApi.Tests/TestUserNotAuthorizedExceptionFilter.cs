using Moq;
using NUnit.Framework;
using Redbridge.Diagnostics;
using Redbridge.Exceptions;
using Redbridge.WebApi.Filters;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Redbridge.WebApi.Tests
{
    [TestFixture()]
    internal class TestUserNotAuthorizedExceptionFilter
    {
        [Test()]
        public void FilterExceptionNormalMessage()
        {
            var logger = new Mock<ILogger>();
            var filter = new UserNotAuthorizedExceptionFilterAttribute(logger.Object);
            var httpActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage(HttpMethod.Get, "orders/api")
                }
            };

            var context = new HttpActionExecutedContext(httpActionContext, new Exception("Some sort of message"));
            filter.OnException(context);
            Assert.IsNotNull(context.Exception);
        }

        [Test()]
        public void FilterUserNotAuthenticatedExceptionFilterReturns()
        {
            var logger = new Mock<ILogger>();
            var filter = new UserNotAuthorizedExceptionFilterAttribute(logger.Object);
            var messageBuilder = new StringBuilder("Unknown/invalid some sort of message:");
            messageBuilder.AppendLine("Link ID: abc");
            messageBuilder.AppendLine("Link ID: def");

            var httpActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage(HttpMethod.Get, "orders/api")
                }
            };

            var context = new HttpActionExecutedContext(httpActionContext, new ValidationException(messageBuilder.ToString()));
            filter.OnException(context);
            Assert.IsNotNull(context.Exception);
        }
    }
}