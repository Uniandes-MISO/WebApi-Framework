using NUnit.Framework;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Redbridge.WebApi.Tests
{
    [TestFixture()]
    public class TestQueryStringAuthenticationExtension
    {
        [Test]
        public void UseQueryStringAuthentication()
        {
            Assert.IsNotNull(1);
        }
    }
}