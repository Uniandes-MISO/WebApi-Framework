using Microsoft.Owin;
using System.Web;

namespace Redbridge.WebApi
{
    public class OwinContextProvider : IOwinContextProvider
    {
        public IOwinContext Current => HttpContext.Current?.GetOwinContext();
    }
}