using Microsoft.Owin;
using System.Collections.Generic;

namespace Redbridge.WebApi
{
    public class OwinContextBuilder : IOwinContextProvider
    {
        public OwinContextBuilder()
        {
            var environment = new Dictionary<string, object>();
            Current = new OwinContext(environment);
        }

        public IOwinContext Current { get; }
    }
}