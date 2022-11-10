using Redbridge.ApiManagement;
using System;
using System.Web.Http;

namespace Redbridge.WebApi.Controllers
{
    public abstract class ApiFactoryController : ApiController
    {
        protected ApiFactoryController(IApiFactory apiFactory)
        {
            ApiFactory = apiFactory ?? throw new ArgumentNullException(nameof(apiFactory));
        }

        protected IApiFactory ApiFactory { get; }
    }
}