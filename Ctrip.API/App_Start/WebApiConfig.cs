using System.Web.Http;
using System.Web.Http.Dispatcher;
using Ctrip.Component;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Security.OAuth;

namespace Ctrip.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }               
            );

            //在这替换控制器选择器
            //config.Services.Replace(typeof(IHttpControllerSelector), new VersionControllerSelector((config)));

            //最后两句话将会使用CamelCase命名法序列化webApi的返回结果
            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

#if !DEBUG 
            //force HTTPs 
            config.Filters.Add(new ForceHttpsAttribute()); 
#endif
        }
    }
}
