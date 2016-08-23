using System.Web.Services;
using System.Web.Services.Protocols;

namespace Ctrip.Component
{
    [WebService(Namespace = "http://DavidFan.cnblogs.com")]
    public class MyService : WebService
    {
        public MySoapHeader header;

        [MyExtensionAttribute]
        [SoapHeader("header", Direction = SoapHeaderDirection.In)]
        public string CheckHeader()
        {
            return "Something done";
        }
    }
}
